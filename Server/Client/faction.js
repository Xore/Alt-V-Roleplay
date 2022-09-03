import * as alt from 'alt';
import * as native from "natives";
import * as game from 'natives';

export let factionBrowser = null;
export let browserReady = false;
let LSPDCarCefOpened = false;
let lastInteract = 0;

alt.onServer("Client:HUD:CreateCEF", () => {
    if (factionBrowser == null) {
        factionBrowser = new alt.WebView("http://resource/client/cef/faction/index.html");
        factionBrowser.on("Client:HUD:cefIsReady", () => {
            alt.setTimeout(function() {
                browserReady = true;
            }, 1000);
        });

        factionBrowser.on("Client:LSPD:SelectJob", (level) => {
            alt.emitServer("Server:LSPD:StartJob", parseInt(level));
        });

        factionBrowser.on("Client:LSPD:SelectGear", (level) => {
            alt.emitServer("Server:LSPD:SelectGear", parseInt(level));
        });

        factionBrowser.on("Client:LSPD:destroyCEF", () => {
            closeLSPDCarCEF();
        });

        factionBrowser.on("Client:LSPD:destroyGearCEF", () => {
            destroyGearCEF();
        });
    }
});

alt.onServer("Client:LSPD:openCEF", () => {
    if (factionBrowser != null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && LSPDCarCefOpened == false) {
        factionBrowser.emit("CEF:LSPD:openCEF");
        alt.emitServer("Server:CEF:setCefStatus", true);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        factionBrowser.focus();
        LSPDCarCefOpened = true;
    }
});

let closeLSPDCarCEF = function() {
    alt.emitServer("Server:CEF:setCefStatus", false);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    alt.showCursor(false);
    alt.toggleGameControls(true);
    factionBrowser.unfocus();
    LSPDCarCefOpened = false;
}