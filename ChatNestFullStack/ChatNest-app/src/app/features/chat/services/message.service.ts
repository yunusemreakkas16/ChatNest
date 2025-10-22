import { Injectable } from '@angular/core';
import { ApiService } from '../../api.service';
import { Observable } from 'rxjs';
import { DeleteMessageResponseModel, MessagesListResponseModel, SendMessageResponseModel } from 'src/app/models/message-response';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private api: ApiService) { }

  getMessages(chatId: string, userId: string): Observable<MessagesListResponseModel> {
  return this.api.post(`/api/Message/GetMessagesList?chatID=${chatId}&userID=${userId}`, {});
  }

  sendMessage(message: { chatId: string; senderId: string; content: string }): Observable<SendMessageResponseModel> {
    return this.api.post(`/api/Message/SendMessage`, message);
  }


  deleteMessage(messageID: string, userID: string): Observable<DeleteMessageResponseModel> {
    return this.api.post<DeleteMessageResponseModel>(
      `/api/Message/DeleteMessage?messageID=${messageID}&userID=${userID}`,
      {} 
    );
  }

}