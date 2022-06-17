import { Component, Input, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatSelectChange } from '@angular/material/select';
import { ControlItem } from 'src/app/models/frontend/controls';

interface Food {
  value: string;
  viewValue: string;
}

@Component({
  selector: 'app-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: SelectComponent,
      multi: true,
    }
  ]
})
export class SelectComponent implements OnInit, ControlValueAccessor  {

  @Input() items: ControlItem[];
  @Input() placeholder: string;
  
  value: string;
  disabled: boolean;

  constructor() { }

  onChangeFun: any = () => {};
  onTouchedFun: any = () => {};

  writeValue(obj: string): void {
    this.value = obj;
  }
  
  registerOnChange(fn: any): void {
    this.onChangeFun = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouchedFun = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  ngOnInit(): void {
  }

  onChange(change: MatSelectChange){
    this.value = change.value;
    this.onChangeFun(this.value);
  }

  onBlur(){
    this.onTouchedFun();
  }

}
