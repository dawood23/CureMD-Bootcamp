import { State, Action, StateContext, Selector } from '@ngxs/store';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs';
import { ApiService } from '../../services/api/api.service';
import { Role } from '../../models/roles';
import { LoadRoles } from './role.actions';

export interface RoleStateModel {
  roles: Role[];
}

@State<RoleStateModel>({
  name: 'roles',
  defaults: {
    roles: []
  }
})
@Injectable()
export class RoleState {
  constructor(private api: ApiService) {}

  @Selector()
  static roles(state: RoleStateModel) {
    return state.roles;
  }

  @Action(LoadRoles)
  loadRoles({ patchState }: StateContext<RoleStateModel>) {
    return this.api.getAllRoles().pipe(
      tap(roles => patchState({ roles }))
    );
  }
}
