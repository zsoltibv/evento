import { AuthService } from './../../services/auth-service';
import { Component, computed, inject, signal } from '@angular/core';
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

  private chatService = inject(ChatService);
  private authService = inject(AuthService);

  targetUserIds: string[] = ['00000000-0000-0000-0000-000000000001'];

  messages = computed(() => {
    const all = this.chatService.messages();
    return all.filter(
      (msg) =>
        this.targetUserIds.includes(msg.sender.userId) ||
        this.targetUserIds.includes(msg.receiver.userId)
    );
  });

  currentUserId = computed(() => {
    return this.authService.userId();
  });

  toggleChat() {
    this.isOpen.set(!this.isOpen());
  }

  async ngOnInit() {
    await this.chatService.start();
  }

  async sendMessage() {
    if (!this.messageText()) return;

    const senderId = this.authService.userId();
    if (!senderId) return;

    await this.chatService.sendMessageToUsers(senderId, this.targetUserIds, this.messageText());

    this.messageText.set('');
  }
}
