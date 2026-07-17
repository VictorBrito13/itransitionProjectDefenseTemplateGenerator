describe('Template Search', () => {
  beforeEach(() => {
    cy.fixture('users.json').as('users');
    cy.fixture('templates.json').as('templates');
  });

  it('should search templates by title', function () {
    // Visit home page
    cy.visit('/');

    // Wait for templates to load
    cy.get('[data-cy="home-templates-container"]').should('be.visible');

    // Intercept search API
    cy.intercept('GET', '/template/get-by-query*').as('searchTemplates');

    // Enter search term
    cy.get('[data-cy="home-search-input"]')
      .clear()
      .type('Test');

    // Wait for search results
    cy.wait('@searchTemplates');

    // Verify search results container is visible
    cy.get('[data-cy="home-search-results"]').should('be.visible');
  });

  it('should search templates by description', function () {
    // Visit home page
    cy.visit('/');

    // Wait for templates to load
    cy.get('[data-cy="home-templates-container"]').should('be.visible');

    // Intercept search API
    cy.intercept('GET', '/template/get-by-query*').as('searchTemplates');

    // Enter search term in description
    cy.get('[data-cy="home-search-input"]')
      .clear()
      .type('description');

    // Wait for search results
    cy.wait('@searchTemplates');

    // Verify search results container is visible
    cy.get('[data-cy="home-search-results"]').should('be.visible');
  });

  it('should show message for no results', function () {
    // Visit home page
    cy.visit('/');

    // Wait for templates to load
    cy.get('[data-cy="home-templates-container"]').should('be.visible');

    // Intercept search API with empty result
    cy.intercept('GET', '/template/get-by-query*', {
      statusCode: 404,
      body: { error: 'No templates were found try other terms' }
    }).as('searchNoResults');

    // Enter non-existent search term
    cy.get('[data-cy="home-search-input"]')
      .clear()
      .type('nonexistenttemplate12345');

    // Wait for search response
    cy.wait('@searchNoResults');

    // Verify no results message appears
    cy.get('[data-cy="home-search-results"]').should('be.visible');
  });

  it('should clear search and show all templates', function () {
    // Visit home page
    cy.visit('/');

    // Wait for templates to load
    cy.get('[data-cy="home-templates-container"]').should('be.visible');

    // Enter search term
    cy.get('[data-cy="home-search-input"]')
      .clear()
      .type('Test');

    // Clear search
    cy.get('[data-cy="home-search-input"]')
      .clear();

    // Verify all templates are shown again
    cy.get('[data-cy="home-templates-container"]').should('be.visible');
  });
});
