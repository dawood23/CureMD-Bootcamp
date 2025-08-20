import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-contact-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './contact-page.component.html',
  styleUrls: ['./contact-page.component.scss']
})
export class ContactPageComponent {
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
      console.log(this.form.value);
    }
  }
}
