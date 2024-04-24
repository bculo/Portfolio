import * as signalR from "@microsoft/signalr";
import { environment } from "./environments/environment";
import { getAccessToken } from "./utilities/token";

const url = environment.websocket;
const authority = environment.oAuth2Config.authority;
const clientId = environment.oAuth2Config.client_id;

class Connector {
    private groups: string[] = [];
    private connection: signalR.HubConnection;
    public onConnect: (null | (() => void)) = null;
    static instance: Connector;


    constructor() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(url, { accessTokenFactory: () => getAccessToken(authority, clientId) })
            .withAutomaticReconnect()
            .build();

        this.connection.start()
            .then((_) => this.onConnect && this.onConnect())
            .catch(err => document.write(err));

    }

    public joinGroup(assetName: string, assetType: 'crypto' | 'stock') {
        if(this.groups.includes(this.formatGroupName(assetName, assetType))) {
            return;
        }
        this.groups.push(this.formatGroupName(assetName, assetType))
        this.connection.invoke("JoinGroup", [assetName, assetType]);
    }
    
    private formatGroupName(assetName: string, assetType: string): string {
        return `${assetName}${assetType}`
    }

    public static getInstance(): Connector {
        if (!Connector.instance)
            Connector.instance = new Connector();
        return Connector.instance;
    }
}

export default Connector.getInstance;