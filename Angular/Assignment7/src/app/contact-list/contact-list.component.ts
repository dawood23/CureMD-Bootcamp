import { Component,EventEmitter,Input,OnInit,Output } from '@angular/core';
import { Contact } from '../contact';
@Component({
  selector: 'app-contact-list',
  imports: [],
  templateUrl: './contact-list.component.html',
  styleUrl: './contact-list.component.scss',
  standalone:true
})
export class ContactListComponent implements OnInit{
  @Input() contactList:Contact[]=[]
  @Output() id:EventEmitter<number>=new EventEmitter();
  filteredList:Contact[]=[]

  onContactCardClick(contactid:number){
      this.id.emit(contactid);
  }

  ngOnInit(): void {
    this.filteredList=this.contactList
  }
  filter(filterby:string){
    this.filteredList=this.contactList.filter(contact=>(contact.groups.includes(filterby)))
  }
  noFilter(){
    this.filteredList=this.contactList
  }


}
