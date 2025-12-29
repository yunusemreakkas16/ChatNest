import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/features/authenticate/services/auth.service';
import { LoginResponseDto } from '../../user/models/user';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  logInForm: FormGroup = new FormGroup({});
  errorMessage: string = '';
  errorCode: number | null = null; 
  reactivateIfDeleted: boolean = false;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.logInForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.logInForm.valid) {
      this.auth.login(this.logInForm.value).subscribe({
        next: (res: LoginResponseDto) => {
          if (res.accessToken && res.refreshToken) {
            // Save tokens to localStorage
            localStorage.setItem('accessToken', res.accessToken);
            localStorage.setItem('refreshToken', res.refreshToken);
            this.router.navigate(['/chats']);
          } 
          else {
            // Failed login handling
            this.errorCode = res.messageID ?? null;
            this.errorMessage = res.messageDescription;
          }
        },
        error: (err) => {
          console.error(err);
          this.errorCode = err.error?.messageID ?? null;
          this.errorMessage = err.error?.messageDescription || 'Login failed. Please check your credentials.';
        }
      });
    }
  }

  retryLogin() {
  const payload = {
    ...this.logInForm.value,
    reactivateIfDeleted: this.reactivateIfDeleted
  };

  this.auth.login(payload).subscribe({
    next: (res: LoginResponseDto) => {
      if (res.accessToken && res.refreshToken) {
        localStorage.setItem('accessToken', res.accessToken);
        localStorage.setItem('refreshToken', res.refreshToken);
        this.router.navigate(['/chats']);
      } else {
        this.errorCode = res.messageID ?? null;
        this.errorMessage = res.messageDescription;
      }
    }
  });
}



  goToRegister() {
    this.router.navigate(['/auth/register']); 
  }
}