#!/bin/bash
set -eo pipefail
gradle -q packageLibs
mv build/distributions/nohouse-function.zip build/nohouse-function-lib.zip