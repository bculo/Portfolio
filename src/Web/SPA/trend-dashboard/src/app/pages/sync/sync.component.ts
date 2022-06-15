import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Observable, Subject, takeUntil } from 'rxjs';
import { FormMapperService } from 'src/app/services/form-mapper/form-mapper.service';

import * as fromRoot from 'src/app/store';
import { SETTINGS_FROM } from './constants';

import * as fromSync from './store/sync';
import { Dictionary, DictionaryList, SyncStatus } from './store/sync';

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
    private formHelper: FormMapperService) { }

  ngOnDestroy(): void {
    this.formHelper.removeForm(SETTINGS_FROM);
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

    this.formHelper.addForm(SETTINGS_FROM, this.form);

    this.store.dispatch(fromSync.fetchStatuses());

    this.statuses$ = this.store.select(fromSync.syncGetItems);
    this.loading$ = this.store.select(fromSync.syncGetLoading);
  }

  disable() {
    console.log(this.formHelper.getAll());
  }

  enable() {
    this.form.enable();
  }

  onSubmit() {
    if(this.form.invalid){
      console.log("onSubmit failed");
      return;
    }

    console.log(this.form.value);

    this.store.dispatch(fromSync.addNewWord({newSetting: this.form.value}));
  }

  getControl(name: string) : AbstractControl {
    return this.form.controls[name];
  }

  canPressSubmitButton() {

  }


}
