import { Component, Input, OnInit } from '@angular/core';
import { SyncStatus } from '../../store/sync';

@Component({
  selector: 'app-sync-item',
  templateUrl: './sync-item.component.html',
  styleUrls: ['./sync-item.component.scss']
})
export class SyncItemComponent implements OnInit {

  @Input() item: SyncStatus; 

  constructor() { }

  ngOnInit(): void {
    
  }

}
