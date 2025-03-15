import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PassCardComponent } from './pass-card.component';

describe('PassCardComponent', () => {
  let component: PassCardComponent;
  let fixture: ComponentFixture<PassCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PassCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PassCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
