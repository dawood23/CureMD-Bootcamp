import { Injectable } from '@angular/core';
import { State, Action, StateContext, Selector } from '@ngxs/store';
import { tap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { ApiService } from '../../services/api/api.service';
import * as PatientActions from '../actions/patient.actions';
import { PatientStateModel } from '../models/patient.state.model';
import { Patient } from '../../models/patient';

@State<PatientStateModel>({
  name: 'patient',
  defaults: {
    patients: [],
    loading: false,
    error: null
  }
})
@Injectable()
export class PatientState {
  constructor(private apiService: ApiService) {}

  @Selector()
  static patients(state: PatientStateModel): Patient[] {
    return state.patients;
  }

  @Selector()
  static loading(state: PatientStateModel): boolean {
    return state.loading;
  }

  @Selector()
  static error(state: PatientStateModel): string | null {
    return state.error;
  }

  @Selector()
  static patientById(state: PatientStateModel) {
    return (id: number) => state.patients.find(p => p.patientID === id);
  }

  @Selector()
  static totalPatients(state: PatientStateModel): number {
    return state.patients.length;
  }

  @Action(PatientActions.LoadPatients)
  loadPatients(ctx: StateContext<PatientStateModel>) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.getPatients().pipe(
      tap(patients => {
        ctx.dispatch(new PatientActions.LoadPatientsSuccess(patients));
      }),
      catchError(error => {
        ctx.dispatch(new PatientActions.LoadPatientsFailure(error.message || 'Failed to load patients'));
        return of(null);
      })
    );
  }

  @Action(PatientActions.LoadPatientsSuccess)
  loadPatientsSuccess(ctx: StateContext<PatientStateModel>, action: PatientActions.LoadPatientsSuccess) {
    ctx.patchState({
      patients: action.payload,
      loading: false,
      error: null
    });
  }

  @Action(PatientActions.LoadPatientsFailure)
  loadPatientsFailure(ctx: StateContext<PatientStateModel>, action: PatientActions.LoadPatientsFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(PatientActions.CreatePatient)
  createPatient(ctx: StateContext<PatientStateModel>, action: PatientActions.CreatePatient) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.createPatient(action.payload).pipe(
      tap(patient => {
        ctx.dispatch(new PatientActions.CreatePatientSuccess(patient));
        ctx.dispatch(new PatientActions.LoadPatients());
      }),
      catchError(error => {
        ctx.dispatch(new PatientActions.CreatePatientFailure(error.message || 'Failed to create patient'));
        return of(null);
      })
    );
  }

  @Action(PatientActions.CreatePatientSuccess)
  createPatientSuccess(ctx: StateContext<PatientStateModel>, action: PatientActions.CreatePatientSuccess) {
    const state = ctx.getState();
    ctx.patchState({
      patients: [...state.patients, action.payload],
      loading: false,
      error: null
    });
  }

  @Action(PatientActions.CreatePatientFailure)
  createPatientFailure(ctx: StateContext<PatientStateModel>, action: PatientActions.CreatePatientFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(PatientActions.UpdatePatient)
  updatePatient(ctx: StateContext<PatientStateModel>, action: PatientActions.UpdatePatient) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.updatePatient(action.payload).pipe(
      tap(patient => {
        ctx.dispatch(new PatientActions.UpdatePatientSuccess(patient));
        ctx.dispatch(new PatientActions.LoadPatients());
      }),
      catchError(error => {
        ctx.dispatch(new PatientActions.UpdatePatientFailure(error.message || 'Failed to update patient'));
        return of(null);
      })
    );
  }

  @Action(PatientActions.UpdatePatientSuccess)
  updatePatientSuccess(ctx: StateContext<PatientStateModel>, action: PatientActions.UpdatePatientSuccess) {
    const state = ctx.getState();
    const updatedPatients = state.patients.map(p => 
      p.patientID === action.payload.patientID ? action.payload : p
    );
    
    ctx.patchState({
      patients: updatedPatients,
      loading: false,
      error: null
    });
  }

  @Action(PatientActions.UpdatePatientFailure)
  updatePatientFailure(ctx: StateContext<PatientStateModel>, action: PatientActions.UpdatePatientFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(PatientActions.DeletePatient)
  deletePatient(ctx: StateContext<PatientStateModel>, action: PatientActions.DeletePatient) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.deletePatient(action.payload).pipe(
      tap(() => {
        ctx.dispatch(new PatientActions.DeletePatientSuccess(action.payload));
      }),
      catchError(error => {
        ctx.dispatch(new PatientActions.DeletePatientFailure(error.message || 'Failed to delete patient'));
        return of(null);
      })
    );
  }

  @Action(PatientActions.DeletePatientSuccess)
  deletePatientSuccess(ctx: StateContext<PatientStateModel>, action: PatientActions.DeletePatientSuccess) {
    const state = ctx.getState();
    const filteredPatients = state.patients.filter(p => p.patientID !== action.payload);
    
    ctx.patchState({
      patients: filteredPatients,
      loading: false,
      error: null
    });
  }

  @Action(PatientActions.DeletePatientFailure)
  deletePatientFailure(ctx: StateContext<PatientStateModel>, action: PatientActions.DeletePatientFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }
}
