import { Component, inject, signal } from '@angular/core';
import { VenueService } from '../../services/venue-service';
import { Venue } from '../../models/Venue';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-venues-component',
  imports: [CardModule, ButtonModule, CommonModule],
  templateUrl: './venues-component.html',
  styleUrl: './venues-component.scss',
})
export class VenuesComponent {
  private venueService = inject(VenueService);
  private router = inject(Router);

  venues = signal<Venue[]>([]);

  ngOnInit() {
    this.loadVenues();
  }

  async loadVenues() {
    const result = await this.venueService.getVenues();
    this.venues.set(result);
  }

  protected goToVenue(venue: Venue) {
    this.router.navigate(['/venues', venue.slug]);
  }

  getBackgroundImage(url: string) {
    return url ? `url(${url})` : 'none';
  }
}
