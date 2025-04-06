import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SideScrollerComponent } from './side-scroller.component';

describe('SideScrollerComponent', () => {
  let component: SideScrollerComponent;
  let fixture: ComponentFixture<SideScrollerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SideScrollerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SideScrollerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
