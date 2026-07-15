# Phase 2: Design Token System & Visual Polish — Context

**Gathered:** 2026-07-14
**Status:** Ready for planning
**Source:** Codebase audit after Phase 1 completion

<domain>
## Phase Boundary

This phase completes the UI redesign by establishing a formal design token system and polishing remaining views and code. It covers:

- Creation of a comprehensive design token system (CSS custom properties, token documentation)
- Implementation of Sonner.js toast notifications (D-05 gap from Phase 1)
- Redesign of Error.cshtml with Tailwind
- Visual consistency audit: Fix Bootstrap class remnants in JS files
- Cleanup of unused Bootstrap library files
- Consistent micro-interactions and transitions across all views

**Out of scope:** Backend changes, database schema, API modifications, new features, authentication logic changes, structural view changes (layout already finalized in Phase 1).

</domain>

<decisions>
## Implementation Decisions

### D-11: Design Token System — CSS Custom Properties + JSON Docs
**Decision:** Create a comprehensive design token system using CSS custom properties (in tailwind.css) and a JSON documentation file.
**Rationale:** Currently, design values (colors, spacing, shadows) are scattered between tailwind.config.js and individual views. A centralized token system ensures visual consistency and makes future theming possible.
**Implementation:**
- Define tokens as CSS custom properties in `wwwroot/css/tailwind.css`
- Create a design token documentation file as reference
- Extend Tailwind config to reference the custom properties
- Tokens to include: color palette (with semantic variants), typography scale, spacing scale, shadow/elevation tokens, border radius tokens, animation timing/easing tokens

### D-12: Sonner.js Toast Implementation
**Decision:** Implement Sonner.js toast notifications across all views (previously decided in D-05 but never implemented).
**Rationale:** Phase 1 deferred this. Bootstrap alert-based notifications still exist in some JS files. Sonner.js provides non-intrusive, accessible toast notifications.
**Implementation:**
- Add Sonner.js via CDN (matching the CDN approach used for Tailwind in Phase 1)
- Create a JS toast utility for consistent usage
- Replace all Bootstrap `.alert` patterns and inline error displays
- Toast variants: success (green), error (red), warning (amber), info (blue)

### D-13: Error Page Redesign — Branded Error UI
**Decision:** Redesign Error.cshtml with Tailwind to match the app's visual identity.
**Rationale:** The current error page uses Bootstrap `text-danger` classes and has a generic, unfriendly appearance. A branded error page improves UX during error states.
**Implementation:**
- Centered card layout (matching auth page pattern from Phase 1)
- Friendly illustration/icon instead of raw error text
- Clear navigation options: "Go Home", "Contact Support"
- Maintains debug information in Development mode
- Tailwind classes only, no Bootstrap remnants

### D-14: Visual Consistency Audit — Bootstrap Removal in JS
**Decision:** Systematically audit and replace all Bootstrap class references in JavaScript files with Tailwind equivalents.
**Rationale:** Phase 1 focused on CSHTML views but left Bootstrap class references in JS files that dynamically create DOM elements. These need to be migrated for full consistency.
**Implementation:**
- Replace `btn btn-danger` with Tailwind button classes
- Replace `form-control` with Tailwind input classes
- Replace `btn btn-success` with Tailwind button classes
- Remove or update any other Bootstrap patterns found in JS
- Clean up wwwroot/lib/bootstrap/ directory (unused library files)

### OpenCode's Discretion
- Specific Sonner.js toast positioning (bottom-right recommended)
- Exact animation timing values (within reasonable range)
- Token naming conventions (BEM-style or kebab-case)
- Error page illustration style (SVG icon or emoji)
- Whether to add micro-interactions to specific elements

</decisions>

<canonical_refs>
## Canonical References

**Downstream agents MUST read these before planning or implementing.**

### Completed Phase 1 Outputs
- `.planning/phases/01-ui-redesign/01-01-SUMMARY.md` — Tailwind setup, layout, theme config (CDN approach)
- `.planning/phases/01-ui-redesign/01-02-SUMMARY.md` — Auth pages, form validation patterns
- `.planning/phases/01-ui-redesign/01-03-SUMMARY.md` — Home page, skeleton loading, template grid
- `.planning/phases/01-ui-redesign/01-04-SUMMARY.md` — Template builder, Alpine.js modals, progress tracking

### Current Theme Config
- `tailwind.config.js` — Existing custom theme (primary: #3B82F6, secondary: #10B981, error: #EF4444, warning: #F59E0B)
- `wwwroot/css/tailwind.css` — Tailwind directives + custom component classes
- `package.json` — NPM config with tailwindcss dependency (build not functional — CDN used instead)

### Files with Bootstrap Remnants (to be fixed)
- `Views/Shared/Error.cshtml` — Uses `text-danger` Bootstrap classes
- `wwwroot/js/createTemplate/Checkbox-question.js` — `btn btn-danger`
- `wwwroot/js/createTemplate/Multiple-options-question.js` — `btn btn-primary`, `btn btn-danger`
- `wwwroot/js/createTemplate/Multiline-question.js` — `form-control`, `btn btn-danger`
- `wwwroot/js/createTemplate/Single-string-question.js` — `form-control`, `btn btn-danger`
- `wwwroot/js/createTemplate/Positive-integer-question.js` — `form-control`, `btn btn-danger`
- `wwwroot/js/utils/buildForm.js` — `btn btn-success`
- `wwwroot/UI/components/btnLogOut.js` — `btn btn-danger`
- `wwwroot/lib/bootstrap/` — Entire Bootstrap lib directory (unused, can be cleaned)

### Reference Views (for design token patterns)
- `Views/User/LogInView.cshtml` — Card layout pattern
- `Views/User/SignUpView.cshtml` — Card layout with validation
- `Views/Home/Index.cshtml` — Template grid + search
- `Views/Template/CreateTemplate.cshtml` — Complex form with Alpine.js
- `Views/Template/TemplateView.cshtml` — Response form with progress

</canonical_refs>

<specifics>
## Specific Ideas

- Design tokens should include: color (primary, secondary, error, warning, gray scale, success, info), typography (font sizes from xs to 4xl, weights 400-700), spacing (4px base unit, 0.5/1/1.5/2/3/4/6/8), shadows (sm, md, lg, xl, soft), borders (none, sm, md, lg, xl, 2xl, full), transitions (fast: 150ms, normal: 200ms, slow: 300ms, easing: ease-out)
- Sonner.js should be configured with: position: bottom-right, richColors: true, closeButton: true, duration: 4000ms
- Error page should show: large 500/404 icon, "Something went wrong" heading, description text, "Go Home" button, debug info in dev mode
- JS Bootstrap cleanup should create a consistent button pattern: `btn btn-danger` → `px-4 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition-colors`
- Form inputs should use: `w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary focus:border-transparent transition-all`

</specifics>

<deferred>
## Deferred Ideas

- Proper Tailwind CLI build pipeline (requires npm registry access that was unavailable in Phase 1 — config files are ready for when network is available)
- Dark mode support (requires theming infrastructure beyond current scope)
- Bootstrap library directory cleanup (safe to remove but defer if risky)

</deferred>

---

*Phase: 02-design-tokens*
*Context gathered: 2026-07-14 via codebase audit*
