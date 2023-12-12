# PT_Demo_NET_Kubernetes

## Contents

- [Prerequisites](#prerequisites)
- [Setup](#setup)
- [Teardown](#teardown)
- [Terms](#terms)
- [Links](#links)

## Prerequisites

1. Install `Docker Desktop`.

2. Enable `Kubernetes` in Docker Desktop:
```
Docker Desktop > Settings > Kubernetes > [v] Enable Kubernetes
```

3. Check that you have `kubectl` CLI by using any of the following commands:
```
kubectl version
kubectl --help
```

4. Install `Helm` from [here](https://helm.sh/docs/intro/install/).

On Windows, open Power Shell `as Admin` and execute the following command:

```
choco install kubernetes-helm
```

5. Check that you have `helm` CLI installed by using any of the following commands:
```
helm version
helm --help
```

## Setup

1. Create a new .NET blank solution `PT_Demo_NET_Kubernetes`

2. Create a new .NET Web API project `DemoNetKubernetes`

3. Introduce Memory- and Cpu- Controllers with endpoints processing Memory / CPU intensive workloads.

4. Create `Dockerfile` in the DemoNetKubernetes.csproj directory:
```
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DemoNetKubernetes.csproj", "./"]
RUN dotnet restore "DemoNetKubernetes.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "DemoNetKubernetes.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoNetKubernetes.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoNetKubernetes.dll"]
```

5. Build `Docker Image` and push it in Docker Hub

```
docker build -t petartotev/demonetkubernetes:latest .
docker push petartotev/demonetkubernetes:latest
```

6. Create a /k8s directory and add the following files:
- `deployment.yaml`
```
apiVersion: apps/v1
kind: Deployment
metadata:
  name: demonetkubernetes-deployment
spec:
  replicas: 2
  selector:
    matchLabels:
      app: demonetkubernetes
  template:
    metadata:
      labels:
        app: demonetkubernetes
    spec:
      containers:
      - name: demonetkubernetes
        image: petartotev/demonetkubernetes:latest
        ports:
        - containerPort: 80
        resources:
          requests:
            cpu: "100m"
          limits:
            cpu: "500m"
```

- `service.yaml`
```
apiVersion: v1
kind: Service
metadata:
  name: demonetkubernetes-service
spec:
  type: NodePort
  selector:
    app: demonetkubernetes
  ports:
    - protocol: TCP
      port: 80
      targetPort: 80
```

7. Apply those using the following commands:
```
kubectl apply -f deployment.yaml
kubectl apply -f service.yaml
```

8. Get the service by using the following command:
```
kubectl get service demonetkubernetes-service
```
The output should be like:
```
NAME                        TYPE       CLUSTER-IP      EXTERNAL-IP   PORT(S)        AGE
demonetkubernetes-service   NodePort   10.102.113.24   <none>        80:31129/TCP   2m
```

9. Finally, get the external port and access the application using the following URL:
```
http://localhost:31129/cpu/doload/{number-of-operations}
```

![happy-end](./res/scrot_demo_01.png)

## Teardown

You can do any of the following:
- Go to Docker > Settings > Kubernetes > [Reset Kubernetes Cluster]

![reset-kubernetes-cluster](./res/scrot_demo_02.png)

- Try to remove the Kubernetes infrastructure manually by using the following command:
```
kubectl delete deployments,services --all
```

## Terms
- [Helm](https://helm.sh/docs/) = the package manager for Kubernetes

## Links
- https://forums.docker.com/t/unable-to-install-kubernetes-stuck-on-starting-state/117048