param(
    [Parameter(Mandatory)][string] $rgAKS,
    [Parameter(Mandatory)][string] $clusterName,
    [Parameter(Mandatory)][string] $rgACR,
    [Parameter(Mandatory)][string] $acrName
)

# Get the client id
Write-Host "Getting service principal profile client id"
$clientId = $(az aks show -g $rgAKS -n $clusterName --q "servicePrincipalProfile.clientId" -o tsv)

# Get ACR registry resource Id
Write-Host "Getting ACR resource id"
$acrId = $(az acr show -n $acrName -g $rgACR --query "id" -o tsv)

# Create the role assignment
Write-Host "Creating the role assignment"
az role assignment create --assignee $clientId --role acrpull --scope $acrId