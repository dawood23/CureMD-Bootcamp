import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { NavigationComponent } from '../navigation/navigation.component';
import { VisitType } from '../../models/visittype';
import { LetterOnlyDirective } from '../../directive/letter-only.directive';
import { VisitTypeState } from '../../store/state/visit-type.state';
import { LoadVisitTypes,CreateVisitType,UpdateVisitType,DeleteVisitType } from '../../store/actions/visit-type.actions';

@Component({
  selector: 'app-visit-types',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NavigationComponent, LetterOnlyDirective],
  templateUrl: './visittypes.component.html',
  styleUrls: ['./visittypes.component.scss']
})
export class VisittypesComponent implements OnInit {
  store = inject(Store);
  fb = inject(FormBuilder);

  visitTypes$ = this.store.select(VisitTypeState.visitTypes);
  loading$ = this.store.select(VisitTypeState.loading);
  error$ = this.store.select(VisitTypeState.error);
  
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
    this.store.dispatch(new LoadVisitTypes());
  }

  editVisitType(visitType: VisitType): void {
    this.form.patchValue(visitType);
    this.isEditing = true;
  }

  deleteVisitType(id: number): void {
    if (confirm('Delete this visit type?')) {
      this.store.dispatch(new DeleteVisitType(id));
    }
  }
  loadVisitTypes(): void {
  this.store.dispatch(new LoadVisitTypes());
}

  onSave(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const formValue = this.form.value as VisitType;

    if (this.isEditing) {
      this.store.dispatch(new UpdateVisitType(formValue)).subscribe({
        next: () => this.resetForm(),
        error: (error) => alert('Error updating visit type: ' + error.statusText)
      });
    } else {
      this.store.dispatch(new CreateVisitType(formValue)).subscribe({
        next: () => this.resetForm(),
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