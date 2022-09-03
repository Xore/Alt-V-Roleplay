import * as alt from 'alt';
import * as native from "natives";
import * as game from 'natives';
import { Player, Vector3 } from "alt";
import { closeInventoryCEF } from './inventory.js';
import { closeTabletCEF } from './tablet.js';
import { Raycast, GetDirectionFromRotation, setClothes, setTattoo, clearTattoos, setCorrectTattoos } from './utilities.js';

export let hudBrowser = null;
export let browserReady = false;
let deathScreen = null;
let identityCardApplyCEFopened = false;
let BankAccountManageFormOpened = false;
let ATMcefOpened = false;
let ShopCefOpened = false;
let BarberCefOpened = false;
let GarageCefOpened = false;
let VehicleShopCefOpened = false;
let FuelStationCefOpened = false;
let bankFactionATMCefOpened = false;
let ClothesShopCefOpened = false;
let GivePlayerBillCefOpened = false;
let FactionStorageCefOpened = false;
let RecievePlayerBillCefOpened = false;
let VehicleTrunkCefOpened = false;
let VehicleLicensingCefOpened = false;
let VehicleKeyCefOpened = false;
let VehicleSellCefOpened = false;
let PlayerSearchInventoryCefOpened = false;
let GivePlayerLicenseCefOpened = false;
let MinijobBusdriverCefOpened = false;
let MinijobPilotCefOpened = false;
let HotelRentCefOpened = false;
let HouseEntranceCefOpened = false;
let DeathscreenCefOpened = false;
let HouseManageCefOpened = false;
let TownhallHouseSelectorCefOpened = false;
let ClothesRadialCefOpened = false;
let TuningMenuCefOpened = false;
let ClothesStorageCefOpened = false;
let curSpeed = 0;
let curKm = 0;
let curTuningVeh = null;
let isPlayerDead = false;
let isTattooShopOpened = false;
let isPlayerUsingMegaphone = false;
let changeVehOwnerCefOpened = false;
let playerVehicle = null;
let lastInteract = 0;

alt.onServer("Client:HUD:CreateCEF", (hunger, thirst, currentmoney) => {
    if (hudBrowser == null) {
        hudBrowser = new alt.WebView("http://resource/client/cef/hud/index.html");
        hudBrowser.on("Client:HUD:cefIsReady", () => {
            alt.setTimeout(function() {
                hudBrowser.emit("CEF:HUD:updateDesireHUD", hunger, thirst);
                hudBrowser.emit("CEF:HUD:updateMoney", currentmoney);
                browserReady = true;
            }, 1000);
            alt.setInterval(function() {
                const health = native.getEntityHealth(alt.Player.local.scriptID) - 100;
                hudBrowser.emit("CEF:HUD:updateHealth", health);
                browserReady = true;
            }, 1000);
        });

        hudBrowser.on("Client:Farming:StartProcessing", (neededItem, producedItem, neededItemAmount, producedItemAmount, duration, neededItemTWO, neededItemTHREE, neededItemTWOAmount, neededItemTHREEAmount) => {
            if (lastInteract + 500 > Date.now()) return;
            lastInteract = Date.now();
            alt.emitServer("Server:Farming:StartProcessing", neededItem, producedItem, neededItemAmount, producedItemAmount, duration, neededItemTWO, neededItemTHREE, neededItemTWOAmount, neededItemTHREEAmount);
        });

        hudBrowser.on("Client:Farming:closeCEF", () => {
            closeFarmingCEF();
        });

        alt.onServer("Client:Farming:createCEF", (neededItem, producedItem, neededItemAmount, producedItemAmount, duration, neededItemTWO, neededItemTHREE, neededItemTWOAmount, neededItemTHREEAmount) => {
            if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && ShopCefOpened == false) {
                if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true) return;
                hudBrowser.emit("CEF:Farming:createCEF", neededItem, producedItem, neededItemAmount, producedItemAmount, duration, neededItemTWO, neededItemTHREE, neededItemTWOAmount, neededItemTHREEAmount);
                alt.emitServer("Server:CEF:setCefStatus", true);
                alt.showCursor(true);
                alt.toggleGameControls(false);
                hudBrowser.focus();
            }
        });

        hudBrowser.on("Client:Vehicle:changeVehOwner", (targetId) => {
            alt.emitServer("Server:Vehicle:changeVehOwner", parseInt(targetId));
        });

        hudBrowser.on("Client:Vehicle:closeChangeVehOwnerHUD", () => {
            alt.emitServer("Server:CEF:setCefStatus", false);
            game.freezeEntityPosition(alt.Player.local.scriptID, false);
            alt.showCursor(false);
            alt.toggleGameControls(true);
            hudBrowser.unfocus();
            changeVehOwnerCefOpened = false;
        });

        //Tattoo Shop
        hudBrowser.on("Client:TattooShop:closeShop", () => {
            isTattooShopOpened = false;
            alt.showCursor(false);
            alt.toggleGameControls(true);
            hudBrowser.unfocus();
            alt.emitServer("Server:CEF:setCefStatus", false);
            alt.emitServer("Server:ClothesShop:RequestCurrentSkin");
            clearTattoos(alt.Player.local.scriptID);
            setCorrectTattoos();
        });

        hudBrowser.on("Client:TattooShop:buyTattoo", (shopId, tattooId) => {
            alt.emitServer("Server:TattooShop:buyTattoo", parseInt(shopId), parseInt(tattooId));
        });

        hudBrowser.on("Client:TattooShop:deleteTattoo", (id) => {
            alt.emitServer("Server:TattooShop:deleteTattoo", parseInt(id));
        });

        hudBrowser.on("Client:TattooShop:previewTattoo", (hash, collection) => {
            clearTattoos(alt.Player.local.scriptID);
            setTattoo(alt.Player.local.scriptID, collection, hash);
        });

        //Rotation HUD
        hudBrowser.on("Client:Utilities:setRotation", (rotZ) => {
            game.setEntityRotation(alt.Player.local.scriptID, game.getEntityPitch(alt.Player.local.scriptID), game.getEntityRoll(alt.Player.local.scriptID), rotZ, 2, true);
        });

        hudBrowser.on("Client:HUD:sendIdentityCardApplyForm", (birthplace) => {
            alt.emitServer("Server:HUD:sendIdentityCardApplyForm", birthplace);
            alt.emitServer("Server:CEF:setCefStatus", false);
            game.freezeEntityPosition(game.playerPedId(), false);
            alt.showCursor(false);
            alt.toggleGameControls(true);
            hudBrowser.unfocus();
            identityCardApplyCEFopened = false;
        });

        hudBrowser.on("Client:Bank:BankAccountdestroyCEF", () => {
            closeBankCEF();
        });

        hudBrowser.on("Client:Bank:BankAccountCreateNewAccount", (selectedBank) => {
            alt.emitServer("Server:Bank:CreateNewBankAccount", selectedBank);
            closeBankCEF();
        });

        hudBrowser.on("Client:Bank:BankAccountAction", (action, accountNumber) => {
            alt.emitServer("Server:Bank:BankAccountAction", action, accountNumber);
            closeBankCEF();
        });

        hudBrowser.on("Client:ATM:requestBankData", (accountNr) => {
            alt.emitServer("Server:ATM:requestBankData", accountNr);
        });

        hudBrowser.on("Client:ATM:WithdrawMoney", (accountNr, amount, zoneName) => {
            alt.emitServer("Server:ATM:WithdrawMoney", accountNr, parseInt(amount), zoneName);
        });

        hudBrowser.on("Client:ATM:DepositMoney", (accountNr, amount, zoneName) => {
            alt.emitServer("Server:ATM:DepositMoney", accountNr, parseInt(amount), zoneName);
        });

        hudBrowser.on("Client:ATM:TransferMoney", (accountNr, targetNr, amount, zoneName) => {
            alt.emitServer("Server:ATM:TransferMoney", accountNr, parseInt(targetNr), parseInt(amount), zoneName);
        });

        hudBrowser.on("Client:ATM:TryPin", (action, curATMAccountNumber) => {
            alt.emitServer("Server:ATM:TryPin", action, curATMAccountNumber);
        });

        hudBrowser.on("Client:ATM:BankATMdestroyCEF", () => {
            closeATMCEF();
        });

        hudBrowser.on("Client:Shop:buyItem", (shopId, amount, itemname) => {
            alt.emitServer("Server:Shop:buyItem", parseInt(shopId), parseInt(amount), itemname);
        });

        hudBrowser.on("Client:Shop:sellItem", (shopId, amount, itemname) => {
            alt.emitServer("Server:Shop:sellItem", parseInt(shopId), parseInt(amount), itemname);
        });

        hudBrowser.on("Client:Barber:UpdateHeadOverlays", (headoverlayarray) => {
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

        hudBrowser.on("Client:Barber:finishBarber", (headoverlayarray) => {
            alt.emitServer("Server:Barber:finishBarber", headoverlayarray);
            closeBarberCEF();
        });

        hudBrowser.on("Client:Barber:RequestCurrentSkin", () => {

            alt.emitServer("Server:Barber:RequestCurrentSkin");
        });

        hudBrowser.on("Client:Barber:destroyBarberCEF", () => {
            closeBarberCEF();
        });

        hudBrowser.on("Client:Shop:destroyShopCEF", () => {
            closeShopCEF();
        });

        hudBrowser.on("Client:Garage:destroyGarageCEF", () => {
            closeGarageCEF();
        });

        hudBrowser.on("Client:VehicleShop:destroyVehicleShopCEF", () => {
            closeVehicleShopCEF();
        });

        hudBrowser.on("Client:VehicleShop:BuyVehicle", (shopId, hash) => {
            alt.emitServer("Server:VehicleShop:BuyVehicle", parseInt(shopId), hash);
        });

        hudBrowser.on("Client:Garage:DoAction", (garageid, action, vehid) => {
            alt.emitServer("Server:Garage:DoAction", parseInt(garageid), action, parseInt(vehid));
        });

        hudBrowser.on("Client:FuelStation:FuelVehicleAction", (vehID, fuelStationId, fueltype, selectedLiterAmount, selectedLiterPrice) => {
            alt.emitServer("Server:FuelStation:FuelVehicleAction", parseInt(vehID), parseInt(fuelStationId), fueltype, parseInt(selectedLiterAmount), parseInt(selectedLiterPrice));
        });

        hudBrowser.on("Client:FuelStation:destroyCEF", () => {
            closeFuelstationCEF();
        });

        hudBrowser.on("Client:ClothesShop:setClothes", (componentId, drawableId, textureId) => {
            game.setPedComponentVariation(alt.Player.local.scriptID, parseInt(componentId), parseInt(drawableId), parseInt(textureId), 0);
        });

        hudBrowser.on("Client:ClothesShop:setAccessory", (componentId, drawableId, textureId) => {
            game.setPedPropIndex(alt.Player.local.scriptID, componentId, drawableId, textureId, false);
        });

        hudBrowser.on("Client:ClothesShop:RequestCurrentSkin", () => {
            alt.emitServer("Server:ClothesShop:RequestCurrentSkin");
        });

        hudBrowser.on("Client:ClothesShop:destroyCEF", () => {
            closeClothesShopCEF();
        });

        hudBrowser.on("Client:ClothesShop:buyItem", (shopId, itemName) => {
            alt.emitServer("Server:ClothesShop:buyItem", parseInt(shopId), parseInt(1), itemName);
        });

        hudBrowser.on("Client:ClothesStorage:destroyCEF", () => {
            closeClothesStorageCEF();
        });

        hudBrowser.on("Client:InteractionMenu:giveRequestedAction", (type, action) => {
            InterActionMenuDoAction(type, action);
        });

        hudBrowser.on("Client:ClothesRadial:giveRequestedAction", (action) => {
            InterActionMenuDoActionClothesRadialMenu(action);
        });

        hudBrowser.on("Client:FactionBank:destroyCEF", () => {
            closeBankFactionATMCEF();
        });

        hudBrowser.on("Client:FactionBank:DepositMoney", (type, factionId, amount) => {
            alt.emitServer("Server:FactionBank:DepositMoney", type, parseInt(factionId), parseInt(amount));
        });

        hudBrowser.on("Client:FactionBank:WithdrawMoney", (type, factionId, amount) => {
            alt.emitServer("Server:FactionBank:WithdrawMoney", type, parseInt(factionId), parseInt(amount));
        });

        hudBrowser.on("Client:GivePlayerBill:giveBill", (type, targetCharId, reason, moneyamount) => {
            alt.emitServer("Server:PlayerBill:giveBill", type, reason, parseInt(targetCharId), parseInt(moneyamount));
            /*let drawmarkertarget = alt.everyTick(() => {
                if (targetCharId != 0)
                    game.drawMarker(27, alt.Player.local.pos.x, alt.Player.local.pos.y, alt.Player.local.pos.z, 0, 0, 0, 0, 0, 0, 1, 1, 0.2, 52, 152, 219, 50, 0, false, 2, false, undefined, undefined, false);
            })
            alt.setTimeout(() => {
                alt.clearEveryTick(drawmarkertarget);
            }, 500);*/
        });

        hudBrowser.on("Client:GivePlayerBill:destroyCEF", () => {
            closeGivePlayerBillCEF();
        });


        hudBrowser.on("Client:PlayerBill:BillAction", (action, billType, factionCompanyId, moneyAmount, reason, charId) => {
            alt.emitServer("Server:PlayerBill:BillAction", action, billType, parseInt(factionCompanyId), parseInt(moneyAmount), reason, parseInt(charId));
        });

        hudBrowser.on("Client:RecievePlayerBill:destroyCEF", () => {
            closeRecievePlayerBillCEF();
        });


        hudBrowser.on("Client:FactionStorage:destroyCEF", () => {
            closeFactionStorageCEF();
        });

        hudBrowser.on("Client:FactionStorage:FactionStorageAction", (action, factionId, charId, type, itemName, amount, fromContainer) => {
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

        hudBrowser.on("Client:VehicleTrunk:destroyCEF", () => {
            closeVehicleTrunkCEF();
        });

        hudBrowser.on("Client:VehicleTrunk:VehicleTrunkAction", (action, vehId, charId, itemName, itemAmount, fromContainer, type) => {
            if (action == "storage") {
                alt.emitServer("Server:VehicleTrunk:StorageItem", parseInt(vehId), parseInt(charId), itemName, parseInt(itemAmount), fromContainer, type);
            } else if (action == "take") {
                alt.emitServer("Server:VehicleTrunk:TakeItem", parseInt(vehId), parseInt(charId), itemName, parseInt(itemAmount), type);
            }
        });

        hudBrowser.on("Client:VehicleLicensing:LicensingAction", (action, vehId, vehPlate, newPlate) => {
            alt.emitServer("Server:VehicleLicensing:LicensingAction", action, parseInt(vehId), vehPlate, newPlate);
        });

        hudBrowser.on("Client:VehicleKey:KeyAction", (action, vehId, charId, itemName, vehPlate, itemAmount) => {
            alt.emitServer("Server:VehicleKey:KeyAction", action, parseInt(vehId), vehPlate, charId, itemName, itemAmount);
        });

        hudBrowser.on("Client:VehicleSell:SellAction", (action, vehId, charId, vehPlate) => {
            alt.emitServer("Server:VehicleSell:SellAction", action, parseInt(vehId), vehPlate, charId);
        });

        hudBrowser.on("Client:VehicleKey:destroyCEF", () => {
            closeVehicleKeyCEF();
        });

        hudBrowser.on("Client:VehicleSell:destroyCEF", () => {
            closeVehicleSellCEF();
        });

        hudBrowser.on("Client:VehicleLicensing:destroyCEF", () => {
            closeVehicleLicensingCEF();
        });

        hudBrowser.on("Client:PlayerSearch:TakeItem", (targetCharId, itemName, itemLocation, itemAmount) => {

            alt.emitServer("Server:PlayerSearch:TakeItem", parseInt(targetCharId), itemName, itemLocation, parseInt(itemAmount));
        });

        hudBrowser.on("Client:PlayerSearch:destroyCEF", () => {
            closePlayerSearchCEF();
        });

        hudBrowser.on("Client:GivePlayerLicense:destroyCEF", () => {
            closeGivePlayerLicenseCEF();
        });

        hudBrowser.on("Client:GivePlayerLicense:GiveLicense", (targetCharId, licname) => {
            alt.emitServer("Server:GivePlayerLicense:GiveLicense", parseInt(targetCharId), licname);
        });

        hudBrowser.on("Client:MinijobBusdriver:StartJob", (routeId) => {
            alt.emitServer("Server:MinijobBusdriver:StartJob", parseInt(routeId));
        });

        hudBrowser.on("Client:MinijobPilot:SelectJob", (level) => {
            alt.emitServer("Server:MinijobPilot:StartJob", parseInt(level));
        });

        hudBrowser.on("Client:MinijobPilot:destroyCEF", () => {
            closeMinijobPilotCEF();
        });

        hudBrowser.on("Client:MinijobBusdriver:destroyCEF", () => {
            closeMinijobBusdriverCEF();
        });

        hudBrowser.on("Client:Hotel:destroyCEF", () => {
            closeHotelRentCEF();
        });

        hudBrowser.on("Client:Hotel:RentHotel", (hotelId, apartmentId) => {
            alt.emitServer("Server:Hotel:RentHotel", parseInt(hotelId), parseInt(apartmentId));
        });

        hudBrowser.on("Client:Hotel:LockHotel", (hotelId, apartmentId) => {
            alt.emitServer("Server:Hotel:LockHotel", parseInt(hotelId), parseInt(apartmentId));
        });

        hudBrowser.on("Client:Hotel:EnterHotel", (hotelId, apartmentId) => {
            alt.emitServer("Server:Hotel:EnterHotel", parseInt(hotelId), parseInt(apartmentId));
        });

        hudBrowser.on("Client:HouseEntrance:destroyCEF", () => {
            closeHouseEntranceCEF();
        });

        hudBrowser.on("Client:Shop:robShop", (shopId) => {
            alt.emitServer("Server:Shop:robShop", parseInt(shopId));
        });

        hudBrowser.on("Client:HouseEntrance:BuyHouse", (houseId) => {
            alt.emitServer("Server:House:BuyHouse", parseInt(houseId));
        });

        hudBrowser.on("Client:HouseEntrance:EnterHouse", (houseId) => {
            alt.emitServer("Server:House:EnterHouse", parseInt(houseId));
        });

        hudBrowser.on("Client:HouseEntrance:RentHouse", (houseId) => {
            alt.emitServer("Server:House:RentHouse", parseInt(houseId));
        });

        hudBrowser.on("Client:HouseEntrance:UnrentHouse", (houseId) => {
            alt.emitServer("Server:House:UnrentHouse", parseInt(houseId));
        });

        hudBrowser.on("Client:House:SellHouse", (houseId) => {
            alt.emitServer("Server:House:SellHouse", parseInt(houseId));
        });

        hudBrowser.on("Client:Vehicle:SellVehicle", (houseId) => {
            alt.emitServer("Server:Vehicle:SellVehicle", parseInt(houseId));
        });

        hudBrowser.on("Client:House:setMainHouse", (houseId) => {
            alt.emitServer("Server:House:setMainHouse", parseInt(houseId));
        });

        hudBrowser.on("Client:HouseManage:destroyCEF", () => {
            closeHouseManageCEF();
        });

        hudBrowser.on("Client:HouseManage:setRentPrice", (houseId, rentPrice) => {
            alt.emitServer("Server:HouseManage:setRentPrice", parseInt(houseId), parseInt(rentPrice));
        });

        hudBrowser.on("Client:HouseManage:setRentState", (houseId, rentState) => {
            alt.emitServer("Server:HouseManage:setRentState", parseInt(houseId), `${rentState}`);
        });

        hudBrowser.on("Client:HouseManage:RemoveRenter", (houseId, renterId) => {
            alt.emitServer("Server:HouseManage:RemoveRenter", parseInt(houseId), parseInt(renterId));
        });

        hudBrowser.on("Client:HouseManage:BuyUpgrade", (houseId, upgrade) => {
            alt.emitServer("Server:HouseManage:BuyUpgrade", parseInt(houseId), upgrade);
        });

        hudBrowser.on("Client:HouseManage:TresorAction", (houseId, action, money) => {
            if (action == "withdraw") {
                alt.emitServer("Server:HouseManage:WithdrawMoney", parseInt(houseId), parseInt(money));
            } else if (action == "deposit") {
                alt.emitServer("Server:HouseManage:DepositMoney", parseInt(houseId), parseInt(money));
            }
        });

        hudBrowser.on("Client:Townhall:destroyHouseSelector", () => {
            destroyTownHallHouseSelector();
        });

        hudBrowser.on("Client:Animation:playAnimation", (animDict, animName, animFlag, animDuration) => {
            playAnimation(animDict, animName, animFlag, animDuration);
        });

        hudBrowser.on("Client:Animations:hideClothesRadialMenu", () => {
            destroyClothesRadialMenu();
        });

        hudBrowser.on("Client:Tuning:switchTuningColor", (type, action, r, g, b) => {
            if (curTuningVeh == null) return;
            alt.emitServer("Server:Tuning:switchTuningColor", curTuningVeh, type, action, parseInt(r), parseInt(g), parseInt(b));
        });

        hudBrowser.on("Client:Tuning:switchTuning", (type, id, action) => {
            if (curTuningVeh == null) return;
            alt.emitServer("Server:Tuning:switchTuning", curTuningVeh, type, parseInt(id), action);
        });

        hudBrowser.on("Client:Tuning:closeCEF", () => {
            closeTuningCEF();
        });

        hudBrowser.on("Client:ClothesStorage:setCharacterClothes", (clothesName, clothesTyp) => {
            if (clothesName == undefined || clothesTyp == undefined) return;
            alt.emitServer("Server:ClothesStorage:setCharacterClothes", clothesTyp, clothesName);
        });
    }
});

// Geld-HUD
alt.onServer("Client:HUD:updateMoney", (currentMoney) => {
    if (hudBrowser != null) {
        hudBrowser.emit("CEF:HUD:updateMoney", currentMoney);
    }
});

alt.onServer("Client:HUD:UpdateDesire", (hunger, thirst) => {
    if (hudBrowser != null) {
        hudBrowser.emit("CEF:HUD:updateDesireHUD", hunger, thirst);
    }
});

alt.onServer("Client:HUD:updateHUDPosInVeh", (state, fuel, km) => {
    if (hudBrowser != null) {
        hudBrowser.emit("CEF:HUD:updateHUDPosInVeh", state, fuel, km);
    }
});

alt.onServer("Client:HUD:sendNotification", (type, duration, msg, delay) => {
    alt.setTimeout(() => {
        if (hudBrowser != null) {
            hudBrowser.emit("CEF:HUD:sendNotification", type, duration, msg);
        }
    }, delay);
});

alt.on("Client:HUD:sendNotification", (type, duration, msg) => {
    if (hudBrowser != null) {
        hudBrowser.emit("CEF:HUD:sendNotification", type, duration, msg);
    }
});

alt.on("Client:SaltyChat:MicStateChanged", (state, voiceRange) => {
    if (hudBrowser == null || !browserReady) return;
    if (state) hudBrowser.emit("CEF:HUD:updateHUDVoice", 0);
    else hudBrowser.emit("CEF:HUD:updateHUDVoice", voiceRange);
});

alt.onServer("client::updateVoiceRange", (voiceRange) => {
    if (hudBrowser == null || !browserReady) return;
    hudBrowser.emit("CEF:HUD:updateHUDVoice", voiceRange);
    let drawmarkertick = alt.everyTick(() => {
        if (voiceRange != 0)
            game.drawMarker(28, alt.Player.local.pos.x, alt.Player.local.pos.y, alt.Player.local.pos.z - 0.95, 0, 0, 0, 0, 0, 0, voiceRange * 2, voiceRange * 2, 0.2, 52, 152, 219, 50, 0, false, 2, false, undefined, undefined, false);
    })
    alt.setTimeout(() => {
        alt.clearEveryTick(drawmarkertick);
    }, 500);
});

alt.onServer("Client:HUD:createIdentityCardApplyForm", (charname, gender, adress, birthdate, curBirthpl) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && curBirthpl == "None" && identityCardApplyCEFopened == false) {
        hudBrowser.emit("CEF:HUD:createIdentityCardApplyForm", charname, gender, adress, birthdate);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        identityCardApplyCEFopened = true;
    }
});

alt.onServer("Client:IdentityCard:showIdentityCard", (type, infoArray) => {
    if (hudBrowser != null) {
        hudBrowser.emit("CEF:IdentityCard:showIdentityCard", type, infoArray);
    }
});

alt.onServer("Client:Bank:createBankAccountManageForm", (bankArray, curBank) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && BankAccountManageFormOpened == false) {
        hudBrowser.emit("CEF:Bank:createBankAccountManageForm", bankArray, curBank);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        BankAccountManageFormOpened = true;
    }
});

alt.onServer("Client:ATM:BankATMcreateCEF", (pin, accNumber, zoneName) => {
    alt.emitServer("Server:Inventory:closeCEF");
    alt.setTimeout(function() {
        if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true) return;
        hudBrowser.emit("CEF:ATM:BankATMcreateCEF", pin, accNumber, zoneName);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        ATMcefOpened = true;
    }, 500);
});

alt.onServer("Client:ATM:BankATMSetRequestedData", (curBalance, paperArray) => {
    if (hudBrowser != null && ATMcefOpened == true) {
        hudBrowser.emit("CEF:ATM:BankATMSetRequestedData", curBalance, paperArray);
    }
});

alt.onServer("Client:ATM:BankATMdestroyCEFBrowser", () => {
    if (hudBrowser != null && ATMcefOpened == true) {
        hudBrowser.emit("CEF:ATM:BankATMdestroyCEF");
    }
});

alt.onServer("Client:Shop:shopCEFCreateCEF", (itemArray, shopId, isOnlySelling) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && ShopCefOpened == false) {
        if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true) return;
        hudBrowser.emit("CEF:Shop:shopCEFBoxCreateCEF", itemArray, shopId, isOnlySelling);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        ShopCefOpened = true;
    }
});

alt.onServer("Client:Barber:barberCreateCEF", (headoverlayarray) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && BarberCefOpened == false) {
        hudBrowser.emit("CEF:Barber:barberCEFBoxCreateCEF", headoverlayarray);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        BarberCefOpened = true;

        let barberInterval = alt.setInterval(() => {
            game.invalidateIdleCam();
            if (BarberCefOpened === false) {
                alt.clearInterval(barberInterval);
            }
        }, 5000);
    }
});

alt.onServer("Client:Garage:OpenGarage", (garageId, garagename, garageInArray, garageOutArray) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && GarageCefOpened == false) {
        hudBrowser.emit("CEF:Garage:OpenGarage", garageId, garagename, garageInArray, garageOutArray);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        GarageCefOpened = true;
    }
});

alt.onServer("Client:VehicleShop:OpenCEF", (shopId, shopname, itemArray) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && VehicleShopCefOpened == false) {
        hudBrowser.emit("CEF:VehicleShop:SetListContent", shopId, shopname, itemArray);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        VehicleShopCefOpened = true;
    }
});

alt.onServer("Client:FuelStation:OpenCEF", (fuelStationId, stationName, owner, maxFuel, availableLiter, fuelArray, vehID) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && FuelStationCefOpened == false) {
        if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true) return;
        hudBrowser.emit("CEF:FuelStation:OpenCEF", fuelStationId, stationName, owner, maxFuel, availableLiter, fuelArray, vehID);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        FuelStationCefOpened = true;
    }
});

alt.onServer("Client:ClothesShop:createCEF", (shopId) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && ClothesShopCefOpened == false) {
        if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true) return;
        hudBrowser.emit("CEF:ClothesShop:createCEF", shopId);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        ClothesShopCefOpened = true;
        let shopInterval = alt.setInterval(() => {
            game.invalidateIdleCam();
            if (ClothesShopCefOpened === false) {
                alt.clearInterval(shopInterval);
            }
        }, 5000);
    }
});

alt.onServer("Client:ClothesShop:sendItemsToClient", (items) => {
    if (hudBrowser != null) {
        hudBrowser.emit("CEF:ClothesShop:sendItemsToClient", items);
    }
});

alt.onServer("Client:ClothesStorage:createCEF", () => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && ClothesStorageCefOpened == false) {
        if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true) return;
        hudBrowser.emit("CEF:ClothesStorage:createCEF");
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        ClothesStorageCefOpened = true;
        let shopInterval = alt.setInterval(() => {
            game.invalidateIdleCam();
            if (ClothesStorageCefOpened === false) {
                alt.clearInterval(shopInterval);
            }
        }, 5000);
    }
});

alt.onServer("Client:ClothesStorage:sendItemsToClient", (items) => {
    if (hudBrowser != null) {
        hudBrowser.emit("CEF:ClothesStorage:sendItemsToClient", items);
    }
});

alt.onServer("Client:FactionBank:createCEF", (type, factionId, factionBalance) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && bankFactionATMCefOpened == false) {
        if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true) return;
        hudBrowser.emit("CEF:FactionBank:createCEF", type, factionId, factionBalance);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        bankFactionATMCefOpened = true;
    }
});

alt.onServer("Client:Vehicle:showChangeOwnerHUD", (vehName) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && changeVehOwnerCefOpened == false) {
        hudBrowser.emit("CEF:Vehicle:showChangeOwnerHUD", vehName);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        changeVehOwnerCefOpened = true;
    }
});

alt.onServer("Client:GivePlayerBill:openCEF", (type, targetCharId) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && GivePlayerBillCefOpened == false) {
        if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true) return;
        hudBrowser.emit("CEF:GivePlayerBill:openCEF", type, targetCharId);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        GivePlayerBillCefOpened = true;
    }
});

alt.onServer("Client:RecievePlayerBill:openCEF", (type, factionCompanyId, moneyAmount, reason, factionCompanyName, charId) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && RecievePlayerBillCefOpened == false) {
        hudBrowser.emit("CEF:RecievePlayerBill:openCEF", type, factionCompanyId, moneyAmount, reason, factionCompanyName, charId);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        RecievePlayerBillCefOpened = true;
    }
});

alt.onServer("Client:FactionStorage:openCEF", (charId, factionId, type, invArray, storageArray) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && FactionStorageCefOpened == false) {
        if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true) return;
        hudBrowser.emit("CEF:FactionStorage:openCEF", charId, factionId, type, invArray, storageArray);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        FactionStorageCefOpened = true;
    }
});

alt.onServer("Client:VehicleTrunk:openCEF", (charId, vehID, type, invArray, storageArray, currentweight, maxweight) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && VehicleTrunkCefOpened == false) {
        hudBrowser.emit("CEF:VehicleTrunk:openCEF", charId, vehID, type, invArray, storageArray, currentweight, maxweight);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        VehicleTrunkCefOpened = true;
    }
});

alt.onServer("Client:VehicleLicensing:openCEF", (vehArray) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && VehicleLicensingCefOpened == false) {
        hudBrowser.emit("CEF:VehicleLicensing:openCEF", vehArray);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        VehicleLicensingCefOpened = true;
    }
});

alt.onServer("Client:VehicleKey:openCEF", (vehArray) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && VehicleKeyCefOpened == false) {
        hudBrowser.emit("CEF:VehicleKey:openCEF", vehArray);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        VehicleKeyCefOpened = true;
    }
});

alt.onServer("Client:VehicleSell:openCEF", (vehArray) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && VehicleSellCefOpened == false) {
        hudBrowser.emit("CEF:VehicleSell:openCEF", vehArray);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        VehicleSellCefOpened = true;
    }
});

alt.onServer("Client:PlayerSearch:openCEF", (targetCharId, invArray) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && PlayerSearchInventoryCefOpened == false) {
        hudBrowser.emit("CEF:PlayerSearch:openCEF", targetCharId, invArray);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        PlayerSearchInventoryCefOpened = true;
    }
});

alt.onServer("Client:GivePlayerLicense:openCEF", (targetCharId, licArray) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && GivePlayerLicenseCefOpened == false) {
        hudBrowser.emit("CEF:GivePlayerLicense:SetGivePlayerLicenseCEFContent", targetCharId, licArray);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        GivePlayerLicenseCefOpened = true;
    }
});

alt.onServer("Client:MinijobBusdriver:openCEF", (routeArray) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && MinijobBusdriverCefOpened == false) {
        hudBrowser.emit("CEF:MinijobBusdriver:openCEF", routeArray);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        MinijobBusdriverCefOpened = true;
    }
});

alt.onServer("Client:MinijobPilot:openCEF", () => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && MinijobPilotCefOpened == false) {
        hudBrowser.emit("CEF:MinijobPilot:openCEF");
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        MinijobPilotCefOpened = true;
    }
});

alt.onServer("Client:HouseManage:openCEF", (houseInfoArray, renterArray) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && HouseManageCefOpened == false) {
        hudBrowser.emit("CEF:HouseManage:openCEF", houseInfoArray, renterArray);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        HouseManageCefOpened = true;
    }
});

alt.onServer("Client:Hotel:setApartmentItems", (array) => {
    if (hudBrowser != null) {
        hudBrowser.emit("CEF:Hotel:setApartmentItems", array);
    }
});

alt.onServer("Client:Hotel:openCEF", (hotelname) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && HotelRentCefOpened == false) {
        HotelRentCefOpened = true;
        hudBrowser.emit("CEF:Hotel:openCEF", hotelname);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
    }
});

alt.onServer("Client:HouseEntrance:openCEF", (charId, houseArray, isRentedIn) => {
    if (hudBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && HouseEntranceCefOpened == false) {
        hudBrowser.emit("CEF:HouseEntrance:openCEF", charId, houseArray, isRentedIn);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        HouseEntranceCefOpened = true;
    }
});

alt.onServer("Client:Tuning:openTuningMenu", (veh, Items) => {
    if (hudBrowser != null && TuningMenuCefOpened == false) {
        curTuningVeh = veh;
        hudBrowser.emit("CEF:Tuning:openTuningMenu", Items);
        alt.emitServer("Server:CEF:setCefStatus", true);
        alt.toggleGameControls(false);
        alt.showCursor(true);
        hudBrowser.focus();
        TuningMenuCefOpened = true;
    }
});

alt.onServer("Client:Deathscreen:openCEF", () => {
    if (hudBrowser != null && DeathscreenCefOpened == false) {
        closeAllCEFs();
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.setEntityInvincible(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        DeathscreenCefOpened = true;
        isPlayerDead = true;
        deathScreen = new alt.WebView("http://resource/client/cef/deathscreen/death.html");
        deathScreen.focus();
        // alt.setTimeout(() => {
        //     hudBrowser.focus();
        //     hudBrowser.emit("CEF:Deathscreen:openCEF");
        // }, 3000);
    }
});

alt.onServer("Client:Deathscreen:closeCEF", () => {
    if (hudBrowser != null || deathScreen != null) {
        deathScreen.destroy();
        hudBrowser.emit("CEF:Deathscreen:closeCEF");
        alt.emitServer("Server:CEF:setCefStatus", false);
        game.freezeEntityPosition(alt.Player.local.scriptID, false);
        game.setEntityInvincible(alt.Player.local.scriptID, false);
        alt.showCursor(false);
        alt.toggleGameControls(true);
        hudBrowser.unfocus();
        DeathscreenCefOpened = false;
        isPlayerDead = false;
    }
});


alt.onServer("Client:Townhall:openHouseSelector", (array) => {
    if (hudBrowser != null && !TownhallHouseSelectorCefOpened) {
        hudBrowser.emit("CEF:Townhall:openHouseSelector", array);
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        hudBrowser.focus();
        TownhallHouseSelectorCefOpened = true;
    }
});

alt.onServer("Client:Animations:setupItems", (array) => {
    let interval = alt.setInterval(() => {
        if (hudBrowser != null && browserReady) {
            alt.clearInterval(interval);
            hudBrowser.emit("CEF:Animations:setupItems", array);
        }
    }, 10);
});

let OldVehKMPos,
    curVehKMid = 0,
    GetVehKMPos = false;

alt.onServer("Client:HUD:GetDistanceForVehicleKM", () => {
    if (hudBrowser != null && alt.Player.local.vehicle != null) {
        if (curVehKMid == 0) { curVehKMid = alt.Player.local.vehicle.scriptID; }
        if (curVehKMid != alt.Player.local.vehicle.scriptID) { GetVehKMPos = false; }

        if (!GetVehKMPos) {
            OldVehKMPos = alt.Player.local.vehicle.pos;
            GetVehKMPos = true;
            return;
        }

        if (GetVehKMPos) {
            let curPos = alt.Player.local.vehicle.pos;
            let dist = game.getDistanceBetweenCoords(OldVehKMPos.x, OldVehKMPos.y, OldVehKMPos.z, curPos.x, curPos.y, curPos.z, false);
            alt.emitServer("Server:Vehicle:UpdateVehicleKM", dist);
            OldVehKMPos = alt.Player.local.vehicle.pos;
        }
    }
});

let vehicle = null;
let interactVehicle = null;
let interactPlayer = null;
let playerRC = null;
let selectedRaycastId = null;
let InteractMenuUsing = false;
let ClothesRadialMenuUsing = false;

//  on vehicle exit toggle off - when using seatbelt toggle on
alt.onServer("Client:Vehicle:ToggleSeatbelt", (toggleSwitch) => {
    native.setPedConfigFlag(alt.Player.local.scriptID, 32, toggleSwitch);
    console.log("getPedConfigFlag: " + native.getPedConfigFlag(alt.Player.local.scriptID, 32, 1));
});

//  actually unneeded atm
alt.onServer("Client:Vehicle:SwitchSeatbelt", () => {
    let seatbeltStatus = native.getPedConfigFlag(alt.Player.local.scriptID, 32, 1);
    let seatbeltStatusSwitched = !seatbeltStatus;
    native.setPedConfigFlag(alt.Player.local.scriptID, 32, seatbeltStatusSwitched);
    console.log("current Status Seatbelt: " + seatbeltStatus);
    console.log("switched state: " + seatbeltStatusSwitched);
    console.log("getPedConfigFlag: " + native.getPedConfigFlag(alt.Player.local.scriptID, 32, 1));
});


alt.onServer("Client:RaycastMenu:SetMenuItems", (type, itemArray) => { //Type: player, vehicleOut, vehicleIn
    if (hudBrowser != null) {
        hudBrowser.emit("CEF:InteractionMenu:toggleInteractionMenu", true, type, itemArray);
    }
});

alt.onServer("Client:ClothesRadial:SetMenuItems", (itemArray) => {
    if (hudBrowser != null) {
        hudBrowser.emit("CEF:ClothesRadial:toggleInteractionMenu", true, itemArray);
    }
});

alt.on('keydown', (key) => {
    if (key == 'X'.charCodeAt(0)) {
        if (alt.Player.local.getSyncedMeta("IsCefOpen")) return;
        let result = Raycast.line(1.5, 2.5);
        if (result == undefined && !alt.Player.local.vehicle) return;
        if (!alt.Player.local.vehicle) {
            if (result.isHit && result.entityType != 0) {
                if (result.entityType == 1 && hudBrowser != null) {
                    selectedRaycastId = result.hitEntity;
                    interactPlayer = alt.Player.all.find(x => x.scriptID == selectedRaycastId);
                    if (!interactPlayer) return;
                    InteractMenuUsing = true;
                    hudBrowser.focus();
                    alt.showCursor(true);
                    alt.toggleGameControls(false);
                    alt.emitServer("Server:InteractionMenu:GetMenuPlayerItems", "player", interactPlayer);
                    interactPlayer = null;
                    return;
                } else if (result.entityType == 2 && hudBrowser != null) {
                    selectedRaycastId = result.hitEntity;
                    interactVehicle = alt.Vehicle.all.find(x => x.scriptID == selectedRaycastId);
                    if (!interactVehicle) return;
                    InteractMenuUsing = true;
                    hudBrowser.focus();
                    alt.showCursor(true);
                    alt.toggleGameControls(false);
                    alt.emitServer("Server:InteractionMenu:GetMenuVehicleItems", "vehicleOut", interactVehicle);
                    interactVehicle = null;
                    return;
                }
            }
        }

        if (alt.Player.local.vehicle && hudBrowser != null) {
            selectedRaycastId = alt.Player.local.vehicle.scriptID;
            interactVehicle = alt.Vehicle.all.find(x => x.scriptID == selectedRaycastId);
            InteractMenuUsing = true;
            hudBrowser.focus();
            alt.showCursor(true);
            alt.toggleGameControls(false);
            if (!interactVehicle) return;
            alt.emitServer("Server:InteractionMenu:GetMenuVehicleItems", "vehicleIn", interactVehicle);
            interactVehicle = null;
            return;
        }
    } else if (key == 'K'.charCodeAt(0)) {
        if (alt.Player.local.getSyncedMeta("IsCefOpen")) return;
        ClothesRadialMenuUsing = true;
        hudBrowser.focus();
        alt.showCursor(true);
        alt.toggleGameControls(false);
        alt.emitServer("Server:ClothesRadial:GetClothesRadialItems");
        return;
    }
    // SALTYCHAT 
    else if (key === 220 && !alt.Player.local.getSyncedMeta("IsCefOpen")) {
        alt.emit("SaltyChat:ToggleRange");
    } else if (key === 'Z'.charCodeAt(0) && alt.Player.local.vehicle && alt.Player.local.scriptID == game.getPedInVehicleSeat(alt.Player.local.vehicle.scriptID, -1) && game.getVehicleClass(alt.Player.local.vehicle.scriptID) == 18) {
        isPlayerUsingMegaphone = true;
        alt.emit("SaltyChat:UseMegaphone", true);
    }
});

alt.on('keyup', (key) => {
    if (key == 'X'.charCodeAt(0)) {
        if (hudBrowser == null || InteractMenuUsing == false) return;
        hudBrowser.emit("CEF:InteractionMenu:requestAction");
        hudBrowser.emit("CEF:InteractionMenu:toggleInteractionMenu", false);
        InteractMenuUsing = false;
        hudBrowser.unfocus();
        alt.showCursor(false);
        alt.toggleGameControls(true);
    } else if (key == 'K'.charCodeAt(0)) {
        if (alt.Player.local.getSyncedMeta("IsCefOpen")) return;
        hudBrowser.emit("CEF:ClothesRadial:requestAction");
        hudBrowser.emit("CEF:ClothesRadial:toggleInteractionMenu", false);
        ClothesRadialMenuUsing = false;
        hudBrowser.unfocus();
        alt.showCursor(false);
        alt.toggleGameControls(true);
        // SALTYCHAT
    } else if (key === 'Z'.charCodeAt(0) && isPlayerUsingMegaphone) {
        isPlayerUsingMegaphone = false;
        alt.emit("SaltyChat:UseMegaphone", false);
    }
});

function InterActionMenuDoAction(type, action) {
    if (selectedRaycastId != null && selectedRaycastId != 0 && type != "none") {
        if (type == "vehicleOut") { type = "vehicle"; }
        if (type == "vehicleIn") { type = "vehicle"; }
        if (type == "vehicle") {
            vehicle = alt.Vehicle.all.find(x => x.scriptID == selectedRaycastId);
            if (!vehicle) return;
            if (action == "vehtoggleLock") {
                alt.emitServer("Server:Raycast:LockVehicle", vehicle);
                playAnimation("anim@mp_player_intmenu@key_fob@", "fob_click_fp", 49, 1000);
            } else if (action == "vehtoggleEngine") {
                alt.emitServer("Server:Raycast:ToggleVehicleEngine", vehicle);
            } else if (action == "seatbelt") {
                alt.emitServer("Server:Vehicle:ToggleSeatbelt", vehicle, true);
            } else if (action == "vehFuelVehicle") {
                alt.emitServer("Server:Raycast:OpenVehicleFuelMenu", vehicle);
            } else if (action == "vehRepair") {
                alt.emitServer("Server:Raycast:RepairVehicle", vehicle);
                playAnimation("mini@repair", "fixing_a_player", 49, 5000);
            } else if (action == "vehClear") {
                alt.emitServer("Server:Raycast:WashVehicle", vehicle);
                playAnimation("mini@repair", "fixing_a_player", 49, 5000);
            } else if (action == "vehOpenCloseTrunk") {
                alt.emitServer("Server:Raycast:OpenCloseVehicleTrunk", vehicle);
            } else if (action == "vehViewTrunkContent") {
                alt.emitServer("Server:Raycast:ViewVehicleTrunk", vehicle);
            } else if (action == "vehViewGloveboxContent") {
                alt.emitServer("Server:Raycast:ViewVehicleGlovebox", vehicle);
            } else if (action == "vehTow") {
                alt.emitServer("Server:Raycast:towVehicle", vehicle);
            } else if (action == "vehTuning") {
                alt.emitServer("Server:Raycast:tuneVehicle", vehicle);
            } else if (action == "vehchangeowner") {
                alt.emitServer("Server:Raycast:showChangeOwnerHUD", vehicle);
            } else if (action == "vehBreakVehicle") {
                alt.emitServer("Server:Raycast:BreakVehicle", vehicle);
            } else if (action == "vehbreakEngine") {
                alt.emitServer("Server:Raycast:BreakVehicleEngine", vehicle);
            }
            vehicle = null;
            //newVehicleBlip = null;
        } else if (type == "player") {
            playerRC = alt.Player.all.find(x => x.scriptID == selectedRaycastId);
            if (!playerRC) return;
            if (action == "playersupportId") {
                alt.emitServer("Server:Raycast:showPlayerSupportId", playerRC);
            } else if (action == "playergiveItem") {
                alt.emitServer("Server:Raycast:givePlayerItemRequest", playerRC);
            } else if (action == "playergiveFactionBill") {
                alt.emitServer("Server:Raycast:OpenGivePlayerBillCEF", playerRC, "faction");
                /*} else if (action == "playergiveCompanyBill") {
                    alt.emitServer("Server:Raycast:OpenGivePlayerBillCEF", playerRC, "company");*/
            } else if (action == "playerGiveTakeHandcuffs") {
                alt.emitServer("Server:Raycast:GiveTakeHandcuffs", playerRC);
            } else if (action == "playerGiveTakeRopeCuffs") {
                alt.emitServer("Server:Raycast:GiveTakeRopeCuffs", playerRC);
            } else if (action == "playerSearchInventory") {
                alt.emitServer("Server:Raycast:SearchPlayerInventory", playerRC);
            } else if (action == "playerGiveLicense") {
                alt.emitServer("Server:Raycast:openGivePlayerLicenseCEF", playerRC);
            } else if (action == "playerRevive") {
                alt.emitServer("Server:Raycast:RevivePlayer", playerRC);
            } else if (action == "playerJail") {
                alt.emitServer("Server:Raycast:jailPlayer", playerRC);
            } else if (action == "showIdCard") {
                alt.emitServer("Server:Raycast:showIdcard", playerRC);
            } else if (action == "healPlayer") {
                alt.emitServer("Server:Raycast:healPlayer", playerRC);
            }
            playerRC = null;

            //newPlayerBlip = null;

        }
        selectedRaycastId = null;
    }
}

function InterActionMenuDoActionClothesRadialMenu(action) {
    new Promise((resolve, reject) => {
        alt.emitServer("Server:ClothesRadial:SetNormalSkin", action);
    }).then(() => {
        //console.log("Fehler aufgetreten...")
    });
}

alt.everyTick(() => {
    game.setPedConfigFlag(alt.Player.local.scriptID, 35, false);
    if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true) {
        game.disableControlAction(0, 24, true);
        game.disableControlAction(0, 25, true);
        game.disableControlAction(0, 12, true);
        game.disableControlAction(0, 13, true);
        game.disableControlAction(0, 14, true);
        game.disableControlAction(0, 15, true);
        game.disableControlAction(0, 16, true);
        game.disableControlAction(0, 17, true);
        game.disableControlAction(0, 37, true);
        game.disableControlAction(0, 44, true);
        game.disableControlAction(0, 45, true);
        game.disableControlAction(0, 263, true);
        game.disableControlAction(0, 264, true);
        game.disableControlAction(0, 140, true);
        game.disableControlAction(0, 141, true);
        game.disableControlAction(0, 257, true);
        game.disableControlAction(0, 345, true);
    } else {
        game.enableControlAction(0, 24, true);
        game.enableControlAction(0, 25, true);
        game.enableControlAction(0, 12, true);
        game.enableControlAction(0, 13, true);
        game.enableControlAction(0, 14, true);
        game.enableControlAction(0, 15, true);
        game.enableControlAction(0, 16, true);
        game.enableControlAction(0, 17, true);
        game.enableControlAction(0, 37, true);
        game.enableControlAction(0, 44, true);
        game.enableControlAction(0, 45, true);
        game.enableControlAction(0, 263, true);
        game.enableControlAction(0, 264, true);
        game.enableControlAction(0, 140, true);
        game.enableControlAction(0, 141, true);
        game.enableControlAction(0, 257, true);
        game.enableControlAction(0, 345, true);
    }
    if (hudBrowser == null) return;
    if (alt.Player.local.vehicle == null) return;
    const street = game.getStreetNameAtCoord(alt.Player.local.pos.x, alt.Player.local.pos.y, alt.Player.local.pos.z);
    const zoneName = game.getLabelText(game.getNameOfZone(alt.Player.local.pos.x, alt.Player.local.pos.y, alt.Player.local.pos.z));
    const streetName = game.getStreetNameFromHashKey(street[1]);
    hudBrowser.emit("CEF:HUD:updateStreetLocation", streetName + ", " + zoneName);
    GetVehicleSpeed();
    hudBrowser.emit("CEF:HUD:SetPlayerHUDVehicleSpeed", curSpeed);
    if (alt.Player.local.vehicle.model != 2621610858 && alt.Player.local.vehicle.model != 1341619767 && alt.Player.local.vehicle.model != 2999939664) {
        game.setPedConfigFlag(alt.Player.local.scriptID, 429, 1);
    } else {
        game.setPedConfigFlag(alt.Player.local.scriptID, 429, 0);
    }
    game.setPedConfigFlag(alt.Player.local.scriptID, 184, true);
    game.setAudioFlag("DisableFlightMusic", true);
});

let closeFarmingCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    alt.toggleGameControls(true);
    alt.showCursor(false);
    hudBrowser.unfocus();
}

let closeBankCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(game.playerPedId(), false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    BankAccountManageFormOpened = false;
}

let closeATMCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(game.playerPedId(), false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    ATMcefOpened = false;
}

let closeShopCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(game.playerPedId(), false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    ShopCefOpened = false;
}

let closeBarberCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    BarberCefOpened = false;
}

let closeGarageCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    GarageCefOpened = false;
}

let closeVehicleShopCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    VehicleShopCefOpened = false;
}

let closeFuelstationCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    FuelStationCefOpened = false;
}

let closeClothesShopCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    ClothesShopCefOpened = false;
}

let closeClothesStorageCEF = function() {
    alt.emitServer("Server:ClothesShop:RequestCurrentSkin");
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    ClothesStorageCefOpened = false;
}

let closeBankFactionATMCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    bankFactionATMCefOpened = false;
}

let closeGivePlayerBillCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    GivePlayerBillCefOpened = false;
}

let closeRecievePlayerBillCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    RecievePlayerBillCefOpened = false;
}


let closeFactionStorageCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    FactionStorageCefOpened = false;
}

let closeVehicleTrunkCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    VehicleTrunkCefOpened = false;
}

let closeVehicleLicensingCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    VehicleLicensingCefOpened = false;
}

let closeVehicleKeyCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    VehicleKeyCefOpened = false;
}

let closeVehicleSellCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    VehicleSellCefOpened = false;
    console.log("closed")
}

let closePlayerSearchCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    PlayerSearchInventoryCefOpened = false;
}

let closeGivePlayerLicenseCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    GivePlayerLicenseCefOpened = false;
}

let closeMinijobBusdriverCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    MinijobBusdriverCefOpened = false;
}

let closeMinijobPilotCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    MinijobPilotCefOpened = false;
}

let closeHotelRentCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    HotelRentCefOpened = false;
}

let closeHouseEntranceCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    HouseEntranceCefOpened = false;
}

let closeHouseManageCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    HouseManageCefOpened = false;
}

let destroyTownHallHouseSelector = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    TownhallHouseSelectorCefOpened = false;
}

let destroyClothesRadialMenu = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    hudBrowser.unfocus();
    ClothesRadialCefOpened = false;
}

let closeTuningCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    alt.toggleGameControls(true);
    alt.showCursor(false);
    hudBrowser.unfocus();
    TuningMenuCefOpened = false;
    alt.emitServer("Server:Tuning:resetToNormal", curTuningVeh);
    curTuningVeh = null;
}

let closeAllCEFs = function() {
    if (hudBrowser == null) return;
    if (identityCardApplyCEFopened || TuningMenuCefOpened || BankAccountManageFormOpened || ATMcefOpened || ShopCefOpened || BarberCefOpened || GarageCefOpened || VehicleShopCefOpened || FuelStationCefOpened || ClothesShopCefOpened || bankFactionATMCefOpened || GivePlayerBillCefOpened || FactionStorageCefOpened || RecievePlayerBillCefOpened || VehicleTrunkCefOpened || VehicleLicensingCefOpened || VehicleKeyCefOpened || VehicleSellCefOpened || PlayerSearchInventoryCefOpened || GivePlayerLicenseCefOpened || MinijobPilotCefOpened || MinijobBusdriverCefOpened || HotelRentCefOpened || HouseEntranceCefOpened || HouseManageCefOpened || TownhallHouseSelectorCefOpened) {
        hudBrowser.emit("CEF:General:hideAllCEFs");
        identityCardApplyCEFopened = false,
            BankAccountManageFormOpened = false,
            ATMcefOpened = false,
            ShopCefOpened = false,
            BarberCefOpened = false,
            GarageCefOpened = false,
            VehicleShopCefOpened = false,
            FuelStationCefOpened = false,
            ClothesShopCefOpened = false,
            bankFactionATMCefOpened = false,
            GivePlayerBillCefOpened = false,
            FactionStorageCefOpened = false,
            RecievePlayerBillCefOpened = false,
            VehicleTrunkCefOpened = false,
            VehicleLicensingCefOpened = false,
            VehicleKeyCefOpened = false,
            VehicleSellCefOpened = false,
            PlayerSearchInventoryCefOpened = false,
            GivePlayerLicenseCefOpened = false,
            MinijobBusdriverCefOpened = false,
            MinijobPilotCefOpened = false,
            HotelRentCefOpened = false,
            HouseEntranceCefOpened = false,
            HouseManageCefOpened = false,
            TownhallHouseSelectorCefOpened = false,
            TuningMenuCefOpened = false,
            ClothesStorageCefOpened = false;
        alt.emitServer("Server:CEF:setCefStatus", false);
        alt.showCursor(false);
        alt.toggleGameControls(true);
        hudBrowser.unfocus();
    }
    closeInventoryCEF();
    closeTabletCEF();
}

function GetVehicleSpeed() {
    let vehicle = alt.Player.local.vehicle;
    let speed = game.getEntitySpeed(vehicle.scriptID);
    curSpeed = speed * 3.6;
}

function playAnimation(animDict, animName, animFlag, animDuration) {
    if (animDict == undefined || animName == undefined || animFlag == undefined || animDuration == undefined) return;
    game.requestAnimDict(animDict);
    let interval = alt.setInterval(() => {
        if (game.hasAnimDictLoaded(animDict)) {
            alt.clearInterval(interval);
            game.taskPlayAnim(alt.Player.local.scriptID, animDict, animName, 8.0, 1, animDuration, animFlag, 1, false, false, false);
        }
    }, 0);
}

//Tattoo Shop
alt.onServer("Client:TattooShop:openShop", (gender, shopId, ownTattoosJSON) => {
    if (hudBrowser == null || isTattooShopOpened) return;
    alt.emitServer("Server:CEF:setCefStatus", true);
    isTattooShopOpened = true;
    hudBrowser.emit("CEF:TattooShop:openShop", shopId, ownTattoosJSON);
    alt.showCursor(true);
    alt.toggleGameControls(false);
    hudBrowser.focus();
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
    if (hudBrowser == null) return;
    hudBrowser.emit("CEF:TattooShop:sendItemsToClient", items);
});

alt.everyTick(() => {
    // 6 returns true if you are equipped with any weapon except melee weapons.
    if (native.isPedArmed(alt.Player.local.scriptID, 6)) {
        native.disableControlAction(0, 140, true); // INPUT_MELEE_ATTACK_LIGHT (R button - B on controller)
        native.disableControlAction(0, 141, true); // INPUT_MELEE_ATTACK_HEAVY (Q button - A on controller)
        native.disableControlAction(0, 142, true); // INPUT_MELEE_ATTACK_ALTERNATE (LMB - RT on controller)
    }
});
//Hud Color????
const rgba = [153, 102, 255, 255]
native.replaceHudColourWithRgba(143, ...rgba)
native.replaceHudColourWithRgba(116, ...rgba)