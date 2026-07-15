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
