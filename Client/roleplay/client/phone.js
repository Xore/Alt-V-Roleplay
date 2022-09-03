import * as alt from 'alt';
import * as native from "natives";
import * as game from 'natives';
import { Player, Vector3 } from "alt";
export let phoneBrowser = null;
export let browserReady = false;
let isPlayerDead = false;
let isPhoneEquipped = false;
let currentRadioFrequence = null;
alt.onServer("Client:HUD:CreateCEF", () => {
    if (phoneBrowser == null) {
        phoneBrowser = new alt.WebView("http://resource/client/cef/phone/index.html");
        phoneBrowser.on("Client:HUD:cefIsReady", () => {
            alt.setTimeout(function() {
                browserReady = true;
            }, 1000);
        });
        phoneBrowser.on("Client:Smartphone:joinRadioFrequence", (currentRadioFrequence) => {
            alt.emitServer("Server:Smartphone:joinRadioFrequence", `${currentRadioFrequence}`);
        });
        phoneBrowser.on("Client:Smartphone:leaveRadioFrequence", () => {
            if (currentRadioFrequence == null) return;
            alt.emitServer("Server:Smartphone:leaveRadioFrequence");
        });
        /* Smartphone */
        phoneBrowser.on("Client:Smartphone:tryCall", (number) => {
            alt.emitServer("Server:Smartphone:tryCall", parseInt(number));
        });
        phoneBrowser.on("Client:Smartphone:denyCall", () => {
            alt.emitServer("Server:Smartphone:denyCall");
        });
        phoneBrowser.on("Client:Smartphone:acceptCall", () => {
            alt.emitServer("Server:Smartphone:acceptCall");
        });
        phoneBrowser.on("Client:Smartphone:requestChats", () => {
            alt.emitServer("Server:Smartphone:requestChats");
        });
        phoneBrowser.on("Client:Smartphone:requestChatMessages", (chatId) => {
            alt.emitServer("Server:Smartphone:requestChatMessages", parseInt(chatId));
        });
        phoneBrowser.on("Client:Smartphone:createNewChat", (targetNumber) => {
            alt.emitServer("Server:Smartphone:createNewChat", parseInt(targetNumber));
        });
        phoneBrowser.on("Client:Smartphone:sendChatMessage", (selectedChatId, userPhoneNumber, targetMessageUser, unix, encodedText) => {
            if (selectedChatId <= 0 || userPhoneNumber <= 0 || targetMessageUser <= 0) return;
            alt.emitServer("Server:Smartphone:sendChatMessage", parseInt(selectedChatId), parseInt(userPhoneNumber), parseInt(targetMessageUser), parseInt(unix), encodedText);
        });
        phoneBrowser.on("Client:Smartphone:deleteChat", (chatId) => {
            alt.emitServer("Server:Smartphone:deleteChat", parseInt(chatId));
        });
        phoneBrowser.on("Client:Smartphone:setFlyModeEnabled", (isEnabled) => {
            alt.emitServer("Server:Smartphone:setFlyModeEnabled", isEnabled);
        });
        phoneBrowser.on("Client:Smartphone:requestPhoneContacts", () => {
            alt.emitServer("Server:Smartphone:requestPhoneContacts");
        });
        phoneBrowser.on("Client:Smartphone:requestPhoneVerlauf", () => {
            alt.emitServer("Server:Smartphone:requestPhoneVerlauf");
        });
        phoneBrowser.on("Client:Smartphone:deleteContact", (contactId) => {
            alt.emitServer("Server:Smartphone:deleteContact", parseInt(contactId));
        });
        phoneBrowser.on("Client:Smartphone:addNewContact", (name, number) => {
            alt.emitServer("Server:Smartphone:addNewContact", name, parseInt(number));
        });
        phoneBrowser.on("Client:Smartphone:editContact", (id, name, number) => {
            alt.emitServer("Server:Smartphone:editContact", parseInt(id), name, parseInt(number));
        });
        phoneBrowser.on("Client:Smartphone:SearchLSPDIntranetPeople", (name) => {
            alt.emitServer("Server:Smartphone:SearchLSPDIntranetPeople", name);
        });
        phoneBrowser.on("Client:Smartphone:GiveLSPDIntranetWanteds", (selectedCharId, wantedList) => {
            alt.emitServer("Server:Smartphone:GiveLSPDIntranetWanteds", parseInt(selectedCharId), wantedList);
        });
        phoneBrowser.on("Client:Smartphone:requestLSPDIntranetPersonWanteds", (charid) => {
            alt.emitServer("Server:Smartphone:requestLSPDIntranetPersonWanteds", parseInt(charid));
        });
        phoneBrowser.on("Client:Smartphone:DeleteLSPDIntranetWanted", (id, charid) => {
            alt.emitServer("Server:Smartphone:DeleteLSPDIntranetWanted", parseInt(id), parseInt(charid));
        });
        phoneBrowser.on("Client:Smartphone:requestPoliceAppMostWanteds", () => {
            alt.emitServer("Server:Smartphone:requestPoliceAppMostWanteds");
        });
        phoneBrowser.on("Client:Smartphone:locateMostWanted", (X, Y) => {
            game.setNewWaypoint(parseFloat(X), parseFloat(Y));
        });
        phoneBrowser.on("Client:Smartphone:setWallpaperId", (wallpaperId) => {
            alt.emitServer("Server:Smartphone:setWallpaperId", `${wallpaperId}`);
        });
        phoneBrowser.on("Client:Smartphone:requestPhonebusiness", () => {
            alt.emitServer("Server:Smartphone:requestPhonebusiness");
        });
    }
});
alt.onServer("Client:Smartphone:setCurrentFunkFrequence", (funkfrequence) => {
    if (funkfrequence == null || funkfrequence == "null") {
        currentRadioFrequence = null;
        return;
    }
    currentRadioFrequence = funkfrequence;
});
alt.onServer("Client:Smartphone:equipPhone", (isEquipped, phoneNumber, isFlyModeEnabled, wallpaperId) => {
    let interval = alt.setInterval(() => {
        if (phoneBrowser != null && browserReady) {
            alt.clearInterval(interval);
            phoneBrowser.emit("CEF:Smartphone:equipPhone", isEquipped, phoneNumber, isFlyModeEnabled, wallpaperId);
            isPhoneEquipped = isEquipped;
        }
    }, 0);
});
alt.onServer("Client:Smartphone:showPhoneReceiveCall", (number) => {
    if (phoneBrowser == null || !browserReady) return;
    phoneBrowser.emit("CEF:Smartphone:showPhoneReceiveCall", number);
});
alt.onServer("Client:Smartphone:showPhoneCallActive", (number) => {
    if (phoneBrowser == null || !browserReady) return;
    phoneBrowser.emit("CEF:Smartphone:showPhoneCallActive", number);
});
alt.onServer("Client:Smartphone:addChatJSON", (chats) => {
    if (phoneBrowser == null || !browserReady) return;
    phoneBrowser.emit("CEF:Smartphone:addChatJSON", chats);
});
alt.onServer("Client:Smartphone:addMessageJSON", (msg) => {
    if (phoneBrowser == null || !browserReady) return;
    phoneBrowser.emit("CEF:Smartphone:addMessageJSON", msg);
});
alt.onServer("Client:Smartphone:setAllMessages", () => {
    if (phoneBrowser == null || !browserReady) return;
    phoneBrowser.emit("CEF:Smartphone:setAllMessages");
});
alt.onServer("Client:Smartphone:setAllChats", () => {
    if (phoneBrowser == null || !browserReady) return;
    phoneBrowser.emit("CEF:Smartphone:setAllChats");
});
alt.onServer("Client:Smartphone:recieveNewMessage", (chatId, phoneNumber, message) => {
    if (phoneBrowser == null || !browserReady) return;
    phoneBrowser.emit("CEF:Smartphone:recieveNewMessage", chatId, phoneNumber, message);
});
alt.onServer("Client:Smartphone:ShowPhoneCallError", (errorId) => {
    if (phoneBrowser == null || !browserReady) return;
    phoneBrowser.emit("CEF:Smartphone:ShowPhoneCallError", errorId);
});
alt.onServer("Client:Smartphone:addContactJSON", (json) => {
    if (phoneBrowser == null && !browserReady) return;
    phoneBrowser.emit("CEF:Smartphone:addContactJSON", json);
});
alt.onServer("Client:Smartphone:setAllContacts", () => {
    if (phoneBrowser == null || !browserReady) return;
    phoneBrowser.emit("CEF:Smartphone:setAllContacts");
});
alt.onServer("Client:Smartphone:ShowLSPDIntranetApp", (shouldBeVisible, serverWanteds) => {
    let interval = alt.setInterval(() => {
        if (phoneBrowser != null && browserReady) {
            alt.clearInterval(interval);
            phoneBrowser.emit("CEF:Smartphone:ShowLSPDIntranetApp", shouldBeVisible, serverWanteds);
        }
    }, 1000);
});
alt.onServer("Client:Smartphone:SetLSPDIntranetSearchedPeople", (searchedPersonsJSON) => {
    if (phoneBrowser == null || !browserReady) return;
    phoneBrowser.emit("CEF:Smartphone:SetLSPDIntranetSearchedPeople", searchedPersonsJSON);
});
alt.onServer("Client:Smartphone:setLSPDIntranetPersonWanteds", (json) => {
    if (phoneBrowser == null || !browserReady) return;
    phoneBrowser.emit("CEF:Smartphone:setLSPDIntranetPersonWanteds", json);
});
alt.onServer("Client:Smartphone:setPoliceAppMostWanteds", (mostWanteds) => {
    if (phoneBrowser == null || !browserReady) return;
    phoneBrowser.emit("CEF:Smartphone:setPoliceAppMostWanteds", mostWanteds);
});
alt.onServer("Client:Smartphone:showNotification", (message, app, fn, sound) => {
    if (phoneBrowser == null || !browserReady) return;
    phoneBrowser.emit("CEF:Smartphone:showNotification", message, app, fn, sound);
});
alt.onServer("Client:Smartphone:addVerlaufJSON", (json) => {
    if (phoneBrowser == null && !browserReady) return;
    phoneBrowser.emit("CEF:Smartphone:addVerlaufJSON", json);
});
alt.onServer("Client:Smartphone:setAllVerlauf", () => {
    if (phoneBrowser == null || !browserReady) return;
    phoneBrowser.emit("CEF:Smartphone:setAllVerlauf");
});
alt.onServer("Client:Smartphone:addbusinessJSON", (json) => {
    if (phoneBrowser == null && !browserReady) return;
    phoneBrowser.emit("CEF:Smartphone:addbusinessJSON", json);
});
alt.onServer("Client:Smartphone:setAllbusiness", () => {
    if (phoneBrowser == null || !browserReady) return;
    phoneBrowser.emit("CEF:Smartphone:setAllbusiness");
});
alt.on("Client:Phone:Key33", () => {
        //Smartphone Bild hoch
        if (phoneBrowser == null || !browserReady || isPlayerDead || !isPhoneEquipped || alt.Player.local.getSyncedMeta("IsCefOpen") == true || alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true) return;
        phoneBrowser.emit("CEF:Smartphone:togglePhone", true);
        playAnimation("cellphone@in_car@ds", "cellphone_text_read_base", 49, -1);
        alt.emit('objectAttacher:attachObject', 'phone');
        alt.showCursor(true);
        alt.emitServer("Server:CEF:setCefStatus", true);
        phoneBrowser.on("Client:Tablet:sendDispatchToFaction", sendDispatchToFaction);
        alt.toggleGameControls(false);
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
        phoneBrowser.focus();
});
alt.on("Client:Phone:Key34", () => {
        //Smartphone Bild runter
        if (phoneBrowser == null || !browserReady || !isPhoneEquipped) return;
        if (alt.Player.local.getSyncedMeta("HasHandcuffs") == false && alt.Player.local.getSyncedMeta("HasRopeCuffs") == false) game.clearPedTasks(alt.Player.local.scriptID);
        phoneBrowser.emit("CEF:Smartphone:togglePhone", false);
        alt.emit('objectAttacher:detachObject');
        alt.emitServer("Server:CEF:setCefStatus", false);
        phoneBrowser.off("Client:Tablet:sendDispatchToFaction", sendDispatchToFaction);
        alt.showCursor(false);
        alt.toggleGameControls(true);
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
        phoneBrowser.unfocus();
});
alt.on("Client:Phone:KeyN", () => {
        if (currentRadioFrequence == null || currentRadioFrequence == undefined || alt.Player.local.getSyncedMeta("IsCefOpen")) return;
        alt.emit("SaltyChat:UseRadio", true, false);
});
alt.on("Client:Phone:KeyNDown", () => {
        if (currentRadioFrequence == null || currentRadioFrequence == undefined || alt.Player.local.getSyncedMeta("IsCefOpen")) return;
        alt.emit("SaltyChat:UseRadio", true, true);
});
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

function sendDispatchToFaction(factionId, msg) {
    if (!isPhoneEquipped) return;
    if (factionId <= 0 || msg == undefined || msg == "") return;
    alt.emitServer("Server:Tablet:sendDispatchToFaction", parseInt(factionId), msg);
}