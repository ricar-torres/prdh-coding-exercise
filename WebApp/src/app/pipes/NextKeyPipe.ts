import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'nextKey'
})
export class NextKeyPipe implements PipeTransform {
  transform(value: any, args?: any): any {
    const keys = Object.keys(value);
    const index = keys.indexOf(args);
    const nextIndex = index + 1;
    return keys[nextIndex];
  }
}
