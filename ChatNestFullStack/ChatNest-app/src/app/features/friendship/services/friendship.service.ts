import { Injectable } from '@angular/core';
import { ApiService } from '../../api.service';
import { FriendListResponseModel, FriendRequestListResponseModel, FriendshipStatusResponseModel, ManageFriendRequestResponseModel, RemoveFriendResponseModel, SendFriendRequestResponseModel } from '../models/friendship';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FriendshipService {

  constructor(
    private ApiService: ApiService
  ) {}

  private baseUrl = '/api/Friendship';

  SendFriendRequestAsync(requesterId:string, recieverEmail:string): Observable<SendFriendRequestResponseModel> {
    return this.ApiService.post<SendFriendRequestResponseModel>(`${this.baseUrl}/SendFriendRequest?requesterId=${requesterId}&recieverEmail=${recieverEmail}`,{}).pipe(
      tap(response => {
        if (response.messageID === 2) {
          console.log('Request re-sent from cancelled state');
        } else if (response.messageID === 3) {
          console.log('Request re-sent from rejected state');
        } else if (response.messageID === 4) {
          console.log('Request re-sent from deleted state');
        }
      })
    );
  }

  GetFriendRequestsAsync(userId:string, direction: 'Sent' | 'Received'): Observable<FriendRequestListResponseModel> {
    return this.ApiService.post<FriendRequestListResponseModel>(`${this.baseUrl}/GetFriendRequests?userId=${userId}&direction=${direction}`, {});    
  }

  GetFriendlistAsync(userId:string): Observable<FriendListResponseModel> {
    return this.ApiService.post<FriendListResponseModel>(`${this.baseUrl}/GetFriendList?userId=${userId}`, {});    
  }

  ManageFriendRequestAsync(clientUserID:string, otherUserID:string, action: 'Accept' | 'Reject' | 'Cancel'): Observable<ManageFriendRequestResponseModel> {
    return this.ApiService.post<ManageFriendRequestResponseModel>(`${this.baseUrl}/ManageFriendRequest?clientUserID=${clientUserID}&otherUserID=${otherUserID}&action=${action}`,{});
  }

  GetFriendshipStatusAsync(userId:string, targetUserId:string): Observable<FriendshipStatusResponseModel> {
    return this.ApiService.post<FriendshipStatusResponseModel>(`${this.baseUrl}/FriendShipStatus?userId=${userId}&targetUserId=${targetUserId}`,{});
  }

  RemoveFriendAsync(userId:string, friendId:string): Observable<RemoveFriendResponseModel> {
    return this.ApiService.post<RemoveFriendResponseModel>(`${this.baseUrl}/RemoveFriend?userId=${userId}&friendId=${friendId}`,{});
  }
}
