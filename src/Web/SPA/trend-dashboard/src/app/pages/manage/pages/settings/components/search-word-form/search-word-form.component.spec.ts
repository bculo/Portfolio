import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchWordFormComponent } from './search-word-form.component';

describe('SearchWordFormComponent', () => {
  let component: SearchWordFormComponent;
  let fixture: ComponentFixture<SearchWordFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SearchWordFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchWordFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
