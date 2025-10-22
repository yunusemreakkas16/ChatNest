import { BaseResponse } from "./base-response";

export interface ChatResponse {
    chatID: string;
    displayName: string;
    isGroup: boolean;
    lastMessageContent: string;
    lastMessageDate: string;
}

export interface ChatResponseModel extends BaseResponse {
  chats: ChatResponse[];
}

export interface CreateChatResponseModel extends BaseResponse {}
export interface AddUserToChatResponseModel extends BaseResponse {}
export interface RemoveUserFromChatResponseModel extends BaseResponse {}
export interface LeaveChatResponseModel extends BaseResponse {}
export interface DeleteChatResponseModel extends BaseResponse {}
export interface ManageGroupAdminResponseModel extends BaseResponse {}
export interface UpdateGroupNameResponseModel extends BaseResponse {}



