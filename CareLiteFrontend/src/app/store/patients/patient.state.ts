import { State, Action, StateContext, Selector } from '@ngxs/store';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs';
import { ApiService } from '../../services/api/api.service';
import { Patient } from '../../models/patient';
import { LoadPatients, AddPatient, UpdatePatient, DeletePatient } from './patient.actions';

export interface PatientStateModel {
  patients: Patient[];
}

@State<PatientStateModel>({
  name: 'patients',
  defaults: {
    patients: []
  }
})
@Injectable()
export class PatientState {
  constructor(private api: ApiService) {}

  @Selector()
  static patients(state: PatientStateModel) {
    return state.patients;
  }

  @Action(LoadPatients)
  loadPatients({ patchState }: StateContext<PatientStateModel>) {
    return this.api.getAllPatients().pipe(
      tap(patients => patchState({ patients }))
    );
  }

  @Action(AddPatient)
  addPatient({ dispatch }: StateContext<PatientStateModel>, { payload }: AddPatient) {
    return this.api.createPatient(payload).pipe(
      tap(() => dispatch(new LoadPatients()))
    );
  }

  @Action(UpdatePatient)
  updatePatient({ dispatch }: StateContext<PatientStateModel>, { payload }: UpdatePatient) {
    return this.api.updatePatient(payload).pipe(
      tap(() => dispatch(new LoadPatients()))
    );
  }

  @Action(DeletePatient)
  deletePatient({ dispatch }: StateContext<PatientStateModel>, { id }: DeletePatient) {
    return this.api.deletePatient(id).pipe(
      tap(() => dispatch(new LoadPatients()))
    );
  }
}
