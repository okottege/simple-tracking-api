param(
    [string] $webAppVersion = "v1.1",
    [string] $imageName = "tracking-svc-api"
)

Write-Host "Script root is: $PSScriptRoot"
. $PSScriptRoot\db-vars.ps1

Write-Host "Building an image for image: $imageName with version: $webAppVersion"
docker build -f $PSScriptRoot/../multi-stage.DOCKERFILE -t "${imageName}:$webAppVersion" .
Write-Host "Successfully built the image."

Write-Host "Loging in to the acr"
az acr login --name $acrName

$acrLoginServer = az acr show --name $acrName --query loginServer --output tsv

Write-Host "Tagging web app image"
$webAppImageFullName = "$acrLoginServer/${imageName}:$webAppVersion"
docker tag ${imageName}:$webAppVersion $webAppImageFullName

Write-Host "Pushing the image: '$webAppImageFullName' to ACR"
docker push $webAppImageFullName