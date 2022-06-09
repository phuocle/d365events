@echo off &setlocal enabledelayedexpansion
for /f "delims=" %%d in ('dir /a:d /o:-n /s ..\packages\DynamicsCrm.DevKit.Cli.* /b') do (
    set DynamicsCrmDevKitCli=%%d
    goto :break1
)
:break1
for /f "delims=" %%d in ('dir /a:d /o:-n /s ..\packages\Microsoft.CrmSdk.CoreTools.* /b') do (
    set MicrosoftCrmSdkCoreTools=%%d
    goto :break2
)
:break2
set "str1=%MicrosoftCrmSdkCoreTools%"
set "sstr=Microsoft.CrmSdk.CoreTools"
set /a position=0
set "sst0=!str1:*%sstr%=!"
set "sst1=!str1:%sstr%%sst0%=!"
if "%sst1%" neq "" for /l %%i in (0,1,8189) do if "!sst1:~%%i,1!" neq "" set /a position+=1
set /a "index=%position% + 27"
set version=!MicrosoftCrmSdkCoreTools:~%index%,50!
set CrmConnection="AuthType=ClientSecret;Url=https://orgc0877a1f.crm5.dynamics.com;ClientId=b2e1ffb7-4163-4d60-87b4-e504321ecd60;ClientSecret=HcI8Q~lQYb-7Em1JeX7mZ2GgRxP1RRbtGcG0Kazd;"
"%DynamicsCrmDevKitCli%\tools\DynamicsCrm.DevKit.Cli.exe" /conn:%CrmConnection% /json:"..\DynamicsCrm.DevKit.Cli.json" /type:"solutionpackagers" /profile:"Extract-Both" /version:"%version%"