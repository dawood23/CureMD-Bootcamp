import { Injectable } from '@angular/core';
import { Contact } from '../contact';

@Injectable({
  providedIn: 'root'
})
export class DataIndexService {
  private data_index: number = 0;
  private contacts: Contact[] = [];

  setIndex(value: number): void {
    this.data_index = value;
  }

  getIndex(): number {
    return this.data_index;
  }

  setContacts(contacts: Contact[]): void {
    this.contacts = contacts;
  }

  getContacts(): Contact[] {
    return this.contacts;
  }

  addContact(contact: Contact): void {
    this.contacts.push(contact);
    this.data_index = this.contacts.length+1;
  }

  updateContact(updated: Contact): void {
    const ind = this.contacts.findIndex(c => c.id === updated.id);
    if (ind !== -1) {
      this.contacts[ind] = updated;
    }
  }

}

