import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgIconComponent } from '@ng-icons/core';
import { NAVIGATION_ITEMS } from '../../app.routes';

@Component({
  selector: 'admin-dashboard-navigation-layout',
  standalone: true,
  imports: [CommonModule, NgIconComponent],
  templateUrl: './navigation-layout.component.html',
  styleUrl: './navigation-layout.component.scss',
})
export class NavigationLayoutComponent {
  items = NAVIGATION_ITEMS
}
