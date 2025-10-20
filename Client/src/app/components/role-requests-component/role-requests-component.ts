import { RoleRequest } from '../../models/RoleRequest';
import { RoleRequestService } from '../../services/role-request-service';
import { Component, inject, signal } from '@angular/core';
import { RoleRequestCard } from './role-request-card/role-request-card';

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
    console.log(result);
    this.roleRequests.set(result);
  }

  protected onRequestCancelled(requestId: number) {
    this.roleRequests.set(this.roleRequests().filter((r) => r.venueId !== requestId));
  }
}
