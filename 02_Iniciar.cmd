@echo off
setlocal
cd /d "%~dp0"
echo Iniciando API de Inventario...
echo Cuando aparezca Now listening on, abri http://localhost:5082/swagger
echo Para detener la API usa Ctrl+C.
echo.
dotnet run --project ApiInventario/ApiInventario.vbproj
if errorlevel 1 goto error
exit /b 0
:error
echo.
echo La API no pudo iniciarse. Copia el error mostrado arriba para revisarlo.
pause
exit /b 1
