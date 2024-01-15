import { Injectable, WritableSignal, signal } from '@angular/core';
import { ActiveStep, Step } from './stepper.model';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StepperService {
  steps: WritableSignal<Step[]> = signal([]);
  activeStep: WritableSignal<ActiveStep | null> = signal(null);

  private onComplete = new Subject<void>();
  private onComplete$ = this.onComplete.asObservable();

  init(steps: Step[]): void {
    this.steps.set(steps);
    this.activeStep.set({ ...steps[0], index: 0 });
  }

  next() {
    const step = this.activeStep();
    const index = step!.index + 1;
    this.activeStep.set({ ...this.steps()[index], index })
  }

  previous() {
    const step = this.activeStep()!;
    const index = step!.index - 1;
    this.activeStep.set({ ...this.steps()[index], index })
  }  

  complete() {
    this.onComplete.next();
  }

  clear() {
    this.steps.set([]);
    this.activeStep.set(null);
  }
}
