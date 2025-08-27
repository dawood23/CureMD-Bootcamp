import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { NavigationComponent } from '../navigation/navigation.component';
import { ApiService } from '../../services/api/api.service';
import { VisitType } from '../../models/visittype';
import { LetterOnlyDirective } from '../../directive/letter-only.directive';
@Component({
  selector: 'app-visit-types',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NavigationComponent,LetterOnlyDirective],
  templateUrl: './visittypes.component.html',
  styleUrls: ['./visittypes.component.scss']
})
export class VisittypesComponent implements OnInit {
  api = inject(ApiService);
  fb = inject(FormBuilder);

  visitTypes: VisitType[] = [];
  isEditing = false;

  form = this.fb.group({
    visitTypeID: this.fb.control<number>(0),
    typeName: this.fb.nonNullable.control('', [Validators.required]),
    baseFee: this.fb.nonNullable.control(0, [Validators.required, Validators.min(0.01)]),
    estimatedDuration: this.fb.nonNullable.control(0, [Validators.required, Validators.min(1)])
  });

  get f() {
    return this.form.controls;
  }

  ngOnInit(): void {
    this.loadVisitTypes();
  }

  loadVisitTypes(): void {
    this.api.getVisitTypes().subscribe({
      next: (visitTypes) => (this.visitTypes = visitTypes),
      error: () => alert('Error loading visit types.')
    });
  }

  editVisitType(visitType: VisitType): void {
    this.form.patchValue(visitType);
    this.isEditing = true;
  }

  deleteVisitType(id: number): void {
    if (confirm('Delete this visit type?')) {
      this.api.deleteVisitType(id).subscribe({
        next: () => this.loadVisitTypes(),
        error: () => alert('Error deleting visit type.')
      });
    }
  }

  onSave(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const formValue = this.form.value as VisitType;

    if (this.isEditing) {
      this.api.updateVisitType(formValue).subscribe({
        next: () => {
          this.loadVisitTypes();
          this.resetForm();
        },
        error: (error) => alert('Error updating visit type: ' + error.statusText)
      });
    } else {
      this.api.createVisitType(formValue).subscribe({
        next: () => {
          this.loadVisitTypes();
          this.resetForm();
        },
        error: (error) => alert('Error creating visit type: ' + error.statusText)
      });
    }
  }

  resetForm(): void {
    this.form.reset({
      visitTypeID: 0,
      typeName: '',
      baseFee: 0,
      estimatedDuration: 0
    });
    this.isEditing = false;
  }
}