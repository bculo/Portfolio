import { Injectable, WritableSignal, signal } from '@angular/core';
import { ActiveStep, Step } from './stepper.model';
import { Subject, filter } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StepperService {
  steps: WritableSignal<Step[]> = signal([]);
  activeStep: WritableSignal<ActiveStep | null> = signal(null);

  private complete = new Subject<void>();
  complete$ = this.complete.asObservable();

  private next = new Subject<boolean>();
  next$ = this.next.asObservable().pipe(
    filter(isOk => isOk)
  );

  private check = new Subject<void>();
  check$ = this.check.asObservable();

  init(steps: Step[]): void {
    this.steps.set(steps);
    this.activeStep.set({ ...steps[0], index: 0 });
  }

  onNext() {
    const step = this.activeStep();
    const index = step!.index + 1;
    this.activeStep.set({ ...this.steps()[index], index })
  }

  onPrevious() {
    const step = this.activeStep()!;
    const index = step!.index - 1;
    this.activeStep.set({ ...this.steps()[index], index })
  }  

  onComplete() {
    this.complete.next();
  }

  onCheck() {
    this.check.next();
  }

  onCheckHandled(valid: boolean) {
    this.next.next(valid);
  }

  clear() {
    this.steps.set([]);
    this.activeStep.set(null);
  }
}
