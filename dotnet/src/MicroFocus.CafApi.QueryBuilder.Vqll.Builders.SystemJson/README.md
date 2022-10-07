## VQLL

VQLL is a Lisp-style grammar that writes values out in JSON arrays.

The general syntax is:

    [ operator, operands... ]

Here are some expressions written in VQLL:

    ["in", "repository_id", 1234, 2345]

    ["==", "classification", "classified"]

    ["and", ["in", "repository_id", 1234, 2345], ["==", "classification", "classified"]]
