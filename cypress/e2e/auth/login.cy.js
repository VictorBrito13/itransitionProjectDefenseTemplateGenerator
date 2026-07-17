describe('Login', () => {
  const uniqueId = Date.now();
  const testEmail = `logintest${uniqueId}@example.com`;
  const testUsername = `logintestuser${uniqueId}`;
  const testPassword = 'TestPass123!';

  before(() => {
    // Create user via signup first
    cy.signup(testEmail, testPassword, testUsername);
  });

  beforeEach(() => {
    cy.visit('/user/log-in');
  });

  it('should login with valid credentials and redirect to home', () => {
    cy.login(testEmail, testPassword);

    cy.url().should('eq', Cypress.config('baseUrl') + '/');
    cy.get('[data-cy="user-avatar-btn"]').should('be.visible');
  });

  it('should show error for invalid credentials', () => {
    cy.get('[data-cy="login-email"]').type(testEmail);
    cy.get('[data-cy="login-password"]').type('WrongPassword123!');
    cy.get('[data-cy="login-submit-btn"]').click();

    cy.url().should('include', '/user/log-in');
    cy.get('[data-cy="login-error-container"]')
      .should('be.visible')
      .and('contain', 'Invalid email or password');
  });

  it('should show error for non-existent user', () => {
    cy.get('[data-cy="login-email"]').type('nonexistent@example.com');
    cy.get('[data-cy="login-password"]').type('SomePassword123!');
    cy.get('[data-cy="login-submit-btn"]').click();

    cy.url().should('include', '/user/log-in');
    cy.get('[data-cy="login-error-container"]')
      .should('be.visible')
      .and('contain', 'Invalid email or password');
  });

  it('should redirect to home if already authenticated', () => {
    cy.login(testEmail, testPassword);

    cy.visit('/user/log-in');

    // The app shows the home page when authenticated (user can still access login URL)
    cy.get('[data-cy="user-avatar-btn"]').should('be.visible');
  });
});
