import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Bill } from '../../../models/bill';
import { Payment } from '../../../models/payments';
import { ApiService } from '../../../services/api/api.service';
import dayjs from 'dayjs';
import { HighlightPipe } from '../../../pipes/highlight/highlight.pipe';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-finance-dashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HighlightPipe, RouterLink],
  templateUrl: './finance-dashboard.component.html',
  styleUrls: ['./finance-dashboard.component.scss']
})
export class FinanceDashboardComponent implements OnInit {
  private fb = inject(FormBuilder);
  private billingService = inject(ApiService);

  bills: Bill[] = [];
  payments: Payment[] = [];
  filteredBills: Bill[] = [];
  dailyCollections: Payment[] = [];

  filterForm = this.fb.group({
    patientName: [''],
    doctorName: [''],
    status: ['All'],
    collectionDate: [new Date().toISOString().slice(0, 10)]
  });

  ngOnInit(): void {
    this.loadBills();
    this.loadPayments();

    this.filterForm.valueChanges.subscribe(() => {
      this.applyFilters();
      this.calculateDailyCollections();
    });
  }

  private loadBills() {
    this.billingService.getBills().subscribe({
      next: (res) => {
        this.bills = res;
        this.applyFilters();
      }
    });
  }

  private loadPayments() {
    this.billingService.getPayment().subscribe({
      next: (res) => {
        this.payments = Array.isArray(res) ? res : [res];
        this.calculateDailyCollections();
      }
    });
  }

  applyFilters() {
    const { patientName, doctorName, status } = this.filterForm.value;

    this.filteredBills = this.bills.filter((b) => {
      const matchesPatient =
        !patientName || b.patientName.toLowerCase().includes(patientName.toLowerCase());
      const matchesDoctor =
        !doctorName || b.doctorName.toLowerCase().includes(doctorName.toLowerCase());
      const matchesStatus = status === 'All' || b.status === status;

      return matchesPatient && matchesDoctor && matchesStatus;
    });
  }

  calculateDailyCollections() {
    const date = this.filterForm.value.collectionDate;
    if (!date) return;

    this.dailyCollections = this.payments.filter((p) =>
      dayjs(p.paidAt).isSame(dayjs(date), 'day')
    );
  }

  getDailyTotal(): number {
    return this.dailyCollections.reduce((acc, p) => acc + p.amount, 0);
  }

  exportCSV(data: any[], filename: string) {
    const csv = this.convertToCSV(data);
    const blob = new Blob([csv], { type: 'text/csv' });
    const link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    link.download = filename;
    link.click();
  }

  private convertToCSV(objArray: any[]): string {
    if (!objArray.length) return '';
    const header = Object.keys(objArray[0]).join(',');
    const rows = objArray.map((row) =>
      Object.values(row)
        .map((v) => `"${v}"`)
        .join(',')
    );
    return [header, ...rows].join('\r\n');
  }
}
