import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PassesCMSComponent } from './passes-cms.component';

describe('PassesCMSComponent', () => {
  let component: PassesCMSComponent;
  let fixture: ComponentFixture<PassesCMSComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PassesCMSComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PassesCMSComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
