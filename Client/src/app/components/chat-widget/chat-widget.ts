import { AuthService } from './../../services/auth-service';
import { Component, inject, signal } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { ButtonModule } from 'primeng/button';
import { PopoverModule } from 'primeng/popover';
import { ChatMessage } from '../../models/ChatMessage';
import { ChatService } from '../../services/chat-service';
import { ChatUser } from '../../models/ChatUser';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-chat-widget',
  imports: [PanelModule, FormsModule, ButtonModule, PopoverModule, CommonModule, InputTextModule],
  templateUrl: './chat-widget.html',
  styleUrl: './chat-widget.scss',
})
export class ChatWidget {
  isOpen = signal(false);
  messageText = signal('');
  messages = signal<ChatMessage[]>([]);

  private chatService = inject(ChatService);
  private authService = inject(AuthService);

  targetUserIds: string[] = ['00000000-0000-0000-0000-000000000001'];

  toggleChat() {
    this.isOpen.set(!this.isOpen());
  }

  async ngOnInit() {
    await this.chatService.start();

    const relevantMessages = this.chatService
      .messages()
      .filter(
        (msg) =>
          this.targetUserIds.includes(msg.sender.userId) ||
          this.targetUserIds.includes(msg.receiver.userId)
      );
    this.messages.set(relevantMessages);

    this.chatService.messages().forEach((msg) => {
      if (
        this.targetUserIds.includes(msg.sender.userId) ||
        this.targetUserIds.includes(msg.receiver.userId)
      ) {
        this.messages.update((prev) => [...prev, msg]);
      }
    });
  }

  async sendMessage() {
    if (!this.messageText()) return;

    const senderId = this.authService.userId();
    await this.chatService.sendMessageToUsers(senderId!, this.targetUserIds, this.messageText());

    const sender: ChatUser = this.chatService.onlineUsers().find((u) => u.userId === senderId)!;
    this.targetUserIds.forEach((receiverId) => {
      const receiver: ChatUser = this.chatService
        .onlineUsers()
        .find((u) => u.userId === receiverId)!;
      const msg: ChatMessage = {
        sender,
        receiver,
        messageText: this.messageText(),
        sentAt: new Date(),
      };
      this.messages.update((prev) => [...prev, msg]);
    });

    this.messageText.set('');
  }
}
