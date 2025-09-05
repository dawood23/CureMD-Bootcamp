import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../services/api/api.service';
import { Patient } from '../../../models/patient';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, switchMap } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-patient-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './patient-list.component.html',
  styleUrl: './patient-list.component.scss'
})
export class PatientListComponent {
  patientService = inject(ApiService);
  router=inject(Router)
  patients: Patient[] = [];
  searchControl = new FormControl('');
  
  ngOnInit() {
  this.searchControl.valueChanges
    .pipe(
      debounceTime(300),
      switchMap(() => this.patientService.getAllPatients())
    )
    .subscribe(patients => {
      const query = this.searchControl.value?.toLowerCase() || '';

      this.patients = patients.filter(p =>
        (p.firstName + ' ' + p.lastName).toLowerCase().includes(query) ||
        (p.email?.toLowerCase() ?? '').includes(query) ||
        (p.phone ?? '').toLowerCase().includes(query)
      );
    });

    return this.patientService.getAllPatients().subscribe(
      {
        next:(patients)=>this.patients=patients,
        error:()=>alert("An error occured while loading the patients")
      }
    );
}

  redirect(){
    this.router.navigate(['/patient-add'])
  }

  goBack(){
    this.router.navigate(['/dashboard'])
  }
}
