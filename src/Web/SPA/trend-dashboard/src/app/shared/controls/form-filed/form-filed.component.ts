import { Component, Input, OnInit } from '@angular/core';
import { AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-form-filed',
  templateUrl: './form-filed.component.html',
  styleUrls: ['./form-filed.component.scss']
})
export class FormFiledComponent implements OnInit {

  @Input() label: string;
  @Input() control: AbstractControl;

  error: string = 'ERROR';
  

  constructor() { }

  ngOnInit(): void {

  }

  isValid(): boolean{
    return this.control.touched && this.control.valid;
  }

  get errorKey(){
    return this.control && this.control.touched && this.control.errors && Object.keys(this.control.errors)[0];
  }

  getErrorMessage() {
    return ;
  }

}
