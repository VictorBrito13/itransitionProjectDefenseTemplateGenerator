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

describe('Template Viewing', () => {
  beforeEach(() => {
    cy.fixture('users.json').as('users');
  });

  it('should display templates on home page', function () {
    cy.intercept('GET', '/template/templates*', {
      statusCode: 200,
      body: mockTemplates,
    }).as('getTemplates');

    cy.visit('/');

    cy.wait('@getTemplates');
    cy.get('[data-cy="home-templates-container"]').should('be.visible');
    cy.get('[data-cy="home-templates-header"]').should('contain', 'Latest Templates');
  });

  it('should open template details when clicking a template', function () {
    cy.intercept('GET', '/template/templates*', {
      statusCode: 200,
      body: mockTemplates,
    }).as('getTemplates');

    cy.login(this.users.validUser.email, this.users.validUser.password);
    cy.visit('/');

    cy.wait('@getTemplates');
    cy.get('[data-cy="home-templates-container"]').should('be.visible');

    cy.get('[data-cy="home-templates-container"]')
      .find('[data-cy="template-card-link"]')
      .first()
      .click();

    cy.get('[data-cy="form-title"]').should('be.visible');
    cy.get('[data-cy="form-description"]').should('be.visible');
  });

  it('should show template questions on template view', function () {
    cy.intercept('GET', '/template/templates*', {
      statusCode: 200,
      body: mockTemplates,
    }).as('getTemplates');

    cy.login(this.users.validUser.email, this.users.validUser.password);
    cy.visit('/');

    cy.wait('@getTemplates');
    cy.get('[data-cy="home-templates-container"]').should('be.visible');

    cy.get('[data-cy="home-templates-container"]')
      .find('[data-cy="template-card-link"]')
      .first()
      .click();

    cy.get('[data-cy="response-form"]').should('be.visible');
    cy.get('[data-cy="btn-submit-response"]').should('be.visible');
  });

  it('should redirect to login for unauthenticated template view', function () {
    cy.visit('/template/template');
    cy.url().should('include', '/user/log-in');
  });
});
