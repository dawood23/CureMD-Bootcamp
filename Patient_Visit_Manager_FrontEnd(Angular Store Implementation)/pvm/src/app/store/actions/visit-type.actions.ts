import { VisitType } from "../../models/visittype";

export class LoadVisitTypes {
  static readonly type = '[VisitType] Load Visit Types';
}

export class LoadVisitTypesSuccess {
  static readonly type = '[VisitType] Load Visit Types Success';
  constructor(public payload: VisitType[]) {}
}

export class LoadVisitTypesFailure {
  static readonly type = '[VisitType] Load Visit Types Failure';
  constructor(public payload: string) {}
}

export class CreateVisitType {
  static readonly type = '[VisitType] Create Visit Type';
  constructor(public payload: VisitType) {}
}

export class CreateVisitTypeSuccess {
  static readonly type = '[VisitType] Create Visit Type Success';
  constructor(public payload: VisitType) {}
}

export class CreateVisitTypeFailure {
  static readonly type = '[VisitType] Create Visit Type Failure';
  constructor(public payload: string) {}
}

export class UpdateVisitType {
  static readonly type = '[VisitType] Update Visit Type';
  constructor(public payload: VisitType) {}
}

export class UpdateVisitTypeSuccess {
  static readonly type = '[VisitType] Update Visit Type Success';
  constructor(public payload: VisitType) {}
}

export class UpdateVisitTypeFailure {
  static readonly type = '[VisitType] Update Visit Type Failure';
  constructor(public payload: string) {}
}

export class DeleteVisitType {
  static readonly type = '[VisitType] Delete Visit Type';
  constructor(public payload: number) {}
}

export class DeleteVisitTypeSuccess {
  static readonly type = '[VisitType] Delete Visit Type Success';
  constructor(public payload: number) {}
}

export class DeleteVisitTypeFailure {
  static readonly type = '[VisitType] Delete Visit Type Failure';
  constructor(public payload: string) {}
}
