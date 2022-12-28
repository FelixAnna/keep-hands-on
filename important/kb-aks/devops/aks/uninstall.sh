## uninstall basic 
app=demo  # microservice/deployment name
env=$1  # dev or prod

cd ./services

sh uninstall.sh $app $env

## destory infrastructure

cd ../terraform/profiles/$env
terraform init -reconfigure
terraform destroy -auto-approve
