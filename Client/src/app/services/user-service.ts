import { inject, Injectable } from '@angular/core';
import { RestApiService } from './rest-api-service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private api = inject(RestApiService);

  async getStripeCustomerId(userId: string): Promise<string> {
    return await this.api.get<string>(`/api/users/customer-id/${userId}`);
  }
}
