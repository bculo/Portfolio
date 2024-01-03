import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsStore } from '../../store/news-store';
import { ArticleCardComponent } from '../../components/article-card/article-card.component';

@Component({
  selector: 'admin-dashboard-view-page',
  standalone: true,
  imports: [CommonModule, ArticleCardComponent],
  templateUrl: './view-page.component.html',
  styleUrl: './view-page.component.scss',
})
export class ViewPageComponent implements OnInit {
  readonly newsStore = inject(NewsStore);

  articles = this.newsStore.entities;
  deactivate = this.newsStore.deactivate

  ngOnInit(): void {

  }  
}
