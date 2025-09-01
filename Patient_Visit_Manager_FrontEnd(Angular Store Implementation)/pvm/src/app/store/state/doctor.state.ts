import { Injectable } from '@angular/core';
import { State, Action, StateContext, Selector } from '@ngxs/store';
import { tap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { ApiService } from '../../services/api/api.service';
import * as DoctorActions from '../actions/doctor.actions';
import { DoctorStateModel } from '../models/doctor.state.model';
import { Doctor } from '../../models/doctor';

@State<DoctorStateModel>({
  name: 'doctor',
  defaults: {
    doctors: [],
    loading: false,
    error: null
  }
})
@Injectable()
export class DoctorState {
  constructor(private apiService: ApiService) {}

  @Selector()
  static doctors(state: DoctorStateModel): Doctor[] {
    return state.doctors;
  }

  @Selector()
  static loading(state: DoctorStateModel): boolean {
    return state.loading;
  }

  @Selector()
  static error(state: DoctorStateModel): string | null {
    return state.error;
  }

  @Selector()
  static doctorById(state: DoctorStateModel) {
    return (id: number) => state.doctors.find(d => d.doctorID === id);
  }

  @Selector()
  static totalDoctors(state: DoctorStateModel): number {
    return state.doctors.length;
  }

  @Action(DoctorActions.LoadDoctors)
  loadDoctors(ctx: StateContext<DoctorStateModel>) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.getDoctors().pipe(
      tap(doctors => {
        ctx.dispatch(new DoctorActions.LoadDoctorsSuccess(doctors));
      }),
      catchError(error => {
        ctx.dispatch(new DoctorActions.LoadDoctorsFailure(error.message || 'Failed to load doctors'));
        return of(null);
      })
    );
  }

  @Action(DoctorActions.LoadDoctorsSuccess)
  loadDoctorsSuccess(ctx: StateContext<DoctorStateModel>, action: DoctorActions.LoadDoctorsSuccess) {
    ctx.patchState({
      doctors: action.payload,
      loading: false,
      error: null
    });
  }

  @Action(DoctorActions.LoadDoctorsFailure)
  loadDoctorsFailure(ctx: StateContext<DoctorStateModel>, action: DoctorActions.LoadDoctorsFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(DoctorActions.CreateDoctor)
  createDoctor(ctx: StateContext<DoctorStateModel>, action: DoctorActions.CreateDoctor) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.createDoctor(action.payload).pipe(
      tap(doctor => {
        ctx.dispatch(new DoctorActions.CreateDoctorSuccess(doctor));
        ctx.dispatch(new DoctorActions.LoadDoctors());
      }),
      catchError(error => {
        ctx.dispatch(new DoctorActions.CreateDoctorFailure(error.message || 'Failed to create doctor'));
        return of(null);
      })
    );
  }

  @Action(DoctorActions.CreateDoctorSuccess)
  createDoctorSuccess(ctx: StateContext<DoctorStateModel>, action: DoctorActions.CreateDoctorSuccess) {
    const state = ctx.getState();
    ctx.patchState({
      doctors: [...state.doctors, action.payload],
      loading: false,
      error: null
    });
  }

  @Action(DoctorActions.CreateDoctorFailure)
  createDoctorFailure(ctx: StateContext<DoctorStateModel>, action: DoctorActions.CreateDoctorFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(DoctorActions.UpdateDoctor)
  updateDoctor(ctx: StateContext<DoctorStateModel>, action: DoctorActions.UpdateDoctor) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.updateDoctor(action.payload).pipe(
      tap(doctor => {
        ctx.dispatch(new DoctorActions.UpdateDoctorSuccess(doctor));
        ctx.dispatch(new DoctorActions.LoadDoctors());
      }),
      catchError(error => {
        ctx.dispatch(new DoctorActions.UpdateDoctorFailure(error.message || 'Failed to update doctor'));
        return of(null);
      })
    );
  }

  @Action(DoctorActions.UpdateDoctorSuccess)
  updateDoctorSuccess(ctx: StateContext<DoctorStateModel>, action: DoctorActions.UpdateDoctorSuccess) {
    const state = ctx.getState();
    const updatedDoctors = state.doctors.map(d => 
      d.doctorID === action.payload.doctorID ? action.payload : d
    );
    
    ctx.patchState({
      doctors: updatedDoctors,
      loading: false,
      error: null
    });
  }

  @Action(DoctorActions.UpdateDoctorFailure)
  updateDoctorFailure(ctx: StateContext<DoctorStateModel>, action: DoctorActions.UpdateDoctorFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(DoctorActions.DeleteDoctor)
  deleteDoctor(ctx: StateContext<DoctorStateModel>, action: DoctorActions.DeleteDoctor) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.deleteDoctor(action.payload).pipe(
      tap(() => {
        ctx.dispatch(new DoctorActions.DeleteDoctorSuccess(action.payload));
      }),
      catchError(error => {
        ctx.dispatch(new DoctorActions.DeleteDoctorFailure(error.message || 'Failed to delete doctor'));
        return of(null);
      })
    );
  }

  @Action(DoctorActions.DeleteDoctorSuccess)
  deleteDoctorSuccess(ctx: StateContext<DoctorStateModel>, action: DoctorActions.DeleteDoctorSuccess) {
    const state = ctx.getState();
    const filteredDoctors = state.doctors.filter(d => d.doctorID !== action.payload);
    
    ctx.patchState({
      doctors: filteredDoctors,
      loading: false,
      error: null
    });
  }

  @Action(DoctorActions.DeleteDoctorFailure)
  deleteDoctorFailure(ctx: StateContext<DoctorStateModel>, action: DoctorActions.DeleteDoctorFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }
}