// ***********************************************
// Custom Cypress Commands for Authentication
// ***********************************************

/**
 * Login via UI
 * @param {string} email - User email
 * @param {string} password - User password
 */
Cypress.Commands.add('login', (email, password) => {
  cy.visit('/user/log-in');
  cy.get('[data-cy="login-email"]').clear().type(email);
  cy.get('[data-cy="login-password"]').clear().type(password);
  cy.get('[data-cy="login-submit-btn"]').click();
  cy.url().should('eq', Cypress.config('baseUrl') + '/');
});

/**
 * Sign up via UI
 * @param {string} email - User email
 * @param {string} password - User password
 * @param {string} username - Username
 */
Cypress.Commands.add('signup', (email, password, username) => {
  cy.visit('/user/sign-up');
  cy.get('[data-cy="signup-email"]').clear().type(email);
  cy.get('[data-cy="signup-username"]').click(); // blurs email, triggers validateEmail
  cy.get('[data-cy="signup-username"]').clear().type(username);
  cy.get('[data-cy="signup-password"]').click(); // blurs username, triggers validateUsername
  cy.get('[data-cy="signup-password"]').clear().type(password);
  cy.get('[data-cy="signup-confirm-password"]').clear().type(password);
  cy.get('[data-cy="signup-submit-btn"]').should('not.be.disabled').click();
  cy.url().should('eq', Cypress.config('baseUrl') + '/');
});

/**
 * Logout via UI
 * Clicks the user avatar to open dropdown, then clicks sign out
 */
Cypress.Commands.add('logout', () => {
  cy.get('[data-cy="user-avatar-btn"]').click();
  cy.get('[data-cy="sign-out-link"]').click();
  cy.url().should('include', '/user/log-in');
});

/**
 * Create session via API (bypass UI for faster test setup)
 * @param {string} email - User email
 * @param {string} password - User password
 */
Cypress.Commands.add('createSession', (email, password) => {
  cy.request({
    method: 'POST',
    url: '/user/log-in',
    form: true,
    body: {
      email: email,
      password: password,
    },
  }).then((response) => {
    expect(response.status).to.eq(200);
  });
});
