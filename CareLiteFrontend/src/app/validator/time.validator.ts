import { AbstractControl, ValidationErrors } from '@angular/forms';
 
export function appointmentValidator(control: AbstractControl): ValidationErrors | null {
  const value = control.value;
  if (!value) return null;
  const date = new Date(value);
 
  if (isNaN(date.getTime())) return { invalidDateTime: true };
 
  const now = new Date();
  const today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
  const selectedDay = new Date(date.getFullYear(), date.getMonth(), date.getDate());
 
  if (selectedDay < today) {
    return { dateInPast: true };
  }
 
  const hours = date.getHours();
  const minutes = date.getMinutes();
 
  if (minutes % 15 !== 0) {
    return { notQuarterHour: true };
  }
 
  if (hours < 9 || (hours === 18 && minutes > 0) || hours > 18) {
    return { timeOutOfRange: true };
  }
 
  return null;
}