/**
 * Error display utilities for consistent error UI across the application.
 * Integrates with Sonner.js toast notifications and provides DOM-based error display.
 *
 * Usage:
 *   showError('Failed to save template');           // Shows toast
 *   showErrorInContainer('Failed to load', errorEl); // Shows in DOM element + toast
 *   showFieldError(input, errorEl, 'Required');     // Inline field validation
 */

/**
 * Show an error message as a toast notification.
 * @param {string} message - Error message to display
 * @param {Object} options - Toast configuration options
 */
function showError(message, options = {}) {
  if (typeof showToast === 'function') {
    showToast('error', message, options);
  } else {
    console.error('Error:', message);
  }
}

/**
 * Show an error in a DOM container element (for inline error display).
 * @param {string} message - Error message to display
 * @param {HTMLElement|string} container - DOM element or element ID
 */
function showErrorInContainer(message, container) {
  const el = typeof container === 'string' ? document.getElementById(container) : container;
  if (!el) return;

  el.innerHTML = `
    <div class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-xl flex items-center gap-3">
      <svg class="w-5 h-5 text-red-500 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v3.75m-9.303 3.376c-.866 1.5.217 3.374 1.948 3.374h14.71c1.73 0 2.813-1.874 1.948-3.374L13.949 3.378c-.866-1.5-3.032-1.5-3.898 0L2.697 16.126zM12 15.75h.007v.008H12v-.008z" />
      </svg>
      <span></span>
    </div>
  `;
  el.querySelector('span').textContent = message;
  el.classList.remove('hidden');
}

/**
 * Clear error display from a container element.
 * @param {HTMLElement|string} container - DOM element or element ID
 */
function clearError(container) {
  const el = typeof container === 'string' ? document.getElementById(container) : container;
  if (el) {
    el.innerHTML = '';
    el.classList.add('hidden');
  }
}

/**
 * Show a field-level validation error.
 * @param {HTMLElement} input - The input element to mark as invalid
 * @param {HTMLElement} errorEl - The error message element to display
 * @param {string} message - The error message text
 */
function showFieldError(input, errorEl, message) {
  input.classList.add('border-red-500');
  input.classList.remove('border-gray-200');
  errorEl.textContent = message;
  errorEl.classList.remove('hidden');
}

/**
 * Clear a field-level validation error.
 * @param {HTMLElement} input - The input element to restore
 * @param {HTMLElement} errorEl - The error message element to hide
 */
function clearFieldError(input, errorEl) {
  input.classList.remove('border-red-500');
  input.classList.add('border-gray-200');
  errorEl.classList.add('hidden');
}

/**
 * Parse error response from server and return user-friendly message.
 * Handles both old format ({ errorMsg }) and new format ({ error: { message } }).
 * @param {Object} json - Parsed JSON response
 * @returns {string} User-friendly error message
 */
function parseErrorMessage(json) {
  if (!json) return 'An unexpected error occurred';

  // New structured format: { error: { code, message, details } }
  if (json.error && json.error.message) {
    return json.error.message;
  }

  // Legacy format: { errorMsg }
  if (json.errorMsg) {
    return json.errorMsg;
  }

  return 'An unexpected error occurred';
}

// Export for use in other modules
if (typeof window !== 'undefined') {
  window.showError = showError;
  window.showErrorInContainer = showErrorInContainer;
  window.clearError = clearError;
  window.showFieldError = showFieldError;
  window.clearFieldError = clearFieldError;
  window.parseErrorMessage = parseErrorMessage;
}
