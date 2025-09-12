import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { inject } from '@angular/core';
import { ApiService } from '../../../services/api/api.service';
import { FormBuilder, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';
import { PaymentRequest } from '../../../models/payment-request';

@Component({
  selector: 'app-add-payement',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule,FormsModule],
  templateUrl: './add-payement.component.html',
  styleUrl: './add-payement.component.scss'
})
export class AddPayementComponent {
  private fb = inject(FormBuilder);
  private paymentService = inject(ApiService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private authService = inject(AuthService);

  billID: number = 0;
  userId: number = 0;

  errorMessage: string | null = null;
  successMessage: string | null = null;

  cardNumber: string = '';
  expiry: string = '';
  cvc: string = '';

  form = this.fb.group({
    amount: [0, [Validators.required, Validators.min(1)]],
    method: ['Cash', Validators.required]
  });

  ngOnInit(): void {
    this.billID = Number(this.route.snapshot.queryParamMap.get('billID'));
    this.userId = this.authService.getUserId() ?? 1;
  }

  private simulateStripePayment(): Promise<{ id: string }> {
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        if (this.cardNumber.replace(/\s+/g, '') === '4000000000000002') {
          reject(new Error('Card declined (mock)'));
        } else {
          resolve({ id: 'pm_mock_' + Math.random().toString(36).substring(2, 10) });
        }
      }, 1500);
    });
  }

  async submit() {
    if (this.form.invalid) {
      this.errorMessage = 'Please enter valid payment details.';
      return;
    }

    if (this.form.value.method === 'Card') {
      try {
        const result = await this.simulateStripePayment();
        console.log('Mock Payment success:', result);

        this.recordPayment('Card');
      } catch (err: any) {
        this.errorMessage = err.message || 'Card payment failed.';
      }
    } else {
      this.recordPayment('Cash');
    }
  }

  private recordPayment(method: string) {
    const request: PaymentRequest = {
      billID: this.billID,
      amount: this.form.value.amount!,
      method,
      recordedBy: this.userId
    };

    this.paymentService.recordPayment(request).subscribe({
      next: (res) => {
        this.successMessage = 'Payment recorded successfully!';
        this.errorMessage = null;
        setTimeout(() => this.router.navigate(['/bill-list']), 1500);
      },
      error: (err) => {
        this.successMessage = null;
        this.errorMessage = err.error?.message || 'Failed to record payment.';
      }
    });
  }

  cancel() {
    this.router.navigate(['/bill-list']);
  }
}
