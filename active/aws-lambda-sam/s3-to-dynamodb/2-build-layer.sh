#!/bin/bash
set -eo pipefail
gradle -q packageLibs
mv build/distributions/s3-to-dynamodb.zip build/s3-to-dynamodb-lib.zip