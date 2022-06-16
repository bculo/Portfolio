import { Injectable } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { Dictionary, DictionaryList } from 'src/app/models/frontend/dictionary';
import { pascalToCamelCase } from 'src/app/shared/utils/string';

interface FormState {
  form: FormGroup,
  mapping: Dictionary<string>
}

@Injectable({
  providedIn: 'root'
})
export class FormHelperService {

  private forms: Dictionary<FormState> = {};

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

  removeForm(formIdentifier: string): void {
    delete this.forms[formIdentifier];
  }

  handleValidationError(formIdentifier: string, errors: DictionaryList<string>): void {
    const formState = this.forms[formIdentifier];
    if(!formState) return;

    const mappingFunction = formState.mapping ? this.definedNameMapping : this.pascalCaseNameMapping;

    for(let key in errors){
      const formControlName: string = mappingFunction(key, formState);
      const formControl: AbstractControl = formState.form.controls[formControlName];
      const formControlError = errors[key][0];
      formControl.setErrors({ serverError: { message: formControlError }});
    }
  }

  private pascalCaseNameMapping(key: string, formState: FormState = null): string {
    return pascalToCamelCase(key);
  } 

  private definedNameMapping(key: string, formState: FormState = null): string {
    return formState.mapping[key];
  } 

}
