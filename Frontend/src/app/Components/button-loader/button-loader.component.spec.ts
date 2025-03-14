import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ButtonLoaderComponent } from './button-loader.component';

describe('ButtonLoaderComponent', () => {
  let component: ButtonLoaderComponent;
  let fixture: ComponentFixture<ButtonLoaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ButtonLoaderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ButtonLoaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
