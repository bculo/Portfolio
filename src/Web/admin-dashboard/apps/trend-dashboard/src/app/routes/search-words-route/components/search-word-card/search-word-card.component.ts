import { Component, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchWordItem } from '../../models/search-words.model';
import { SideModalComponent } from 'apps/trend-dashboard/src/app/shared/components/side-modal/side-modal.component';
import { ModalService } from 'apps/trend-dashboard/src/app/shared/services/modal.service';
import { NgIconComponent } from '@ng-icons/core';
import { SearchWordStore } from '../../store/search-word-store';

@Component({
  selector: 'admin-dashboard-search-word-card',
  standalone: true,
  imports: [CommonModule, SideModalComponent, NgIconComponent],
  templateUrl: './search-word-card.component.html',
  styleUrl: './search-word-card.component.scss',
})
export class SearchWordCardComponent {
  readonly modalService = inject(ModalService);
  readonly searchWordStore = inject(SearchWordStore);

  @Input() searchWord!: SearchWordItem;


  onDelete(): void {
    this.searchWordStore.deactivate(this.searchWord.id);
  }
}
