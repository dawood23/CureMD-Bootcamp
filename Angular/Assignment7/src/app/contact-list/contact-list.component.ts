import { Component,EventEmitter,Input,OnChanges,OnInit,Output } from '@angular/core';
import { Contact } from '../contact';
@Component({
  selector: 'app-contact-list',
  imports: [],
  templateUrl: './contact-list.component.html',
  styleUrl: './contact-list.component.scss',
  standalone:true
})
export class ContactListComponent{
  @Input() contactList:Contact[]=[]
  @Output() id:EventEmitter<number>=new EventEmitter();
   CurrentFilter:string="All"

  onContactCardClick(contactid:number){
      this.id.emit(contactid);
  }

  filter(filterby:string){
    this.CurrentFilter=filterby
  }

  filteredItem(){
    if(this.CurrentFilter=="All") return this.contactList
    else{
      return this.contactList.filter(contact=>(contact.groups.includes(this.CurrentFilter)))
    }
  }

}
