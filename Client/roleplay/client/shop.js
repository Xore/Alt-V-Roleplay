import * as alt from 'alt';
import * as native from "natives";
import * as game from 'natives';

export let shopBrowser = null;
export let browserReady = false;
let ShopCefOpened = false;

alt.onServer("Client:HUD:CreateCEF", () => {
    if (shopBrowser == null) {
        shopBrowser = new alt.WebView("http://resource/client/cef/shop/index.html");
        shopBrowser.on("Client:Shop:closeShop", () => {
            closeShop();
        });
    }
    shopBrowser.on("Client:Shop:buyItem", (shopId, itemName, itemAmount) => {
        alt.emitServer("Server:Shop:buyItem", parseInt(shopId), itemName, parseInt(itemAmount));
    });

    shopBrowser.on("Client:Shop:sellItem", (shopId, itemName, itemAmount) => {
        alt.emitServer("Server:Shop:sellItem", parseInt(shopId), itemName, parseInt(itemAmount));
    });

    shopBrowser.on("Client:Shop:destroyShopCEF", () => {
        alt.emitServer("Player:Meta:SetCEFOpen", false);
        closeShop();
    });

    shopBrowser.on("Client:Shop:robShop", (shopId) => {
        alt.emitServer("Server:Shop:robShop", parseInt(shopId));
    });

    shopBrowser.on("Client:Shop:PayOut", (shopId) => {
        alt.emitServer("Server:Shop:PayOut", shopId);
    });

    shopBrowser.on("Client:Shop:CloseShop1", (shopId) => {
        alt.emitServer("Server:Shop:CloseShop1", shopId);
    });

    shopBrowser.on("Client:Shop:OpenShop1", (shopId) => {
        alt.emitServer("Server:Shop:OpenShop1", shopId);
    });

    shopBrowser.on("Client:Shop:depositShopItem", (shopId, itemName, itemAmount) => {
        alt.emitServer("Server:Shop:depositShopItem", parseInt(shopId), itemName, parseInt(itemAmount));
    });

    shopBrowser.on("Client:Shop:takeShopItem", (shopId, itemName, itemAmount) => {
        alt.emitServer("Server:Shop:takeShopItem", parseInt(shopId), itemName, parseInt(itemAmount));
    });

    shopBrowser.on("Client:Shop:setItemPrice", (shopId, itemName, itemPrice) => {
        alt.emitServer("Server:Shop:setItemPrice", parseInt(shopId), itemName, parseInt(itemPrice));
    });
});

//Shop
alt.onServer("Client:Shop:shopBuyCEFCreateCEF", (shopId, items, isOnlySelling) => {
    if (shopBrowser == null || ShopCefOpened) return;
    ShopCefOpened = true;
    shopBrowser.emit("CEF:Shop:openBuyShop", shopId, items, isOnlySelling);
    alt.showCursor(true);
    alt.toggleGameControls(false);
    game.triggerScreenblurFadeIn(5); //TODO
    shopBrowser.focus();
    ShopCefOpened = true;
});

alt.onServer("Client:Shop:openShopManager", (shopId, inventoryItems, shopItems, shopCash) => {
    if (shopBrowser == null || ShopCefOpened) return;
    ShopCefOpened = true;
    shopBrowser.emit("CEF:Shop:openShopManager", shopId, inventoryItems, shopItems, shopCash);
    alt.showCursor(true);
    alt.toggleGameControls(false);
    game.triggerScreenblurFadeIn(5); //TODO
    shopBrowser.focus();
    ShopCefOpened = true;
});

alt.onServer("Client:Shop:shopSellCEFCreateCEF", (shopId, items, isOnlySelling) => {
    if (shopBrowser == null || ShopCefOpened) return;
    ShopCefOpened = true;
    shopBrowser.emit("CEF:Shop:openSellShop", shopId, items, isOnlySelling);
    alt.showCursor(true);
    alt.toggleGameControls(false);
    game.triggerScreenblurFadeIn(5); //TODO
    shopBrowser.focus();
    ShopCefOpened = true;
});

let closeShop = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    alt.emitServer("Player:Meta:SetCEFOpen", false);
    game.freezeEntityPosition(game.playerPedId(), false);
    game.triggerScreenblurFadeOut(5); //TODO
    alt.showCursor(false);
    alt.toggleGameControls(true);
    shopBrowser.unfocus();
    ShopCefOpened = false;
}