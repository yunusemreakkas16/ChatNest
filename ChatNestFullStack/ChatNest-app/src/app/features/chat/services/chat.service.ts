import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApiService } from '../../api.service';
import { ChatResponseModel, ChatResponse } from 'src/app/models/chats-response';

@Injectable({
  providedIn: 'root'
})

export class ChatService {

  private baseUrl = '/api/Chat';

  constructor(private api:ApiService) {}

  private chatsCache: ChatResponse[] = [];


  getChats(userID: string): Observable<ChatResponseModel> {
  return this.api.post<ChatResponseModel>(`${this.baseUrl}/GetChatSummariesForUser?userID=${userID}`, {})
    .pipe(
      tap(res => this.chatsCache = res.chats) // cache tut
    );
  }

  getChatName(chatId: string): string {
  return this.chatsCache.find(c => c.chatID === chatId)?.displayName ?? '';
  }


  createChat(payload: any): Observable<any> {
    return this.api.post<any>(`${this.baseUrl}/CreateChat`, payload);
  }
  addUserToChat(payload: any): Observable<any> {
    return this.api.post<any>(`${this.baseUrl}/AddUserToChat`, payload);
  }
  
  removeUserFromChat(payload: any): Observable<any> {
    return this.api.post<any>(`${this.baseUrl}/RemoveUserFromChat`, payload);
  }

  deleteChat(chatID: string, requesterID: string): Observable<any> {
    return this.api.post<any>(`${this.baseUrl}/DeleteChat?chatID=${chatID}&requesterID=${requesterID}`, {});
  }
  leaveChat(chatID: string, userID: string): Observable<any> {
    return this.api.post<any>(`${this.baseUrl}/LeaveChat?chatID=${chatID}&userID=${userID}`, {});
  }

  setGroupAdmin(payload: any): Observable<any> {
    return this.api.post<any>(`${this.baseUrl}/SetGroupAdmin`, payload);
  }

  updateGroupName(payload: any): Observable<any> {
    return this.api.post<any>(`${this.baseUrl}/UpdateGroupName`, payload);
  }
}
