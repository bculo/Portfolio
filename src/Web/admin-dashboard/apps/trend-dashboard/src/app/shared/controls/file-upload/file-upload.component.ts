import { Component, ElementRef, ViewChild, forwardRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DragAndDropDirective } from '../../directives/drag-and-drop.directive';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'admin-dashboard-file-upload',
  standalone: true,
  imports: [CommonModule, DragAndDropDirective],
  templateUrl: './file-upload.component.html',
  styleUrl: './file-upload.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FileUploadComponent),
      multi: true,
    }
  ]
})
export class FileUploadComponent implements ControlValueAccessor  {
  @ViewChild("fileDropRef") fileDropEl!: ElementRef;

  value?: File;
  disabled: boolean = false;
  
  onChangeFun: any = () => {};
  onTouchedFun: any = () => {};

  onFileDropped(event: any) {
    console.log(event);
  }

  onFileSelected(files: FileList) {
    if(files.length === 0) return;
    this.onChangeFun(files[0]);
  }

  writeValue(value: File): void {
    this.value = value;
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
}
