@echo off
for /f "delims=" %%d in ('dir /a:d /o:-n /s ..\packages\DynamicsCrm.DevKit.Cli.* /b') do (
    set DynamicsCrmDevKitCli=%%d
    goto break
)
:break
set CrmConnection="AuthType=ClientSecret;Url=https://orgc0877a1f.crm5.dynamics.com;ClientId=b2e1ffb7-4163-4d60-87b4-e504321ecd60;ClientSecret=HcI8Q~lQYb-7Em1JeX7mZ2GgRxP1RRbtGcG0Kazd;"
"%DynamicsCrmDevKitCli%\tools\DynamicsCrm.DevKit.Cli.exe" /conn:%CrmConnection% /json:"..\DynamicsCrm.DevKit.Cli.json" /type:"downloadreports" /profile:"DEBUG"