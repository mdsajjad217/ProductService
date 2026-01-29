docker build -t mdsajjad217/orderservice:latest ./OrderService
docker push mdsajjad217/orderservice:latest

docker build -t mdsajjad217/productservice:latest ./ProductService
docker push mdsajjad217/productservice:latest

kubectl apply -f ./k8s-manifests/kafka.yaml
kubectl apply -f ./k8s-manifests/kafka-init.yaml
kubectl apply -f ./k8s-manifests/orderservice.yaml
kubectl apply -f ./k8s-manifests/productservice.yaml


kubectl port-forward -n apps svc/productservice 8080:8080
kubectl port-forward -n apps svc/orderservice 8081:8081

kubectl create namespace security

kubectl apply -f keycloak.yaml
kubectl apply -f keycloak-service.yaml

kubectl port-forward svc/keycloak 8080:8080 -n security