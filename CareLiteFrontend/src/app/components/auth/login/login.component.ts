import { Component, inject } from '@angular/core';
import { LoginRequest } from '../../../models/loginRequest';
import { AuthService } from '../../../services/auth/auth.service';
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule,ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
   
    authService=inject(AuthService)
    router=inject(Router)
    fb=inject(FormBuilder)

    form=this.fb.group({
      username:this.fb.nonNullable.control('',[Validators.required]),
      password:this.fb.nonNullable.control('',[Validators.required])
    })

    public formControl=this.form.controls

    onLogin(credentials:LoginRequest):void{
      this.authService.login(credentials).subscribe({
        next:()=>{
          this.router.navigate(['/dashboard']);
        },
         error: (err) => {
          const backendMsg = err?.error?.message || "Login failed";
          alert(backendMsg);
       }
      })
    }

    SubmitForm(){
      if(this.form.invalid){
        alert('Invalid Credentials')
      }
      const value=this.form.value as LoginRequest
      
      this.onLogin(value)
    }
}
