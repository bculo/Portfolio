import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot,
  ActivatedRoute
} from '@angular/router';
import { Store } from '@ngrx/store';
import { catchError, filter, Observable, of, switchMap, take, timeout } from 'rxjs';

import * as fromRoot from 'src/app/store/';
import * as fromSync from 'src/app/pages/manage/store/sync';
import { SyncStatus } from 'src/app/pages/manage/store/sync/sync.models';

@Injectable()
export class SyncResolver implements Resolve<SyncStatus> {

  constructor(private store: Store<fromRoot.State>, private router: Router, private activatedRoute: ActivatedRoute) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<SyncStatus> {
    const syncId: string = route.params['id'];
    return this.store.select(fromSync.selectEntityById({id: syncId})).pipe(
      take(1),
      switchMap((status: SyncStatus) => {
        if(status) {
          return of(status);
        }

        this.store.dispatch(fromSync.fetchSyncItem({id: syncId}));
        return this.store.select(fromSync.getAdditionallyFetchedItem).pipe(
          timeout(5000),
          filter(i => !!i), 
          take(1),
          catchError(() => {
            this.router.navigate(['/manage/sync'])
            return of(null);
          })
        );
      })
    );
  }

}
