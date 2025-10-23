import { inject, Injectable } from '@angular/core';
import { RestApiService } from './rest-api-service';
import { RoleRequest } from '../models/RoleRequest';

@Injectable({
  providedIn: 'root',
})
export class RoleRequestService {
  private api = inject(RestApiService);

  async getRoleRequests(): Promise<RoleRequest[]> {
    return await this.api.get<RoleRequest[]>(`/api/role-requests`);
  }

  async approveRoleRequest(id: number): Promise<string> {
    const response = await this.api.post<{ token: string }>(`/api/role-requests/${id}/approve`, {});
    return response.token;
  }
}
