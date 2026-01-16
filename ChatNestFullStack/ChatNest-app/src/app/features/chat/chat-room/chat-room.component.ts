import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MessageService } from '../services/message.service';
import { AuthService } from 'src/app/features/authenticate/services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageViewModel } from 'src/app/features/chat/models/view-models/message-view';
import { ChatService } from '../services/chat.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
    selector: 'app-chat-room',
    templateUrl: './chat-room.component.html',
    styleUrls: ['./chat-room.component.css'],
    standalone: false
})
export class ChatRoomComponent implements OnInit {

  messageForm!: FormGroup;
  chatId!: string;
  userId!: string;
  groupName: string = '';
  messages: MessageViewModel[] = [];

  statusMessage: string = '';
  statusType: 'success' | 'error' | '' = '';

  constructor(
    private route: ActivatedRoute,
    private messageService: MessageService, 
    private authService: AuthService,
    private chatService: ChatService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.chatId = this.route.snapshot.paramMap.get('id')!;
    this.userId = this.authService.getUserID();

    this.messageForm = this.fb.group({
      content: ['', [Validators.required, Validators.maxLength(10000)]]
    });

    // Find group name
    this.chatService.getChats(this.userId).subscribe({
      next: (res) => {
        const chat = res.chats.find(c => c.chatID === this.chatId);
        this.groupName = chat?.displayName ?? '';
      },
      error: (err: HttpErrorResponse) => {
        console.error('Error fetching chat info:', err);
        this.statusMessage = err.error?.messageDescription || 'Failed to load chat info.';
        this.statusType = 'error';
      }
    });

    // Load messages
    this.messageService.getMessages(this.chatId, this.userId).subscribe({
      next: (res) => {
        this.messages = [
          ...(res.groupMessages || []).map(m => ({
            messageID: m.messageID,
            chatID: m.chatID,
            content: m.content,
            sentAt: m.sentAt,
            senderID: m.senderID,
            senderName: m.displayName,
            status: 'sent' as const
          })),
          ...(res.oneToOneMessages || []).map(m => ({
            messageID: m.messageID,
            chatID: m.chatID,
            content: m.content,
            sentAt: m.sentAt,
            senderID: m.senderID,
            senderName: m.senderName,
            status: 'sent' as const
          }))
        ];
      },
      error: (err: HttpErrorResponse) => {
        console.error('Error fetching messages:', err);
        this.statusMessage = err.error?.messageDescription || 'Failed to load messages.';
        this.statusType = 'error';
      }
    });
  }

  sendMessage(): void {
    if (this.messageForm.invalid) {
      return;
    }
    
    const content = this.messageForm.value.content;

    // Optimistic UI Update
    const tempMessage: MessageViewModel = {
      messageID: 'temp-' + Date.now(),
      chatID: this.chatId,
      content,
      sentAt: new Date().toISOString(),
      senderID: this.userId,
      senderName: 'You',
      status: 'sending'
    };
    this.messages.push(tempMessage);

    this.messageService.sendMessage({ chatId: this.chatId, senderId: this.userId, content }).subscribe({
      next: (res) => {
        tempMessage.messageID = res.newMessageID;
        tempMessage.status = 'sent';
        this.messageForm.reset();
      },
      error: (err: HttpErrorResponse) => {
        tempMessage.status = 'failed';
        console.error('Error sending message:', err);
        this.statusMessage = err.error?.messageDescription || 'Failed to send message.';
        this.statusType = 'error';
      }
    });
  }

  deleteMessage(messageID: string): void {
    this.messageService.deleteMessage(messageID, this.userId).subscribe({
      next: () => {
        this.messages = this.messages.filter(m => m.messageID !== messageID);
        this.statusMessage = 'Message deleted successfully.';
        this.statusType = 'success';
      },
      error: (err: HttpErrorResponse) => {
        console.error('Error deleting message:', err);
        this.statusMessage = err.error?.messageDescription || 'Failed to delete message.';
        this.statusType = 'error';
      }
    });
  }
}