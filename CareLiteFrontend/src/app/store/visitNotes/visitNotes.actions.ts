import { CreateVisitRequeset, UpdateVisitRequest } from "../../models/visitNotes";

export class LoadVisits {
  static readonly type = '[Visit] Load';
}

export class AddVisit {
  static readonly type = '[Visit] Add';
  constructor(public payload: CreateVisitRequeset) {}
}

export class UpdateVisit {
  static readonly type = '[Visit] Update';
  constructor(public id: number, public payload: UpdateVisitRequest) {}
}

export class DeleteVisit {
  static readonly type = '[Visit] Delete';
  constructor(public id: number) {}
}

export class GetVisitById {
  static readonly type = '[Visit] Get By Id';
  constructor(public id: number) {}
}
