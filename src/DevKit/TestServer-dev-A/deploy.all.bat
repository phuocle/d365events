@echo off
echo ************************************************************
echo Building solution: RELEASE MODE
echo ************************************************************
set MsBuild=""
if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" (
	set MsBuild="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
)
if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe" (
	set MsBuild="C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
)
if %MsBuild%=="" (
	echo msbuild.exe not found !!!
) else (
	call %MsBuild% /nologo /noautorsp /verbosity:minimal -p:Configuration=Release -target:Clean;Build Dev.DevKit.sln
	del Release\dll\*.pdb
	del Release\dll\*.xml
	del Release\dll\Microsoft.*.dll
)
for /f "delims=" %%d in ('dir /a:d /o:-n /s packages\DynamicsCrm.DevKit.Cli.* /b') do (
    set DynamicsCrmDevKitCli=%%d
    goto break
)
:break
set CrmConnection="AuthType=ClientSecret;Url=https://dynamicscrm-devkit-sep-2021.crm.dynamics.com;ClientId=baaf08d8-d8f9-4930-97fa-874ffb09f31c;ClientSecret=wUc7Q~3Pq06KQw42gKcl0y9ClyUaiEp_5Lo12;"
"%DynamicsCrmDevKitCli%\tools\DynamicsCrm.DevKit.Cli.exe" /conn:%CrmConnection% /json:"DynamicsCrm.DevKit.Cli.json" /type:"servers" /profile:"RELEASE"