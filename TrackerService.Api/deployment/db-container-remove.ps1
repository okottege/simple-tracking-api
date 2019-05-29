param(
    [Parameter(Mandatory)][string] $rgForAci,
    [Parameter(Mandatory)][string] $azureLoginId,
    [Parameter(Mandatory)][string] $azureLoginSecret,
    [Parameter(Mandatory)][string] $azureTenantId
)

Write-Host "Removing the database container MSSQL 2017" -ForegroundColor Green
Write-Host "Loging in to azure using azure cli"
az login --service-principal -u $azureLoginId -p $azureLoginSecret -t $azureTenantId

$rgExists = az group exists --name $rgForAci
Write-Host "resource exists? $rgExists"
Write-Host "Resource group name: ($env:IntTestDbAciRsourceGroup)"

if($rgExists) {
    Write-Host "Removing the resource group $rgForAci"
    az group delete --name $rgForAci --yes
}

Write-Host "Successfully removed the container instance MSSQL 2017" -ForegroundColor Green