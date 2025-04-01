import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingsCMSComponent } from './trainings-cms.component';

describe('TrainingsCMSComponent', () => {
  let component: TrainingsCMSComponent;
  let fixture: ComponentFixture<TrainingsCMSComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrainingsCMSComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrainingsCMSComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
