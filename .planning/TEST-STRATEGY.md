# Test Strategy — ItransitionTemplates

**Created:** 2026-07-30 via `/gsd-testing-strategy`
**Basis:** no ADR (architecture derived from PROJECT.md + codebase: Transaction Script / CRUD-over-DB for all service subdomains). Extends existing `tests/` infrastructure — keeps all existing test rigor.

## Level emphasis per subdomain (shape follows architecture)

The shape is an *output* of the architecture, not a chosen target. Sociable tests by default; mock only at ports. Architecture rung per subdomain = **Transaction Script / CRUD-over-DB** — primary level is **medium (integration, real DB)**.

| Subdomain | Architecture rung | Primary level | Why |
|-----------|-------------------|---------------|-----|
| User service (Login, AddUser) | Transaction Script | medium (integration) + small for hash | Thin DB logic; password hashing earns pure unit tests |
| Template service (CRUD, search, like) | Transaction Script | medium (integration) | Thin DB orchestration; confidence at the DB boundary |
| Admin service | Transaction Script | medium (integration) | Simple DB queries |
| Question service | Transaction Script | medium (integration) | DB insert/add |
| QuestionOption service | Transaction Script | medium (integration) | DB insert/add |
| Response service | Transaction Script | medium (integration) | DB insert with exception path |
| Topic service | Transaction Script | medium (integration) | Simple DB read |
| HashText | Pure utility | **small (unit)** | PBKDF2 algorithm, salt, constant-time comparison |
| JsonResponse | Pure utility | **small (unit)** | Response shaping, pure functions |
| Exception middleware | Middleware | medium (integration, HTTP pipeline) | Test through WebApplicationFactory |
| Session/Auth utils | Thin wrapper | small (unit) | Simple serialization/validation wrappers |
| Controllers | MVC Controller | medium (integration) | Test through WebApplicationFactory with real pipeline |

**Resulting distribution:** integration-heavy (diamond-ish) — correct for a Transaction Script app where the behavior lives at the DB boundary.

## What to unit-test (the gnarly bits)

- **HashText** — PBKDF2 hashing with salt generation, constant-time comparison (`CryptographicOperations.FixedTimeEquals`), legacy SHA-256 verification (`VerifyOldSha256Hash`)
- **JsonResponse** — response object shaping with correct status codes and data/error property wrapping
- **LikeAction** (Template service) — idempotent like/unlike state machine (add like if not exists, remove if exists, handle double-like)
- **Auth.ValidateSession** — null/valid/incomplete session object handling

## What NOT to test / no duplicate coverage

- EF Core framework behavior, LINQ query translation, migration correctness
- ASP.NET Core pipeline internals (model binding, routing — tested via integration)
- Model property getters/setters (trivial)
- Razor view rendering details (covered by integration)
- SignalR hub behavior (not yet used)
- AutoMapper configuration (if added — test at your mapping seam only)
- **Each behavior tested once, at the cheapest level** — no duplicate unit + integration + e2e coverage of the same behavior

## Integration tests

- Against **real** dependencies where possible: currently using EF Core InMemory (acceptable for L1 but has semantic gaps). Priority gaps to migrate to TestContainers MySQL:
  - `FromSqlRaw` / full-text search (`MATCH...AGAINST`) — InMemory does not support
  - Transaction rollback behavior testing
- Sociable: services use real DbContext (not mocked). Only mock at architectural boundaries (external HTTP, file system — none exist currently).
- Controllers tested through `CustomWebApplicationFactory` with session auth flow.

## End-to-end

- **Persistent (CI smoke / critical journeys):** full Phase 6 Cypress suite — signup → login → logout, session-aware UI, create template → view → list, search templates, like/unlike, template update. Keep lean (<5 min). If full suite exceeds budget, run auth + template CRUD subset on every PR and defer search/like edge cases to nightly.
- **Transient (dev-loop, throwaway):** validate freshly-built flows during development. Not kept in CI. Demote to integration once covered cheaper.

## Coverage & mutation

- Coverage = **floor**, not a target. Currently no coverage tool configured — add `coverlet.collector` and set floor at 70%.
- Mutation testing (Stryker) on: `HashText`, `JsonResponse` — the pure utility modules where assertion quality matters. Target: ≥80% mutation score.

## CI execution map (feeds `/gsd-cicd-strategy`)

| Pipeline stage | Runs | Budget |
|---|---|---|
| PR gate (blocking) | `dotnet test` (all unit + integration) + Cypress smoke subset | ≤10 min |
| Merge to main | `dotnet test` (full) + coverage gate | — |
| Nightly / scheduled | Full Cypress suite + Stryker mutation on critical modules | — |

## TDD stance

- Behavior-level tests, **small uniform increments**, regression floor, real RED step.
- Test-first vs test-after: **off** (`workflow.tdd_mode = false`). Not mandated for CRUD-heavy Transaction Script code. Test-first recommended for new pure utility modules (HashText-level logic) and for security-critical fixes.
- Existing test pattern (seed → act → assert sociably with real InMemory DB) is correct and should be followed for all new service tests.

## Notes

- **Existing tests:** 7 unit test files (services) + 2 utility test files + 5 integration test controller files = ~68 tests. All pass with `dotnet test`.
- **InMemory → MySQL gap:** the `GetTemplatesByQuery` / `GetUserByUsername` / `GetTemplatesByUserId` services use MySQL-specific SQL (`FromSqlRaw` with `MATCH...AGAINST`) which cannot be tested with InMemory. These methods are only tested for error handling. **Recommended:** add TestContainers MySQL for a `Services/` integration test project that tests the real SQL.
- **Security tests (IDOR):** add integration tests verifying that unauthenticated requests to `/template/get-template`, `/template/template/user`, `/user/get-by-username`, `/template/get-by-query` return 401. These are the session-gating gaps identified in SECURITY-STRATEGY.md.
- **No property-based tests (fast-check) currently.** Not needed for L1 Transaction Script code. Add if state machine complexity increases (e.g., complex LikeAction logic, multi-step template workflows).
- **No Moq usage** despite being referenced in the unit test csproj. Current sociable pattern (real InMemory DB) is correct for this architecture — prefer this over mocking DbContext.
- **Cypress E2E tests are planned in Phase 6** with `data-cy` attributes already added across all views.

---
*Test strategy. Consumed by `/gsd-add-tests`, `/gsd-execute-phase`, `/gsd-plan-phase`.*
