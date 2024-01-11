import { AfterViewInit, Component, Input, OnDestroy, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchWordStore } from '../../store/search-word-store';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { toObservable } from '@angular/core/rxjs-interop';
import { Subject, takeUntil, tap } from 'rxjs';
import { FormFieldComponent } from 'apps/trend-dashboard/src/app/shared/controls/form-field/form-field.component';
import { InputComponent } from 'apps/trend-dashboard/src/app/shared/controls/input/input.component';
import { ButtonComponent } from 'apps/trend-dashboard/src/app/shared/components/button/button.component';

@Component({
  selector: 'admin-dashboard-search-word-detail',
  standalone: true,
  imports: [CommonModule, FormFieldComponent, InputComponent, ButtonComponent, ReactiveFormsModule],
  templateUrl: './search-word-detail.component.html',
  styleUrl: './search-word-detail.component.scss',
})
export class SearchWordDetailComponent implements OnInit, OnDestroy {
  readonly searchWordStore = inject(SearchWordStore);
  readonly formBuilder = inject(FormBuilder);

  updateItem = this.searchWordStore.updateItem;
  updateItem$ = toObservable(this.searchWordStore.updateItem);

  form: FormGroup = this.formBuilder.group({
    searchWord: new FormControl<string | null>(null, { updateOn: 'change', validators: [Validators.required] }),
  });

  ngOnInit(): void {
    const item = this.updateItem();
    if(item) {
      this.form.patchValue(item);
    }
  }

  ngOnDestroy(): void {

  }
}