import { RoleRequest } from '../../models/RoleRequest';
import { RoleRequestService } from '../../services/role-request-service';
import { Component, computed, inject, signal } from '@angular/core';
import { RoleRequestCard } from './role-request-card/role-request-card';
import { RequestStatus } from '../enums/RequestStatus';
import { exportRoleRequestsToExcel } from '../../utils/excel-export.util';
import { exportRoleRequestsToPdf } from '../../utils/pdf-export.util';
import { FormsModule } from '@angular/forms';
import { SelectModule } from 'primeng/select';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { SearchBar } from '../shared/search-bar/search-bar';

@Component({
  selector: 'app-role-requests-component',
  imports: [RoleRequestCard, FormsModule, SelectModule, ButtonModule, CardModule, SearchBar],
  templateUrl: './role-requests-component.html',
  styleUrl: './role-requests-component.scss',
})
export class RoleRequestsComponent {
  private roleRequestService = inject(RoleRequestService);

  roleRequests = signal<RoleRequest[]>([]);
  searchTerm = signal('');
  statusFilter = signal<RequestStatus | undefined>(undefined);

  statusOptions = [
    { label: 'All', value: undefined },
    { label: 'Pending', value: RequestStatus.Pending },
    { label: 'Approved', value: RequestStatus.Approved },
    { label: 'Rejected', value: RequestStatus.Rejected },
  ];

  filteredRoleRequests = computed(() => {
    const term = this.searchTerm().toLowerCase();
    const status = this.statusFilter();

    return this.roleRequests().filter((r) => {
      const matchesSearch =
        !term ||
        r.user?.userName.toLowerCase().includes(term) ||
        r.roleName.toLowerCase().includes(term);

      const matchesStatus = !status || r.status === status;

      return matchesSearch && matchesStatus;
    });
  });

  async ngOnInit() {
    const result = await this.roleRequestService.getRoleRequests();
    this.roleRequests.set(result);
  }

  onSearch(term: string) {
    this.searchTerm.set(term);
  }

  clearFilters() {
    this.searchTerm.set('');
    this.statusFilter.set(undefined);
  }

  downloadExcel() {
    if (!this.filteredRoleRequests().length) return;
    exportRoleRequestsToExcel(this.filteredRoleRequests(), 'RoleRequests');
  }

  downloadPdf() {
    if (!this.filteredRoleRequests().length) return;
    exportRoleRequestsToPdf(this.filteredRoleRequests(), 'Role Requests');
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
