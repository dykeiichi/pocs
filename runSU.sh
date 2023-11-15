# /bin/bash

dotnet restore
sudo dotnet build  Main --no-restore --output runSu -c Release
sudo ./runSu/Main || sudo rm -R ./runSu