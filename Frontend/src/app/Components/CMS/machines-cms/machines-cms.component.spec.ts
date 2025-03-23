import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MachinesCMSComponent } from './machines-cms.component';

describe('MachinesCMSComponent', () => {
  let component: MachinesCMSComponent;
  let fixture: ComponentFixture<MachinesCMSComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MachinesCMSComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MachinesCMSComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
