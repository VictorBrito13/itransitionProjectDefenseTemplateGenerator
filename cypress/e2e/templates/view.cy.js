const mockTemplates = {
  data: [
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
  ],
};

describe('Template Viewing', () => {
  const uniqueId = Date.now();
  const testEmail = `viewtest${uniqueId}@example.com`;
  const testUsername = `viewtestuser${uniqueId}`;
  const testPassword = 'TestPass123!';

  before(() => {
    cy.signup(testEmail, testPassword, testUsername);
  });

  it('should display templates on home page', function () {
    cy.intercept('GET', '/template/templates*', mockTemplates).as('getTemplates');

    cy.visit('/');
    cy.wait('@getTemplates');
    cy.get('[data-cy="home-templates-container"]').should('be.visible');
  });

  it('should open template details when clicking a template', function () {
    cy.intercept('GET', '/template/templates*', mockTemplates).as('getTemplates');

    cy.login(testEmail, testPassword);
    cy.visit('/');
    cy.wait('@getTemplates');

    cy.get('[data-cy="home-templates-container"]')
      .find('a')
      .first()
      .click();

    cy.get('[data-cy="form-title"]').should('be.visible');
  });

  it('should show template questions on template view', function () {
    cy.intercept('GET', '/template/templates*', mockTemplates).as('getTemplates');

    cy.login(testEmail, testPassword);
    cy.visit('/');
    cy.wait('@getTemplates');

    cy.get('[data-cy="home-templates-container"]')
      .find('a')
      .first()
      .click();

    cy.get('[data-cy="response-form"]').should('be.visible');
  });

  it('should redirect to login for unauthenticated template view', function () {
    cy.visit('/template/template');
    cy.url().should('include', '/user/log-in');
  });
});
