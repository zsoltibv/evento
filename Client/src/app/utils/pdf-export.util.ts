import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { BookingWithVenueName } from '../models/BookingWithVenueName';
import { RoleRequest } from '../models/RoleRequest';

export function exportBookingsToPdf(bookings: BookingWithVenueName[], title: string): void {
  const doc = new jsPDF('landscape');

  doc.setFontSize(16);
  doc.text(title, 14, 15);

  const tableData = bookings.map((b) => [
    b.id,
    b.venueName,
    b.status,
    b.isPaid ? 'Paid' : 'Unpaid',
    new Date(b.startDate).toLocaleDateString(),
    new Date(b.endDate).toLocaleDateString(),
  ]);

  autoTable(doc, {
    startY: 25,
    head: [['ID', 'Venue', 'Status', 'Payment', 'From Date', 'To Date']],
    body: tableData,
    styles: {
      fontSize: 9,
    },
    headStyles: {
      fillColor: [41, 128, 185],
    },
  });

  doc.save(`${title}.pdf`);
}

export function exportRoleRequestsToPdf(requests: RoleRequest[], title: string): void {
  const doc = new jsPDF('landscape');

  doc.setFontSize(16);
  doc.text(title, 14, 15);

  const tableData = requests.map((r) => [
    r.id,
    r.user?.userName!,
    r.roleName,
    r.status,
    new Date(r.requestDate).toLocaleDateString(),
  ]);

  autoTable(doc, {
    startY: 25,
    head: [['ID', 'Username', 'Requested Role', 'Status', 'Requested At']],
    body: tableData,
    styles: { fontSize: 9 },
    headStyles: { fillColor: [41, 128, 185] },
  });

  doc.save(`${title}.pdf`);
}
