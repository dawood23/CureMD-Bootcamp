import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { NavigationComponent } from '../navigation/navigation.component';
import { UserRole } from '../../models/user-roles';
import { LetterOnlyDirective } from '../../directive/letter-only.directive';
import { UserRoleState } from '../../store/state/user-role.state';
import { LoadUserRoles,CreateUserRole,UpdateUserRole,DeleteUserRole } from '../../store/actions/user-role.actions';

@Component({
  selector: 'app-userroles',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NavigationComponent, LetterOnlyDirective],
  templateUrl: './userroles.component.html',
  styleUrls: ['./userroles.component.scss']
})
export class UserRolesComponent implements OnInit {
  store = inject(Store);
  fb = inject(FormBuilder);

  userRoles$ = this.store.select(UserRoleState.userRoles);
  loading$ = this.store.select(UserRoleState.loading);
  error$ = this.store.select(UserRoleState.error);
  
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
    this.store.dispatch(new LoadUserRoles());
  }

  editRole(role: UserRole): void {
    this.form.patchValue(role);
    this.isEditing = true;
  }
  loadUserRoles(): void {
  this.store.dispatch(new LoadUserRoles());
}

  deleteRole(id: number): void {
    if (confirm('Delete this role?')) {
      this.store.dispatch(new DeleteUserRole(id));
    }
  }

  onSave(): void {
    if (this.form.invalid) {
      alert('Role name is required');
      return;
    }

    const formValue = this.form.value as UserRole;

    if (this.isEditing) {
      this.store.dispatch(new UpdateUserRole(formValue)).subscribe({
        next: () => this.resetForm(),
        error: () => alert('Save failed')
      });
    } else {
      this.store.dispatch(new CreateUserRole(formValue)).subscribe({
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
