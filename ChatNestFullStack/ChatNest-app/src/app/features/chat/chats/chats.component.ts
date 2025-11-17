import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/features/authenticate/services/auth.service';
import { ChatService } from '../services/chat.service';
import { ChatResponse } from 'src/app/features/chat/models/chats-response';

@Component({
  selector: 'app-chats',
  templateUrl: './chats.component.html',
  styleUrls: ['./chats.component.css']
})
export class ChatsComponent {

chats: ChatResponse[] = [];

  constructor(private router: Router, private authService: AuthService, private chatService:ChatService) {}

    chatId!: string;

ngOnInit(): void {
  const userId = this.authService.getUserId();
  this.chatService.getChats(userId).subscribe({
    next: (res) => {
      this.chats = res.chats || [];
    },
    error: (err) => console.error('There is not any chat to show', err)
  });
}

  openChat(chat: any)
  {
    this.router.navigate(['/chats', chat.chatID]);
  }

}
