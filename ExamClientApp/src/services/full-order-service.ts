import {LogManager, autoinject} from "aurelia-framework";
import {HttpClient} from 'aurelia-fetch-client';
import {IFullOrder} from "../interfaces/IFullOrder";
import {BaseService} from "./base-service";
import {AppConfig} from "../app-config";

export var log = LogManager.getLogger('FullOrderService');

@autoinject
export class FullOrderService extends BaseService<IFullOrder> {

  constructor(
    private httpClient: HttpClient,
    private appConfig: AppConfig
  ) {
    super(httpClient, appConfig, 'FullOrders');
  }

}
