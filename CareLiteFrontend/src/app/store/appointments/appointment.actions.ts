export class LoadAppointments {
  static readonly type = '[Appointment] Load';
}
export class AddAppointment {
  static readonly type = '[Appointment] Add';
  constructor(public payload: any) {}
}
export class UpdateAppointment {
  static readonly type = '[Appointment] Update';
  constructor(public payload: any) {}
}
export class DeleteAppointment {
  static readonly type = '[Appointment] Delete';
  constructor(public id: number) {}
}