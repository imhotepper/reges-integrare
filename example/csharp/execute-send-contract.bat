@echo off
REM Script pentru trimitere mesaje Contract catre REGES API
REM Utilizare: execute-send-contract.bat

cd RegesIntegration

REM Verificare daca variabilele de mediu sunt setate
if "%API_USER%"=="" (
    echo EROARE: Variabila API_USER nu este setata
    echo Setati variabilele de mediu sau modificati scriptul
    pause
    exit /b 1
)

if "%API_PASSWORD%"=="" (
    echo EROARE: Variabila API_PASSWORD nu este setata
    echo Setati variabilele de mediu sau modificati scriptul
    pause
    exit /b 1
)

REM Setare domenii (modificati dupa necesitate)
set LOGIN_DOMAIN=https://login.dev.inspectiamuncii.org
set API_DOMAIN=https://api.dev.inspectiamuncii.org

echo ===============================================
echo   Trimitere Mesaje Contract catre REGES API
echo ===============================================
echo.
echo Utilizator: %API_USER%
echo Login Domain: %LOGIN_DOMAIN%
echo API Domain: %API_DOMAIN%
echo.

dotnet run -- --type send-contract --user %API_USER% --password %API_PASSWORD% --loginDomain %LOGIN_DOMAIN% --apiDomain %API_DOMAIN%

echo.
echo ===============================================
echo   Procesare finalizata
echo ===============================================
pause
