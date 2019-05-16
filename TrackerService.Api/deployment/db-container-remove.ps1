. .\db-vars.ps1

Write-Host "Removing the database container MSSQL 2017" -ForegroundColor Green

Write-Host "Removing the resource group $rgForAci"
az group delete --name $rgForAci --yes

Write-Host "Removing the Service Principal used to connect create container instance"
$spId = (az ad sp show --id "http://$spName" --query appId --output tsv)
az ad sp delete --id $spId

Write-Host "Successfully removed the container instance MSSQL 2017" -ForegroundColor Green