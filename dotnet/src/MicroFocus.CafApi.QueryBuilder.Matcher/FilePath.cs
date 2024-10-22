/**
 * Copyright 2022-2024 Open Text.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.IO;
using System.Linq;
using System.Text;

namespace MicroFocus.CafApi.QueryBuilder.Matcher
{
    /**
     * An abstract representation of file and directory pathnames based on Java's java.io.File and java.io.FileSystem.
     */
    internal sealed class FilePath
    {
        private readonly string _path;
        private readonly int _prefixLength;
        private static readonly char PATH_SEPARATOR_CHAR = Path.DirectorySeparatorChar;

        public FilePath(string pathname)
        {
            _path = Normalize(pathname);
            _prefixLength = PrefixLength(_path);
        }

        private FilePath(string pathname, int prefixLength)
        {
            _path = pathname;
            _prefixLength = prefixLength;
        }

        /*
         * Converts this abstract pathname into a pathname string. The resulting
         * string uses the default name-separator character to separate the names in the name sequence.
         */
        public string GetPath()
        {
            return _path;
        }

        /*
         * Returns the abstract pathname of this abstract pathname's parent,
         * or null if this pathname does not name a parent directory.
         * 
         * The parent of an abstract pathname consists of the
         * pathname's prefix, if any, and each name in the pathname's name
         * sequence except for the last. If the name sequence is empty then
         * the pathname does not name a parent directory.
         * 
         */
        public FilePath GetParentFile()
        {
            string p = GetParent();
            if (p == null) return null;
            if (GetType() != typeof(FilePath))
            {
                p = Normalize(p);
            }
            return new FilePath(p, _prefixLength);
        }

        private int PrefixLength(string path)
        {
            char slash = PATH_SEPARATOR_CHAR;
            int n = path.Length;
            if (n == 0) return 0;
            char c0 = path.ElementAt(0);
            char c1 = (char)((n > 1) ? path.ElementAt(1) : 0);
            if (c0 == slash)
            {
                if (c1 == slash) return 2;  /* Absolute UNC pathname "\\\\foo" */
                return 1;                   /* Drive-relative "\\foo" */
            }
            if (char.IsLetter(c0) && (c1 == ':'))
            {
                if ((n > 2) && (path.ElementAt(2) == slash))
                    return 3;               /* Absolute local pathname "z:\\foo" */
                return 2;                   /* Directory-relative "z:foo" */
            }
            return 0;                       /* Completely relative */
        }

        /*
         * Returns the pathname string of this abstract pathname's parent, or
         * null if this pathname does not name a parent directory.
         * 
         * The parent of an abstract pathname consists of the
         * pathname's prefix, if any, and each name in the pathname's name
         * sequence except for the last.  If the name sequence is empty then
         * the pathname does not name a parent directory.
         * 
         */
        private string GetParent()
        {
            int index = _path.LastIndexOf(Path.DirectorySeparatorChar);
            if (index < _prefixLength)
            {
                if ((_prefixLength > 0) && (_path.Length > _prefixLength))
                    return _path.Substring(0, _prefixLength);
                return null;
            }
            return _path.Substring(0, index);
        }

        /*
         * Check that the given pathname is normal.  If not, invoke the real
         * normalizer on the part of the pathname that requires normalization.
         * This way we iterate through the whole pathname string only once.
         */
        private string Normalize(string path)
        {
            int n = path.Length;
            char slash = PATH_SEPARATOR_CHAR;
            char altSlash = (slash == '\\') ? '/' : '\\';
            char prev = '0';
            for (int i = 0; i < n; i++)
            {
                char c = path.ElementAt(i);
                if (c == altSlash)
                    return Normalize(path, n, (prev == slash) ? i - 1 : i);
                if ((c == slash) && (prev == slash) && (i > 1))
                    return Normalize(path, n, i - 1);
                if ((c == ':') && (i > 1))
                    return Normalize(path, n, 0);
                prev = c;
            }
            if (prev == slash) return Normalize(path, n, n - 1);
            return path;
        }

        /*
         * Normalize the given pathname, whose length is len, starting at the given
         * offset; everything before this offset is already normal.
         */
        private string Normalize(string path, int len, int off)
        {
            if (len == 0) return path;
            if (off < 3) off = 0;   /* Avoid fencepost cases with UNC pathnames */
            int src;
            char slash = PATH_SEPARATOR_CHAR;
            StringBuilder sb = new StringBuilder(len);

            if (off == 0)
            {
                /* Complete normalization, including prefix */
                src = NormalizePrefix(path, len, sb);
            }
            else
            {
                /* Partial normalization */
                src = off;
                sb.Append(path, 0, off);
            }

            /* Remove redundant slashes from the remainder of the path, forcing all
               slashes into the preferred slash */
            while (src < len)
            {
                char c = path.ElementAt(src++);
                if (IsSlash(c))
                {
                    while ((src < len) && IsSlash(path.ElementAt(src))) src++;
                    if (src == len)
                    {
                        /* Check for trailing separator */
                        int sn = sb.Length;
                        if ((sn == 2) && (sb[1] == ':'))
                        {
                            /* "z:\\" */
                            sb.Append(slash);
                            break;
                        }
                        if (sn == 0)
                        {
                            /* "\\" */
                            sb.Append(slash);
                            break;
                        }
                        if ((sn == 1) && (IsSlash(sb[0])))
                        {
                            /* "\\\\" is not collapsed to "\\" because "\\\\" marks
                               the beginning of a UNC pathname.  Even though it is
                               not, by itself, a valid UNC pathname, we leave it as
                               is in order to be consistent with the win32 APIs,
                               which treat this case as an invalid UNC pathname
                               rather than as an alias for the root directory of
                               the current drive. */
                            sb.Append(slash);
                            break;
                        }
                        /* Path does not denote a root directory, so do not append
                           trailing slash */
                        break;
                    }
                    else
                    {
                        sb.Append(slash);
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /*
         * A normal Win32 pathname contains no duplicate slashes, except possibly
         * for a UNC prefix, and does not end with a slash.  It may be the empty
         * string.  Normalized Win32 pathnames have the convenient property that
         * the length of the prefix almost uniquely identifies the type of the path
         * and whether it is absolute or relative:
         *   0  relative to both drive and directory
         *   1  drive-relative (begins with '\\')
         *   2  absolute UNC (if first char is '\\'),
         *         else directory-relative (has form "z:foo")
         *   3  absolute local pathname (begins with "z:\\")
         */
        private int NormalizePrefix(string path, int len, StringBuilder sb)
        {
            int src = 0;
            while ((src < len) && IsSlash(path.ElementAt(src))) src++;
            char c;
            if ((len - src >= 2)
                && char.IsLetter(c = path.ElementAt(src))
                && path.ElementAt(src + 1) == ':')
            {
                /* Remove leading slashes if followed by drive specifier.
                   This hack is necessary to support file URLs containing drive
                   specifiers (e.g., "file://c:/path").  As a side effect,
                   "/c:/path" can be used as an alternative to "c:/path". */
                sb.Append(c);
                sb.Append(':');
                src += 2;
            }
            else
            {
                src = 0;
                if ((len >= 2)
                    && IsSlash(path.ElementAt(0))
                    && IsSlash(path.ElementAt(1)))
                {
                    /* UNC pathname: Retain first slash; leave src pointed at
                       second slash so that further slashes will be collapsed
                       into the second slash.  The result will be a pathname
                       beginning with "\\\\" followed (most likely) by a host
                       name. */
                    src = 1;
                    sb.Append(PATH_SEPARATOR_CHAR);
                }
            }
            return src;
        }

        private bool IsSlash(char c)
        {
            return (c == '\\') || (c == '/');
        }
    }
}
