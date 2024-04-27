#!/bin/sh

export FRP_SERVER_ADDR=x.x.x.x
export FRP_SSH_REMOTE_PORT=6000
export CNAME="onprem.metadlw.com"

# export STATIC_DIR="D:/utb"
# export USER_NAME="admin"
# export PASSWORD="admin"

./frpc -c ./frpc.toml