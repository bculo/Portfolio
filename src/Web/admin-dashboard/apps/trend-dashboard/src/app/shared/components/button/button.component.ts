import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

export type ButtonStyleType = 'empty' | 'full';
export type ButtonType = 'submit' | 'button';

@Component({
  selector: 'admin-dashboard-button',
  standalone: true,
  imports: [CommonModule],
  template: `
    @if(styleType === 'empty') {
      <button [type]="type" class="text-cyan-700 px-2 py-2 border-cyan-700 border rounded-md min-w-32 transition-all delay-75 ease-out">{{text}}</button>
    }
    @else {
      <button [type]="type" class="bg-cyan-700 px-4 py-2 hover:bg-cyan-700 rounded-md min-w-32 transition-all delay-75 ease-out">{{text}}</button>
    }
  `,
  styles: `
    :host {
        display: block;
    }
  `
})
export class ButtonComponent {
  @Input() type: ButtonType = 'button';
  @Input() styleType: ButtonStyleType = 'full';
  @Input() text: string = '';
}
