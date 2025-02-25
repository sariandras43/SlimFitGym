describe('Signup Form Validation (Tab off/Input)', () => {
  describe('Signup Form Validation frontend', () => {
    beforeEach(() => {
      // Visit the page containing the signup form
      cy.visit('http://localhost:4200/signup'); // Adjust the URL based on your app's route
    });
  
    it('should show an error message when the email is empty after tabbing off', () => {
      cy.get('#singUpEmail').clear(); // Clear the email input
      cy.get('#singUpEmail').blur(); // Simulate tabbing off by triggering blur
      cy.get('.errorField')
        .should('be.visible')
        .contains('Kérjük adja meg az email címét'); // Expected error message for empty email
    });
  
    it('should show an error message when the email is invalid after typing', () => {
      cy.get('#singUpEmail').clear().type('invalid-email'); // Type an invalid email
      cy.get('#singUpEmail').blur(); // Simulate tabbing off by triggering blur
      cy.get('.errorField')
        .should('be.visible')
        .contains('Kérjük adjon meg egy valós emailt'); // Expected error message for invalid email
    });
  
    it('should show an error message when the name is empty after tabbing off', () => {
      cy.get('#singUpName').clear(); // Clear the name input
      cy.get('#singUpName').blur(); // Simulate tabbing off by triggering blur
      cy.get('.errorField')
        .should('be.visible')
        .contains('Kérjük adja meg a nevét.'); // Expected error message for empty name
    });
  
    it('should show an error message when the name is too short after typing', () => {
      cy.get('#singUpName').clear().type('Jon'); // Type a short name
      cy.get('#singUpName').blur(); // Simulate tabbing off by triggering blur
      cy.get('.errorField')
        .should('be.visible')
        .contains('A név mezőnek legalább 4 karakterből kell állnia'); // Expected error message for short name
    });
  
    it('should show an error message when the phone is empty after tabbing off', () => {
      cy.get('#singUpPhone').clear(); // Clear the phone input
      cy.get('#singUpPhone').blur(); // Simulate tabbing off by triggering blur
      cy.get('.errorField')
        .should('be.visible')
        .contains('Kérjük adja meg a telefonszámát'); // Expected error message for empty phone
    });
  
    it('should show an error message when the phone is invalid after typing', () => {
      cy.get('#singUpPhone').clear().type('123'); // Type an invalid phone number
      cy.get('#singUpPhone').blur(); // Simulate tabbing off by triggering blur
      cy.get('.errorField')
        .should('be.visible')
        .contains('Kérjük adjon meg egy rendes telefonszámot (pl.:+36301234567)'); // Expected error message for invalid phone
    });
  
    it('should show an error message when the password is empty after tabbing off', () => {
      cy.get('#singUpPassword').clear(); // Clear the password input
      cy.get('#singUpPassword').blur(); // Simulate tabbing off by triggering blur
      cy.get('.errorField')
        .should('be.visible')
        .contains('Kérjük adjon meg egy biztonságos jelszavat.'); // Expected error message for empty password
    });
  
    it('should show an error message for weak password (no uppercase letter) after typing', () => {
      cy.get('#singUpPassword').clear().type('password1!'); // Type a weak password
      cy.get('#singUpPassword').blur(); // Simulate tabbing off by triggering blur
      cy.get('.errorField')
        .should('be.visible')
        .contains('A jelszóban legyen legalább egy nagybetű.'); // Expected error message for weak password (no uppercase)
    });
  
    it('should show an error message for weak password (no lowercase letter) after typing', () => {
      cy.get('#singUpPassword').clear().type('PASSWORD1!'); // Type a password with no lowercase
      cy.get('#singUpPassword').blur(); // Simulate tabbing off by triggering blur
      cy.get('.errorField')
        .should('be.visible')
        .contains('A jelszóban legyen legalább egy kisbetű.'); // Expected error message for weak password (no lowercase)
    });
  
    it('should show an error message for weak password (no number) after typing', () => {
      cy.get('#singUpPassword').clear().type('Password!'); // Type a password with no number
      cy.get('#singUpPassword').blur(); // Simulate tabbing off by triggering blur
      cy.get('.errorField')
        .should('be.visible')
        .contains('A jelszóban legyen legalább egy szám.'); // Expected error message for weak password (no number)
    });
  
    it('should show an error message for weak password (no special character) after typing', () => {
      cy.get('#singUpPassword').clear().type('Password1'); // Type a password with no special character
      cy.get('#singUpPassword').blur(); // Simulate tabbing off by triggering blur
      cy.get('.errorField')
        .should('be.visible')
        .contains('A jelszóban legyen legalább egy speciális karakter.'); // Expected error message for weak password (no special character)
    });
  
    it('should show an error message for weak password (too short) after typing', () => {
      cy.get('#singUpPassword').clear().type('Pwd1!'); // Type a password that's too short
      cy.get('#singUpPassword').blur(); // Simulate tabbing off by triggering blur
      cy.get('.errorField')
        .should('be.visible')
        .contains('A jelszóban legyen legalább 8 karakter hosszú.'); // Expected error message for short password
    });
  
    it('should show an error message when the confirm password is empty after tabbing off', () => {
      cy.get('#singUpPasswordAgain').clear(); // Clear the confirm password input
      cy.get('#singUpPasswordAgain').blur(); // Simulate tabbing off by triggering blur
      cy.get('.errorField')
        .should('be.visible')
        .contains('Kérjük adja meg újból a jelszavát!'); // Expected error message for empty confirm password
    });
  
    it('should show an error message when passwords do not match after typing', () => {
      cy.get('#singUpPassword').clear().type('ValidPassword1!'); // Type a valid password
      cy.get('#singUpPasswordAgain').clear().type('DifferentPassword1!'); // Type a non-matching password
      cy.get('#singUpPasswordAgain').blur(); // Simulate tabbing off by triggering blur
      cy.get('.errorField')
        .should('be.visible')
        .contains('A két jelszó nem egyezik meg'); // Expected error message for password mismatch
    });
  
    
  });
  
  
});
