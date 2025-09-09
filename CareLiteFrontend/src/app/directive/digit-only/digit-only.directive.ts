import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: '[appDigitOnly]',
  standalone: true
})
export class DigitOnlyDirective {

  constructor() { }

  @HostListener('input',['$event']) onInput(event:Event){
    const input = event.target as HTMLInputElement

    const check=input.value.replace(/[^0-9]+/g,'');

    if(input.value!=check){
      input.value=check
    }
  }
}
