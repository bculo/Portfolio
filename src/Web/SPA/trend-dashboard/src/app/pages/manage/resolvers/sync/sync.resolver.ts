import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot,
  ActivatedRoute
} from '@angular/router';
import { Store } from '@ngrx/store';
import { map, filter, Observable, of, switchMap, take } from 'rxjs';

import * as fromRoot from 'src/app/store/';

import * as syncSelectors from 'src/app/pages/manage/store/sync/sync.selectors';
import * as syncActions from 'src/app/pages/manage/store/sync/sync.actions';
import * as syncModels from 'src/app/pages/manage/store/sync/sync.models';

@Injectable()
export class SyncResolver implements Resolve<syncModels.SyncStatus> {

  constructor(private store: Store<fromRoot.State>, private router: Router, private activatedRoute: ActivatedRoute) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<syncModels.SyncStatus> {
    const syncId: string = route.params['id'];
    return this.store.select(syncSelectors.selectEntityById({id: syncId})).pipe(
      take(1),
      switchMap((status: syncModels.SyncStatus) => {
        if(status) {
          return of(status);
        }

        this.store.dispatch(syncActions.fetchSyncItem({id: syncId}));
        return this.store.select(syncSelectors.getLoadingSync).pipe(
          filter(loading => !loading), 
          take(1),
          switchMap(() => {
            return this.store.select(syncSelectors.getAdditionallyFetchedItem).pipe(
              take(1),
              map((status) => {
                if(status) return status;
                this.router.navigate(["/manage/sync"]);
                return null;
              })
            )
          })
        );
      })
    );
  }

}
