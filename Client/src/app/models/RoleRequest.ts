import { User } from './User';
import { Venue } from './Venue';

export interface RoleRequest {
  id: number;
  roleName: string;
  status: string;
  requestDate: Date;
  venueId?: number;
  venue?: Venue;
  user?: User;
}
