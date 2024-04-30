import * as signalR from "@microsoft/signalr";

import { injectable } from "inversify";
import "reflect-metadata";
import { WebSocketService } from "./interfaces";
import { getAccessToken } from "../utilities/token";
import { environment } from "../environments/environment";

const url = environment.websocket;
const authority = environment.oAuth2Config.authority;
const clientId = environment.oAuth2Config.client_id;

@injectable()
class SignalRConnector implements WebSocketService {
    private groups: string[] = [];
    private connection: signalR.HubConnection;
    private initialized = false;

    constructor() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(url, { accessTokenFactory: () => getAccessToken(authority, clientId) })
            .withAutomaticReconnect()
            .build();
    }

    public connect(onConnectResult: (success: boolean) => void): void {
        if(this.initialized) {
            return;
        }
        this.initialized = true;
        this.connection.start()
            .then((_) => onConnectResult(true))
            .catch(err => {
                console.error(err);
                onConnectResult(false);
            });
    }

    public joinGroup<T>(assetName: string, onMessage: (message: T) => void) {
        if(this.groups.includes(this.formatGroupName(assetName))) {
            return;
        }

        const groupName = this.formatGroupName(assetName)
        this.groups.push(groupName)
        this.connection.invoke("JoinGroup", groupName)
            .catch(err => console.log(err));

        this.connection.on(assetName.toUpperCase(), (data: T) => onMessage(data))
    }
    
    private formatGroupName(assetName: string): string {
        return `${assetName.toLowerCase()}-stock`
    }
}

export { SignalRConnector }