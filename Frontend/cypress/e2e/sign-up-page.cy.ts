describe('template spec', () => {
  it('létezik az email mező', () => {
    cy.visit('http://localhost:4200/signup')
    
    cy.get('#singUpEmail')
  })
  it('létezik a név mező', () => {
    cy.visit('http://localhost:4200/signup')
    
    cy.get('#singUpName')
  })
  it('létezik a telefonszám mező', () => {
    cy.visit('http://localhost:4200/signup')
    
    cy.get('#singUpPhone')
  })
  it('létezik a jelszó mező', () => {
    cy.visit('http://localhost:4200/signup')
    
    cy.get('#singUpPassword')
  })
  it('létezik a jelszó újra mező', () => {
    cy.visit('http://localhost:4200/signup')
    
    cy.get('#singUpPasswordAgain')
  })
})