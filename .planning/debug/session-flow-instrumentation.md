---
status: instrumentation-added
trigger: "add debugging instrumentation to check what is going on when the login → create template flow is executed"
created: 2026-07-17
updated: 2026-07-17
---

# Debug Session: session-flow-instrumentation

## Symptoms

- User logs in successfully and sees their username on the home page
- When navigating to `/template/create`, user gets redirected to `/user/log-in`
- Session appears valid for GET `/` but invalid for GET `/template/create`
- Need to trace session state at each step to find root cause

## Current Focus

**Hypothesis:** Session cookie or session data is not persisting between requests, or `Auth.ValidateSession` is failing for a different reason than `Session.GetObject` in HomeController.

**Test:** Add logging to trace session state at login, home page, and create template page.

**Expecting:** Log output showing session key presence, JSON content, and deserialization result at each step.

**Next action:** Run the app, reproduce the flow (login → home → /template/create), and examine the DEBUG log lines to identify where session state diverges.

## Instrumentation Added

Diagnostic `_logger.LogInformation` calls added to:

1. **Controllers/UserController.cs** — `LogIn` and `SignUp` methods: log session key existence, raw JSON, and deserialized User properties immediately after `Session.Store`
2. **Controllers/HomeController.cs** — `Index` method: log session key existence, raw JSON, and deserialized User properties before `Session.GetObject` call
3. **Controllers/Templates.cs** — `CreateTemplateView` method: log session state BEFORE `Auth.ValidateSession`, then log the result AFTER including which specific validation condition failed (null user, null email, zero userId, null username)

All log lines are prefixed with `DEBUG [MethodName]` for easy grep.

## Evidence

- HomeController uses `Session.GetObject<Models.User>` directly → works (user sees username)
- CreateTemplateView uses `Auth.ValidateSession(HttpContext)` → returns null → redirects to login
- Both methods call the same `Session.GetObject<Models.User>` internally
- Session configured with `IsEssential = true`, 60min timeout, SameSite=Lax
- Tag helpers replaced with direct URLs to eliminate routing ambiguity
- `Session.Store` serializes anonymous type `new { UserId, Username, Email }` — `System.Text.Json` deserializes into `Models.User` which has additional required properties (Password, Likes, responses)
- `Auth.ValidateSession` and `Auth.ValidateUserSession` are identical implementations
- Build passes with 0 errors after instrumentation

## Eliminated

- Routing ambiguity (tag helpers replaced with direct URLs)
