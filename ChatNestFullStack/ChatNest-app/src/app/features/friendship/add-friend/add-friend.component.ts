import { Component } from '@angular/core';
import { FriendshipService } from '../services/friendship.service';
import { AuthService } from '../../authenticate/services/auth.service';
import { SendFriendRequestResponseModel } from '../models/friendship';

@Component({
    selector: 'app-add-friend',
    templateUrl: './add-friend.component.html',
    styleUrls: ['./add-friend.component.css'],
    standalone: false
})
export class AddFriendComponent {
  friendEmail: string = '';
  userId!: string;

  statusMessage: string = '';
  statusType: 'success' | 'error' | '' = '';

  constructor(
    private authService: AuthService,
    private FriendshipService: FriendshipService
  ) {}

  ngOnInit(): void {
    this.userId = this.authService.getUserID();
  }

  SendFriendRequest(): void {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!this.friendEmail || !emailRegex.test(this.friendEmail)) {
      this.statusMessage = 'Please enter a valid email address.';
      this.statusType = 'error';
      return;
    }

    this.FriendshipService.SendFriendRequestAsync(this.userId, this.friendEmail).subscribe({
      next: (response: SendFriendRequestResponseModel) => {
        if (response.messageID >= 1 && response.messageID <= 4) {
          const successMessages: Record<number, string> = {
            1: 'Friend request sent successfully!',
            2: 'Friend request re-sent from previously cancelled request!',
            3: 'Friend request re-sent from previously rejected request!',
            4: 'Friend request re-sent from previously removed request!'
          };

          this.statusMessage = successMessages[response.messageID];
          this.statusType = 'success';
          this.friendEmail = '';
        } else {
          this.statusMessage = 'Failed to send friend request. ' + response.messageDescription;
          this.statusType = 'error';
        }
      },
      error: (error) => {
        console.error('Error sending friend request:', error);
        if (error.error && error.error.messageDescription) {
          this.statusMessage = error.error.messageDescription;
        } else {
          // fallback
          this.statusMessage = 'Network error: ' + error.message;
        }
        this.statusType = 'error';
      }
    });
  }
}