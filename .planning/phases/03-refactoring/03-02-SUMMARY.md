---
phase: 03-refactoring
plan: 02
subsystem: frontend
tags: [refactoring, deduplication, base-class, shared-utilities, cleanup]
dependency_graph:
  requires: []
  provides: [BaseQuestion, shared-validation, toast-partial]
  affects: [createTemplate, signUp, logIn, index, templateView]
tech_stack:
  added: []
  patterns: [class-inheritance, es-modules, razor-partial]
key_files:
  created:
    - wwwroot/js/createTemplate/BaseQuestion.js
    - wwwroot/js/utils/validation.js
    - Views/Shared/_ToastPartial.cshtml
  modified:
    - wwwroot/js/createTemplate/Single-string-question.js
    - wwwroot/js/createTemplate/Multiline-question.js
    - wwwroot/js/createTemplate/Positive-integer-question.js
    - wwwroot/js/createTemplate/Checkbox-question.js
    - wwwroot/js/createTemplate/Multiple-options-question.js
    - wwwroot/js/signUp/signUp.js
    - wwwroot/js/index/index.js
    - Views/User/LogInView.cshtml
    - Views/User/SignUpView.cshtml
    - Views/Template/CreateTemplate.cshtml
    - Views/Template/TemplateView.cshtml
    - Views/Home/Index.cshtml
    - Views/Shared/_Layout.cshtml
  deleted:
    - wwwroot/js/updateTemplate/updateTemplate.js
    - wwwroot/js/site.js
decisions:
  - "BaseQuestion uses template method pattern — subclasses override createInput() and getQuestionType()"
  - "MultipleOptionsQuestion overrides addEditionControls() for unique edit-options button"
  - "Checkbox container uses Tailwind flex/gap/items-center instead of Bootstrap d-flex/gap-3/align-items-center"
  - "LogInView script converted to type=module to enable ES module imports for validation"
  - "LogInView keeps errorContainer.textContent fallback alongside toast partial (view-specific behavior)"
metrics:
  duration: 341s
  completed: 2026-07-15T12:34:03Z
  tasks: 2
  files_created: 3
  files_modified: 12
  files_deleted: 2
---

# Phase 3 Plan 2: Frontend JavaScript Deduplication Summary

Extracted shared BaseQuestion class, validation utility, and toast partial to eliminate ~80% code duplication across question types, form views, and toast notification blocks.

## Tasks Completed

### Task 1: Create BaseQuestion class and refactor all 5 question types
- **Commit:** `1181994`
- Created `BaseQuestion.js` with template method pattern: shared constructor, `createContainer()`, `createLabel()`, `addEditionControls()`, and abstract `createInput()`/`getQuestionType()` methods
- All 5 question classes (SingleLine, Multiline, PositiveInteger, Checkbox, MultipleOptions) now extend BaseQuestion with only their unique input creation logic
- Replaced Bootstrap remnants in Checkbox-question (`d-flex`, `form-check-input`, `form-check-label`) with Tailwind equivalents (`flex`, `gap-3`, `items-center`, custom checkbox styling)
- MultipleOptionsQuestion overrides `addEditionControls()` to add its unique "Edit options" button
- `insertQuestions.js` works without changes — same class names and constructor signatures preserved

### Task 2: Extract shared validation utility, toast partial, remove unused files
- **Commit:** `c6c0a62`
- Created `wwwroot/js/utils/validation.js` with `showFieldError` and `clearFieldError` named exports
- Created `Views/Shared/_ToastPartial.cshtml` — reusable Razor partial reading `TempData["errorMsg"]` and `TempData["successMsg"]`
- Replaced inline toast `<script>` blocks in all 4 views (CreateTemplate, TemplateView, LogInView, SignUpView) with `@await Html.PartialAsync("_ToastPartial")`
- LogInView and signUp.js now import `showFieldError`/`clearFieldError` from shared utility instead of defining locally
- LogInView script converted to `type="module"` to enable ES module imports
- Deleted `wwwroot/js/updateTemplate/updateTemplate.js` (empty exported function, never imported)
- Deleted `wwwroot/js/site.js` (comments only, no code) and removed `<script>` reference from `_Layout.cshtml`
- Replaced 6 hardcoded skeleton card HTML blocks in `Index.cshtml` with a single `@for(int i = 0; i < 6; i++)` Razor loop
- Extracted `loadAndRenderTemplates(fetchFn, mode)` helper in `index.js` — deduplicated the toggle handler's two identical try/catch blocks

## Deviations from Plan

None — plan executed exactly as written.

## Verification Results

| Check | Result |
|-------|--------|
| All 5 question classes extend BaseQuestion | ✅ 5/5 |
| No Bootstrap remnants in Checkbox-question | ✅ 0 found |
| `showFieldError` imported from validation.js (not locally defined) | ✅ |
| `_ToastPartial.cshtml` used by all 4 views | ✅ |
| `updateTemplate.js` deleted | ✅ |
| `site.js` deleted | ✅ |
| Layout reference to site.js removed | ✅ |
| Skeleton cards use `@for` loop | ✅ 1 block, 6 iterations |
| `loadAndRenderTemplates` helper exists | ✅ |

## Self-Check: PASSED

All 3 created files found on disk. Both deleted files confirmed absent. Both commit hashes verified in git log.
