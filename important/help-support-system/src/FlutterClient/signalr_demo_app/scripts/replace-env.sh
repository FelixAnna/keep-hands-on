#!/bin/bash
envPath='env' # Path to the folder where all our .env files live
env=$1

if [ -z "$1" ]; then
    echo "No environment supplied, allowed: [stage, prod]"
    exit 1
fi

cp "$envPath/$env.env" "$envPath/.env" 
echo "Copied '$envPath/$env.env' to '$envPath/.env'"
