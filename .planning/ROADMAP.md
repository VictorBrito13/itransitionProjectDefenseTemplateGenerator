# Roadmap — ItransitionTemplates

## Milestone 1: UI Redesign & UX Enhancement

### Phase 1: UI Redesign — Tailwind CSS + Nielsen Heuristics
**Goal:** Redesign the website UI to be more attractive and usable, following Nielsen Heuristic principles and Information Architecture guidelines, while refactoring all components to use Tailwind CSS.

**Depends on:** None (first phase)

**Requirements:** [REQ-01, REQ-02, REQ-03, REQ-04, REQ-05]

**Plans:** 4 plans

**Wave 1** *(independent, can run in parallel)*:
- [ ] 01-01-PLAN.md — Tailwind CSS setup and layout foundation
- [ ] 01-02-PLAN.md — Authentication pages redesign (Login, SignUp)

**Wave 2** *(blocked on Wave 1 — requires layout from 01-01)*:
- [ ] 01-03-PLAN.md — Home page redesign with template discovery UX
- [ ] 01-04-PLAN.md — Template builder and response pages redesign

**Cross-cutting constraints:**
- All views must use Tailwind CSS classes (no Bootstrap)
- All interactive elements must have focus states (focus:ring-2)
- All forms must have inline validation
- Color palette: primary=#3B82F6, secondary=#10B981, error=#EF4444

---

*Phase 1 focuses on frontend-only changes. Backend services, controllers, and data models remain unchanged. All UI improvements are achieved through view layer modifications and Tailwind CSS migration.*
