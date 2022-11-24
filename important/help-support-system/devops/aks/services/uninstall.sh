
env=$1
rgName=hss-$env-rg
clusterName="${env}Cluster"
ns=hssns

## installing all services
echo "removing all services"

## switch context
az aks get-credentials --resource-group $rgName --name $clusterName --overwrite-existing

## uninstall services
helm uninstall hss -n $ns
helm uninstall consul -n consul
helm uninstall cert-manager -n cert-manager
helm uninstall ingress-nginx  -n ingress-basic

echo "done"
