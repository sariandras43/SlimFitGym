import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainigsPageComponent } from './trainigs-page.component';

describe('TrainigsPageComponent', () => {
  let component: TrainigsPageComponent;
  let fixture: ComponentFixture<TrainigsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrainigsPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrainigsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
