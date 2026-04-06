#!/bin/bash
# Script pentru trimitere mesaje Salariat catre REGES API
# Utilizare: ./execute-send-salariat.sh

cd RegesIntegration || exit 1

# Verificare daca variabilele de mediu sunt setate
if [ -z "$API_USER" ]; then
    echo "EROARE: Variabila API_USER nu este setata"
    echo "Setati variabilele de mediu sau modificati scriptul"
    exit 1
fi

if [ -z "$API_PASSWORD" ]; then
    echo "EROARE: Variabila API_PASSWORD nu este setata"
    echo "Setati variabilele de mediu sau modificati scriptul"
    exit 1
fi

# Setare domenii (modificati dupa necesitate)
LOGIN_DOMAIN="${LOGIN_DOMAIN:-https://login.dev.inspectiamuncii.org}"
API_DOMAIN="${API_DOMAIN:-https://api.dev.inspectiamuncii.org}"

echo "==============================================="
echo "  Trimitere Mesaje Salariat catre REGES API"
echo "==============================================="
echo ""
echo "Utilizator: $API_USER"
echo "Login Domain: $LOGIN_DOMAIN"
echo "API Domain: $API_DOMAIN"
echo ""

dotnet run -- --type send-salariat \
    --user "$API_USER" \
    --password "$API_PASSWORD" \
    --loginDomain "$LOGIN_DOMAIN" \
    --apiDomain "$API_DOMAIN"

echo ""
echo "==============================================="
echo "  Procesare finalizata"
echo "==============================================="
