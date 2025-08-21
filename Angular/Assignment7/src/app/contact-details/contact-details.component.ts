import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Contact } from '../contact';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-contact-details',
  standalone: true,
  imports:[CommonModule],
  templateUrl: './contact-details.component.html',
  styleUrl: './contact-details.component.scss'
})
export class ContactDetailsComponent {
  @Input() contact: any;
  @Output() Toggle: EventEmitter<string> = new EventEmitter();

  toggle(group: string) {
    this.Toggle.emit(group);
  }
}
