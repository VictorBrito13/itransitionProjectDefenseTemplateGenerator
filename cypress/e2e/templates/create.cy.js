describe('Template Creation', () => {
  beforeEach(() => {
    cy.fixture('users.json').as('users');
    cy.fixture('templates.json').as('templates');
  });

  it('should create a new template when authenticated', function () {
    // Login first
    cy.login(this.users.validUser.email, this.users.validUser.password);

    // Visit template creation page
    cy.visit('/template/create');

    // Verify page loaded
    cy.get('[data-cy="page-title"]').should('contain', 'Create a new template');

    // Select topic
    cy.get('[data-cy="template-topic-select"]').select(this.templates.validTemplate.topicId);

    // Set template title
    cy.get('[data-cy="template-title-input"]')
      .clear()
      .type(this.templates.validTemplate.title);

    // Set template description
    cy.get('[data-cy="template-description-input"]')
      .clear()
      .type(this.templates.validTemplate.description);

    // Add a question
    cy.get('[data-cy="add-single-line-btn"]').click();

    // Submit template
    cy.get('[data-cy="btn-create-template"]').click();

    // Verify API call was made
    cy.intercept('POST', '/template/create').as('createTemplate');
    cy.wait('@createTemplate').then((interception) => {
      expect(interception.response.statusCode).to.eq(200);
    });
  });

  it('should redirect to login when not authenticated', function () {
    // Visit template creation page without login
    cy.visit('/template/create');

    // Should redirect to login page
    cy.url().should('include', '/user/log-in');
  });

  it('should show error for missing required fields', function () {
    // Login first
    cy.login(this.users.validUser.email, this.users.validUser.password);

    // Visit template creation page
    cy.visit('/template/create');

    // Try to submit without filling required fields
    cy.get('[data-cy="btn-create-template"]').click();

    // Verify error message appears
    cy.get('[data-cy="server-responses"]').should('be.visible');
  });

  it('should show error for invalid topic', function () {
    // Login first
    cy.login(this.users.validUser.email, this.users.validUser.password);

    // Visit template creation page
    cy.visit('/template/create');

    // Set template title and description but leave topic as default (invalid)
    cy.get('[data-cy="template-title-input"]')
      .clear()
      .type('Test Template');

    cy.get('[data-cy="template-description-input"]')
      .clear()
      .type('Test description');

    // Submit without selecting valid topic
    cy.get('[data-cy="btn-create-template"]').click();

    // Verify error message appears
    cy.get('[data-cy="server-responses"]').should('be.visible');
  });
});
