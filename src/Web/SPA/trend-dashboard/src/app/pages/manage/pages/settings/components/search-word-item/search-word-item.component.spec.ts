import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchWordItemComponent } from './search-word-item.component';

describe('SearchWordItemComponent', () => {
  let component: SearchWordItemComponent;
  let fixture: ComponentFixture<SearchWordItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SearchWordItemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchWordItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
