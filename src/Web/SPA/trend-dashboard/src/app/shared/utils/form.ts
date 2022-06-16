import { FormGroup } from "@angular/forms";

export function markFormAsTouched(form: FormGroup) {
    for(var control in form.controls) {
        form.controls[control].markAsTouched();
    }
}