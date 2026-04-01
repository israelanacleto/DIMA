# 💎 DIMA: Project Roadmap & Features Registry

This document serves as a technical inventory of what has been implemented and a Strategic Backlog for future enhancements.

---

## ✅ Currently Implemented (Ready for Demo)

### 🔑 Identity & Security
- [x] **SOLID Identity System**: Fully decoupled custom Register, Login, and User Info endpoints.
- [x] **Cookie-based Auth**: Secure session management with local development HTTP support.
- [x] **Profile Management**: Backend handlers and frontend pages for user profile updates.
- [x] **Password Management**: Change password functionality with backend validation.

### 📂 Financial Management
- [x] **Category CRUD**: Full lifecycle management for income/expense categories.
- [x] **Transaction Engine**: Detailed recording of deposits and withdrawals.
- [x] **Period Filtering**: Extraction of financial history based on custom dates.

### 📊 Intelligence & UX
- [x] **High-Performance Dashboards**: Powered by SQL Views for instant data aggregation.
- [x] **Interactive Charts**: Visual breakdown of income vs. expenses and category distribution.
- [x] **Smart Seeding Engine**: UX option during registration to populate demo data.
- [x] **Help & Docs Center**: Interactive, publicly accessible guide integrated into the UI.
- [x] **Modern Design System**: Indigo/Violet theme with rounded aesthetics and smooth transitions.

### 💳 Premium Ecosystem
- [x] **Stripe Integration**: Infrastructure ready for payment session creation.
- [x] **Order Management**: Tracking of plan purchases and statuses.
- [x] **Subscription Logic**: Support for premium features based on user claims.
- [x] **Refund Support**: Backend logic for processing order reversals.

### 🏗️ Infrastructure
- [x] **Auto-Migration**: Database schema and Views created automatically on startup.
- [x] **Docker Ready**: Containerized API and Web services.
- [x] **CI/CD Pipeline**: GitHub Actions configured for automated Docker builds.

---

## 🚀 Future Roadmap (Technical & Business TO-DO)

### 🛠️ Phase 1: Technical Excellence (High Priority for Interviews)
- [ ] **Automated Testing Suite**: Implement Unit Tests for Handlers and Integration Tests for API Endpoints (xUnit/FluentAssertions).
- [ ] **Global Exception Handling**: Implement a centralized middleware to catch and standardize all API errors.
- [ ] **Audit Logging**: Track changes to sensitive data (orders and profile) for security compliance.
- [ ] **API Versioning**: Move from implicit versioning to explicit Header/URL versioning (v1, v2).

### ✨ Phase 2: Advanced User Features
- [ ] **Data Export Center**: Allow users to download their financial reports in PDF and Excel formats.
- [ ] **Recurring Transactions**: Support for fixed monthly costs (subscriptions, rent) that repeat automatically.
- [ ] **Multi-Currency Support**: Integration with an external API to convert balances to USD/EUR/BRL in real-time.
- [ ] **Real-time Notifications**: Use SignalR to notify users when a payment is confirmed or a limit is reached.

### 🎨 Phase 3: Visual & UX Refinement
- [ ] **Custom Icon Library**: Replace Material Icons with a custom curated set for a more unique brand identity.
- [ ] **Advanced Filtering**: Add search and multi-select filters to the transaction list.
- [ ] **Onboarding Tour**: An interactive walkthrough for first-time users (using MudBlazor Tour component).

---

## 📈 Strategic Summary for Recruiters
*When asked about the project's architecture:*
"DIMA is built on a **Layered Architecture** using **ASP.NET Core 10 Minimal APIs** and **Blazor WASM**. I focused heavily on **SOLID principles** by decoupling the Identity system from standard templates, ensuring that the backend remains testable and maintainable. I also implemented an automated database lifecycle management using **EF Core Migrations** and **SQL Views** for performance-optimized analytics."
