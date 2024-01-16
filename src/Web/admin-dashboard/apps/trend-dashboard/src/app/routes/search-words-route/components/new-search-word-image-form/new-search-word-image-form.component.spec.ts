import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NewSearchWordImageFormComponent } from './new-search-word-image-form.component';

describe('NewSearchWordImageFormComponent', () => {
  let component: NewSearchWordImageFormComponent;
  let fixture: ComponentFixture<NewSearchWordImageFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewSearchWordImageFormComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(NewSearchWordImageFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
