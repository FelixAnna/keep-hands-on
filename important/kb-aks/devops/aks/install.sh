## provision infrastructure 

env=$1  # dev or prod

cd ../terraform/profile/$env
terraform init -reconfigure

terraform apply -auto-approve


## install basic 

cd ../../../services

sh basic_services.sh $env

sh demo_services.sh $env