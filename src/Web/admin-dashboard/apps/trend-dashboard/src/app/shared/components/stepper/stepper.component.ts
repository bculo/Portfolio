import { Component, OnDestroy, OnInit, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StepperService } from './stepper.service';
import { ButtonComponent } from '../button/button.component';
import { Subject, takeUntil, tap } from 'rxjs';

@Component({
  selector: 'admin-dashboard-stepper',
  standalone: true,
  imports: [CommonModule, ButtonComponent],
  templateUrl: './stepper.component.html',
  styleUrl: './stepper.component.scss',
})
export class StepperComponent implements OnInit, OnDestroy {
  readonly stepperService = inject(StepperService);

  steps = this.stepperService.steps;
  activeStep = this.stepperService.activeStep;

  isLastStep = computed(() => this.activeStep()!.index === this.steps().length - 1);
  isFirstStep = computed(() => this.activeStep()!.index === 0);

  private lifeCycle = new Subject<void>();

  onNext() {
    this.stepperService.onCheck();
  }

  onComplete() {
    this.stepperService.onComplete();
  }

  onPrev() {
    this.stepperService.onPrevious();
  }

  ngOnInit(): void {
    this.stepperService.next$.pipe(
      takeUntil(this.lifeCycle),
      tap(x => this.stepperService.onNext())
    ).subscribe();
  }

  ngOnDestroy(): void {
    this.stepperService.clear();

    this.lifeCycle.next();
    this.lifeCycle.complete();
  }
}
