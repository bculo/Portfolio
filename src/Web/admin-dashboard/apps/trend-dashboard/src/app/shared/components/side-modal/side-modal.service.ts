import { Injectable } from '@angular/core';
import { SideModalInstance } from './side-modal.model';

@Injectable({
  providedIn: 'root'
})
export class SideModalService {
  private modals: SideModalInstance[] = [];

  add(modal: SideModalInstance) {
    if (!modal.id || this.modals.find(x => x.id === modal.id)) {
      this.remove(modal);
    }
    this.modals.push(modal);
  }

  remove(modal: SideModalInstance) {
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

  visible(id: string): boolean {
    const modal = this.modals.find(x => x.isOpen && x.id);
    return modal?.isOpen ?? false;
  }
}
