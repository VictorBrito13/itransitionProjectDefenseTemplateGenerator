# Security Strategy — ItransitionTemplates

**Decided:** 2026-07-30 · **Status:** Accepted · **ASVS level:** 1 (written to `security_asvs_level`)

> The app-wide, decide-once posture. Per-feature enforcement stays folded (per-phase threat models → security-auditor → secure-phase; supply-chain in cicd). Scale-to-zero: low-sensitivity app, half-page posture.

## Data classification & regime

- **Most sensitive data:** internal — user PII (email, username, PBKDF2-hashed password), form templates (some public, some private), template responses
- **Regulatory regime(s):** none — no GDPR/PCI/HIPAA trigger

## ASVS level (derived, not defaulted)

**L1** — the app is a form-template tool with no regulated data, no financial transactions, and limited blast radius (template content + user accounts). Users can register, create templates, and submit responses. Written back to `security_asvs_level`.

## Authn / Authz model

- **Authz:** RBAC + ownership checks — two roles (user, template-admin). Deny-by-default, server-side enforced through `Auth.ValidateSession(HttpContext)` + `AdminService.IsUserAdmin()`. Per-feature BOLA/BFLA checks stay folded.
- **Auth method:** opaque-cookie session (ASP.NET Core Session with httpOnly+SameSite=Lax httpOnly cookies). No JWT, no OAuth — single first-party MVC app. Refresh on every login; clear on logout.
- **Multi-tenancy:** single (users share the app; templates scoped via `UserId` on `Admin` join table)
- Actors/roles: `User` (authenticated), `Admin` (per-template admin, mapped via `Admins` table)

## Public endpoints (no session required)

1. `POST /user/sign-up` + `GET /user/sign-up` — registration
2. `POST /user/log-in` + `GET /user/log-in` — login
3. `GET /template/templates` — public template listing (returns only `IsPublic=true`)

All other endpoints require a valid session (`Auth.ValidateSession`).

## Session-gating gaps (folded — per-phase enforcement)

The following endpoints currently lack session validation:

| Endpoint | Risk |
|---|---|
| `GET /user/get-by-username?username=` | Leaks user existence — needs session gate |
| `GET /template/template/user?userId=&page=&limit=` | Leaks all templates (incl. private) by a user — needs session + IsPublic filter |
| `GET /template/get-template?templateId=` | Returns any template incl. private — needs IsPublic check or ownership gate |
| `GET /template/get-by-query?text=` | Searches all templates incl. private (no IsPublic filter in SQL) — needs IsPublic filter |
| `GET /template/likes?templateId=` | Returns likes for any template — low risk, but needs alignment |

## IDOR prevention (folded — per-feature)

Key IDOR risks identified:

- `GET /template/get-template` — **BOLA**: anyone can fetch any template by ID including private ones. Must check `IsPublic` or verify user is admin/owner before returning private templates.
- `GET /template/template/user` — **BFLA**: returns templates by any user including their private templates. Needs user-scoped filter or session ownership check.
- `GET /template/get-by-query` — **search leak**: the SQL full-text query doesn't filter by `IsPublic`. Injects `MATCH(title, description)` directly — also a SQL injection concern (raw `FromSqlRaw` with user text).
- `PUT /template/update` / `DELETE /template/delete` — already has admin check ✓
- `GET /user/get-by-username` — information disclosure via user enumeration. Needs session gate at minimum.

## XSS prevention (folded — per-feature)

- Razor views use `@` syntax (auto-HTML-encoded) ✓
- Client-side template rendering in `printTemplates.js` uses `escapeHtml()` helper (textContent→innerHTML pattern) ✓
- `templateView.js` renders template title/description via `.textContent` ✓
- **Wildcard `innerHTML` usage** in `index.js` for search results — currently uses static template literals (no raw user interpolation), but the pattern is fragile. Tracked for hardening.
- **Comment rendering** (future): comments must use `.textContent` or `escapeHtml()` — never raw `innerHTML` with user content.

## Password hashing

Current: PBKDF2-SHA256 (100K iterations, 16-byte salt). The floor calls for **Argon2id**. Track: upgrade to `Konscious.Security.Cryptography.Argon2` for ASVS L2+ hardening. Acceptable at L1, but should be uplifted.

## App-wide threat-model parent

### Trust boundaries
- **Browser ↔ API** (TLS ≥1.2) — the primary attack surface. Session cookie crosses this boundary.
- **API ↔ MySQL DB** (internal network, no TLS currently — production should enforce TLS)
- **API ↔ filesystem** (image uploads to `wwwroot/images/` — validate MIME type and extension)

### Data flows (sensitive)
- User credentials → login endpoint → PBKDF2 verify → session created
- Template data → CRUD endpoints → MySQL storage
- Response data → response endpoint → MySQL storage (includes user email)
- Search query → `FromSqlRaw` → MySQL full-text index

### Top-level STRIDE

| Threat | Disposition |
|---|---|
| **Spoofing** | Session-based auth; PBKDF2 password hashing. No MFA (L1). |
| **Tampering** | TLS for transport; at-rest encryption via MySQL transparent encryption (platform default). No request signing. |
| **Repudiation** | Server-side logging for mutations (create/update/delete). No audit trail for reads. |
| **Information disclosure** | **Key gap**: private templates accessible without auth via `get-template`, `get-by-query`, `template/user`. Session-gating gap on 5 endpoints. User enumeration via `get-by-username`. |
| **DoS** | No rate limiting. Acceptable at L1. |
| **Elevation of privilege** | Admin check via `IsUserAdmin()` on update/delete ✓. No privilege escalation vector in current code. |

## Secrets / key strategy

- **No secrets in code** — connection string from `appsettings.json` / environment variable (`ConnectionStrings__DefaultConnection`)
- **Workload-identity-first** — cloud-specific config → `INFRA-STRATEGY.md` (TBD)
- **No secret manager at L1** — env vars sufficient. Trigger for dedicated manager: audit requirement or ASVS L2+

## Security DoD (blocking CI gates — L1 floor)

- No secrets in code — pre-commit + repo scan
- SCA/dependency scan (`dotnet list package --vulnerable`)
- Lockfile + `ci` integrity check
- SAST on changed code (Semgrep or similar)
- Read-only CI token, pinned-`sub` OIDC
- Branch ruleset (require PR, require status checks)
- CSRF validation on all state-changing POST requests (ASP.NET Core `AutoValidateAntiforgeryTokenAttribute` — currently not enforced)

## Rungs above the floor

| Rung | Triggered? | Trigger |
|---|---|---|
| Dedicated secret manager | n | L1 app, env vars sufficient |
| Field-level encryption / tokenization | n | No regulated fields |
| mTLS service-to-service | n | Single-process app, no mesh |
| HSM / FIPS | n | Far below trigger |
| Argon2id password hashing | **track** | Floor requires it; L1 can defer |
| Anti-CSRF token validation | **track** | Not currently enforced; needed at L1 for state-changing POST endpoints |
