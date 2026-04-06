#!/bin/bash
# Script pentru primire raspunsuri din coada REGES API
# Utilizare: ./execute-receive.sh

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
MAX_MESSAGES="${MAX_MESSAGES:-10}"

echo "==============================================="
echo "  Primire Mesaje din Coada REGES API"
echo "==============================================="
echo ""
echo "Utilizator: $API_USER"
echo "Login Domain: $LOGIN_DOMAIN"
echo "API Domain: $API_DOMAIN"
echo "Numar maxim mesaje: $MAX_MESSAGES"
echo ""

dotnet run -- --type receive \
    --user "$API_USER" \
    --password "$API_PASSWORD" \
    --loginDomain "$LOGIN_DOMAIN" \
    --apiDomain "$API_DOMAIN" \
    --max-message "$MAX_MESSAGES"

echo ""
echo "==============================================="
echo "  Procesare finalizata"
echo "==============================================="
