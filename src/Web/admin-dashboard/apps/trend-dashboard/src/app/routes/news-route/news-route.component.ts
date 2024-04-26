import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationLayoutComponent } from '../../shared/components/navigation-layout/navigation-layout.component';
import { RouterOutlet } from '@angular/router';
import { WebSocketService } from '../../shared/services/web-socket/web-socket.service';
import { environment } from '../../environments/environment';
import { Subject, filter, takeUntil, tap } from 'rxjs';
import { NewsStore } from './store/news-store';

@Component({
  selector: 'admin-dashboard-news-route',
  standalone: true,
  imports: [CommonModule, NavigationLayoutComponent, RouterOutlet],
  template: `<router-outlet></router-outlet>`,
})
export class NewsRouteComponent implements OnInit, OnDestroy {
  private readonly webSocketService = inject(WebSocketService);

  ngOnInit(): void {
    this.webSocketService.addToJoinQueue(environment.webSocketGroups.syncExecuted);
  }

  ngOnDestroy(): void {
    this.webSocketService.exitGroup(environment.webSocketGroups.syncExecuted);
  }
}

