import * as alt from 'alt';
import * as native from "natives";
import * as game from 'natives';
import { Player, Vector3 } from "alt";
import { Raycast, GetDirectionFromRotation, setClothes, setTattoo, clearTattoos, setCorrectTattoos } from './utilities.js';

export let documentBrowser = null;
export let browserReady = false;

alt.onServer("Client:HUD:CreateCEF", () => {
    if (documentBrowser == null) {
        documentBrowser = new alt.WebView("http://resource/client/cef/documents/index.html");
        documentBrowser.on("Client:HUD:cefIsReady", () => {});
    }
});

alt.onServer("Client:IdentityCard:showIdentityCard", (type, infoArray) => {
    if (documentBrowser != null) {
        documentBrowser.emit("CEF:IdentityCard:showIdentityCard", type, infoArray);
    }
});

alt.onServer("Client:IdentityCard:showDriversLic", (type, infoArray) => {
    if (documentBrowser != null) {
        documentBrowser.emit("CEF:IdentityCard:showDriversLic", type, infoArray);
    }
});

alt.onServer("Client:IdentityCard:showWepLic", (type, infoArray) => {
    if (documentBrowser != null) {
        documentBrowser.emit("CEF:IdentityCard:showWepLic", type, infoArray);
    }
});

alt.onServer("Client:IdentityCard:showFactionCard", (type, infoArray) => {
    if (documentBrowser != null) {
        documentBrowser.emit("CEF:IdentityCard:showFactionCard", type, infoArray);
    }
});

alt.onServer("Client:IdentityCard:showPoliceCard", (type, infoArray) => {
    if (documentBrowser != null) {
        documentBrowser.emit("CEF:IdentityCard:showPoliceCard", type, infoArray);
    }
});

alt.onServer("Client:IdentityCard:showFIBCard", (type, infoArray) => {
    if (documentBrowser != null) {
        documentBrowser.emit("CEF:IdentityCard:showFIBCard", type, infoArray);
    }
});

alt.onServer("Client:IdentityCard:showLSMDCard", (type, infoArray) => {
    if (documentBrowser != null) {
        documentBrowser.emit("CEF:IdentityCard:showLSMDCard", type, infoArray);
    }
});
//ToDo
alt.onServer("Client:IdentityCard:showVehReg", (type, infoArray) => {
    if (documentBrowser != null) {
        documentBrowser.emit("CEF:IdentityCard:showVehReg", type, infoArray);
    }
});