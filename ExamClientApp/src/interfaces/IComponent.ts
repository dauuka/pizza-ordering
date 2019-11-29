import {IBaseEntity} from "./IBaseEntity";

export interface IComponent extends IBaseEntity {
  componentName: string;
  componentValue: number;
}
