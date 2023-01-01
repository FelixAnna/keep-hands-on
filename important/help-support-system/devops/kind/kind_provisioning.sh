tag=$1
app=$2  # microservice/deployment name

if [ "$app" == '' ];
then
    app=hss
fi

if [ "$tag" == '' ];
then
    tag=latest
fi

ns="${app}ns"

kind delete clusters $app-cluster

kind create cluster --config $app-cluster.yml

echo "install nginx  ..."
echo "(if you need kong, please uninstall nginx, then follow readme.md)"
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/main/deploy/static/provider/kind/deploy.yaml

echo "install metrics server ..."
kubectl apply -f ../components/metrics/metrics.yaml

echo "wait for nginx controller up before install services ..."
kubectl wait --namespace ingress-nginx \
  --for=condition=ready pod \
  --selector=app.kubernetes.io/component=controller \
  --timeout=300s

echo "install services ..."
cd ..

sed -i "s/imageVersion/$tag/" ./$app-chart-nossl/values_dev.yaml

helm upgrade --install $app ./$app-chart-nossl/ \
--namespace $ns \
--create-namespace \
--values ./$app-chart-nossl/values_dev.yaml \
--wait

kubectl get all -n $noss

sed -i "s/$tag/imageVersion/" ./$app-chart-nossl/values_dev.yaml
echo "done"