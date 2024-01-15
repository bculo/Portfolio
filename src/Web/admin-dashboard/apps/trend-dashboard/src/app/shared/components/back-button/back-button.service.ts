import { Injectable, inject } from '@angular/core';
import { Location } from "@angular/common";
import { NavigationEnd, Router } from '@angular/router';
import { Subject, takeUntil, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BackButtonService {
  private router = inject(Router);
  private location = inject(Location);

  private history: string[] = [];
  private lifeCycle = new Subject<void>();
 
  init() {
    this.router.events.pipe(
      takeUntil(this.lifeCycle),
      tap((event) => {
        if (event instanceof NavigationEnd) {
          this.history.push(event.urlAfterRedirects);
        }
      })
    ).subscribe();
  }

  destroy() {
    this.lifeCycle.next();
    this.lifeCycle.complete();
  }
 
  back(): void {
    this.history.pop();
    if (this.history.length > 0) {
      this.location.back();
    } else {
      this.router.navigateByUrl("/");
    }
  }
}
