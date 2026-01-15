import { inject, Injectable } from '@angular/core';
import { RestApiService } from './rest-api-service';
import { StatisticsDto } from '../models/Statistics';

@Injectable({
  providedIn: 'root',
})
export class StatisticsService {
  private api = inject(RestApiService);

  getStatistics(month?: number, year?: number): Promise<StatisticsDto> {
    const params: any = {};

    if (month) {
      params.month = month;
    }

    if (year) {
      params.year = year;
    }

    return this.api.get<StatisticsDto>('/api/statistics', { params });
  }
}
