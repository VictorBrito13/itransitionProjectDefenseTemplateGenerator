# Phase 6: E2E Testing with Cypress - Context

**Gathered:** 2026-07-17
**Status:** Ready for planning
**Source:** User request

<domain>
## Phase Boundary

Implement comprehensive end-to-end testing using Cypress for the entire ItransitionTemplates application. Tests must cover the complete user journey from sign up to liking templates.

Key user flows to test:
1. User registration (sign up)
2. User authentication (login/logout)
3. Template creation
4. Template viewing
5. Template search
6. Template liking/unliking
7. Template responses
8. Session management (authenticated vs unauthenticated states)

</domain>

<decisions>
## Implementation Decisions

### Testing Framework
- Use Cypress for E2E testing (user decision - locked)
- Cypress should be configured for ASP.NET Core MVC application
- Tests must run against the running application

### Test Organization
- Avoid duplicated code across tests
- Use Cypress custom commands to encapsulate repeated logic (especially sign-in)
- Follow Cypress best practices for test structure

### Test Coverage
- Sign up flow (valid/invalid inputs)
- Login flow (valid/invalid credentials)
- Logout flow
- Template CRUD operations
- Like/unlike functionality
- Search functionality
- Session-aware UI elements (logout button visibility, sign-in button visibility)

### the agent's Discretion
- Cypress configuration details (base URL, viewport, etc.)
- Test file organization structure
- Fixture data strategy
- CI/CD integration approach

</decisions>

<canonical_refs>
## Canonical References

**Downstream agents MUST read these before planning or implementing.**

### Application Structure
- `Controllers/UserController.cs` — User authentication endpoints (login, signup, logout)
- `Controllers/Templates.cs` — Template CRUD and like endpoints
- `Views/User/LogInView.cshtml` — Login form
- `Views/User/SignUpView.cshtml` — Signup form
- `Views/Home/Index.cshtml` — Home page with template listing
- `Views/Template/CreateTemplate.cshtml` — Template creation form
- `Program.cs` — Application configuration and middleware

### Existing Tests
- `tests/` — Existing unit and integration test projects (Phase 3)

</canonical_refs>

<specifics>
## Specific Ideas

- User explicitly requested Cypress custom commands to encapsulate sign-in logic
- Tests should avoid code duplication
- Must test "all the features from sign up to give likes to the templates"
- Sign-in should be reusable across test files via Cypress commands

</specifics>

<deferred>
## Deferred Ideas

None — user request is clear and comprehensive

</deferred>

---

*Phase: 6 - E2E Testing with Cypress*
*Context gathered: 2026-07-17*
