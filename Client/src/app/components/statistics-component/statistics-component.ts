import { AuthService } from './../../services/auth-service';
import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CardModule } from 'primeng/card';
import { ChartData, ChartOptions } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { SelectModule } from 'primeng/select';
import { StatisticsService } from '../../services/statistics-service';
import { UserService } from '../../services/user-service';
import { StatisticsDto } from '../../models/Statistics';

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
  private authService = inject(AuthService);

  isAdmin = computed(() => this.authService.isAdmin());
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
  years: number[] = Array.from({ length: 5 }, (_, i) => new Date().getFullYear() - i);
  selectedYear = signal<number>(new Date().getFullYear());

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

  adminVenueBookingsChart = signal<ChartData<'bar'>>({
    labels: [],
    datasets: [
      {
        label: 'Bookings per venue',
        data: [],
        backgroundColor: '#3b82f6',
      },
    ],
  });

  adminVenueRevenueChart = signal<ChartData<'bar'>>({
    labels: [],
    datasets: [
      {
        label: 'Revenue per venue',
        data: [],
        backgroundColor: '#22c55e',
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
    const stats: StatisticsDto = await this.statisticsService.getStatistics(
      this.selectedMonth(),
      this.selectedYear()
    );

    this.bookingsCount.set(stats.bookingsCount);
    this.venuesCount.set(stats.venuesCount);
    this.revenueData.set(stats.weeklyRevenue);

    this.bookingChartData.set({
      labels: ['Bookings'],
      datasets: [
        {
          label: 'Bookings',
          data: [stats.bookingsCount],
          backgroundColor: '#10b981',
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

    const weekLabels = this.getWeekLabels(this.selectedYear(), this.selectedMonth());
    this.revenueChartData.set({
      labels: weekLabels,
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

    // admin graphs
    if (this.isAdmin() && stats.venueStatistics?.length) {
      const labels = stats.venueStatistics.map((v) => v.venueName);

      this.adminVenueBookingsChart.set({
        labels,
        datasets: [
          {
            label: 'Bookings per venue',
            data: stats.venueStatistics.map((v) => v.bookingsCount),
            backgroundColor: '#3b82f6',
          },
        ],
      });

      this.adminVenueRevenueChart.set({
        labels,
        datasets: [
          {
            label: 'Revenue per venue',
            data: stats.venueStatistics.map((v) => v.totalRevenue),
            backgroundColor: '#22c55e',
          },
        ],
      });
    }
  }

  private getWeekLabels(year: number, month: number): string[] {
    const labels: string[] = [];

    const firstDayOfMonth = new Date(year, month - 1, 1);
    const lastDayOfMonth = new Date(year, month, 0);

    let current = new Date(firstDayOfMonth);

    while (current <= lastDayOfMonth) {
      const weekStart = new Date(current);
      const weekEnd = new Date(current);
      weekEnd.setDate(weekEnd.getDate() + 6);

      if (weekEnd > lastDayOfMonth) {
        weekEnd.setTime(lastDayOfMonth.getTime());
      }

      labels.push(
        `${weekStart.toLocaleDateString(undefined, { month: 'short', day: 'numeric' })}–` +
          `${weekEnd.toLocaleDateString(undefined, { month: 'short', day: 'numeric' })}`
      );

      current.setDate(current.getDate() + 7);
    }

    return labels;
  }
}
