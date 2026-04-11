<div align="center">

# DIMA — Plataforma Financeira

### Gestão moderna de finanças pessoais com nível corporativo

<img src="Dima.Web/wwwroot/img/logo_light.svg" alt="DIMA Logo" width="180" />

[![.NET 10](https://img.shields.io/badge/.NET-10.0-0F2D5E?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-WASM-0F2D5E?style=for-the-badge&logo=blazor)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![MudBlazor](https://img.shields.io/badge/MudBlazor-7.x-00B4D8?style=for-the-badge)](https://mudblazor.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=for-the-badge&logo=docker)](https://www.docker.com/)

[Funcionalidades](#-funcionalidades) • [Stack](#-stack-tecnológica) • [Arquitetura](#-arquitetura) • [Início Rápido](#-início-rápido)

</div>

---

## Visão Geral

**DIMA** é uma plataforma de gestão financeira pessoal construída com foco em experiência corporativa. O projeto vai além do controle de gastos — apresenta uma landing page institucional, dashboard analítico, gestão de categorias, transações, planos de assinatura e integração com Stripe.

Desenvolvido como projeto de portfólio para demonstrar domínio em arquitetura Blazor WASM, design systems e integração de APIs financeiras.

---

## Funcionalidades

| Funcionalidade | Descrição | Status |
| :--- | :--- | :---: |
| **Landing Page** | Homepage institucional com hero, features, planos e CTA | ✅ |
| **Autenticação** | Registro, login e logout com Identity desacoplado | ✅ |
| **Dashboard** | Visão de saldo, receitas, despesas e gráficos por categoria | ✅ |
| **Transações** | Cadastro, edição e listagem de entradas e saídas | ✅ |
| **Categorias** | Organização por categorias com grid visual | ✅ |
| **Planos** | Cards de assinatura Free e Premium | ✅ |
| **Stripe** | Fluxo completo de checkout para upgrade de plano | ✅ |
| **Modo Escuro** | Dark mode com paleta Navy refinada | ✅ |
| **Seed de Dados** | Geração de dados demo no cadastro | ✅ |
| **Docker** | Containerização completa com docker-compose | ✅ |

---

## Stack Tecnológica

- **Frontend**: Blazor WebAssembly (.NET 10) — SPA com render client-side
- **Backend**: ASP.NET Core Minimal APIs — endpoints de alta performance
- **Banco de Dados**: EF Core + SQL Server com Migrations automatizadas
- **UI/UX**: MudBlazor com design system customizado (Navy `#0F2D5E` + Cyan `#00B4D8`)
- **Tipografia**: Inter (interface) + JetBrains Mono (valores financeiros)
- **Pagamentos**: Stripe Checkout integrado via JS Interop

---

## Arquitetura

O projeto segue **Arquitetura em Camadas** com separação clara de responsabilidades:

```mermaid
graph LR
    User((Usuário)) --> Web[Dima.Web · Blazor WASM]
    Web --> API[Dima.Api · Minimal APIs]
    API --> DB[(SQL Server)]
    Web & API -.-> Core[Dima.Core · Lógica Compartilhada]

    style Web fill:#0F2D5E,stroke:#00B4D8,color:#fff
    style API fill:#0A1628,stroke:#00B4D8,color:#fff
    style Core fill:#00B4D8,stroke:#fff,color:#0A1628
    style DB fill:#1E293B,stroke:#fff,color:#fff
```

---

## Início Rápido

### Docker (recomendado)
```bash
docker-compose up -d
```

### Execução Manual
```bash
# Compilar solução
dotnet build

# Executar API
dotnet run --project Dima.Api

# Executar Web (em outro terminal)
dotnet run --project Dima.Web
```

---

<div align="center">

Desenvolvido por **Israel Anacleto**

</div>
