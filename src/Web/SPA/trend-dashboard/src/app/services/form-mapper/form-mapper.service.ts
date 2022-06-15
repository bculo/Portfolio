import { Injectable } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { Dictionary, DictionaryList } from 'src/app/models/frontend/dictionary';
import { pascalToCamelCase } from 'src/app/shared/utils/string';

export interface FormState {
  form: FormGroup,
  mapping: Dictionary<string>
}

@Injectable({
  providedIn: 'root'
})
export class FormMapperService {

  private forms: Dictionary<FormState> = {};

  constructor() { }

  isAvailable(formIdentifier: string): boolean {
    return this.forms[formIdentifier] != null;
  }

  getForm(formIdentifier: string): FormGroup  {
    return this.forms[formIdentifier].form;
  }

  addForm(formIdentifier: string, form: FormGroup, mapping: Dictionary<string> | null = null): void {
    this.forms[formIdentifier] = {
      form: form,
      mapping: mapping
    };
  }

  removeForm(formIdentifier: string) {
    delete this.forms[formIdentifier];
  }

  handleValidationError(formIdentifier: string, errors: DictionaryList<string>): void {
    const formState = this.forms[formIdentifier];
    if(!formState) return;

    if(formState.mapping) { //use custom mapping
      for(let key in errors){
        const formControlName: string = formState.mapping[key];
        const formControl: AbstractControl = formState.form.controls[formControlName];
        const formControlError = errors[key][0];
        formControl.setErrors({ serverError: { message: formControlError}});
      }
      return;
    }

    for(let key in errors){ //use pascalecase mapping
      const formControl: AbstractControl = formState.form.controls[pascalToCamelCase(key)];
      const formControlError = errors[key][0];
      formControl.setErrors({ serverError: { message: formControlError}});
    }
  }

  getAll() {
    return {...this.forms};
  }
}
