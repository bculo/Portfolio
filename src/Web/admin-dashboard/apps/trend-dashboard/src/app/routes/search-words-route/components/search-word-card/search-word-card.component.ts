import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchWordItem } from '../../models/search-words.model';

@Component({
  selector: 'admin-dashboard-search-word-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './search-word-card.component.html',
  styleUrl: './search-word-card.component.scss',
})
export class SearchWordCardComponent {
  @Input() searchWord!: SearchWordItem;
}
