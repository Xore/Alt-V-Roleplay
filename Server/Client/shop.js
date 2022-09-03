import * as alt from 'alt';
import * as native from "natives";
import * as game from 'natives';
import { Player, Vector3 } from "alt";
import { closeInventoryCEF } from './inventory.js';
import { closeTabletCEF } from './tablet.js';
import { Raycast, GetDirectionFromRotation, setClothes, setTattoo, clearTattoos, setCorrectTattoos } from './utilities.js';

export let shopBrowser = null;
export let browserReady = false;
let ShopCefOpened = false;
let BarberCefOpened = false;
let isTattooShopOpened = false;

alt.onServer("Client:HUD:CreateCEF", () => {
    if (shopBrowser == null) {
        shopBrowser = new alt.WebView("http://resource/client/cef/hud/index.html");
        shopBrowser.on("Client:HUD:cefIsReady", () => {});

        //Tattoo Shop
        shopBrowser.on("Client:TattooShop:closeShop", () => {
            isTattooShopOpened = false;
            alt.showCursor(false);
            alt.toggleGameControls(true);
            shopBrowser.unfocus();
            alt.emitServer("Server:CEF:setCefStatus", false);
            alt.emitServer("Server:ClothesShop:RequestCurrentSkin");
            clearTattoos(alt.Player.local.scriptID);
            setCorrectTattoos();
        });

        shopBrowser.on("Client:TattooShop:buyTattoo", (shopId, tattooId) => {
            alt.emitServer("Server:TattooShop:buyTattoo", parseInt(shopId), parseInt(tattooId));
        });

        shopBrowser.on("Client:TattooShop:deleteTattoo", (id) => {
            alt.emitServer("Server:TattooShop:deleteTattoo", parseInt(id));
        });

        shopBrowser.on("Client:TattooShop:previewTattoo", (hash, collection) => {
            clearTattoos(alt.Player.local.scriptID);
            setTattoo(alt.Player.local.scriptID, collection, hash);
        });

        shopBrowser.on("Client:Shop:buyItem", (shopId, amount, itemname) => {
            alt.emitServer("Server:Shop:buyItem", parseInt(shopId), parseInt(amount), itemname);
        });

        shopBrowser.on("Client:Shop:sellItem", (shopId, amount, itemname) => {
            alt.emitServer("Server:Shop:sellItem", parseInt(shopId), parseInt(amount), itemname);
        });

        shopBrowser.on("Client:Barber:UpdateHeadOverlays", (headoverlayarray) => {
            let headoverlays = JSON.parse(headoverlayarray);
            game.setPedHeadOverlayColor(alt.Player.local.scriptID, 1, 1, parseInt(headoverlays[2][1]), 1);
            game.setPedHeadOverlayColor(alt.Player.local.scriptID, 2, 1, parseInt(headoverlays[2][2]), 1);
            game.setPedHeadOverlayColor(alt.Player.local.scriptID, 5, 2, parseInt(headoverlays[2][5]), 1);
            game.setPedHeadOverlayColor(alt.Player.local.scriptID, 8, 2, parseInt(headoverlays[2][8]), 1);
            game.setPedHeadOverlayColor(alt.Player.local.scriptID, 10, 1, parseInt(headoverlays[2][10]), 1);
            game.setPedEyeColor(alt.Player.local.scriptID, parseInt(headoverlays[0][14]));
            game.setPedHeadOverlay(alt.Player.local.scriptID, 0, parseInt(headoverlays[0][0]), parseInt(headoverlays[1][0]));
            game.setPedHeadOverlay(alt.Player.local.scriptID, 1, parseInt(headoverlays[0][1]), parseFloat(headoverlays[1][1]));
            game.setPedHeadOverlay(alt.Player.local.scriptID, 2, parseInt(headoverlays[0][2]), parseFloat(headoverlays[1][2]));
            game.setPedHeadOverlay(alt.Player.local.scriptID, 3, parseInt(headoverlays[0][3]), parseInt(headoverlays[1][3]));
            game.setPedHeadOverlay(alt.Player.local.scriptID, 4, parseInt(headoverlays[0][4]), parseInt(headoverlays[1][4]));
            game.setPedHeadOverlay(alt.Player.local.scriptID, 5, parseInt(headoverlays[0][5]), parseInt(headoverlays[1][5]));
            game.setPedHeadOverlay(alt.Player.local.scriptID, 6, parseInt(headoverlays[0][6]), parseInt(headoverlays[1][6]));
            game.setPedHeadOverlay(alt.Player.local.scriptID, 7, parseInt(headoverlays[0][7]), parseInt(headoverlays[1][7]));
            game.setPedHeadOverlay(alt.Player.local.scriptID, 8, parseInt(headoverlays[0][8]), parseInt(headoverlays[1][8]));
            game.setPedHeadOverlay(alt.Player.local.scriptID, 9, parseInt(headoverlays[0][9]), parseInt(headoverlays[1][9]));
            game.setPedHeadOverlay(alt.Player.local.scriptID, 10, parseInt(headoverlays[0][10]), parseInt(headoverlays[1][10]));
            game.setPedComponentVariation(alt.Player.local.scriptID, 2, parseInt(headoverlays[0][13]), 0, 0);
            game.setPedHairColor(alt.Player.local.scriptID, parseInt(headoverlays[2][13]), parseInt(headoverlays[1][13]));
        });

        shopBrowser.on("Client:Barber:finishBarber", (headoverlayarray) => {
            alt.emitServer("Server:Barber:finishBarber", headoverlayarray);
            closeBarberCEF();
        });

        shopBrowser.on("Client:Barber:RequestCurrentSkin", () => {
            alt.emitServer("Server:Barber:RequestCurrentSkin");
        });

        shopBrowser.on("Client:Barber:destroyBarberCEF", () => {
            closeBarberCEF();
        });

        shopBrowser.on("Client:Shop:destroyShopCEF", () => {
            closeShopCEF();
        });

        shopBrowser.on("Client:Shop:robShop", (shopId) => {
            alt.emitServer("Server:Shop:robShop", parseInt(shopId));
        });

        shopBrowser.on("Client:Animation:playAnimation", (animDict, animName, animFlag, animDuration) => {
            playAnimation(animDict, animName, animFlag, animDuration);
        });
    }
});

alt.onServer("Client:HUD:sendNotification", (type, duration, msg, delay) => {
    alt.setTimeout(() => {
        if (shopBrowser != null) {
            shopBrowser.emit("CEF:HUD:sendNotification", type, duration, msg);
        }
    }, delay);
});

alt.on("Client:HUD:sendNotification", (type, duration, msg) => {
    if (shopBrowser != null) {
        shopBrowser.emit("CEF:HUD:sendNotification", type, duration, msg);
    }
});

alt.onServer("Client:Shop:shopCEFCreateCEF", (itemArray, shopId, isOnlySelling) => {
    if (shopBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && ShopCefOpened == false) {
        if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true) return;
        shopBrowser.emit("CEF:Shop:shopCEFBoxCreateCEF", itemArray, shopId, isOnlySelling);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        shopBrowser.focus();
        ShopCefOpened = true;
    }
});

alt.onServer("Client:Barber:barberCreateCEF", (headoverlayarray) => {
    if (shopBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && BarberCefOpened == false) {
        shopBrowser.emit("CEF:Barber:barberCEFBoxCreateCEF", headoverlayarray);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        shopBrowser.focus();
        BarberCefOpened = true;

        let barberInterval = alt.setInterval(() => {
            game.invalidateIdleCam();
            if (BarberCefOpened === false) {
                alt.clearInterval(barberInterval);
            }
        }, 5000);
    }
});

let closeShopCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(game.playerPedId(), false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    shopBrowser.unfocus();
    ShopCefOpened = false;
}

let closeBarberCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    shopBrowser.unfocus();
    BarberCefOpened = false;
}

//Tattoo Shop
alt.onServer("Client:TattooShop:openShop", (gender, shopId, ownTattoosJSON) => {
    if (shopBrowser == null || isTattooShopOpened) return;
    alt.emitServer("Server:CEF:setCefStatus", true);
    isTattooShopOpened = true;
    shopBrowser.emit("CEF:TattooShop:openShop", shopId, ownTattoosJSON);
    alt.showCursor(true);
    alt.toggleGameControls(false);
    shopBrowser.focus();
    if (gender == 0) {
        setClothes(alt.Player.local.scriptID, 11, 15, 0);
        setClothes(alt.Player.local.scriptID, 8, 15, 0);
        setClothes(alt.Player.local.scriptID, 3, 15, 0);
        setClothes(alt.Player.local.scriptID, 4, 21, 0);
        setClothes(alt.Player.local.scriptID, 6, 34, 0);
    } else {
        setClothes(alt.Player.local.scriptID, 11, 15, 0);
        setClothes(alt.Player.local.scriptID, 8, 15, 0);
        setClothes(alt.Player.local.scriptID, 3, 15, 0);
        setClothes(alt.Player.local.scriptID, 4, 15, 0);
        setClothes(alt.Player.local.scriptID, 6, 35, 0);
    }
});

alt.onServer("Client:TattooShop:sendItemsToClient", (items) => {
    if (shopBrowser == null) return;
    shopBrowser.emit("CEF:TattooShop:sendItemsToClient", items);
});