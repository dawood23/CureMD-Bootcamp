import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[appSearchBar]',
  standalone: true
})
export class SearchBarDirective {

  constructor(private el:ElementRef) { }

  @HostListener('input',['$event']) onInput(event:Event){

    const input=event.target as HTMLInputElement
    const check=input.value.replace(/[^a-zA-Z ]+/g, '');

    if(input.value!=check){
      input.value=check
    }
   
  }
}

