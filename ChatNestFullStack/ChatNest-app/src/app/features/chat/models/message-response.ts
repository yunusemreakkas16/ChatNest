import { BaseResponse } from '../../../base-response-model/base-response';

export interface SendMessageResponseModel extends BaseResponse {
  newMessageID: string;
}
export interface DeleteMessageResponseModel extends BaseResponse {}

export interface GroupMessageResponse {
  messageID: string;
  chatID: string;
  content: string;
  sentAt: string;   
  senderID: string;   
  displayName: string;  
}

export interface OneToOneMessageResponse {
  messageID: string;
  chatID: string;
  content: string;
  sentAt: string;
  senderID: string;
  senderName: string;
  receiverID: string;
  receiverName: string;
}

export interface MessagesListResponseModel extends BaseResponse {
  groupMessages?: GroupMessageResponse[];
  oneToOneMessages?: OneToOneMessageResponse[];
}