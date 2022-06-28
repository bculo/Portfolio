import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { filter, Observable, take, tap } from 'rxjs';

import * as fromRoot from 'src/app/store/index';

import * as settingsSelectors from 'src/app/pages/manage/store/settings/settings.selectors';
import * as settingsActions from 'src/app/pages/manage/store/settings/settings.actions';
import { Setting } from '../../store/settings/settings.models';

import * as dictionariesActions from 'src/app/pages/manage/store/dictionaries/dictionaries.actions';
import * as dictionariesSelectors from 'src/app/pages/manage/store/dictionaries/dictionaries.selectors';
import { MatDialog } from '@angular/material/dialog';
import { SearchWordFormComponent } from './components/search-word-form/search-word-form.component';


@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {

  itemsLoaded$: Observable<boolean>;
  loadingSearchWords$: Observable<boolean>;

  items$: Observable<Setting[]>;

  constructor(private store: Store<fromRoot.State>, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.items$ = this.store.select(settingsSelectors.selectAll);
    this.itemsLoaded$ = this.store.select(dictionariesSelectors.areLoaded);
    this.loadingSearchWords$ = this.store.select(settingsSelectors.loading);

    this.store.select(settingsSelectors.selectTotal).pipe(
      take(1),
      filter(num => num == 0),
      tap(() => this.store.dispatch(settingsActions.settingsFetch()))
    ).subscribe();

    this.store.select(dictionariesSelectors.shouldLoad).pipe(
      tap(result => result),
      take(1),
      filter(load => load),
      tap(() => this.store.dispatch(dictionariesActions.fetchDictionaries()))
    ).subscribe();
  }

  addNewWord(): void {
    this.dialog.open(SearchWordFormComponent, {
      height: 'auto',
      width: '60rem',
      panelClass: 'modal',
      autoFocus: false
    });
  }

  refresh(): void {
    this.store.dispatch(settingsActions.settingsFetch());
  }
}
