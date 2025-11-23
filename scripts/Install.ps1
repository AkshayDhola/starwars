$repo = "AkshayDhola/starwars"
$version = "v1.0.0"
$binaryName = "starwars.exe"
$zipName = "starwars-win-x64-v1.0.0.tar.gz"

Write-Host "📦 Downloading $binaryName $version ..."

Invoke-WebRequest -Uri "https://github.com/$repo/releases/download/$version/$zipName" -OutFile $zipName

Write-Host "📂 Extracting..."
Expand-Archive -Path $zipName -DestinationPath "$env:TEMP\starwars" -Force

Write-Host "🚚 Moving binary to PATH location..."
Move-Item "$env:TEMP\starwars\$binaryName" "$env:ProgramFiles\starwars\$binaryName" -Force

$envPath = "$env:ProgramFiles\starwars"
if ($env:Path -notlike "*$envPath*") {
    setx PATH "$env:Path;$envPath"
}

Write-Host "🧹 Cleaning..."
Remove-Item $zipName

Write-Host ""
Write-Host "🎉 Installation complete!"
Write-Host "Reopen PowerShell and run: starwars --help"
