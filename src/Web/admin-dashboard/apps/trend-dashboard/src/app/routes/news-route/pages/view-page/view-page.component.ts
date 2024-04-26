import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsStore } from '../../store/news-store';
import { ArticleCardComponent } from '../../components/article-card/article-card.component';
import { DebounceSearchComponent } from 'apps/trend-dashboard/src/app/shared/components/debounce-search/debounce-search.component';
import { SpinnerComponent } from 'apps/trend-dashboard/src/app/shared/components/spinner/spinner.component';
import { WebSocketService } from 'apps/trend-dashboard/src/app/shared/services/web-socket/web-socket.service';
import { Subject, filter, takeUntil, tap } from 'rxjs';
import { environment } from 'apps/trend-dashboard/src/app/environments/environment';

@Component({
  selector: 'admin-dashboard-view-page',
  standalone: true,
  imports: [CommonModule, ArticleCardComponent, DebounceSearchComponent, SpinnerComponent],
  templateUrl: './view-page.component.html',
  styleUrl: './view-page.component.scss',
})
export class ViewPageComponent implements OnInit {
  private readonly newsStore = inject(NewsStore);
  private readonly webSocketService = inject(WebSocketService);

  private lifecycle = new Subject<void>();
  private searchValue: string = '';

  articles = this.newsStore.entities;
  deactivate = this.newsStore.deactivate
  isLoading = this.newsStore.isLoading

  ngOnInit(): void {
    this.webSocketService.serverResponse$.pipe(
      takeUntil(this.lifecycle),
      filter(x => x.groupName === environment.webSocketGroups.syncExecuted),
      tap(data => this.newsStore.fetch({query: this.searchValue}))
    ).subscribe();
  }  

  ngOnDestroy(): void {
    this.lifecycle.next();
    this.lifecycle.complete();
  }

  onValueChange(value: string) {
    this.searchValue = value;
    this.newsStore.fetch({query: this.searchValue})
  }
}
