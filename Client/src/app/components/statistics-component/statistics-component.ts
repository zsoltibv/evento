import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CardModule } from 'primeng/card';
import { ChartData, ChartOptions } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { SelectModule } from 'primeng/select';
import { StatisticsService } from '../../services/statistics-service';

interface MonthOption {
  id: number;
  name: string;
}

@Component({
  selector: 'app-statistics-component',
  imports: [CommonModule, CardModule, FormsModule, SelectModule, BaseChartDirective],
  templateUrl: './statistics-component.html',
  styleUrl: './statistics-component.scss',
})
export class StatisticsComponent {
  private statisticsService = inject(StatisticsService);

  months: MonthOption[] = [
    { id: 1, name: 'January' },
    { id: 2, name: 'February' },
    { id: 3, name: 'March' },
    { id: 4, name: 'April' },
    { id: 5, name: 'May' },
    { id: 6, name: 'June' },
    { id: 7, name: 'July' },
    { id: 8, name: 'August' },
    { id: 9, name: 'September' },
    { id: 10, name: 'October' },
    { id: 11, name: 'November' },
    { id: 12, name: 'December' },
  ];

  selectedMonth = signal<number>(new Date().getMonth() + 1);

  bookingsCount = signal(0);
  venuesCount = signal(0);
  revenueData = signal<number[]>([0, 0, 0, 0]);

  totalRevenue = computed(() => this.revenueData().reduce((a, b) => a + b, 0));

  bookingChartData = signal<ChartData<'bar'>>({
    labels: ['Bookings'],
    datasets: [{ label: 'Bookings', data: [0], backgroundColor: '#10b981' }],
  });

  venueChartData = signal<ChartData<'bar'>>({
    labels: ['Venues'],
    datasets: [{ label: 'Venues', data: [0], backgroundColor: '#f59e0b' }],
  });

  revenueChartData = signal<ChartData<'line'>>({
    labels: ['Week 1', 'Week 2', 'Week 3', 'Week 4'],
    datasets: [
      {
        label: 'Revenue',
        data: [],
        borderColor: '#ef4444',
        backgroundColor: 'rgba(239,68,68,0.2)',
        fill: true,
      },
    ],
  });

  chartOptions: ChartOptions<'bar' | 'line'> = {
    responsive: true,
    plugins: { legend: { display: true } },
  };

  ngOnInit(): void {
    this.loadStatistics();
  }

  async loadStatistics() {
    const stats = await this.statisticsService.getStatistics(this.selectedMonth());

    this.bookingsCount.set(stats.bookingsCount);
    this.venuesCount.set(stats.venuesCount);
    this.revenueData.set(stats.weeklyRevenue);

    this.bookingChartData = signal<ChartData<'doughnut'>>({
      labels: ['Bookings'],
      datasets: [
        {
          data: [0],
          backgroundColor: ['#10b981'],
        },
      ],
    });

    this.venueChartData.set({
      labels: ['Venues'],
      datasets: [
        {
          label: 'Venues',
          data: [stats.venuesCount],
          backgroundColor: '#f59e0b',
        },
      ],
    });

    this.revenueChartData.set({
      labels: ['Week 1', 'Week 2', 'Week 3', 'Week 4'],
      datasets: [
        {
          label: 'Spendings',
          data: stats.weeklyRevenue,
          borderColor: '#ef4444',
          backgroundColor: 'rgba(239,68,68,0.2)',
          fill: true,
        },
      ],
    });
  }
}
