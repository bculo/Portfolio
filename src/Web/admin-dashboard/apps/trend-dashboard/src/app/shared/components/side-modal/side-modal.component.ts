import { Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalInstance } from '../../models/modals.model';
import { ModalService } from '../../services/modal.service';
import { NgIconComponent } from '@ng-icons/core';


@Component({
  selector: 'admin-dashboard-side-modal',
  standalone: true,
  imports: [CommonModule, NgIconComponent],
  templateUrl: './side-modal.component.html',
  styleUrl: './side-modal.component.scss',
})
export class SideModalComponent implements OnInit, OnDestroy, ModalInstance {
  readonly modalService = inject(ModalService);
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
    this.removeListeners();
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

  private removeListeners(): void {
    this.element.removeListeners('click');
  }
}
