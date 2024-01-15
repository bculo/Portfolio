import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgIconComponent } from '@ng-icons/core';
import { BackButtonService } from './back-button.service';

@Component({
  selector: 'admin-dashboard-back-button',
  standalone: true,
  imports: [CommonModule, NgIconComponent],
  templateUrl: './back-button.component.html',
  styleUrl: './back-button.component.scss',
})
export class BackButtonComponent {
  readonly backService = inject(BackButtonService);

  returnBack(): void {
    this.backService.back();
  }
}
