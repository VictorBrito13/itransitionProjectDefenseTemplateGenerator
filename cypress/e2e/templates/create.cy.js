describe('Template Creation', () => {
  const uniqueId = Date.now();
  const testEmail = `templatetest${uniqueId}@example.com`;
  const testUsername = `templatetestuser${uniqueId}`;
  const testPassword = 'TestPass123!';

  before(() => {
    cy.signup(testEmail, testPassword, testUsername);
  });

  it('should create a new template when authenticated', function () {
    cy.login(testEmail, testPassword);
    cy.visit('/template/create');

    cy.get('[data-cy="page-title"]').should('contain', 'Create a new template');

    // Select first available topic
    cy.get('[data-cy="template-topic-select"]').find('option').then(($options) => {
      if ($options.length > 0) {
        cy.get('[data-cy="template-topic-select"]').select($options.first().val());
      }
    });

    cy.get('[data-cy="template-title-input"]').clear().type('Test Template');
    cy.get('[data-cy="template-description-input"]').clear().type('A test template');
    cy.get('[data-cy="add-single-line-btn"]').click();

    cy.intercept('POST', '/template/create').as('createTemplate');
    cy.get('[data-cy="btn-create-template"]').click();
    cy.wait('@createTemplate');
  });

  it('should redirect to login when not authenticated', function () {
    cy.visit('/template/create');
    cy.url().should('include', '/user/log-in');
  });

  it('should show error for missing required fields', function () {
    cy.login(testEmail, testPassword);
    cy.visit('/template/create');

    cy.get('[data-cy="btn-create-template"]').click();

    // The error should appear somewhere (toast or inline)
    cy.get('.sonner, [data-cy="server-responses"]').should('exist');
  });

  it('should show error for invalid topic', function () {
    cy.login(testEmail, testPassword);
    cy.visit('/template/create');

    cy.get('[data-cy="template-title-input"]').clear().type('Test Template');
    cy.get('[data-cy="template-description-input"]').clear().type('Test description');

    cy.get('[data-cy="btn-create-template"]').click();

    cy.get('.sonner, [data-cy="server-responses"]').should('exist');
  });
});
