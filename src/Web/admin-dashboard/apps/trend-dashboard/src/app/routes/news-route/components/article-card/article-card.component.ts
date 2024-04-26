import { NgIconComponent } from '@ng-icons/core';
import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Article } from '../../models/news.model';
import { WebAddressPipe } from 'apps/trend-dashboard/src/app/shared/pipes/web-address.pipe';

@Component({
  selector: 'admin-dashboard-article-card',
  standalone: true,
  imports: [CommonModule, NgIconComponent, WebAddressPipe],
  templateUrl: './article-card.component.html',
  styleUrl: './article-card.component.scss',
})
export class ArticleCardComponent {
  @Input() article!: Article;

  date = new Date()
}
