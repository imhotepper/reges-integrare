# Implementare C# pentru REGES API Integration

Aceasta implementare C# oferă aceeași funcționalitate ca și exemplul Java pentru integrarea cu API-ul REGES Online.

## Cuprins

1. [Descriere](#descriere)
2. [Cerințe](#cerințe)
3. [Structura Proiectului](#structura-proiectului)
4. [Instalare](#instalare)
5. [Configurare](#configurare)
6. [Utilizare](#utilizare)
7. [Exemple](#exemple)
8. [Testare](#testare)
9. [Arhitectura Aplicației](#arhitectura-aplicației)

## Descriere

Implementarea C# oferă următoarele funcționalități:

- **Trimitere mesaje Salariat**: Trimite fișiere XML cu informații despre angajați către API-ul REGES
- **Trimitere mesaje Contract**: Trimite fișiere XML cu informații despre contracte către API-ul REGES
- **Primire răspunsuri**: Citește răspunsurile asincrone din coada API-ului REGES

## Cerințe

- .NET 8.0 SDK sau superior
- Acces la API-ul REGES (credențiale de autentificare)
- Conexiune la internet

## Structura Proiectului

```
example/csharp/
├── RegesIntegration/              # Proiectul principal
│   ├── DTOs/                      # Data Transfer Objects
│   │   ├── Header.cs
│   │   ├── MessageResponse.cs
│   │   ├── MessageResult.cs
│   │   ├── Result.cs
│   │   ├── TokenResponse.cs
│   │   └── UserDTO.cs
│   ├── Services/                  # Servicii pentru logica aplicației
│   │   ├── ReceiveMessages.cs
│   │   ├── SendContractMessages.cs
│   │   ├── SendSalariatMessages.cs
│   │   └── Utils.cs
│   ├── Program.cs                 # Punct de intrare în aplicație
│   └── appsettings.json          # Fișier de configurare
├── RegesIntegration.Tests/        # Proiect de teste
│   └── UnitTest1.cs              # Teste unitare
├── IN-SALARIAT/                   # Folder pentru mesaje salariat (de creat)
├── IN-CONTRACT/                   # Folder pentru mesaje contract (de creat)
├── OUT/                           # Folder pentru răspunsuri și fișiere procesate (de creat)
└── receive/                       # Folder pentru mesaje primite (de creat)
```

## Instalare

### 1. ClonareaRepository-ului

```bash
git clone https://github.com/reges-ro/integrare.git
cd integrare/example/csharp
```

### 2. Restore Dependințe

```bash
cd RegesIntegration
dotnet restore
```

### 3. Build Proiect

```bash
dotnet build
```

### 4. Crearea Folderelor Necesare

Creați următoarele foldere în directorul unde veți rula aplicația:

```bash
mkdir IN-SALARIAT
mkdir IN-CONTRACT
mkdir OUT
mkdir receive
```

## Configurare

### 1. Obținerea Credențialelor API

Pentru a obține credențialele API urmați pașii din documentația principală (README.md din rădăcina repository-ului), secțiunea "7.1 Pași pentru a obține un API Key".

### 2. Parametri de Configurare

Aplicația acceptă următorii parametri în linia de comandă:

| Parametru | Descriere | Obligatoriu | Exemplu |
|-----------|-----------|-------------|---------|
| `--type` | Tipul operației (`send-salariat`, `send-contract`, `receive`) | Da | `send-salariat` |
| `--user` | Utilizator API | Da | `your-api-user` |
| `--password` | Parola API | Da | `your-api-password` |
| `--loginDomain` | Domeniul de autentificare | Da | `https://login.dev.inspectiamuncii.org` |
| `--apiDomain` | Domeniul API | Da | `https://api.dev.inspectiamuncii.org` |
| `--max-message` | Numărul maxim de mesaje de primit (doar pentru `receive`) | Doar pentru receive | `10` |

## Utilizare

### 1. Trimitere Mesaje Salariat

Plasați fișierele XML cu mesaje salariat în folderul `IN-SALARIAT`, apoi rulați:

```bash
cd RegesIntegration
dotnet run -- --type send-salariat --user YOUR_USER --password YOUR_PASSWORD --loginDomain https://login.dev.inspectiamuncii.org --apiDomain https://api.dev.inspectiamuncii.org
```

**Ce se întâmplă:**
- Aplicația citește toate fișierele `.xml` din folderul `IN-SALARIAT`
- Se autentifică la API folosind OAuth2
- Trimite fiecare fișier către endpoint-ul `/api/Salariat`
- Salvează răspunsurile în folderul `OUT` cu prefixul `response-sal-`
- Mută fișierele procesate în folderul `OUT`

### 2. Trimitere Mesaje Contract

Plasați fișierele XML cu mesaje contract în folderul `IN-CONTRACT`, apoi rulați:

```bash
cd RegesIntegration
dotnet run -- --type send-contract --user YOUR_USER --password YOUR_PASSWORD --loginDomain https://login.dev.inspectiamuncii.org --apiDomain https://api.dev.inspectiamuncii.org
```

**Ce se întâmplă:**
- Aplicația citește toate fișierele `.xml` din folderul `IN-CONTRACT`
- Se autentifică la API folosind OAuth2
- Trimite fiecare fișier către endpoint-ul `/api/Contract`
- Salvează răspunsurile în folderul `OUT` cu prefixul `response-con-`
- Mută fișierele procesate în folderul `OUT`

### 3. Primire Răspunsuri

Pentru a primi răspunsurile asincrone din coada API:

```bash
cd RegesIntegration
dotnet run -- --type receive --user YOUR_USER --password YOUR_PASSWORD --loginDomain https://login.dev.inspectiamuncii.org --apiDomain https://api.dev.inspectiamuncii.org --max-message 10
```

**Ce se întâmplă:**
- Aplicația se conectează la API și citește mesaje din coadă
- Salvează fiecare mesaj primit în folderul `receive`
- Continuă până la primirea a `max-message` mesaje sau până când coada este goală
- Mesajele citite sunt consumate din coadă (metoda PollMessage)

## Exemple

### Exemplu 1: Trimitere Salariat pe Mediul de Test

```bash
cd RegesIntegration
dotnet run -- --type send-salariat \
  --user test-user \
  --password test-pass \
  --loginDomain https://login.dev.inspectiamuncii.org \
  --apiDomain https://api.dev.inspectiamuncii.org
```

### Exemplu 2: Trimitere Contract pe Mediul de Producție

```bash
cd RegesIntegration
dotnet run -- --type send-contract \
  --user prod-user \
  --password prod-pass \
  --loginDomain https://login.inspectiamuncii.ro \
  --apiDomain https://api.inspectiamuncii.ro
```

### Exemplu 3: Primire Maximum 50 de Mesaje

```bash
cd RegesIntegration
dotnet run -- --type receive \
  --user test-user \
  --password test-pass \
  --loginDomain https://login.dev.inspectiamuncii.org \
  --apiDomain https://api.dev.inspectiamuncii.org \
  --max-message 50
```

### Exemplu 4: Utilizare cu Script Batch (Windows)

Creați un fișier `run-send-salariat.bat`:

```batch
@echo off
cd RegesIntegration
dotnet run -- --type send-salariat --user %API_USER% --password %API_PASSWORD% --loginDomain https://login.dev.inspectiamuncii.org --apiDomain https://api.dev.inspectiamuncii.org
pause
```

### Exemplu 5: Utilizare cu Script Shell (Linux/Mac)

Creați un fișier `run-send-salariat.sh`:

```bash
#!/bin/bash
cd RegesIntegration
dotnet run -- --type send-salariat \
  --user "$API_USER" \
  --password "$API_PASSWORD" \
  --loginDomain https://login.dev.inspectiamuncii.org \
  --apiDomain https://api.dev.inspectiamuncii.org
```

Apoi faceți-l executabil:

```bash
chmod +x run-send-salariat.sh
./run-send-salariat.sh
```

## Testare

Proiectul include teste unitare pentru toate componentele principale.

### Rulare Teste

```bash
cd RegesIntegration.Tests
dotnet test
```

### Acoperire Teste

Testele acoperă:
- ✅ Crearea și validarea DTO-urilor
- ✅ Parsarea argumentelor din linia de comandă
- ✅ Validarea autentificării
- ✅ Serializare/deserializare XML

### Rezultate Așteptate

```
Passed!  - Failed:     0, Passed:     9, Skipped:     0, Total:     9
```

## Arhitectura Aplicației

### 1. DTOs (Data Transfer Objects)

Clasele DTO sunt folosite pentru serializare/deserializare XML:

- **UserDTO**: Credențiale utilizator
- **Header**: Metadate mesaj (MessageId, Operation, etc.)
- **MessageResponse**: Răspuns sincron (conține ResponseId - recipisa)
- **MessageResult**: Răspuns asincron (conține Result cu status operație)
- **Result**: Detalii rezultat (Code, Description, Ref)
- **TokenResponse**: Token OAuth2

### 2. Services

Serviciile implementează logica business:

- **Utils**: Autentificare OAuth2 și utilitare
- **SendSalariatMessages**: Trimitere mesaje salariat
- **SendContractMessages**: Trimitere mesaje contract
- **ReceiveMessages**: Primire răspunsuri din coadă

### 3. Program.cs

Punctul de intrare care:
1. Parsează argumentele
2. Validează parametrii
3. Creează serviciile necesare
4. Execută operația solicitată

## Flux de Lucru

### Pentru Trimitere Mesaje:

```
1. Citire fișiere XML din folder (IN-SALARIAT sau IN-CONTRACT)
2. Autentificare OAuth2 (obținere token)
3. Pentru fiecare fișier:
   a. Citire conținut XML
   b. Trimitere HTTP POST către API cu token
   c. Primire MessageResponse (sincron)
   d. Salvare răspuns în folder OUT
   e. Mutare fișier procesat în OUT
4. Afișare statistici
```

### Pentru Primire Răspunsuri:

```
1. Autentificare OAuth2 (obținere token)
2. Repetă până la max-message:
   a. HTTP POST către /api/Status/PollMessage
   b. Dacă există mesaj:
      - Salvare MessageResult în folder receive
      - Incrementare contor
      - Așteaptă 1 secundă
   c. Dacă nu există mesaje:
      - Oprire și afișare mesaj
3. Afișare statistici
```

## Diferențe față de Implementarea Java

| Aspect | Java | C# |
|--------|------|-----|
| Framework | Spring Boot | Console App .NET |
| Dependency Injection | Spring @Autowired | Manual în Program.cs |
| Configurare | application.yml | appsettings.json |
| HTTP Client | RestTemplate / HttpClient | HttpClient |
| XML Serialization | Jackson XML | System.Xml.Serialization |
| Build Tool | Maven | dotnet CLI |
| Argumente CLI | -Dkey=value | --key value |

## Troubleshooting

### Problema: "Folderul X nu exista"

**Soluție**: Creați folderele necesare:
```bash
mkdir IN-SALARIAT IN-CONTRACT OUT receive
```

### Problema: "Failed to obtain access token"

**Soluție**: Verificați:
- Credențialele (user/password) sunt corecte
- loginDomain este accesibil
- Conexiunea la internet funcționează

### Problema: "Nu exista mesaje pentru consum"

**Soluție**: Aceasta nu este o eroare - înseamnă că coada API este goală. Trimiteți mai întâi mesaje folosind `send-salariat` sau `send-contract`.

### Problema: Build Error

**Soluție**: Verificați versiunea .NET:
```bash
dotnet --version
```

Asigurați-vă că aveți .NET 8.0 sau superior instalat.

## Resurse Adiționale

- [Documentație principală REGES](../../README.md)
- [Schema XSD REGES](../../Schema%20reges.xsd)
- [Colecție Postman](../../Events.postman_collection.json)
- [Mediu de test REGES](https://reges.dev.inspectiamuncii.org)
- [Swagger API](https://api.dev.inspectiamuncii.org/swagger/)

## Suport

Pentru probleme sau întrebări:
1. Consultați documentația principală
2. Verificați issues pe GitHub: https://github.com/reges-ro/integrare/issues
3. Contactați echipa de suport REGES

## Licență

Acest cod este furnizat ca exemplu pentru integrarea cu API-ul REGES Online și poate fi modificat conform nevoilor dumneavoastră.
