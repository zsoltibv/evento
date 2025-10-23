import { RequestStatus } from '../components/enums/RequestStatus';
import { User } from './User';
import { Venue } from './Venue';

export interface RoleRequest {
  id: number;
  roleName: string;
  status: RequestStatus;
  requestDate: Date;
  venueId?: number;
  venue?: Venue;
  user?: User;
}
