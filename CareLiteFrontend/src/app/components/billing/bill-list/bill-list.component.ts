import { Component, ViewChild, AfterViewInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Bill } from '../../../models/bill';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime } from 'rxjs';
import { Router } from '@angular/router';
import { HighlightPipe } from '../../../pipes/highlight/highlight.pipe';

import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

import { ApiService } from '../../../services/api/api.service';

@Component({
  selector: 'app-bill-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    HighlightPipe,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
  templateUrl: './bill-list.component.html',
  styleUrls: ['./bill-list.component.scss']
})
export class BillListComponent implements AfterViewInit {
  billService = inject(ApiService);
  router = inject(Router);

  displayedColumns: string[] = [
    'billID',
    'appointmentID',
    'patientID',
    'patientName',
    'doctorName',
    'status',
    'totalAmount',
    'pendingAmount',
    'generatedAt',
    'actions'
  ];
  dataSource = new MatTableDataSource<Bill>();

  searchControl = new FormControl('');

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit() {
    this.billService.getBills().subscribe({
      next: (bills) => {
        this.dataSource.data = bills;
      },
      error: () => alert('An error occurred while loading the bills')
    });

    this.searchControl.valueChanges
      .pipe(debounceTime(300))
      .subscribe(query => {
        this.dataSource.filter = query?.trim().toLowerCase() || '';
      });

    this.dataSource.filterPredicate = (data: Bill, filter: string) => {
      const str = filter.toLowerCase();
      return (
        data.patientName.toLowerCase().includes(str) ||
        data.doctorName.toLowerCase().includes(str) ||
        data.status.toLowerCase().includes(str) ||
        data.billID.toString().includes(str) ||
        data.appointmentID.toString().includes(str) ||
        data.patientID.toString().includes(str)
      );
    };
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  redirect() {
    this.router.navigate(['/bill-add']);
  }

  goBack() {
    this.router.navigate(['/dashboard']);
  }

  addpayment(id:number){
    this.router.navigate(['/add-payment'],{
      queryParams:{billID:id}
    })
  }
}
