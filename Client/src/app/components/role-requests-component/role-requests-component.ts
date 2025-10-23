import { RoleRequest } from '../../models/RoleRequest';
import { RoleRequestService } from '../../services/role-request-service';
import { Component, inject, signal } from '@angular/core';
import { RoleRequestCard } from './role-request-card/role-request-card';
import { RequestStatus } from '../enums/RequestStatus';

@Component({
  selector: 'app-role-requests-component',
  imports: [RoleRequestCard],
  templateUrl: './role-requests-component.html',
  styleUrl: './role-requests-component.scss',
})
export class RoleRequestsComponent {
  private roleRequestService = inject(RoleRequestService);
  roleRequests = signal<RoleRequest[]>([]);

  async ngOnInit() {
    const result = await this.roleRequestService.getRoleRequests();
    this.roleRequests.set(result);
  }

  protected onRequestRejected(requestId: number) {
    this.roleRequests.set(
      this.roleRequests().map((r) =>
        r.id === requestId ? { ...r, status: RequestStatus.Rejected } : r
      )
    );
  }

  protected onRequestApproved(requestId: number) {
    this.roleRequests.set(
      this.roleRequests().map((r) =>
        r.id === requestId ? { ...r, status: RequestStatus.Approved } : r
      )
    );
  }
}
