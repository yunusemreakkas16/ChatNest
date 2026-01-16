import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { AuthService } from '../../authenticate/services/auth.service';
import { UserResponseModelDetailed, UserResponseDetailed, UpdateUserRequestDto } from '../models/user';
import { BaseResponse } from 'src/app/base-response-model/base-response';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
    selector: 'app-user-panel',
    templateUrl: './user-panel.component.html',
    styleUrls: ['./user-panel.component.css'],
    standalone: false
})
export class UserPanelComponent implements OnInit {

  userID: string = '';
  user?: UserResponseDetailed;   // store only inner user for cleaner template
  loading = false;
  errorMessage = '';
  successMessage = '';
  updateForm!: FormGroup;

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.userID = this.authService.getUserID();
    this.loadUserDetails();

    this.updateForm = this.fb.group({
      username: [''],
      email: [''],
      passwordHash: ['']
    });
  }


  loadUserDetails(): void {
    const userParam = { userID: this.userID };
    this.loading = true;
    this.userService.GetUserDetails(userParam).subscribe({
      next: (response: UserResponseModelDetailed) => {
        if (response.messageID === 1 && response.user) {
          this.user = response.user;
          this.successMessage = response.messageDescription ?? 'User details loaded successfully.';
        } else {
          this.errorMessage = response.messageDescription ?? 'Failed to load user details.';
        }
        this.loading = false;
      },
      error: (error) => {
        this.errorMessage = 'An error occurred while fetching user details.';
        console.error(error);
        this.loading = false;
      }
    });
  }


  updateUser(): void {
  if (!this.user) return;

  const formValues = this.updateForm.value;

  const dto: UpdateUserRequestDto = {
    userID: this.user.userID,
    username: formValues.username || this.user.userName,
    email: formValues.email || this.user.userEmail,
    passwordHash: formValues.passwordHash || this.user.userPasswordHash
  };

  this.loading = true;
  this.userService.UpdateUser(dto).subscribe({
    next: (response: UserResponseModelDetailed) => {
      if (response.messageID === 1 && response.user) {
        this.user = response.user;
        this.successMessage = response.messageDescription ?? 'User updated successfully.';
      } else {
        this.errorMessage = response.messageDescription ?? 'Failed to update user.';
      }
      this.loading = false;
    },
    error: (error) => {
      this.errorMessage = 'An error occurred while updating user.';
      console.error(error);
      this.loading = false;
    }
  });
}

  softDeleteUser(): void {
    const confirmed = confirm("Are you sure you want to delete your account?");
    if (!confirmed) {
      return; // user cancelled the action
    }

    this.loading = true;
    this.userService.SoftDeleteUser({ userID: this.userID }).subscribe({
      next: (response: BaseResponse) => {
        if (response.messageID === 1) {
          this.successMessage = response.messageDescription ?? 'User soft-deleted successfully.';
          this.user = undefined;
          this.router.navigate(['/login']);
        } else {
          this.errorMessage = response.messageDescription ?? 'Failed to soft-delete user.';
        }
        this.loading = false;
      },
      error: (error) => {
        this.errorMessage = 'An error occurred while deleting user.';
        console.error(error);
        this.loading = false;
      }
    });
  }

  logout(): void {
    this.loading = true;
    this.authService.logout().subscribe({
      next: (response: BaseResponse) => {
        if (response.messageID === 1) {
          this.successMessage = response.messageDescription ?? 'Logged out successfully.';
          this.router.navigate(['/login']);
        } else {
          this.errorMessage = response.messageDescription ?? 'Failed to logout.';
        }
        this.loading = false;
      },
      error: (error) => {
        this.errorMessage = 'An error occurred while logging out.';
        console.error(error);
        this.loading = false;
        this.router.navigate(['/login']);
      }
    });
  }

  logoutAllDevices(): void {
    this.loading = true;
    this.authService.logoutAllDevices().subscribe({
      next: (response: BaseResponse) => {
        if (response.messageID === 1) {
          this.successMessage = response.messageDescription ?? 'Logged out from all devices.';
          this.user = undefined;
          this.router.navigate(['/login']);
        } else {
          this.errorMessage = response.messageDescription ?? 'Logout from all devices failed.';
        }
        this.loading = false;
      },
      error: (error) => {
        this.errorMessage = 'An error occurred while logging out from all devices.';
        console.error(error);
        this.loading = false;
        this.router.navigate(['/login']);
      }
    });
  }


}