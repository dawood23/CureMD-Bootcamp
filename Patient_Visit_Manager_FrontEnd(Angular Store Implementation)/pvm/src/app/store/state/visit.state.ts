import { Injectable } from '@angular/core';
import { State, Action, StateContext, Selector } from '@ngxs/store';
import { tap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { ApiService } from '../../services/api/api.service';
import * as VisitActions from '../actions/visit.actions';
import { VisitStateModel } from '../models/visit.state.model';
import { Visit } from '../../models/visit';

@State<VisitStateModel>({
  name: 'visit',
  defaults: {
    visits: [],
    loading: false,
    error: null
  }
})
@Injectable()
export class VisitState {
  constructor(private apiService: ApiService) {}

  @Selector()
  static visits(state: VisitStateModel): Visit[] {
    return state.visits;
  }

  @Selector()
  static loading(state: VisitStateModel): boolean {
    return state.loading;
  }

  @Selector()
  static error(state: VisitStateModel): string | null {
    return state.error;
  }

  @Selector()
  static visitById(state: VisitStateModel) {
    return (id: number) => state.visits.find(v => v.visitID === id);
  }

  @Selector()
  static totalVisits(state: VisitStateModel): number {
    return state.visits.length;
  }

  @Selector()
  static visitsByPatient(state: VisitStateModel) {
    return (patientId: number) => state.visits.filter(v => v.patientID === patientId);
  }

  @Action(VisitActions.LoadVisits)
  loadVisits(ctx: StateContext<VisitStateModel>) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.getVisits().pipe(
      tap(visits => {
        ctx.dispatch(new VisitActions.LoadVisitsSuccess(visits));
      }),
      catchError(error => {
        ctx.dispatch(new VisitActions.LoadVisitsFailure(error.message || 'Failed to load visits'));
        return of(null);
      })
    );
  }

  @Action(VisitActions.LoadVisitsSuccess)
  loadVisitsSuccess(ctx: StateContext<VisitStateModel>, action: VisitActions.LoadVisitsSuccess) {
    ctx.patchState({
      visits: action.payload,
      loading: false,
      error: null
    });
  }

  @Action(VisitActions.LoadVisitsFailure)
  loadVisitsFailure(ctx: StateContext<VisitStateModel>, action: VisitActions.LoadVisitsFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(VisitActions.CreateVisit)
  createVisit(ctx: StateContext<VisitStateModel>, action: VisitActions.CreateVisit) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.createVisit(action.payload).pipe(
      tap(visit => {
        ctx.dispatch(new VisitActions.CreateVisitSuccess(visit));
        ctx.dispatch(new VisitActions.LoadVisits())
      }),
      catchError(error => {
        ctx.dispatch(new VisitActions.CreateVisitFailure(error.message || 'Failed to create visit'));
        return of(null);
      })
    );
  }

  @Action(VisitActions.CreateVisitSuccess)
  createVisitSuccess(ctx: StateContext<VisitStateModel>, action: VisitActions.CreateVisitSuccess) {
    const state = ctx.getState();
    ctx.patchState({
      visits: [...state.visits, action.payload],
      loading: false,
      error: null
    });
  }

  @Action(VisitActions.CreateVisitFailure)
  createVisitFailure(ctx: StateContext<VisitStateModel>, action: VisitActions.CreateVisitFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(VisitActions.UpdateVisit)
  updateVisit(ctx: StateContext<VisitStateModel>, action: VisitActions.UpdateVisit) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.updateVisit(action.payload).pipe(
      tap(visit => {
        ctx.dispatch(new VisitActions.UpdateVisitSuccess(visit));
        ctx.dispatch(new VisitActions.LoadVisits())
      }),
      catchError(error => {
        ctx.dispatch(new VisitActions.UpdateVisitFailure(error.message || 'Failed to update visit'));
        return of(null);
      })
    );
  }

  @Action(VisitActions.UpdateVisitSuccess)
  updateVisitSuccess(ctx: StateContext<VisitStateModel>, action: VisitActions.UpdateVisitSuccess) {
    const state = ctx.getState();
    const updatedVisits = state.visits.map(v => 
      v.visitID === action.payload.visitID ? action.payload : v
    );
    
    ctx.patchState({
      visits: updatedVisits,
      loading: false,
      error: null
    });
  }

  @Action(VisitActions.UpdateVisitFailure)
  updateVisitFailure(ctx: StateContext<VisitStateModel>, action: VisitActions.UpdateVisitFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(VisitActions.DeleteVisit)
  deleteVisit(ctx: StateContext<VisitStateModel>, action: VisitActions.DeleteVisit) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.deleteVisit(action.payload).pipe(
      tap(() => {
        ctx.dispatch(new VisitActions.DeleteVisitSuccess(action.payload));
      }),
      catchError(error => {
        ctx.dispatch(new VisitActions.DeleteVisitFailure(error.message || 'Failed to delete visit'));
        return of(null);
      })
    );
  }

  @Action(VisitActions.DeleteVisitSuccess)
  deleteVisitSuccess(ctx: StateContext<VisitStateModel>, action: VisitActions.DeleteVisitSuccess) {
    const state = ctx.getState();
    const filteredVisits = state.visits.filter(v => v.visitID !== action.payload);
    
    ctx.patchState({
      visits: filteredVisits,
      loading: false,
      error: null
    });
  }

  @Action(VisitActions.DeleteVisitFailure)
  deleteVisitFailure(ctx: StateContext<VisitStateModel>, action: VisitActions.DeleteVisitFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }
}