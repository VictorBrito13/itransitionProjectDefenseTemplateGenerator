# Phase 1: UI Redesign — Context

**Gathered:** 2026-07-14
**Status:** Ready for planning
**Source:** User requirements

<domain>
## Phase Boundary

This phase delivers a complete visual and UX redesign of the ItransitionTemplates website. It covers:
- Migration from Bootstrap 5 to Tailwind CSS
- Redesign of all 5 views (Home, CreateTemplate, TemplateView, SignUp, LogIn)
- New shared layout with improved navigation
- Nielsen Heuristic compliance across all user-facing pages
- Information architecture improvements
- Responsive design implementation

**Out of scope:** Backend changes, database schema, API modifications, new features, authentication logic changes.

</domain>

<decisions>
## Implementation Decisions

### D-01: CSS Framework — Tailwind CSS
**Decision:** Migrate from Bootstrap 5 to Tailwind CSS.
**Rationale:** User explicitly requested Tailwind CSS refactor. Provides utility-first approach for rapid UI development, smaller bundle via purging, and fine-grained control over design.
**Implementation:** Use Tailwind CSS via CDN initially for rapid development, configure purge for production builds.

### D-02: Design System — Custom Tailwind Theme
**Decision:** Create a custom Tailwind configuration with project-specific design tokens.
**Rationale:** Ensures consistency across all pages. Define color palette, typography scale, spacing, and component variants.
**Implementation:** Create `tailwind.config.js` with custom theme extending default Tailwind values.

### D-03: Navigation Pattern — Sidebar + Top Bar
**Decision:** Implement a responsive sidebar navigation for authenticated users, top navbar for guests.
**Rationale:** Provides clear information hierarchy. Sidebar allows quick access to template management, top bar is simpler for guests.
**Implementation:** Sidebar collapses to hamburger on mobile. Top navbar shows logo, search, and auth buttons.

### D-04: Component Library — Reusable Tailwind Components
**Decision:** Create reusable component partials for common UI patterns.
**Rationale:** Ensures consistency and reduces duplication. Common patterns: buttons, cards, forms, modals, toasts.
**Implementation:** Create `Views/Shared/Components/` directory with reusable partials.

### D-05: Toast Notifications — Sonner.js
**Decision:** Use Sonner.js for toast notifications instead of Bootstrap alerts.
**Rationale:** Non-intrusive feedback for success/error actions. Better UX than inline alerts or modal dialogs.
**Implementation:** Include Sonner.js via CDN, create JS utility for showing toasts.

### D-06: Loading States — Skeleton Screens
**Decision:** Implement skeleton screen loading states for all async content.
**Rationale:** Nielsen Heuristic #1 (Visibility of system status). Provides immediate visual feedback during data loading.
**Implementation:** CSS-only skeleton animations with Tailwind, show during API calls.

### D-07: Form Validation — Client-side Inline Validation
**Decision:** Implement inline form validation with real-time feedback.
**Rationale:** Nielsen Heuristic #5 (Error prevention) and #9 (Help users recognize errors). Prevents form submission with invalid data.
**Implementation:** Use HTML5 validation attributes + custom JS for real-time feedback on blur.

### D-08: Empty States — Friendly Empty State Designs
**Decision:** Design dedicated empty state components for lists with no content.
**Rationale:** Nielsen Heuristic #2 (Match between system and real world). Instead of blank areas, show helpful messages with calls-to-action.
**Implementation:** Create empty state component with icon, message, and action button.

### D-09: Typography — Inter Font Family
**Decision:** Use Inter font family for body text.
**Rationale:** Modern, highly readable sans-serif designed for screens. Excellent for UI with wide language support.
**Implementation:** Load Inter from Google Fonts, configure in Tailwind theme.

### D-10: Color Palette — Modern Professional
**Decision:** Define a modern, professional color palette.
**Rationale:** Consistent visual identity. Primary blue for actions, neutral grays for backgrounds, semantic colors for feedback.
**Implementation:** Primary: #3B82F6 (blue-500), Secondary: #10B981 (emerald-500), Error: #EF4444 (red-500), Warning: #F59E0B (amber-500), Neutral: gray scale.

### OpenCode's Discretion
- Specific component variants (button sizes, card styles)
- Animation timing and easing curves
- Exact breakpoint values (Tailwind defaults are acceptable)
- Shadow and border radius values
- Z-index stacking order

</decisions>

<canonical_refs>
## Canonical References

**Downstream agents MUST read these before planning or implementing.**

### Current Implementation
- `Views/Shared/_Layout.cshtml` — Current layout to be replaced
- `Views/Home/Index.cshtml` — Current home page
- `Views/Template/CreateTemplate.cshtml` — Current template builder
- `Views/Template/TemplateView.cshtml` — Current template view
- `Views/User/SignUpView.cshtml` — Current signup form
- `Views/User/LogInView.cshtml` — Current login form
- `wwwroot/css/site.css` — Current minimal CSS (72 lines)
- `wwwroot/js/` — Client-side JavaScript files

### Design References
- Nielsen's 10 Usability Heuristics — Standard for heuristic evaluation
- Tailwind CSS Documentation — Utility classes and configuration
- Information Architecture Principles — Navigation and content organization

</canonical_refs>

<specifics>
## Specific Ideas

- The current CreateTemplate page is the most complex view — it needs the most attention for UX improvements
- Template search on home page should have better filtering and sorting options
- Like/favorite functionality needs better visual feedback
- Error messages currently use inline alerts — should be replaced with toast notifications
- The "Danger Zone" for template deletion should be visually separated and use confirmation patterns
- User management modals (admin assignment, user access) need better UX flow
- Question type selection in template builder should be more intuitive (icons, descriptions)
- The template response form should have better progress indication

</specifics>

<deferred>
## Deferred Ideas

None — all items are within phase scope.

</deferred>

---

*Phase: 01-ui-redesign*
*Context gathered: 2026-07-14 via user requirements*
