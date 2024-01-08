import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

export type InputType = 'text' | 'number'

@Component({
  selector: 'admin-dashboard-input',
  standalone: true,
  imports: [CommonModule],
  providers: [
    {
        provide: NG_VALUE_ACCESSOR,
        useExisting: InputComponent,
        multi: true,
    },
  ],
  template: `
    <input 
      class="px-4 py-2 rounded-lg bg-gray-900 opacity-70 border-gray-700 border min-w-60 w-72 placeholder:italic placeholder:text-stone-600 inline-block"
      type="text" 
      [disabled]="disabled" 
      [value]="value" 
      [type]="type"
      [maxLength]="maxLength"
      (keyup)="onUpdate($any($event).target.value)"
      [placeholder]="placeholder"
      (blur)="onBlur()"/>
  `,
  styles: `
    :host {
      display: block;
    } 
  `,
})
export class InputComponent implements ControlValueAccessor {
  onChange: any = () => {};
  onTouch: any = () => {};

  @Input() type: InputType = 'text';
  @Input() placeholder: string = '';
  @Input() maxLength: number = 120;

  value: string = '';
  disabled: boolean = false;

  writeValue(value: string): void {
    this.value = value;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouch = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onUpdate(value: string){
    this.value = value;
    this.onChange(this.value);
  }

  onBlur() {
    this.onTouch();
  }
}
