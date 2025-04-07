import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubscribedTrainingsComponent } from './subscribed-trainings.component';

describe('SubscribedTrainingsComponent', () => {
  let component: SubscribedTrainingsComponent;
  let fixture: ComponentFixture<SubscribedTrainingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubscribedTrainingsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SubscribedTrainingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
