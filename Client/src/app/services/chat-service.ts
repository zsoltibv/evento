import { AuthService } from './auth-service';
import { inject, Injectable, signal } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../../environment';
import { ChatMessage } from '../models/ChatMessage';
import { ChatUser } from '../models/ChatUser';
import { ChatClaim } from '../models/ChatClaim';
import { RestApiService } from './rest-api-service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private authService = inject(AuthService);
  private connection?: HubConnection;
  private api = inject(RestApiService);
  private router = inject(Router);

  messages = signal<ChatMessage[]>([]);
  onlineUsers = signal<ChatUser[]>([]);
  chatClaimed = signal<ChatClaim | null>(null);
  unreadMessagesGrouped = signal<Record<string, ChatMessage[]>>({});

  constructor() {
    this.connection = new HubConnectionBuilder()
      .withUrl(`${environment.apiBaseUrl}/hubs/chat`, {
        accessTokenFactory: () => this.authService.getToken() || '',
        transport: HttpTransportType.WebSockets,
        skipNegotiation: true,
      })
      .withAutomaticReconnect()
      .build();

    this.connection.on('ReceiveMessage', (msg: ChatMessage) => {
      this.messages.update((prev) => [...prev, msg]);
    });

    this.connection.on('UserOnline', (user: ChatUser) => {
      this.onlineUsers.update((list) => [...list.filter((u) => u.userId !== user.userId), user]);
    });

    this.connection.on('UserOffline', (user: ChatUser) => {
      this.onlineUsers.update((list) => list.filter((u) => u.userId !== user.userId));
    });

    this.connection.on('ChatClaimed', (userId: string, ownerId: string, ownerName?: string) => {
      console.log(`Chat with ${userId} claimed by ${ownerName || ownerId}`);
      this.chatClaimed.set({ userId, ownerId, ownerName } as ChatClaim);
    });

    this.connection.on('UnreadMessagesNotification', async () => {
      console.log('Fetching grouped unread messages...');
      if (!this.connection) return;

      try {
        const groupedUnread = await this.connection.invoke<Record<string, ChatMessage[]>>(
          'GetUnreadMessages',
          this.authService.userId()
        );

        console.log('Grouped unread messages:', groupedUnread);
        this.unreadMessagesGrouped.set(groupedUnread);
      } catch (err) {
        console.error('Failed to fetch grouped unread messages:', err);
      }
    });
  }

  async start() {
    if (this.connection?.state === 'Connected') return;
    await this.connection?.start();
    console.log('Connected to SignalR hub');

    const users = await this.connection?.invoke<ChatUser[]>('GetOnlineUsers');
    if (users) this.onlineUsers.set(users);
  }

  async sendMessage(senderId: string, receiverId: string, messageText: string) {
    await this.connection?.invoke('SendMessage', senderId, receiverId, messageText);
  }

  async sendMessageToUsers(senderId: string, receiverIds: string[], messageText: string) {
    await this.connection?.invoke('SendMessageToUsers', senderId, receiverIds, messageText);
  }

  async loadChatHistory(userId1: string, userId2: string) {
    const history = await this.connection?.invoke<ChatMessage[]>(
      'GetChatHistory',
      userId1,
      userId2
    );
    if (history) this.messages.set(history);
  }

  async getUserChats(userId: string): Promise<ChatUser[]> {
    return this.api.get<ChatUser[]>('/api/chats/user');
  }

  async markMessagesAsRead(senderId: string) {
    if (!this.connection || this.connection.state !== 'Connected') return;

    const receiverId = this.authService.userId();

    try {
      await this.connection.invoke('MarkMessagesAsRead', receiverId, senderId);

      const updated = { ...this.unreadMessagesGrouped() };
      delete updated[senderId];
      this.unreadMessagesGrouped.set(updated);
    } catch (err) {
      console.error('Failed mark as read messages:', err);
    }
  }
}
