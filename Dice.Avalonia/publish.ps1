dotnet publish -c Release -o "C:\Apps\$((Get-Item .).Name)\x64" -r win-x64 -f net7.0 --no-self-contained
