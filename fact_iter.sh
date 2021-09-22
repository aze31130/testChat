#!/bin/sh
if [ -z $2 ] && ! [ -z $1 ]; then
        arg=$1
        factorial=1
        while [ $arg -gt 1 ]
        do
                factorial=$((factorial * $arg))
                arg=$((arg - 1))
        done
        echo $factorial
else
        echo 1
fi
