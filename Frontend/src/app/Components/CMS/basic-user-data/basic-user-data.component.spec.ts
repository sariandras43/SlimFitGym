import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BasicUserDataComponent } from './basic-user-data.component';

describe('BasicUserDataComponent', () => {
  let component: BasicUserDataComponent;
  let fixture: ComponentFixture<BasicUserDataComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BasicUserDataComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BasicUserDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
