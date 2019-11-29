import {IBaseEntity} from "./IBaseEntity";
import {IComponentInLine} from "./IComponentInLine";
import {IProduct} from "./IProduct";

export interface IOrderLine extends IBaseEntity {
  productQuantity: number;
  productValue: number;
  lineSum: number;
  productId: number;
  product: IProduct;
  fullOrderId: number;
  componentInLines: IComponentInLine[]
}
