import { Component, Input, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

export type InputType = 'text' | 'number'

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.scss'],
  providers: [
    {
        provide: NG_VALUE_ACCESSOR,
        useExisting: InputComponent,
        multi: true,
    },
  ]
})
export class InputComponent implements OnInit, ControlValueAccessor  {

  @Input() type: InputType;
  value: string;
  disabled: boolean;

  constructor() { }

  onChange: any = () => {};
  onTouch: any = () => {};

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

  ngOnInit(): void {
  }

  onUpdate(value: string){
    this.value = value;
    this.onChange(this.value);
  }

  onBlur() {
    this.onTouch();
  }

}
