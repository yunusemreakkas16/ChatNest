import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from '../../authenticate/services/auth.service';
import { FriendshipService } from '../services/friendship.service';
import { FriendRequestResponse } from '../models/friendship';

@Component({
  selector: 'app-friend-request',
  templateUrl: './friend-request.component.html',
  styleUrls: ['./friend-request.component.css']
})
export class FriendRequestComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private FriendshipService: FriendshipService
  ) {}

  requests: FriendRequestResponse[] = [];
  activeTab: 'Sent' | 'Received' = 'Received';
  private userID!: string;


  ngOnInit(): void {
    this.userID = this.authService.getUserId();
    this.FriendshipService.GetFriendRequestsAsync(this.userID, this.activeTab).subscribe({
      next: (response) => {
        if(response.messageID == 1){
          this.requests = response.friendRequests;
        }
        else{
          alert('Failed to load friend requests.' + response.messageDescription);
        }
      },
      error: (error) => {
        console.error('Error fetching friend requests:', error);
      }
    });
  }

  switchTab(tab: 'Received' | 'Sent'): void {
    if (this.activeTab === tab) return;
    this.activeTab = tab;
    this.loadRequests();
  }

  private loadRequests(): void {
    this.FriendshipService.GetFriendRequestsAsync(this.userID, this.activeTab).subscribe({
      next: (response) => {
        if (response.messageID === 1) {
        this.requests = response.friendRequests || [];
        }
        else {
          alert('Failed to load friend requests. ' + response.messageDescription);
        }
      },
      error: (error) => console.error('Error fetching friend requests:', error)
    });
  }

  CancelRequestAsync(otherUserID: string): void {
    this.FriendshipService.ManageFriendRequestAsync(this.userID, otherUserID, 'Cancel').subscribe({
      next: (response) => {
        if (response.messageID === 1) {
          this.requests = this.requests.filter(req => req.receiverID !== otherUserID);
        } 
        else {
          // BUSINESS ERROR
          console.error('Business Error:', response.messageID, response.messageDescription);
          alert('Failed: ' + response.messageDescription);
        }
      },
    error: (error) => {
      // HTTP ERROR (401, 404, 500 vb.)
      console.error('HTTP Error:', error);
      if (error.status === 404) {
        alert('Request not found. It may have been already cancelled.');
      } 
      else {
        alert('Network error: ' + error.message);
      }
    }
  });
  }

  AcceptRequestAsync(otherUserID: string): void {
    this.FriendshipService.ManageFriendRequestAsync(this.userID, otherUserID, 'Accept').subscribe({
      next: (response) => {
        if(response.messageID == 1){
          this.requests = this.requests.filter(req => req.requesterID !== otherUserID);
        }
        else{
          alert('Failed to accept friend request.' + response.messageDescription);
        }
      },
      error: (error) => {
        console.error('Error accepting friend request:', error);
      }
    });
  }

  RejectRequestAsync(otherUserID: string): void {
    this.FriendshipService.ManageFriendRequestAsync(this.userID, otherUserID, 'Reject').subscribe({
      next: (response) => {
        if(response.messageID == 1){
          this.requests = this.requests.filter(req => req.requesterID !== otherUserID);
        }
        else{
          alert('Failed to reject friend request.' + response.messageDescription);
        }
      },
      error: (error) => {
        console.error('Error rejecting friend request:', error);
      }
    });
  }
}
