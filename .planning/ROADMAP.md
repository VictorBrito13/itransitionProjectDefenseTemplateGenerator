# Roadmap — ItransitionTemplates

## Milestone 1: UI Redesign & UX Enhancement

### Phase 1: UI Redesign — Tailwind CSS + Nielsen Heuristics
**Goal:** Redesign the website UI to be more attractive and usable, following Nielsen Heuristic principles and Information Architecture guidelines, while refactoring all components to use Tailwind CSS.

**Depends on:** None (first phase)

**Requirements:** [REQ-01, REQ-02, REQ-03, REQ-04, REQ-05]

**Plans:** 4 plans

**Wave 1** *(independent, can run in parallel)*:
- [x] 01-01-PLAN.md — Tailwind CSS setup and layout foundation
- [x] 01-02-PLAN.md — Authentication pages redesign (Login, SignUp)

**Wave 2** *(blocked on Wave 1 — requires layout from 01-01)*:
- [x] 01-03-PLAN.md — Home page redesign with template discovery UX
- [x] 01-04-PLAN.md — Template builder and response pages redesign

**Cross-cutting constraints:**
- All views must use Tailwind CSS classes (no Bootstrap)
- All interactive elements must have focus states (focus:ring-2)
- All forms must have inline validation
- Color palette: primary=#3B82F6, secondary=#10B981, error=#EF4444

### Phase 2: Design Token System & Visual Polish

**Goal:** Define a comprehensive design token system for cross-app consistency, implement remaining UX patterns (toast notifications, error pages, micro-interactions), and polish all views to ensure visually cohesive experience.

**Depends on:** Phase 1

**Requirements:** [REQ-06, REQ-07, REQ-08, REQ-09]

**Plans:** 3 plans

**Wave 1** *(foundation — design token system)*:
- [ ] 02-01-PLAN.md — Design Token System (CSS custom properties, extended config, documentation)

**Wave 2** *(blocked on Wave 1 — uses design tokens)*:
- [ ] 02-02-PLAN.md — Toast Notifications + Error Page Redesign (Sonner.js integration, Error.cshtml)
- [ ] 02-03-PLAN.md — Visual Consistency Audit (fix Bootstrap remnants in JS, remove Bootstrap lib)

**Cross-cutting constraints:**
- Design tokens must be documented in a single source of truth (`.planning/design-tokens.md`)
- All notifications use Sonner.js (per D-05)
- Error pages must follow the app's aesthetic (no default Bootstrap error styling)
- Animations must be tasteful and purposeful (not decorative)
- Consistent with Phase 1 color palette and typography

---

*Phase 1 focuses on frontend-only changes. Backend services, controllers, and data models remain unchanged. All UI improvements are achieved through view layer modifications and Tailwind CSS migration.*
