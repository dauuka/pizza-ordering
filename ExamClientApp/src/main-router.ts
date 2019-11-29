import {PLATFORM, LogManager, autoinject} from "aurelia-framework";
import {RouterConfiguration, Router} from "aurelia-router";
import { AppConfig } from "app-config";

export var log = LogManager.getLogger("MainRouter");

@autoinject
export class MainRouter {

  public router: Router;

  constructor(private appConfig: AppConfig){
    log.debug('constructor');
  }

  configureRouter(config: RouterConfiguration, router: Router){
    log.debug('configureRouter');

    this.router = router;
    config.title = "PizzaApp";
    config.map(
      [
        {route: ['', 'home'], name: 'home', moduleId: PLATFORM.moduleName('home'), nav: false, title: 'Home'},

        {route: 'identity/login', name: 'identity' + 'Login', moduleId: PLATFORM.moduleName('identity/login'), nav: false, title: 'Login'},
        {route: 'identity/register', name: 'identity' + 'Register', moduleId: PLATFORM.moduleName('identity/register'), nav: false, title: 'Register'},
        {route: 'identity/logout', name: 'identity' + 'Logout', moduleId: PLATFORM.moduleName('identity/logout'), nav: false, title: 'Logout'},

        
        {route: 'products/details/:id', name: 'products' + 'Details', moduleId: PLATFORM.moduleName('products/details'), nav: false, title: 'Product'},
        {route: 'products/message', name: 'productsMessage', moduleId: PLATFORM.moduleName('products/message'), nav: false, title: 'Message'},
        
        {route: ['full-orders','full-orders/index'], name: 'fullOrders' + 'Index', moduleId: PLATFORM.moduleName('full-orders/index'), nav: true, title: 'My Orders'},
        {route: 'full-orders/details/:id', name: 'fullOrders' + 'Details', moduleId: PLATFORM.moduleName('full-orders/details'), nav: false, title: 'Details'},
        {route: 'full-orders/message', name: 'message', moduleId: PLATFORM.moduleName('full-orders/message'), nav: false, title: 'Message'},
        
      ]
    );

  }
}
