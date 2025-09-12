export interface PaymentRequest {
  billID: number;
  amount: number;
  method: string;
  recordedBy: number;
}
