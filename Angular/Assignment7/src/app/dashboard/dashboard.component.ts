import { Component, inject, OnInit } from '@angular/core';
import { ContactListComponent } from '../contact-list/contact-list.component';
import { ContactDetailsComponent } from '../contact-details/contact-details.component';
import { Contact } from '../contact';
import { DataIndexService } from '../service/data-index.service';
import { RouterLink } from '@angular/router';
import { SearchFunctionalityComponent } from './search-functionality/search-functionality.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [ContactListComponent, ContactDetailsComponent,SearchFunctionalityComponent ,RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  title: string = "Assignment#7";
  contacts: Contact[] = [];
  selectedContact: Contact | null = null;
  currentFilter: string = "All";
  currentSearch: string = ""; 

  private index = inject(DataIndexService);

  ngOnInit(): void {
    if (this.index.getContacts().length === 0) {
    const generated = this.generateMockContacts(50);
    this.index.setContacts(generated);
    this.index.setIndex(generated.length+1);
  }

  this.contacts = this.index.getContacts();
  }

  generateMockContacts(count: number): Contact[] {
    const groups = ['Favourites', 'Family', 'Friends', 'Classmates'];
    const firstNames = ['Ali', 'Ayesha', 'Usman', 'Zain', 'Hassan', 'Shaheer', 'Ahmed',
      'Bilal', 'Kashif', 'Hammad', 'Tariq', 'Sharjeel', 'Noman', 'Shoaib', 'Arif'];
    const lastNames = ['Khan', 'Malik', 'Sheikh', 'Chaudhry', 'Qureshi', 'Butt', 'Rana',
      'Abbasi', 'Syed', 'Bhatti', 'Hashmi', 'Javed', 'Farooq', 'Hussain',
      'Rehman', 'Shah', 'Nawaz', 'Dar', 'Anwar', 'Yousaf'];
    const cities = ['Karachi', 'Lahore', 'Islamabad', 'Rawalpindi', 'Faisalabad',
      'Multan', 'Peshawar', 'Quetta', 'Sialkot', 'Hyderabad'];

    const data: Contact[] = [];
    for (let i = 1; i <= count; i++) {
      const first = firstNames[Math.floor(Math.random() * firstNames.length)];
      const last = lastNames[Math.floor(Math.random() * lastNames.length)];
      const city = cities[Math.floor(Math.random() * cities.length)];
      data.push({
        id: i,
        name: `${first} ${last}`,
        phone: `0300-0000${i}`,
        email: `${first.toLowerCase()}.${last.toLowerCase()}${i}@gmail.com`,
        gender: i % 2 === 0 ? 'Male' : 'Female',
        address: `House #${Math.floor(Math.random() * 500)}, ${city}`,
        groups: groups.filter(() => Math.random() > 0.6)
      });
    }
    return data;
  }

  displayCardDetails(id: number) {
    const ind = this.contacts.findIndex(c => c.id == id);
    this.selectedContact = this.contacts[ind];
  }

  toggle(str: string) {
    if (!this.selectedContact) return;

    const ind = this.contacts.findIndex(c => c.id == this.selectedContact!.id);
    if (ind == -1) return;

    if (this.contacts[ind].groups.includes(str)) {
      this.contacts[ind].groups =
        this.contacts[ind].groups.filter(group => group != str);
    } else {
      this.contacts[ind].groups = [...this.contacts[ind].groups, str];
    }

    this.index.updateContact(this.contacts[ind]); 
  }

 searchEvent(str: string) {
    this.currentSearch = str;   
  }
}
