import { Visit } from "../../models/visit";

export class LoadVisits {
  static readonly type = '[Visit] Load Visits';
}

export class LoadVisitsSuccess {
  static readonly type = '[Visit] Load Visits Success';
  constructor(public payload: Visit[]) {}
}

export class LoadVisitsFailure {
  static readonly type = '[Visit] Load Visits Failure';
  constructor(public payload: string) {}
}

export class CreateVisit {
  static readonly type = '[Visit] Create Visit';
  constructor(public payload: Visit) {}
}

export class CreateVisitSuccess {
  static readonly type = '[Visit] Create Visit Success';
  constructor(public payload: Visit) {}
}

export class CreateVisitFailure {
  static readonly type = '[Visit] Create Visit Failure';
  constructor(public payload: string) {}
}

export class UpdateVisit {
  static readonly type = '[Visit] Update Visit';
  constructor(public payload: Visit) {}
}

export class UpdateVisitSuccess {
  static readonly type = '[Visit] Update Visit Success';
  constructor(public payload: Visit) {}
}

export class UpdateVisitFailure {
  static readonly type = '[Visit] Update Visit Failure';
  constructor(public payload: string) {}
}

export class DeleteVisit {
  static readonly type = '[Visit] Delete Visit';
  constructor(public payload: number) {}
}

export class DeleteVisitSuccess {
  static readonly type = '[Visit] Delete Visit Success';
  constructor(public payload: number) {}
}

export class DeleteVisitFailure {
  static readonly type = '[Visit] Delete Visit Failure';
  constructor(public payload: string) {}
}