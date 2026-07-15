# Phase 4: Error Handling & Meaningful Error Messages — Context

**Gathered:** 2026-07-15
**Status:** Ready for planning
**Source:** User requirements

<domain>
## Phase Boundary

This phase delivers consistent, meaningful error handling across the entire ItransitionTemplates application. It covers:

- Global exception handling middleware for unhandled backend exceptions
- Structured, consistent JSON error response format from all controllers
- Meaningful, user-friendly error messages (not generic "An error occurred")
- Frontend error display component for HTTP failures
- Service-level error propagation with meaningful exceptions

**Out of scope:** Visual redesign of error pages (covered in Phase 2), toast notification system (covered in Phase 2), form validation UX (covered in Phase 1).

</domain>

<decisions>
## Implementation Decisions

### D-20: Global Exception Handling Middleware
**Decision:** Implement a custom exception handling middleware that catches all unhandled exceptions and returns structured JSON error responses.
**Rationale:** Currently, unhandled exceptions either crash the request or return generic 500 errors. A middleware ensures consistent error handling across all endpoints without duplicating try-catch blocks in every controller.
**Implementation:** Create `Middleware/ExceptionHandlingMiddleware.cs` that catches exceptions, logs them with ILogger, and returns structured JSON responses with appropriate HTTP status codes.

### D-21: Structured Error Response Format
**Decision:** All error responses must follow a consistent JSON structure: `{ error: { code: number, message: string, details?: string } }`.
**Rationale:** The current `JsonResponse.Error()` returns `{ errorMsg, status }` which is inconsistent with the success format `{ data }`. A structured error format makes client-side error handling predictable and type-safe.
**Implementation:** Update `Utils/JsonResponse.cs` to support the new error format. Existing `ErrorMsg` property becomes `error.message`, `status` becomes `error.code`.

### D-22: Meaningful Error Messages
**Decision:** Replace all generic error messages with specific, actionable messages that tell the user what went wrong and what they can do about it.
**Rationale:** Messages like "An error has ocurred" or "This action could not be done" provide no value to the user. Specific messages like "Template not found — it may have been deleted" or "You don't have permission to edit this template" help users understand and resolve issues.
**Implementation:** Audit all `JsonResponse.Error()` calls in controllers and replace generic messages with specific ones. Update service layer to throw meaningful exceptions instead of returning null.

### D-23: Frontend Error Display Component
**Decision:** Create a reusable `ErrorDisplay` component that renders error messages in a consistent, accessible format across all pages.
**Rationale:** Currently, errors are displayed as inline HTML strings (e.g., `<div class="bg-red-50...">${errorMsg}</div>`). A reusable component ensures consistent error styling and behavior.
**Implementation:** Create `wwwroot/js/utils/errorDisplay.js` with a function that creates styled error elements. Integrate with `makeRequest.js` to automatically display errors on HTTP failures.

### D-24: Service Error Propagation
**Decision:** Services should throw meaningful exceptions (using custom `ServiceException` class) instead of returning null or generic error codes.
**Rationale:** The current pattern of returning null from services forces controllers to guess what went wrong. Throwing exceptions with specific error types allows controllers to return accurate error messages to users.
**Implementation:** Create `Models/ServiceException.cs` with error code enum. Update all service implementations to throw `ServiceException` instead of returning null on failures.

### OpenCode's Discretion
- Specific error message text for each controller action
- HTTP status code mapping for different error types
- Error logging format and detail level
- Component styling details (within existing Tailwind design system)

</decisions>

<canonical_refs>
## Canonical References

**Downstream agents MUST read these before planning or implementing.**

### Error Handling Files
- `Utils/JsonResponse.cs` — Current error response utility (to be enhanced)
- `Controllers/Templates.cs` — Example of current error handling patterns
- `Controllers/UserController.cs` — Example of current error handling patterns
- `Controllers/ResponseController.cs` — Example of current error handling patterns
- `Controllers/Question.cs` — Example of current error handling patterns
- `Models/DBException.cs` — Current custom exception (to be extended)
- `Services/User/User.cs` — Example of service error handling
- `Services/Template/Template.cs` — Example of service error handling
- `Program.cs` — App configuration (middleware registration)

### Frontend Error Handling
- `wwwroot/js/utils/http/makeRequest.js` — Current HTTP utility (to be enhanced)
- `wwwroot/js/utils/toast.js` — Toast notification utility (to integrate with errors)
- `wwwroot/js/createTemplate/createTemplate.js` — Example of client-side error handling
- `wwwroot/js/templateView/templateView.js` — Example of client-side error handling
- `Views/Shared/_ToastPartial.cshtml` — Server-side toast integration

</canonical_refs>

<specifics>
## Specific Ideas

- The current `JsonResponse.Error()` method is used inconsistently — some controllers use it, others return `Json()` directly
- Services like `Template.cs` return null on errors, making it impossible for controllers to know why something failed
- The `makeRequest.js` HTTP utility catches fetch errors but only logs to console — no user feedback
- Some controllers have try-catch blocks that catch `Exception` and return generic messages
- The `DBException` class exists but is only used in `UserController` — other services don't use it
- Error messages contain typos ("ocurred" → "occurred", "tamplate" → "template")
- The `_ToastPartial.cshtml` already converts TempData to toasts, but not all controllers use it

</specifics>

<deferred>
## Deferred Ideas

None — all items are within phase scope.

</deferred>

---

*Phase: 04-error-handling*
*Context gathered: 2026-07-15 via user requirements*
