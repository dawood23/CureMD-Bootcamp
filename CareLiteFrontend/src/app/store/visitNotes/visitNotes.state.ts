import { State, Action, StateContext, Selector } from '@ngxs/store';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs';
import { visitNotes } from '../../models/visitNotes';
import { LoadVisits, AddVisit, UpdateVisit, DeleteVisit, GetVisitById } from './visitNotes.actions';
import { ApiService } from '../../services/api/api.service';

export interface VisitStateModel {
  visits: visitNotes[];
  selectedVisit: visitNotes | null;
}

@State<VisitStateModel>({
  name: 'visits',
  defaults: {
    visits: [],
    selectedVisit: null
  }
})
@Injectable()
export class VisitState {
  constructor(private visitService: ApiService) {}

  @Selector()
  static visits(state: VisitStateModel) {
    return state.visits;
  }

  @Selector()
  static selectedVisit(state: VisitStateModel) {
    return state.selectedVisit;
  }

  @Action(LoadVisits)
  loadVisits({ patchState }: StateContext<VisitStateModel>) {
    return this.visitService.getVisits().pipe(
      tap(visits => patchState({ visits }))
    );
  }

  @Action(GetVisitById)
  getVisitById({ patchState }: StateContext<VisitStateModel>, { id }: GetVisitById) {
    return this.visitService.getVisitById(id).pipe(
      tap(visit => patchState({ selectedVisit: visit }))
    );
  }

  @Action(AddVisit)
  addVisit({ dispatch }: StateContext<VisitStateModel>, { payload }: AddVisit) {
    return this.visitService.addVisit(payload).pipe(
      tap(() => dispatch(new LoadVisits()))
    );
  }

  @Action(UpdateVisit)
  updateVisit({ dispatch }: StateContext<VisitStateModel>, { id, payload }: UpdateVisit) {
    return this.visitService.updateVisit(id, payload).pipe(
      tap(() => dispatch(new LoadVisits()))
    );
  }

  @Action(DeleteVisit)
  deleteVisit({ dispatch }: StateContext<VisitStateModel>, { id }: DeleteVisit) {
    return this.visitService.deleteVisit(id).pipe(
      tap(() => dispatch(new LoadVisits()))
    );
  }
}
