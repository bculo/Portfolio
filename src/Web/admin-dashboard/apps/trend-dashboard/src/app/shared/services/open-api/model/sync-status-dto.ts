/**
 * Trend.API
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 1.0
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */
import { SyncStatusWordDto } from './sync-status-word-dto';


export interface SyncStatusDto { 
    id?: string | null;
    started?: string;
    finished?: string;
    totalRequests?: number;
    succeddedRequests?: number;
    searchWords?: Array<SyncStatusWordDto> | null;
}

