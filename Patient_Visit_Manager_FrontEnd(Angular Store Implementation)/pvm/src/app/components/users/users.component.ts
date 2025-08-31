import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { NavigationComponent } from '../navigation/navigation.component';
import { User } from '../../models/user';
import { LetterOnlyDirective } from '../../directive/letter-only.directive';
import { UserState } from '../../store/state/user.state';
import { LoadUsers,CreateUser,UpdateUser,DeleteUser } from '../../store/actions/user.actions';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NavigationComponent, LetterOnlyDirective],
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  store = inject(Store);
  fb = inject(FormBuilder);

  users$ = this.store.select(UserState.users);
  loading$ = this.store.select(UserState.loading);
  error$ = this.store.select(UserState.error);
  
  isEditing = false;

  form = this.fb.group({
    userID: this.fb.control<number>(0),
    username: this.fb.nonNullable.control('', [Validators.required]),
    password: this.fb.nonNullable.control(''),
    roleID: this.fb.nonNullable.control(0, [Validators.required, Validators.min(1)]),
    firstName: this.fb.nonNullable.control('', [Validators.required]),
    lastName: this.fb.nonNullable.control('', [Validators.required])
  });

  get f() {
    return this.form.controls;
  }

  ngOnInit(): void {
    this.store.dispatch(new LoadUsers());
  }

  editUser(user: User): void {
    this.form.patchValue({
      ...user,
      password: ''
    });
    this.isEditing = true;
  }

  deleteUser(id: number): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.store.dispatch(new DeleteUser(id));
    }
  }

  onSave(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const formValue = this.form.value;
    const { password, ...formData } = formValue;
    
    const userData: User = {
      userID: formData.userID || undefined,
      username: formData.username || '',
      roleID: formData.roleID || 0,
      firstName: formData.firstName || '',
      lastName: formData.lastName || ''
    };
    
    if (password) {
      (userData as any).passwordHash = password;
    }

    if (this.isEditing) {
      this.store.dispatch(new UpdateUser({ id: userData.userID!, user: userData })).subscribe({
        next: () => this.resetForm(),
        error: (error) => alert('Error updating user: ' + error.statusText)
      });
    } else {
      this.store.dispatch(new CreateUser(userData)).subscribe({
        next: () => this.resetForm(),
        error: (error) => alert('Error adding user: ' + error.statusText)
      });
    }
  }

  resetForm(): void {
    this.form.reset({
      userID: 0,
      username: '',
      password: '',
      roleID: 0,
      firstName: '',
      lastName: ''
    });
    this.isEditing = false;
  }
}