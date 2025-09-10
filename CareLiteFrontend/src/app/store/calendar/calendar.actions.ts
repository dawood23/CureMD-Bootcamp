export class LoadWeeklyCalendar {
  static readonly type = '[Calendar] Load Weekly Calendar';
  constructor(public doctorId: number, public weekStartDate: string) {}
}

export class SetSelectedDoctor {
  static readonly type = '[Calendar] Set Selected Doctor';
  constructor(public doctor: any) {}
}

export class SetSelectedWeek {
  static readonly type = '[Calendar] Set Selected Week';
  constructor(public weekStartDate: Date) {}
}

export class SetSlotDuration {
  static readonly type = '[Calendar] Set Slot Duration';
  constructor(public duration: number) {}
}