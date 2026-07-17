describe('Logout', () => {
  const uniqueId = Date.now();
  const testEmail = `logouttest${uniqueId}@example.com`;
  const testUsername = `logouttestuser${uniqueId}`;
  const testPassword = 'TestPass123!';

  before(() => {
    // Create user via signup first
    cy.signup(testEmail, testPassword, testUsername);
  });

  beforeEach(() => {
    // Login before each test
    cy.login(testEmail, testPassword);
  });

  it('should logout and redirect to login page', () => {
    cy.logout();

    cy.url().should('include', '/user/log-in');
  });

  it('should clear session on logout', () => {
    cy.logout();

    cy.visit('/');

    cy.url().should('include', '/user/log-in');
  });

  it('should show login page after logout', () => {
    cy.logout();

    cy.get('[data-cy="login-form"]').should('be.visible');
    cy.get('[data-cy="login-email"]').should('be.visible');
    cy.get('[data-cy="login-password"]').should('be.visible');
    cy.get('[data-cy="login-submit-btn"]').should('be.visible');
  });
});
