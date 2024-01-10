import { Component, Input, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchWordItem } from '../../models/search-words.model';
import { SideModalComponent } from 'apps/trend-dashboard/src/app/shared/components/side-modal/side-modal.component';
import { ModalService } from 'apps/trend-dashboard/src/app/shared/services/modal.service';
import { NgIconComponent } from '@ng-icons/core';
import { SearchWordStore } from '../../store/search-word-store';
import { dateDiffInDays } from 'apps/trend-dashboard/src/app/shared/utilities/utilities';
import { SearchWordDetailComponent } from '../search-word-detail/search-word-detail.component';

@Component({
  selector: 'admin-dashboard-search-word-card',
  standalone: true,
  imports: [CommonModule, SideModalComponent, NgIconComponent, SearchWordDetailComponent],
  templateUrl: './search-word-card.component.html',
  styleUrl: './search-word-card.component.scss',
})
export class SearchWordCardComponent implements OnInit {

  readonly modalService = inject(ModalService);
  readonly searchWordStore = inject(SearchWordStore);

  @Input() searchWord!: SearchWordItem;

  ngOnInit(): void {
  }

  onDelete(): void {
    this.searchWordStore.deactivate(this.searchWord.id);
  }

  onActivate(): void {
    this.searchWordStore.activate(this.searchWord.id);
  }

  getDayDiff(): number {
    return dateDiffInDays(this.searchWord!.created!, new Date());
  } 

  modalClosed(): void {
    console.log("modalClosed")
  }
}
