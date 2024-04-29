import { NgIconComponent } from '@ng-icons/core';
import { Component, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Article } from '../../models/news.model';
import { WebAddressPipe } from 'apps/trend-dashboard/src/app/shared/pipes/web-address.pipe';
import { NewsStore } from '../../store/news-store';

@Component({
  selector: 'admin-dashboard-article-card',
  standalone: true,
  imports: [CommonModule, NgIconComponent, WebAddressPipe],
  templateUrl: './article-card.component.html',
  styleUrl: './article-card.component.scss',
})
export class ArticleCardComponent {
  private readonly newsStore = inject(NewsStore);
  @Input() article!: Article;

  date = new Date()

  onActivate() {
    this.newsStore.activate(this.article.id);
  }

  onDeactivate() {
    this.newsStore.deactivate(this.article.id);
  }
}
