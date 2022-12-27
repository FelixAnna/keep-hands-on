
ns=hssns
tag=$2

## deploy services
echo "deploy hss micro services"
cd ../../

sed -i "s/imageVersion/$tag/" ./hss-chart/values_aks_prod.yaml
helm upgrade --install hss ./hss-chart/ --namespace $ns --create-namespace --values ./hss-chart/values_aks_$1.yaml

echo "done"
