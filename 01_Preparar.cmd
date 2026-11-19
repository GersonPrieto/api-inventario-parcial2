@echo off
setlocal
cd /d "%~dp0"
echo Preparando API de Inventario...
echo Se creara o actualizara InventarioDB en el servidor de appsettings.json.
echo.
where dotnet >nul 2>nul
if errorlevel 1 goto sin_dotnet
dotnet restore ApiInventario.sln
if errorlevel 1 goto error
dotnet build ApiInventario.sln --no-restore
if errorlevel 1 goto error
dotnet run --project ApiInventario/ApiInventario.vbproj --no-build -- --verificar-modelo
if errorlevel 1 goto error
dotnet run --project ApiInventario/ApiInventario.vbproj --no-build -- --migrar
if errorlevel 1 goto error
echo.
echo Preparacion completada. Ahora ejecuta 02_Iniciar.cmd.
pause
exit /b 0
:sin_dotnet
echo No se encontro dotnet. Instala el SDK de .NET 10 y vuelve a abrir esta ventana.
pause
exit /b 1
:error
echo.
echo No se completo la preparacion. Copia el error mostrado arriba para revisarlo.
echo No borres bases de datos ni tablas para intentar corregirlo.
pause
exit /b 1
