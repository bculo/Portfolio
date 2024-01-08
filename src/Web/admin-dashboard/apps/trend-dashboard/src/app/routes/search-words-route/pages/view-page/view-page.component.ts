import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchWordStore } from '../../store/search-word-store';
import { InputComponent } from 'apps/trend-dashboard/src/app/shared/controls/input/input.component';
import { DictionaryStore } from 'apps/trend-dashboard/src/app/store/dictionary-store';
import { FormFieldComponent } from 'apps/trend-dashboard/src/app/shared/controls/form-field/form-field.component';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SelectComponent } from 'apps/trend-dashboard/src/app/shared/controls/select/select.component';
import { ButtonComponent } from 'apps/trend-dashboard/src/app/shared/components/button/button.component';
import { toObservable } from '@angular/core/rxjs-interop';
import { filter, take, tap } from 'rxjs';
import { SearchWordFilterModel } from '../../models/search-words.model';
import { SearchWordCardComponent } from '../../components/search-word-card/search-word-card.component';

@Component({
  selector: 'admin-dashboard-view-page',
  standalone: true,
  imports: [CommonModule, InputComponent, SelectComponent, FormFieldComponent, ReactiveFormsModule, ButtonComponent, SearchWordCardComponent],
  templateUrl: './view-page.component.html',
  styleUrl: './view-page.component.scss',
})
export class ViewPageComponent implements OnInit {
  readonly searchWordStore = inject(SearchWordStore);
  readonly dictionaryStore = inject(DictionaryStore);
  readonly formBuilder = inject(FormBuilder);

  searchWords = this.searchWordStore.entities;
  totalSearchWords = this.searchWordStore.totalCount;
  defaultAllValue = this.dictionaryStore.defaultAllValue;
  activeOptions = this.dictionaryStore.activeFilterItemsOptions;
  sortOptions = this.dictionaryStore.sortFilterItemsOptions;
  contextTypes = this.dictionaryStore.contextTypesFilterItemsOptions;
  searchEngines = this.dictionaryStore.searchEngineFilterItemsOptions;

  isLoading$ = toObservable(this.dictionaryStore.isLoading);

  form: FormGroup = this.formBuilder.group({
    query: new FormControl<string | null>(null, { updateOn: 'change' }),
    active: new FormControl<number>(this.defaultAllValue(), { updateOn: 'change', validators: [Validators.required] }),
    searchEngine: new FormControl<number>(this.defaultAllValue(), { updateOn: 'change', validators: [Validators.required] }),
    contextType: new FormControl<number>(this.defaultAllValue(), { updateOn: 'change', validators: [Validators.required] }),
    sort: new FormControl<number>(0, { updateOn: 'change', validators: [Validators.required] }),
  });

  ngOnInit(): void {
    this.isLoading$.pipe(
      filter(isLoading => !isLoading),
      take(1),
      tap(() => {
        const request = this.getRequestFilter();
        this.searchWordStore.initialFetch(request);
      })
    ).subscribe();
  }

  private getRequestFilter(): SearchWordFilterModel {
    return {...this.form!.value, page: 1, take: 50} as SearchWordFilterModel
  }

  onSubmit() {
    const request = this.getRequestFilter();
  }
}
