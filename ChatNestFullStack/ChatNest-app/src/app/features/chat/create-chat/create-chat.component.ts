import { Component } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { FriendshipService } from '../../friendship/services/friendship.service';
import { CreateChatResponseModel } from '../models/chats-response';
import { AuthService } from '../../authenticate/services/auth.service';
import { FriendListResponseModel, FriendResponse } from '../../friendship/models/friendship';
import { CreateChatRequestDTO } from '../models/chats-request';

@Component({
  selector: 'app-create-chat',
  templateUrl: './create-chat.component.html',
  styleUrls: ['./create-chat.component.css']
})
export class CreateChatComponent {

  public friendList: FriendResponse[] = [];
  private userID!: string;


  public emailError: string = '';
  public chatError: string = '';

  public enteredMails: string[] = [];
  public selectedFriends: FriendResponse[] = [];
  public isGroupChat: boolean = false;
  public newEmail: string = '';
  public groupName: string = '';

  constructor(
    private ChatService: ChatService,
    private FriendShipService: FriendshipService,
    private AuthService: AuthService
  ) {}

  ngOnInit(): void {
    this.userID = this.AuthService.getUserID();
    this.FriendShipService.GetFriendlistAsync(this.userID).subscribe({
      next: (response: FriendListResponseModel) => {
        if (response.messageID === 1 || response.messageID === 0) {
          this.friendList = response.friends;
        }
      },
      error: (error: FriendListResponseModel) => {
        alert('An error occurred while fetching friend list: ' + error.messageID + ' ' + error.messageDescription);
      }
    });
  }

  addEmail(): void {
    const email = this.newEmail.trim().toLowerCase();
    if (email.length > 0) {
      const emailRegex = /^[^\s@]+@[^\s@]+\.(com|net|org)$/;
      if (!emailRegex.test(email)) {
        this.emailError = 'Invalid email format.';
        return;
      }
      if (this.enteredMails.includes(email)) {
        this.emailError = 'This email is already added.';
        return;
      }
      this.enteredMails.push(email);
      this.emailError = ''
      this.newEmail = '';
    }
  }



  addFriendToGroup(friendID: string): void {
    const friend = this.friendList.find(f => f.friendID === friendID);
    if (friend && !this.selectedFriends.some(f => f.friendID === friendID)) {
      this.selectedFriends.push(friend);
    }
  }

  removeFriend(friendID: string): void {
    this.selectedFriends = this.selectedFriends.filter(f => f.friendID !== friendID);
  }

  removeEmail(mail: string): void {
    this.enteredMails = this.enteredMails.filter(m => m !== mail);
  }

  CreateChat(isGroup: boolean, selectedFriendIDs: string[], enteredEmails: string[], groupName?: string): void {
  const payload: CreateChatRequestDTO = {
    isGroup: isGroup,
    createdBy: this.userID,
    name: isGroup ? (groupName?.trim().length ? groupName.trim() : undefined) : undefined,
    participantIDs: isGroup ? selectedFriendIDs : undefined,
    targetUserID: !isGroup && selectedFriendIDs.length > 0 ? selectedFriendIDs[0] : undefined,
    targetEmails: enteredEmails.length > 0 ? enteredEmails : undefined
  };

  this.ChatService.createChat(payload).subscribe({
    next: (response: CreateChatResponseModel) => {
      if (response.messageID === 1) {
        alert('Chat created successfully!');
      } else {
        alert('Failed to create chat. ' + response.messageDescription);
      }
    },
    error: (error) => {
      console.error('Error creating chat:', error);
      alert('An error occurred while creating the chat.');
    }
  });
}


  startGroupChat(): void {
    const selectedFriendIDs = this.selectedFriends.map(f => f.friendID);

    if (!selectedFriendIDs.length && !this.enteredMails.length) {
      alert('Please select at least one participant.');
      return;
    }

    const payload: CreateChatRequestDTO = {
      isGroup: true,
      createdBy: this.userID,
      name: this.groupName?.trim().length ? this.groupName.trim() : undefined,
      participantIDs: selectedFriendIDs,
      targetEmails: this.enteredMails.length > 0 ? this.enteredMails : undefined
    };

    console.log('Payload being sent:', payload);

    this.ChatService.createChat(payload).subscribe({
      next: (response: CreateChatResponseModel) => {
        if (response.messageID === 1) {
          this.chatError = '';
          alert('Chat created successfully!');
        } else {
          this.chatError = response.messageDescription;
        }
      },
      error: (error) => {
        console.error('Error creating chat:', error);
        this.chatError = 'An unexpected error occurred.';
      }
    });

  }
}