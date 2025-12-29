import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/features/authenticate/services/auth.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

    registerForm: FormGroup = new FormGroup({});

    constructor(
      private fb: FormBuilder,
      private router: Router,
      private authService: AuthService

    ) { }

    ngOnInit(): void {
      this.registerForm = this.fb.group({
        username: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required]
      } /*,
      { validators: this.passwordsMatchValidator
      } */);
    }

  onSubmit() {

  if (this.registerForm.valid) {
    const { username, email, password } = this.registerForm.value;

    const dto = {
      username: username,
      email: email,
      passwordHash: password
    };

    this.authService.register(dto).subscribe({
      next: (res) => {
        alert('Successfully registered! Please log in.');
        console.log(res);
        this.router.navigate(['/auth/login']);
      },

        error: (err) => {
          console.error(err);
          alert('Registration failed. Please try again.');
        }
      });
    }
  }


      passwordsMatchValidator(formGroup: FormGroup) {
      const password = formGroup.get('password')?.value;
      const confirmPassword = formGroup.get('confirmPassword')?.value;

      if (password !== confirmPassword) {
        formGroup.get('confirmPassword')?.setErrors({ mismatch: true });
      } else {
        formGroup.get('confirmPassword')?.setErrors(null);
      }
    }

}
