import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { NavigationComponent } from '../navigation/navigation.component';
import { ApiService } from '../../services/api/api.service';
import { UserRole } from '../../models/user-roles';
import { LetterOnlyDirective } from '../../directive/letter-only.directive';

@Component({
  selector: 'app-userroles',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NavigationComponent,LetterOnlyDirective],
  templateUrl: './userroles.component.html',
  styleUrls: ['./userroles.component.scss']
})
export class UserRolesComponent {
  api = inject(ApiService);
  fb = inject(FormBuilder);

  userRoles: UserRole[] = [];
  isEditing = false;

  form = this.fb.group({
    roleID: this.fb.control<number>(0),
    roleName: this.fb.nonNullable.control('', [Validators.required]),
    description: this.fb.nonNullable.control('')
  });

  get f() {
    return this.form.controls;
  }

  ngOnInit(): void {
    this.loadUserRoles();
  }

  loadUserRoles(): void {
    this.api.getUserRoles().subscribe({
      next: (roles) => (this.userRoles = roles),
      error: () => alert('Error loading roles.')
    });
  }

  editRole(role: UserRole): void {
    this.form.patchValue(role);
    this.isEditing = true;
  }

  deleteRole(id: number): void {
    if (confirm('Delete this role?')) {
      this.api.deleteUserRole(id).subscribe({
        next: () => this.loadUserRoles(),
        error: () => alert('Delete failed')
      });
    }
  }

  onSave(): void {
    if (this.form.invalid) {
      alert('Role name is required');
      return;
    }

    const formValue = this.form.value as UserRole;

    if (this.isEditing) {
      this.api.updateUserRole(formValue).subscribe({
        next: () => {
          this.loadUserRoles();
          this.resetForm();
        },
        error: () => alert('Save failed')
      });
    } else {
      this.api.createUserRole(formValue).subscribe({
        next: () => {
          this.loadUserRoles();
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
