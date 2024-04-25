import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchWordStore } from '../../store/search-word-store';
import { InputComponent } from 'apps/trend-dashboard/src/app/shared/controls/input/input.component';
import { DictionaryStore } from 'apps/trend-dashboard/src/app/store/dictionary/dictionary-store';
import { FormFieldComponent } from 'apps/trend-dashboard/src/app/shared/controls/form-field/form-field.component';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SelectComponent } from 'apps/trend-dashboard/src/app/shared/controls/select/select.component';
import { ButtonComponent } from 'apps/trend-dashboard/src/app/shared/components/button/button.component';
import { toObservable } from '@angular/core/rxjs-interop';
import { filter, take, tap } from 'rxjs';
import { SearchWordFilterModel } from '../../store/search-word.model';
import { SearchWordCardComponent } from '../../components/search-word-card/search-word-card.component';
import { NgIconComponent } from '@ng-icons/core';
import { SideModalComponent } from 'apps/trend-dashboard/src/app/shared/components/side-modal/side-modal.component';
import { SearchWordDetailComponent } from '../../components/search-word-detail/search-word-detail.component';
import { ActiveEnumOptions, ContextTypeEnumOptions, SearchEngineEnumOptions, SortEnumOptions } from 'apps/trend-dashboard/src/app/shared/enums/enums';
import { Router } from '@angular/router';
import { PageHeaderComponent } from 'apps/trend-dashboard/src/app/shared/components/page-header/page-header.component';
import { SpinnerComponent } from "../../../../shared/components/spinner/spinner.component";

@Component({
    selector: 'admin-dashboard-view-page',
    standalone: true,
    templateUrl: './view-page.component.html',
    styleUrl: './view-page.component.scss',
    imports: [
        CommonModule, InputComponent, SelectComponent, FormFieldComponent,
        ReactiveFormsModule, ButtonComponent, SearchWordCardComponent,
        NgIconComponent, SideModalComponent, SearchWordDetailComponent,
        PageHeaderComponent, SpinnerComponent
    ]
})
export class ViewPageComponent implements OnInit {
  readonly searchWordStore = inject(SearchWordStore);
  readonly dictionaryStore = inject(DictionaryStore);
  readonly formBuilder = inject(FormBuilder);
  readonly router = inject(Router);

  searchWords = this.searchWordStore.entities;
  totalSearchWords = this.searchWordStore.totalCount;
  activeOptions = this.dictionaryStore.activeFilterItemsOptions;
  sortOptions = this.dictionaryStore.sortFilterItemsOptions;
  contextTypes = this.dictionaryStore.contextTypesFilterItemsOptions;
  searchEngines = this.dictionaryStore.searchEngineFilterItemsOptions;
  searchWordModalId = this.searchWordStore.searchWordModal;
  isLoading = this.dictionaryStore.isLoading;

  isLoading$ = toObservable(this.dictionaryStore.isLoading);

  formSnapshotValue: any = null;
  form: FormGroup = this.formBuilder.group({
    query: new FormControl<string>('', { updateOn: 'change' }),
    active: new FormControl<number>(ActiveEnumOptions.All, { updateOn: 'change', validators: [Validators.required] }),
    searchEngine: new FormControl<number>(SearchEngineEnumOptions.All, { updateOn: 'change', validators: [Validators.required] }),
    contextType: new FormControl<number>(ContextTypeEnumOptions.All, { updateOn: 'change', validators: [Validators.required] }),
    sort: new FormControl<number>(SortEnumOptions.Asc, { updateOn: 'change', validators: [Validators.required] }),
  });

  ngOnInit(): void {
    this.formSnapshotValue = this.form.value;
    this.isLoading$.pipe(
      filter(isLoading => !isLoading),
      take(1),
      tap(() => this.searchWordStore.fetch(this.getRequestFilter()))
    ).subscribe();
  }

  private getRequestFilter(): SearchWordFilterModel {
    return {...this.form!.value, page: 1, take: 50} as SearchWordFilterModel
  }

  onSubmit(): void {
    this.searchWordStore.fetch(this.getRequestFilter());
  }

  resetForm(): void {
    this.form.patchValue(this.formSnapshotValue);
    this.onSubmit();
  }

  onAddNew(): void {
    this.router.navigate(['/words/new'])
  }
}
