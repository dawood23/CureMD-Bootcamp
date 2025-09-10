import { Component, inject, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth/auth.service';
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { User } from '../../../models/user';
import { ApiService } from '../../../services/api/api.service';
import { Role } from '../../../models/roles';
@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [RouterModule, ReactiveFormsModule],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.scss'
})
export class SignupComponent implements OnInit{

    roles:Role[]=[]
    authService = inject(AuthService)
    router = inject(Router)
    fb = inject(FormBuilder)
    apiService=inject(ApiService)

    ngOnInit(): void {
      this.apiService.getAllRoles().subscribe({
        next:(roles)=> {this.roles=roles 
          console.log(roles)},
        error:()=>{console.log("Error Loading Roles")}
      })
      
    }
    form = this.fb.group({
      username: this.fb.nonNullable.control('', [Validators.required]),
      email: this.fb.nonNullable.control('', [Validators.required, Validators.email]),
      phone: this.fb.nonNullable.control('', [Validators.required]),
      password: this.fb.nonNullable.control('', [Validators.required, Validators.minLength(6)]),
      confirmPassword: this.fb.nonNullable.control('', [Validators.required]),
      roleID: this.fb.nonNullable.control('', [Validators.required])
    }, { validators: this.passwordMatchValidator })

    public formControl = this.form.controls

    passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
      const password = control.get('password');
      const confirmPassword = control.get('confirmPassword');
      
      if (password && confirmPassword && password.value !== confirmPassword.value) {
        return { passwordMismatch: true };
      }
      return null;
    }

    onSignup(userData: User): void {
      this.authService.signup(userData).subscribe({
        next: () => {
          alert('Account created successfully!');
          this.router.navigate(['/dashboard'])
        },
        error: (err) => {
          const backendMsg = err?.error?.message || "Signup failed";
          alert(backendMsg);
        }
      })
    }

    SubmitForm() {
      if (this.form.invalid) {
        alert('Please fill all fields correctly')
        return;
      }
      
      const formValue = this.form.value;
    
      const userData: User = {
        username: formValue.username!,
        email: formValue.email!,
        phone: formValue.phone!,
        passwordHash: formValue.password!,
        roleID: parseInt(formValue.roleID!),
        active: true
      };
      
      this.onSignup(userData);
    }
}