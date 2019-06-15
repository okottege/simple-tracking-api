param (
    [string]$resourceGroup = "rg-osh-aks-demo",
    [string]$clusterName = "TrackingServiceAKS",
    [string]$location = "westeurope"
)

Write-Host "Creating the resource group"
az group create -n $resourceGroup -l $location

Write-Host "Creating the AKS cluster"
az aks create -g $resourceGroup -n $clusterName --node-count 1 --generate-ssh-keys

Write-Host "Getting credentials for aks"
az aks get-credentials -g $resourceGroup -n $clusterName