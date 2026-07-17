// ***********************************************
// Cypress E2E Support File
// ***********************************************

// Import custom commands
import './commands';

// Global hooks
beforeEach(() => {
  // Clear cookies and local storage before each test
  cy.clearCookies();
  cy.clearLocalStorage();
});
