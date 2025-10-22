export interface MessageViewModel {
  messageID: string;
  chatID: string;
  content: string;
  sentAt: string;
  senderID: string;
  senderName?: string;
  status: 'sending' | 'sent' | 'failed' ; 
}