import {IBaseEntity} from "./IBaseEntity";

export interface ITransport extends IBaseEntity {
  transportName: string;
  transportValue: number;
}
