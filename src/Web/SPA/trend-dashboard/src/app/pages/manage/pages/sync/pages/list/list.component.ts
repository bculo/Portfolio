import { Component, OnInit } from '@angular/core';
import { Observable, take, tap, filter, Subscription } from 'rxjs';
import { Store } from '@ngrx/store';

import * as fromRoot from 'src/app/store';

import * as syncSelectors from 'src/app/pages/manage/store/sync/sync.selectors';
import * as syncModels from 'src/app/pages/manage/store/sync/sync.models';
import * as syncActions from 'src/app/pages/manage/store/sync/sync.actions';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

  syncItems$: Observable<syncModels.SyncStatus[]>;
  executingSync$: Observable<boolean>;
  fetchingItems$: Observable<boolean>;
  loadMoreAvailable$: Observable<boolean>

  subscription: Subscription;

  constructor(private store: Store<fromRoot.State>) { }

  ngOnInit(): void {
    
    this.store.select(syncSelectors.selectTotal).pipe(
      take(1),
      filter(num => num == 0),
      tap(() => this.store.dispatch(syncActions.fetchStatuses()))
    ).subscribe();

    this.loadMoreAvailable$ = this.store.select(syncSelectors.fetchingItemsAvailableSync);
    this.syncItems$ = this.store.select(syncSelectors.selectAll);
    this.executingSync$ = this.store.select(syncSelectors.getExecutingSync);
    this.fetchingItems$ = this.store.select(syncSelectors.getLoadingSync);
  }

  onSync(): void {
    this.store.dispatch(syncActions.sync());
  }

  onLoad(): void {
    this.store.select(syncSelectors.canLoadNextPageSync).pipe(
      take(1),
      filter((canLoad) => canLoad),
      tap(_ => this.store.dispatch(syncActions.fetchStatuses()))
    ).subscribe();
  }

}
