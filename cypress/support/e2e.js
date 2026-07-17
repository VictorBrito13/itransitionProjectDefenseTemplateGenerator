// ***********************************************
// Cypress E2E Support File
// ***********************************************

// Import custom commands
import './commands';

// Ignore uncaught exceptions from the application (e.g. showToast not defined on login page)
Cypress.on('uncaught:exception', (err) => {
  if (err.message.includes('showToast is not defined')) {
    return false;
  }
  return true;
});

// Global hooks
beforeEach(() => {
  cy.clearCookies();
  cy.clearLocalStorage();
});
