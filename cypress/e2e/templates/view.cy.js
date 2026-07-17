describe('Template Viewing', () => {
  beforeEach(() => {
    cy.fixture('users.json').as('users');
    cy.fixture('templates.json').as('templates');
  });

  it('should display templates on home page', function () {
    // Visit home page
    cy.visit('/');

    // Wait for templates to load
    cy.get('[data-cy="home-templates-container"]').should('be.visible');

    // Verify Latest Templates header is visible
    cy.get('[data-cy="home-templates-header"]').should('contain', 'Latest Templates');
  });

  it('should open template details when clicking a template', function () {
    // Login first
    cy.login(this.users.validUser.email, this.users.validUser.password);

    // Visit home page
    cy.visit('/');

    // Wait for templates to load
    cy.get('[data-cy="home-templates-container"]').should('be.visible');

    // Click on first template card
    cy.get('[data-cy="home-templates-container"]')
      .find('[data-cy="template-card-link"]')
      .first()
      .click();

    // Verify template view page loads
    cy.get('[data-cy="form-title"]').should('be.visible');
    cy.get('[data-cy="form-description"]').should('be.visible');
  });

  it('should show template questions on template view', function () {
    // Login first
    cy.login(this.users.validUser.email, this.users.validUser.password);

    // Visit home page
    cy.visit('/');

    // Wait for templates to load
    cy.get('[data-cy="home-templates-container"]').should('be.visible');

    // Click on first template card
    cy.get('[data-cy="home-templates-container"]')
      .find('[data-cy="template-card-link"]')
      .first()
      .click();

    // Verify response form exists
    cy.get('[data-cy="response-form"]').should('be.visible');

    // Verify submit button exists
    cy.get('[data-cy="btn-submit-response"]').should('be.visible');
  });

  it('should redirect to login for unauthenticated template view', function () {
    // Visit template view page without login
    cy.visit('/template/template');

    // Should redirect to login page
    cy.url().should('include', '/user/log-in');
  });
});
