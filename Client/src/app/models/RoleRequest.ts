import { Venue } from './Venue';

export interface RoleRequest {
  roleName: string;
  status: string;
  requestDate: Date;
  venueId?: number;
  venue?: Venue;
}
