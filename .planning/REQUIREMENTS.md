# Requirements — ItransitionTemplates Phase 1

## REQ-01: Tailwind CSS Migration
Replace Bootstrap 5 with Tailwind CSS across all views and layouts. Remove Bootstrap CDN dependency. Configure Tailwind for ASP.NET Core (libman or npm).

## REQ-02: Nielsen Heuristic Compliance
Redesign UI to comply with Nielsen's 10 usability heuristics:
1. Visibility of system status (loading states, progress indicators)
2. Match between system and real world (familiar terminology, icons)
3. User control and freedom (undo, back navigation, cancel actions)
4. Consistency and standards (uniform design patterns, button styles)
5. Error prevention (input validation, confirmation dialogs)
6. Recognition rather than recall (recent templates, suggestions)
7. Flexibility and efficiency of use (keyboard shortcuts, bulk actions)
8. Aesthetic and minimalist design (clear visual hierarchy, whitespace)
9. Help users recognize/recover from errors (inline validation, toast notifications)
10. Help and documentation (onboarding hints, tooltips where needed)

## REQ-03: Information Architecture
Implement clear information architecture:
- Logical page hierarchy and navigation
- Consistent placement of UI elements
- Clear content grouping and categorization
- Breadcrumb navigation where appropriate
- Search with filters and sorting

## REQ-04: User Journey Optimization
Design streamlined user journeys for core operations:
- Guest → Browse templates → Sign up → Create first template
- Authenticated user → Dashboard → Manage templates → Share
- Respondent → Receive link → Fill form → Submit → Confirmation

## REQ-05: Responsive Design
Ensure all redesigned views are fully responsive:
- Mobile-first approach with Tailwind breakpoints
- Touch-friendly targets (min 44x44px)
- Readable typography at all screen sizes
- Collapsible navigation on mobile

## REQ-06: Design Token System
Define and document a comprehensive design token system for cross-app visual consistency:
- Centralized color palette with semantic naming (bg, text, border variants)
- Typography scale with defined weights, sizes, and line heights
- Spacing and sizing scale (4px base unit)
- Shadow and elevation tokens
- Border radius tokens
- Animation timing and easing tokens
- Design token documentation in a single source of truth (CSS custom properties or JSON)
- Consumed by all views for consistent styling

## REQ-07: Toast Notification System
Implement a non-intrusive toast notification system using Sonner.js:
- Success, error, warning, and info toast variants
- Auto-dismiss with configurable duration
- Stacking behavior for multiple notifications
- Accessible announcements for screen readers
- Integration with existing form submission flows
- Replace all Bootstrap alert-based notifications

## REQ-08: Remaining Views Polish
Redesign remaining views and partials that haven't been migrated to Tailwind:
- Error.cshtml - Redesign with Tailwind, branded error page with helpful navigation
- _ValidationScriptsPartial.cshtml - Update if needed for Tailwind-compatible validation
- Any partials with Bootstrap remnants
- Consistent loading, empty, and error states across all views

## REQ-09: Visual Consistency Audit
Audit and fix visual inconsistencies across all migrated views:
- Ensure all views use the defined design tokens
- Fix any remaining Bootstrap class references in views and JS files
- Ensure consistent spacing, typography, and color usage
- Add micro-interactions (hover states, transitions, focus styles) where missing
- Verify responsive behavior on all viewport sizes

---

# Requirements — ItransitionTemplates Phase 3

## REQ-10: Backend Code Deduplication
Eliminate duplicated patterns across all controllers and services:
- Extract session validation into a shared helper (use or refactor Utils/Auth.cs)
- Create a JsonResponse utility to replace all manual JsonSerializer.Serialize(new { data/errorMsg }) patterns
- Consolidate the duplicated like/unlike branches in TemplateController.LikeAction
- Replace all Console.WriteLine calls with ILogger-based logging
- Remove unused using statements from service files

## REQ-11: Frontend Code Deduplication
Eliminate duplicated patterns across JavaScript files and Razor views:
- Create a BaseQuestion class that all 5 question type classes extend (SingleLine, Multiline, PositiveInteger, Checkbox, MultipleOptions)
- Extract showFieldError/clearFieldError into a shared validation utility (wwwroot/js/utils/validation.js)
- Create a reusable Razor partial (_ToastPartial.cshtml) for TempData-based toast notifications
- Replace 6 hardcoded skeleton cards in Index.cshtml with a Razor loop
- Deduplicate the toggle templates loading logic in index.js
- Fix Bootstrap class remnants in Checkbox-question.js

## REQ-12: Unused Code Removal
Identify and remove all unused code:
- Delete wwwroot/js/updateTemplate/updateTemplate.js (empty function, never imported)
- Delete wwwroot/js/site.js (only contains a comment, no code)
- Remove the site.js script reference from _Layout.cshtml
- Remove unused using statements (Microsoft.EntityFrameworkCore.Metadata.Internal, MySqlConnector in User service)

## REQ-13: Test Infrastructure & Test Coverage
Establish a comprehensive test suite:
- Create xUnit test project under tests/ (unit tests)
- Create integration test project under tests/ (integration tests)
- Write unit tests for all 7 service classes (at least 1 test per public method)
- Write unit tests for utility classes (HashText, JsonResponse)
- Write integration tests for all 5 controllers using WebApplicationFactory
- Use EF Core InMemory provider for test isolation
- All tests must pass with `dotnet test`

---

# Requirements — ItransitionTemplates Phase 7

## REQ-20: Security Hardening

Close session-gating gaps, prevent IDOR/BOLA attacks, and add CSRF protection:
- Session validation on all authenticated endpoints (return 401 on missing/invalid session)
- Private template access control (enforce ownership or admin role for non-public templates)
- `IsPublic` filtering in template queries (search, user-scoped listing)
- User enumeration prevention on `get-by-username` endpoint
- Anti-CSRF token validation on all state-changing POST/PUT/DELETE endpoints
- Secure session cookie config (`SameSite=Strict`, `SecurePolicy=Always`)
- Integration tests verifying 401 on unauthenticated access to protected endpoints
