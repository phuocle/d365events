for /d /r . %%d in (bin,obj,packages,Release,.vs,TestResults) do @if exist "%%d" rd /s/q "%%d"