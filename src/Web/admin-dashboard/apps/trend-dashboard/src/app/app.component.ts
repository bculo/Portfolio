import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthStore } from './store/auth-store';
import { NavigationLayoutComponent } from './layouts/navigation-layout/navigation-layout.component';


@Component({
  standalone: true,
  imports: [RouterModule, NavigationLayoutComponent],
  selector: 'admin-dashboard-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent { 
  readonly authStore = inject(AuthStore);
}
