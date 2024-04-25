import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileUploadComponent } from 'apps/trend-dashboard/src/app/shared/controls/file-upload/file-upload.component';
import { SearchWordStore } from '../../store/search-word-store';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormFieldComponent } from 'apps/trend-dashboard/src/app/shared/controls/form-field/form-field.component';
import { StepperService } from 'apps/trend-dashboard/src/app/shared/components/stepper/stepper.service';
import { Subject, takeUntil, tap } from 'rxjs';

@Component({
  selector: 'admin-dashboard-new-search-word-image-form',
  standalone: true,
  imports: [CommonModule, FileUploadComponent, ReactiveFormsModule, FormFieldComponent],
  templateUrl: './new-search-word-image-form.component.html',
  styleUrl: './new-search-word-image-form.component.scss',
})
export class NewSearchWordImageFormComponent implements OnInit {
  readonly wordStore = inject(SearchWordStore);
  readonly formBuilder = inject(FormBuilder);
  readonly stepperService = inject(StepperService);

  newItem = this.wordStore.newItem;

  private lifecycle = new Subject<void>();

  form: FormGroup = this.formBuilder.group({
    image: new FormControl<File | null>(null, { updateOn: 'change', validators: [Validators.required] }),
  });

  ngOnInit(): void {
    this.checkForExistingItem();

    this.stepperService.check$.pipe(
      takeUntil(this.lifecycle),
      tap(() => this.onSubmit())
    ).subscribe()
  }

  checkForExistingItem(): void {
    const submittedItem = this.newItem();
    if(submittedItem) {
      this.form.patchValue({image: submittedItem.file});
    }
  }

  ngOnDestroy(): void {
    this.lifecycle.next();
    this.lifecycle.complete();
  }

  onSubmit() {
    this.form.markAllAsTouched();
    if(!this.form.valid) {
      this.stepperService.onCheckHandled(false);
      return;
    }

    this.wordStore.attachImageToLocalItem(this.form.value.image);
    this.stepperService.onCheckHandled(true);
  }
}
