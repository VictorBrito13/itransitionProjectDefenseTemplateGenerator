---
phase: 04-error-handling
plan: 02
subsystem: Frontend
tags: [error-handling, toast, UX]
dependency_graph:
  requires: [04-01]
  provides: [errorDisplay.js, enhanced makeRequest.js]
  affects: [createTemplate.js, templateView.js]
tech-stack:
  added: []
  patterns: [global-utility-functions, window-export, toast-integration]
key-files:
  created: [wwwroot/js/utils/errorDisplay.js]
  modified:
    - wwwroot/js/utils/http/makeRequest.js
    - Views/Shared/_Layout.cshtml
    - wwwroot/js/createTemplate/createTemplate.js
    - wwwroot/js/templateView/templateView.js
decisions:
  - "Load errorDisplay.js as non-module script (window exports) before toast.js"
  - "makeRequest.js returns { error, data } structure on failures"
  - "parseErrorMessage handles both legacy { errorMsg } and new { error: { message } }"
  - "Use showError for transient errors, showErrorInContainer for persistent ones"
metrics:
  duration: "5 minutes"
  completed: "2026-07-15T14:44:37Z"
  tasks_completed: 2
  files_modified: 5
---

# Phase 4 Plan 02: Frontend Error Display Summary

Created reusable error display utility and enhanced HTTP error handling to show user-friendly toast notifications on backend failures.

## What Was Built

### 1. ErrorDisplay Utility (`wwwroot/js/utils/errorDisplay.js`)
New reusable error display module with six functions:
- **`showError(message)`** — Shows error as Sonner.js toast notification
- **`showErrorInContainer(message, container)`** — Renders styled error in a DOM element (persistent display)
- **`clearError(container)`** — Clears error from a container element
- **`showFieldError(input, errorEl, message)`** — Inline field-level validation error
- **`clearFieldError(input, errorEl)`** — Clears field-level validation error
- **`parseErrorMessage(json)`** — Parses server error responses, handling both legacy `{ errorMsg }` and new `{ error: { message } }` formats

All functions are exported to `window` for global availability.

### 2. Enhanced `makeRequest.js`
Updated the HTTP utility to automatically:
- Parse and display HTTP error responses (4xx, 5xx) as toast notifications
- Show network error toasts when fetch fails (catch block)
- Return structured `{ error: { code, message }, data: null }` on failures instead of `null`

### 3. Layout Integration
Added `errorDisplay.js` script reference in `_Layout.cshtml` BEFORE `toast.js` to ensure `showToast` is available when `showError` is called.

### 4. Updated `createTemplate.js`
- Replaced inline error HTML creation with `showError()` for user search errors
- Updated template update/create handlers to check `json.error` (new structured format) instead of `json.errorMsg`
- Removed `showToast` + `$serverMsgs.textContent` inline patterns

### 5. Updated `templateView.js`
- Template load errors use `showErrorInContainer()` for persistent display
- Like/unlike errors use `showError()` toasts (transient)
- Submit handler errors use `showErrorInContainer()` for persistent display
- Catch block errors use `showErrorInContainer()` for persistent display

## Deviations from Plan

None — plan executed exactly as written.

## Known Stubs

None — all error display functions are fully wired.

## Threat Flags

None — error messages are user-friendly, no stack traces or internal details exposed.

## Self-Check

### Files Created/Modified
- [x] `wwwroot/js/utils/errorDisplay.js` — Created with `showError`, `showErrorInContainer`, `parseErrorMessage`
- [x] `wwwroot/js/utils/http/makeRequest.js` — Enhanced with error handling and toast integration
- [x] `Views/Shared/_Layout.cshtml` — Added `errorDisplay.js` script reference before `toast.js`
- [x] `wwwroot/js/createTemplate/createTemplate.js` — Updated error handling to use `showError` and `json.error` format
- [x] `wwwroot/js/templateView/templateView.js` — Updated error handling to use `showError`/`showErrorInContainer` and `json.error` format

### Commits
- [x] `fa93efc` — feat(04-02): create errorDisplay utility and enhance makeRequest.js
- [x] `8cef57c` — feat(04-02): update frontend JS files with proper error handling

### Verification
- [x] `errorDisplay.js` contains `function showError` ✓
- [x] `errorDisplay.js` contains `function showErrorInContainer` ✓
- [x] `errorDisplay.js` contains `function parseErrorMessage` ✓
- [x] `makeRequest.js` calls `showError()` on HTTP errors ✓
- [x] `makeRequest.js` returns `{ error, data }` structure ✓
- [x] `_Layout.cshtml` loads `errorDisplay.js` before `toast.js` ✓
- [x] Legacy `errorMsg` references reduced in both JS files ✓
- [x] Both files use `showError()` for error notifications ✓

### Build Status
⚠️ Pre-existing build errors in `Controllers/Templates.cs` and `Controllers/UserController.cs` — NOT caused by this plan's changes (all modified files are JavaScript and Razor templates, no C# files touched).

## Self-Check: PASSED
