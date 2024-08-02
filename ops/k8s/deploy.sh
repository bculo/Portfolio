readonly NAMESPACE_VALUE="portfolio"

echo "Start kubernetes cluster deployment"
minikube delete
minikube start
sleep 15
echo "Kubernetes cluster started"

echo "Create namespace"
kubectl create namespace ${NAMESPACE_VALUE}
echo "Namespace created"

echo "Adding volume"
kubectl apply -f volumes/volume.local.yaml
echo "Volume added"

echo "Adding redis"
kubectl apply -f redis/redis.pvc.local.yaml -n ${NAMESPACE_VALUE}
kubectl apply -f redis/redis.app.local.yaml -n ${NAMESPACE_VALUE}
echo "Redis added"

echo "Adding RabbitMQ"
kubectl apply -f rabbit/rabbit.pvc.local.yaml -n ${NAMESPACE_VALUE}
kubectl apply -f rabbit/rabbit.secret.local.yaml -n ${NAMESPACE_VALUE}
kubectl apply -f rabbit/rabbit.app.local.yaml -n ${NAMESPACE_VALUE}
echo "RabbitMQ added"

echo "Adding postgre"
kubectl apply -f postgre/postgre.pvc.local.yaml -n ${NAMESPACE_VALUE}
kubectl apply -f postgre/postgre.secret.local.yaml -n ${NAMESPACE_VALUE}
kubectl apply -f postgre/postgre.app.local.yaml -n ${NAMESPACE_VALUE}
echo "postgre added"

echo "Adding crypto app"
kubectl apply -f crypto-service/crypto.secret.local.yaml -n ${NAMESPACE_VALUE}
kubectl apply -f crypto-service/crypto.config.local.yaml -n ${NAMESPACE_VALUE}
kubectl apply -f crypto-service/crypto.app.local.yaml -n ${NAMESPACE_VALUE}
echo "Crypto app added"

echo "Adding ingress controller"
kubectl apply -f ingress/nginx-ingress.1.5.1.yaml
kubectl apply -f ingress/routing.local.yaml -n ${NAMESPACE_VALUE}
echo "Ingress controller added"

echo "Done!"