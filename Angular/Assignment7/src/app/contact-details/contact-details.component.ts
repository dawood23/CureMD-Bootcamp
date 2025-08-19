import { Component,EventEmitter,Input, Output } from '@angular/core';

@Component({
  selector: 'app-contact-details',
  standalone: true,
  imports: [],
  templateUrl: './contact-details.component.html',
  styleUrl: './contact-details.component.scss'
})
export class ContactDetailsComponent {
  @Input() contact:any
  @Output() Toggle:EventEmitter<string>=new EventEmitter();

  toggle(group:string){
      this.Toggle.emit(group)
  }

}
