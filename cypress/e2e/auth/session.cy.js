describe('Session-Aware UI', () => {
  describe('when authenticated', () => {
    beforeEach(() => {
      cy.fixture('users').then((users) => {
        const { validUser } = users;
        cy.login(validUser.email, validUser.password);
      });
    });

    it('should show logout button when authenticated', () => {
      cy.get('[data-cy="user-avatar-btn"]').should('be.visible');

      // Click avatar to reveal dropdown
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
      cy.fixture('users').then((users) => {
        const { validUser } = users;
        cy.login(validUser.email, validUser.password);

        // Refresh the page
        cy.reload();

        // Verify still authenticated - avatar should be visible
        cy.get('[data-cy="user-avatar-btn"]').should('be.visible');
        cy.get('[data-cy="sign-in-link"]').should('not.exist');
      });
    });
  });
});
