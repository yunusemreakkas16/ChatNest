import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/features/authenticate/services/auth.service';
import { ChatService } from '../services/chat.service';
import { ChatResponse } from 'src/app/features/chat/models/chats-response';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
    selector: 'app-chats',
    templateUrl: './chats.component.html',
    styleUrls: ['./chats.component.css'],
    standalone: false
})
export class ChatsComponent {

  chats: ChatResponse[] = [];
  chatId!: string;

  statusMessage: string = '';
  statusType: 'success' | 'error' | '' = '';

  constructor(
    private router: Router,
    private authService: AuthService,
    private chatService: ChatService
  ) {}

  ngOnInit(): void {
    const userId = this.authService.getUserID();
    this.chatService.getChats(userId).subscribe({
      next: (res) => {
        this.chats = res.chats || [];
        if (this.chats.length === 0) {
          this.statusMessage = 'No chats available.';
          this.statusType = 'error';
        }
      },
      error: (err: HttpErrorResponse) => {
        console.error('Error fetching chats:', err);
        this.statusMessage = err.error?.messageDescription || 'Failed to load chats.';
        this.statusType = 'error';
      }
    });
  }

  openChat(chat: ChatResponse): void {
    this.router.navigate(['/chats', chat.chatID]);
  }
}