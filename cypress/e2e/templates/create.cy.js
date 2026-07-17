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

    cy.intercept('POST', '/template/create').as('createTemplate');
    cy.visit('/template/create');

    cy.get('[data-cy="page-title"]').should('contain', 'Create a new template');

    cy.get('[data-cy="template-topic-select"]').select('1');

    cy.get('[data-cy="template-title-input"]')
      .clear()
      .type('Test Template');

    cy.get('[data-cy="template-description-input"]')
      .clear()
      .type('A test template for E2E testing');

    cy.get('[data-cy="add-single-line-btn"]').click();

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

    cy.get('[data-cy="server-responses"]').should('be.visible');
  });

  it('should show error for invalid topic', function () {
    cy.login(testEmail, testPassword);
    cy.visit('/template/create');

    cy.get('[data-cy="template-title-input"]')
      .clear()
      .type('Test Template');

    cy.get('[data-cy="template-description-input"]')
      .clear()
      .type('Test description');

    cy.get('[data-cy="btn-create-template"]').click();

    cy.get('[data-cy="server-responses"]').should('be.visible');
  });
});
