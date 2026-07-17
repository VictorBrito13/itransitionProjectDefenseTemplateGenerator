describe('Sign Up', () => {
  beforeEach(() => {
    cy.visit('/user/sign-up');
  });

  it('should sign up with valid credentials and redirect to home', () => {
    cy.fixture('users').then((users) => {
      const { newUser } = users;

      cy.get('[data-cy="signup-email"]').type(newUser.email);
      cy.get('[data-cy="signup-username"]').type(newUser.username);
      cy.get('[data-cy="signup-password"]').type(newUser.password);
      cy.get('[data-cy="signup-confirm-password"]').type(newUser.password);
      cy.get('[data-cy="signup-email"]').blur();
      cy.get('[data-cy="signup-username"]').blur();
      cy.get('[data-cy="signup-submit-btn"]').should('not.be.disabled').click();

      cy.url().should('eq', Cypress.config('baseUrl') + '/');
      // Verify username appears in the UI (avatar or welcome)
      cy.get('[data-cy="user-avatar-btn"]').should('be.visible');
    });
  });

  it('should show error for duplicate email', () => {
    cy.fixture('users').then((users) => {
      const { validUser } = users;

      // First signup with the email
      cy.signup(validUser.email, validUser.password, validUser.username);

      // Try to sign up again with the same email
      cy.visit('/user/sign-up');
      cy.get('[data-cy="signup-email"]').type(validUser.email);
      cy.get('[data-cy="signup-username"]').type('differentuser');
      cy.get('[data-cy="signup-password"]').type(validUser.password);
      cy.get('[data-cy="signup-confirm-password"]').type(validUser.password);
      cy.get('[data-cy="signup-email"]').blur();
      cy.get('[data-cy="signup-username"]').blur();
      cy.get('[data-cy="signup-submit-btn"]').should('not.be.disabled').click();

      // Should show error message (from server via TempData or error container)
      cy.get('[data-cy="signup-error-container"]')
        .should('be.visible')
        .and('not.be.empty');
    });
  });

  it('should show error for invalid email format', () => {
    cy.get('[data-cy="signup-email"]').type('notavalidemail');
    cy.get('[data-cy="signup-username"]').type('testuser');
    cy.get('[data-cy="signup-password"]').type('Test123!');
    cy.get('[data-cy="signup-confirm-password"]').type('Test123!');

    // Trigger blur validation on email field
    cy.get('[data-cy="signup-username"]').click();

    // Should show email validation error
    cy.get('[data-cy="signup-email-error"]')
      .should('be.visible')
      .and('contain', 'valid email');
  });

  it('should show error for weak password (less than 8 characters)', () => {
    cy.get('[data-cy="signup-email"]').type('test@example.com');
    cy.get('[data-cy="signup-username"]').type('testuser');
    cy.get('[data-cy="signup-password"]').type('Ab1!');
    cy.get('[data-cy="signup-confirm-password"]').type('Ab1!');

    // Trigger blur validation on password field
    cy.get('[data-cy="signup-confirm-password"]').click();

    // Should show password validation error
    cy.get('[data-cy="signup-password-error"]')
      .should('be.visible')
      .and('contain', '8 characters');
  });
});
