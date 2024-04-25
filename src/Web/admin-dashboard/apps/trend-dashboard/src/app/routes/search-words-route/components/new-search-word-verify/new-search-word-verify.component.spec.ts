import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NewSearchWordVerifyComponent } from './new-search-word-verify.component';

describe('NewSearchWordVerifyComponent', () => {
  let component: NewSearchWordVerifyComponent;
  let fixture: ComponentFixture<NewSearchWordVerifyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewSearchWordVerifyComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(NewSearchWordVerifyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
