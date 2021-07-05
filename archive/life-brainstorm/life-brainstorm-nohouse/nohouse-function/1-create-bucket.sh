#!/bin/bash
BUCKET_NAME=lambda-artifacts-lbn
echo $BUCKET_NAME > bucket-name.txt
aws s3 mb s3://$BUCKET_NAME
