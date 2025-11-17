import { BaseResponse } from "src/app/base-response-model/base-response";

export interface Friendship {
}

export interface SendFriendRequestResponseModel extends BaseResponse {}
export interface RemoveFriendResponseModel extends BaseResponse {}
export interface ManageFriendRequestResponseModel extends BaseResponse {}

export interface FriendResponse{
  friendID: string;
  friendName: string;
  friendMail: string;
  createdAt: string;
}

export interface FriendRequestResponse{

  requesterID: string | null; // Populated only when direction = 'received'; null when direction = 'sent'
  receiverID: string | null; // Populated only when direction = 'sent'; null when direction = 'received'
  name: string;
  email: string;
  friendShipStatus: string;

}

export interface FriendListResponseModel extends BaseResponse {
    friends: FriendResponse[];
}

export interface FriendRequestListResponseModel extends BaseResponse {
    friendRequests: FriendRequestResponse[];
}

export interface FriendshipStatusResponseModel{
    status: string
}

