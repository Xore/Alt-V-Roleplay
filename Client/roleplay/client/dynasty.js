import * as alt from 'alt';
import * as game from 'natives';

export let dynastyBrowser = null;
export let browserReady = false;
let Dynasty8CEFOpened = false;

alt.onServer("Client:HUD:CreateCEF", () => {
    if (dynastyBrowser == null) {
        dynastyBrowser = new alt.WebView("http://resource/client/cef/dynasty/index.html");
        dynastyBrowser.on("Client:Dynasty8:destroy", () => {
            closeDynasty();
        });
    }
    //Dynasty8
    dynastyBrowser.on("Client:Utilities:locatePos", (x, y) => {
        game.setNewWaypoint(x, y);
    });

    dynastyBrowser.on("Client:Dynasty:sellShop", (shopId) => {
        alt.emitServer("Server:Dynasty:sellShop", parseInt(shopId));
    });

    dynastyBrowser.on("Client:Dynasty:buyShop", (shopId) => {
        alt.emitServer("Server:Dynasty:buyShop", parseInt(shopId));
    });

    dynastyBrowser.on("Client:Dynasty:buyStorage", (storageId) => {
        alt.emitServer("Server:Dynasty:buyStorage", parseInt(storageId));
    });

    dynastyBrowser.on("Client:Dynasty:sellStorage", (storageId) => {
        alt.emitServer("Server:Dynasty:sellStorage", parseInt(storageId));
    });
});

// Dynasty8
alt.onServer("Client:Dynasty8:create", (type, myItems, freeItems) => {
    if (dynastyBrowser == null || Dynasty8CEFOpened) return;
    Dynasty8CEFOpened = true;
    dynastyBrowser.emit("CEF:Dynasty8:openDynasty8HUD", type, myItems, freeItems);
    alt.showCursor(true);
    alt.toggleGameControls(false);
    dynastyBrowser.focus();
    Dynasty8CEFOpened = true;
});

let closeDynasty = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    alt.emitServer("Player:Meta:SetCEFOpen", false);
    game.freezeEntityPosition(game.playerPedId(), false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    dynastyBrowser.unfocus();
    Dynasty8CEFOpened = false;
}