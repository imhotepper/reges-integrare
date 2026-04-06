@echo off
REM Script pentru primire raspunsuri din coada REGES API
REM Utilizare: execute-receive.bat

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
set MAX_MESSAGES=10

echo ===============================================
echo   Primire Mesaje din Coada REGES API
echo ===============================================
echo.
echo Utilizator: %API_USER%
echo Login Domain: %LOGIN_DOMAIN%
echo API Domain: %API_DOMAIN%
echo Numar maxim mesaje: %MAX_MESSAGES%
echo.

dotnet run -- --type receive --user %API_USER% --password %API_PASSWORD% --loginDomain %LOGIN_DOMAIN% --apiDomain %API_DOMAIN% --max-message %MAX_MESSAGES%

echo.
echo ===============================================
echo   Procesare finalizata
echo ===============================================
pause
