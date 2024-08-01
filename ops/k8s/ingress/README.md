- install helm `brew install helm`
- add helm repo `helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx`

- Set environment variables for wanted version

```
CHART_VERSION="4.4.0"
APP_VERSION="1.5.1"
```

- create ingress yaml file

```
helm template ingress-nginx ingress-nginx \
--repo https://kubernetes.github.io/ingress-nginx \
--version ${CHART_VERSION} \
./nginx-ingress.${APP_VERSION}.yaml
```

- apply file using kubectl
- check if ingress is working using port forwarding

```
kubectl port-forward svc/ingress-nginx-controller 4321:443
```

all credit to -> https://github.com/marcel-dempers
