import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsStore } from '../../store/news-store';
import { ArticleCardComponent } from '../../components/article-card/article-card.component';
import { DebounceSearchComponent } from 'apps/trend-dashboard/src/app/shared/components/debounce-search/debounce-search.component';
import { SpinnerComponent } from 'apps/trend-dashboard/src/app/shared/components/spinner/spinner.component';

@Component({
  selector: 'admin-dashboard-view-page',
  standalone: true,
  imports: [CommonModule, ArticleCardComponent, DebounceSearchComponent, SpinnerComponent],
  templateUrl: './view-page.component.html',
  styleUrl: './view-page.component.scss',
})
export class ViewPageComponent implements OnInit {
  readonly newsStore = inject(NewsStore);

  articles = this.newsStore.entities;
  deactivate = this.newsStore.deactivate
  isLoading = this.newsStore.isLoading

  ngOnInit(): void {

  }  

  onValueChange(value: string) {
    this.newsStore.fetch({query: value})
  }
}
