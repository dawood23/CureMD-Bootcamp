import { Patient } from "../../models/patient";

export class LoadPatients {
  static readonly type = '[Patient] Load Patients';
}

export class LoadPatientsSuccess {
  static readonly type = '[Patient] Load Patients Success';
  constructor(public payload: Patient[]) {}
}

export class LoadPatientsFailure {
  static readonly type = '[Patient] Load Patients Failure';
  constructor(public payload: string) {}
}

export class CreatePatient {
  static readonly type = '[Patient] Create Patient';
  constructor(public payload: Patient) {}
}

export class CreatePatientSuccess {
  static readonly type = '[Patient] Create Patient Success';
  constructor(public payload: Patient) {}
}

export class CreatePatientFailure {
  static readonly type = '[Patient] Create Patient Failure';
  constructor(public payload: string) {}
}

export class UpdatePatient {
  static readonly type = '[Patient] Update Patient';
  constructor(public payload: Patient) {}
}

export class UpdatePatientSuccess {
  static readonly type = '[Patient] Update Patient Success';
  constructor(public payload: Patient) {}
}

export class UpdatePatientFailure {
  static readonly type = '[Patient] Update Patient Failure';
  constructor(public payload: string) {}
}

export class DeletePatient {
  static readonly type = '[Patient] Delete Patient';
  constructor(public payload: number) {}
}

export class DeletePatientSuccess {
  static readonly type = '[Patient] Delete Patient Success';
  constructor(public payload: number) {}
}

export class DeletePatientFailure {
  static readonly type = '[Patient] Delete Patient Failure';
  constructor(public payload: string) {}
}