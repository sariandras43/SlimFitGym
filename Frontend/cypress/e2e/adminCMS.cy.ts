describe('Admin CMS - Passes Management', () => {
  const testPasses = [
    { name: '15 alkalmas bérlet', entries: 15, price: 10000, type: 'entries' },
    { name: 'Negyedéves', days: 90, price: 40000, type: 'days' },
    { name: 'Éves bérlet', days: 365, price: 120000, type: 'days' },
    { name: 'Heti bérlet', days: 7, entries: 7, price: 5000, type: 'both' },
  ];

  beforeEach(() => {
    cy.viewport('macbook-15')  

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
    cy.viewport('macbook-15')  

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

describe('Admin CMS - Machines Management', () => {
  const seededMachines = [
    { name: 'Bicikli', description: null },
    { name: 'Futópad', description: null },
    { name: 'Elliptikus tréner', description: null },
    { name: 'Evezőgép', description: 'Professzionális evezőgép' },
    { name: 'Súlyzópad', description: 'Állítható dőlésszögű súlyzópad' },
    { name: 'Lépcsőzőgép', description: 'Intenzív kardió edzéshez' },
    { name: 'Mellprés gép', description: 'Mellizom erősítésére' },
    { name: 'Hasprés gép', description: 'Hasizom erősítésére' },
    { name: 'Guggoló állvány', description: 'Guggolás gyakorlására' },
    { name: 'Hátlehúzó gép', description: 'Hátizom edzésére' }
  ];

  beforeEach(() => {
    cy.viewport('macbook-15')  

    cy.request('POST', "http://localhost:5278/seed"); 
    cy.loginAsAdmin();
    cy.visit('user/machines', { timeout: 10000 });

    cy.get('table').should('be.visible');
  });

  describe('Initial State Verification', () => {
    it('should display all seeded machines with correct data', () => {
      cy.get('table tr:not(.deleted)').should('have.length', seededMachines.length + 1); // +1 for header
      
      seededMachines.forEach(machine => {
        cy.contains('tr:not(.deleted)', machine.name).within(() => {
          if (machine.description) {
            cy.get('[data-cell="Leírás"]').should('contain', machine.description);
          } else {
            cy.get('[data-cell="Leírás"]').should('be.empty');
          }
        });
      });
    });
  });

  describe('Sorting Functionality', () => {
    const verifySortOrder = (column: string, order: string[]) => {
      cy.get(`[data-cell="${column}"]`).then($cells => {
        const actual = $cells.map((_, el) => el.innerText).get();
        expect(actual).to.deep.equal(order);
      });
    };

    it('should sort by name ascending/descending', () => {
      const expectedAsc = [
        'Bicikli',
        'Elliptikus tréner',
        'Evezőgép',
        'Futópad',
        'Guggoló állvány',
        'Hasprés gép',
        'Hátlehúzó gép',
        'Lépcsőzőgép',
        'Mellprés gép',
        'Súlyzópad'
      ];

      cy.get('th:contains("Név")').click();
      verifySortOrder('Név', expectedAsc);
      
      cy.get('th:contains("Név")').click();
      verifySortOrder('Név', [...expectedAsc].reverse());
    });

    it('should sort by description with null values first', () => {
      cy.get('th:contains("Leírás")').click();
      cy.get('[data-cell="Leírás"]').first().should('be.empty');
    });
  });

  describe('Search Functionality', () => {
    it('should filter by machine name substring', () => {
      cy.get('.searchBar').type('gép');
      cy.get('[data-cell="Név"]').should('have.length', 5); // Evezőgép, Lépcsőzőgép, Mellprés gép, Hasprés gép, Hátlehúzó gép
    });

    it('should find machines by description content', () => {
      cy.get('.searchBar').type('kardió');
      cy.contains('tr', 'Lépcsőzőgép').should('be.visible');
    });
  });

  describe('CRUD Operations', () => {
    const newMachine = {
      name: 'Új Géptípus',
      description: 'Teszt leírás'
    };

    it('should create new machine', () => {
      cy.get('.newButton').click();
      cy.get('#machineNameInput').type(newMachine.name);
      cy.get('#machineDescriptionInput').type(newMachine.description);
      cy.get('.save-button').click();
      
      cy.contains('tr', newMachine.name).within(() => {
        cy.get('[data-cell="Leírás"]').should('contain', newMachine.description);
      });
    });

    it('should update existing machine description', () => {
      cy.contains('tr', 'Bicikli').within(() => {
        cy.get('button').first().click();
      });
      
      cy.get('#machineDescriptionInput').type(' új leírás');
      cy.get('.save-button').click();
      
      cy.contains('tr', 'Bicikli').within(() => {
        cy.get('[data-cell="Leírás"]').should('contain', ' új leírás');
      });
    });

    it('should delete machine', () => {
      cy.contains('tr', 'Futópad').within(() => {
        cy.get('button[aria-label="delete"]').click();
      });
      cy.contains('tr', 'Futópad').should('not.exist');
    });
  });

  describe('Validation', () => {
    it('should require machine name', () => {
      cy.get('.newButton').click();
      cy.get('.save-button').click();
      cy.get('.error-message').should('contain', 'A név kötelező');
    });

    
  });

  describe('Edge Cases', () => {
    it('should handle maximum length inputs', () => {
      const longName = 'A'.repeat(255);
      
      cy.get('.newButton').click();
      cy.get('#machineNameInput').type(longName);
      cy.get('.save-button').click();
      cy.get('.error-message').should('contain', 'A név minimum 4, maximum 100 karakter hosszú lehet.');
     
    });

    it('should maintain state after refresh', () => {
      cy.contains('tr', 'Elliptikus tréner').within(() => {
        cy.get('button[aria-label="delete"]').click();
      });
      cy.reload();
      cy.contains('tr', 'Elliptikus tréner').should('not.exist');
    });
  });

  // describe('Error Handling', () => {
  //   it('should display network errors', () => {
  //     cy.intercept('POST', '/api/machines', { statusCode: 500 });
  //     cy.get('.newButton').click();
  //     cy.get('#machineNameInput').type('Hálózati hiba gépe');
  //     cy.get('.save-button').click();
  //     cy.get('.error-message').should('contain', 'kérés sikertelen');
  //   });
  // });
});

describe('Admin CMS - Room Management', () => {
  const seededRooms = [
    { name: 'Futós szoba', people: 15 },
    { name: 'Biciklizős Szoba', people: 10 },
    { name: 'Súlyzós terem', people: 20 },
    { name: 'CrossFit terem', people: 12 },
    { name: 'Jógaterem', people: 8 },
    { name: 'Aerobik terem', people: 18 },
    { name: 'Küzdősport terem', people: 10 },
    { name: 'Spinning terem', people: 15 },
    { name: 'Rehabilitációs terem', people: 8 },
    { name: 'Kardió terem', people: 20 }
  ];

  beforeEach(() => {
    cy.viewport('macbook-15')  
    cy.request('POST', "http://localhost:5278/seed");
    cy.loginAsAdmin();
    cy.visit('/user/rooms');
    cy.get('table').should('be.visible');
  });

  describe('Initial State Verification', () => {
    it('should display active rooms with correct data', () => {
      cy.get('table tr:not(.deleted)').should('have.length', seededRooms.length + 1); // +1 for header
      
      seededRooms.forEach(room => {
        cy.contains('tr:not(.deleted)', room.name).within(() => {
          cy.get('[data-cell="Javasolt max létszám"]').should('contain', room.people);
        });
      });
    });
  });

  describe('Sorting Functionality', () => {
    const verifySort = (column: string, values: string[]) => {
      cy.get(`[data-cell="${column}"]`).then($cells => {
        const actual = $cells.map((_, el) => el.innerText).get();
        expect(actual.slice(0, values.length)).to.deep.equal(values);
      });
    };

    it('should sort by name ascending/descending', () => {
      const expectedAsc = [
        'Aerobik terem',
        'Biciklizős Szoba',
        'CrossFit terem',
        'Futós szoba',
        'Jógaterem',
        'Kardió terem',
        'Küzdősport terem',
        'Rehabilitációs terem',
        'Spinning terem',
        'Súlyzós terem'
      ];

      cy.get('th:contains("Név")').click();
      verifySort('Név', expectedAsc);
      
      cy.get('th:contains("Név")').click();
      verifySort('Név', [...expectedAsc].reverse());
    });

    it('should sort by recommended people descending', () => {
      cy.get('th:contains("Javasolt max létszám")').click().click();
      verifySort('Javasolt max létszám', ['20', '20', '18', '15', '15', '12', '10', '10', '8', '8']);
    });
  });

  describe('CRUD Operations', () => {
    it('should create new room with valid data', () => {
      cy.get('.newButton').click();
      
      cy.get('#roomNameInput').type('Új Pilates terem');
      cy.get('#priceInput').type('12');
      cy.get('#roomDescriptionInput').type('hehe');
      cy.get('.save-button').click();
      
      cy.contains('tr', 'Új Pilates terem').should('be.visible');
    });

    it('should update existing room', () => {
      cy.contains('tr', 'CrossFit terem').within(() => {
        cy.get('button[aria-label="update"]').click();
      });
      
      cy.get('#roomNameInput').clear().type('CrossFit Pro terem');
      cy.get('.save-button').click();
      
      cy.contains('tr', 'CrossFit Pro terem').should('be.visible');
    });

    it('should soft-delete room', () => {
      cy.contains('tr', 'Súlyzós terem').within(() => {
        cy.get('button[aria-label="delete"]').click();
      });
      
      cy.get('.toggle-slider').click();
      cy.contains('tr.deleted', 'Súlyzós terem').should('be.visible');
    });
  });

  describe('Form Validation', () => {
    it('should require name field', () => {
      cy.get('.newButton').click();
      cy.get('.save-button').click();
      
      cy.get('.error-message').should('contain', 'A név kötelező');

    });
    it('should require description field', () => {
      cy.get('.newButton').click();
      cy.get('#roomNameInput').type('Új Pilates terem');
      cy.get('#priceInput').type('12');
      cy.get('.save-button').click();

      cy.get('.error-message').should('contain', 'A leírás kötelező');
    });
    it('should prevent zero recommended people values', () => {
      cy.get('.newButton').click();
      cy.get('#roomNameInput').type('Új Pilates terem');
      cy.get('#roomDescriptionInput').type('hehe');

      cy.get('#priceInput').type('0');
      cy.get('.save-button').click();
      
      cy.get('.error-message').should('contain', 'Ajánlott létszám nem lehet nulla');
    });
  });

  describe('Search Functionality', () => {
    it('should filter by name partial match', () => {
      cy.get('.searchBar').type('terem');
      cy.get('[data-cell="Név"]').should('have.length.at.least', 8);
    });

    it('should be case-insensitive', () => {
      cy.get('.searchBar').type('BICIKLI');
      cy.contains('tr', 'Biciklizős Szoba').should('be.visible');
    });
  });

  describe('Edge Cases', () => {
    it('should persist data after refresh', () => {
      cy.get('.newButton').click();
      cy.get('#roomNameInput').type('Perzisztens terem');
      cy.get('#priceInput').type('10');
      cy.get('#roomDescriptionInput').type('hehe');

      cy.get('.save-button').click();
      
      cy.reload();
      cy.contains('tr', 'Perzisztens terem').should('be.visible');
    });

    it('should prevent duplicate room names', () => {
      cy.get('.newButton').click();
      cy.get('#roomNameInput').type('Kardió terem');
      cy.get('#priceInput').type('20');
      cy.get('#roomDescriptionInput').type('hehe');

      cy.get('.save-button').click();
      
      cy.get('.bottom-error').should('contain', 'Ilyen terem már létezik.');
    });
  });
});