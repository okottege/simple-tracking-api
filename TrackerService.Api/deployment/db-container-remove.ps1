param(
    [Parameter(Mandatory)][string] $rgForAci
)

Write-Host "Removing the database container MSSQL 2017" -ForegroundColor Green

Write-Host "Removing the resource group $rgForAci"
az group delete --name $rgForAci --yes

Write-Host "Successfully removed the container instance MSSQL 2017" -ForegroundColor Green