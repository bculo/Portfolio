import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NotificationComponent } from './components/notification/notification.component';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(private snackBar: MatSnackBar) { }

  error(message: string, duration: number = 5000) {
    const snackbar = this.snackBar.openFromComponent(NotificationComponent, {
      data: {
        message: message,
        close: () => { snackbar.dismiss() }
      },
      duration: duration,
      panelClass: ['snackbar', 'snackbar--error']
    });
  }

  success(message: string, duration: number = 5000) {
    const snackbar = this.snackBar.openFromComponent(NotificationComponent, {
      data: {
        message: message,
        close: () => { snackbar.dismiss() }
      },
      duration: duration,
      panelClass: ['snackbar', 'snackbar--success']
    });
  }
}
