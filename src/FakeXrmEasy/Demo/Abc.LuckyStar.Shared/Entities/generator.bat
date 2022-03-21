@echo off
for /f "delims=" %%d in ('dir /a:d /o:-n /s ..\..\packages\DynamicsCrm.DevKit.Cli.* /b') do (
    set DynamicsCrmDevKitCli=%%d
    goto break
)
:break
set CrmConnection="AuthType=ClientSecret;Url=https://orga451b5d1.crm5.dynamics.com;ClientId=a7425f91-f52a-4ebb-a293-9cb8424136d7;ClientSecret=PoU7Q~zxtbd5W1.D~qmWjKXbgelNaVhY0EWka;"
"%DynamicsCrmDevKitCli%\tools\DynamicsCrm.DevKit.Cli.exe" /conn:%CrmConnection% /json:"..\..\DynamicsCrm.DevKit.Cli.json" /type:"generators" /profile:"LATEBOUND"