import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubscriptionsPageComponent } from './subscriptions-page.component';

describe('SubscriptionsPageComponent', () => {
  let component: SubscriptionsPageComponent;
  let fixture: ComponentFixture<SubscriptionsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubscriptionsPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SubscriptionsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
