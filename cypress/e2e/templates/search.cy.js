const mockTemplates = [
  {
    TemplateId: 1,
    Title: 'Test Template',
    Description: 'A test template for E2E testing',
    TopicId: 1,
    Topic: { Name: 'General' },
    Admins: [{ User: { Username: 'testuser' } }],
    Likes: [],
  },
  {
    TemplateId: 2,
    Title: 'Searchable Template',
    Description: 'Unique description for search testing',
    TopicId: 2,
    Topic: { Name: 'Technology' },
    Admins: [{ User: { Username: 'admin' } }],
    Likes: [{ LikeId: 1 }],
  },
];

describe('Template Search', () => {
  it('should search templates by title', function () {
    cy.intercept('GET', '/template/templates*', {
      statusCode: 200,
      body: mockTemplates,
    }).as('getTemplates');

    cy.visit('/');

    cy.wait('@getTemplates');
    cy.get('[data-cy="home-templates-container"]').should('be.visible');

    cy.intercept('GET', '/template/get-by-query*', {
      statusCode: 200,
      body: [mockTemplates[0]],
    }).as('searchTemplates');

    cy.get('[data-cy="home-search-input"]').clear().type('Test');

    cy.wait('@searchTemplates');
    cy.get('[data-cy="home-search-results"]').should('be.visible');
  });

  it('should search templates by description', function () {
    cy.intercept('GET', '/template/templates*', {
      statusCode: 200,
      body: mockTemplates,
    }).as('getTemplates');

    cy.visit('/');

    cy.wait('@getTemplates');
    cy.get('[data-cy="home-templates-container"]').should('be.visible');

    cy.intercept('GET', '/template/get-by-query*', {
      statusCode: 200,
      body: [mockTemplates[1]],
    }).as('searchTemplates');

    cy.get('[data-cy="home-search-input"]').clear().type('description');

    cy.wait('@searchTemplates');
    cy.get('[data-cy="home-search-results"]').should('be.visible');
  });

  it('should show message for no results', function () {
    cy.intercept('GET', '/template/templates*', {
      statusCode: 200,
      body: mockTemplates,
    }).as('getTemplates');

    cy.visit('/');

    cy.wait('@getTemplates');
    cy.get('[data-cy="home-templates-container"]').should('be.visible');

    cy.intercept('GET', '/template/get-by-query*', {
      statusCode: 404,
      body: { error: 'No templates were found try other terms' },
    }).as('searchNoResults');

    cy.get('[data-cy="home-search-input"]').clear().type('nonexistenttemplate12345');

    cy.wait('@searchNoResults');
    cy.get('[data-cy="home-search-results"]').should('be.visible');
  });

  it('should clear search and show all templates', function () {
    cy.intercept('GET', '/template/templates*', {
      statusCode: 200,
      body: mockTemplates,
    }).as('getTemplates');

    cy.visit('/');

    cy.wait('@getTemplates');
    cy.get('[data-cy="home-templates-container"]').should('be.visible');

    cy.intercept('GET', '/template/get-by-query*', {
      statusCode: 200,
      body: [mockTemplates[0]],
    }).as('searchTemplates');

    cy.get('[data-cy="home-search-input"]').clear().type('Test');
    cy.wait('@searchTemplates');

    cy.get('[data-cy="home-search-input"]').clear();

    cy.get('[data-cy="home-templates-container"]').should('be.visible');
  });
});
