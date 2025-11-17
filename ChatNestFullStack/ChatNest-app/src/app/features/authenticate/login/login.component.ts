import { Component, OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/features/authenticate/services/auth.service';
import { from } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  logInForm: FormGroup = new FormGroup({});

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
        next: (res) => {
          localStorage.setItem('accessToken', res.accessToken); // Store JWT token
          localStorage.setItem('refreshToken', res.refreshToken);
          this.router.navigate(['/chats']); // Return to home page
        },
        error: (err) => {
          console.error(err);
          alert('Login failed. Please check your credentials.');
        }
      });
    }
  }

  goToRegister() {
  this.router.navigate(['/auth/register']); 
}


}
