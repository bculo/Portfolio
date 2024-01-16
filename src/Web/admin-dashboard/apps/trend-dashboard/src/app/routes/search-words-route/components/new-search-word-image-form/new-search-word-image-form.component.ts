import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileUploadComponent } from 'apps/trend-dashboard/src/app/shared/controls/file-upload/file-upload.component';
import { SearchWordStore } from '../../store/search-word-store';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormFieldComponent } from 'apps/trend-dashboard/src/app/shared/controls/form-field/form-field.component';

@Component({
  selector: 'admin-dashboard-new-search-word-image-form',
  standalone: true,
  imports: [CommonModule, FileUploadComponent, ReactiveFormsModule, FormFieldComponent],
  templateUrl: './new-search-word-image-form.component.html',
  styleUrl: './new-search-word-image-form.component.scss',
})
export class NewSearchWordImageFormComponent {
  readonly wordStore = inject(SearchWordStore);
  readonly formBuilder = inject(FormBuilder);

  newItem = this.wordStore.newItem;

  form: FormGroup = this.formBuilder.group({
    image: new FormControl<File | null>(null, { updateOn: 'change', validators: [Validators.required] }),
  });

  onSubmit() {
    console.log(this.form.value);
  }
}
