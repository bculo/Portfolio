import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BackButtonComponent } from 'apps/trend-dashboard/src/app/shared/components/back-button/back-button.component';
import { StepperComponent } from 'apps/trend-dashboard/src/app/shared/components/stepper/stepper.component';
import { StepperService } from 'apps/trend-dashboard/src/app/shared/components/stepper/stepper.service';
import { NewSearchWordFormComponent } from '../../components/new-search-word-form/new-search-word-form.component';

@Component({
  selector: 'admin-dashboard-create-new',
  standalone: true,
  imports: [CommonModule, BackButtonComponent, StepperComponent, NewSearchWordFormComponent],
  templateUrl: './create-new.component.html',
  styleUrl: './create-new.component.scss',
})
export class CreateNewComponent implements OnInit {
  readonly stepperService = inject(StepperService);

  activeStep = this.stepperService.activeStep;

  ngOnInit(): void {
    this.stepperService.init([
      { key: 'Information', label: 'Information'},
      { key: 'Picture', label: 'Picture'},
      { key: 'Verify', label: 'Verify'},
    ])
  }
}
