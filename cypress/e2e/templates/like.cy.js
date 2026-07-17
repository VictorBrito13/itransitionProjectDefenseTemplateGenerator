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

describe('Template Like/Unlike', () => {
  beforeEach(() => {
    cy.fixture('users.json').as('users');
  });

  it('should like a template when authenticated', function () {
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

    cy.get('[data-cy="likes-number"]').then(($likes) => {
      const initialLikes = parseInt($likes.text()) || 0;

      cy.intercept('GET', '/template/like*').as('likeTemplate');
      cy.get('[data-cy="btn-like-template"]').click();
      cy.wait('@likeTemplate');

      cy.get('[data-cy="likes-number"]').should('contain', initialLikes + 1);
    });
  });

  it('should unlike a template when already liked', function () {
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

    cy.intercept('GET', '/template/like*').as('likeTemplate');

    cy.get('[data-cy="btn-like-template"]').click();
    cy.wait('@likeTemplate');

    cy.get('[data-cy="likes-number"]').then(($likes) => {
      const likedCount = parseInt($likes.text()) || 0;

      cy.get('[data-cy="btn-like-template"]').click();
      cy.wait('@likeTemplate');

      cy.get('[data-cy="likes-number"]').should('contain', likedCount - 1);
    });
  });

  it('should show error when not authenticated', function () {
    cy.intercept('GET', '/template/templates*', {
      statusCode: 200,
      body: mockTemplates,
    }).as('getTemplates');

    cy.visit('/');

    cy.wait('@getTemplates');
    cy.get('[data-cy="home-templates-container"]').should('be.visible');

    cy.get('[data-cy="home-templates-container"]')
      .find('[data-cy="template-card-link"]')
      .first()
      .click();

    cy.url().should('include', '/user/log-in');
  });

  it('should persist like across page refresh', function () {
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

    cy.intercept('GET', '/template/like*').as('likeTemplate');
    cy.get('[data-cy="btn-like-template"]').click();
    cy.wait('@likeTemplate');

    cy.get('[data-cy="likes-number"]').then(($likes) => {
      const likedCount = parseInt($likes.text()) || 0;

      cy.reload();

      cy.get('[data-cy="likes-number"]').should('contain', likedCount);
    });
  });

  it('should update like count in real-time', function () {
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

    cy.get('[data-cy="likes-number"]').then(($likes) => {
      const initialLikes = parseInt($likes.text()) || 0;

      cy.intercept('GET', '/template/like*').as('likeTemplate');
      cy.get('[data-cy="btn-like-template"]').click();
      cy.wait('@likeTemplate');

      cy.get('[data-cy="likes-number"]').should('contain', initialLikes + 1);
    });
  });
});
