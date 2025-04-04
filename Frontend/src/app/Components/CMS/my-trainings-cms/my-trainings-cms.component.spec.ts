import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyTrainingsCMSComponent } from './my-trainings-cms.component';

describe('MyTrainingsCMSComponent', () => {
  let component: MyTrainingsCMSComponent;
  let fixture: ComponentFixture<MyTrainingsCMSComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyTrainingsCMSComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyTrainingsCMSComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
