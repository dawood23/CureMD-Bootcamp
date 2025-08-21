import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { Contact } from '../contact';
import { DataIndexService } from '../service/data-index.service';
import { CommonModule } from '@angular/common';
import { HighlightPipe } from '../dashboard/search-functionality/highlight.pipe';
@Component({
  selector: 'app-contact-list',
  standalone: true,
  imports:[CommonModule,HighlightPipe],
  templateUrl: './contact-list.component.html',
  styleUrl: './contact-list.component.scss'
})
export class ContactListComponent implements OnInit {
  private index = inject(DataIndexService);
  contactList: Contact[] = this.index.getContacts();

  @Output() id: EventEmitter<number> = new EventEmitter();
  @Input() searchTerm: string = "";  
  CurrentFilter: string = "All";

  ngOnInit(): void {
    this.contactList = this.index.getContacts();
  }

  onContactCardClick(contactid: number) {
    this.id.emit(contactid);
  }

  filter(filterby: string) {
    this.CurrentFilter = filterby;
  }

  filteredItem() {
    let filtered = this.contactList;

    if (this.CurrentFilter !== "All") {
      filtered = filtered.filter(contact =>
        contact.groups.includes(this.CurrentFilter)
      );
    }

    if (this.searchTerm && this.searchTerm.trim() !== "") {
      filtered = filtered.filter(contact =>
        contact.name.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
    }

    return filtered;
  }
}
