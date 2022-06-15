import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormFiledComponent } from './form-filed.component';



@NgModule({
  declarations: [
    FormFiledComponent
  ],
  exports: [
    FormFiledComponent
  ],
  imports: [
    CommonModule
  ]
})
export class FormFiledModule { }
