import { Component, OnInit } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { Store } from '@ngrx/store';

import * as fromRoot from 'src/app/store';

import { SyncStatus } from '../../../../store/sync/sync.models';
import * as fromSync from '../../../../store/sync';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

  syncItems$: Observable<SyncStatus[]>;
  executingSync$: Observable<boolean>;

  constructor(private store: Store<fromRoot.State>) { }

  ngOnInit(): void {
    this.store.dispatch(fromSync.fetchStatuses());
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
    console.log("LOAD");
    
  }
}
