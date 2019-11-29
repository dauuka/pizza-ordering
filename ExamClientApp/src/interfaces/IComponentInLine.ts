import {IBaseEntity} from "./IBaseEntity";
import {IComponent} from "./IComponent";

export interface IComponentInLine extends IBaseEntity {
  orderLineId?: number;
  componentId: number;
  component: IComponent;
}
