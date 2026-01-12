import { inject, Injectable } from '@angular/core';
import { RestApiService } from './rest-api-service';
import { StatisticsDto } from '../models/Statistics';

@Injectable({
  providedIn: 'root',
})
export class StatisticsService {
  private api = inject(RestApiService);

  getStatistics(month?: number): Promise<StatisticsDto> {
    const params = month ? { params: { month } } : {};
    return this.api.get<StatisticsDto>('/api/statistics', params);
  }
}
