import { ChangeDetectorRef, Component, Input, forwardRef, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, SelectControlValueAccessor } from '@angular/forms';
import { ControlItem } from '../../models/controls.model';

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
      useExisting: forwardRef(() => SelectComponent),
      multi: true,
    }
  ]
})
export class SelectComponent implements ControlValueAccessor {
  @Input() items: ControlItem[] = [];
  
  value?: number;
  disabled: boolean = false;

  onChangeFun: any = () => {};
  onTouchedFun: any = () => {};

  writeValue(obj: number): void {
    this.value = obj;
  }
  
  registerOnChange(fn: any): void {
    this.onChangeFun = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouchedFun = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  ngOnInit(): void {
  }

  onChange(value: number){
    this.value = +value;
    this.onChangeFun(this.value);
  }

  onBlur(){
    this.onTouchedFun();
  }
}
