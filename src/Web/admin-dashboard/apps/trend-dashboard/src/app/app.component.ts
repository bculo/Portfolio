import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthStore } from './store/auth-store';


@Component({
  standalone: true,
  imports: [RouterModule],
  selector: 'admin-dashboard-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent { 
  readonly authStore = inject(AuthStore);
}
