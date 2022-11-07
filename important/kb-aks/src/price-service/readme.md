# price api

## functionalities 

a. query all price by product id

b. query latest price by product id
        

## Docker Guide

### Build & Tag
docker build -t demo-price-api:1.0.0 . 

### Check Image

docker image ls

### Tag

docker image tag demo-price-api:1.0.0 yufelix/demo-price-api:1.0.0

### Run (use consul for service registry and discovery)

docker run -d -e AWS_ACCESS_KEY_ID=xyz -e AWS_SECRET_ACCESS_KEY=abc -e AWS_REGION=ap-southeast-1 -e profile=dev  --publish 8282:8282 yufelix/demo-price-api:1.0.0
