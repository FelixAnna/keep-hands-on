kind delete clusters hss-cluster

kind create cluster --config hss-cluster.yml

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
ns=hssns
helm upgrade --install hss ./hss-chart-nossl/ --namespace $ns --create-namespace --values ./hss-chart-nossl/values_dev.yaml --wait

kubectl get all -n $ns
echo "done"