import { VisitType } from "../../models/visittype";

export interface VisitTypeStateModel {
  visitTypes: VisitType[];
  loading: boolean;
  error: string | null;
}
