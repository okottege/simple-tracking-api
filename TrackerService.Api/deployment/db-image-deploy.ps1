. .\db-vars.ps1

Write-Host "Checking if resource group $rgName exists."
$rgExists = az group exists --name $rgName
Write-Host "Resource group exists: '$rgExists'"

if(!$rgExists) {
    Write-Host "Resource group doesn't exist, creating resource group $rgName"
    az group create --name $rgName --location $location
}

Write-Host "Creating acr '$acrName'"
az acr create --resource-group $rgName --name $acrName --sku Basic --admin-enabled true

Write-Host "Loging in to acr: '$acrName'"
az acr login --name $acrName

$acrLoginServer = az acr show --name $acrName --query loginServer --output tsv
Write-Host "Login server is: '$acrLoginServer'"

Write-Host "Tagging the mssql 2017 image"
docker tag mcr.microsoft.com/mssql/server:2017-latest $acrLoginServer/ms-sql-2017:v1

Write-Host "Pushing image to acr"
docker push $acrLoginServer/ms-sql-2017:v1