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

describe('Template Like/Unlike', () => {
  const uniqueId = Date.now();
  const testEmail = `liketest${uniqueId}@example.com`;
  const testUsername = `liketestuser${uniqueId}`;
  const testPassword = 'TestPass123!';

  before(() => {
    cy.signup(testEmail, testPassword, testUsername);
  });

  it('should like a template when authenticated', function () {
    cy.intercept('GET', '/template/templates*', mockTemplates).as('getTemplates');
    cy.intercept('GET', '/template/get-template*', { statusCode: 200, body: mockTemplateDetail }).as('getTemplate');
    cy.intercept('GET', '/template/like*', { statusCode: 200, body: { data: 1 } }).as('likeTemplate');
    cy.intercept('GET', '/template/likes*', { statusCode: 200, body: { data: [] } }).as('getLikes');

    cy.login(testEmail, testPassword);
    cy.visit('/');
    cy.wait('@getTemplates');

    cy.get('[data-cy="home-templates-container"]').find('a').first().click();
    cy.wait('@getTemplate');

    cy.get('[data-cy="form-title"]').should('be.visible');

    cy.get('[data-cy="likes-number"]').then(($likes) => {
      const initialLikes = parseInt($likes.text()) || 0;
      cy.get('[data-cy="btn-like-template"]').click();
      cy.wait('@likeTemplate');
      cy.get('[data-cy="likes-number"]').should('contain', initialLikes + 1);
    });
  });

  it('should unlike a template when already liked', function () {
    cy.intercept('GET', '/template/templates*', mockTemplates).as('getTemplates');
    cy.intercept('GET', '/template/get-template*', { statusCode: 200, body: mockTemplateDetail }).as('getTemplate');
    cy.intercept('GET', '/template/like*', { statusCode: 200, body: { data: 0 } }).as('likeTemplate');
    cy.intercept('GET', '/template/likes*', { statusCode: 200, body: { data: [{ UserId: 999 }] } }).as('getLikes');

    cy.login(testEmail, testPassword);
    cy.visit('/');
    cy.wait('@getTemplates');

    cy.get('[data-cy="home-templates-container"]').find('a').first().click();
    cy.wait('@getTemplate');

    cy.get('[data-cy="form-title"]').should('be.visible');

    // Initial count is 1 (from getLikes mock)
    cy.get('[data-cy="likes-number"]').should('contain', '1');

    cy.get('[data-cy="btn-like-template"]').click();
    cy.wait('@likeTemplate');

    // After unlike, count should be 0
    cy.get('[data-cy="likes-number"]').should('contain', '0');
  });

  it('should show error when not authenticated', function () {
    cy.intercept('GET', '/template/templates*', mockTemplates).as('getTemplates');

    cy.visit('/');
    cy.wait('@getTemplates');

    cy.get('[data-cy="home-templates-container"]').find('a').first().click();
    cy.url().should('include', '/user/log-in');
  });

  it('should persist like across page refresh', function () {
    cy.intercept('GET', '/template/templates*', mockTemplates).as('getTemplates');
    cy.intercept('GET', '/template/get-template*', { statusCode: 200, body: mockTemplateDetail }).as('getTemplate');
    cy.intercept('GET', '/template/like*', { statusCode: 200, body: { data: 1 } }).as('likeTemplate');
    cy.intercept('GET', '/template/likes*', { statusCode: 200, body: { data: [{ UserId: 999 }] } }).as('getLikes');

    cy.login(testEmail, testPassword);
    cy.visit('/');
    cy.wait('@getTemplates');

    cy.get('[data-cy="home-templates-container"]').find('a').first().click();
    cy.wait('@getTemplate');

    cy.get('[data-cy="form-title"]').should('be.visible');

    cy.get('[data-cy="btn-like-template"]').click();
    cy.wait('@likeTemplate');

    cy.get('[data-cy="likes-number"]').then(($likes) => {
      const likedCount = parseInt($likes.text()) || 0;
      cy.reload();
      cy.wait('@getTemplate');
      cy.get('[data-cy="likes-number"]').should('contain', likedCount);
    });
  });

  it('should update like count in real-time', function () {
    cy.intercept('GET', '/template/templates*', mockTemplates).as('getTemplates');
    cy.intercept('GET', '/template/get-template*', { statusCode: 200, body: mockTemplateDetail }).as('getTemplate');
    cy.intercept('GET', '/template/like*', { statusCode: 200, body: { data: 1 } }).as('likeTemplate');
    cy.intercept('GET', '/template/likes*', { statusCode: 200, body: { data: [] } }).as('getLikes');

    cy.login(testEmail, testPassword);
    cy.visit('/');
    cy.wait('@getTemplates');

    cy.get('[data-cy="home-templates-container"]').find('a').first().click();
    cy.wait('@getTemplate');

    cy.get('[data-cy="form-title"]').should('be.visible');

    cy.get('[data-cy="likes-number"]').then(($likes) => {
      const initialLikes = parseInt($likes.text()) || 0;
      cy.get('[data-cy="btn-like-template"]').click();
      cy.wait('@likeTemplate');
      cy.get('[data-cy="likes-number"]').should('contain', initialLikes + 1);
    });
  });
});
