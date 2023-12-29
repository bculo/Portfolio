import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'admin-dashboard-empty-layout',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './empty-layout.component.html',
  styleUrl: './empty-layout.component.scss',
})
export class EmptyLayoutComponent {}
