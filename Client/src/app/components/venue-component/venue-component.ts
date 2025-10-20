import { AuthService } from './../../services/auth-service';
import { VenueService } from './../../services/venue-service';
import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VenueWithBookings } from '../../models/VenueWithBookings';
import { CardModule } from 'primeng/card';
import { TagModule } from 'primeng/tag';
import { BookingCard } from '../booking-card/booking-card';
import { AddOrEditBookingCard } from '../add-or-edit-booking-card/add-or-edit-booking-card';
import { BookingStatusUpdate } from '../../models/BookingStatusUpdate';
import { BookingWithVenueName } from '../../models/BookingWithVenueName';
import { MenuItem, MessageService } from 'primeng/api';
import { MenuModule } from 'primeng/menu';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-venue-component',
  imports: [CardModule, TagModule, BookingCard, AddOrEditBookingCard, MenuModule, ButtonModule],
  templateUrl: './venue-component.html',
  styleUrl: './venue-component.scss',
})
export class VenueComponent {
  private activatedRoute = inject(ActivatedRoute);
  private venueService = inject(VenueService);
  private authService = inject(AuthService);
  private messageService = inject(MessageService);

  venue = signal<VenueWithBookings>({} as VenueWithBookings);
  slug = signal<string>('');

  menuItems: MenuItem[] = [
    {
      label: 'Request for venue admin role',
      icon: 'pi pi-user-plus',
      command: () => this.requestVenuAdminRole(),
    },
  ];

  ngOnInit() {
    this.activatedRoute.params.subscribe((params) => {
      const slug = params['slug'];
      if (slug) {
        this.slug.set(slug);
        this.loadVenueWithBookings();
      }
    });
  }

  protected readonly userBookings = computed<BookingWithVenueName[]>(() => {
    const userId = this.authService.userId();
    return this.venue()?.bookings?.filter((b) => b.userId === userId) || [];
  });

  async loadVenueWithBookings() {
    const result = await this.venueService.getVenueWithBookingsBySlug(this.slug());
    this.venue.set(result);
  }

  protected onCancel(booking: BookingStatusUpdate) {
    this.loadVenueWithBookings();
  }

  protected async requestVenuAdminRole() {
    await this.venueService.requestVenueAdminRole(this.venue().id);

    this.messageService.add({
      severity: 'success',
      summary: 'Request Sent',
      detail: 'Your request for venue admin role has been sent successfully.',
    });
  }
}
