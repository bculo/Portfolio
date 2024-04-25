import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject, debounceTime, distinctUntilChanged, takeUntil, tap } from 'rxjs';

@Component({
  selector: 'admin-dashboard-debounce-search',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './debounce-search.component.html',
  styleUrl: './debounce-search.component.scss',
})
export class DebounceSearchComponent implements OnInit, OnDestroy {
  @Output() onValueChange = new EventEmitter<string>();

  private lifecycle = new Subject<void>();
  private searchText = new Subject<string>();
  private searchText$ = this.searchText.asObservable();

  ngOnInit(): void {
    this.searchText$.pipe(
      takeUntil(this.lifecycle),
      debounceTime(700),
      distinctUntilChanged(),
      tap(text => this.onValueChange.emit(text))
    ).subscribe();
  }

  ngOnDestroy(): void {
    this.lifecycle.next();
    this.lifecycle.complete()
  } 

  onSearchChange(searchText: any): void {
    this.searchText.next(searchText);
  }
}
