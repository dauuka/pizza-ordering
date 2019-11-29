import {LogManager, autoinject} from "aurelia-framework";
import {HttpClient} from 'aurelia-fetch-client';
import {IComponentInLine} from "../interfaces/IComponentInLine";
import {BaseService} from "./base-service";
import {AppConfig} from "../app-config";

export var log = LogManager.getLogger('ComponentInLineService');

@autoinject
export class ComponentInLineService extends BaseService<IComponentInLine> {

  constructor(
    private httpClient: HttpClient,
    private appConfig: AppConfig
  ) {
    super(httpClient, appConfig, 'ComponentInLines');
  }

}
