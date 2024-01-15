import { ChangeDetectorRef, Component, OnDestroy, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SelectComponent } from 'apps/trend-dashboard/src/app/shared/controls/select/select.component';
import { InputComponent } from 'apps/trend-dashboard/src/app/shared/controls/input/input.component';
import { FormFieldComponent } from 'apps/trend-dashboard/src/app/shared/controls/form-field/form-field.component';
import { ButtonComponent } from 'apps/trend-dashboard/src/app/shared/components/button/button.component';
import { ContextTypeEnumOptions, SearchEngineEnumOptions } from 'apps/trend-dashboard/src/app/shared/enums/enums';
import { DictionaryStore } from 'apps/trend-dashboard/src/app/store/dictionary/dictionary-store';
import { StepperService } from 'apps/trend-dashboard/src/app/shared/components/stepper/stepper.service';
import { Subject, takeUntil, tap } from 'rxjs';
import { SearchWordStore } from '../../store/search-word-store';

@Component({
  selector: 'admin-dashboard-new-search-word-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, SelectComponent, InputComponent, FormFieldComponent, ButtonComponent],
  templateUrl: './new-search-word-form.component.html',
  styleUrl: './new-search-word-form.component.scss',
})
export class NewSearchWordFormComponent implements OnInit, OnDestroy {
  readonly formBuilder = inject(FormBuilder);
  readonly dictionaryStore = inject(DictionaryStore);
  readonly wordStore = inject(SearchWordStore);
  readonly stepperService = inject(StepperService);
  readonly cdRef = inject(ChangeDetectorRef)

  newItem = this.wordStore.newItem;
  contextTypes = this.dictionaryStore.contextTypeEditItemsOptions;
  searchEngines = this.dictionaryStore.searchEngineEditItemsOptions;

  form: FormGroup = this.formBuilder.group({
    searchWord: new FormControl<string | null>(null, { updateOn: 'change', validators: [Validators.required] }),
    contextType: new FormControl<number>(ContextTypeEnumOptions.Crypto, { updateOn: 'change', validators: [Validators.required] }),
    searchEngine: new FormControl<number>(SearchEngineEnumOptions.Google, { updateOn: 'change', validators: [Validators.required] }),
  });

  private lifecycle = new Subject<void>();

  ngOnInit(): void {
    this.checkForExistingItem();
    
    this.stepperService.check$.pipe(
      takeUntil(this.lifecycle),
      tap(() => this.onSubmit())
    ).subscribe()
  }

  checkForExistingItem(): void {
    const submittedItem = this.newItem();
    if(submittedItem) {
      this.form.patchValue(submittedItem);
    }
  }

  ngOnDestroy(): void {
    this.lifecycle.next();
    this.lifecycle.complete();
  }

  onSubmit() {
    this.form.markAllAsTouched();
    if(!this.form.valid) {
      this.stepperService.onCheckHandled(false);
      return;
    }

    this.wordStore.setNewItem(this.form.value);
    this.stepperService.onCheckHandled(true);
  }
}
