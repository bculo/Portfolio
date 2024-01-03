import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsStore } from '../../store/news-store';

@Component({
  selector: 'admin-dashboard-view-page',
  standalone: true,
  imports: [CommonModule],
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
