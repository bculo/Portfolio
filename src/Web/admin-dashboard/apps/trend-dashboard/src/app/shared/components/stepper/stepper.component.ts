import { Component, OnDestroy, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StepperService } from './stepper.service';

@Component({
  selector: 'admin-dashboard-stepper',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './stepper.component.html',
  styleUrl: './stepper.component.scss',
})
export class StepperComponent implements OnDestroy {
  readonly stepperService = inject(StepperService);

  steps = this.stepperService.steps;
  activeStep = this.stepperService.activeStep;

  isLastStep = computed(() => this.activeStep()!.index === this.steps().length - 1);
  isFirstStep = computed(() => this.activeStep()!.index === 0);

  onNext() {
    this.stepperService.next();
  }

  onComplete() {
    this.stepperService.complete();
  }

  onPrev() {
    this.stepperService.previous();
  }

  ngOnDestroy(): void {
    this.stepperService.clear();
  }
}
