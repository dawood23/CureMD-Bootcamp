import { Component, inject, OnInit } from '@angular/core';
import { ApiService } from '../../../services/api/api.service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Bill } from '../../../models/bill';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-generate-bill',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './generate-bill.component.html',
  styleUrl: './generate-bill.component.scss'
})
export class GenerateBillComponent implements OnInit{
  billService=inject(ApiService)
  router=inject(ActivatedRoute)
  billData:any
  id:any

  ngOnInit(): void {
    this.router.queryParamMap.subscribe(params=>{
      this.id =params.get("appID")
    })

    this.billService.generateBill(this.id).subscribe({
      next:data=>{this.billData=data as Bill
      },
      error:err=>console.log(err)
    })  
  }



printBill() {
  window.print();
}
}
