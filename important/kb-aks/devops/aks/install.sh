## provision infrastructure 
env=$1  # dev or prod
tag=$2
app=$3  # microservice/deployment name

if [ "$app" == '' ];
then
    app=demo
fi

if [ "$tag" == '' ];
then
    tag=latest
fi

echo $app

## define your variables somewhere
## AWS_ACCESS_KEY_ID=xxx
## AWS_SECRET_ACCESS_KEY=xxx
source d:/code/config.sh
echo $AWS_ACCESS_KEY_ID

cd ./terraform/profiles/$env
terraform init -reconfigure

terraform apply -auto-approve


## install basic 

cd ../../../../
sed -i "s/awsKeyIdPlaceHolder/$(echo -n $AWS_ACCESS_KEY_ID | base64)/" ./$app-chart/values_aks_$env.yaml
sed -i "s/awsSecretKeyPlaceHolder/$(echo -n $AWS_SECRET_ACCESS_KEY | base64)/" ./$app-chart/values_aks_$env.yaml
sed -i "s/imageVersion/$tag/" ./$app-chart/values_aks_$env.yaml

cd aks/services

sh basic_services.sh $env $app
sh main_services.sh $env $app

cd ../../
sed -i "s/$(echo -n $AWS_ACCESS_KEY_ID | base64)/awsKeyIdPlaceHolder/" ./$app-chart/values_aks_$env.yaml
sed -i "s/$(echo -n $AWS_SECRET_ACCESS_KEY | base64)/awsSecretKeyPlaceHolder/" ./$app-chart/values_aks_$env.yaml
sed -i "s/$tag/imageVersion/" ./$app-chart/values_aks_$env.yaml
