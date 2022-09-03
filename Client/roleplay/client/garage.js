import * as alt from 'alt';
import * as native from "natives";
import * as game from 'natives';
import { Player, Vector3 } from "alt";
import { closeInventoryCEF } from './inventory.js';
import { closeTabletCEF } from './tablet.js';

export let garageBrowser = null;
export let browserReady = false;
let GarageCefOpened = false;

alt.onServer("Client:HUD:CreateCEF", () => {
    if (garageBrowser == null) {
        garageBrowser = new alt.WebView("http://resource/client/cef/garage/index.html");
        garageBrowser.on("Client:Garage:destroyGarageCEF", () => {
            closeGarageCEF();
        });
    }

    garageBrowser.on("Client:Garage:destroyGarageCEF", () => {
        alt.emitServer("Player:Meta:SetCEFOpen", false);
        closeGarageCEF();
    });

    garageBrowser.on("Client:Garage:DoAction", (garageid, action, vehid) => {
        alt.emitServer("Server:Garage:DoAction", parseInt(garageid), action, parseInt(vehid));
    });
});

alt.onServer("Client:Garage:OpenGarage", (garageId, garagename, garageInArray, garageOutArray) => {
    if (garageBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && GarageCefOpened == false) {
        garageBrowser.emit("CEF:Garage:OpenGarage", garageId, garagename, garageInArray, garageOutArray);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        garageBrowser.focus();
        GarageCefOpened = true;
    }
});

/*let closeGarageCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    alt.emitServer("Player:Meta:SetCEFOpen", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    garageBrowser.unfocus();
    GarageCefOpened = false;
}*/

let closeGarageCEF = function() {
    if (garageBrowser == null) return;
    if (GarageCefOpened) {
        GarageCefOpened = false,
            alt.emitServer("Server:CEF:setCefStatus", false);
        alt.emitServer("Player:Meta:SetCEFOpen", false);
        game.freezeEntityPosition(alt.Player.local.scriptID, false);
        alt.showCursor(false);
        alt.toggleGameControls(true);
        garageBrowser.unfocus();
    }
}