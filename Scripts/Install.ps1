# Install.ps1 for Windows
# Run this script as Administrator (Right-click -> Run as Administrator)

Write-Host "Starting Star Wars game installation..." -ForegroundColor Cyan

# Define installation directory and executable name
$installDir = "C:\Program Files\StarWarsGame"
$exeName = "starwars.exe" # Ensure your actual executable is named 'starwars.exe'

# Create installation directory and copy files
if (!(Test-Path -Path $installDir)) {
    New-Item -Path $installDir -ItemType Directory | Out-Null
}
Copy-Item -Path .\* -Destination $installDir -Recurse

# Add the installation directory to the System PATH environment variable
$path = (Get-ItemProperty HKLM:\System\CurrentControlSet\Control\SessionManager\Environment -Name PATH).Path
if ($path -notlike "*$installDir*") {
    $newPath = $path + ";" + $installDir
    Set-ItemProperty HKLM:\System\CurrentControlSet\Control\SessionManager\Environment -Name PATH -Value $newPath
    Write-Host "PATH updated. You may need to restart your terminal or Explorer for changes to take effect." -ForegroundColor Yellow
} else {
    Write-Host "Directory already in PATH." -ForegroundColor Green
}

Write-Host "Installation complete. Try running 'starwars' in a new CMD or PowerShell window." -ForegroundColor Cyan
