dotnet clean
dotnet restore
dotnet build

Set-Location Dice
.\publish.ps1

Set-Location ..

Set-Location Dice.Avalonia
.\publish.ps1

Set-Location ..
