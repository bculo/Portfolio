import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgIconComponent } from '@ng-icons/core';
import { NAVIGATION_ITEMS } from '../../app.routes';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { DictionaryStore } from '../../store/dictionary-store';

@Component({
  selector: 'admin-dashboard-navigation-layout',
  standalone: true,
  imports: [CommonModule, NgIconComponent, RouterLink, RouterLinkActive],
  templateUrl: './navigation-layout.component.html',
  styleUrl: './navigation-layout.component.scss',
})
export class NavigationLayoutComponent {
  items = NAVIGATION_ITEMS
}
