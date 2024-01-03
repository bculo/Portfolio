import { NgIconComponent } from '@ng-icons/core';
import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Article } from '../../models/news.model';

@Component({
  selector: 'admin-dashboard-article-card',
  standalone: true,
  imports: [CommonModule, NgIconComponent],
  templateUrl: './article-card.component.html',
  styleUrl: './article-card.component.scss',
})
export class ArticleCardComponent {
  @Input() article!: Article;
}
