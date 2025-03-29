import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserCMSComponent } from './user-cms.component';

describe('UserCMSComponent', () => {
  let component: UserCMSComponent;
  let fixture: ComponentFixture<UserCMSComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserCMSComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserCMSComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
