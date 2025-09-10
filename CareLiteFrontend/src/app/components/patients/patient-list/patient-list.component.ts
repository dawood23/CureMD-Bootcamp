import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Patient } from '../../../models/patient';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime } from 'rxjs';
import { Router } from '@angular/router';
import { HighlightPipe } from '../../../pipes/highlight/highlight.pipe';
import { Select, Store } from '@ngxs/store';
import { PatientState } from '../../../store/patients/patient.state';
import { LoadPatients } from '../../../store/patients/patient.actions';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-patient-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HighlightPipe],
  templateUrl: './patient-list.component.html',
  styleUrl: './patient-list.component.scss'
})
export class PatientListComponent {
  store = inject(Store);
  router = inject(Router);

  @Select(PatientState.patients) patients$!: Observable<Patient[]>;

  patients: Patient[] = [];
  filteredPatients: Patient[] = [];
  searchControl = new FormControl('');

  currentPage = 1;
  pageSize = 5;

  ngOnInit() {
    this.store.dispatch(new LoadPatients());

    this.patients$.subscribe(patients => {
      this.patients = patients;
      this.filteredPatients = patients;
    });

    this.searchControl.valueChanges
      .pipe(debounceTime(300))
      .subscribe(query => {
        const lowerQuery = query?.toLowerCase() || '';
        this.filteredPatients = this.patients.filter(p =>
          (p.firstName + ' ' + p.lastName).toLowerCase().includes(lowerQuery) ||
          (p.email?.toLowerCase() ?? '').includes(lowerQuery) ||
          (p.phone ?? '').toLowerCase().includes(lowerQuery)
        );
        this.currentPage = 1;
      });
  }

  get paginatedPatients(): Patient[] {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    return this.filteredPatients.slice(startIndex, startIndex + this.pageSize);
  }

  nextPage() {
    if (this.currentPage * this.pageSize < this.filteredPatients.length) {
      this.currentPage++;
    }
  }

  get totalPages(): number {
    return Math.ceil(this.filteredPatients.length / this.pageSize);
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  redirect() {
    this.router.navigate(['/patient-add']);
  }

  goBack() {
    this.router.navigate(['/dashboard']);
  }
}
