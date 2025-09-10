import { State, Action, StateContext, Selector } from '@ngxs/store';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs';
import { ApiService } from '../../services/api/api.service';
import { doctor } from '../../models/doctor';
import { LoadDoctors } from './doctor.actions';

export interface DoctorStateModel {
  doctors: doctor[];
}

@State<DoctorStateModel>({
  name: 'doctors',
  defaults: {
    doctors: []
  }
})
@Injectable()
export class DoctorState {
  constructor(private api: ApiService) {}

  @Selector()
  static doctors(state: DoctorStateModel) {
    return state.doctors;
  }

  @Action(LoadDoctors)
  loadDoctors({ patchState }: StateContext<DoctorStateModel>) {
    return this.api.getDoctors().pipe(
      tap(doctors => patchState({ doctors }))
    );
  }
}
