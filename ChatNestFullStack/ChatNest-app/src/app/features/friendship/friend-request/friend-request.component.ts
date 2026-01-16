import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../authenticate/services/auth.service';
import { FriendshipService } from '../services/friendship.service';
import { FriendRequestResponse } from '../models/friendship';

@Component({
    selector: 'app-friend-request',
    templateUrl: './friend-request.component.html',
    styleUrls: ['./friend-request.component.css'],
    standalone: false
})
export class FriendRequestComponent implements OnInit {
  requests: FriendRequestResponse[] = [];
  activeTab: 'Sent' | 'Received' = 'Received';
  statusMessage: string = '';
  statusType: 'success' | 'error' | '' = '';
  private userID!: string;

  constructor(
    private authService: AuthService,
    private FriendshipService: FriendshipService
  ) {}

  ngOnInit(): void {
    this.userID = this.authService.getUserID();
    this.loadRequests();
  }

  switchTab(tab: 'Received' | 'Sent'): void {
    if (this.activeTab === tab) return;
    this.activeTab = tab;
    this.requests = [];
    this.statusMessage = '';
    this.loadRequests();
  }

  private loadRequests(): void {
    this.FriendshipService.GetFriendRequestsAsync(this.userID, this.activeTab).subscribe({
      next: (response) => {
        if (response.messageID === 1) {
          this.requests = response.friendRequests || [];
        } else {
          this.statusMessage = response.messageDescription;
          this.statusType = 'error';
        }
      },
      error: (error) => {
        console.error('Error fetching friend requests:', error);
        this.statusMessage = 'Network error while loading requests.';
        this.statusType = 'error';
      }
    });
  }

  CancelRequestAsync(otherUserID: string): void {
    this.FriendshipService.ManageFriendRequestAsync(this.userID, otherUserID, 'Cancel').subscribe({
      next: (response) => {
        if (response.messageID === 1) {
          this.requests = this.requests.filter(req => req.receiverID !== otherUserID);
          this.statusMessage = 'Friend request cancelled.';
          this.statusType = 'success';
        } else {
          this.statusMessage = response.messageDescription;
          this.statusType = 'error';
        }
      },
      error: (error) => {
        console.error('HTTP Error:', error);
        this.statusMessage = error.status === 404
          ? 'Request not found. It may have been already cancelled.'
          : 'Network error: ' + error.message;
        this.statusType = 'error';
      }
    });
  }

  AcceptRequestAsync(otherUserID: string): void {
    this.FriendshipService.ManageFriendRequestAsync(this.userID, otherUserID, 'Accept').subscribe({
      next: (response) => {
        if (response.messageID === 1) {
          this.requests = this.requests.filter(req => req.requesterID !== otherUserID);
          this.statusMessage = 'Friend request accepted.';
          this.statusType = 'success';
        } else {
          this.statusMessage = response.messageDescription;
          this.statusType = 'error';
        }
      },
      error: (error) => {
        console.error('Error accepting friend request:', error);
        this.statusMessage = 'Network error while accepting request.';
        this.statusType = 'error';
      }
    });
  }

  RejectRequestAsync(otherUserID: string): void {
    this.FriendshipService.ManageFriendRequestAsync(this.userID, otherUserID, 'Reject').subscribe({
      next: (response) => {
        if (response.messageID === 1) {
          this.requests = this.requests.filter(req => req.requesterID !== otherUserID);
          this.statusMessage = 'Friend request rejected.';
          this.statusType = 'success';
        } else {
          this.statusMessage = response.messageDescription;
          this.statusType = 'error';
        }
      },
      error: (error) => {
        console.error('Error rejecting friend request:', error);
        this.statusMessage = 'Network error while rejecting request.';
        this.statusType = 'error';
      }
    });
  }
}