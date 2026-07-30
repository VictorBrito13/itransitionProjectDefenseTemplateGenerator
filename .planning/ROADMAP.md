# Roadmap — ItransitionTemplates

## Milestone 1: UI Redesign & UX Enhancement

### Phase 1: UI Redesign — Tailwind CSS + Nielsen Heuristics

**Goal:** Redesign the website UI to be more attractive and usable, following Nielsen Heuristic principles and Information Architecture guidelines, while refactoring all components to use Tailwind CSS.

**Depends on:** None (first phase)

**Requirements:** [REQ-01, REQ-02, REQ-03, REQ-04, REQ-05]

**Plans:** 4 plans

**Wave 1** *(independent, can run in parallel)*:

- [x] 01-01-PLAN.md — Tailwind CSS setup and layout foundation
- [x] 01-02-PLAN.md — Authentication pages redesign (Login, SignUp)

**Wave 2** *(blocked on Wave 1 — requires layout from 01-01)*:

- [x] 01-03-PLAN.md — Home page redesign with template discovery UX
- [x] 01-04-PLAN.md — Template builder and response pages redesign

**Cross-cutting constraints:**

- All views must use Tailwind CSS classes (no Bootstrap)
- All interactive elements must have focus states (focus:ring-2)
- All forms must have inline validation
- Color palette: primary=#3B82F6, secondary=#10B981, error=#EF4444

### Phase 2: Design Token System & Visual Polish

**Goal:** Define a comprehensive design token system for cross-app consistency, implement remaining UX patterns (toast notifications, error pages, micro-interactions), and polish all views to ensure visually cohesive experience.

**Depends on:** Phase 1

**Requirements:** [REQ-06, REQ-07, REQ-08, REQ-09]

**Plans:** 3 plans

**Wave 1** *(foundation — design token system)*:

- [ ] 02-01-PLAN.md — Design Token System (CSS custom properties, extended config, documentation)

**Wave 2** *(blocked on Wave 1 — uses design tokens)*:

- [ ] 02-02-PLAN.md — Toast Notifications + Error Page Redesign (Sonner.js integration, Error.cshtml)
- [ ] 02-03-PLAN.md — Visual Consistency Audit (fix Bootstrap remnants in JS, remove Bootstrap lib)

**Cross-cutting constraints:**

- Design tokens must be documented in a single source of truth (`.planning/design-tokens.md`)
- All notifications use Sonner.js (per D-05)
- Error pages must follow the app's aesthetic (no default Bootstrap error styling)
- Animations must be tasteful and purposeful (not decorative)
- Consistent with Phase 1 color palette and typography

### Phase 6: E2E Testing with Cypress

**Goal:** Implement comprehensive end-to-end testing using Cypress for all application features from sign up to template likes, with reusable custom commands to avoid code duplication.

**Requirements**: TBD

**Depends on:** Phase 5

**Plans:** 4 plans

**Wave 0** *(prep — add data-cy test attributes to all views)*:
- [x] 06-00-PLAN.md — Add data-cy attributes to Razor views and JS dynamic elements

**Wave 1** *(foundation — Cypress setup, blocked on Wave 0)*:
- [x] 06-01-PLAN.md — Cypress setup and custom commands for authentication

**Wave 2** *(blocked on Wave 1 — requires Cypress foundation)*:
- [x] 06-02-PLAN.md — Authentication E2E tests (signup, login, logout, session-aware UI)
- [x] 06-03-PLAN.md — Template E2E tests (create, view, search, like/unlike)

**Cross-cutting constraints:**
- All interactive elements must have `data-cy` attributes with "cy-" prefix convention
- All Cypress selectors must use `[data-cy="..."]` — no classes, IDs, or tags
- All tests must use Cypress custom commands to avoid code duplication
- Sign-in logic must be encapsulated in cy.login() custom command
- Tests must cover both positive and negative scenarios
- All tests must pass with `npx cypress run`

---

*Phase 1 focuses on frontend-only changes. Backend services, controllers, and data models remain unchanged. All UI improvements are achieved through view layer modifications and Tailwind CSS migration.*

## Milestone 2: Code Quality & Testing

### Phase 3: Code Refactoring & Test Infrastructure

**Goal:** Eliminate duplicated code across backend controllers and frontend JavaScript, remove unused code, enforce separation of concerns following MVC best practices, and establish a comprehensive test suite with unit and integration tests.

**Depends on:** Phase 1 (completed)

**Requirements:** [REQ-10, REQ-11, REQ-12, REQ-13]

**Plans:** 4 plans

**Wave 1** *(independent, can run in parallel)*:

- [ ] 03-01-PLAN.md — Backend refactoring (shared JSON response helper, session validation consolidation, ILogger migration)
- [ ] 03-02-PLAN.md — Frontend refactoring (BaseQuestion class, shared validation utility, toast partial, unused file removal)

**Wave 2** *(blocked on Wave 1 — requires refactored services)*:

- [ ] 03-03-PLAN.md — Test infrastructure & unit tests (xUnit project, service tests, utility tests)

**Wave 3** *(blocked on Wave 2 — requires test infrastructure)*:

- [ ] 03-04-PLAN.md — Integration tests (WebApplicationFactory, controller endpoint tests)

**Cross-cutting constraints:**

- All controllers must use shared JsonResponse utility (no manual JsonSerializer.Serialize)
- All logging must use ILogger (no Console.WriteLine)
- All question type JS classes must extend BaseQuestion
- Test projects must be under tests/{unit,integration}/
- All tests must pass with `dotnet test`

### Phase 4: Error Handling & Meaningful Error Messages

**Goal:** Add consistent, meaningful error handling across the entire application — backend middleware for unhandled exceptions, structured error responses from all controllers and services, and a frontend error display component that shows clear, user-friendly messages when something goes wrong.

**Depends on:** Phase 1 (completed)

**Requirements:** [REQ-14, REQ-15, REQ-16, REQ-17]

**Plans:** 2 plans

**Wave 1** *(independent, can run in parallel)*:

- [ ] 04-01-PLAN.md — Backend error handling (global exception middleware, structured error responses, meaningful error messages in all controllers)
- [ ] 04-02-PLAN.md — Frontend error display component (ErrorDisplay component, HTTP error handling in makeRequest.js, error state UI patterns)

**Cross-cutting constraints:**

- All controllers must return structured JSON error responses (consistent format: `{ error: { code, message, details? } }`)
- All services must throw meaningful exceptions (not return null silently)
- Global exception middleware catches unhandled exceptions and returns proper HTTP status codes
- Frontend must display user-friendly error messages for all HTTP failures
- Error messages must be specific and actionable (not generic "An error occurred")
- All error responses must include appropriate HTTP status codes (400, 401, 403, 404, 500)

### Phase 5: Glassmorphism & UI Polish

**Goal:** Add glassmorphism visual effects to frontend elements for a modern, frosted-glass aesthetic. Fix session-aware button visibility: show logout only when authenticated, hide sign-in when authenticated (and vice versa).

**Depends on:** Phase 1, Phase 2

**Requirements:** [REQ-18, REQ-19]

**Plans:** 2 plans

**Wave 1** *(independent, can run in parallel)*:

- [ ] 05-01-PLAN.md — Glassmorphism effects (backdrop-blur, semi-transparent backgrounds, glass cards, frosted sidebar)
- [ ] 05-02-PLAN.md — Session-aware button visibility (conditional logout/sign-in rendering in layout, session check in views)

**Cross-cutting constraints:**

- Glassmorphism uses `backdrop-blur-md bg-white/70 border border-white/20` patterns
- Must work on both light and semi-dark backgrounds
- Button visibility checks `TempData["username"]` or session state
- Logout button only visible when user is authenticated
- Sign-in/Sign-up buttons hidden when user is authenticated
- All changes must maintain existing responsive behavior

### Phase 7: Security Hardening — Session Auth, IDOR Prevention & CSRF

**Goal:** Close the session-gating and IDOR gaps identified in SECURITY-STRATEGY.md: add session validation to unprotected endpoints, enforce private-template access control, filter `IsPublic` in search queries, prevent user enumeration, and add anti-CSRF validation on state-changing POST endpoints.

**Depends on:** Phase 3, Phase 4 (needs refactored controllers + error middleware)

**Requirements:** [REQ-20]

**Plans:** 2 plans

**Wave 1** *(independent — session gates + IDOR fixes)*:

- [x] 07-01-PLAN.md — Session-gating + IDOR prevention (add Auth.ValidateSession to 5 unprotected endpoints, IsPublic filtering, ownership checks)

**Wave 2** *(blocked on Wave 1 — CSRF + hardening)*:

- [x] 07-02-PLAN.md — Anti-CSRF token enforcement + response security hardening

**Cross-cutting constraints:**
- All authenticated endpoints must call `Auth.ValidateSession(HttpContext)` and return 401 on failure
- Private template access must check ownership/admin via `AdminService.IsUserAdmin()`
- `GetTemplatesByQuery` must filter by `IsPublic` or require auth
- Anti-CSRF tokens required on all POST/PUT/DELETE endpoints
- `SameSite=Strict` for session cookie in production
- All changes must pass existing unit + integration tests
