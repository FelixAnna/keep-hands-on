tag=$1
app=$2  # microservice/deployment name

if [ "$app" == '' ];
then
    app=kse
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

AWS_ACCESS_KEY_ID=
AWS_SECRET_ACCESS_KEY=
echo $AWS_ACCESS_KEY_ID
sed -i "s/awsKeyIdPlaceHolder/$(echo -n $AWS_ACCESS_KEY_ID | base64)/" ./$app-chart-nossl/values_dev.yaml
sed -i "s/awsSecretKeyPlaceHolder/$(echo -n $AWS_SECRET_ACCESS_KEY | base64)/" ./$app-chart-nossl/values_dev.yaml
sed -i "s/imageVersion/$tag/" ./$app-chart-nossl/values_dev.yaml
sed -i "s/connPlaceHolder/$(echo -n $AppConnectionString | base64)/" ./$app-chart/values_aks_$env.yaml

helm upgrade --install $app ./$app-chart-nossl/ \
--namespace $ns \
--create-namespace \
--values ./$app-chart-nossl/values_dev.yaml \
--wait

kubectl get all -n $ns

sed -i "s/$(echo -n $AWS_ACCESS_KEY_ID | base64)/awsKeyIdPlaceHolder/" ./$app-chart-nossl/values_dev.yaml
sed -i "s/$(echo -n $AWS_SECRET_ACCESS_KEY | base64)/awsSecretKeyPlaceHolder/" ./$app-chart-nossl/values_dev.yaml
sed -i "s/$tag/imageVersion/" ./$app-chart-nossl/values_dev.yaml
sed -i "s/$(echo -n $AppConnectionString | base64)/connPlaceHolder/" ./$app-chart/values_aks_$env.yaml
echo "done"