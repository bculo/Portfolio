/*
 * Trend.API
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * OpenAPI spec version: 1.0
 *
 * NOTE: This class is auto generated by OpenAPI Generator.
 * https://github.com/OpenAPITools/openapi-generator
 *
 * OpenAPI generator version: 7.2.0-SNAPSHOT
 */


import http from "k6/http";
import { group, check, sleep } from "k6";

const BASE_URL = "/";
// Sleep duration between successive requests.
// You might want to edit the value of this variable or remove calls to the sleep function on the script.
const SLEEP_DURATION = 0.1;
// Global variables should be initialized.

export default function() {
    group("/api/v1/News/GetLatestNews", () => {

        // Request No. 1: 
        {
            let url = BASE_URL + `/api/v1/News/GetLatestNews`;
            let request = http.get(url);

            check(request, {
                "Success": (r) => r.status === 200
            });
        }
    });

    group("/api/v1/News/GetLatestEtfNews", () => {

        // Request No. 1: 
        {
            let url = BASE_URL + `/api/v1/News/GetLatestEtfNews`;
            let request = http.get(url);

            check(request, {
                "Success": (r) => r.status === 200
            });
        }
    });

    group("/api/v1/SearchWord/GetAvailableContextTypes", () => {

        // Request No. 1: 
        {
            let url = BASE_URL + `/api/v1/SearchWord/GetAvailableContextTypes`;
            let request = http.get(url);

            check(request, {
                "Success": (r) => r.status === 200
            });
        }
    });

    group("/api/v1/Sync/Sync", () => {

        // Request No. 1: 
        {
            let url = BASE_URL + `/api/v1/Sync/Sync`;
            let request = http.get(url);

            check(request, {
                "No Content": (r) => r.status === 204
            });
        }
    });

    group("/api/v1/SearchWord/GetAvailableSearchEngines", () => {

        // Request No. 1: 
        {
            let url = BASE_URL + `/api/v1/SearchWord/GetAvailableSearchEngines`;
            let request = http.get(url);

            check(request, {
                "Success": (r) => r.status === 200
            });
        }
    });

    group("/api/v1/SearchWord/GetSearchWords", () => {

        // Request No. 1: 
        {
            let url = BASE_URL + `/api/v1/SearchWord/GetSearchWords`;
            let request = http.get(url);

            check(request, {
                "Success": (r) => r.status === 200
            });
        }
    });

    group("/api/v1/Sync/GetSync/{id}", () => {
        let id = 'TODO_EDIT_THE_ID'; // specify value as there is no example value for this parameter in OpenAPI spec

        // Request No. 1: 
        {
            let url = BASE_URL + `/api/v1/Sync/GetSync/${id}`;
            let request = http.get(url);

            check(request, {
                "Success": (r) => r.status === 200
            });
        }
    });

    group("/api/v1/News/GetLatestEconomyNews", () => {

        // Request No. 1: 
        {
            let url = BASE_URL + `/api/v1/News/GetLatestEconomyNews`;
            let request = http.get(url);

            check(request, {
                "Success": (r) => r.status === 200
            });
        }
    });

    group("/api/v1/Sync/GetSyncStatuses", () => {

        // Request No. 1: 
        {
            let url = BASE_URL + `/api/v1/Sync/GetSyncStatuses`;
            let request = http.get(url);

            check(request, {
                "Success": (r) => r.status === 200
            });
        }
    });

    group("/api/v1/SearchWord/RemoveSearchWord/{id}", () => {
        let id = 'TODO_EDIT_THE_ID'; // specify value as there is no example value for this parameter in OpenAPI spec

        // Request No. 1: 
        {
            let url = BASE_URL + `/api/v1/SearchWord/RemoveSearchWord/${id}`;
            let request = http.del(url);

            check(request, {
                "Success": (r) => r.status === 200
            });
        }
    });

    group("/api/v1/News/GetLatestStockNews", () => {

        // Request No. 1: 
        {
            let url = BASE_URL + `/api/v1/News/GetLatestStockNews`;
            let request = http.get(url);

            check(request, {
                "Success": (r) => r.status === 200
            });
        }
    });

    group("/api/v1/SearchWord/AddNewSearchWord", () => {

        // Request No. 1: 
        {
            let url = BASE_URL + `/api/v1/SearchWord/AddNewSearchWord`;
            // TODO: edit the parameters of the request body.
            let body = {"searchWord": "string", "searchEngine": "integer", "contextType": "integer"};
            let params = {headers: {"Content-Type": "application/json", "Accept": "application/json"}};
            let request = http.post(url, JSON.stringify(body), params);

            check(request, {
                "Success": (r) => r.status === 200
            });
        }
    });

    group("/api/v1/Sync/GetSyncStatusWords/{id}", () => {
        let id = 'TODO_EDIT_THE_ID'; // specify value as there is no example value for this parameter in OpenAPI spec

        // Request No. 1: 
        {
            let url = BASE_URL + `/api/v1/Sync/GetSyncStatusWords/${id}`;
            let request = http.get(url);

            check(request, {
                "Success": (r) => r.status === 200
            });
        }
    });

    group("/api/v1/News/GetLatestCryptoNews", () => {

        // Request No. 1: 
        {
            let url = BASE_URL + `/api/v1/News/GetLatestCryptoNews`;
            let request = http.get(url);

            check(request, {
                "Success": (r) => r.status === 200
            });
        }
    });

}
