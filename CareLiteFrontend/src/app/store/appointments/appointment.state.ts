import { State, Action, StateContext, Selector } from '@ngxs/store';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs';
import { ApiService } from '../../services/api/api.service';
import { Appointment } from '../../models/appointment';
import {
  LoadAppointments, AddAppointment, UpdateAppointment, DeleteAppointment,
  getAppointmentByID
} from './appointment.actions';

export interface AppointmentStateModel {
  appointments: Appointment[];
  currentAppointment:Appointment|null
}

@State<AppointmentStateModel>({
  name: 'appointments',
  defaults: {
    appointments: [],
    currentAppointment:null
  }
})
@Injectable()
export class AppointmentState {
  constructor(private api: ApiService) {}

  @Selector()
  static appointments(state: AppointmentStateModel) {
    return state.appointments;
  }
  @Selector()
  static currentAppointment(state:AppointmentStateModel){
    return state.currentAppointment
  }

  @Action(LoadAppointments)
  loadAppointments({ patchState }: StateContext<AppointmentStateModel>) {
    return this.api.getAppointments().pipe(
      tap(appointments => patchState({ appointments:appointments }))
    );
  }

  @Action(getAppointmentByID)
  getAppoitnmentByID({patchState}:StateContext<AppointmentStateModel>,{id}:getAppointmentByID){
    return this.api.getAppointmentByID(id).pipe(
      tap(appointment=>patchState({currentAppointment:appointment}))
    )
  }
  @Action(AddAppointment)
  addAppointment({ dispatch }: StateContext<AppointmentStateModel>, { payload }: AddAppointment) {
    return this.api.AddAppointment(payload).pipe(
      tap(() => dispatch(new LoadAppointments()))
    );
  }

  @Action(UpdateAppointment)
  updateAppointment({ dispatch }: StateContext<AppointmentStateModel>, { payload }: UpdateAppointment) {
    return this.api.updateAppointment(payload).pipe(
      tap(() => dispatch(new LoadAppointments()))
    );
  }

  @Action(DeleteAppointment)
  deleteAppointment({ dispatch }: StateContext<AppointmentStateModel>, { id }: DeleteAppointment) {
    return this.api.deleteAppointment(id).pipe(
      tap(() => dispatch(new LoadAppointments()))
    );
  }
}
