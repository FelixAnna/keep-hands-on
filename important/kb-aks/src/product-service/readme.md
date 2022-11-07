# product api

## functionalities 

a. query all product

b. query product by id
        

## Docker Guide

### Build

docker build -t product-api . 
or
docker build -t product-api:1.0.0 . 
### Check Image

docker image ls

### Tag

docker image tag product-api:latest product-api:1.0.0

### Run (use consul for service registry and discovery)

docker run -d -e AWS_ACCESS_KEY_ID=xyz -e AWS_SECRET_ACCESS_KEY=abc -e AWS_REGION=ap-southeast-1 -e profile=dev  --publish 8181:8181 product-api:1.0.0
