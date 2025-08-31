import { Visit } from "../../models/visit";

export interface VisitStateModel {
  visits: Visit[];
  loading: boolean;
  error: string | null;
}
