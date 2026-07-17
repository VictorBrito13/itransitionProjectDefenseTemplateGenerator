describe('Login', () => {
  beforeEach(() => {
    cy.visit('/user/log-in');
  });

  it('should login with valid credentials and redirect to home', () => {
    cy.fixture('users').then((users) => {
      const { validUser } = users;

      cy.login(validUser.email, validUser.password);

      cy.url().should('eq', Cypress.config('baseUrl') + '/');
      cy.get('[data-cy="user-avatar-btn"]').should('be.visible');
    });
  });

  it('should show error for invalid credentials', () => {
    cy.fixture('users').then((users) => {
      const { validUser } = users;

      cy.get('[data-cy="login-email"]').type(validUser.email);
      cy.get('[data-cy="login-password"]').type('WrongPassword123!');
      cy.get('[data-cy="login-submit-btn"]').click();

      cy.url().should('include', '/user/log-in');
      cy.get('[data-cy="login-error-container"]')
        .should('be.visible')
        .and('contain', 'Invalid email or password');
    });
  });

  it('should show error for non-existent user', () => {
    cy.fixture('users').then((users) => {
      const { invalidUser } = users;

      cy.get('[data-cy="login-email"]').type(invalidUser.email);
      cy.get('[data-cy="login-password"]').type(invalidUser.password);
      cy.get('[data-cy="login-submit-btn"]').click();

      cy.url().should('include', '/user/log-in');
      cy.get('[data-cy="login-error-container"]')
        .should('be.visible')
        .and('contain', 'Invalid email or password');
    });
  });

  it('should redirect to home if already authenticated', () => {
    cy.fixture('users').then((users) => {
      const { validUser } = users;

      // Login first
      cy.login(validUser.email, validUser.password);

      // Try to visit login page while authenticated
      cy.visit('/user/log-in');

      // Should redirect back to home
      cy.url().should('eq', Cypress.config('baseUrl') + '/');
    });
  });
});
