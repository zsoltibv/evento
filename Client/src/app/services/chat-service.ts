import { AuthService } from './auth-service';
import { inject, Injectable, signal } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../../environment';
import { ChatMessage } from '../models/ChatMessage';
import { ChatUser } from '../models/ChatUser';
import { ChatClaim } from '../models/ChatClaim';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private authService = inject(AuthService);
  private connection?: HubConnection;

  messages = signal<ChatMessage[]>([]);
  onlineUsers = signal<ChatUser[]>([]);
  chatClaimed = signal<ChatClaim | null>(null);

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
}
