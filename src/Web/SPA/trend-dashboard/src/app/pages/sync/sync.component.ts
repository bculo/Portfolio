import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';

import * as fromRoot from 'src/app/store';

import * as fromSync from './store/sync';
import { SyncStatus } from './store/sync';

@Component({
  selector: 'app-sync',
  templateUrl: './sync.component.html',
  styleUrls: ['./sync.component.scss']
})
export class SyncComponent implements OnInit {

  statuses$: Observable<SyncStatus[]>;
  loading$: Observable<boolean>;

  constructor(private store: Store<fromRoot.State>) { }

  ngOnInit(): void {
    this.statuses$ = this.store.select(fromSync.getItems);
    this.loading$ = this.store.select(fromSync.getLoading);
  }

  test(): void {
    this.store.dispatch(fromSync.fetchStatuses());
  }

}
