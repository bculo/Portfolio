import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot,
  ActivatedRoute
} from '@angular/router';
import { Store } from '@ngrx/store';
import { map, filter, Observable, of, switchMap, take, tap } from 'rxjs';

import * as fromRoot from 'src/app/store/';
import * as fromSync from 'src/app/pages/manage/store/sync';
import { SyncStatus } from 'src/app/pages/manage/store/sync/sync.models';

@Injectable()
export class SyncResolver implements Resolve<SyncStatus> {

  constructor(private store: Store<fromRoot.State>, private router: Router, private activatedRoute: ActivatedRoute) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<SyncStatus> {
    console.log("HELLO");
    const syncId: string = route.params['id'];
    return this.store.select(fromSync.selectEntityById({id: syncId})).pipe(
      take(1),
      switchMap((status: SyncStatus) => {
        if(status) {
          return of(status);
        }

        this.store.dispatch(fromSync.fetchSyncItem({id: syncId}));
          return this.store.select(fromSync.getLoadingSync).pipe(
            filter(loading => !loading), 
            take(1),
            switchMap(() => {
              return this.store.select(fromSync.getAdditionallyFetchedItem).pipe(
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
