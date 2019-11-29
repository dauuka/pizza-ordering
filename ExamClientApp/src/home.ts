import {LogManager, View, autoinject} from "aurelia-framework";
import {RouteConfig, NavigationInstruction} from "aurelia-router";
import {IProduct} from "interfaces/IProduct";
import {ProductService} from "services/product-service";

export var log = LogManager.getLogger("Home");

@autoinject
export class Home {

  private products: IProduct[] = [];
  
  constructor(private productService: ProductService){
    log.debug('constructor');
  }


  // ============ View LifeCycle events =============
  created(owningView: View, myView: View){
    log.debug('created');
  }

  bind(bindingContext: Object, overrideContext: Object){
    log.debug('bind');
  }

  attached(){
    log.debug('attached');
    this.productService.fetchAll().then(
      jsonData => {
        log.debug('jsonData', jsonData);
        this.products = jsonData;
      }
    );
  }

  detached(){
    log.debug('detached');
  }

  unbind(){
    log.debug('unbind');
  }


  // ============== Router Events ==============
  canActivate(params: any, routerConfig: RouteConfig, navigationInstruction: NavigationInstruction){
    log.debug('canActivate');
  }

  activate(params: any, routerConfig: RouteConfig, navigationInstruction: NavigationInstruction){
    log.debug('activate');
  }

  canDeactivate(){
    log.debug('canDeactivate');
  }

  deactivate(){
    log.debug('deactivate');
  }
}
