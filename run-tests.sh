#!/usr/bin/env bash
for d in test/*; 
    do 
        if [[ ${d} == *Tests ]]; then
            cd ${d} & dotnet xunit & cd ..;
        fi 
done