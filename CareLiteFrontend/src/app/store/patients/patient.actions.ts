import { Patient } from "../../models/patient";

export class LoadPatients {
  static readonly type = '[Patient] Load';
}
export class AddPatient {
  static readonly type = '[Patient] Add';
  constructor(public payload: Patient) {}
}
export class UpdatePatient {
  static readonly type = '[Patient] Update';
  constructor(public payload: Patient) {}
}
export class DeletePatient {
  static readonly type = '[Patient] Delete';
  constructor(public id: number) {}
}