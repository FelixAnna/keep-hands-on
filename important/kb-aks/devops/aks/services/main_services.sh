env=$1
tag=$2
app=$3

ns="${app}ns"

## deploy services
echo "deploy $app micro services"

cd ../../

sed -i "s/imageVersion/$tag/" ./$app-chart/values_aks_$env.yaml
helm upgrade --install $app ./$app-chart/ --namespace $ns --create-namespace --values ./$app-chart/values_aks_$env.yaml

echo "done"
