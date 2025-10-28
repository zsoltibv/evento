import { AuthService } from './../../services/auth-service';
import { Component, computed, inject, signal } from '@angular/core';
import { ChatService } from '../../services/chat-service';
import { FormsModule } from '@angular/forms';
import { PanelModule } from 'primeng/panel';
import { ListboxModule } from 'primeng/listbox';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { CommonModule } from '@angular/common';
import { ChatUser } from '../../models/ChatUser';
import { ChatMessage } from '../../models/ChatMessage';

@Component({
  selector: 'app-chat-component',
  imports: [
    FormsModule,
    PanelModule,
    ListboxModule,
    TextareaModule,
    ButtonModule,
    InputTextModule,
    CommonModule,
  ],
  templateUrl: './chat-component.html',
  styleUrl: './chat-component.scss',
})
export class ChatComponent {
  private authService = inject(AuthService);
  private chatService = inject(ChatService);

  messageText = signal('');
  selectedUser = signal<ChatUser | null>(null);

  currentUser = computed(() => {
    const userId = this.authService.userId();
    return this.chatService.onlineUsers().find((u) => u.userId === userId) ?? null;
  });

  showOnlineUsers = computed(() =>
    this.chatService.onlineUsers().filter((user) => user.userId !== this.authService.userId())
  );

  messages = computed(() =>
    this.chatService
      .messages()
      .filter(
        (m) =>
          (m.sender.userId === this.currentUser()?.userId &&
            m.receiver.userId === this.selectedUser()?.userId) ||
          (m.sender.userId === this.selectedUser()?.userId &&
            m.receiver.userId === this.currentUser()?.userId)
      )
  );

  async ngOnInit() {
    await this.chatService.start();

    const users = this.showOnlineUsers();
    if (users.length > 0) {
      this.selectedUser.set(users[0]);
    }
  }

  selectUser(user: ChatUser) {
    this.selectedUser.set(user);
  }

  async sendMessage() {
    if (!this.currentUser() || !this.selectedUser() || !this.messageText()) return;

    const msg: ChatMessage = {
      sender: this.currentUser()!,
      receiver: this.selectedUser()!,
      message: this.messageText(),
    };

    await this.chatService.sendMessage(msg);
    this.messageText.set('');
  }
}
