import { TestBed } from '@angular/core/testing';

import { PassService } from './pass.service';

describe('PassService', () => {
  let service: PassService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PassService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
