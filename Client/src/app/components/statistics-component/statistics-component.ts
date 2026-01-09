import { CommonModule } from '@angular/common';
import { Component, computed, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CardModule } from 'primeng/card';
import { ChartData, ChartOptions } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { SelectModule } from 'primeng/select';

interface MonthOption {
  name: string;
  value: number;
}

@Component({
  selector: 'app-statistics-component',
  imports: [CommonModule, CardModule, FormsModule, SelectModule, BaseChartDirective],
  templateUrl: './statistics-component.html',
  styleUrl: './statistics-component.scss',
})
export class StatisticsComponent {
  months = [
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

  selectedMonth = signal(new Date().getMonth() + 1);

  usersCount = signal(120);
  bookingsCount = signal(45);
  venuesCount = signal(10);
  revenueData = signal([5000, 7000, 6000, 8000, 5500, 7500, 9000]);

  public totalRevenue = computed(() => this.revenueData().reduce((a, b) => a + b, 0));

  userChartData: ChartData<'bar'> = {
    labels: ['Users'],
    datasets: [{ label: 'Users', data: [this.usersCount()], backgroundColor: '#3b82f6' }],
  };

  bookingChartData: ChartData<'bar'> = {
    labels: ['Bookings'],
    datasets: [{ label: 'Bookings', data: [this.bookingsCount()], backgroundColor: '#10b981' }],
  };

  venueChartData: ChartData<'bar'> = {
    labels: ['Venues'],
    datasets: [{ label: 'Venues', data: [this.venuesCount()], backgroundColor: '#f59e0b' }],
  };

  revenueChartData: ChartData<'line'> = {
    labels: ['Week 1', 'Week 2', 'Week 3', 'Week 4'],
    datasets: [
      {
        label: 'Revenue',
        data: this.revenueData(),
        borderColor: '#ef4444',
        backgroundColor: 'rgba(239,68,68,0.2)',
        fill: true,
      },
    ],
  };

  chartOptions: ChartOptions<'bar' | 'line'> = {
    responsive: true,
    plugins: { legend: { display: true } },
  };

  loadBookings() {
    console.log('Selected month:', this.selectedMonth());
    //TODO: to call api filter endpoint
  }
}
