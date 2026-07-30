# Phase 7: Security Hardening — Session Auth, IDOR Prevention & CSRF

## Problem

SECURITY-STRATEGY.md identified multiple session-gating and IDOR gaps. Several API endpoints lack session validation, private templates are accessible without ownership checks, search queries leak private content, user enumeration is possible, and no anti-CSRF protection exists on state-changing endpoints.

## Current state

**Session-gating gaps (unauthenticated access allowed):**
1. `GET /user/get-by-username?username=` — returns user info without auth
2. `GET /template/template/user?userId=&page=&limit=` — returns all templates (incl. private) by any user
3. `GET /template/get-template?templateId=` — returns any template by ID incl. private ones
4. `GET /template/get-by-query?text=` — searches all templates without IsPublic filter
5. `GET /template/likes?templateId=` — returns like count for any template

**IDOR/BOLA risks:**
- Private templates accessible via `/template/get-template` by guessing ID
- `GetTemplatesByUserId` returns all templates (public + private) for any user
- `GetTemplatesByQuery` uses raw MySQL full-text with no `IsPublic` filter
- User lookup via `/user/get-by-username` enables user enumeration

**CSRF:**
- No `AutoValidateAntiforgeryTokenAttribute` on POST/PUT/DELETE endpoints
- No `@Html.AntiForgeryToken()` in forms

**Session cookie:**
- `SameSite=Lax` (should be `Strict` in production)
- `CookieSecurePolicy` not explicitly set

## Target state

- All authenticated endpoints call `Auth.ValidateSession(HttpContext)` and return 401 on failure
- Private templates enforce ownership check (creator/admin)
- `GetTemplatesByQuery` and `GetTemplatesByUserId` filter by `IsPublic`
- `GetUserByUsername` requires authenticated session
- Anti-CSRF token validation on all POST/PUT/DELETE endpoints
- Secure session cookie config (`SameSite=Strict`, `SecurePolicy=Always`)

## Scope

- **In scope:** 5 controller endpoints needing session validation, private template access control, IsPublic filtering in 2 service methods, CSRF token attributes on controllers, session cookie hardening
- **Out of scope:** Argon2id password upgrade (tracked in SECURITY-STRATEGY.md), HSTS configuration, SignalR auth, rate limiting

## Key files

| File | Purpose |
|------|---------|
| `Controllers/Templates.cs` | Template endpoints (session + IDOR fixes) |
| `Controllers/UserController.cs` | User endpoints (session gate for get-by-username) |
| `Controllers/HomeController.cs` | Home (cookie config reference) |
| `Controllers/Question.cs` | Question endpoint (session check exists already) |
| `Controllers/ResponseController.cs` | Response endpoint (session check exists already) |
| `Utils/Auth.cs` | Session validation utility (already exists) |
| `Utils/Session.cs` | Session store/get |
| `Services/Template/Template.cs` | Add IsPublic filtering |
| `Services/User/User.cs` | GetUserByUsername (needs alternate query for testability) |
| `Program.cs` | Session cookie config |
| `tests/` | Integration tests for 401 on unauthenticated access |
