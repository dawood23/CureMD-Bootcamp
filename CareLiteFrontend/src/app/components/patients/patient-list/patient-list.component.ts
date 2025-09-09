import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../services/api/api.service';
import { Patient } from '../../../models/patient';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, switchMap } from 'rxjs';
import { Router } from '@angular/router';
import { HighlightPipe } from '../../../pipes/highlight/highlight.pipe';

@Component({
  selector: 'app-patient-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,HighlightPipe],
  templateUrl: './patient-list.component.html',
  styleUrl: './patient-list.component.scss'
})
export class PatientListComponent {
  patientService = inject(ApiService);
  router = inject(Router);

  patients: Patient[] = [];
  filteredPatients: Patient[] = [];
  searchControl = new FormControl('');

  currentPage = 1;
  pageSize = 5; 

  ngOnInit() {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(300),
        switchMap(() => this.patientService.getAllPatients())
      )
      .subscribe(patients => {
        const query = this.searchControl.value?.toLowerCase() || '';

        this.filteredPatients = patients.filter(p =>
          (p.firstName + ' ' + p.lastName).toLowerCase().includes(query) ||
          (p.email?.toLowerCase() ?? '').includes(query) ||
          (p.phone ?? '').toLowerCase().includes(query)
        );
        this.currentPage = 1; 
      });

    this.patientService.getAllPatients().subscribe({
      next: (patients) => {
        this.patients = patients;
        this.filteredPatients = patients;
      },
      error: () => alert('An error occurred while loading the patients')
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
