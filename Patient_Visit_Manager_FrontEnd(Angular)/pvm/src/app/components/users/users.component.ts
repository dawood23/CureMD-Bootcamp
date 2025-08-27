import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { NavigationComponent } from '../navigation/navigation.component';
import { ApiService } from '../../services/api/api.service';
import { User } from '../../models/user';
import { LetterOnlyDirective } from '../../directive/letter-only.directive';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NavigationComponent,LetterOnlyDirective],
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  api = inject(ApiService);
  fb = inject(FormBuilder);

  users: User[] = [];
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
    this.loadUsers();
  }

  loadUsers(): void {
    this.api.getUsers().subscribe({
      next: (users) => (this.users = users),
      error: () => alert('Error loading users.')
    });
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
      this.api.deleteUser(id).subscribe({
        next: () => this.loadUsers(),
        error: () => alert('Error deleting user.')
      });
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
      this.api.updateUser(userData.userID!, userData).subscribe({
        next: () => {
          this.loadUsers();
          this.resetForm();
        },
        error: (error) => alert('Error updating user: ' + error.statusText)
      });
    } else {
      this.api.createUser(userData).subscribe({
        next: () => {
          this.loadUsers();
          this.resetForm();
        },
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