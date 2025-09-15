import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Patient } from '../../../models/patient';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime } from 'rxjs';
import { Router } from '@angular/router';
import { HighlightPipe } from '../../../pipes/highlight/highlight.pipe';
import { ApiService } from '../../../services/api/api.service';

@Component({
  selector: 'app-patient-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HighlightPipe],
  templateUrl: './patient-list.component.html',
  styleUrl: './patient-list.component.scss'
})
export class PatientListComponent {
  private api = inject(ApiService);
  router = inject(Router);

  patients: Patient[] = [];
  searchControl = new FormControl('');

  currentPage = 1;
  pageSize = 5;
  totalCount = 0;

  ngOnInit() {
    this.loadPatients();

    this.searchControl.valueChanges
      .pipe(debounceTime(300))
      .subscribe(() => {
        this.currentPage = 1;
        this.loadPatients();
      });
  }

  loadPatients() {
    const query = this.searchControl.value || '';
    this.api.getPatientsPaged(this.currentPage, this.pageSize, query)
      .subscribe((res: { data: Patient[], totalCount: number }) => {
        this.patients = res.data;
        this.totalCount = res.totalCount;
      });
  }

  nextPage() {
    if (this.currentPage * this.pageSize < this.totalCount) {
      this.currentPage++;
      this.loadPatients();
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadPatients();
    }
  }

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  redirect() {
    this.router.navigate(['/patient-add']);
  }

  goBack() {
    this.router.navigate(['/dashboard']);
  }
}
