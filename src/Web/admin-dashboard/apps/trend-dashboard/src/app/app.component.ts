import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { KeycloakService } from './shared/services/keycloak.service';
import { NewsService } from './shared/services/open-api';
import { take } from 'rxjs';

@Component({
  standalone: true,
  imports: [RouterModule],
  selector: 'admin-dashboard-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {

  constructor(private service: KeycloakService,
    private newsservice: NewsService) {

  }

  ngOnInit(): void {
    this.service.init()

    this.newsservice.getLastStockNews().pipe(
      take(1)
    ).subscribe();
  }
}
