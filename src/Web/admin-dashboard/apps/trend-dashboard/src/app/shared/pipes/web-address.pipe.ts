import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'webAddress',
  standalone: true,
})
export class WebAddressPipe implements PipeTransform {
  transform(value: string, ...args: unknown[]): string {
    return value.replace(/.+\/\/|www.|\..+/g, '')
  }
}
