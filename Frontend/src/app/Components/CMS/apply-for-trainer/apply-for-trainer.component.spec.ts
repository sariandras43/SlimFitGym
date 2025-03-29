import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplyForTrainerComponent } from './apply-for-trainer.component';

describe('ApplyForTrainerComponent', () => {
  let component: ApplyForTrainerComponent;
  let fixture: ComponentFixture<ApplyForTrainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApplyForTrainerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApplyForTrainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
