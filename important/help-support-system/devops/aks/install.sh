## provision infrastructure 

env=$1  # dev or prod
tag=0.5.0

cd ./terraform/profiles/$env
terraform init -reconfigure

terraform apply -auto-approve

## install basic 

cd ../../../services

sh basic_services.sh $env

sh hss_services.sh $env $tag
