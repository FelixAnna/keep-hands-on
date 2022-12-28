## provision infrastructure 
app=demo  # microservice/deployment name
env=$1  # dev or prod
tag=1.0.0

cd ./terraform/profiles/$env
terraform init -reconfigure

terraform apply -auto-approve


## install basic 

cd ../../../../
sed -i "s/awsKeyIdPlaceHolder/${AWS_ACCESS_KEY_ID}/" ./$app-chart/values_aks_$env.yaml
sed -i "s/awsSecretKeyPlaceHolder/${AWS_SECRET_ACCESS_KEY}/" ./$app-chart/values_aks_$env.yaml

cd aks/services

sh basic_services.sh $app $env
sh main_services.sh $app $env $tag
