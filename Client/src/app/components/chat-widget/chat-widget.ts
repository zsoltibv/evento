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

  targetUserIds = [this.authService.userId()];

  toggleChat() {
    this.isOpen.set(!this.isOpen());
  }

  async ngOnInit() {
    await this.chatService.start();

    this.chatService.messages().forEach((msg) => {
      if (
        this.targetUserIds.includes(msg.receiver.userId) ||
        this.targetUserIds.includes(msg.sender.userId)
      ) {
        this.messages.set([...this.messages(), msg]);
      }
    });
  }

  // async sendMessage() {
  //   if (!this.messageText()) return;

  //   const currentUser = this.chatService
  //     .onlineUsers()
  //     .find((u) => u.userId === this.authService.userId())!;

  //   for (const userId of this.targetUserIds) {
  //     const receiver: ChatUser = this.chatService.onlineUsers().find((u) => u.userId === userId)!;

  //     const msg: ChatMessage = {
  //       sender: currentUser,
  //       receiver,
  //       messageText: this.messageText(),
  //     };

  //     await this.chatService.sendMessage(msg);
  //     this.messages.set([...this.messages(), msg]);
  //   }

  //   this.messageText.set('');
  // }
}
