import { Component } from '@angular/core';
import { AuthService } from '../../authenticate/services/auth.service';
import { FriendResponse, RemoveFriendResponseModel } from '../models/friendship';
import { FriendshipService } from '../services/friendship.service';

@Component({
  selector: 'app-friend-list',
  templateUrl: './friend-list.component.html',
  styleUrls: ['./friend-list.component.css']
})
export class FriendListComponent {

  constructor(
    private AuthService: AuthService,
    private FriendshipService: FriendshipService
  ) {}

  friends: FriendResponse[] = [];
  private userId!: string;

  ngOnInit(): void {

    this.userId = this.AuthService.getUserId();
    this.FriendshipService.GetFriendlistAsync(this.userId).subscribe({
        next: (res) => {
          if(res.messageID == 1) {
            this.friends = res.friends || [];
          }
          else {
            alert('Failed to load friend list: ' + res.messageDescription);
          }
        },
        error: (err) => console.error('Failed to load friend list', err)
      }
    ); 
  }

  RemoveFriend(friendID: string): void {
    this.FriendshipService.RemoveFriendAsync(this.userId, friendID).subscribe({
      next: (res) => {
        if (res.messageID == 1) {
          alert('Friend removed successfully.');
          // To Delete From UI
          this.friends = this.friends.filter(f => f.friendID !== friendID);
        } else {
          alert('Failed to remove friend: ' + res.messageDescription);
        }
      },
      error: (err) => console.error('Failed to remove friend', err)
    });
  }
}
