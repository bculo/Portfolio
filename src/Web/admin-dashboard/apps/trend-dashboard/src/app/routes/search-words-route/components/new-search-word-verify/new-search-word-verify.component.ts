import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchWordStore } from '../../store/search-word-store';
import { StepperService } from 'apps/trend-dashboard/src/app/shared/components/stepper/stepper.service';
import { Subject, takeUntil, tap } from 'rxjs';

@Component({
  selector: 'admin-dashboard-new-search-word-verify',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './new-search-word-verify.component.html',
  styleUrl: './new-search-word-verify.component.scss',
})
export class NewSearchWordVerifyComponent implements OnInit, OnDestroy {
  readonly wordStore = inject(SearchWordStore);
  readonly stepperService = inject(StepperService);
  url: string | null = null;

  newItem = this.wordStore.newItem;

  private lifecycle = new Subject<void>();

  ngOnInit(): void {
    this.stepperService.complete$.pipe(
      takeUntil(this.lifecycle),
      tap(() => this.wordStore.createWord())
    ).subscribe()

    const file = this.newItem()?.file;
    if(!file) return;  
    this.url = URL.createObjectURL(file)
  }

  ngOnDestroy(): void {
    if(!this.url) return;
    URL.revokeObjectURL(this.url);
  }
}
