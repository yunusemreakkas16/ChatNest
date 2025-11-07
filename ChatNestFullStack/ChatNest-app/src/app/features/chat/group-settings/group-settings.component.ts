import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ChatService } from '../services/chat.service';
import { RemoveUserRequestDTO, SetGroupAdminRequestDTO, UpdateGroupNameRequestDTO } from 'src/app/models/chats-request';
import { AuthService } from 'src/app/services/auth.service';
import { ChatMember, GetChatMembersResponseModel } from 'src/app/models/chats-response';

@Component({
  selector: 'app-group-settings',
  templateUrl: './group-settings.component.html',
  styleUrls: ['./group-settings.component.css']
})
export class GroupSettingsComponent implements OnInit {

  chatID: string = '';
  currentUserID: string = '';
  currentUserIsAdmin: boolean = false;
  groupName: string = '';
  members: ChatMember[] = [];

  newUserId: string = '';

  constructor(
    private route: ActivatedRoute,
    private chatService: ChatService,
    private authService: AuthService,
    private router:Router) { }

  ngOnInit(): void {
    this.chatID = this.route.snapshot.paramMap.get('id')!;
    this.currentUserID = localStorage.getItem('userID') ?? '';
    this.currentUserID = this.authService.getUserId() || '';
    this.loadMembers();

  }

  loadMembers(): void {
    this.chatService.getChatMembers(this.chatID).subscribe({
      next: (res:GetChatMembersResponseModel) => {
        if (res.messageID === 1) {
          this.members = res.members;
          
          this.currentUserIsAdmin = this.members.find(m => m.userID === this.currentUserID)?.isAdmin ?? false;
          console.log('currentUserID:', this.currentUserID);
          console.log('members:', this.members);


        }
        else {
          console.error(res.messageDescription);
        }
      }
    });
  }

    updateGroupName(): void {
    const payload: UpdateGroupNameRequestDTO = {
      chatID: this.chatID,
      newName: this.groupName,
      userID: this.currentUserID
    };

    this.chatService.updateGroupName(payload).subscribe({
      next: res =>{
        if (res.messageID === 1) {
          alert('Group name updated');
        }
        else {
          alert(res.messageDescription);
        }
      },
      error: err => {
        alert(`Group name could not be updated: ${err.error?.messageDescription || err.message}`);
        console.error('HTTP error:', err);
      }
    });
  }

  makeAdmin(member: ChatMember): void {
    const payload: SetGroupAdminRequestDTO = {
      chatID: this.chatID,
      userID: member.userID,
      adminID: this.currentUserID,
      makeAdmin: true
    };

    this.chatService.setGroupAdmin(payload).subscribe({
      next: res => {
        if (res.messageID === 1) {
          member.isAdmin = true;
        }
        else {
          alert(res.messageDescription);
        }
      },
      error: err => {
        alert(`Could not set admin: ${err.error?.messageDescription || err.message}`);
        console.error('HTTP error:', err);
      }
    });
  }

  addUser(newUserId: string): void {
    const ids = this.newUserId
      .split(',')
      .map(id => id.trim())
      .filter(id => id.length > 0);

    const payload = {
      chatID: this.chatID,
      userIDs: ids, // Array of user IDs
      adminID: this.currentUserID
    };
    
    this.chatService.addUserToChat(payload).subscribe({
      next: res => {
        if (res.messageID === 1) {
          // Return only new members
          res.members.forEach(m => {
            if (!this.members.find(x => x.userID === m.userID)) {
              this.members.push(m);
            }
          });
          this.newUserId = ''; // Clean input field
        } 
        else {
          console.error('Business error:', res.messageDescription);
        }
      },
      error: err => {
      // const backendMsg = err.error?.messageDescription || err.message;
        const backendMsg = err.error?.messageDescription || err.message;
        alert(`Users could not be added: ${backendMsg}`);
        console.error('HTTP error:', err);

      },
      complete: () => {
        console.log('Add user request completed');
      }
    });
  }

  removeUser(member: ChatMember): void {
    const payload: RemoveUserRequestDTO = {
      chatID: this.chatID,
      userID: member.userID,
      adminID: this.currentUserID
    };

    this.chatService.removeUserFromChat(payload).subscribe({
      next: res => {
      if (res.messageID === 1) {
        this.members = this.members.filter(m => m.userID !== member.userID);
      }
      else {
        console.error(res.messageDescription);
        }
      },
      error: err => {
        alert(`User could not be removed: ${err.error?.messageDescription || err.message}`);
        console.error('HTTP error:', err);
      }
    });
  }

  deleteChat(): void {
    this.chatService.deleteChat(this.chatID, this.currentUserID).subscribe(
      {
        next: res => {
          if (res.messageID === 1) {
            alert('Chat deleted successfully');
            this.router.navigate(['/chats']);
          }
          else {
            alert(res.messageDescription);
          }
        },
        error: err => {
          alert(`Chat could not be deleted: ${err.error?.messageDescription || err.message}`);
        },
        complete: () => {
          console.log('Delete chat request completed');
        }
      }
    )
  }

  leaveChat(): void {
    this.chatService.leaveChat(this.chatID, this.currentUserID).subscribe(
      {
        next: res => {
          if (res.messageID === 1) {
            alert('You have left the chat');
            this.router.navigate(['/chats']);
          }
          else {
            alert(res.messageDescription);
          }
        },
        error: err => {
          alert(`Could not leave the chat: ${err.error?.messageDescription || err.message}`);
        },
        complete: () => {
          console.log('Leave chat request completed');
        }
      }
    );
  }
}
