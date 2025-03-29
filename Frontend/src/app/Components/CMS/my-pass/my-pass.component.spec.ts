import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyPassComponent } from './my-pass.component';

describe('MyPassComponent', () => {
  let component: MyPassComponent;
  let fixture: ComponentFixture<MyPassComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyPassComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyPassComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
