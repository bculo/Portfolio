import { Component, Input, OnDestroy, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchWordItem } from '../../models/search-words.model';
import { SearchWordStore } from '../../store/search-word-store';

@Component({
  selector: 'admin-dashboard-search-word-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './search-word-detail.component.html',
  styleUrl: './search-word-detail.component.scss',
})
export class SearchWordDetailComponent implements OnInit {
  readonly searchWordStore = inject(SearchWordStore);

  @Input() item: SearchWordItem | null = null;


  ngOnInit(): void {
    console.log("ngOnInit")
  }
}
