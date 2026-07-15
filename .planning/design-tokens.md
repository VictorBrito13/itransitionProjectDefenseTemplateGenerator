# Design Tokens — ItransitionTemplates

**Last updated:** 2026-07-14
**Source of truth:** `wwwroot/css/tailwind.css` (`:root` block)
**Consumed by:** All CSHTML views via Tailwind utility classes and CSS custom properties

## Quick Reference

| Token Category | Files | Prefix |
|----------------|-------|--------|
| Color | tailwind.css, tailwind.config.js | `--color-` |
| Typography | tailwind.css, tailwind.config.js | `--font-`, `--font-size-` |
| Spacing | tailwind.css | `--spacing-` |
| Border Radius | tailwind.css, tailwind.config.js | `--radius-` |
| Shadows | tailwind.css, tailwind.config.js | `--shadow-` |
| Transitions | tailwind.css, tailwind.config.js | `--transition-` |
| Z-Index | tailwind.css, tailwind.config.js | `--z-` |

---

## Color Tokens

### Primary (Blue) — `#3B82F6`
Used for: primary actions, links, active states, brand elements

| Token | Value | Usage |
|-------|-------|-------|
| `--color-primary-50` | #EFF6FF | Hover backgrounds |
| `--color-primary-100` | #DBEAFE | Light backgrounds |
| `--color-primary-500` | #3B82F6 | **Default primary** |
| `--color-primary-600` | #2563EB | Hover state |
| `--color-primary-700` | #1D4ED8 | Active state |

Tailwind class: `bg-primary`, `text-primary`, `hover:bg-primary/10`, `focus:ring-primary`

### Secondary (Emerald) — `#10B981`
Used for: success states, confirmations, positive actions

| Token | Value | Usage |
|-------|-------|-------|
| `--color-secondary-500` | #10B981 | **Default secondary** |
| `--color-secondary-600` | #059669 | Hover state |

Tailwind class: `bg-secondary`, `text-secondary`, `hover:bg-secondary/10`

### Error (Red) — `#EF4444`
Used for: destructive actions, error messages, validation errors

| Token | Value | Usage |
|-------|-------|-------|
| `--color-error-50` | #FEF2F2 | Error background |
| `--color-error-500` | #EF4444 | **Default error** |
| `--color-error-600` | #DC2626 | Hover state |

Tailwind class: `bg-error`, `text-error`, `border-error`, `hover:bg-error/10`

### Warning (Amber) — `#F59E0B`
Used for: warnings, cautions, pending states

| Token | Value | Usage |
|-------|-------|-------|
| `--color-warning-500` | #F59E0B | **Default warning** |

Tailwind class: `bg-warning`, `text-warning`

### Gray / Neutral Scale
Used for: backgrounds, borders, text, dividers

| Token | Value | Usage |
|-------|-------|-------|
| `--color-gray-50` | #F9FAFB | Page background |
| `--color-gray-100` | #F3F4F6 | Card/section background |
| `--color-gray-200` | #E5E7EB | Borders, dividers |
| `--color-gray-300` | #D1D5DB | Disabled borders |
| `--color-gray-400` | #9CA3AF | Placeholder text |
| `--color-gray-500` | #6B7280 | Secondary text |
| `--color-gray-600` | #4B5563 | Body text |
| `--color-gray-700` | #374151 | Heading text |
| `--color-gray-800` | #1F2937 | Strong headings |
| `--color-gray-900` | #111827 | Primary text |

### Semantic Color Aliases

| Token | Value | Maps To | Purpose |
|-------|-------|---------|---------|
| `--color-bg-primary` | #FFFFFF | — | Main page background |
| `--color-bg-secondary` | #F9FAFB | gray-50 | Secondary background |
| `--color-text-primary` | #111827 | gray-900 | Primary text color |
| `--color-text-secondary` | #6B7280 | gray-500 | Secondary/muted text |
| `--color-text-inverse` | #FFFFFF | — | Text on dark backgrounds |
| `--color-border-default` | #E5E7EB | gray-200 | Default border color |
| `--color-border-focus` | #3B82F6 | primary-500 | Focus ring color |

---

## Typography Tokens

### Font Family
- Primary: `Inter` (loaded via Google Fonts)
- Fallback: `system-ui, -apple-system, sans-serif`
- Tailwind class: `font-sans` (configured in tailwind.config.js)

### Font Size Scale

| Token | Value | Tailwind | Used For |
|-------|-------|----------|----------|
| `--font-size-xs` | 0.75rem (12px) | `text-xs` | Labels, captions |
| `--font-size-sm` | 0.875rem (14px) | `text-sm` | Secondary text |
| `--font-size-base` | 1rem (16px) | `text-base` | Body text |
| `--font-size-lg` | 1.125rem (18px) | `text-lg` | Large body |
| `--font-size-xl` | 1.25rem (20px) | `text-xl` | Subheadings |
| `--font-size-2xl` | 1.5rem (24px) | `text-2xl` | Section headings |
| `--font-size-3xl` | 1.875rem (30px) | `text-3xl` | Page headings |
| `--font-size-4xl` | 2.25rem (36px) | `text-4xl` | Hero headings |

### Font Weights
| Token | Value | Tailwind | Used For |
|-------|-------|----------|----------|
| `--font-weight-normal` | 400 | `font-normal` | Body text |
| `--font-weight-medium` | 500 | `font-medium` | Emphasized text |
| `--font-weight-semibold` | 600 | `font-semibold` | Subheadings |
| `--font-weight-bold` | 700 | `font-bold` | Headings |

---

## Spacing Tokens (4px Base)

| Token | Value | Tailwind | Used For |
|-------|-------|----------|----------|
| `--spacing-0` | 0px | `p-0`, `m-0` | No spacing |
| `--spacing-1` | 0.25rem (4px) | `p-1`, `gap-1` | Micro spacing |
| `--spacing-2` | 0.5rem (8px) | `p-2`, `gap-2` | Tight spacing |
| `--spacing-3` | 0.75rem (12px) | `p-3`, `gap-3` | Compact spacing |
| `--spacing-4` | 1rem (16px) | `p-4`, `gap-4` | **Default spacing** |
| `--spacing-6` | 1.5rem (24px) | `p-6`, `gap-6` | Section padding |
| `--spacing-8` | 2rem (32px) | `p-8`, `gap-8` | Card padding |
| `--spacing-12` | 3rem (48px) | `p-12`, `gap-12` | Large sections |
| `--spacing-16` | 4rem (64px) | `p-16`, `gap-16` | Page sections |
| `--spacing-20` | 5rem (80px) | `p-20` | Hero sections |

---

## Border Radius Tokens

| Token | Value | Tailwind | Used For |
|-------|-------|----------|----------|
| `--radius-sm` | 0.25rem | `rounded-sm` | Inputs, badges |
| `--radius-md` | 0.375rem | `rounded-md` | Buttons |
| `--radius-lg` | 0.5rem | `rounded-lg` | Cards, containers |
| `--radius-xl` | 1rem | `rounded-xl` | **Default card radius** |
| `--radius-2xl` | 1.5rem | `rounded-2xl` | Large cards, modals |
| `--radius-full` | 9999px | `rounded-full` | Avatars, pills |

---

## Shadow Tokens

| Token | Value | Tailwind | Used For |
|-------|-------|----------|----------|
| `--shadow-sm` | 0 1px 2px rgba(0,0,0,0.05) | `shadow-sm` | Subtle elevation |
| `--shadow-md` | 0 4px 6px -1px rgba(0,0,0,0.1) | `shadow-md` | Cards |
| `--shadow-lg` | 0 10px 15px -3px rgba(0,0,0,0.1) | `shadow-lg` | Dropdowns, modals |
| `--shadow-xl` | 0 20px 25px -5px rgba(0,0,0,0.1) | `shadow-xl` | Large modals |
| `--shadow-soft` | 0 2px 15px -3px rgba(0,0,0,0.07) | `shadow-soft` | **Default card shadow** |

---

## Transition Tokens

| Token | Value | Tailwind | Used For |
|-------|-------|----------|----------|
| `--transition-fast` | 150ms | `duration-fast` | Micro-interactions |
| `--transition-normal` | 200ms | `duration-normal` | Hover states |
| `--transition-slow` | 300ms | `duration-slow` | Panel slide, modals |
| `--ease-out` | cubic-bezier(0.16,1,0.3,1) | `ease-out` | Elements entering |
| `--ease-in-out` | cubic-bezier(0.4,0,0.2,1) | `ease-in-out` | Toggle transitions |

---

## Z-Index Scale Tokens

| Token | Value | Purpose |
|-------|-------|---------|
| `--z-dropdown` | 50 | Dropdown menus |
| `--z-sticky` | 100 | Sticky headers |
| `--z-fixed` | 200 | Fixed navbars |
| `--z-modal-backdrop` | 300 | Modal backdrops |
| `--z-modal` | 400 | Modal dialogs |
| `--z-toast` | 500 | Toast notifications |

---

## Usage Examples

### In .cshtml views (preferred — use Tailwind utility classes):
```html
<button class="bg-primary text-white px-6 py-2.5 rounded-xl hover:bg-primary-600 transition-colors duration-fast">
  Submit
</button>
<div class="card p-8">
  <h2 class="text-2xl font-bold text-gray-900">Title</h2>
  <p class="text-gray-500 mt-2">Description</p>
</div>
```

### In JavaScript (for dynamically created elements):
```javascript
element.className = 'px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-primary';
```

### Via CSS custom properties (for component styles in tailwind.css):
```css
.custom-element {
  background-color: var(--color-bg-secondary);
  border: 1px solid var(--color-border-default);
  border-radius: var(--radius-xl);
  box-shadow: var(--shadow-soft);
  transition: box-shadow var(--transition-normal) var(--ease-out);
}
```

---

## Adding New Tokens

1. Add the token value to `:root` in `wwwroot/css/tailwind.css`
2. Add a Tailwind utility mapping in `tailwind.config.js` (if needed as a utility class)
3. Document the new token in this file
4. Update existing views to use the new token where appropriate
