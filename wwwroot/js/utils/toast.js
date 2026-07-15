/**
 * Toast notification utility using Sonner.js
 * 
 * Usage:
 *   showToast('success', 'Template saved successfully!');
 *   showToast('error', 'Failed to save template.');
 *   showToast('warning', 'Session expiring soon.');
 *   showToast('info', 'New update available.');
 * 
 * Dependencies: Sonner.js (loaded via CDN in _Layout.cshtml)
 */

function showToast(type, message, options = {}) {
  if (typeof toast === 'undefined') {
    console.warn('Sonner.js not loaded — falling back to console.', type, message);
    return;
  }

  const defaults = {
    duration: 4000,
    position: 'bottom-right',
    richColors: true,
    closeButton: true,
  };

  const config = { ...defaults, ...options };

  switch (type) {
    case 'success':
      toast.success(message, config);
      break;
    case 'error':
      toast.error(message, config);
      break;
    case 'warning':
      toast.warning(message, config);
      break;
    case 'info':
    default:
      toast.info(message, config);
      break;
  }
}

// Legacy alert replacement — replaces Bootstrap-style alert patterns
function showFormError(elementId, message) {
  const container = document.getElementById(elementId);
  if (container) {
    container.textContent = message;
    container.classList.remove('hidden');
    showToast('error', message);
  }
}

function showFormSuccess(elementId, message) {
  const container = document.getElementById(elementId);
  if (container) {
    container.textContent = message;
    container.classList.remove('hidden');
    showToast('success', message);
  }
}
