import {IBaseEntity} from "./IBaseEntity";

export interface IProduct extends IBaseEntity {
  productName: string;
  productDescription: string;
  productValue: number;
  productTypeId: number;
}
