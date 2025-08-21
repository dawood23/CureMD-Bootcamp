import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'highlight',
  standalone: true
})
export class HighlightPipe implements PipeTransform {

  transform(value: string, search:string) {
      if(!search ||!value) return value;

      const regex=new RegExp(search,'gi');


      return value.replace(regex,match=>`<mark>${match}</mark>`)
  }

}
