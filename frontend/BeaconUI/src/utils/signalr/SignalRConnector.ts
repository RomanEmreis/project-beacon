import * as signalR from "@microsoft/signalr";

const URL = process.env['services__beacon.api__1'];

class SignalRConnector {
    private static instance: SignalRConnector;
    public readonly connection: signalR.HubConnection;

    constructor(hubUrl: string, onConnected: () => void) {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(`${URL}${hubUrl}`)
            .withAutomaticReconnect()
            .build();
        this.connection
            .start()
            .then(onConnected)
            .catch(err => console.error(err));
    }

    public static getInstance(hubUrl: string, onConnected: () => void): SignalRConnector {
        if (!SignalRConnector.instance) {
            SignalRConnector.instance = new SignalRConnector(hubUrl, onConnected);
        }
        return SignalRConnector.instance;
    }
}
export default SignalRConnector.getInstance;