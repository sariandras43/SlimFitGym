describe('Admin CMS - Passes Management', () => {
  const testPasses = [
    { name: '15 alkalmas bérlet', entries: 15, price: 10000, type: 'entries' },
    { name: 'Negyedéves', days: 90, price: 40000, type: 'days' },
    { name: 'Éves bérlet', days: 365, price: 120000, type: 'days' },
    { name: 'Heti bérlet', days: 7, entries: 7, price: 5000, type: 'both' },
  ];

  beforeEach(() => {
    cy.visit('');
    cy.loginAsAdmin();
    cy.request('POST', "http://localhost:5278/seed"); 
    cy.visit('/user/passes', { timeout: 10000 });
    cy.get('table').should('be.visible');
  });

  describe('Initial State Verification', () => {
    it('should display active passes with correct data', () => {
      cy.get('table tr:not(.deleted)').should('have.length', testPasses.length + 1);
      
      testPasses.forEach(pass => {
        cy.contains('tr:not(.deleted)', pass.name).within(() => {
          if (pass.type === 'days') {
            cy.get('[data-cell="Napok"]').should('contain', pass.days);
            cy.get('[data-cell="Max. belépések"]').should('contain', '-');
          }
          if (pass.type === 'entries') {
            cy.get('[data-cell="Max. belépések"]').should('contain', pass.entries);
            cy.get('[data-cell="Napok"]').should('contain', '-');
          }
          if (pass.type === 'both') {
            cy.get('[data-cell="Napok"]').should('contain', pass.days);
            cy.get('[data-cell="Max. belépések"]').should('contain', pass.entries);
          }
          cy.get('[data-cell="Ár"]').should('contain', pass.price);
        });
      });
    });

    it('should hide deleted passes by default', () => {
      cy.contains('tr:not(.deleted)', 'Havi').should('not.exist');
      cy.get('tr.deleted').should('not.exist');
    });
  });

  describe('Sorting Functionality', () => {
    const verifySort = (column: string, values: any[]) => {
      cy.get(`[data-cell="${column}"]`).then($cells => {
        const actual = $cells.map((_, el) => el.innerText).get();
        expect(actual).to.deep.equal(values);
      });
    };

    it('should sort by price ascending/descending', () => {
      cy.get('th:contains("Ár")').click();
      verifySort('Ár', ['5000 Ft', '10000 Ft', '40000 Ft', '120000 Ft']);
      
      cy.get('th:contains("Ár")').click();
      verifySort('Ár', ['120000 Ft', '40000 Ft', '10000 Ft', '5000 Ft']);
    });

    it('should sort by days descending', () => {
      cy.get('th:contains("Napok")').click().click();
      verifySort('Napok', ['365', '90', '7', '-']);
    });

    it('should show correct sort indicators', () => {
      cy.get('th:contains("Név")').click();
      cy.get('th:contains("Név")').should('contain', '⬆');
      
      cy.get('th:contains("Név")').click();
      cy.get('th:contains("Név")').should('contain', '⬇');
    });
  });

  describe('CRUD Operations', () => {
    it('should create new pass with valid data', () => {
      cy.get('.newButton').click();
      cy.get('#passNameInput').type('Premium Pass');
      cy.get('#priceInput').type('25000');
      cy.get('#daysInput').type('60');
      cy.get('.save-button').click();
      cy.contains('tr', 'Premium Pass').should('be.visible');
    });

    it('should update existing pass', () => {
      cy.contains('tr', 'Heti bérlet').within(() => {
        cy.get('button[aria-label="update"]').click();
      });
      cy.get('#passNameInput').clear().type('Weekly Pro');
      cy.get('.save-button').click();
      cy.contains('tr', 'Weekly Pro').should('be.visible');
    });

    it('should soft-delete pass', () => {
      cy.contains('tr', '15 alkalmas bérlet').within(() => {
        cy.get('button[aria-label="delete"]').click();
      });
      cy.get('.toggle-slider').click();
      cy.contains('tr.deleted', '15 alkalmas bérlet').should('be.visible');
    });
  });

  describe('Form Validation', () => {
    it('should require mandatory fields', () => {
      cy.get('.newButton').click();
      cy.get('.save-button').click();
      cy.get('.error-message').should('contain', 'A név kötelező');
      cy.get('.error-message').should('contain', 'Az ár kötelező');
    });

    it('should prevent zero days/entries combination', () => {
      cy.get('.newButton').click();
      cy.get('#passNameInput').type('Invalid Pass');
      cy.get('#priceInput').type('10000');
      cy.get('.save-button').click();
      cy.get('.bottom-error').should('contain', 'nap');
      cy.get('.bottom-error').should('contain', 'belépés');
    });

    it('should handle negative values', () => {
      cy.get('.newButton').click();
      cy.get('#priceInput').type('-5000');
      cy.get('#daysInput').type('30');
      cy.get('.save-button').click();
      cy.get('.error-message').should('contain', 'Az ár kötelező');
    });
  });

  describe('Deleted Passes Management', () => {
    beforeEach(() => {
      cy.contains('tr', '15 alkalmas bérlet').within(() => {
        cy.get('button[aria-label="delete"]').click();
      });
    });

    it('should toggle deleted passes visibility', () => {
      cy.get('.toggle-slider').click();
      cy.get('tr.deleted').should('have.length.at.least', 2);
      cy.get('.toggle-slider').click();
      cy.get('tr.deleted').should('not.exist');
    });

    it('should prevent editing deleted passes', () => {
      cy.get('.toggle-slider').click();
      cy.contains('tr.deleted', '15 alkalmas bérlet').within(() => {
        cy.get('button[aria-label="update"]').should('not.exist');
      });
    });
  });

  describe('Search Functionality', () => {
    it('should filter by name partial match', () => {
      cy.get('.searchBar').type('éves');
      cy.get('tr:not(.deleted) [data-cell="Név"]').should('have.length', 2);
    });

    it('should be case-insensitive', () => {
      cy.get('.searchBar').type('NEgyEdÉvES');
      cy.contains('tr:not(.deleted)', 'Negyedéves').should('be.visible');
    });

    it('should search benefits content', () => {
      cy.get('.newButton').click();
      cy.get('#passNameInput').type('Special Pass');
      cy.get('#daysInput').type('60');

      cy.get('#priceInput').type('15000');
      cy.get('.add-benefit').click();
      cy.get('.benefit-item input').type('Exclusive Access');
      cy.get('.save-button').click();
      
      cy.get('.searchBar').type('Exclusive');
      cy.contains('tr', 'Special Pass').should('be.visible');
    });
  });

  describe('Benefits Management', () => {
    it('should add/remove benefits', () => {
      cy.get('.newButton').click();
      cy.get('#passNameInput').type('Benefit Test');
      cy.get('#priceInput').type('15000');
      cy.get('#daysInput').type('60');
      
      // Add benefits
      cy.get('.add-benefit').click().click();
      cy.get('.benefit-item input').first().type('Pool Access');
      cy.get('.benefit-item input').eq(1).type('Sauna Access');
      
      // Remove one benefit
      cy.get('.benefit-item button').first().click();
      cy.get('.save-button').click();
      
      cy.contains('tr', 'Benefit Test').within(() => {
        cy.get('[data-cell="Előnyök"]').should('contain', 'Sauna Access');
        cy.get('[data-cell="Előnyök"]').should('not.contain', 'Pool Access');
      });
    });
  });

  describe('Edge Cases', () => {
    it('should persist data after refresh', () => {
      cy.contains('tr', 'Éves bérlet').within(() => {
        cy.get('button[aria-label="update"]').click();
      });
      cy.get('#passNameInput').clear().type('Annual Premium');
      cy.get('.save-button').click();
      cy.reload();
      cy.contains('tr', 'Annual Premium').should('be.visible');
    });

    it('should handle concurrent deletions', () => {
      // Delete multiple passes
      cy.contains('tr', 'Negyedéves').within(() => {
        cy.get('button[aria-label="delete"]').click();
      });
      cy.contains('tr', 'Heti bérlet').within(() => {
        cy.get('button[aria-label="delete"]').click();
      });
      
      cy.get('.toggle-slider').click();
      cy.get('tr.deleted').should('have.length', 3); // 2 new + 1 seeded
    });

    it('should handle maximum value inputs', () => {
      cy.get('.newButton').click();
      cy.get('#passNameInput').type('Max Values Pass');
      cy.get('#priceInput').type('9999999');
      cy.get('#daysInput').type('36500');
      cy.get('#maxEntriesInput').type('1000');
      cy.get('.save-button').click();
      
      cy.contains('tr', 'Max Values Pass').within(() => {
        cy.get('[data-cell="Ár"]').should('contain', '9999999');
        cy.get('[data-cell="Napok"]').should('contain', '36500');
        cy.get('[data-cell="Max. belépések"]').should('contain', '1000');
      });
    });
  });
});

describe('Admin CMS - User Management', () => {
  const seededUsers = [
    { 
      name: 'admin', 
      email: 'admin@gmail.com',
      role: 'admin',
      phone: '+36123456789',
      hungarianRole: 'admin' // No translation in userInHungarian()
    },
    { 
      name: 'kazmer', 
      email: 'kazmer@gmail.com',
      role: 'trainer',
      phone: '+36123456799',
      hungarianRole: 'Edző'
    },
    { 
      name: 'pista', 
      email: 'pista@gmail.com',
      role: 'user',
      phone: '+36123456788',
      hungarianRole: 'Felhasználó'
    },
    { 
      name: 'ica', 
      email: 'ica@gmail.com',
      role: 'employee',
      phone: '+36126456788',
      hungarianRole: 'Dolgozó'
    }
  ];

  beforeEach(() => {
    cy.request('POST', "http://localhost:5278/seed");
    cy.loginAsAdmin();
    cy.visit('/user/users', { timeout: 10000 });
    cy.get('table').should('be.visible');
  });

  describe('Initial State Verification', () => {
    it('should display all seeded users with correct data', () => {
      cy.get('table tr:not(.deleted)').should('have.length', seededUsers.length + 1); // +1 for header
      
      seededUsers.forEach(user => {
        cy.contains('tr:not(.deleted)', user.name).within(() => {
          cy.get('[data-cell="Email"]').should('contain', user.email);
          cy.get('[data-cell="Telefonszám"]').should('contain', user.phone);
          cy.get('[data-cell="Szerep"]').should('contain', user.hungarianRole);
        });
      });
    });

    it('should show admin role untranslated', () => {
      cy.contains('tr', 'admin').within(() => {
        cy.get('[data-cell="Szerep"]').should('contain', 'admin');
      });
    });
  });

  describe('Role Specific Features', () => {
    it('should show trainer approval buttons only for applicants', () => {
      cy.contains('tr', 'kazmer').within(() => {
        cy.get('button.success').should('not.exist');
        cy.get('button.danger').should('not.exist');
      });
    });

    it('should prevent deleting admin account', () => {
      cy.contains('tr', 'admin').within(() => {
        cy.get('button[aria-label="delete"]').should('not.exist');
      });
    });
  });

  describe('User Actions', () => {
    it('should delete regular user', () => {
      cy.contains('tr', 'pista').within(() => {
        cy.get('button[aria-label="delete"]').click();
      });
      cy.contains('tr', 'pista').should('not.exist');
    });

    it('should toggle deleted users visibility', () => {
      // Delete a user first
      cy.contains('tr', 'pista').within(() => {
        cy.get('button[aria-label="delete"]').click();
      });
      
      cy.get('.toggle-slider').click();
      cy.get('tr.deleted').should('contain', 'pista');
      cy.get('.toggle-slider').click();
      cy.get('tr.deleted').should('not.exist');
    });
  });

  describe('Search Functionality', () => {
    it('should filter by partial phone number', () => {
      cy.get('.searchBar').type('612345');
      cy.get('tr:not(.deleted)').should('have.length', 4); // header + admin, kazmer, pista
    });

    it('should search by role translation', () => {
      cy.get('.searchBar').type('Edző');
      cy.contains('tr', 'kazmer').should('be.visible');
      cy.get('tr:not(.deleted)').should('have.length', 2); // header + kazmer
    });
  });
});