import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { filter, take, tap } from 'rxjs';

import * as fromRoot from 'src/app/store/index';

import * as settingsSelectors from 'src/app/pages/manage/store/settings/settings.selectors';
import * as settingsActions from 'src/app/pages/manage/store/settings/settings.actions';

import * as dictionariesActions from 'src/app/pages/manage/store/dictionaries/dictionaries.actions';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {

  constructor(private store: Store<fromRoot.State>) { }

  ngOnInit(): void {
    this.store.select(settingsSelectors.selectTotal).pipe(
      take(1),
      filter(num => num == 0),
      tap(() => this.store.dispatch(settingsActions.settingsFetch()))
    ).subscribe();

    this.store.dispatch(dictionariesActions.fetchDictionaries());
  }

  addNewWord(): void {
    console.log("ADD NEW WORD");
  }

}
