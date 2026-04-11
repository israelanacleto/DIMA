# Dima.Web — Frontend Blazor WASM

Projeto frontend da plataforma DIMA. SPA em Blazor WebAssembly com design system corporativo Navy & Cyan.

## Design System

- **Paleta**: Navy `#0F2D5E` (primária) + Cyan `#00B4D8` (destaque) + `#0A1628` (drawer/dark bg)
- **Tipografia**: Inter (400/500/600/700) para interface + JetBrains Mono para valores numéricos
- **Tema**: configurado em `Configuration.cs` via `MudTheme` (PaletteLight + PaletteDark)
- **CSS**: tokens globais em `wwwroot/css/app.css` + estilos da landing em `wwwroot/css/landing.css`

## Estrutura de Layouts

| Layout | Rota | Uso |
| :--- | :--- | :--- |
| `LandingLayout` | `/` | Homepage pública sem autenticação |
| `HeadlessLayout` | `/login`, `/register`, `/logout` | Tela dividida 50/50 — painel Navy + formulário |
| `MainLayout` | Demais rotas autenticadas | AppBar Navy + drawer lateral + conteúdo |

## Logos

Logos em SVG vetorial — sem dependência de raster:

| Arquivo | Uso |
| :--- | :--- |
| `wwwroot/img/logo_dark.svg` | Wordmark para fundos escuros/Navy (texto branco + Cyan) |
| `wwwroot/img/logo_light.svg` | Wordmark para fundos claros (texto Navy + Cyan) |
| `wwwroot/img/icon.svg` | Badge compacto para favicon e tamanhos pequenos |
