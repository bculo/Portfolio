import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NewSearchWordFormComponent } from './new-search-word-form.component';

describe('NewSearchWordFormComponent', () => {
  let component: NewSearchWordFormComponent;
  let fixture: ComponentFixture<NewSearchWordFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewSearchWordFormComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(NewSearchWordFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
