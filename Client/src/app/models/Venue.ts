import { Signal, WritableSignal } from '@angular/core';

export interface Venue {
  id: number;
  name: string;
  description?: string;
  location: string;
  capacity: number;
  imageUrl?: string;
  slug: string;
  pricePerHour: number;
}

export interface VenueUi extends Venue {
  loading: WritableSignal<boolean>;
  editing: WritableSignal<boolean>;
  generatedDescription: WritableSignal<string | null>;
}
