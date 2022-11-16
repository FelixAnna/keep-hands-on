## uninstall basic 
env=$1

cd ../services

sh uninstall.sh $env

## destory infrastructure

cd ../terraform/profile/$env

terraform destroy -auto-approve