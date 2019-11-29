import {LogManager, autoinject} from "aurelia-framework";
import {HttpClient} from 'aurelia-fetch-client';
import {IOrderLine} from "../interfaces/IOrderLine";
import {BaseService} from "./base-service";
import {AppConfig} from "../app-config";

export var log = LogManager.getLogger('OrderLineService');

@autoinject
export class OrderLineService extends BaseService<IOrderLine> {

  constructor(
    private httpClient: HttpClient,
    private appConfig: AppConfig
  ) {
    super(httpClient, appConfig, 'OrderLines');
  }

}
