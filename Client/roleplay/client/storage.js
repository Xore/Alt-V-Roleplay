import * as alt from 'alt';
import * as game from 'natives';


export let laborBrowser = null;
let isLaborCEFOpened = false;
let isStorageCEFOpened = false;
let isDynasty8CEFOpened = false;
// let laborBrowser = null;

alt.onServer('Client:HUD:CreateCEF', () => {
    if (laborBrowser == null) {
        laborBrowser = new alt.WebView("http://resource/client/cef/labor/index.html");
        laborBrowser.focus();
        // Storage
        laborBrowser.on("Client:Storage:switchItemToStorage", (storageType, identifierId, name, amount) => {
            alt.emitServer("Server:Storage:switchItemToStorage", parseInt(identifierId), name, parseInt(amount));
        });

        laborBrowser.on("Client:Storage:switchItemToInventory", (storageType, identifierId, name, amount) => {
            alt.emitServer("Server:Storage:switchItemToInventory", parseInt(identifierId), name, parseInt(amount));
        });

        laborBrowser.on("Client:Storage:destroy", () => {
            isStorageCEFOpened = false;
            alt.showCursor(false);
            game.freezeEntityPosition(alt.Player.local.scriptID, false);
            alt.toggleGameControls(true);
            laborBrowser.unfocus();
            alt.emitServer("Server:CEF:setCefStatus", false);
        });
    }
});

alt.onServer("Client:Storage:openStorage", (type, id, invItems, storageItems) => {
    if (laborBrowser == null || isStorageCEFOpened) return;
    isStorageCEFOpened = true;
    laborBrowser.emit("CEF:Storage:openStorage", type, id, invItems, storageItems);
    alt.showCursor(true);
    alt.toggleGameControls(false);
    laborBrowser.focus();
    alt.emitServer("Server:CEF:setCefStatus", true);
});