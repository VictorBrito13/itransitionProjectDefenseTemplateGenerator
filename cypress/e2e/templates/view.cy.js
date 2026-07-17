const mockTemplateDetail = {
  data: {
    TemplateId: 1,
    Title: 'Test Template',
    Description: 'A test template for E2E testing',
    TopicId: 1,
    Topic: { Name: 'General' },
    Admins: [{ User: { Username: 'testuser', UserId: 1 } }],
    Likes: [],
    Questions: [
      { QuestionId: 1, QuestionType: '1', Label: 'Your name', QuestionTemplateId: 1 },
    ],
  },
};

const mockTemplates = {
  data: [mockTemplateDetail.data],
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
    cy.intercept('GET', '/template/get-template*', { statusCode: 200, body: mockTemplateDetail }).as('getTemplate');
    cy.intercept('GET', '/template/likes*', { statusCode: 200, body: { data: [] } }).as('getLikes');

    cy.login(testEmail, testPassword);
    cy.visit('/');
    cy.wait('@getTemplates');

    cy.get('[data-cy="home-templates-container"]').find('a').first().click();
    cy.wait('@getTemplate');

    cy.get('[data-cy="form-title"]').should('be.visible');
    cy.get('[data-cy="form-description"]').should('be.visible');
  });

  it('should show template questions on template view', function () {
    cy.intercept('GET', '/template/templates*', mockTemplates).as('getTemplates');
    cy.intercept('GET', '/template/get-template*', { statusCode: 200, body: mockTemplateDetail }).as('getTemplate');
    cy.intercept('GET', '/template/likes*', { statusCode: 200, body: { data: [] } }).as('getLikes');

    cy.login(testEmail, testPassword);
    cy.visit('/');
    cy.wait('@getTemplates');

    cy.get('[data-cy="home-templates-container"]').find('a').first().click();
    cy.wait('@getTemplate');

    cy.get('[data-cy="response-form"]').should('be.visible');
    cy.get('[data-cy="btn-submit-response"]').should('be.visible');
  });

  it('should redirect to login for unauthenticated template view', function () {
    cy.visit('/template/template');
    cy.url().should('include', '/user/log-in');
  });
});
