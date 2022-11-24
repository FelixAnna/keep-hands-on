
ns=hssns

## deploy services
echo "deploy hss micro services"
cd ../../
helm upgrade --install hss ./hss-chart/ --namespace $ns --create-namespace --values ./hss-chart/values_aks_$1.yaml

echo "done"
