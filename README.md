<div align="center">

# 💎 DIMA: Elite Financial Ecosystem
### *Modern Management — Empowering Your Financial Journey*

<img src="Dima.Web/wwwroot/img/logo_light.png" alt="Dima Logo" width="200" />

[![.NET 10](https://img.shields.io/badge/.NET-10.0-indigo?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-WASM-violet?style=for-the-badge&logo=blazor)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![MudBlazor](https://img.shields.io/badge/MudBlazor-Interactive-6366F1?style=for-the-badge)](https://mudblazor.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=for-the-badge&logo=docker)](https://www.docker.com/)

[Features](#-key-features) • [Tech Stack](#-technology-stack) • [Architecture](#-architecture--design) • [Quick Start](#-quick-start)

</div>

---

## 🏛️ Overview

**DIMA** is a high-performance personal finance management platform designed for users who demand precision, security, and a premium user experience. Built with the latest .NET 10 ecosystem, the project prioritizes **SOLID principles**, architectural robustness, and professional-grade UI/UX.

This version features a modernized **Indigo & Violet** aesthetic, decoupled security layers, and an automated data seeding engine to provide a seamless "Out of the Box" experience.

---

## 📸 Interface Experience

<div align="center">

### 📊 Professional Dashboard
| Light Mode (Indigo) | Dark Mode (Slate) |
| :---: | :---: |
| ![Dashboard Light](screenshots/Dima_Dashboard_Light.jpeg) | ![Dashboard Dark](screenshots/Dima_Dashboard_Dark.jpeg) |

</div>

---

## ✨ Key Features

| Feature | Description | Status |
| :--- | :--- | :---: |
| **🛡️ SOLID Identity** | Decoupled authentication system with custom registration and secure session handling. | ✅ |
| **📈 Dynamic Analytics** | Real-time interactive charts for income, expenses, and category-based distribution. | ✅ |
| **💳 Stripe Integration** | Full checkout flow for premium subscriptions and order management. | ✅ |
| **🌱 Smart Seeding** | User-choice demo data generation upon registration for immediate exploration. | ✅ |
| **🎨 Modern UI/UX** | Refined typography (Inter), rounded aesthetics, and smooth CSS transitions. | ✅ |
| **🚢 Containerized** | Full Docker support for both API and Frontend with CI/CD GitHub Actions. | ✅ |

---

## 🛠️ Technology Stack

### 🚀 **Frontend: Blazor WebAssembly**
Powered by .NET 10, the frontend executes directly in the browser via WebAssembly. This ensures a strictly typed Single Page Application (SPA) experience with near-native performance.

### ⚡ **Backend: ASP.NET Core & Minimal APIs**
A lightweight and ultra-fast backend architecture. We leveraged Minimal APIs to reduce overhead and focus on high-concurrency performance and clean endpoint definitions.

### 💾 **Data: EF Core & SQL Server**
Uses Entity Framework Core for robust Object-Relational Mapping (ORM). Advanced dashboard views are managed directly via **EF Migrations** to ensure database schema consistency across environments.

### 🧩 **UI: MudBlazor & Custom CSS**
A premium component library enhanced with custom Indigo-theme transitions, modern hover effects, and optimized data tables for a true 'Fintech' look and feel.

---

## 🏗️ Architecture & Design

The project follows a clean **Layered Architecture**, ensuring that business logic is separated from infrastructure and presentation concerns.

```mermaid
graph LR
    User((User)) --> Web[Dima.Web - Blazor WASM]
    Web --> API[Dima.Api - Minimal APIs]
    API --> DB[(SQL Server)]
    Web & API -.-> Core[Dima.Core - Models & Shared Logic]
    
    style Web fill:#6366F1,stroke:#fff,color:#fff
    style API fill:#8B5CF6,stroke:#fff,color:#fff
    style Core fill:#1E293B,stroke:#fff,color:#fff
    style DB fill:#0F172A,stroke:#fff,color:#fff
```

### 📂 Repository Structure
- **Dima.Web**: The interactive client-side application.
- **Dima.Api**: High-performance backend services and security handlers.
- **Dima.Core**: Shared contracts, enums, and domain models.

---

## 🚥 Quick Start

### 1. Using Docker (Recommended)
```bash
docker-compose up -d
```

### 2. Manual Run
```bash
# Restore and Build
dotnet build

# Run API
dotnet run --project Dima.Api

# Run Web
dotnet run --project Dima.Web
```

---

<div align="center">

Crafted with technical excellence by **[Israel Anacleto]**.
</div>
