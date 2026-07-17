describe('Template Creation', () => {
  beforeEach(() => {
    cy.fixture('users.json').as('users');
    cy.fixture('templates.json').as('templates');
  });

  it('should create a new template when authenticated', function () {
    cy.login(this.users.validUser.email, this.users.validUser.password);

    cy.intercept('POST', '/template/create').as('createTemplate');
    cy.visit('/template/create');

    cy.get('[data-cy="page-title"]').should('contain', 'Create a new template');

    cy.get('[data-cy="template-topic-select"]').select(this.templates.validTemplate.topicId);

    cy.get('[data-cy="template-title-input"]')
      .clear()
      .type(this.templates.validTemplate.title);

    cy.get('[data-cy="template-description-input"]')
      .clear()
      .type(this.templates.validTemplate.description);

    cy.get('[data-cy="add-single-line-btn"]').click();

    cy.get('[data-cy="btn-create-template"]').click();

    cy.wait('@createTemplate');
  });

  it('should redirect to login when not authenticated', function () {
    cy.visit('/template/create');
    cy.url().should('include', '/user/log-in');
  });

  it('should show error for missing required fields', function () {
    cy.login(this.users.validUser.email, this.users.validUser.password);
    cy.visit('/template/create');

    cy.get('[data-cy="btn-create-template"]').click();

    cy.get('[data-cy="server-responses"]').should('be.visible');
  });

  it('should show error for invalid topic', function () {
    cy.login(this.users.validUser.email, this.users.validUser.password);
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
