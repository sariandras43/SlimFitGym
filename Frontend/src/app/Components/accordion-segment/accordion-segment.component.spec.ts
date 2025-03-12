import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccordionSegmentComponent } from './accordion-segment.component';

describe('AccordionSegmentComponent', () => {
  let component: AccordionSegmentComponent;
  let fixture: ComponentFixture<AccordionSegmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AccordionSegmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AccordionSegmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
