import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WebSocketService } from '../../services/web-socket/web-socket.service';

@Component({
  selector: 'admin-dashboard-web-socket-component',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './web-socket-component.component.html',
  styleUrl: './web-socket-component.component.scss',
})
export class WebSocketComponentComponent implements OnInit, OnDestroy {
  private readonly webSocketService = inject(WebSocketService);

  ngOnInit(): void {
    this.webSocketService.connect();
  }

  ngOnDestroy(): void {
      this.webSocketService.clean();
  }
}
