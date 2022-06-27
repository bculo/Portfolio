import { Component, Input, OnInit } from '@angular/core';

import * as settingsModels from 'src/app/pages/manage/store/settings/settings.models';

@Component({
  selector: 'app-search-word-item',
  templateUrl: './search-word-item.component.html',
  styleUrls: ['./search-word-item.component.scss']
})
export class SearchWordItemComponent implements OnInit {

  @Input() setting: settingsModels.Setting

  constructor() { }

  ngOnInit(): void {
  }

}
