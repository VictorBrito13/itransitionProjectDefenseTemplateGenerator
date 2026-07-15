---
phase: 04-error-handling
plan: 01
subsystem: backend
tags: [error-handling, middleware, exception, api]
dependency_graph:
  requires: []
  provides: [ExceptionHandlingMiddleware, ServiceException, JsonResponse]
  affects: [Controllers, Services]
tech_stack:
  added: []
  patterns: [global-exception-middleware, structured-error-response, service-error-propagation]
key_files:
  created:
    - Middleware/ExceptionHandlingMiddleware.cs
    - Models/ServiceException.cs
  modified:
    - Utils/JsonResponse.cs
    - Program.cs
    - Controllers/Templates.cs
    - Controllers/UserController.cs
    - Controllers/ResponseController.cs
    - Controllers/Question.cs
    - Services/Template/ITemplate.cs
    - Services/Template/Template.cs
    - Services/Response/IResponse.cs
    - Services/Response/Response.cs
decisions:
  - "Global exception middleware registered before routing pipeline"
  - "ServiceException carries error code enum and maps to HTTP status codes"
  - "JsonResponse.Error() now returns structured format { error: { code, message, details } }"
  - "Services throw exceptions instead of returning int status codes"
  - "Controller catch blocks convert ServiceException to structured JSON responses"
metrics:
  duration: 549s
  completed: 2026-07-15T14:53:49Z
  tasks_completed: 3
  files_created: 2
  files_modified: 11
---

# Phase 4 Plan 01: Error Handling Infrastructure Summary

**One-liner:** Global exception middleware, structured error responses, and meaningful error messages across all controllers and services.

## What Was Built

### Task 1: Error Handling Infrastructure
- Created `Models/ServiceException.cs` with `ServiceErrorCode` enum (NotFound, Validation, Unauthorized, Forbidden, Conflict, Database, Unknown) and automatic HTTP status code mapping
- Created `Middleware/ExceptionHandlingMiddleware.cs` for global exception handling — catches ServiceException and generic Exception, returns structured JSON `{ error: { code, message, details } }`
- Updated `Utils/JsonResponse.cs` with structured error format: `Error()` method now returns `{ error: { code, message, details } }`, added overload with details parameter, enhanced `NotFound()` with structured format
- Registered middleware in `Program.cs` before routing pipeline

### Task 2: Controller Error Messages
- Updated all 4 controllers with specific, actionable error messages
- Fixed typos: "tamplate" → "template", "ocurred" → "occurred", "answers" → "responses"
- All 401 responses use "Please sign in to ..." prefix
- All error messages provide user guidance ("please try again", "check your input")
- Templates.cs: 13 error messages updated
- UserController.cs: 2 error messages updated
- ResponseController.cs: 1 error message updated
- Question.cs: 3 error messages updated

### Task 3: Service Error Propagation
- Updated `Services/Template/Template.cs`: `UpdateTemplate` and `DeleteTemplate` now throw `ServiceException` instead of returning int codes
- Updated `Services/Response/Response.cs`: `AddResponses` now throws `ServiceException` instead of returning int codes
- Updated interfaces (`ITemplate.cs`, `IResponse.cs`) to reflect new signatures
- Updated controllers to catch `ServiceException` and return structured error responses
- Updated 7 unit tests to assert `ServiceException` with correct error codes

## Deviations from Plan

### Auto-fixed Issues

**1. [Rule 2 - Missing Critical Functionality] Updated Response service for consistency**
- **Found during:** Task 3
- **Issue:** Response service returned int codes while Template service was updated to throw ServiceException
- **Fix:** Updated `Services/Response/Response.cs` and `IResponse.cs` to throw ServiceException, updated `ResponseController.cs` to catch exceptions
- **Files modified:** Services/Response/IResponse.cs, Services/Response/Response.cs, Controllers/ResponseController.cs
- **Commit:** 898dbea

**2. [Rule 1 - Bug] Fixed test compilation errors from interface changes**
- **Found during:** Task 3
- **Issue:** Unit tests were written for old interface (int return types) and failed to compile
- **Fix:** Updated TemplateServiceTests.cs, ResponseServiceTests.cs, and JsonResponseTests.cs to match new interface and response format
- **Files modified:** tests/ItransitionTemplates.Tests.Unit/Services/TemplateServiceTests.cs, tests/ItransitionTemplates.Tests.Unit/Services/ResponseServiceTests.cs, tests/ItransitionTemplates.Tests.Unit/Utils/JsonResponseTests.cs
- **Commit:** 898dbea

## Threat Flags

| Flag | File | Description |
|------|------|-------------|
| T-04-01 | Middleware/ExceptionHandlingMiddleware.cs | Unhandled exceptions logged server-side, sanitized message returned to client (mitigated) |
| T-04-03 | Middleware/ExceptionHandlingMiddleware.cs | Exception logging at appropriate levels (Warning for ServiceException, Error for unhandled) |

## Self-Check: PASSED

- [x] Middleware/ExceptionHandlingMiddleware.cs exists and contains `class ExceptionHandlingMiddleware`
- [x] Models/ServiceException.cs exists with `ServiceErrorCode` enum and `class ServiceException : Exception`
- [x] Utils/JsonResponse.cs has structured error format `{ error: { code, message, details } }`
- [x] Program.cs contains `app.UseMiddleware<ExceptionHandlingMiddleware>()`
- [x] All 4 controllers updated with specific error messages
- [x] Services throw ServiceException instead of returning int codes
- [x] All 39 unit tests passing
- [x] Build succeeds with 0 errors
