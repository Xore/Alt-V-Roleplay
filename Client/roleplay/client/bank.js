import * as alt from 'alt';
import * as native from "natives";
import * as game from 'natives';

export let bankBrowser = null;
export let browserReady = false;

let BankAccountManageFormOpened = false;
let ATMcefOpened = false;
let bankFactionATMCefOpened = false;


alt.onServer("Client:HUD:CreateCEF", () => {
    if (bankBrowser == null) {
        bankBrowser = new alt.WebView("http://resource/client/cef/bank/index.html");
        bankBrowser.on("Client:HUD:cefIsReady", () => {});



        bankBrowser.on("Client:Bank:BankAccountdestroyCEF", () => {
            alt.emitServer("Player:Meta:SetCEFOpen", false);
            closeBankCEF();
        });

        bankBrowser.on("Client:Bank:BankAccountCreateNewAccount", (selectedBank) => {
            alt.emitServer("Server:Bank:CreateNewBankAccount", selectedBank);
            alt.emitServer("Player:Meta:SetCEFOpen", false);
            closeBankCEF();
        });

        bankBrowser.on("Client:Bank:BankAccountAction", (action, accountNumber) => {
            alt.emitServer("Server:Bank:BankAccountAction", action, accountNumber);
            alt.emitServer("Player:Meta:SetCEFOpen", false);
            closeBankCEF();
        });

        bankBrowser.on("Client:ATM:requestBankData", (accountNr) => {
            alt.emitServer("Server:ATM:requestBankData", accountNr);
        });

        bankBrowser.on("Client:ATM:WithdrawMoney", (accountNr, amount, zoneName) => {
            alt.emitServer("Server:ATM:WithdrawMoney", accountNr, parseInt(amount), zoneName);
        });

        bankBrowser.on("Client:ATM:DepositMoney", (accountNr, amount, zoneName) => {
            alt.emitServer("Server:ATM:DepositMoney", accountNr, parseInt(amount), zoneName);
        });

        bankBrowser.on("Client:ATM:TransferMoney", (accountNr, targetNr, amount, zoneName) => {
            alt.emitServer("Server:ATM:TransferMoney", accountNr, parseInt(targetNr), parseInt(amount), zoneName);
        });

        bankBrowser.on("Client:ATM:TryPin", (action, curATMAccountNumber) => {
            alt.emitServer("Server:ATM:TryPin", action, curATMAccountNumber);
        });

        bankBrowser.on("Client:ATM:BankATMdestroyCEF", () => {
            alt.emitServer("Player:Meta:SetCEFOpen", false);
            closeATMCEF();
        });

        bankBrowser.on("Client:FactionBank:destroyCEF", () => {
            alt.emitServer("Player:Meta:SetCEFOpen", false);
            closeBankFactionATMCEF();
        });

        bankBrowser.on("Client:FactionBank:DepositMoney", (type, factionId, amount) => {
            alt.emitServer("Server:FactionBank:DepositMoney", type, parseInt(factionId), parseInt(amount));
        });

        bankBrowser.on("Client:FactionBank:WithdrawMoney", (type, factionId, amount) => {
            alt.emitServer("Server:FactionBank:WithdrawMoney", type, parseInt(factionId), parseInt(amount));
        });
    }
});

alt.onServer("Client:Bank:createBankAccountManageForm", (bankArray, curBank) => {
    if (bankBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && BankAccountManageFormOpened == false) {
        bankBrowser.emit("CEF:Bank:createBankAccountManageForm", bankArray, curBank);
        alt.emitServer("Server:CEF:setCefStatus", true);
        alt.emitServer("Player:Meta:SetCEFOpen", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        bankBrowser.focus();
        BankAccountManageFormOpened = true;
        game.triggerScreenblurFadeIn(5); //TODO
    }
});

alt.onServer("Client:ATM:BankATMcreateCEF", (pin, accNumber, zoneName) => {
    alt.emitServer("Server:Inventory:closeCEF");
    alt.setTimeout(function() {
        if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true) return;
        bankBrowser.emit("CEF:ATM:BankATMcreateCEF", pin, accNumber, zoneName);
        alt.emitServer("Server:CEF:setCefStatus", true);
        alt.emitServer("Player:Meta:SetCEFOpen", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        bankBrowser.focus();
        ATMcefOpened = true;
        game.triggerScreenblurFadeIn(5); //TODO
    }, 500);
});

alt.onServer("Client:ATM:BankATMSetRequestedData", (curBalance, paperArray) => {
    if (bankBrowser != null && ATMcefOpened == true) {
        alt.emitServer("Player:Meta:SetCEFOpen", true);
        bankBrowser.emit("CEF:ATM:BankATMSetRequestedData", curBalance, paperArray);
    }
});

alt.onServer("Client:ATM:BankATMdestroyCEFBrowser", () => {
    if (bankBrowser != null && ATMcefOpened == true) {
        alt.emitServer("Player:Meta:SetCEFOpen", true);
        bankBrowser.emit("CEF:ATM:BankATMdestroyCEF");
    }
});

alt.onServer("Client:FactionBank:createCEF", (type, factionId, factionBalance) => {
    if (bankBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && bankFactionATMCefOpened == false) {
        if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true) return;
        bankBrowser.emit("CEF:FactionBank:createCEF", type, factionId, factionBalance);
        alt.emitServer("Server:CEF:setCefStatus", true);
        alt.emitServer("Player:Meta:SetCEFOpen", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        bankBrowser.focus();
        bankFactionATMCefOpened = true;
        game.triggerScreenblurFadeIn(5); //TODO
    }
});

let closeBankCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    alt.emitServer("Player:Meta:SetCEFOpen", false);
    game.freezeEntityPosition(game.playerPedId(), false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    bankBrowser.unfocus();
    BankAccountManageFormOpened = false;
    game.triggerScreenblurFadeOut(5); //TODO
}

let closeATMCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    alt.emitServer("Player:Meta:SetCEFOpen", false);
    game.freezeEntityPosition(game.playerPedId(), false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    bankBrowser.unfocus();
    ATMcefOpened = false;
    game.triggerScreenblurFadeOut(5); //TODO
}

let closeBankFactionATMCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    alt.emitServer("Player:Meta:SetCEFOpen", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    bankBrowser.unfocus();
    bankFactionATMCefOpened = false;
    game.triggerScreenblurFadeOut(5); //TODO
}