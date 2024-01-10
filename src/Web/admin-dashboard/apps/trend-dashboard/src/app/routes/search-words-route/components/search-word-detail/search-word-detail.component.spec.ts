import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SearchWordDetailComponent } from './search-word-detail.component';

describe('SearchWordDetailComponent', () => {
  let component: SearchWordDetailComponent;
  let fixture: ComponentFixture<SearchWordDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchWordDetailComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(SearchWordDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
