import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ChatService } from '../services/chat.service';
import { RemoveUserRequestDTO, SetGroupAdminRequestDTO, UpdateGroupNameRequestDTO } from 'src/app/features/chat/models/chats-request';
import { AuthService } from 'src/app/features/authenticate/services/auth.service';
import { ChatMember, GetChatMembersResponseModel } from 'src/app/features/chat/models/chats-response';

@Component({
    selector: 'app-group-settings',
    templateUrl: './group-settings.component.html',
    styleUrls: ['./group-settings.component.css'],
    standalone: false
})
export class GroupSettingsComponent implements OnInit {

  chatID: string = '';
  currentUserID: string = '';
  currentUserIsAdmin: boolean = false;
  groupName: string = '';
  members: ChatMember[] = [];

  isGroupChat: boolean = false;
  newUserId: string = '';

  // 🔴 Inline feedback state
  statusMessage: string = '';
  statusType: 'success' | 'error' | '' = '';

  constructor(
    private route: ActivatedRoute,
    private chatService: ChatService,
    private authService: AuthService,
    private router:Router) { }

  ngOnInit(): void {
    this.chatID = this.route.snapshot.paramMap.get('id')!;
    this.currentUserID = this.authService.getUserID() || '';

    // Getting chat type and group name
    this.chatService.getChats(this.currentUserID).subscribe({
      next: res => {
        const chat = res.chats.find(c => c.chatID === this.chatID);
        this.isGroupChat = chat?.isGroup ?? false;
        this.groupName = chat?.displayName ?? '';
        this.loadMembers(); // Load members after determining chat type
      },
      error: err => {
        this.statusMessage = err.error?.messageDescription || 'Failed to load chat info.';
        this.statusType = 'error';
      }
    });
  }

  loadMembers(): void {
    this.chatService.getChatMembers(this.chatID).subscribe({
      next: (res: GetChatMembersResponseModel) => {
        if (res.messageID === 1) {
          this.members = res.members;
          this.currentUserIsAdmin = this.members.find(m => m.userID === this.currentUserID)?.isAdmin ?? false;
        } else {
          this.statusMessage = res.messageDescription;
          this.statusType = 'error';
        }
      },
      error: err => {
        this.statusMessage = err.error?.messageDescription || 'Failed to load members.';
        this.statusType = 'error';
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
          this.statusMessage = 'Group name updated successfully.';
          this.statusType = 'success';
        } else {
          this.statusMessage = res.messageDescription;
          this.statusType = 'error';
        }
      },
      error: err => {
        this.statusMessage = err.error?.messageDescription || 'Group name could not be updated.';
        this.statusType = 'error';
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
          this.statusMessage = `${member.userName} is now an admin.`;
          this.statusType = 'success';
        } else {
          this.statusMessage = res.messageDescription;
          this.statusType = 'error';
        }
      },
      error: err => {
        this.statusMessage = err.error?.messageDescription || 'Could not set admin.';
        this.statusType = 'error';
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
      userIDs: ids,
      adminID: this.currentUserID
    };
    
    this.chatService.addUserToChat(payload).subscribe({
      next: res => {
        if (res.messageID === 1) {
          res.members.forEach(m => {
            if (!this.members.find(x => x.userID === m.userID)) {
              this.members.push(m);
            }
          });
          this.newUserId = '';
          this.statusMessage = 'User(s) added successfully.';
          this.statusType = 'success';
        } else {
          this.statusMessage = res.messageDescription;
          this.statusType = 'error';
        }
      },
      error: err => {
        this.statusMessage = err.error?.messageDescription || 'Users could not be added.';
        this.statusType = 'error';
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
          this.statusMessage = `${member.userName} removed from chat.`;
          this.statusType = 'success';
        } else {
          this.statusMessage = res.messageDescription;
          this.statusType = 'error';
        }
      },
      error: err => {
        this.statusMessage = err.error?.messageDescription || 'User could not be removed.';
        this.statusType = 'error';
      }
    });
  }

  deleteChat(): void {
    this.chatService.deleteChat(this.chatID, this.currentUserID).subscribe({
      next: res => {
        if (res.messageID === 1) {
          this.statusMessage = 'Chat deleted successfully.';
          this.statusType = 'success';
          this.router.navigate(['/chats']);
        } else {
          this.statusMessage = res.messageDescription;
          this.statusType = 'error';
        }
      },
      error: err => {
        this.statusMessage = err.error?.messageDescription || 'Chat could not be deleted.';
        this.statusType = 'error';
      }
    });
  }

  leaveChat(): void {
    this.chatService.leaveChat(this.chatID, this.currentUserID).subscribe({
      next: res => {
        if (res.messageID === 1) {
          this.statusMessage = 'You have left the chat.';
          this.statusType = 'success';
          this.router.navigate(['/chats']);
        } else {
          this.statusMessage = res.messageDescription;
          this.statusType = 'error';
        }
      },
      error: err => {
        this.statusMessage = err.error?.messageDescription || 'Could not leave the chat.';
        this.statusType = 'error';
      }
    });
  }
}