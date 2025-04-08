import { TestBed } from '@angular/core/testing';

import { TrainerApplicationService } from './trainer-application.service';

describe('TrainerApplicationService', () => {
  let service: TrainerApplicationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TrainerApplicationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
