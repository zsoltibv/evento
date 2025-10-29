import { ChatUser } from './ChatUser';

export interface ChatMessage {
  sender: ChatUser;
  receiver: ChatUser;
  messageText: string;
  sentAt: Date;
}
