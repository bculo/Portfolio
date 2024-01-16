import { Directive, EventEmitter, HostListener, Output } from '@angular/core';

@Directive({
  selector: '[adminDashboardDragAndDrop]',
  standalone: true,
})
export class DragAndDropDirective {
  @Output() fileDropped = new EventEmitter<any>();

  @HostListener('drop', ['$event']) 
  public onDrop(event: any) {
    event.preventDefault();
    event.stopPropagation();
    const files = event.dataTransfer.files;
    if (files.length > 0) {
      this.fileDropped.emit(files);
    }
  }

  @HostListener('dragover', ['$event']) 
  public onDragOver(event: any) {
    event.preventDefault();
    event.stopPropagation();
  }

  @HostListener('dragleave', ['$event']) 
  public onDragLeave(event: any) {
    event.preventDefault();
    event.stopPropagation();
  }
}
