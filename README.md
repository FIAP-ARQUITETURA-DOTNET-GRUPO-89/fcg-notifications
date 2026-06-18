# 🚀 FcgNotifications

Template de API desenvolvido com **ArchForge**, uma CLI para geração de projetos .NET padronizados.

O objetivo do ArchForge é acelerar a criação de novos serviços, eliminando tarefas repetitivas de configuração e fornecendo uma estrutura consistente, testável e pronta para evolução.

## 📑 Sumário

- [📋 Tecnologias Utilizadas](#-tecnologias-utilizadas)
- [🏛 Arquitetura](#-arquitetura)
- [📁 Estrutura da Solução](#-estrutura-da-solução)
- [▶️ Executando Localmente](#️-executando-localmente)
- [🗄 Banco de Dados](#-banco-de-dados)
- [🔐 Autenticação JWT](#-autenticação-jwt)
  - [Gerando Token Manualmente](#gerando-token-manualmente)
  - [Policies Disponíveis](#policies-disponíveis)
- [📡 Coleção Postman](#-coleção-postman)
- [🧪 Executando Testes](#-executando-testes)
- [📚 Documentação](#-documentação)
  - [ADRs](#adrs)
  - [Diagramas](#diagramas)
  - [Linguagem Ubíqua](#linguagem-ubíqua)
- [🎯 Objetivos do Template](#-objetivos-do-template)

## 📋 Tecnologias Utilizadas

- .NET 10
- ASP.NET Core Minimal API
- .NET Aspire
- MediatR
- FluentValidation
- Mapperly
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- Health Checks
- Serilog
- xUnit
- Shouldly
- NSubstitute
- Aspire Testing
- Respawn

## 🏛 Arquitetura

Este template segue princípios de:

- Clean Architecture
- Domain-Driven Design (DDD)
- CQRS
- SOLID
- Separation of Concerns

### Camadas

| Projeto                    | Responsabilidade                                       |
| -------------------------- | ------------------------------------------------------ |
| FcgNotifications.Api             | Endpoints, Middlewares e Configurações                 |
| FcgNotifications.Application     | Casos de uso, Commands, Queries, Validators e Handlers |
| FcgNotifications.Domain          | Entidades, Regras de Negócio e Contratos               |
| FcgNotifications.Infrastructure  | Persistência, EF Core e Repositórios                   |
| FcgNotifications.IoC             | Registro de dependências                               |
| FcgNotifications.SharedKernel    | Componentes compartilhados                             |
| FcgNotifications.ServiceDefaults | Configurações compartilhadas Aspire                    |
| FcgNotifications.AppHost         | Orquestração Aspire                                    |

## 📁 Estrutura da Solução

```text
src/
├── FcgNotifications.Api
├── FcgNotifications.AppHost
├── FcgNotifications.Application
├── FcgNotifications.Domain
├── FcgNotifications.Infrastructure
├── FcgNotifications.IoC
├── FcgNotifications.ServiceDefaults
└── FcgNotifications.SharedKernel

tests/
├── FcgNotifications.UnitTests
└── FcgNotifications.IntegrationTests

docs/
├── adrs
├── api-collection
├── diagrams
└── linguagem-ubiqua
```

## ▶️ Executando Localmente

### Restaurar dependências

```bash
dotnet restore
```

### Compilar

```bash
dotnet build
```

### Executar com Aspire

```bash
dotnet run --project src/FcgNotifications.AppHost
```

### Executar apenas a API

```bash
dotnet run --project src/FcgNotifications.Api
```

## 🗄 Banco de Dados

Criar migration:

```bash
dotnet ef migrations add MinhaMigration -p src/FcgNotifications.Infrastructure -s src/FcgNotifications.Api
```

Aplicar migrations:

```bash
dotnet ef database update -p src/FcgNotifications.Infrastructure -s src/FcgNotifications.Api
```

## 🔐 Autenticação JWT

O template já possui autenticação JWT configurada.

Configuração padrão:

```json
"JwtSettings": {
    "Issuer": "FcgNotifications-Issuer",
    "SecurityKey": "FcgNotifications_Secret_Key_2026_High_Security_Token",
    "ExpirationHours": 2
}
```

### Gerando Token Manualmente

Acesse:

🌐 https://jwt.io

#### Header

```json
{
  "alg": "HS256",
  "typ": "JWT"
}
```

#### Payload para perfil Admin

```json
{
  "iss": "FcgNotifications-Issuer",
  "sub": "1",
  "name": "Administrador",
  "role": "Admin",
  "exp": 1893456000
}
```

#### Payload para perfil Customer

```json
{
  "iss": "FcgNotifications-Issuer",
  "sub": "2",
  "name": "Cliente",
  "role": "Customer",
  "exp": 1893456000
}
```

#### Secret

```text
FcgNotifications_Secret_Key_2026_High_Security_Token
```

Após gerar o token, utilize:

```http
Authorization: Bearer {TOKEN}
```

### Policies Disponíveis

| Policy         | Roles Permitidas |
| -------------- | ---------------- |
| CustomerPolicy | Admin, Customer  |
| AdminPolicy    | Admin            |

## 📡 Coleção Postman

A coleção da API está disponível em:

```text
docs/api-collection/
```

Importe o arquivo `.json` no Postman para iniciar os testes rapidamente.

## 🧪 Executando Testes

### Todos os testes

```bash
dotnet test
```

### Unitários

```bash
dotnet test tests/FcgNotifications.UnitTests
```

### Integração

```bash
dotnet test tests/FcgNotifications.IntegrationTests
```

## 📊 Cobertura de Testes

O template já possui suporte à geração de cobertura de testes utilizando Coverlet.

### Gerar cobertura dos testes unitários

```bash
dotnet test tests/FcgNotifications.UnitTests --collect:"XPlat Code Coverage" --settings .runsettings
```

### Instalar o ReportGenerator

Caso ainda não possua a ferramenta instalada:

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

### Gerar relatório HTML

```bash
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:"Html;MarkdownSummary"
```

### Visualizar relatório

Abra o arquivo:

```text
coverage-report/index.html
```

## 📚 Documentação

A documentação do projeto fica centralizada na pasta:

```text
docs/
```

### ADRs

```text
docs/adrs
```

Registro das decisões arquiteturais.

### Diagramas

```text
docs/diagrams
```

Diagramas de arquitetura e fluxo.

### Linguagem Ubíqua

```text
docs/linguagem-ubiqua
```

Glossário do domínio.

## 🎯 Objetivos do Template

Este template foi criado para fornecer:

- Estrutura pronta
- Padronização entre serviços
- Alta cobertura de testes
- Baixo tempo de setup
- Facilidade de manutenção
- Evolução arquitetural consistente

Gerado com ❤️ utilizando ArchForge.
