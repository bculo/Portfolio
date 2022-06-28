import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { SETTINGS_FORM_IDENTIFIER } from 'src/app/pages/manage/constants';
import { ControlItem } from 'src/app/pages/manage/store/dictionaries/dictionaries.models';
import { FormHelperService } from 'src/app/services/form-mapper/form-helper.service';

import * as fromRoot from 'src/app/store/index';

import * as dictionariesSelector from 'src/app/pages/manage/store/dictionaries/dictionaries.selectors';
import { markFormAsTouched } from 'src/app/shared/utils/form';

@Component({
  selector: 'app-search-word-form',
  templateUrl: './search-word-form.component.html',
  styleUrls: ['./search-word-form.component.scss']
})
export class SearchWordFormComponent implements OnInit, OnDestroy {

  form: FormGroup;

  contextTypes$: Observable<ControlItem[]>;
  engineTypes$: Observable<ControlItem[]>;

  constructor(private store: Store<fromRoot.State>,
    private fb: FormBuilder,
    private formHelper: FormHelperService,
    private dialogRef: MatDialogRef<SearchWordFormComponent>) { }

  ngOnInit(): void {
    this.formHelper.addForm(SETTINGS_FORM_IDENTIFIER, this.form);

    this.form = this.fb.group({
      'searchWord': [null, {
        updateOn: 'blur',
        validators: [Validators.required, Validators.minLength(2)]
      }],
      'searchEngine': [null, {
        updateOn: 'change',
        validators: [Validators.required]
      }],
      'contextType': [null, {
        updateOn: 'change',
        validators: [Validators.required]
      }],              
    });

    this.contextTypes$ = this.store.select(dictionariesSelector.getContextTypesDict);
    this.engineTypes$ = this.store.select(dictionariesSelector.getEngineTypesDict);
  }

  ngOnDestroy(): void {
    this.formHelper.removeForm(SETTINGS_FORM_IDENTIFIER);
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

  onSubmit() {
    if(this.form.invalid){
      markFormAsTouched(this.form);
      return;
    }

    console.log(this.form.value);

    //this.store.dispatch(fromSync.addNewWord({newSetting: this.form.value}));
  }

  getControl(name: string) : AbstractControl {
    return this.form.controls[name];
  }

  onClear(): void {
    this.form.reset();
  }
}
