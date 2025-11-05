import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApiService } from '../../api.service';
import { ChatResponseModel, ChatResponse, CreateChatResponseModel, AddUserToChatResponseModel, RemoveUserFromChatResponseModel, DeleteChatResponseModel, LeaveChatResponseModel, ManageGroupAdminResponseModel, UpdateGroupNameResponseModel, GetChatMembersResponseModel } from 'src/app/models/chats-response';
import { AddUserRequestDTO, CreateChatRequestDTO, RemoveUserRequestDTO, SetGroupAdminRequestDTO, UpdateGroupNameRequestDTO } from 'src/app/models/chats-request';


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

  getChatMembers(chatID: string): Observable<GetChatMembersResponseModel> {
  return this.api.post<GetChatMembersResponseModel>(`${this.baseUrl}/GetChatMembers?chatID=${chatID}`, {}
  );
}


  createChat(payload: CreateChatRequestDTO): Observable<CreateChatResponseModel> {
    return this.api.post<CreateChatResponseModel>(`${this.baseUrl}/CreateChat`, payload);
  }
  addUserToChat(payload: AddUserRequestDTO): Observable<AddUserToChatResponseModel> {
    return this.api.post<AddUserToChatResponseModel>(`${this.baseUrl}/AddUserToChat`, payload);
  }
  
  removeUserFromChat(payload: RemoveUserRequestDTO): Observable<RemoveUserFromChatResponseModel> {
    return this.api.post<RemoveUserFromChatResponseModel>(`${this.baseUrl}/RemoveUserFromChat`, payload);
  }

  deleteChat(chatID: string, requesterID: string): Observable<DeleteChatResponseModel> {
    return this.api.post<DeleteChatResponseModel>(`${this.baseUrl}/DeleteChat?chatID=${chatID}&requesterID=${requesterID}`, {});
  }
  leaveChat(chatID: string, userID: string): Observable<LeaveChatResponseModel> {
    return this.api.post<LeaveChatResponseModel>(`${this.baseUrl}/LeaveChat?chatID=${chatID}&userID=${userID}`, {});
  }

  setGroupAdmin(payload: SetGroupAdminRequestDTO): Observable<ManageGroupAdminResponseModel> {
    return this.api.post<ManageGroupAdminResponseModel>(`${this.baseUrl}/SetGroupAdmin`, payload);
  }

  updateGroupName(payload: UpdateGroupNameRequestDTO): Observable<UpdateGroupNameResponseModel> {
    return this.api.post<UpdateGroupNameResponseModel>(`${this.baseUrl}/UpdateGroupName`, payload);
  }
}
