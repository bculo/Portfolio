import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchWordStore } from '../../store/search-word-store';
import { InputComponent } from 'apps/trend-dashboard/src/app/shared/controls/input/input.component';
import { DictionaryStore } from 'apps/trend-dashboard/src/app/store/dictionary-store';
import { FormFieldComponent } from 'apps/trend-dashboard/src/app/shared/controls/form-field/form-field.component';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SelectComponent } from 'apps/trend-dashboard/src/app/shared/controls/select/select.component';
import { ButtonComponent } from 'apps/trend-dashboard/src/app/shared/components/button/button.component';

@Component({
  selector: 'admin-dashboard-view-page',
  standalone: true,
  imports: [CommonModule, InputComponent, SelectComponent, FormFieldComponent, ReactiveFormsModule, ButtonComponent],
  templateUrl: './view-page.component.html',
  styleUrl: './view-page.component.scss',
})
export class ViewPageComponent {
  readonly searchWordStore = inject(SearchWordStore);
  readonly dictionaryStore = inject(DictionaryStore);
  readonly formBuilder = inject(FormBuilder);

  activeOptions = this.dictionaryStore.activeFilterOptions;
  sortOptions = this.dictionaryStore.sortOptions;
  contextTypes = this.dictionaryStore.contextTypes;
  searchEngines = this.dictionaryStore.searchEngines;

  form: FormGroup = this.formBuilder.group({
    query: new FormControl<string | null>(null, { updateOn: 'change' }),
    active: new FormControl<number>(0, { updateOn: 'change', validators: [Validators.required] }),
    searchEngine: new FormControl<number>(0, { updateOn: 'change', validators: [Validators.required] }),
    contextType: new FormControl<number>(0, { updateOn: 'change', validators: [Validators.required] }),
    sort: new FormControl<number>(0, { updateOn: 'change', validators: [Validators.required] }),
  });

  onSubmit() {
    console.log(this.form.value)
  }
}
