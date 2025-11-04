import { BookingDetails } from './../../models/BookingDetails';
import { VenueService } from './../../services/venue-service';
import { AuthService } from './../../services/auth-service';
import { Component, computed, effect, inject, input, signal } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { ButtonModule } from 'primeng/button';
import { PopoverModule } from 'primeng/popover';
import { ChatService } from '../../services/chat-service';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';

const defaultChatOwnerName: string = 'Support Agent';

@Component({
  selector: 'app-chat-widget',
  imports: [PanelModule, FormsModule, ButtonModule, PopoverModule, CommonModule, InputTextModule],
  templateUrl: './chat-widget.html',
  styleUrl: './chat-widget.scss',
})
export class ChatWidget {
  booking = input.required<BookingDetails | null>();

  isOpen = signal(false);
  messageText = signal('');

  private chatService = inject(ChatService);
  private authService = inject(AuthService);
  private venueService = inject(VenueService);

  targetUserIds = signal<string[]>([]);
  chatOwnerName = signal<string>(defaultChatOwnerName);

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
      const booking = this.booking();
      if (!booking) return;
      await this.chatService.start();

      const adminIds = await this.venueService.getVenueAdminIds(booking.venue.id);
      this.targetUserIds.set(adminIds);

      const claim = this.chatService.chatClaimed();
      const currentUser = this.currentUserId();

      if (claim && claim.userId === currentUser) {
        this.targetUserIds.set([claim.ownerId]);
        this.chatOwnerName.set(claim.ownerName || defaultChatOwnerName);
        await this.chatService.loadChatHistory(claim.userId, claim.ownerId);
      }
    });

    effect(async () => {
      const claim = this.chatService.chatClaimed();
      const currentUser = this.currentUserId();

      if (claim && claim.userId === currentUser) {
        this.targetUserIds.set([claim.ownerId]);
        this.chatOwnerName.set(claim.ownerName || defaultChatOwnerName);
        await this.chatService.loadChatHistory(claim.userId, claim.ownerId);
      }
    });
  }

  toggleChat() {
    this.isOpen.set(!this.isOpen());
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
