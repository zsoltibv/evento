import { AuthService } from './../../services/auth-service';
import { Component, computed, effect, inject, signal } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { ButtonModule } from 'primeng/button';
import { PopoverModule } from 'primeng/popover';
import { ChatService } from '../../services/chat-service';
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

  targetUserIds = signal<string[]>(['00000000-0000-0000-0000-000000000001']);
  chatOwnerName = signal<string>('Support Agent');

  messages = computed(() => {
    const all = this.chatService.messages();
    const targets = this.targetUserIds();
    return all.filter(
      (msg) => targets.includes(msg.sender.userId) || targets.includes(msg.receiver.userId)
    );
  });

  currentUserId = computed(() => this.authService.userId());

  constructor() {
    effect(async () => {
      const claim = this.chatService.chatClaimed();
      const currentUser = this.currentUserId();
      console.log(claim + ' ' + currentUser);

      if (claim && claim.userId === currentUser) {
        console.log(`Chat claimed by ${claim.ownerName || claim.ownerId}`);
        this.targetUserIds.set([claim.ownerId]);
        this.chatOwnerName.set(claim.ownerName || 'Support Agent');
        await this.chatService.loadChatHistory(claim.userId, claim.ownerId);
      }
    });
  }

  toggleChat() {
    this.isOpen.set(!this.isOpen());
  }

  async ngOnInit() {
    await this.chatService.start();

    const claim = this.chatService.chatClaimed();
    const currentUser = this.authService.userId();

    if (claim && claim.userId === currentUser) {
      this.targetUserIds.set([claim.ownerId]);
      this.chatOwnerName.set(claim.ownerName || 'Support Agent');
      await this.chatService.loadChatHistory(claim.userId, claim.ownerId);
    }
  }

  async sendMessage() {
    if (!this.messageText()) return;
    const senderId = this.authService.userId();
    const targets = this.targetUserIds();
    if (!senderId || targets.length === 0) return;

    await this.chatService.sendMessageToUsers(senderId, targets, this.messageText());
    this.messageText.set('');
  }
}
