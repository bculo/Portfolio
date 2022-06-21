import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SyncStatus } from 'src/app/pages/manage/store/sync/sync.models';

@Component({
  selector: 'app-list-item',
  templateUrl: './list-item.component.html',
  styleUrls: ['./list-item.component.scss']
})
export class ListItemComponent implements OnInit {

  @Input() item: SyncStatus; 

  constructor(private router: Router, private activedRoute: ActivatedRoute) { }

  ngOnInit(): void {
  }

  displayDetails(): void {
    this.router.navigate([`${this.item.id}`], {relativeTo: this.activedRoute })
  }
}
