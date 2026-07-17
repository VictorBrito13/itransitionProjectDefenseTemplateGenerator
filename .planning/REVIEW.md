---
phase: full-codebase-audit
reviewed: 2026-07-17T00:00:00Z
depth: deep
files_reviewed: 52
files_reviewed_list:
  - Controllers/HomeController.cs
  - Controllers/Question.cs
  - Controllers/ResponseController.cs
  - Controllers/Templates.cs
  - Controllers/UserController.cs
  - Services/User/User.cs
  - Services/User/IUserService.cs
  - Services/Template/Template.cs
  - Services/Template/ITemplate.cs
  - Services/Admin/Admin.cs
  - Services/Admin/IAdmin.cs
  - Services/Question/Question.cs
  - Services/Question/IQuestion.cs
  - Services/QuestionOption/QuestionOption.cs
  - Services/QuestionOption/IQuestionOption.cs
  - Services/Response/Response.cs
  - Services/Response/IResponse.cs
  - Services/Topic/Topics.cs
  - Services/Topic/ITopic.cs
  - Models/User.cs
  - Models/Template.cs
  - Models/Question.cs
  - Models/QuestionOption.cs
  - Models/Response.cs
  - Models/Admin.cs
  - Models/Like.cs
  - Models/Topic.cs
  - Models/Comment.cs
  - Models/Tag.cs
  - Models/DBException.cs
  - Models/ServiceException.cs
  - Models/UserAllowedToAnswer.cs
  - Data/ApplicationDBContext.cs
  - Middleware/ExceptionHandlingMiddleware.cs
  - Utils/Auth.cs
  - Utils/HashText.cs
  - Utils/JsonResponse.cs
  - Utils/Session.cs
  - Program.cs
  - Views/Shared/_Layout.cshtml
  - Views/Home/Index.cshtml
  - Views/User/LogInView.cshtml
  - Views/User/SignUpView.cshtml
  - Views/Template/CreateTemplate.cshtml
  - Views/Template/TemplateView.cshtml
  - wwwroot/js/utils/http/makeRequest.js
  - wwwroot/js/createTemplate/createTemplate.js
  - wwwroot/js/templateView/templateView.js
  - wwwroot/js/index/index.js
  - wwwroot/js/signUp/signUp.js
  - wwwroot/js/utils/errorDisplay.js
  - wwwroot/js/utils/buildForm.js
  - wwwroot/js/utils/templates/printTemplates.js
  - wwwroot/js/utils/createTemplate/deleteTemplate.js
  - wwwroot/UI/components/btnLogOut.js
findings:
  critical: 14
  warning: 12
  info: 6
  total: 32
status: issues_found
---

# Full Codebase Audit — Code Review Report

**Reviewed:** 2026-07-17
**Depth:** deep (cross-file analysis)
**Files Reviewed:** 52 source files (Controllers, Services, Models, Views, JS, Middleware, Utils)
**Status:** issues_found

## Summary

This is a full adversarial audit of the ItransitionTemplates ASP.NET Core MVC application. The codebase has **14 critical/blocker issues** spanning security vulnerabilities (XSS, authorization bypasses, insecure password storage), correctness bugs (null reference crashes, dead code paths, broken property-name contracts between frontend and backend), and **12 warnings** covering fragile patterns, missing error handling, and logic errors. The application is **not safe to deploy** in its current state — multiple authorization bypasses allow any authenticated user to delete/update any template, and stored XSS via template titles can compromise any user who views affected pages.

---

## Critical Issues

### CR-01: Stored XSS via template title/description in search results

**File:** `wwwroot/js/index/index.js:94-106`
**Issue:** Template `Title`, topic `Name`, and creator `Username` are interpolated directly into `innerHTML` without HTML escaping. An attacker who creates a template with `<img src=x onerror=steal()>` as the title will execute arbitrary JavaScript in every user's browser who sees the search result or template card.
**Fix:**
```javascript
function escapeHtml(str) {
    const div = document.createElement('div');
    div.textContent = str;
    return div.innerHTML;
}
// Then use:
<h3>${escapeHtml(template.Title)}</h3>
<p>${escapeHtml(topicName)} &middot; by ${escapeHtml(creatorName)}</p>
```

### CR-02: Stored XSS via template title/description in template cards

**File:** `wwwroot/js/utils/templates/printTemplates.js:55-56`
**Issue:** Same as CR-01 — `template.Title` and `template.Description` are injected via `innerHTML` in `createTemplateCard()` without escaping. Every template listing page is vulnerable.
**Fix:** Use the same `escapeHtml()` utility from CR-01 for all user-controlled text inserted via innerHTML.

### CR-03: XSS in error display container

**File:** `wwwroot/js/utils/errorDisplay.js:33-40`
**Issue:** `showErrorInContainer()` inserts the `message` parameter directly into `innerHTML`. If any server error message contains user-controlled content (e.g., duplicate entry values from the DB), it becomes an XSS vector.
**Fix:**
```javascript
function showErrorInContainer(message, container) {
  const el = typeof container === 'string' ? document.getElementById(container) : container;
  if (!el) return;
  el.textContent = message; // Use textContent instead of innerHTML
  // ... or escape the message before insertion
}
```

### CR-04: Authorization bypass — any user can like/unlike as any other user

**File:** `Controllers/Templates.cs:120`
**Issue:** The `LikeAction` endpoint takes `userId` as a `[FromQuery]` parameter from the client. A malicious user can pass any `userId` to perform like/unlike actions on behalf of another user. The session user is validated but never used for the actual action.
**Fix:**
```csharp
// Remove userId from query params; use session user instead:
public async Task<IActionResult> LikeAction([FromQuery] ulong templateId, [FromQuery] string action) {
    Models.User? userSession = Auth.ValidateSession(HttpContext);
    if(userSession == null) return JsonResponse.Error("Please sign in", 401);
    
    Like[] actionCompleted = await _TemplateService.LikeAction(userSession.UserId, templateId, action);
    // ...
}
```

### CR-05: Authorization bypass — any authenticated user can delete any template

**File:** `Controllers/Templates.cs:152-170`
**Issue:** `DeleteTemplate` validates that the user has a session but never checks whether the user is an admin/owner of the template. Any logged-in user can delete any template by passing its ID.
**Fix:** Add an ownership check before deletion:
```csharp
bool isAdmin = await _AdminService.IsUserAdmin(userSession.UserId, templateId);
if (!isAdmin) return JsonResponse.Error("You are not authorized to delete this template", 403);
```

### CR-06: Authorization bypass — any authenticated user can update any template

**File:** `Controllers/Templates.cs:94-115`
**Issue:** Same pattern as CR-05. `UpdateTemplate` validates session but never verifies the user is an admin of the target template.
**Fix:** Add the same admin/ownership check as CR-05.

### CR-07: No authentication on response submission endpoint

**File:** `Controllers/ResponseController.cs:17-25`
**Issue:** The `/response/add` endpoint has no authentication check. Any anonymous user (or bot) can submit responses. The `UserId` in the response body is taken from the client, so responses can be forged as any user.
**Fix:**
```csharp
[HttpPost("/response/add")]
public async Task<ActionResult> SaveResponses([FromBody] Models.Response[] responses) {
    Models.User? userSession = Auth.ValidateSession(HttpContext);
    if (userSession == null) return JsonResponse.Error("Please sign in", 401);
    
    // Force UserId from session, not from client input
    foreach (var r in responses) r.UserId = userSession.UserId;
    // ...
}
```

### CR-08: No authentication on question creation endpoint

**File:** `Controllers/Question.cs:23-42`
**Issue:** `/question/add` has no authentication or authorization check. Anyone can add questions to the database.
**Fix:** Add session validation before processing.

### CR-09: NullReferenceException — TemplateController.GetTemplateView

**File:** `Controllers/Templates.cs:74-78`
**Issue:** `Auth.ValidateUserSession()` returns `null` after redirecting when the session is invalid. Line 76 then accesses `user.Email` on the null reference, crashing with `NullReferenceException`. The redirect on line 11 of `Auth.cs` does NOT stop execution — it just sets a response header.
**Fix:**
```csharp
Models.User? user = Auth.ValidateUserSession(HttpContext);
if (user == null) return Redirect("/user/log-in");
TempData["userEmail"] = user.Email;
```

### CR-10: NullReferenceException — CreateTemplate accesses template before null check

**File:** `Controllers/Templates.cs:29-43`
**Issue:** Line 30 accesses `template.TopicId` before the null check on line 41. If model binding fails and `template` is null, this throws `NullReferenceException`. Additionally, the null check on line 41 is unreachable for a `[FromBody]` parameter — ASP.NET returns 400 before the action runs if the body can't be deserialized.
**Fix:** Remove the dead null check on line 41. Add `[Required]` attribute or explicit validation:
```csharp
public async Task<ActionResult<Models.Template>> CreateTemplate([FromBody] Models.Template template) {
    if (template == null) return JsonResponse.Error("Template data is missing");
    if (template.TopicId <= 0) { /* ... */ }
    // ...
}
```

### CR-11: Insecure password hashing — SHA256 without salt

**File:** `Utils/HashText.cs:7-9`, `Services/User/User.cs:20,31`
**Issue:** Passwords are hashed with bare SHA256 (no salt, no key stretching). This is vulnerable to rainbow table attacks and brute-force. SHA256 is designed for speed, making it trivially crackable with modern GPUs.
**Fix:** Use ASP.NET Core's built-in `PasswordHasher<T>` or at minimum PBKDF2/Argon2 with per-user salts:
```csharp
var hasher = new PasswordHasher<User>();
string hashedPassword = hasher.HashPassword(user, user.Password);
// To verify:
var result = hasher.VerifyHashedPassword(existingUser, existingUser.Password, providedPassword);
```

### CR-12: Sensitive data logging enabled unconditionally

**File:** `Program.cs:22`
**Issue:** `EnableSensitiveDataLogging()` is enabled without an environment check. In production, this logs parameter values (including hashed passwords from queries) to the configured log output.
**Fix:**
```csharp
builder.Services.AddDbContext<ApplicationDBContext>(options => {
    var useSensitiveLogging = builder.Environment.IsDevelopment();
    options.UseMySql(dbConnection, serverVersion, 
        sqlOptions => { if (useSensitiveLogging) sqlOptions.EnableSensitiveDataLogging(); });
});
```

### CR-13: Session data written to console in production

**File:** `Views/Shared/_Layout.cshtml:81`
**Issue:** `@{Console.WriteLine("User: " + Context.Session.GetString("userSession"));}` writes the full session JSON (including user ID, email, username) to stdout on every page load. This is an information disclosure vulnerability and a performance issue.
**Fix:** Remove this line entirely. It serves no purpose in production.

### CR-14: TempData key case mismatch — DB errors never displayed during signup

**File:** `Controllers/UserController.cs:48` vs `Views/User/SignUpView.cshtml:20`
**Issue:** The controller sets `TempData["ErrorMsg"]` (capital E) but the view reads `TempData["errorMsg"]` (lowercase e). TempData keys are case-sensitive. DB error messages (duplicate entry, null value) during signup are silently lost and never shown to the user.
**Fix:**
```csharp
// In UserController.cs line 48, change to:
TempData["errorMsg"] = err.Msg;
```

---

## Warnings

### WR-01: NullReferenceException risk in Question controller

**File:** `Controllers/Question.cs:25`
**Issue:** `questionAndOptions.questions.Length` will throw `NullReferenceException` if `questions` array is null (even though the outer object is non-null). The null check only guards the outer object.
**Fix:**
```csharp
if (questionAndOptions == null || questionAndOptions.questions == null || questionAndOptions.questions.Length == 0)
```

### WR-02: FirstAsync() throws instead of returning null — dead null check

**File:** `Services/User/User.cs:58-61`
**Issue:** `FirstAsync()` throws `InvalidOperationException` when no user is found. The null check on line 61 (`if(user == null) return user;`) is dead code. The catch block on line 64 catches this but logs it as an error and returns null, masking what is actually a "not found" condition.
**Fix:** Use `FirstOrDefaultAsync()` instead:
```csharp
Models.User? user = await _context.Users.FromSqlRaw(...).FirstOrDefaultAsync();
if (user == null) return null;
return user;
```

### WR-03: Non-atomic template + admin creation — orphaned data risk

**File:** `Controllers/Templates.cs:45-53`
**Issue:** Template is saved first (line 45), then Admin (line 49). If admin creation fails (DB error, constraint violation), the template exists in the DB without any admin — an orphaned, unmanageable record. No transaction wraps both operations.
**Fix:** Wrap in a transaction or use a service method that handles both atomically:
```csharp
using var transaction = await _context.Database.BeginTransactionAsync();
try {
    var saved = await _TemplateService.AddTemplate(template);
    admin.TemplateId = saved.TemplateId;
    await _AdminService.AddAdmin(admin);
    await transaction.CommitAsync();
} catch { await transaction.RollbackAsync(); throw; }
```

### WR-04: Like action — unhandled duplicate key exception

**File:** `Services/Template/Template.cs:85-104`
**Issue:** When `action == "like"`, the code adds a new `Like` without checking if one already exists. The composite key `(TemplateId, UserId)` will cause a `DbUpdateException` on duplicate, which is not caught. This bubbles up as a 500 error to the client.
**Fix:** Check for existing like before adding:
```csharp
if (action == "like") {
    var existing = await _context.Likes.FindAsync(userId, templateId);
    if (existing != null) return await _context.Likes.Where(l => l.TemplateId == templateId).ToArrayAsync();
    await _context.Likes.AddAsync(like);
}
```

### WR-05: UpdateTemplate replaces entire navigation collections — EF tracking conflicts

**File:** `Services/Template/Template.cs:72-76`
**Issue:** Assigning `found.Admins = template.Admins` replaces the tracked collection with detached entities from the request body. This causes EF Core tracking exceptions ("cannot be tracked because another instance with the same key value"). The controller tries to detect this via string matching on exception messages (line 108) — extremely fragile.
**Fix:** Update child entities explicitly rather than replacing collections, or use `AsNoTracking()` for the read and explicit attach/update patterns.

### WR-06: Synchronous SaveChanges in async method

**File:** `Services/Response/Response.cs:16`
**Issue:** `_context.SaveChanges()` is called in an `async Task` method, blocking the thread. Should be `await _context.SaveChangesAsync()`.
**Fix:** Replace with `int n = await _context.SaveChangesAsync();`

### WR-07: Frontend checks wrong property for HTTP status — auth redirect never fires

**File:** `wwwroot/js/createTemplate/createTemplate.js:238,260`
**Issue:** The code checks `templateUpdatedJSON.status === 401` but the server returns `{ error: { code: 401, ... } }`. The `status` property doesn't exist on the response. The 401 redirect to login never fires.
**Fix:**
```javascript
if (templateUpdatedJSON.error?.code === 401) {
    location.assign(`${location.origin}/user/log-in`);
}
```

### WR-08: deleteTemplate.js checks wrong property and uses wrong error format

**File:** `wwwroot/js/utils/createTemplate/deleteTemplate.js:13,21`
**Issue:** Line 13 checks `deleteJSON.status === 401` (wrong property, same as WR-07). Line 21 reads `deleteJSON.errorMsg` which is the old error format — the new format is `deleteJSON.error.message`. Error messages after failed deletion are never displayed.
**Fix:**
```javascript
if (deleteJSON.error?.code === 401) { location.assign(...); }
// ...
$serverMsgs.innerHTML = `<p ...>${deleteJSON.error?.message || 'Delete failed'}</p>`;
```

### WR-09: createTemplate.js reads templateId from wrong path

**File:** `wwwroot/js/createTemplate/createTemplate.js:272`
**Issue:** After successful template creation, the code reads `templateSavedJSON.templateId` but the server wraps data: `{ data: { TemplateId: ... } }`. The link to edit the new template will have `templateId=undefined`.
**Fix:**
```javascript
href="${location.origin}/template/create?templateId=${templateSavedJSON.data.TemplateId}"
```

### WR-10: buildForm.js checks wrong error property

**File:** `wwwroot/js/utils/buildForm.js:22`
**Issue:** `if(json.errorMsg)` uses the old error format. The server now returns `{ error: { message } }`. The "form does not exist" fallback never triggers.
**Fix:**
```javascript
if (json.error) {
```

### WR-11: _Layout.cshtml Substring(0,1) on JSON session returns wrong character

**File:** `Views/Shared/_Layout.cshtml:89-90`
**Issue:** The session stores a JSON string like `{"UserId":1,"Username":"john","Email":"..."}`. `Substring(0, 1)` returns `{` — not the user's initial. The avatar always shows `{` instead of the user's first letter.
**Fix:** Deserialize the session to extract the username:
```csharp
@{
    var sessionJson = Context.Session.GetString("userSession");
    var sessionUser = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(sessionJson);
    var displayName = sessionUser.GetProperty("Username").GetString();
}
<div class="...">@displayName?[0].ToString().ToUpper()</div>
```

### WR-12: getUserByEmail in createTemplate.js accesses response incorrectly

**File:** `wwwroot/js/createTemplate/createTemplate.js:148,156`
**Issue:** Line 148 uses raw `fetch` instead of `makeRequest`, bypassing error handling. Line 156 reads `json.user.Email` but the server returns `{ data: { UserId, Username, Email, ... } }` — the property is `data`, not `user`. This will throw `TypeError: Cannot read properties of undefined`.
**Fix:**
```javascript
const json = await makeRequest(`user/get-by-username?username=${username}`);
if (json.error) { showError(json.error.message); }
else {
    $btn.textContent = json.data.Email;
    // ...
}
```

---

## Info

### IN-01: Debug console.log statements left in production code

**Files:** `createTemplate.js:36,80,96,103,223,236,264`, `templateView.js:29,116`
**Issue:** Multiple `console.log()` statements remain in production JavaScript. These leak internal data structures to the browser console.
**Fix:** Remove all debug `console.log()` calls or gate behind a dev-only flag.

### IN-02: contentEditable no-op statement

**File:** `wwwroot/js/createTemplate/Multiple-options-question.js:65`
**Issue:** `$h4.contentEditable;` reads the property but doesn't assign to it. This is a no-op — likely intended to be `$h4.contentEditable = "true"`.
**Fix:** Set the property: `$h4.contentEditable = "true";`

### IN-03: Unused import in IUserService.cs

**File:** `Services/User/IUserService.cs:1`
**Issue:** `using System.Runtime.CompilerServices;` is imported but never used.
**Fix:** Remove the unused import.

### IN-04: Unused import in Admin.cs model

**File:** `Models/Admin.cs:1`
**Issue:** `using Microsoft.EntityFrameworkCore.Metadata.Internal;` is imported but never used. This references an internal EF namespace.
**Fix:** Remove the unused import.

### IN-05: Typo in variable name — `$tempalteDescription`

**File:** `wwwroot/js/createTemplate/createTemplate.js:31`
**Issue:** Variable is named `$tempalteDescription` (transposition of 'l' and 'a'). Should be `$templateDescription`.
**Fix:** Rename to `$templateDescription`.

### IN-06: Typo in HTML element ID — `search-result-contianer`

**File:** `Views/Home/Index.cshtml:64`
**Issue:** Element ID is `search-result-contianer` (transposition of 'i' and 'a'). Referenced in `index.js:11` with the same typo, so it works, but it's a latent bug if either side is fixed independently.
**Fix:** Correct to `search-result-container` in both files.

---

## Cross-Cutting Observations

### Missing Anti-Forgery Protection
No POST/PUT/DELETE endpoints use `[ValidateAntiForgeryToken]`. Forms submit without anti-forgery tokens. This enables CSRF attacks.

### No Rate Limiting on Authentication Endpoints
`/user/log-in` has no rate limiting, making it vulnerable to brute-force credential stuffing.

### Session deserialization returns empty object instead of null
`Session.GetObject<T>` deserializes `"{}"` when the session key is missing, creating an object with default/null properties rather than returning null. All auth validation depends on checking individual properties for null/zero — fragile and error-prone.

---

_Reviewed: 2026-07-17_
_Reviewer: the agent (gsd-code-reviewer)_
_Depth: deep_
