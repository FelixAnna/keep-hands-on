
env=$1
rgName=demo-$env-rg
clusterName="${env}Cluster"
ns=demons

## installing all services
echo "removing all services"

## switch context
az aks get-credentials --resource-group $rgName --name $clusterName

## uninstall services
helm uninstall demo -n $ns
helm uninstall consul -n consul
helm uninstall cert-manager -n cert-manager
helm uninstall ingress-nginx  -n ingress-basic

echo "done"
