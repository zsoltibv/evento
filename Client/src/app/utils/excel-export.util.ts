import * as XLSX from 'xlsx';
import { saveAs } from 'file-saver';
import { BookingWithVenueName } from '../models/BookingWithVenueName';

export function exportBookingsToExcel(bookings: BookingWithVenueName[], fileName: string) {
  const worksheetData = bookings.map((b) => ({
    ID: b.id,
    Venue: b.venueName,
    Status: b.status,
    'Start Date': new Date(b.startDate).toLocaleDateString(),
    'End Date': new Date(b.endDate).toLocaleDateString(),
    'Booking Date': new Date(b.bookingDate).toLocaleDateString(),
    Paid: b.isPaid ? 'Yes' : 'No',
    'Amount Paid': b.amountPaid,
  }));

  const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(worksheetData);
  const workbook: XLSX.WorkBook = {
    Sheets: { Bookings: worksheet },
    SheetNames: ['Bookings'],
  };

  const excelBuffer = XLSX.write(workbook, {
    bookType: 'xlsx',
    type: 'array',
  });

  const blob = new Blob([excelBuffer], {
    type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8',
  });

  saveAs(blob, `${fileName}.xlsx`);
}
