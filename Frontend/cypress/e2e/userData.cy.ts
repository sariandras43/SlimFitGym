describe('User data ', () => {
  describe('Initial State', () => {
    beforeEach(() => {
      cy.visit('');

      cy.viewport('macbook-15');
      cy.request('POST', 'http://localhost:5278/seed');
      cy.loginAsUser(); // Custom command to log in as a regular user
      cy.visit('/user/userData');
    });

    it('should display user name, email, and phone correctly', () => {
      cy.get('#name').should('have.value', 'pista');
      cy.get('#email').should('have.value', 'pista@gmail.com');
      cy.get('#phone').should('have.value', '+36123456788');
    });

    it('should show default profile image if no image is set', () => {
      cy.get('.profilePic img.default').should('be.visible');
    });

    it('should disable the modify button initially', () => {
      cy.get('.edit').should('have.class', 'disabled');
    });

    it('should display user role correctly', () => {
      cy.contains('Jelenlegi bérlet: 15 alkalmas bérlet').should('exist');
    });
  });
  describe('Edit User Data', () => {
    beforeEach(() => {
      cy.loginAsUser();
      cy.visit('/user/userData');
    });

    it('should enable modify button when fields change', () => {
      cy.get('#name').clear().type('Jane Doe');
      cy.get('.edit').should('not.have.class', 'disabled');
    });

    it('should persist updated name after submission', () => {
      cy.intercept('PUT', 'http://localhost:5278/api/auth/modify/3', {
        statusCode: 200,
      }).as('updateUser');
      cy.get('#name').clear().type('Jane Doe');
      cy.get('.edit').click();
      cy.wait('@updateUser')
        .its('request.body')
        .should('deep.include', { name: 'Jane Doe' });
      cy.get('#name').should('have.value', 'Jane Doe');
    });

    it('should show error for invalid email format', () => {
      cy.get('#email').clear().type('invalid-email');
      cy.get('.edit').click();
      cy.get('.error-message').should('contain', 'Érvénytelen formátumú email cím');
    });

    it('should validate phone number format', () => {
      cy.get('#phone').clear().type('123');
      cy.get('.edit').click();

      cy.get('.error-message').should('contain', 'Érvénytelen formátumú telefonszám');
    });
  });
  describe('Role-Specific Features', () => {
    it('should show "Apply for Trainer" button for regular users', () => {
      cy.loginAsUser();
      cy.visit('/user/userData');
      cy.get('app-apply-for-trainer').should('exist');
    });

    it('should display admin role correctly', () => {
      cy.loginAsAdmin();
      cy.visit('/user/userData');
      cy.contains('Jelenlegi szerep: Admin').should('exist');
    });
  });
});
