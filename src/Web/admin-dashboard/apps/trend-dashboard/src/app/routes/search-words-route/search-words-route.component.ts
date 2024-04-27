import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { NavigationLayoutComponent } from '../../shared/components/navigation-layout/navigation-layout.component';
import { WebSocketService } from '../../shared/services/web-socket/web-socket.service';
import { environment } from '../../environments/environment';

const groups = environment.webSocketGroups;

@Component({
  selector: 'admin-dashboard-search-words-route',
  standalone: true,
  imports: [CommonModule, RouterOutlet, NavigationLayoutComponent],
  template: `<div><router-outlet></router-outlet></div>`,
})
export class SearchWordsRouteComponent {
  private readonly webSocketService = inject(WebSocketService);

  ngOnInit(): void {
    this.webSocketService.addToJoinQueue(groups.searchWordStatusChanged);
  }

  ngOnDestroy(): void {
    this.webSocketService.exitGroup(groups.searchWordStatusChanged);
  }
}
