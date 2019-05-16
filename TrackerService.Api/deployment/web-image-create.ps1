param(
    [string] $webAppVersion = "v1.1",
    [string] $imageName = "tracking-svc-api"
)
Write-Host "Building an image for image: $imageName with version: $webAppVersion"
Write-Host "Current working directory is: ${pwd}"
docker build -f TrackerService.Api/multi-stage.DOCKERFILE -t "${imageName}:$webAppVersion" .
Write-Host "Successfully built the image."
