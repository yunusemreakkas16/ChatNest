// models/chat-request.ts
export interface CreateChatRequestDTO {
  isGroup: boolean;
  name?: string;
  createdBy: string;
  participantIDs?: string[];
  targetUserID?: string;
}

export interface AddUserRequestDTO {
  chatID: string;
  adminID: string;
  userIDs?: string[];
}

export interface RemoveUserRequestDTO {
  chatID: string;
  adminID: string;
  userID: string;
}

export interface SetGroupAdminRequestDTO {
  chatID: string;
  adminID: string;
  userID: string;
  makeAdmin: boolean;
}

export interface UpdateGroupNameRequestDTO {
  chatID: string;
  userID: string;
  newName: string;
}