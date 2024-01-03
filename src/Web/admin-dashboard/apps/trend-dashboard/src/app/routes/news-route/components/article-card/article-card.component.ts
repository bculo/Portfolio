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
  imageUrl: string = "https://images.unsplash.com/photo-1628238289656-6457bb6e6d76?q=80&w=2072&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
}
