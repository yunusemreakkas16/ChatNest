import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ChatService } from '../services/chat.service';
import { ChatMember, GetChatMembersResponseModel } from 'src/app/models/chat-member';
import { RemoveUserRequestDTO, SetGroupAdminRequestDTO, UpdateGroupNameRequestDTO } from 'src/app/models/chats-request';

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

  constructor(private route: ActivatedRoute,
    private chatService: ChatService) { }

  ngOnInit(): void {
    this.chatID = this.route.snapshot.paramMap.get('id')!;
    this.currentUserID = localStorage.getItem('userID') ?? '';
    this.loadMembers();

  }

  loadMembers(): void {
    this.chatService.getChatMembers(this.chatID).subscribe((res: GetChatMembersResponseModel) => {
      if (res.messageID === 1) {
        this.members = res.members;
        
        this.currentUserIsAdmin = this.members.find(m => m.userID === this.currentUserID)?.isAdmin ?? false;

      } else {
        console.error(res.messageDescription);
      }
    });
  }

    updateGroupName(): void {
    const payload: UpdateGroupNameRequestDTO = {
      chatID: this.chatID,
      newName: this.groupName,
      userID: this.currentUserID
    };

    this.chatService.updateGroupName(payload).subscribe(res => {
      if (res.messageID === 1) {
        console.log('Group name updated');
      } else {
        console.error(res.messageDescription);
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

    this.chatService.setGroupAdmin(payload).subscribe(res => {
      if (res.messageID === 1) {
        member.isAdmin = true;
      } else {
        console.error(res.messageDescription);
      }
    });
  }

  removeUser(member: ChatMember): void {
    const payload: RemoveUserRequestDTO = {
      chatID: this.chatID,
      userID: member.userID,
      adminID: this.currentUserID
    };

    this.chatService.removeUserFromChat(payload).subscribe(res => {
      if (res.messageID === 1) {
        this.members = this.members.filter(m => m.userID !== member.userID);
      } else {
        console.error(res.messageDescription);
      }
    });
  }


}
