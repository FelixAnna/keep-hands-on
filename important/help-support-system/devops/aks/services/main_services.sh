env=$1
app=$2

ns="${app}ns"

## deploy services
echo "deploy $app micro services"

cd ../../        ## go to root folder who have chart
helm upgrade --install $app ./$app-chart/ --namespace $ns --create-namespace --values ./$app-chart/values_aks_$env.yaml
cd aks/services/ ## return to current: ./aks/services dir

echo "done"
