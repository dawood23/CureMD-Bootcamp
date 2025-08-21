import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormControl } from '@angular/forms';
import { DataIndexService } from '../service/data-index.service';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-contact-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './contact-page.component.html',
  styleUrls: ['./contact-page.component.scss']
})
export class ContactPageComponent {
  formData:any;
  index=inject(DataIndexService)
  router=inject(Router)

  constructor(public fb: FormBuilder) {}

  groupOptions = ['Favourites', 'Family', 'Friends', 'Classmates'];

  form = this.fb.group({
    id: this.fb.control<number | null>(null),
    name: this.fb.nonNullable.control('', [Validators.required]),
    phone: this.fb.nonNullable.control('', [
      Validators.required,
      Validators.pattern(/^\d{4}-\d{7}$/)
    ]),
    email: this.fb.nonNullable.control('', [
      Validators.required,
      Validators.email
    ]),
    gender: this.fb.nonNullable.control<'male' | 'female'>('male', [
      Validators.required
    ]),
    address: this.fb.control('', Validators.required), 
    groups: this.fb.array(
      this.groupOptions.map(() => this.fb.control(false))
    )
  });
  

  get f() {
    return this.form.controls;
  }

  onSubmit() {
    if (this.form.valid) {
       this.formData=this.form.value;
       this.formData["id"]=this.index.getIndex();
       const selectedGroups: string[] = [];

      this.formData["groups"].forEach((option: boolean, index: number) => {
          if (option) {
            selectedGroups.push(this.groupOptions[index]);
          }
        });

      this.formData["groups"] = selectedGroups;


      this.index.addContact(this.formData)
      this.router.navigateByUrl('/dashboard')
    }
   
  }
}
