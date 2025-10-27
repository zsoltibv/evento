import { AuthService } from './auth-service';
import { inject, Injectable, signal } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../../environment';
import { ChatMessage } from '../models/ChatMessage';
import { ChatUser } from '../models/ChatUser';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private authService = inject(AuthService);
  private connection?: HubConnection;

  messages = signal<ChatMessage[]>([]);
  onlineUsers = signal<ChatUser[]>([]);

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
  }

  async start() {
    await this.connection?.start();
    console.log('Connected to SignalR hub');

    const users = await this.connection?.invoke<ChatUser[]>('GetOnlineUsers');
    if (users) this.onlineUsers.set(users);
  }

  async sendMessage(message: ChatMessage) {
    await this.connection?.invoke(
      'SendMessage',
      message.sender.userId,
      message.receiver.userId,
      message.message
    );
  }
}
