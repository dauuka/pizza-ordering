import {LogManager, View, autoinject} from "aurelia-framework";
import {RouteConfig, NavigationInstruction, Router} from "aurelia-router";
import {ProductService} from "../services/product-service";
import {IProduct} from "../interfaces/IProduct";
import {OrderLineService} from "../services/order-line-service";
import {IOrderLine} from "../interfaces/IOrderLine";
import {ComponentService} from "../services/component-service";
import {IComponentInLine} from "../interfaces/IComponentInLine";
import {IComponent} from "../interfaces/IComponent";

export var log = LogManager.getLogger('Products.Details');

@autoinject
export class Details {

  private product: IProduct | null = null;
  private orderLine: IOrderLine = new class implements IOrderLine {
    componentInLines: IComponentInLine[] = [];
    fullOrderId: number;
    id: number;
    lineSum: number;
    productId: number;
    product: IProduct;
    productQuantity: number;
    productValue: number;
  };
  private components: IComponent[] = [];
  private quantity: number = 1;
  private selectedComponents: IComponent[] = [];
  private price: number = 0;

  constructor(
    private router: Router,
    private productService: ProductService,
    private orderLineService: OrderLineService,
    private componentService: ComponentService
  ) {
    log.debug('constructor');
  }

  // ============ View methods ==============
  submit():void{
    log.debug('orderLine', this.orderLine);
    if(this.product != null){
      this.orderLine.productValue = this.product.productValue;
      this.orderLine.productQuantity = this.quantity;
      this.orderLine.lineSum = this.price;
      this.orderLine.productId = this.product.id;
      
      for(let i=0; i<this.selectedComponents.length; i++){
        let comp: any = new class ComponentInLine {
          componentId: number;
          id?: number;
          orderLineId?: number;
        };
        comp.componentId = this.selectedComponents[i].id;
        this.orderLine.componentInLines.push(comp);
      }
      
      this.orderLineService.post(this.orderLine).then(
        response => {
          if (response.status == 201){
            this.router.navigateToRoute("productsMessage");
          } else {
            log.error('Error in response!', response);
          }
        }
      );
    }
  }

  changePrice():void{
    let productPrice: number = 0;
    let componentsPrice: number = 0;
    if (this.product != null){
      productPrice = this.product.productValue;
    }
    for(let i=0; i<this.selectedComponents.length; i++){
      componentsPrice += this.selectedComponents[i].componentValue;
    }
    this.price = (productPrice + componentsPrice) * this.quantity;
  }
  
  increaseQuantity():void{
    if (this.quantity < 500){
	  this.quantity += 1;
      this.changePrice();
	}
  }
  
  decreaseQuantity():void{
    if (this.quantity > 1){
	  this.quantity -= 1;
      this.changePrice();
	}
  }

  goBack():void{
    this.router.navigateBack();
  }
  
  // ============ View LifeCycle events ==============
  created(owningView: View, myView: View) {
    log.debug('created');
  }

  bind(bindingContext: Object, overrideContext: Object) {
    log.debug('bind');
  }

  attached() {
    log.debug('attached');
    this.componentService.fetchAll().then(
      jsonData => {
        log.debug('jsonData', jsonData);
        this.components = jsonData;
      }
    );
  }

  detached() {
    log.debug('detached');
  }

  unbind() {
    log.debug('unbind');
  }

  // ============= Router Events =============
  canActivate(params: any, routerConfig: RouteConfig, navigationInstruction: NavigationInstruction) {
    log.debug('canActivate');
  }

  activate(params: any, routerConfig: RouteConfig, navigationInstruction: NavigationInstruction) {
    log.debug('activate', params);
    this.productService.fetch(params.id).then(
      product => {
        log.debug('product', product);
        this.product = product;
        this.product.id = params.id;
        this.changePrice();
      }
    );
  }

  canDeactivate() {
    log.debug('canDeactivate');
  }

  deactivate() {
    log.debug('deactivate');
  }
}
