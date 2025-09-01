import { Injectable } from '@angular/core';
import { State, Action, StateContext, Selector } from '@ngxs/store';
import { tap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { ApiService } from '../../services/api/api.service';
import * as VisitTypeActions from '../actions/visit-type.actions';
import { VisitType } from '../../models/visittype';
import { VisitTypeStateModel } from '../models/visit-type.state.model';

@State<VisitTypeStateModel>({
  name: 'visitType',
  defaults: {
    visitTypes: [],
    loading: false,
    error: null
  }
})
@Injectable()
export class VisitTypeState {
  constructor(private apiService: ApiService) {}

  @Selector()
  static visitTypes(state: VisitTypeStateModel): VisitType[] {
    return state.visitTypes;
  }

  @Selector()
  static loading(state: VisitTypeStateModel): boolean {
    return state.loading;
  }

  @Selector()
  static error(state: VisitTypeStateModel): string | null {
    return state.error;
  }

  @Selector()
  static visitTypeById(state: VisitTypeStateModel) {
    return (id: number) => state.visitTypes.find(vt => vt.visitTypeID === id);
  }

  @Selector()
  static totalVisitTypes(state: VisitTypeStateModel): number {
    return state.visitTypes.length;
  }

  @Action(VisitTypeActions.LoadVisitTypes)
  loadVisitTypes(ctx: StateContext<VisitTypeStateModel>) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.getVisitTypes().pipe(
      tap(visitTypes => {
        ctx.dispatch(new VisitTypeActions.LoadVisitTypesSuccess(visitTypes));
      }),
      catchError(error => {
        ctx.dispatch(new VisitTypeActions.LoadVisitTypesFailure(error.message || 'Failed to load visit types'));
        return of(null);
      })
    );
  }

  @Action(VisitTypeActions.LoadVisitTypesSuccess)
  loadVisitTypesSuccess(ctx: StateContext<VisitTypeStateModel>, action: VisitTypeActions.LoadVisitTypesSuccess) {
    ctx.patchState({
      visitTypes: action.payload,
      loading: false,
      error: null
    });
  }

  @Action(VisitTypeActions.LoadVisitTypesFailure)
  loadVisitTypesFailure(ctx: StateContext<VisitTypeStateModel>, action: VisitTypeActions.LoadVisitTypesFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(VisitTypeActions.CreateVisitType)
  createVisitType(ctx: StateContext<VisitTypeStateModel>, action: VisitTypeActions.CreateVisitType) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.createVisitType(action.payload).pipe(
      tap(visitType => {
        ctx.dispatch(new VisitTypeActions.CreateVisitTypeSuccess(visitType));
        ctx.dispatch(new VisitTypeActions.LoadVisitTypes())
      }),
      catchError(error => {
        ctx.dispatch(new VisitTypeActions.CreateVisitTypeFailure(error.message || 'Failed to create visit type'));
        return of(null);
      })
    );
  }

  @Action(VisitTypeActions.CreateVisitTypeSuccess)
  createVisitTypeSuccess(ctx: StateContext<VisitTypeStateModel>, action: VisitTypeActions.CreateVisitTypeSuccess) {
    const state = ctx.getState();
    ctx.patchState({
      visitTypes: [...state.visitTypes, action.payload],
      loading: false,
      error: null
    });
  }

  @Action(VisitTypeActions.CreateVisitTypeFailure)
  createVisitTypeFailure(ctx: StateContext<VisitTypeStateModel>, action: VisitTypeActions.CreateVisitTypeFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(VisitTypeActions.UpdateVisitType)
  updateVisitType(ctx: StateContext<VisitTypeStateModel>, action: VisitTypeActions.UpdateVisitType) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.updateVisitType(action.payload).pipe(
      tap(visitType => {
        ctx.dispatch(new VisitTypeActions.UpdateVisitTypeSuccess(visitType));
        ctx.dispatch(new VisitTypeActions.LoadVisitTypes())
      }),
      catchError(error => {
        ctx.dispatch(new VisitTypeActions.UpdateVisitTypeFailure(error.message || 'Failed to update visit type'));
        return of(null);
      })
    );
  }

  @Action(VisitTypeActions.UpdateVisitTypeSuccess)
  updateVisitTypeSuccess(ctx: StateContext<VisitTypeStateModel>, action: VisitTypeActions.UpdateVisitTypeSuccess) {
    const state = ctx.getState();
    const updatedVisitTypes = state.visitTypes.map(vt => 
      vt.visitTypeID === action.payload.visitTypeID ? action.payload : vt
    );
    
    ctx.patchState({
      visitTypes: updatedVisitTypes,
      loading: false,
      error: null
    });
  }

  @Action(VisitTypeActions.UpdateVisitTypeFailure)
  updateVisitTypeFailure(ctx: StateContext<VisitTypeStateModel>, action: VisitTypeActions.UpdateVisitTypeFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(VisitTypeActions.DeleteVisitType)
  deleteVisitType(ctx: StateContext<VisitTypeStateModel>, action: VisitTypeActions.DeleteVisitType) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.deleteVisitType(action.payload).pipe(
      tap(() => {
        ctx.dispatch(new VisitTypeActions.DeleteVisitTypeSuccess(action.payload));
      }),
      catchError(error => {
        ctx.dispatch(new VisitTypeActions.DeleteVisitTypeFailure(error.message || 'Failed to delete visit type'));
        return of(null);
      })
    );
  }

  @Action(VisitTypeActions.DeleteVisitTypeSuccess)
  deleteVisitTypeSuccess(ctx: StateContext<VisitTypeStateModel>, action: VisitTypeActions.DeleteVisitTypeSuccess) {
    const state = ctx.getState();
    const filteredVisitTypes = state.visitTypes.filter(vt => vt.visitTypeID !== action.payload);
    
    ctx.patchState({
      visitTypes: filteredVisitTypes,
      loading: false,
      error: null
    });
  }

  @Action(VisitTypeActions.DeleteVisitTypeFailure)
  deleteVisitTypeFailure(ctx: StateContext<VisitTypeStateModel>, action: VisitTypeActions.DeleteVisitTypeFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }
}