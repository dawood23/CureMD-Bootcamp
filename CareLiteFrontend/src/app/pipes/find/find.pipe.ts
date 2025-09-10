import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'find',
})
export class FindPipe implements PipeTransform {
  transform(array: any[], property: string, value: any): any {
    if (!array || !property) return null;
    return array.find(item => item[property] === value);
  }
}