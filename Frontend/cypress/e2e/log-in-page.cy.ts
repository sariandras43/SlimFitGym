describe('Signup Form Validation (Tab off/Input)', () => {
  describe('Signup Form Validation frontend', () => {
    beforeEach(() => {
      // Visit the page containing the signup form
      cy.visit('/login'); // Adjust the URL based on your app's route
    });
    it('login should redirect to user page if correct password and email', () => {
      cy.get('#loginEmail').clear().type('admin@gmail.com'); 
      cy.get('#logInPassword').clear().type('admin'); 
      cy.get('[data-cy="loginButton"]').click();
      cy.url().should('eq', 'http://localhost:4200/user/userData');

    });
    it('error should be thrown if email or password not added', () => {
      cy.get('[data-cy="loginButton"]').click();
      cy.get('.alert').should('be.visible')
      .contains('Email cím és jelszó megadása kötelező!'); 
    });
    it('error should be thrown if bad password', () => {
      cy.get('#loginEmail').clear().type('admin@gmail.com'); 
      cy.get('#logInPassword').clear().type('bad-password'); 
      cy.get('[data-cy="loginButton"]').click();
      cy.get('[data-cy="errorMessage"]').should('be.visible')
      .contains('Helytelen email cím vagy jelszó.'); 
    });
  
    
  });
  
  
});
