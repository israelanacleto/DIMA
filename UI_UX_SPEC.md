# 🎨 DIMA: UI/UX Redesign Specification

This document provides a technical and functional map for the Product Designer / UI Specialist.

## 🎯 Project Vision
**DIMA** is an Elite Financial Ecosystem. The goal is to move away from generic "Material Design" and achieve a **Premium Fintech** aesthetic (Clean, Sophisticated, High-Contrast).

---

## 🏗️ Technical Constraints
- **Framework**: Blazor WebAssembly (.NET 10).
- **Component Library**: MudBlazor (Must follow its grid and component logic).
- **Themes**: Must support both **Light Mode** and **Dark Mode**.
- **Current Palette**: Indigo (#6366F1) and Violet (#8B5CF6).

---

## 📄 Page Inventory (Views to be Redesigned)

### 1. Public / Onboarding
- **Login Page (`/login`)**: Floating card style, needs to be inviting and secure.
- **Register Page (`/register`)**: Includes a demo data seeding option.
- **Help Center (`/help`)**: Interactive tabbed documentation.

### 2. Analytics & Dashboards
- **Main Dashboard (`/`)**: 
    - Summary Cards (Balance, Incomes, Expenses).
    - Bar Charts (Monthly comparison).
    - Pie/Donut Charts (Category distribution).

### 3. Management (CRUDs)
- **Categories (`/categorias`)**: List, Create, and Edit views.
- **Transactions (`/lancamentos/historico`)**: 
    - Advanced data table with period filtering.
    - Transaction forms (Create/Edit).

### 4. Premium & E-commerce
- **Pricing Plans (`/plans`)**: Product cards showcasing features.
- **Order Details (`/orders/{number}`)**: Receipt/Order summary view.
- **Orders History (`/orders`)**: List of past purchases.

### 5. Settings
- **User Profile (`/settings/me`)**: Personal info management.
- **Security (`/settings/password`)**: Password change forms.

---

## 🧩 Global Components
- **Sidebar**: Main navigation menu.
- **AppBar**: Header with notifications, user profile, and theme toggle.
- **Data Tables**: Standard styling for all lists.
- **Modals/Dialogs**: Confirmations and quick alerts.

---

## 🎨 Visual Identity Requirements (Ask for Israel)
1. **Typography**: Define Inter or a similar professional sans-serif pairing.
2. **Spacing**: Increase whitespace for a modern, airy feel.
3. **Icons**: Suggest a consistent, thin-stroke icon set (e.g., Lucide or custom).
4. **Color System**: Detailed Indigo/Violet palette including 'Muted' and 'Surface' variations.
