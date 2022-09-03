import * as alt from 'alt';
import * as native from "natives";
import * as game from 'natives';
import { Player, Vector3 } from "alt";
import { closeInventoryCEF } from './inventory.js';
import { closeTabletCEF } from './tablet.js';
import { Raycast, GetDirectionFromRotation, setClothes, setTattoo, clearTattoos, setCorrectTattoos } from './utilities.js';

export let vehShopBrowser = null;
export let browserReady = false;

let VehicleShopCefOpened = false;


alt.onServer("Client:vehShop:CreateCEF", () => {
    if (vehShopBrowser == null) {
        vehShopBrowser = new alt.WebView("http://resource/client/cef/vehShop/index.html");
        vehShopBrowser.on("Client:vehShop:cefIsReady", () => {});

        vehShopBrowser.on("Client:VehicleShop:destroyVehicleShopCEF", () => {
            alt.emitServer("Player:Meta:SetCEFOpen", false);
            closeVehicleShopCEF();
        });

        vehShopBrowser.on("Client:VehicleShop:BuyVehicle", (shopId, hash) => {
            alt.emitServer("Server:VehicleShop:BuyVehicle", parseInt(shopId), hash);
        });
    }
});

alt.onServer("Client:VehicleShop:OpenCEF", (shopId, shopname, itemArray) => {
    if (vehShopBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && VehicleShopCefOpened == false) {
        vehShopBrowser.emit("CEF:VehicleShop:SetListContent", shopId, shopname, itemArray);
        alt.emitServer("Server:CEF:setCefStatus", true);
        alt.emitServer("Player:Meta:SetCEFOpen", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        vehShopBrowser.focus();
        VehicleShopCefOpened = true;
        game.triggerScreenblurFadeIn(5); //TODO
    }
});

let closeVehicleShopCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    alt.emitServer("Player:Meta:SetCEFOpen", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    vehShopBrowser.unfocus();
    VehicleShopCefOpened = false;
    game.triggerScreenblurFadeOut(5); //TODO
}