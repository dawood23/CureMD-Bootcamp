import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { NavigationComponent } from '../navigation/navigation.component';
import { Doctor } from '../../models/doctor';
import { LetterOnlyDirective } from '../../directive/letter-only.directive';
import { DoctorState } from '../../store/state/doctor.state';
import { LoadDoctors,CreateDoctor,UpdateDoctor,DeleteDoctor } from '../../store/actions/doctor.actions';

@Component({
  selector: 'app-doctors',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NavigationComponent, LetterOnlyDirective],
  templateUrl: './doctor.component.html',
  styleUrls: ['./doctor.component.scss']
})
export class DoctorsComponent implements OnInit {
  store = inject(Store);
  fb = inject(FormBuilder);

  doctors$ = this.store.select(DoctorState.doctors);
  loading$ = this.store.select(DoctorState.loading);
  error$ = this.store.select(DoctorState.error);
  
  isEditing = false;

  form = this.fb.group({
    doctorID: this.fb.control<number>(0),
    firstName: this.fb.nonNullable.control('', [Validators.required]),
    lastName: this.fb.nonNullable.control('', [Validators.required]),
    phoneNumber: this.fb.nonNullable.control('', [Validators.required, Validators.minLength(11), Validators.maxLength(11)]),
    email: this.fb.nonNullable.control('', [Validators.email])
  });

  get f() {
    return this.form.controls;
  }

  ngOnInit(): void {
    this.store.dispatch(new LoadDoctors());
  }

  loadDoctors(): void {
  this.store.dispatch(new LoadDoctors());
}

  editDoctor(doctor: Doctor): void {
    this.form.patchValue(doctor);
    this.isEditing = true;
  }

  deleteDoctor(id: number): void {
    if (confirm('Delete this doctor?')) {
      this.store.dispatch(new DeleteDoctor(id));
    }
  }

  onSave(): void {
    if (this.form.invalid) {
      alert('First & Last name required');
      return;
    }

    const formValue = this.form.value as Doctor;

    if (this.isEditing) {
      this.store.dispatch(new UpdateDoctor(formValue)).subscribe({
        next: () => this.resetForm(),
        error: () => alert('Save failed')
      });
    } else {
      this.store.dispatch(new CreateDoctor(formValue)).subscribe({
        next: () => this.resetForm(),
        error: () => alert('Save failed')
      });
    }
  }

  resetForm(): void {
    this.form.reset();
    this.isEditing = false;
  }
}