describe('Template Like/Unlike', () => {
  beforeEach(() => {
    cy.fixture('users.json').as('users');
    cy.fixture('templates.json').as('templates');
  });

  it('should like a template when authenticated', function () {
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

    // Get initial like count
    cy.get('[data-cy="likes-number"]').then(($likes) => {
      const initialLikes = parseInt($likes.text());

      // Click like button
      cy.get('[data-cy="btn-like-template"]').click();

      // Verify like count increases
      cy.get('[data-cy="likes-number"]').should('contain', initialLikes + 1);
    });
  });

  it('should unlike a template when already liked', function () {
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

    // Like the template first
    cy.get('[data-cy="btn-like-template"]').click();

    // Get current like count
    cy.get('[data-cy="likes-number"]').then(($likes) => {
      const currentLikes = parseInt($likes.text());

      // Click unlike button
      cy.get('[data-cy="btn-like-template"]').click();

      // Verify like count decreases
      cy.get('[data-cy="likes-number"]').should('contain', currentLikes - 1);
    });
  });

  it('should show error when not authenticated', function () {
    // Visit home page
    cy.visit('/');

    // Wait for templates to load
    cy.get('[data-cy="home-templates-container"]').should('be.visible');

    // Click on first template card
    cy.get('[data-cy="home-templates-container"]')
      .find('[data-cy="template-card-link"]')
      .first()
      .click();

    // Try to like without login - should redirect to login
    cy.url().should('include', '/user/log-in');
  });

  it('should persist like across page refresh', function () {
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

    // Like the template
    cy.get('[data-cy="btn-like-template"]').click();

    // Get like count after liking
    cy.get('[data-cy="likes-number"]').then(($likes) => {
      const likedCount = parseInt($likes.text());

      // Refresh the page
      cy.reload();

      // Verify like count persists
      cy.get('[data-cy="likes-number"]').should('contain', likedCount);
    });
  });

  it('should update like count in real-time', function () {
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

    // Get initial like count
    cy.get('[data-cy="likes-number"]').then(($likes) => {
      const initialLikes = parseInt($likes.text());

      // Intercept like API
      cy.intercept('GET', '/template/like*').as('likeTemplate');

      // Click like button
      cy.get('[data-cy="btn-like-template"]').click();

      // Wait for API response
      cy.wait('@likeTemplate');

      // Verify like count updates without page refresh
      cy.get('[data-cy="likes-number"]').should('contain', initialLikes + 1);
    });
  });
});
