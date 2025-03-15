import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneralPurposeCardComponent } from './general-purpose-card.component';

describe('GeneralPurposeCardComponent', () => {
  let component: GeneralPurposeCardComponent;
  let fixture: ComponentFixture<GeneralPurposeCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GeneralPurposeCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GeneralPurposeCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
