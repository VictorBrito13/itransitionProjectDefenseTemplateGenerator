---
phase: 06-e2e-testing-with-cypress
plan: 00
subsystem: testing
tags: [cypress, e2e, data-cy, selectors, testing-infrastructure]

# Dependency graph
requires: []
provides:
  - "data-cy attributes on all interactive elements across 6 Razor views"
  - "data-cy attributes on dynamic elements in 3 JavaScript files"
  - "Stable selectors for Cypress E2E test automation"
affects: [06-01, 06-02, 06-03]

# Tech tracking
tech-stack:
  added: []
  patterns: [data-cy-selector-convention]

key-files:
  created: []
  modified:
    - Views/Shared/_Layout.cshtml
    - Views/User/LogInView.cshtml
    - Views/User/SignUpView.cshtml
    - Views/Home/Index.cshtml
    - Views/Template/CreateTemplate.cshtml
    - Views/Template/TemplateView.cshtml
    - wwwroot/js/utils/templates/printTemplates.js
    - wwwroot/js/createTemplate/BaseQuestion.js
    - wwwroot/js/createTemplate/Multiple-options-question.js

key-decisions:
  - "Used data-cy attributes instead of CSS classes/IDs for Cypress selectors"
  - "Applied kebab-case naming convention with view-prefixed names where IDs overlap"
  - "Used dataset.cy for JavaScript dynamic elements"

patterns-established:
  - "data-cy selector pattern: kebab-case with view prefix (e.g., login-email, signup-email)"

requirements-completed: []

# Metrics
duration: 7min
completed: 2026-07-17
---

# Phase 6 Plan 00: Add data-cy Attributes Summary

**Added 107 data-cy test selectors across 6 Razor views and 3 JavaScript files for Cypress E2E testing**

## Performance

- **Duration:** 7 min
- **Started:** 2026-07-17T15:16:51Z
- **Completed:** 2026-07-17T15:23:51Z
- **Tasks:** 7
- **Files modified:** 9

## Accomplishments
- Added data-cy attributes to all interactive elements in _Layout.cshtml (15 elements)
- Added data-cy attributes to all form elements in LogInView.cshtml (9 elements)
- Added data-cy attributes to all form elements in SignUpView.cshtml (19 elements)
- Added data-cy attributes to all interactive elements in Home/Index.cshtml (12 elements)
- Added data-cy attributes to all controls and modals in CreateTemplate.cshtml (36 elements)
- Added data-cy attributes to all interactive elements in TemplateView.cshtml (16 elements)
- Added data-cy attributes to dynamic elements in JavaScript files (5 elements)

## Task Commits

Each task was committed atomically:

1. **Task 1: Add data-cy to _Layout.cshtml** - `0b487f5` (test)
2. **Task 2: Add data-cy to LogInView.cshtml** - `a278c40` (test)
3. **Task 3: Add data-cy to SignUpView.cshtml** - `06a0787` (test)
4. **Task 4: Add data-cy to Home/Index.cshtml** - `dc0a524` (test)
5. **Task 5: Add data-cy to CreateTemplate.cshtml** - `55c0439` (test)
6. **Task 6: Add data-cy to TemplateView.cshtml** - `9295a3a` (test)
7. **Task 7: Add data-cy to dynamic JS elements** - `c72b290` (test)

## Files Created/Modified
- `Views/Shared/_Layout.cshtml` - Layout with data-cy attributes on navbar, sidebar, auth links
- `Views/User/LogInView.cshtml` - Login form with data-cy attributes on all form elements
- `Views/User/SignUpView.cshtml` - Signup form with data-cy attributes on all form elements
- `Views/Home/Index.cshtml` - Home page with data-cy attributes on search, templates, actions
- `Views/Template/CreateTemplate.cshtml` - Template builder with data-cy attributes on all controls and modals
- `Views/Template/TemplateView.cshtml` - Template view with data-cy attributes on like, response form
- `wwwroot/js/utils/templates/printTemplates.js` - Template cards with data-cy attributes on dynamic links
- `wwwroot/js/createTemplate/BaseQuestion.js` - Question elements with data-cy attributes on delete buttons
- `wwwroot/js/createTemplate/Multiple-options-question.js` - Multi-choice elements with data-cy attributes on edit/delete buttons

## Decisions Made
- Used data-cy attributes instead of CSS classes/IDs for Cypress selectors to ensure tests don't break when styling changes
- Applied kebab-case naming convention with view-prefixed names where IDs overlap (e.g., login-email vs signup-email)
- Used dataset.cy for JavaScript dynamic elements for cleaner code

## Deviations from Plan

None - plan executed exactly as written.

## Issues Encountered

None

## User Setup Required

None - no external service configuration required.

## Next Phase Readiness
- All interactive elements now have stable data-cy selectors for Cypress tests
- Ready for Phase 6 Plan 01: Cypress installation and configuration
- Ready for Phase 6 Plan 02: Writing E2E tests using the data-cy selectors

---
*Phase: 06-e2e-testing-with-cypress*
*Completed: 2026-07-17*

## Self-Check: PASSED

- All 7 commits verified in git log
- All 9 modified files verified on disk
- Total data-cy count: 107 across 6 Razor views + 5 dynamic elements in 3 JS files
