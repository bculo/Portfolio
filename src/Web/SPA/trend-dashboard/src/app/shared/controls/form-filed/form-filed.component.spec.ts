import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormFiledComponent } from './form-filed.component';

describe('FormFiledComponent', () => {
  let component: FormFiledComponent;
  let fixture: ComponentFixture<FormFiledComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FormFiledComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FormFiledComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
