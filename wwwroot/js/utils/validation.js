/**
 * Shared form validation utilities.
 * Used by LogInView, SignUpView, and any form with inline field validation.
 */

/**
 * Shows a field-level validation error.
 * @param {HTMLElement} input - The input element to mark as invalid
 * @param {HTMLElement} errorEl - The error message element to display
 * @param {string} message - The error message text
 */
export function showFieldError(input, errorEl, message) {
    input.classList.add("border-red-500");
    input.classList.remove("border-gray-300");
    errorEl.textContent = message;
    errorEl.classList.remove("hidden");
}

/**
 * Clears a field-level validation error.
 * @param {HTMLElement} input - The input element to restore
 * @param {HTMLElement} errorEl - The error message element to hide
 */
export function clearFieldError(input, errorEl) {
    input.classList.remove("border-red-500");
    input.classList.add("border-gray-300");
    errorEl.classList.add("hidden");
}
