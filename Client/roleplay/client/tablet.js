import * as alt from 'alt';
import * as game from 'natives';

let tabletBrowser = null;
let tabletReady = false;


alt.on("Client:Tablet:Key0x73", () => {
        if (tabletBrowser == null) {
            alt.emitServer("Server:Tablet:openCEF");
            alt.emit('objectAttacher:attachObjectAnimated', 'tablet', true);
        } else {
            closeTabletCEF();
            alt.emit('objectAttacher:detachObject');
        }
});

let tablet = null;

alt.onServer('Client:Tablet:createCEF', () => {
    openTabletCEF();
});

alt.onServer('Client:Tablet:finaly', () => {
    if (tabletBrowser != null) {
        let interval = alt.setInterval(() => {
            if (tabletReady) {
                alt.clearInterval(interval);
                tabletBrowser.emit("CEF:Tablet:openCEF");
            }
        }, 0);
    }
});

alt.onServer("Client:Tablet:setTabletHomeAppData", (array) => {
    if (tabletBrowser != null) {
        tabletBrowser.emit("CEF:Tablet:SetInternetAppAppStoreContent", array);
    }
});

alt.onServer("Client:Tablet:SetBankingAppContent", (bankArray, historyArray) => {
    if (tabletBrowser != null) {
        tabletBrowser.emit("CEF:Tablet:SetBankingAppContent", bankArray, historyArray);
    }
});

alt.onServer("Client:Tablet:SetEventsAppContent", (array) => {
    if (tabletBrowser != null) {
        tabletBrowser.emit("CEF:Tablet:SetEventsAppEventEntrys", array);
    }
});

alt.onServer("Client:Tablet:NotesAppAddNotesContent", (array) => {
    if (tabletBrowser != null) {
        tabletBrowser.emit("CEF:Tablet:NotesAppAddNotesContent", array);
    }
});

alt.onServer("Client:Tablet:SetVehiclesAppContent", (array) => {
    if (tabletBrowser != null) {
        tabletBrowser.emit("CEF:Tablet:SetVehiclesAppContent", array);
    }
});

alt.onServer("Client:Tablet:SetShopsContent", (array) => {
    if (tabletBrowser != null) {
        tabletBrowser.emit("CEF:Tablet:SetShopsContent", array);
    }
});

alt.onServer("Client:Tablet:SetVehicleStoreAppContent", (array) => {
    if (tabletBrowser != null) {
        tabletBrowser.emit("CEF:Tablet:SetVehicleStoreAppContent", array);
    }
});

alt.onServer("Client:Tablet:SetFactionManagerAppContent", (factionId, infoArray, memberArray, rankArray) => {
    if (tabletBrowser != null) {
        tabletBrowser.emit("CEF:Tablet:SetFactionManagerAppContent", factionId, infoArray, memberArray, rankArray);
    }
});

alt.onServer("Client:Tablet:SetGangManagerAppContent", (gangId, infoArray, memberArray, rankArray) => {
    if (tabletBrowser != null) {
        tabletBrowser.emit("CEF:Tablet:SetGangManagerAppContent", gangId, infoArray, memberArray, rankArray);
    }
});

alt.onServer("Client:Tablet:SetFactionAppContent", (dutyMemberCount, dispatchCount, vehicleArray) => {
    if (tabletBrowser != null) {
        tabletBrowser.emit("CEF:Tablet:SetFactionAppContent", dutyMemberCount, dispatchCount, vehicleArray);
    }
});

alt.onServer("Client:Tablet:SetLSPDAppPersonSearchData", (charName, gender, birthdate, birthplace, address, job, mainBankAcc, firstJoinDate) => {
    if (tabletBrowser != null) {
        tabletBrowser.emit("CEF:Tablet:SetLSPDAppPersonSearchData", charName, gender, birthdate, birthplace, address, job, mainBankAcc, firstJoinDate);
    }
});

alt.onServer("Client:Tablet:SetLSPDAppSearchVehiclePlateData", (owner, name, manufactor, buydate, trunk, maxfuel, tax, fueltype) => {
    if (tabletBrowser != null) {
        tabletBrowser.emit("CEF:Tablet:SetLSPDAppSearchVehiclePlateData", owner, name, manufactor, buydate, trunk, maxfuel, tax, fueltype);
    }
});

alt.onServer("Client:Tablet:SetLSPDAppLicenseSearchData", (charName, licArray) => {
    if (tabletBrowser != null) {
        tabletBrowser.emit("CEF:Tablet:SetLSPDAppLicenseSearchData", charName, licArray);
    }
});

alt.onServer("Client:Tablet:SetJusticeAppSearchedBankAccounts", (accountArray) => {
    if (tabletBrowser != null) {
        tabletBrowser.emit("CEF:Tablet:SetJusticeAppSearchedBankAccounts", accountArray);
    }
});

alt.onServer("Client:Tablet:SetJusticeAppBankTransactions", (array) => {
    if (tabletBrowser != null) {
        tabletBrowser.emit("CEF:Tablet:SetJusticeAppBankTransactions", array);
    }
});

alt.onServer("Client:Tablet:setDispatches", (factionId, dispatchArray) => {
    if (tabletBrowser != null) {
        tabletBrowser.emit("CEF:Tablet:SetDispatches", factionId, dispatchArray);
    }
});

alt.onServer("Client:Tablet:SetTutorialAppContent", (array) => {
    if (tabletBrowser != null) {
        tabletBrowser.emit("CEF:Tablet:SetTutorialAppContent", array);
    }
});

alt.onServer("Client:Tablet:sendDispatchSound", (filePath) => {
    if (tabletBrowser != null) {
        tabletBrowser.emit("CEF:Tablet:playDispatchSound", filePath);
    }
})

alt.onServer('Client:Tablet:closeCEF', () => {
    closeTabletCEF();
});

let openTabletCEF = function() {
    if (tabletBrowser == null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && alt.Player.local.getSyncedMeta("PLAYER_SPAWNED") == true) {
        alt.showCursor(true);
        alt.toggleGameControls(false);
        tabletBrowser = new alt.WebView("http://resource/client/cef/tablet/index.html");
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
        tabletBrowser.focus();
        alt.emitServer("Server:CEF:setCefStatus", true);
        tabletBrowser.on("Client:Tablet:cefIsReady", () => {
            tabletReady = true;
            alt.emitServer("Server:Tablet:RequestTabletData");
        });

        tabletBrowser.on("Client:Tablet:AppStoreInstallUninstallApp", AppStoreInstallUninstallApp);
        tabletBrowser.on("Client:Tablet:BankingAppnewTransaction", BankingAppnewTransaction);
        tabletBrowser.on("Client:Tablet:EventsAppNewEntry", EventsAppNewEntry);
        tabletBrowser.on("Client:Tablet:NotesAppNewNote", NotesAppNewNote);
        tabletBrowser.on("Client:Tablet:NotesAppDeleteNote", NotesAppDeleteNote);
        tabletBrowser.on("Client:Tablet:LocateVehicle", LocateTabletVehicle);
        tabletBrowser.on("Client:Tablet:VehicleStoreBuyVehicle", VehicleStoreBuyVehicle);
        tabletBrowser.on("Client:Tablet:FactionManagerAppInviteNewMember", FactionManagerAppInviteNewMember);
        tabletBrowser.on("Client:Tablet:FactionManagerRankAction", FactionManagerRankAction);
        tabletBrowser.on("Client:Tablet:FactionManagerSetRankPaycheck", FactionManagerSetRankPaycheck);
        tabletBrowser.on("Client:Tablet:GangManagerAppInviteNewMember", GangManagerAppInviteNewMember);
        tabletBrowser.on("Client:Tablet:GangManagerRankAction", GangManagerRankAction);
        tabletBrowser.on("Client:Tablet:LSPDAppSearchPerson", LSPDAppSearchPerson);
        tabletBrowser.on("Client:Tablet:LSPDAppSearchVehiclePlate", LSPDAppSearchVehiclePlate);
        tabletBrowser.on("Client:Tablet:LSPDAppSearchLicense", LSPDAppSearchLicense);
        tabletBrowser.on("Client:Tablet:LSPDAppTakeLicense", LSPDAppTakeLicense);
        tabletBrowser.on("Client:Tablet:JusticeAppGiveWeaponLicense", JusticeAppGiveWeaponLicense);
        tabletBrowser.on("Client:Tablet:JusticeAppSearchBankAccounts", JusticeAppSearchBankAccounts);
        tabletBrowser.on("Client:Tablet:JusticeAppViewBankTransactions", JusticeAppViewBankTransactions);
        tabletBrowser.on("Client:Tablet:sendDispatchToFaction", sendDispatchToFaction);
        tabletBrowser.on("Client:Tablet:DeleteFactionDispatch", DeleteFactionDispatch);
    }
}

function DeleteFactionDispatch(factionId, senderId) {
    if (factionId <= 0 || senderId < 0) return;
    alt.emitServer("Server:Tablet:DeleteFactionDispatch", parseInt(factionId), parseInt(senderId));
}

function AppStoreInstallUninstallApp(action, appname) {
    if (action != "install" && action != "uninstall") return;
    if (appname == "" || appname == "undefined") return;
    let isInstalling = false;
    if (action == "install") { isInstalling = true; } else if (action == "uninstall") { isInstalling = false; }
    alt.emitServer("Server:Tablet:AppStoreInstallUninstallApp", appname, isInstalling);
}

function BankingAppnewTransaction(targetBankNumber, transactiontext, moneyAmount) {
    alt.emitServer("Server:Tablet:BankingAppNewTransaction", parseInt(targetBankNumber), transactiontext, parseInt(moneyAmount));
}

function EventsAppNewEntry(title, callNumber, eventDate, Time, location, eventType, information) {
    alt.emitServer("Server:Tablet:EventsAppNewEntry", title, callNumber, eventDate, Time, location, eventType, information);
}

function NotesAppNewNote(title, text, color) {
    alt.emitServer("Server:Tablet:NotesAppNewNote", title, text, color);
}

function NotesAppDeleteNote(noteId) {
    alt.emitServer("Server:Tablet:NotesAppDeleteNote", parseInt(noteId));
}

function LocateTabletVehicle(x, y) {
    if (x == null || y == null || x == undefined || y == undefined) return;
    game.setNewWaypoint(x, y);
}

function VehicleStoreBuyVehicle(hash, shopId, color) {
    alt.emitServer("Server:Tablet:VehicleStoreBuyVehicle", hash, parseInt(shopId), color);
}

function FactionManagerAppInviteNewMember(charName, dienstnummer, factionId) {
    if (charName == "" || dienstnummer <= 0 || factionId <= 0 || dienstnummer == null || dienstnummer == undefined || factionId == undefined || factionId == null) return;
    alt.emitServer("Server:Tablet:FactionManagerAppInviteNewMember", charName, parseInt(dienstnummer), parseInt(factionId));
}

function GangManagerAppInviteNewMember(charName, gangId) {
    if (charName == "" || gangId <= 0 || gangId == undefined || gangId == null) return;
    alt.emitServer("Server:Tablet:GangManagerAppInviteNewMember", charName, parseInt(gangId));
}

function FactionManagerRankAction(action, charId) {
    if (action != "rankup" && action != "rankdown" && action != "remove") return;
    if (charId <= 0 || charId == undefined) return;
    alt.emitServer("Server:Tablet:FactionManagerRankAction", action, parseInt(charId));
}

function GangManagerRankAction(action, charId) {
    if (action != "rankup" && action != "rankdown" && action != "remove") return;
    if (charId <= 0 || charId == undefined) return;
    alt.emitServer("Server:Tablet:GangManagerRankAction", action, parseInt(charId));
}

function FactionManagerSetRankPaycheck(rankId, paycheck) {
    if (rankId <= 0 || paycheck <= 0) return;
    alt.emitServer("Server:Tablet:FactionManagerSetRankPaycheck", parseInt(rankId), parseInt(paycheck));
}

function LSPDAppSearchPerson(charName) {
    if (charName.length <= 0 || charName == "") return;
    alt.emitServer("Server:Tablet:LSPDAppSearchPerson", charName);
}

function LSPDAppSearchVehiclePlate(plate) {
    if (plate.length <= 0 || plate == "") return;
    alt.emitServer("Server:Tablet:LSPDAppSearchVehiclePlate", plate);
}

function LSPDAppSearchLicense(charName) {
    if (charName.length <= 0 || charName == "") return;
    alt.emitServer("Server:Tablet:LSPDAppSearchLicense", charName);
}

function LSPDAppTakeLicense(charName, lic) {
    if (charName.length <= 0 || charName == "" || lic == "" || lic.length <= 0) return;
    alt.emitServer("Server:Tablet:LSPDAppTakeLicense", charName, lic);
}

function JusticeAppGiveWeaponLicense(charName) {
    if (charName.length <= 0) return;
    alt.emitServer("Server:Tablet:JusticeAppGiveWeaponLicense", charName);
}

function JusticeAppSearchBankAccounts(charName) {
    if (charName.length <= 0) return;
    alt.emitServer("Server:Tablet:JusticeAppSearchBankAccounts", charName);
}

function JusticeAppViewBankTransactions(accNumber) {
    if (accNumber.length <= 0) return;
    alt.emitServer("Server:Tablet:JusticeAppViewBankTransactions", parseInt(accNumber));
}

function sendDispatchToFaction(factionId, msg) {
    if (factionId <= 0 || msg == undefined || msg == "") return;
    alt.emitServer("Server:Tablet:sendDispatchToFaction", parseInt(factionId), msg);
}

export function closeTabletCEF() {
    if (tabletBrowser != null) {
        alt.emitServer("Server:CEF:setCefStatus", false);
        tabletBrowser.off("Client:Tablet:AppStoreInstallUninstallApp", AppStoreInstallUninstallApp);
        tabletBrowser.off("Client:Tablet:BankingAppnewTransaction", BankingAppnewTransaction);
        tabletBrowser.off("Client:Tablet:EventsAppNewEntry", EventsAppNewEntry);
        tabletBrowser.off("Client:Tablet:NotesAppNewNote", NotesAppNewNote);
        tabletBrowser.off("Client:Tablet:NotesAppDeleteNote", NotesAppDeleteNote);
        tabletBrowser.off("Client:Tablet:LocateVehicle", LocateTabletVehicle);
        tabletBrowser.off("Client:Tablet:VehicleStoreBuyVehicle", VehicleStoreBuyVehicle);
        tabletBrowser.off("Client:Tablet:FactionManagerAppInviteNewMember", FactionManagerAppInviteNewMember);
        tabletBrowser.off("Client:Tablet:FactionManagerRankAction", FactionManagerRankAction);
        tabletBrowser.off("Client:Tablet:FactionManagerSetRankPaycheck", FactionManagerSetRankPaycheck);
        tabletBrowser.off("Client:Tablet:GangManagerAppInviteNewMember", GangManagerAppInviteNewMember);
        tabletBrowser.off("Client:Tablet:GangManagerRankAction", GangManagerRankAction);
        tabletBrowser.off("Client:Tablet:LSPDAppSearchPerson", LSPDAppSearchPerson);
        tabletBrowser.off("Client:Tablet:LSPDAppSearchVehiclePlate", LSPDAppSearchVehiclePlate);
        tabletBrowser.off("Client:Tablet:LSPDAppSearchLicense", LSPDAppSearchLicense);
        tabletBrowser.off("Client:Tablet:LSPDAppTakeLicense", LSPDAppTakeLicense);
        tabletBrowser.off("Client:Tablet:JusticeAppGiveWeaponLicense", JusticeAppGiveWeaponLicense);
        tabletBrowser.off("Client:Tablet:JusticeAppSearchBankAccounts", JusticeAppSearchBankAccounts);
        tabletBrowser.off("Client:Tablet:JusticeAppViewBankTransactions", JusticeAppViewBankTransactions);
        tabletBrowser.off("Client:Tablet:sendDispatchToFaction", sendDispatchToFaction);
        tabletBrowser.off("Client:Tablet:DeleteFactionDispatch", DeleteFactionDispatch);
        tabletBrowser.unfocus();
        tabletBrowser.destroy();
        tabletBrowser = null;
        alt.showCursor(false);
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
        alt.toggleGameControls(true);
    }

    tabletReady = false;
    game.clearPedTasks(alt.Player.local.scriptID);
    if (!tablet || tablet == null) return;
    alt.setTimeout(() => {
        game.detachEntity(tablet, true, false);
        game.deleteObject(tablet);
        tablet = null;
    }, 800);
}