---
phase: 03-refactoring
plan: 04
subsystem: testing
tags: [xunit, webapplicationfactory, integration-tests, inmemory-db, aspnetcore-testing]

# Dependency graph
requires:
  - phase: 03-refactoring/03-03
    provides: Unit test project and patterns
provides:
  - Integration test project with WebApplicationFactory
  - CustomWebApplicationFactory with InMemory DB and session support
  - 25 integration tests covering all 5 controllers
  - Authenticated client helper for session-based testing
affects: [future-phases, verification]

# Tech tracking
tech-stack:
  added: [Microsoft.AspNetCore.Mvc.Testing, Microsoft.EntityFrameworkCore.InMemory]
  patterns: [WebApplicationFactory, IClassFixture, InMemory DB per test class, session cookie auth]

key-files:
  created:
    - tests/ItransitionTemplates.Tests.Integration/ItransitionTemplates.Tests.Integration.csproj
    - tests/ItransitionTemplates.Tests.Integration/CustomWebApplicationFactory.cs
    - tests/ItransitionTemplates.Tests.Integration/Controllers/HomeControllerTests.cs
    - tests/ItransitionTemplates.Tests.Integration/Controllers/TemplateControllerTests.cs
    - tests/ItransitionTemplates.Tests.Integration/Controllers/UserControllerTests.cs
    - tests/ItransitionTemplates.Tests.Integration/Controllers/QuestionControllerTests.cs
    - tests/ItransitionTemplates.Tests.Integration/Controllers/ResponseControllerTests.cs
  modified:
    - Program.cs (added public partial class Program for WebApplicationFactory access)
    - ItransitionTemplates.sln (added integration test project)

key-decisions:
  - "Use separate JsonSerializerOptions for requests (no ReferenceHandler.Preserve) vs responses"
  - "MySQL-specific endpoints (MATCH...AGAINST) tested for graceful error handling with InMemory DB"
  - "AddDistributedMemoryCache added in test factory for session middleware support"
  - "Each test class uses IClassFixture for shared factory, InitializeDatabaseAsync for DB setup"

patterns-established:
  - "CustomWebApplicationFactory pattern: override ConfigureWebHost, replace DbContext with InMemory"
  - "CreateAuthenticatedClientAsync: seed user, POST login, return client with session cookie"
  - "SeedAsync helper: scoped DB access for test data setup"

requirements-completed: [REQ-13]

# Metrics
duration: 10min
completed: 2026-07-15
---

# Phase 03 Plan 04: Integration Tests Summary

**WebApplicationFactory integration test infrastructure with 25 passing tests across all 5 controllers using InMemory DB and session-based auth**

## Performance

- **Duration:** 10 min
- **Started:** 2026-07-15T12:52:47Z
- **Completed:** 2026-07-15T13:03:09Z
- **Tasks:** 2
- **Files modified:** 9

## Accomplishments
- Integration test project created with WebApplicationFactory and InMemory DB
- CustomWebApplicationFactory with distributed memory cache for session support and authenticated client helper
- 25 integration tests covering all 5 controllers (Home, Template, User, Question, Response)
- Both success and error paths tested, including authenticated and unauthenticated scenarios
- MySQL-specific endpoints (MATCH...AGAINST) tested for graceful error handling with InMemory DB

## Task Commits

Each task was committed atomically:

1. **Task 1: Create integration test project with CustomWebApplicationFactory** - `864bb2b` (feat)
2. **Task 2: Write integration tests for all controller endpoints** - `090efdd` (feat)

## Files Created/Modified
- `tests/ItransitionTemplates.Tests.Integration/ItransitionTemplates.Tests.Integration.csproj` - Integration test project with xUnit, Mvc.Testing, InMemory packages
- `tests/ItransitionTemplates.Tests.Integration/CustomWebApplicationFactory.cs` - WebApplicationFactory with InMemory DB, session support, auth helper
- `tests/ItransitionTemplates.Tests.Integration/Controllers/HomeControllerTests.cs` - 2 tests: Index and Error views
- `tests/ItransitionTemplates.Tests.Integration/Controllers/TemplateControllerTests.cs` - 12 tests: CRUD, likes, search, auth guards
- `tests/ItransitionTemplates.Tests.Integration/Controllers/UserControllerTests.cs` - 7 tests: login, signup, logout, user lookup
- `tests/ItransitionTemplates.Tests.Integration/Controllers/QuestionControllerTests.cs` - 2 tests: question creation with options
- `tests/ItransitionTemplates.Tests.Integration/Controllers/ResponseControllerTests.cs` - 2 tests: response saving
- `Program.cs` - Added `public partial class Program { }` for WebApplicationFactory access
- `ItransitionTemplates.sln` - Added integration test project to solution

## Decisions Made
- Used separate `JsonSerializerOptions` for request serialization (no `ReferenceHandler.Preserve`) since the server's model binder doesn't expect `$id`/`$ref` metadata in incoming payloads
- MySQL-specific endpoints (`GetUserByUsername`, `GetTemplatesByQuery`) tested for graceful error handling since InMemory DB doesn't support `FromSqlRaw` with `MATCH...AGAINST`
- Added `AddDistributedMemoryCache()` in the test factory because `AddSession()` requires an `IDistributedCache` implementation that wasn't registered in Program.cs
- Used `WebApplicationFactoryClientOptions.AllowAutoRedirect = false` for testing redirect responses (302)

## Deviations from Plan

### Auto-fixed Issues

**1. [Rule 1 - Bug] Fixed QuestionType enum serialization mismatch**
- **Found during:** Task 2 (QuestionControllerTests)
- **Issue:** Test sent enum as string ("singleLineString") but server expects integer (no JsonStringEnumConverter configured)
- **Fix:** Changed test payload to use integer enum values (0 for singleLineString, 1 for multipleLineText)
- **Files modified:** tests/ItransitionTemplates.Tests.Integration/Controllers/QuestionControllerTests.cs
- **Verification:** Test passes with integer enum values
- **Committed in:** 090efdd (Task 2 commit)

**2. [Rule 1 - Bug] Fixed login redirect assertion**
- **Found during:** Task 2 (UserControllerTests)
- **Issue:** `RedirectToAction("Index", "Home")` resolves to `/` via default route, not `/Home`
- **Fix:** Updated assertion to accept both `/` and `/Home` as valid redirect targets
- **Files modified:** tests/ItransitionTemplates.Tests.Integration/Controllers/UserControllerTests.cs
- **Verification:** Test passes with flexible location check
- **Committed in:** 090efdd (Task 2 commit)

**3. [Rule 1 - Bug] Fixed empty question options causing BadRequest**
- **Found during:** Task 2 (QuestionControllerTests)
- **Issue:** Sending empty `questionOptions` array caused `SaveChangesAsync` to return 0, service returned null, controller returned error
- **Fix:** Seeded a question first, then sent options referencing it so `SaveChangesAsync` returns >= 1
- **Files modified:** tests/ItransitionTemplates.Tests.Integration/Controllers/QuestionControllerTests.cs
- **Verification:** Test passes with seeded question and valid options
- **Committed in:** 090efdd (Task 2 commit)

**4. [Rule 3 - Blocking] Added missing using directives**
- **Found during:** Task 2 (build phase)
- **Issue:** `WebApplicationFactoryClientOptions` and `CreateScope` required additional using directives
- **Fix:** Added `using Microsoft.AspNetCore.Mvc.Testing` and `using Microsoft.Extensions.DependencyInjection`
- **Files modified:** UserControllerTests.cs, ResponseControllerTests.cs
- **Verification:** Build succeeds with 0 errors
- **Committed in:** 090efdd (Task 2 commit)

---

**Total deviations:** 4 auto-fixed (3 bug fixes, 1 blocking fix)
**Impact on plan:** All auto-fixes necessary for test correctness. No scope creep.

## Issues Encountered
- `ReferenceHandler.Preserve` in request serialization caused model binding issues — resolved by using separate serialization options for requests vs responses
- InMemory DB doesn't support `FromSqlRaw` — MySQL-specific endpoints tested for graceful error handling instead of happy path
- `AddDistributedMemoryCache()` not registered in Program.cs — added in test factory's `ConfigureServices`

## User Setup Required
None - no external service configuration required.

## Next Phase Readiness
- Full test infrastructure in place: unit tests (Plan 03) + integration tests (Plan 04)
- All 5 controllers covered with both unit and integration tests
- Combined test suite provides confidence for future refactoring and feature development

---
*Phase: 03-refactoring*
*Completed: 2026-07-15*

## Self-Check: PASSED
- All 8 key files verified on disk
- Both commits (864bb2b, 090efdd) found in git log
- 25/25 integration tests passing
