param(
    [Parameter(Mandatory)][string] $dbHostName,
    [Parameter(Mandatory)][string] $dbSAPassword,
    [Parameter(Mandatory)][string] $location,
    [Parameter(Mandatory)][string] $acrName,
    [Parameter(Mandatory)][string] $rgForAci,
    [Parameter(Mandatory)][string] $azureLoginId,
    [Parameter(Mandatory)][string] $azureLoginSecret,
    [Parameter(Mandatory)][string] $azureTenantId
)

Write-Host "Loging into azure using azure cli"
az login --service-principal -u $azureLoginId -p $azureLoginSecret -t $azureTenantId

Write-Host "Creating the Azure Container Instance with MSSQL 2017" -ForegroundColor Green
Write-Host "Creating the resource group for ACI"
$rgGuid = New-Guid
$newRgNameForAci = "${rgForAci}_${rgGuid}"
Write-Host "Writing the resource group name to VSO variable with new value: $newRgNameForAci"
Write-Host "##vso[task.setvariable variable=IntTestDbAciRsourceGroup]$newRgNameForAci"
az group create --name $newRgNameForAci --location $location

Write-Host "Getting acr login server"
$acrLoginServer = (az acr show --name $acrName --query loginServer --output tsv)

$containerGroupName = "ms-sql-2017-${rgGuid}"
Write-Host "Creating the container: $containerGroupName"
az container create --resource-group $newRgNameForAci `
    --name $containerGroupName `
    --image $acrLoginServer/ms-sql-2017:v1 `
    --cpu 1 --memory 3.5 `
    --registry-login-server $acrLoginServer `
    --registry-username $azureLoginId --registry-password $azureLoginSecret `
    --dns-name-label $dbHostName --ports 1433 `
    --environment-variables ACCEPT_EULA="Y" SA_PASSWORD="$dbSAPassword"

Write-Host "Successfully created the container instance with MSSQL 2017" -ForegroundColor Green