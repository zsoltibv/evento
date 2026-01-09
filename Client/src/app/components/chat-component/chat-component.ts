import { AuthService } from './../../services/auth-service';
import { Component, computed, effect, ElementRef, inject, signal, viewChild } from '@angular/core';
import { ChatService } from '../../services/chat-service';
import { FormsModule } from '@angular/forms';
import { PanelModule } from 'primeng/panel';
import { ListboxModule } from 'primeng/listbox';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { CommonModule } from '@angular/common';
import { ChatUser } from '../../models/ChatUser';
import { HourOnlyPipe } from '../../pipe/hour-only-pipe-pipe';
import { ActivatedRoute } from '@angular/router';
import { EmojiPicker } from '../emoji-picker/emoji-picker';

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
    HourOnlyPipe,
    EmojiPicker,
  ],
  templateUrl: './chat-component.html',
  styleUrl: './chat-component.scss',
})
export class ChatComponent {
  private authService = inject(AuthService);
  private chatService = inject(ChatService);
  private route = inject(ActivatedRoute);

  messageText = signal('');
  selectedUser = signal<ChatUser | null>(null);
  chatUsers = signal<ChatUser[]>([]);
  showDateIndex: number | null = null;
  senderId?: string;

  messagesContainer = viewChild<ElementRef<HTMLDivElement>>('messagesContainer');

  currentUser = computed(() => {
    const userId = this.authService.userId();
    return this.chatService.onlineUsers().find((u) => u.userId === userId) ?? null;
  });

  showOnlineUsers = computed(() =>
    this.chatService.onlineUsers().filter((u) => u.userId !== this.authService.userId())
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

  constructor() {
    effect(() => {
      this.messages();
      setTimeout(() => this.scrollToBottomOfChatContainer(), 50);
    });
  }

  async ngOnInit() {
    await this.chatService.start();

    const chatUsers = await this.chatService.getUserChats(this.authService.userId()!);
    this.chatUsers.set(chatUsers);

    if (chatUsers.length > 0) {
      this.selectedUser.set(chatUsers[0]);
      await this.loadChatHistory();
    }

    //subcribe to chat redirects
    this.route.queryParams.subscribe(async (params) => {
      this.senderId = params['user'];

      if (this.senderId) {
        await this.chatService.markMessagesAsRead(this.senderId);
        const currentUserId = this.authService.userId();
        await this.chatService.loadChatHistory(currentUserId!, this.senderId);
      }
    });
  }

  async selectUser(user: ChatUser) {
    this.selectedUser.set(user);
    await this.loadChatHistory();
  }

  private async loadChatHistory() {
    if (!this.currentUser() || !this.selectedUser()) return;
    await this.chatService.loadChatHistory(this.currentUser()!.userId, this.selectedUser()!.userId);
  }

  async sendMessage() {
    if (!this.currentUser() || !this.selectedUser() || !this.messageText()) return;

    await this.chatService.sendMessage(
      this.currentUser()!.userId,
      this.selectedUser()!.userId,
      this.messageText()
    );

    this.messageText.set('');
  }

  isUserOnline(user: ChatUser): boolean {
    return this.showOnlineUsers().some((u) => u.userId === user.userId);
  }

  private scrollToBottomOfChatContainer() {
    const container = this.messagesContainer()?.nativeElement;
    if (container) {
      container.scrollTop = container.scrollHeight;
    }
  }

  toggleDate(index: number) {
    if (this.showDateIndex === index) {
      this.showDateIndex = null;
    } else {
      this.showDateIndex = index;
    }

    const msgs = this.messages();
    if (index === msgs.length - 1) {
      setTimeout(() => this.scrollToBottomOfChatContainer(), 50);
    }
  }

  openEmojiPicker(event: MouseEvent, picker: HTMLElement) {
    event.stopPropagation();
    picker.toggleAttribute('hidden');
  }
}
