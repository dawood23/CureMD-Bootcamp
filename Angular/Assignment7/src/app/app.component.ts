import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Contact } from './contact';
import { ContactListComponent } from './contact-list/contact-list.component';
import { ContactDetailsComponent } from './contact-details/contact-details.component';
import { group } from '@angular/animations';

@Component({
  selector: 'app-root',
  imports:[ContactListComponent,ContactDetailsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  standalone:true
})
export class AppComponent implements OnInit{
  title:string="Assignment#7"
  contacts:Contact[]=[];
  selectedContact:any=[];

  ngOnInit(): void {
    this.contacts=this.generateMockContacts(50);
  }
 
generateMockContacts(count: number): Contact[] {
  const groups = ['Favourites', 'Family', 'Friends', 'Classmates'];

  const firstNames = [
    'Ali', 'Ayesha', 'Usman', 'Zain', 'Hassan', 'Shaheer', 'Ahmed',
    'Bilal', 'Kashif', 'Hammad', 'Tariq', 'Sharjeel', 'Noman',
    'Shoaib', 'Arif'
  ];

  const lastNames = [
    'Khan', 'Malik', 'Sheikh', 'Chaudhry', 'Qureshi', 'Butt', 'Rana',
    'Abbasi', 'Syed', 'Bhatti', 'Hashmi', 'Javed', 'Farooq', 'Hussain',
    'Rehman', 'Shah', 'Nawaz', 'Dar', 'Anwar', 'Yousaf'
  ];

  const cities = [
    'Karachi', 'Lahore', 'Islamabad', 'Rawalpindi', 'Faisalabad',
    'Multan', 'Peshawar', 'Quetta', 'Sialkot', 'Hyderabad'
  ];

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

  displayCardDetails(id:number){
      this.selectedContact=this.contacts[id];
  }

  toggle(str:string){
    if(!this.selectedContact){
        return;
      }
      
      const ind=this.contacts.findIndex(c=>c.id==this.selectedContact.id)

      if(ind==-1) return;

      if(this.contacts[ind].groups.includes(str)){
        this.contacts[ind].groups=this.contacts[ind].groups.filter(group=>group!=str)
      }
      else this.contacts[ind].groups=[...this.contacts[ind].groups,str]
  }

  
}
