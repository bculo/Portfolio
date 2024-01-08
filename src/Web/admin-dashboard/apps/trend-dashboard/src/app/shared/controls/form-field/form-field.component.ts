import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl } from '@angular/forms';

@Component({
  selector: 'admin-dashboard-form-field',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './form-field.component.html',
  styleUrl: './form-field.component.scss',
})
export class FormFieldComponent {
  @Input() label: string = '';
  @Input() control!: AbstractControl;
  @Input() required: boolean = false;

  error: string = 'ERROR';
  
  hasError(): boolean{
    return this.control && this.control.touched && this.control.invalid;
  }

  get errorKey(){
    return this.control && this.control.touched && this.control.errors && Object.keys(this.control.errors)[0];
  }

}
