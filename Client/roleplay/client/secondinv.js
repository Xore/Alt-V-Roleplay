import * as alt from 'alt';
import * as native from "natives";
import * as game from 'natives';
import { Player, Vector3 } from "alt";
import { closeInventoryCEF } from './inventory.js';
import { closeTabletCEF } from './tablet.js';

export let invBrowser = null;
export let browserReady = false;

let FactionStorageCefOpened = false;
let GangStorageCefOpened = false;
let VehicleTrunkCefOpened = false;
let PlayerSearchInventoryCefOpened = false;

alt.onServer("Client:HUD:CreateCEF", () => {
    if (invBrowser == null) {
        invBrowser = new alt.WebView("http://resource/client/cef/secondinv/index.html");
        invBrowser.on("Client:HUD:cefIsReady", () => {});

        invBrowser.on("Client:FactionStorage:destroyCEF", () => {
            alt.emitServer("Player:Meta:SetCEFOpen", false);
            closeFactionStorageCEF();
        });

        invBrowser.on("Client:GangStorage:destroyCEF", () => {
            alt.emitServer("Player:Meta:SetCEFOpen", false);
            closeGangStorageCEF();
        });

        invBrowser.on("Client:FactionStorage:FactionStorageAction", (action, factionId, charId, type, itemName, amount, fromContainer) => {
            if (action == "storage") {
                if (type == "faction") {
                    alt.emitServer("Server:FactionStorage:StorageItem", parseInt(factionId), parseInt(charId), itemName, parseInt(amount), fromContainer);
                } else if (type == "hotel") {
                    alt.emitServer("Server:HotelStorage:StorageItem", parseInt(factionId), itemName, parseInt(amount), fromContainer);
                } else if (type == "house") {
                    alt.emitServer("Server:HouseStorage:StorageItem", parseInt(factionId), itemName, parseInt(amount), fromContainer);
                }
            } else if (action == "take") {
                if (type == "faction") {
                    alt.emitServer("Server:FactionStorage:TakeItem", parseInt(factionId), parseInt(charId), itemName, parseInt(amount));
                } else if (type == "hotel") {
                    alt.emitServer("Server:HotelStorage:TakeItem", parseInt(factionId), itemName, parseInt(amount));
                } else if (type == "house") {
                    alt.emitServer("Server:HouseStorage:TakeItem", parseInt(factionId), itemName, parseInt(amount));
                }
            }
        });

        invBrowser.on("Client:GangStorage:GangStorageAction", (action, gangId, charId, type, itemName, amount, fromContainer) => {
            if (action == "storage") {
                if (type == "gang") {
                    alt.emitServer("Server:GangStorage:StorageItem", parseInt(gangId), parseInt(charId), itemName, parseInt(amount), fromContainer);
                }
            } else if (action == "take") {
                if (type == "gang") {
                    alt.emitServer("Server:GangStorage:TakeItem", parseInt(gangId), parseInt(charId), itemName, parseInt(amount));
                }
            }
        });

        invBrowser.on("Client:VehicleTrunk:destroyCEF", () => {
            alt.emitServer("Player:Meta:SetCEFOpen", false);
            closeVehicleTrunkCEF();
        });

        invBrowser.on("Client:VehicleTrunk:VehicleTrunkAction", (action, vehId, charId, itemName, itemAmount, fromContainer, type) => {
            if (action == "storage") {
                alt.emitServer("Server:VehicleTrunk:StorageItem", parseInt(vehId), parseInt(charId), itemName, parseInt(itemAmount), fromContainer, type);
            } else if (action == "take") {
                alt.emitServer("Server:VehicleTrunk:TakeItem", parseInt(vehId), parseInt(charId), itemName, parseInt(itemAmount), type);
            }
        });


        invBrowser.on("Client:PlayerSearch:TakeItem", (targetCharId, itemName, itemLocation, itemAmount) => {

            alt.emitServer("Server:PlayerSearch:TakeItem", parseInt(targetCharId), itemName, itemLocation, parseInt(itemAmount));
        });

        invBrowser.on("Client:PlayerSearch:destroyCEF", () => {
            alt.emitServer("Player:Meta:SetCEFOpen", false);
            closePlayerSearchCEF();
        });
    }
});

alt.onServer("Client:FactionStorage:openCEF", (charId, factionId, type, invArray, storageArray) => {
    if (invBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && FactionStorageCefOpened == false) {
        if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true) return;
        invBrowser.emit("CEF:FactionStorage:openCEF", charId, factionId, type, invArray, storageArray);
        alt.emitServer("Server:CEF:setCefStatus", true);
        alt.emitServer("Player:Meta:SetCEFOpen", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        invBrowser.focus();
        FactionStorageCefOpened = true;
    }
});

alt.onServer("Client:GangStorage:openCEF", (charId, gangId, type, invArray, storageArray) => {
    if (invBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && GangStorageCefOpened == false) {
        if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true) return;
        invBrowser.emit("CEF:GangStorage:openCEF", charId, gangId, type, invArray, storageArray);
        alt.emitServer("Server:CEF:setCefStatus", true);
        alt.emitServer("Player:Meta:SetCEFOpen", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        invBrowser.focus();
        GangStorageCefOpened = true;
    }
});

alt.onServer("Client:VehicleTrunk:openCEF", (charId, vehID, type, invArray, storageArray, currentweight, maxweight) => {
    if (invBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && VehicleTrunkCefOpened == false) {
        invBrowser.emit("CEF:VehicleTrunk:openCEF", charId, vehID, type, invArray, storageArray, currentweight, maxweight);
        alt.emitServer("Server:CEF:setCefStatus", true);
        alt.emitServer("Player:Meta:SetCEFOpen", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        invBrowser.focus();
        VehicleTrunkCefOpened = true;
    }
});

alt.onServer("Client:PlayerSearch:openCEF", (targetCharId, invArray) => {
    if (invBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && PlayerSearchInventoryCefOpened == false) {
        invBrowser.emit("CEF:PlayerSearch:openCEF", targetCharId, invArray);
        alt.emitServer("Server:CEF:setCefStatus", true);
        alt.emitServer("Player:Meta:SetCEFOpen", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        invBrowser.focus();
        PlayerSearchInventoryCefOpened = true;
    }
});

let closeFactionStorageCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    alt.emitServer("Player:Meta:SetCEFOpen", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    invBrowser.unfocus();
    FactionStorageCefOpened = false;
}

let closeGangStorageCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    alt.emitServer("Player:Meta:SetCEFOpen", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    invBrowser.unfocus();
    GangStorageCefOpened = false;
}

let closeVehicleTrunkCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    alt.emitServer("Player:Meta:SetCEFOpen", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    invBrowser.unfocus();
    VehicleTrunkCefOpened = false;
}


let closePlayerSearchCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    alt.emitServer("Player:Meta:SetCEFOpen", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    invBrowser.unfocus();
    PlayerSearchInventoryCefOpened = false;
}