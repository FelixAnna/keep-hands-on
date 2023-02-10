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
cd ../../../  ## return to current: ./aks dir

## install basic 
## define your variables somewhere
## AWS_ACCESS_KEY_ID=xxx
## AWS_SECRET_ACCESS_KEY=xxx
source d:/code/config.sh
echo $AWS_ACCESS_KEY_ID
##echo $AppConnectionString

sed -i "s/imageVersion/$tag/" ../$app-chart/values_aks_$env.yaml
sed -i "s/connPlaceHolder/$(echo -n $AppConnectionString | base64 -w 0)/" ../$app-chart/values_aks_$env.yaml

cd services/
sh basic_services.sh $env $app
sh main_services.sh $env $app

cd ../../
sed -i "s/$tag/imageVersion/" ./$app-chart/values_aks_$env.yaml
sed -i "s/$(echo -n $AppConnectionString | base64 -w 0)/connPlaceHolder/" ./$app-chart/values_aks_$env.yaml
