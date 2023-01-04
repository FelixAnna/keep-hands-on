## provision infrastructure 
env=$1  # dev or prod
tag=$2
app=$3  # microservice/deployment name

if [ "$app" == '' ];
then
    app=hss
fi

if [ "$tag" == '' ];
then
    tag=latest
fi

echo $app
cd ./terraform/profiles/$env
terraform init -reconfigure

terraform apply -auto-approve


## install basic 

cd ../../../../
sed -i "s/imageVersion/$tag/" ./$app-chart/values_aks_$env.yaml

cd aks/services

sh basic_services.sh $env $app
sh main_services.sh $env $app

cd ../../
sed -i "s/$tag/imageVersion/" ./$app-chart/values_aks_$env.yaml
