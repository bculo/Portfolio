import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, tap } from 'rxjs';
import { SyncStatus } from 'src/app/models/backend/sync';

import * as fromRoot from 'src/app/store/index';
import * as fromSync from 'src/app/pages/manage/store/sync';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.scss']
})
export class DetailComponent implements OnInit, OnDestroy {

  status: SyncStatus;

  constructor(private router: ActivatedRoute, private store: Store<fromRoot.State>) { }

  ngOnDestroy(): void {

  }

  ngOnInit(): void {
    this.status = this.router.snapshot.data[0];
  }

}
