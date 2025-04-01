describe('Admin CMS - Passes Management', () => {
  const testPasses = [
    { name: 'Havi', days: 30, price: 15000, type: 'days' },
    { name: '15 alkalmas bérlet', entries: 15, price: 10000, type: 'entries' },
    { name: 'Negyedéves', days: 90, price: 40000, type: 'days' },
    { name: 'Éves bérlet', days: 365, price: 120000, type: 'days' },
    { name: 'Heti bérlet', days: 7, entries: 7, price: 5000, type: 'both' },
  ];

  beforeEach(() => {
    cy.loginAsAdmin();
    cy.visit('/user/passes', { timeout: 10000 });
    cy.get('table').should('be.visible');
  });


  it('should sort by different columns', () => {
   
    cy.get('th').contains('Ár').click();
    
    cy.get('tr:not(.deleted) [data-cell="Ár"]').then($cells => {
      const prices = $cells.map((_, el) => {
        const priceText = el.innerText.replace(/\s/g, '');
        return parseInt(priceText);
      }).get();
  
      const expectedPrices = [5000, 10000, 15000, 40000, 120000];
      expect(prices).to.deep.equal(expectedPrices);
    });
  
    cy.get('th').contains('Napok').click().click();
    cy.get('tr:not(.deleted) [data-cell="Napok"]').then($cells => {
      const days = $cells.map((_, el) => {
        const dayText = el.innerText;
        return dayText === '-' ? 0 : parseInt(dayText);
      }).get();
  
      const expectedDays = [365, 90, 30, 7, 0];
      expect(days).to.deep.equal(expectedDays);
    });
  });
  it('should sort by entries and highlighted status', () => {
    // Sort by entries
    cy.get('th').contains('Max. belépések').click().click();
    cy.get('[data-cell="Max. belépések"]').then($cells => {
      const entries = $cells.map((_, el) => parseInt(el.innerText) || 0).get();
      expect(entries).to.deep.equal([15, 7, 0, 0, 0]);
    });

    // Sort by highlighted
    cy.get('th').contains('Kiemelt').click();
    cy.get('[data-cell="Kiemelt"]').then($cells => {
      const values = $cells.map((_, el) => el.innerText).get();
      expect(values).to.deep.equal(['Nem Kiemelt', 'Nem Kiemelt', 'Nem Kiemelt', 'Nem Kiemelt', 'Nem Kiemelt']);
    });
  });
  it('should display initial passes with correct data', () => {
    cy.get('table tr:not(.deleted)').should(
      'have.length',
      testPasses.length + 1
    ); // +1 for header

    testPasses.forEach((pass) => {
      cy.contains('tr:not(.deleted)', pass.name).within(() => {
        if (pass.type === 'days') {
          cy.get('[data-cell="Napok"]').should('contain', pass.days);
          cy.get('[data-cell="Max. belépések"]').should('contain', '-');
        }
        if (pass.type === 'entries') {
          cy.get('[data-cell="Max. belépések"]').should(
            'contain',
            pass.entries
          );
          cy.get('[data-cell="Napok"]').should('contain', '-');
        }
        if (pass.type === 'both') {
          cy.get('[data-cell="Napok"]').should('contain', pass.days);
          cy.get('[data-cell="Max. belépések"]').should(
            'contain',
            pass.entries
          );
        }
        cy.get('[data-cell="Ár"]').should('contain', pass.price.toString());
      });
    });
  });

  it('should validate day/entry requirements', () => {
    cy.get('.newButton').click();

    // Fill required fields first
    cy.get('#passNameInput').type('Test Pass');
    cy.get('#priceInput').type('10000');

    cy.get('#daysInput').clear();
    cy.get('#maxEntriesInput').clear();
    cy.get('.save-button').click();

    cy.get('.bottom-error')
      .should('be.visible')
      .and(
        'contain.text',
        'Kötelező megadni legalább a maximum belépések számát vagy a felhasználható napok értékét'
      );

    cy.get('#daysInput').type('30');
    cy.get('.save-button').click();
    cy.get('.bottom-error').should('not.exist');

    cy.get('.newButton').click();
    cy.get('#passNameInput').type('Test Pass 2');
    cy.get('#priceInput').type('15000');

    cy.get('#maxEntriesInput').type('10');
    cy.get('.save-button').click();
    cy.get('.bottom-error').should('not.exist');
  });

  

  it('should handle complex pass combinations', () => {
    // Test creating pass with both days and entries
    cy.get('.newButton').click();
    cy.get('#passNameInput').type('Kombinált bérlet');
    cy.get('#priceInput').type('20000');
    cy.get('#daysInput').type('30');
    cy.get('#maxEntriesInput').type('20');
    cy.get('.save-button').click();

    cy.contains('tr', 'Kombinált bérlet').within(() => {
      cy.get('[data-cell="Napok"]').should('contain', '30');
      cy.get('[data-cell="Max. belépések"]').should('contain', '20');
    });
  });

  
  it('should filter passes using search functionality', () => {
    // Test name search
    cy.get('.searchBar').type('havi');
    cy.get('tr:not(.deleted) [data-cell="Név"]').should('contain', 'Havi');
    cy.get('tr:not(.deleted)').should('have.length', 2);

    // Test benefit search
    // cy.get('.searchBar').clear().type('előny');
    // cy.get('[data-cell="Előnyök"]').first().should('not.contain', '-');
  });
  it('should maintain data integrity after edit', () => {
    cy.contains('tr', 'Heti bérlet').within(() => {
      cy.get('button[aria-label="update"]').click();
    });

    // Change from weekly to monthly
    cy.get('#passNameInput').clear().type('Havi bérlet');
    cy.get('#daysInput').clear().type('30');
    cy.get('#maxEntriesInput').clear();
    cy.get('.save-button').click();

    cy.get('table').should('contain', 'Havi bérlet');
    cy.contains('tr', 'Havi bérlet').within(() => {
      cy.get('[data-cell="Napok"]').should('contain', '30');
      cy.get('[data-cell="Max. belépések"]').should('contain', '-');
    });
  });
  it('should toggle deleted passes visibility', () => {
    // Delete a pass first
    cy.contains('tr', '15 alkalmas bérlet').within(() => {
      cy.get('button[aria-label="delete"]').click();
    });

    // Verify toggle
    cy.get('.toggle-slider').click();
    cy.get('tr.deleted').should('be.visible');
    cy.get('.toggle-slider').click();
    cy.get('tr.deleted').should('not.exist');
  });

  it('should validate required fields in form', () => {
    cy.get('.newButton').click();
    
    // Test empty name
    cy.get('#priceInput').type('10000');
    cy.get('#daysInput').type('30');
    cy.get('.save-button').click();
    cy.get('.error-message').should('contain', 'A név kötelező');

    // Test invalid price
    cy.get('#passNameInput').type('Test Pass');
    cy.get('#priceInput').clear().type('0');
    cy.get('.save-button').click();
    cy.get('.error-message').should('contain', 'Az ár kötelező');
  });

  it('should handle benefits management', () => {
    cy.get('.newButton').click();
    cy.get('#passNameInput').type('Benefit Test');
    cy.get('#priceInput').type('15000');
    cy.get('#daysInput').type('30');

    // Add benefits
    cy.get('.add-benefit').click();
    cy.get('.benefit-item input').first().type('Uszkve');
    cy.get('.add-benefit').click();
    cy.get('.benefit-item input').eq(1).type('Másik előny');
    
    // Remove one benefit
    cy.get('.benefit-item button').first().click();
    cy.get('.save-button').click();

    // Verify in table
    cy.contains('tr', 'Benefit Test').within(() => {
      cy.get('[data-cell="Előnyök"]').should('contain', 'Másik előny');
      cy.get('[data-cell="Előnyök"]').should('not.contain', 'Uszkve');
    });
  });

 

  it('should cancel modal without saving', () => {
    cy.get('.newButton').click();
    cy.get('#passNameInput').type('Temp Pass');
    cy.get('.cancel-button').click();
    cy.contains('Temp Pass').should('not.exist');
  });

  // it('should handle server errors gracefully', () => {
  //   cy.intercept('POST', '/api/passes', {
  //     statusCode: 500,
  //     body: { message: 'Server error' }
  //   });

  //   cy.get('.newButton').click();
  //   cy.get('#passNameInput').type('Error Test');
  //   cy.get('#priceInput').type('10000');
  //   cy.get('#daysInput').type('30');
  //   cy.get('.save-button').click();

  //   cy.get('.bottom-error').should('contain', 'Hiba történt');
  // });

  it('should edit and persist highlighted status', () => {
    cy.contains('tr', 'Éves bérlet').within(() => {
      cy.get('button[aria-label="update"]').click();
    });

    cy.get('.toggleFields > .toggle > .toggle-slider').click();
    cy.get('.save-button').click();

    cy.contains('tr', 'Éves bérlet').within(() => {
      cy.get('[data-cell="Kiemelt"]').should('contain', 'Kiemelt');
    });
  });

});
