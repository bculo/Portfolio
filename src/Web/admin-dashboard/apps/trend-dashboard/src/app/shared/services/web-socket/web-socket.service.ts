import { Injectable, inject } from '@angular/core';
import * as signalR from '@microsoft/signalr'
import { KeycloakService } from '../keycloak/keycloak.service';
import { environment } from '../../../environments/environment';
import { Subject, Subscription, catchError, delayWhen, interval, of, take, tap, throwError } from 'rxjs';

const url = environment.websocket;

type WebSocketResponse = {
  groupName: string
  data: unknown
}

@Injectable({
  providedIn: 'root'
})
export class WebSocketService {
  private readonly keycloak = inject(KeycloakService);

  private connection: signalR.HubConnection = new signalR.HubConnectionBuilder()
      .withAutomaticReconnect()
      .withUrl(url, { accessTokenFactory: () => this.keycloak.authStore.authToken()! })
      .build();

  private groupQueue = new Subject<string>();
  private subscription?: Subscription;
  private isSocketReady: boolean = false;

  private serverResponse = new Subject<WebSocketResponse>();
  public serverResponse$ = this.serverResponse.asObservable();

  public connect() {
    this.connection.start()
      .then((_) => { this.isSocketReady = true; })
      .catch((error) => console.error(error))

      this.subscription = this.groupQueue.asObservable().pipe(
        delayWhen(groupName => !this.isSocketReady ? interval(1000) : of(groupName)),
        tap((groupName) => this.joinGroup(groupName))
      ).subscribe();
  }

  public addToJoinQueue(groupName: string) {
    this.groupQueue.next(groupName);
  }

  private joinGroup(groupName: string) {
    of(this.connection.invoke("JoinGroup", groupName)).pipe(
      take(1),
      tap((_) => console.log(`Connected to group ${groupName}`)),
      catchError((error) => throwError(() => error))
    )

    this.connection.on(groupName, data => {
      this.serverResponse.next({ groupName: groupName, data: data })
    });
  }

  public exitGroup(groupName: string) {
    if(this.connection.state !== signalR.HubConnectionState.Connected) {
      return;
    }

    of(this.connection.invoke("LeaveGroup", groupName)).pipe(
      tap((_) => console.log(`Disconnected from group ${groupName}`)),
      catchError((error) => throwError(() => error))
    )
  }

  public clean() {
    this.subscription?.unsubscribe();
    this.connection.stop();
  }
}
