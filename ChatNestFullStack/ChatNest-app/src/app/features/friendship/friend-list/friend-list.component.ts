import { Component } from '@angular/core';
import { AuthService } from '../../authenticate/services/auth.service';
import { FriendResponse } from '../models/friendship';
import { FriendshipService } from '../services/friendship.service';

@Component({
  selector: 'app-friend-list',
  templateUrl: './friend-list.component.html',
  styleUrls: ['./friend-list.component.css']
})
export class FriendListComponent {
  friends: FriendResponse[] = [];
  private userId!: string;

  statusMessage: string = '';
  statusType: 'success' | 'error' | '' = '';

  constructor(
    private AuthService: AuthService,
    private FriendshipService: FriendshipService
  ) {}

  ngOnInit(): void {
    this.userId = this.AuthService.getUserID();
    this.FriendshipService.GetFriendlistAsync(this.userId).subscribe({
      next: (res) => {
        if (res.messageID === 1) {
          this.friends = res.friends || [];
        } else {
          this.statusMessage = res.messageDescription;
          this.statusType = 'error';
        }
      },
      error: (err) => {
        console.error('Failed to load friend list', err);
        this.statusMessage = 'Network error while loading friend list.';
        this.statusType = 'error';
      }
    });
  }

  RemoveFriend(friendID: string): void {
    this.FriendshipService.RemoveFriendAsync(this.userId, friendID).subscribe({
      next: (res) => {
        if (res.messageID === 1) {
          this.friends = this.friends.filter(f => f.friendID !== friendID);
          this.statusMessage = 'Friend removed successfully.';
          this.statusType = 'success';
        } else {
          this.statusMessage = res.messageDescription;
          this.statusType = 'error';
        }
      },
      error: (err) => {
        console.error('Failed to remove friend', err);
        this.statusMessage = 'Network error while removing friend.';
        this.statusType = 'error';
      }
    });
  }
}