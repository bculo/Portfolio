import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-sync',
  templateUrl: './manage.component.html',
})
export class ManageComponent implements OnInit {

  /*
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

    //this.store.dispatch(fromSync.fetchStatuses());
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
  */

  ngOnInit(): void {
    
  }
}
