import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { catchError, map, Observable, of, switchMap, take, tap, throwError } from "rxjs";

@Injectable()
export class AppConfig {
    private config: any = null;
    private env: any = null;

    constructor(private http: HttpClient) {}

    public getConfigSection(key: any): any {
        return this.config[key];
    }

    public getEnvironment(key: any): any {
        return this.env[key];
    }

    public load(): void {
        console.log("LOAD");
        this.http.get('./assets/env.json').pipe(
            take(1),
            tap(env => this.env = env),
            switchMap(env => this.resoloveEnvironmentRequest(env).pipe(
                tap(config => this.config = config)
            )),
            catchError(error => {
                console.log(error);
                return of(error)
            })
        ).subscribe();
    }

    private resoloveEnvironmentRequest(envInfo: any): Observable<any> {
        switch(envInfo.environment) {
            case 'production':
                return this.http.get('./assets/env.production.json');
            case 'developoment':
                return this.http.get('./assets/env.development.json');
            default: 
                throw new Error("Environment not defiend");
        }
    }
}

export function initConfig(config: AppConfig) {
    return () => config.load();
}