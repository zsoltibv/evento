import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'bookingDate',
})
export class BookingDatePipe implements PipeTransform {
  private datePipe = new DatePipe('en-US');

  transform(value: Date | string | null): string {
    if (!value) return '';
    return this.datePipe.transform(value, 'MMMM d, y h:mm a') || '';
  }
}
