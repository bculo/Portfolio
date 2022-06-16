import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { FormHelperService } from 'src/app/services/form-mapper/form-helper.service';
import { markFormAsTouched } from 'src/app/shared/utils/form';

import * as fromRoot from 'src/app/store';
import { SETTINGS_FORM_IDENTIFIER } from './constants';

import * as fromSync from './store/sync';
import { SyncStatus } from './store/sync';

@Component({
  selector: 'app-sync',
  templateUrl: './sync.component.html',
  styleUrls: ['./sync.component.scss']
})
export class SyncComponent implements OnInit, OnDestroy {

  form: FormGroup;

  statuses$: Observable<SyncStatus[]>;
  loading$: Observable<boolean>;

  constructor(private store: Store<fromRoot.State>,
    private fb: FormBuilder,
    private formHelper: FormHelperService) { }

  ngOnDestroy(): void {
    this.formHelper.removeForm(SETTINGS_FORM_IDENTIFIER);
  }

  ngOnInit(): void {
    
    this.form = this.fb.group({
      'searchWord': [null, {
        updateOn: 'blur',
        validators: [Validators.required, Validators.minLength(2)]
      }],
      'searchEngine': [null, {
        updateOn: 'blur',
        validators: [Validators.required]
      }],
      'contextType': [null, {
        updateOn: 'blur',
        validators: [Validators.required]
      }]            
    });

    this.formHelper.addForm(SETTINGS_FORM_IDENTIFIER, this.form);

    this.store.dispatch(fromSync.fetchStatuses());

    this.statuses$ = this.store.select(fromSync.syncGetItems);
    this.loading$ = this.store.select(fromSync.syncGetLoading);
  }

  onSubmit() {
    if(this.form.invalid){
      markFormAsTouched(this.form);
      return;
    }

    this.store.dispatch(fromSync.addNewWord({newSetting: this.form.value}));
  }

  getControl(name: string) : AbstractControl {
    return this.form.controls[name];
  }
}
