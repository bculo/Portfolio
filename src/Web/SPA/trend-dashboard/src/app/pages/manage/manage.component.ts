import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { FormHelperService } from 'src/app/services/form-mapper/form-helper.service';
import { markFormAsTouched } from 'src/app/shared/utils/form';

import * as fromRoot from 'src/app/store';
import { SETTINGS_FORM_IDENTIFIER } from './constants';

import * as fromSync from './store/sync';
import * as fromDictionaries from './store/dictionaries';

import { ControlItem } from './store/dictionaries';

@Component({
  selector: 'app-sync',
  templateUrl: './manage.component.html',
  styleUrls: ['./manage.component.scss']
})
export class ManageComponent implements OnInit, OnDestroy {

  form: FormGroup;

  contextTypes$: Observable<ControlItem[]>;
  engineTypes$: Observable<ControlItem[]>;

  constructor(private store: Store<fromRoot.State>,
    private fb: FormBuilder,
    private formHelper: FormHelperService) { }

  ngOnDestroy(): void {
    this.formHelper.removeForm(SETTINGS_FORM_IDENTIFIER);
  }

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

    this.store.dispatch(fromSync.fetchStatuses());
    this.store.dispatch(fromDictionaries.fetchDictionaries());

    this.contextTypes$ = this.store.select(fromDictionaries.getContextTypesDict);
    this.engineTypes$ = this.store.select(fromDictionaries.getEngineTypesDict);
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
