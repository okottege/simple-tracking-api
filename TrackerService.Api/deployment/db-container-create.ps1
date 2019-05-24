param(
    [Parameter(Mandatory=$true)][string] $dbHostName,
    [Parameter(Mandatory=$true)][string] $dbSAPassword,
    [Parameter][string] $location = "eastus",
    [Parameter(Mandatory=$true)][string] $acrName,
    [Parameter(Mandatory=$true)][string] $rgForAci,
    [Parameter(Mandatory=$true)][string] $spName
)

Write-Host "Creating the Azure Container Instance with MSSQL 2017" -ForegroundColor Green
Write-Host "Creating the resource group for ACI"
az group create --name $rgForAci --location $location

$acrId = (az acr show --name $acrName --query id --output tsv)

Write-Host "Creating a service principal to connect to acr"
$spPassword = (az ad sp create-for-rbac --name "http://$spName" --scopes $acrId --role acrpull --query password --output tsv)
$spId = (az ad sp show --id "http://$spName" --query appId --output tsv)
Write-Host "Service Principal created successfully."

Write-Host "Getting acr login server"
$acrLoginServer = (az acr show --name $acrName --query loginServer --output tsv)

Write-Host "Creating the container"
az container create --resource-group $rgForAci `
    --name "ms-sql-2017" `
    --image $acrLoginServer/ms-sql-2017:v1 `
    --cpu 1 --memory 3.5 `
    --registry-login-server $acrLoginServer `
    --registry-username $spId --registry-password $spPassword `
    --dns-name-label $dbHostName --ports 1433 `
    --environment-variables ACCEPT_EULA="Y" SA_PASSWORD="$dbSAPassword"

Write-Host "Successfully created the container instance with MSSQL 2017" -ForegroundColor Green