import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoomCMSComponent } from './room-cms.component';

describe('RoomCMSComponent', () => {
  let component: RoomCMSComponent;
  let fixture: ComponentFixture<RoomCMSComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RoomCMSComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RoomCMSComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
