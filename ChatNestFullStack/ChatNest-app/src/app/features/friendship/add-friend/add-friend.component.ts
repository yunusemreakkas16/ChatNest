import { Component } from '@angular/core';
import { FriendshipService } from '../services/friendship.service';
import { AuthService } from '../../authenticate/services/auth.service';
import { SendFriendRequestResponseModel } from '../models/friendship';

@Component({
  selector: 'app-add-friend',
  templateUrl: './add-friend.component.html',
  styleUrls: ['./add-friend.component.css']
})
export class AddFriendComponent {
  friendEmail: string = '';
  userId!: string

  constructor(
    private authService: AuthService,
    private FriendshipService: FriendshipService
  ) {}

    ngOnInit(): void {
    this.userId = this.authService.getUserId(); // burada set et
  }


  SendFriendRequest(): void {
    if (!this.friendEmail) {
      alert('Please enter a valid email address.');
      return;
    }

    this.FriendshipService.SendFriendRequestAsync(this.userId, this.friendEmail).subscribe({
      next: (response: SendFriendRequestResponseModel) => {
      if (response.messageID >= 1 && response.messageID <= 4) {
        const successMessages = {
          1: 'Friend request sent successfully!',
          2: 'Friend request re-sent from previously cancelled request!',
          3: 'Friend request re-sent from previously rejected request!', 
          4: 'Friend request re-sent from previously removed request!'
        };

        alert(successMessages[response.messageID as keyof typeof successMessages]);
        this.friendEmail = '';
      }
        else {
          alert('Failed to send friend request. ' + response.messageDescription);
        }
      },
      error: (error) => {
        console.error('Error sending friend request:', error);
        alert('An error occurred while sending the friend request.');
      }
    });

  }

}
