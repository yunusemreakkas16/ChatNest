import { BaseResponse } from "./base-response";

export interface ChatMember {
  userID: string;  
  chatID: string;   
  userName: string;
  isAdmin: boolean;
  joinedAt: string;
}

export interface GetChatMembersResponseModel extends BaseResponse {
  members: ChatMember[];
}
