describe('Logout', () => {
  beforeEach(() => {
    cy.fixture('users').then((users) => {
      const { validUser } = users;
      cy.login(validUser.email, validUser.password);
    });
  });

  it('should logout and redirect to login page', () => {
    cy.logout();

    cy.url().should('include', '/user/log-in');
  });

  it('should clear session on logout', () => {
    cy.logout();

    // Try to visit a protected page after logout
    cy.visit('/');

    // Should redirect to login or show login page
    cy.url().should('include', '/user/log-in');
  });

  it('should show login page after logout', () => {
    cy.logout();

    // Verify login form is visible
    cy.get('[data-cy="login-form"]').should('be.visible');
    cy.get('[data-cy="login-email"]').should('be.visible');
    cy.get('[data-cy="login-password"]').should('be.visible');
    cy.get('[data-cy="login-submit-btn"]').should('be.visible');
  });
});
