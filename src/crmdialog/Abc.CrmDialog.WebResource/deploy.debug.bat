@echo off
for /f "delims=" %%d in ('dir /a:d /o:-n /s ..\packages\DynamicsCrm.DevKit.Cli.* /b') do (
    set DynamicsCrmDevKitCli=%%d
    goto break
)
:break
set CrmConnection="AuthType=ClientSecret;Url=https://org581e92dc.crm5.dynamics.com;ClientId=ac758a0d-4aee-4763-b91a-5c269d2dd983;ClientSecret=YnoI6CA-3-v4RWSHqm~5u92i43n_0S71__;"
"%DynamicsCrmDevKitCli%\tools\DynamicsCrm.DevKit.Cli.exe" /conn:%CrmConnection% /json:"..\DynamicsCrm.DevKit.Cli.json" /type:"webresources" /profile:"DEBUG"
exit