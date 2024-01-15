import { Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SideModalService } from './side-modal.service';
import { NgIconComponent } from '@ng-icons/core';
import { SideModalInstance } from './side-modal.model';


@Component({
  selector: 'admin-dashboard-side-modal',
  standalone: true,
  imports: [CommonModule, NgIconComponent],
  templateUrl: './side-modal.component.html',
  styleUrl: './side-modal.component.scss',
})
export class SideModalComponent implements OnInit, OnDestroy, SideModalInstance {
  readonly modalService = inject(SideModalService);
  readonly elRef = inject(ElementRef);

  isOpen: boolean = false;

  @Input() id: string = ''; 

  @Output() onClose = new EventEmitter();  
  @Output() onOpen = new EventEmitter();  

  private element: any = this.elRef.nativeElement;

  ngOnInit(): void {
    this.modalService.add(this);
    document.body.appendChild(this.element);
    this.addListeners();
  }

  ngOnDestroy(): void {
    this.modalService.remove(this);
    this.element.remove();
  }

  open() {
    this.isOpen = true;
    this.onOpen.emit();
  }

  close() {
    this.isOpen = false;
    this.onClose.emit();
  }

  private addListeners(): void {
    this.element.addEventListener('click', (el: any) => {
      if (el.target.classList.contains('modal-background')) {
          this.close();
      }
    });
  }
}
