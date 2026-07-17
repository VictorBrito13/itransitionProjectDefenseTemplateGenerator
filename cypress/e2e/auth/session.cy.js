describe('Session-Aware UI', () => {
  const uniqueId = Date.now();
  const testEmail = `sessiontest${uniqueId}@example.com`;
  const testUsername = `sessiontestuser${uniqueId}`;
  const testPassword = 'TestPass123!';

  before(() => {
    // Create user via signup first
    cy.signup(testEmail, testPassword, testUsername);
  });

  describe('when authenticated', () => {
    beforeEach(() => {
      cy.login(testEmail, testPassword);
    });

    it('should show logout button when authenticated', () => {
      cy.get('[data-cy="user-avatar-btn"]').should('be.visible');

      cy.get('[data-cy="user-avatar-btn"]').click();
      cy.get('[data-cy="sign-out-link"]').should('be.visible');
    });

    it('should hide sign-in and sign-up links when authenticated', () => {
      cy.get('[data-cy="sign-in-link"]').should('not.exist');
      cy.get('[data-cy="get-started-link"]').should('not.exist');
    });
  });

  describe('when not authenticated', () => {
    it('should show sign-in and sign-up links when not authenticated', () => {
      cy.visit('/');

      cy.get('[data-cy="sign-in-link"]').should('be.visible');
      cy.get('[data-cy="get-started-link"]').should('be.visible');
    });

    it('should hide logout button when not authenticated', () => {
      cy.visit('/');

      cy.get('[data-cy="user-avatar-btn"]').should('not.exist');
      cy.get('[data-cy="sign-out-link"]').should('not.exist');
    });
  });

  describe('session persistence', () => {
    it('should maintain session across page refreshes', () => {
      cy.login(testEmail, testPassword);

      cy.reload();

      cy.get('[data-cy="user-avatar-btn"]').should('be.visible');
      cy.get('[data-cy="sign-in-link"]').should('not.exist');
    });
  });
});
