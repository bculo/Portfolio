import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchWordItem } from '../../store/search-words.model';
import { SideModalComponent } from 'apps/trend-dashboard/src/app/shared/components/side-modal/side-modal.component';
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
export class SearchWordCardComponent {
  readonly searchWordStore = inject(SearchWordStore);

  @Input() searchWord!: SearchWordItem;

  @Output() onDetailsClick = new EventEmitter<SearchWordItem>();


  onDelete(): void {
    this.searchWordStore.deactivate(this.searchWord.id);
  }

  onActivate(): void {
    this.searchWordStore.activate(this.searchWord.id);
  }

  getDayDiff(): number {
    return dateDiffInDays(this.searchWord!.created!, new Date());
  } 

  onDetails(): void {
    this.onDetailsClick.emit(this.searchWord);
  }
}
