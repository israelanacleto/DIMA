# DIMA — Gestão Financeira Pessoal

DIMA é uma aplicação fullstack de gestão financeira pessoal construída com **.NET 8**, **ASP.NET Core** e **Blazor WebAssembly**.

## Funcionalidades

- **Transações**: registro de depósitos e retiradas com categoria, valor e data
- **Categorias**: organização das transações por categoria personalizada
- **Pedidos / Pagamentos**: gestão de pedidos com integração ao **Stripe**
- **Relatórios financeiros**:
  - Resumo financeiro
  - Receitas vs. despesas
  - Despesas por categoria
  - Receitas por categoria

## Tecnologias

| Camada | Tecnologias |
|--------|-------------|
| Backend (API) | ASP.NET Core 8, Entity Framework Core 8, SQL Server, ASP.NET Core Identity, Stripe.net, Swagger |
| Frontend | Blazor WebAssembly 8, MudBlazor |
| Compartilhado | .NET 8 Class Library (modelos, handlers, requests/responses) |

## Estrutura do projeto

```
src/
├── Dima.Api      # API REST (ASP.NET Core)
├── Dima.Core     # Biblioteca compartilhada (modelos, enums, contratos)
└── Dima.Web      # Frontend Blazor WebAssembly
```

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (local ou remoto)
- Conta no [Stripe](https://stripe.com) (para funcionalidades de pagamento)

## Como executar

1. Clone o repositório:
   ```bash
   git clone https://github.com/israelanacleto/DIMA.git
   cd DIMA
   ```

2. Configure a string de conexão e a chave da API do Stripe em `src/Dima.Api/appsettings.json` (ou via `user-secrets`).

3. Aplique as migrations do banco de dados:
   ```bash
   cd src/Dima.Api
   dotnet ef database update
   ```

4. Inicie a API:
   ```bash
   dotnet run --project src/Dima.Api
   ```

5. Em outro terminal, inicie o frontend:
   ```bash
   dotnet run --project src/Dima.Web
   ```

6. Acesse `https://localhost:5001` no navegador.
