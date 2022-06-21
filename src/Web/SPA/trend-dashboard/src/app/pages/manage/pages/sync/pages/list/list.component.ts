import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subject, takeUntil, take, tap, filter } from 'rxjs';
import { Store } from '@ngrx/store';

import * as fromRoot from 'src/app/store';

import { SyncStatus } from '../../../../store/sync/sync.models';
import * as fromSync from '../../../../store/sync';
import { NotificationService } from 'src/app/services/notification/notification.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

  syncItems$: Observable<SyncStatus[]>;
  executingSync$: Observable<boolean>;

  constructor(private store: Store<fromRoot.State>, private notification: NotificationService) { }


  ngOnInit(): void {
    
    this.store.select(fromSync.selectTotal).pipe(
      take(1),
      filter(num => num == 0),
      tap(() => this.store.dispatch(fromSync.fetchStatuses()))
    ).subscribe();

    this.syncItems$ = this.store.select(fromSync.selectAll);
    this.executingSync$ = this.store.select(fromSync.getExecutingSync).pipe(
      tap((result) => {
        if(result) this.store.dispatch(fromSync.fetchStatuses());
      })
    );
  }

  onSync(): void {
    this.store.dispatch(fromSync.sync());
  }

  onLoad(): void {
    
    this.notification.success("test for success message");
  }
}
