describe('Signup Form Validation (Tab off/Input)', () => {
  describe('Signup Form Validation frontend', () => {
    beforeEach(() => {
      // Visit the page containing the signup form
      cy.visit('http://localhost:4200/login'); // Adjust the URL based on your app's route
    });
    it('should show an error message when the email is empty after tabbing off', () => {
      cy.get('#loginEmail').clear().type('admin@gmail.com'); 
      cy.get('#logInPassword').clear().type('admin'); 
      cy.get('button').click();
    });
    it('should show an error message when the email is empty after tabbing off', () => {
      cy.get('button').click();
      cy.get('.alert').should('be.visible')
      .contains('Email cím és jelszó megadása kötelező!'); 
    });
  
    
  });
  
  
});
