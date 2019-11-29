import {LogManager, View, autoinject} from "aurelia-framework";
import {RouteConfig, NavigationInstruction, Router} from "aurelia-router";
import {TransportService} from "../services/transport-service";
import {ITransport} from "../interfaces/ITransport";
import {OrderLineService} from "../services/order-line-service";
import {IOrderLine} from "../interfaces/IOrderLine";
import {FullOrderService} from "../services/full-order-service";
import {IFullOrder} from "../interfaces/IFullOrder";
import {IOrderState} from "../interfaces/IOrderState";

export var log = LogManager.getLogger('Details');

@autoinject
export class Details {

  private cart: IFullOrder | null = null;
  private transports: ITransport[] = [];
  private selectedTransport: ITransport;
  private submitOrderErrorMessage: string = "";

  constructor(
    private router: Router,
    private transportService: TransportService,
    private orderLineService: OrderLineService,
    private fullOrderService: FullOrderService
  ) {
    log.debug('constructor');
  }

  deleteOrderLine(orderLineId:number):void{
    this.orderLineService.delete(orderLineId).then(response => {
      if (this.cart != null && (response.status == 200 || response.status == 204)){
        this.fullOrderService.fetch(this.cart.id).then(
          fullOrder => {
            log.debug('fullOrder', fullOrder);
            this.cart = fullOrder;
          });
      } else {
        log.debug('response', response);
      }
    });
  }
  
  checkOrderForSubmit(orderLines:IOrderLine[]):void{
    this.submitOrderErrorMessage = "";
    if(this.cart != null){
      if(this.cart.address == null){
        this.submitOrderErrorMessage = "Address cannot be empty!";
      } else if(this.cart.phoneNumber == null){
        this.submitOrderErrorMessage = "Phone number cannot be empty!";
      } else if(this.cart.address.length < 1 || this.cart.address.length > 64){
        this.submitOrderErrorMessage = "The length of address must be between 1 to 64!";
      } else if(this.cart.phoneNumber.length < 1 || this.cart.phoneNumber.length > 25){
        this.submitOrderErrorMessage = "The length of phone number must be between 1 to 25!";
      } else {
        let succeeded: boolean = true;
        if(orderLines.length < 1){
          succeeded = false;
          this.submitOrderErrorMessage = "Order must have at least one line!";
        }
        if(succeeded){
          this.submitOrder();
        }
      }
    }
  }

  submitOrder():void{
    if(this.cart != null){
      let fullOrder: IFullOrder = new class implements IFullOrder {
        address: string;
        appUserId: number;
        id: number;
        orderStateId: number;
        orderState: IOrderState;
        phoneNumber: string;
        sum: number;
        time: Date;
        transportId: number;
      };
      fullOrder.address = this.cart.address;
      fullOrder.appUserId = this.cart.appUserId;
      fullOrder.id = this.cart.id;
      fullOrder.phoneNumber = this.cart.phoneNumber;
      fullOrder.sum = this.cart.sum + this.selectedTransport.transportValue;
      fullOrder.transportId = this.selectedTransport.id;
      this.fullOrderService.put(fullOrder!).then(
        response => {
          if (response.status == 204){
            this.router.navigateToRoute("message");
          } else {
            log.error('Error in response!', response);
          }
        });
    }
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
    this.transportService.fetchAll().then(
      jsonData => {
        log.debug('jsonData', jsonData);
        this.transports = jsonData;
        this.selectedTransport = this.transports[0];
      });
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
    this.fullOrderService.fetch(params.id).then(
      fullOrder => {
        log.debug('fullOrder', fullOrder);
        this.cart = fullOrder;
        this.cart.id = params.id;
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
