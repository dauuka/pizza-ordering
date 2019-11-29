import {IBaseEntity} from "./IBaseEntity";
import {IOrderState} from "./IOrderState";

export interface IFullOrder extends IBaseEntity {
  sum: number;
  time: Date;
  address: string;
  phoneNumber: string;
  orderStateId: number;
  orderState: IOrderState;
  appUserId: number;
  transportId: number;
}
