describe('Sign Up', () => {
  const uniqueId = Date.now();

  beforeEach(() => {
    cy.visit('/user/sign-up');
  });

  it('should sign up with valid credentials and redirect to home', () => {
    const email = `newuser${uniqueId}@example.com`;
    const username = `newuser${uniqueId}`;
    const password = 'TestPass123!';

    cy.get('[data-cy="signup-email"]').type(email);
    cy.get('[data-cy="signup-username"]').type(username);
    cy.get('[data-cy="signup-password"]').type(password);
    cy.get('[data-cy="signup-confirm-password"]').type(password);
    cy.get('body').click();
    cy.get('[data-cy="signup-email"]').blur();
    cy.get('[data-cy="signup-username"]').blur();
    cy.get('[data-cy="signup-submit-btn"]').should('not.be.disabled').click();

    cy.url().should('eq', Cypress.config('baseUrl') + '/');
    cy.get('[data-cy="user-avatar-btn"]').should('be.visible');
  });

  it('should show error for duplicate email', () => {
    const email = `dupuser${uniqueId}@example.com`;

    // First signup
    cy.signup(email, 'TestPass123!', `dupuser${uniqueId}`);

    // Try again with same email
    cy.visit('/user/sign-up');
    cy.get('[data-cy="signup-email"]').type(email);
    cy.get('[data-cy="signup-username"]').type(`different${uniqueId}`);
    cy.get('[data-cy="signup-password"]').type('TestPass123!');
    cy.get('[data-cy="signup-confirm-password"]').type('TestPass123!');
    cy.get('body').click();
    cy.get('[data-cy="signup-email"]').blur();
    cy.get('[data-cy="signup-username"]').blur();
    cy.get('[data-cy="signup-submit-btn"]').should('not.be.disabled').click();

    cy.get('[data-cy="signup-error-container"]')
      .should('be.visible')
      .and('not.be.empty');
  });

  it('should show error for invalid email format', () => {
    cy.get('[data-cy="signup-email"]').type('notavalidemail');
    cy.get('[data-cy="signup-username"]').type('testuser');
    cy.get('[data-cy="signup-password"]').type('TestPass123!');
    cy.get('[data-cy="signup-confirm-password"]').type('TestPass123!');

    cy.get('[data-cy="signup-username"]').click();

    cy.get('[data-cy="signup-email-error"]')
      .should('be.visible')
      .and('contain', 'valid email');
  });

  it('should show error for weak password (less than 8 characters)', () => {
    cy.get('[data-cy="signup-email"]').type(`weakpw${uniqueId}@example.com`);
    cy.get('[data-cy="signup-username"]').type(`weakpwuser${uniqueId}`);
    cy.get('[data-cy="signup-password"]').type('Ab1!');
    cy.get('[data-cy="signup-confirm-password"]').type('Ab1!');

    cy.get('[data-cy="signup-confirm-password"]').click();

    cy.get('[data-cy="signup-password-error"]')
      .should('be.visible')
      .and('contain', '8 characters');
  });
});
