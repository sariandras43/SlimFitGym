import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MachineCardComponent } from './machine-card.component';

describe('MachineCardComponent', () => {
  let component: MachineCardComponent;
  let fixture: ComponentFixture<MachineCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MachineCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MachineCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
