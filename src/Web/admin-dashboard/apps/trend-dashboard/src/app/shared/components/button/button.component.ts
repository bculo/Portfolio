import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

export type ButtonStyleType = 'empty' | 'full';
export type ButtonType = 'submit' | 'button';

@Component({
  selector: 'admin-dashboard-button',
  standalone: true,
  imports: [CommonModule],
  template: `
    @if(styleType === 'empty') {
      <button [type]="type" class="box-border text-cyan-700 px-2 py-2 border-cyan-700 border rounded-md min-w-32 transition-all delay-75 ease-out">{{text}}</button>
    }
    @else {
      <button [type]="type" class="box-border bg-cyan-700 px-4 py-2 hover:bg-cyan-800 rounded-md min-w-32 transition-all delay-75 ease-out">{{text}}</button>
    }
  `,
  styles: ``
})
export class ButtonComponent {
  @Input() type: ButtonType = 'button';
  @Input() styleType: ButtonStyleType = 'full';
  @Input() text: string = '';
}
