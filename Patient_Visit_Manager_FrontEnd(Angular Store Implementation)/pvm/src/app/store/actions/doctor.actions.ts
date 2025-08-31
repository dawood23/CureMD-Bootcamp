import { Doctor } from "../../models/doctor";

export class LoadDoctors {
  static readonly type = '[Doctor] Load Doctors';
}

export class LoadDoctorsSuccess {
  static readonly type = '[Doctor] Load Doctors Success';
  constructor(public payload: Doctor[]) {}
}

export class LoadDoctorsFailure {
  static readonly type = '[Doctor] Load Doctors Failure';
  constructor(public payload: string) {}
}

export class CreateDoctor {
  static readonly type = '[Doctor] Create Doctor';
  constructor(public payload: Doctor) {}
}

export class CreateDoctorSuccess {
  static readonly type = '[Doctor] Create Doctor Success';
  constructor(public payload: Doctor) {}
}

export class CreateDoctorFailure {
  static readonly type = '[Doctor] Create Doctor Failure';
  constructor(public payload: string) {}
}

export class UpdateDoctor {
  static readonly type = '[Doctor] Update Doctor';
  constructor(public payload: Doctor) {}
}

export class UpdateDoctorSuccess {
  static readonly type = '[Doctor] Update Doctor Success';
  constructor(public payload: Doctor) {}
}

export class UpdateDoctorFailure {
  static readonly type = '[Doctor] Update Doctor Failure';
  constructor(public payload: string) {}
}

export class DeleteDoctor {
  static readonly type = '[Doctor] Delete Doctor';
  constructor(public payload: number) {}
}

export class DeleteDoctorSuccess {
  static readonly type = '[Doctor] Delete Doctor Success';
  constructor(public payload: number) {}
}

export class DeleteDoctorFailure {
  static readonly type = '[Doctor] Delete Doctor Failure';
  constructor(public payload: string) {}
}