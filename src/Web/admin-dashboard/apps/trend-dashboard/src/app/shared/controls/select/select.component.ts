import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';


export interface ControlItem {
  value: number;
  displayValue: string
}

@Component({
  selector: 'admin-dashboard-select',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './select.component.html',
  styles: `
    :host {
      display: block;
    } 
  `,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: SelectComponent,
      multi: true,
    }
  ]
})
export class SelectComponent implements ControlValueAccessor {
  @Input() items: ControlItem[] = [];
  @Input() placeholder: string = '';
  
  value!: string;
  disabled: boolean = false;

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

  onBlur(){
    this.onTouchedFun();
  }
}
