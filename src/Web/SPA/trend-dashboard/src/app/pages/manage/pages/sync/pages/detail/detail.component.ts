import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SyncStatus } from 'src/app/models/backend/sync';

import { Location } from '@angular/common'

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.scss']
})
export class DetailComponent implements OnInit, OnDestroy {

  status: SyncStatus;

  constructor(private router: ActivatedRoute, private location: Location) { }

  ngOnDestroy(): void {

  }

  ngOnInit(): void {
    this.status = this.router.snapshot.data[0];
  }

  goBack(): void {
    this.location.back();
  }

}
