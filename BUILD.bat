@echo off
chcp 65001 > nul
setlocal 
echo Версии сборки:
echo 1 - Debug
echo 2 - Release
:SetBuild
set /p choice="Введите номер требуемой сборки: "
if "%choice%"=="1" ( 
echo Сборка Debug версии...
dotnet build -c Debug
)else if "%choice%"=="2" ( 
echo Сборка Release версии...
dotnet build -c Release
)else ( 
echo Некорректный номер сборки. Пожалуйста, выберите 1 или 2.
goto SetBuild)
endlocal
pause