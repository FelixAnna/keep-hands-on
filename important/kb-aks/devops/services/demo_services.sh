
ns=demons

## deploy services
echo "deploy demo micro services"
cd ../
helm upgrade --install demo ./demo-chart/ --namespace $ns --create-namespace --values ./demo-chart/values_aks.yaml

echo "done"