import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SearchWordCardComponent } from './search-word-card.component';

describe('SearchWordCardComponent', () => {
  let component: SearchWordCardComponent;
  let fixture: ComponentFixture<SearchWordCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchWordCardComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(SearchWordCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
