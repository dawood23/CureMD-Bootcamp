import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { NavigationComponent } from '../navigation/navigation.component';
import { ApiService } from '../../services/api/api.service';
import { Doctor } from '../../models/doctor';

@Component({
  selector: 'app-doctors',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NavigationComponent],
  templateUrl: './doctor.component.html',
  styleUrls: ['./doctor.component.scss']
})
export class DoctorsComponent {
  api = inject(ApiService);
  fb = inject(FormBuilder);

  doctors: Doctor[] = [];
  isEditing = false;

  form = this.fb.group({
    doctorID: this.fb.control<number>(0),
    firstName: this.fb.nonNullable.control('', [Validators.required]),
    lastName: this.fb.nonNullable.control('', [Validators.required]),
    phoneNumber: this.fb.nonNullable.control(''),
    email: this.fb.nonNullable.control('', [Validators.email])
  });

  get f() {
    return this.form.controls;
  }

  ngOnInit(): void {
    this.loadDoctors();
  }

  loadDoctors(): void {
    this.api.getDoctors().subscribe({
      next: (doctors) => (this.doctors = doctors),
      error: () => alert('Error loading doctors.')
    });
  }

  editDoctor(doctor: Doctor): void {
    this.form.patchValue(doctor);
    this.isEditing = true;
  }

  deleteDoctor(id: number): void {
    if (confirm('Delete this doctor?')) {
      this.api.deleteDoctor(id).subscribe({
        next: () => this.loadDoctors(),
        error: () => alert('Delete failed')
      });
    }
  }

  onSave(): void {
    if (this.form.invalid) {
      alert('First & Last name required');
      return;
    }

    const formValue = this.form.value as Doctor;

    if (this.isEditing) {
      this.api.updateDoctor(formValue).subscribe({
        next: () => {
          this.loadDoctors();
          this.resetForm();
        },
        error: () => alert('Save failed')
      });
    } else {
      this.api.createDoctor(formValue).subscribe({
        next: () => {
          this.loadDoctors();
          this.resetForm();
        },
        error: () => alert('Save failed')
      });
    }
  }

  resetForm(): void {
    this.form.reset();
    this.isEditing = false;
  }
}
