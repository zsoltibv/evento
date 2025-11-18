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
