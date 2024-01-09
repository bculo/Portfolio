import { Injectable } from '@angular/core';
import { ModalInstance } from '../models/modals.model';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private modals: ModalInstance[] = [];

  add(modal: ModalInstance) {
      if (!modal.id || this.modals.find(x => x.id === modal.id)) {
          throw new Error('modal must have a unique id attribute');
      }
      this.modals.push(modal);
  }

  remove(modal: ModalInstance) {
      this.modals = this.modals.filter(x => x === modal);
  }

  open(id: string) {
      const modal = this.modals.find(x => x.id === id);
      modal?.open();
  }

  close(id: string) {
      const modal = this.modals.find(x => x.isOpen && x.id);
      modal?.close();
  }
}
