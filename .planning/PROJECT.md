# ItransitionTemplates

## Overview
ASP.NET Core 8.0 MVC web application for creating, sharing, and managing form templates. Creators define templates with various question types (single-line, multiline, positive integer, checkbox, multiple options), control visibility (public/private), manage admins, and allow users to respond. Built with Entity Framework Core + MySQL (Pomelo), session-based authentication, Bootstrap 5, and jQuery.

## Tech Stack
- **Framework:** ASP.NET Core 8.0 (MVC)
- **ORM:** Entity Framework Core 8.0.10
- **Database:** MySQL 8.0 (via Pomelo.EntityFrameworkCore.MySql 8.0.2)
- **Styling:** Bootstrap 5 (CDN) + custom CSS
- **JS:** jQuery (vanilla for API calls)
- **Auth:** Session-based (ASP.NET Core Session)
- **Real-time:** SignalR (referenced in csproj)
- **Mapping:** AutoMapper 12.0.1

## Architecture
Standard MVC pattern with service layer:
- `Controllers/` — 5 controllers (Home, Template, User, Question, Response)
- `Models/` — 13 EF entities (Template, User, Question, QuestionOption, Response, Like, Admin, Tag, Topic, Comment, etc.)
- `Services/` — Service interfaces and implementations per domain
- `Views/` — Razor views (Home, Template, User, Shared)
- `wwwroot/js/` — Client-side JS organized by feature
- `wwwroot/css/` — Minimal custom CSS (72 lines)
- `Data/` — EF DbContext
- `Migrations/` — EF migrations

## Key Files
- `Program.cs` — App configuration, DI registration
- `ItransitionTemplates.csproj` — Package references
- `Views/Shared/_Layout.cshtml` — Main layout (Bootstrap 5 navbar)
- `Views/Home/Index.cshtml` — Home page with template search and listing
- `Views/Template/CreateTemplate.cshtml` — Template builder (most complex view)
- `Views/Template/TemplateView.cshtml` — Template response/fill view
- `Views/User/SignUpView.cshtml` — Registration form
- `Views/User/LogInView.cshtml` — Login form
- `wwwroot/css/site.css` — Global styles (72 lines)
- `wwwroot/js/index/index.js` — Home page logic
- `wwwroot/js/createTemplate/createTemplate.js` — Template builder logic

## Current State
- Working application with basic Bootstrap 5 styling
- Minimal custom CSS (72 lines in site.css)
- Inconsistent visual design across pages
- No design system or component library
- Basic form layouts without UX considerations
- Typos in some UI text ("tempalte", "contianer")
- No loading states, toast notifications, or empty states
- jQuery-based API calls without proper error handling UX
