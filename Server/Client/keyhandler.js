import * as alt from 'alt';
import * as game from 'natives';
import * as Keybinds from "altv-os-keybinds";
import Fingerpointing from './fingerpoint.js';
var canUseEKey = true;
var lastInteract = 0;
let toggleCrouch = false;
let pointing = new Fingerpointing();

function canInteract() { return lastInteract + 1000 < Date.now() }

Keybinds.registerKeybind("B", true, 0, () => { if (alt.Player.local.getSyncedMeta("IsCefOpen") == false) { pointing.stop(); } }, () => { if (alt.Player.local.getSyncedMeta("IsCefOpen") == false) { pointing.start(); } });

alt.on('keyup', (key) => {
    if (!canInteract && alt.Player.local.getSyncedMeta("IsCefOpen") == false) return;
    if (key == 'E'.charCodeAt(0) && alt.Player.local.getSyncedMeta("IsCefOpen") == false) {
        alt.emitServer("Server:KeyHandler:PressE");
    } else if (key == 'E'.charCodeAt(0) && alt.Player.local.getSyncedMeta("IsCefOpen") == false) {
        alt.emitServer("Server:KeyHandler:PressE");
    } else if (key == 'U'.charCodeAt(0) && alt.Player.local.getSyncedMeta("IsCefOpen") == false) {
        alt.emitServer("Server:KeyHandler:PressU");
    } else if (key === 117 && alt.Player.local.getSyncedMeta("IsCefOpen") == false) {
        alt.emitServer("Server:KeyHandler:PressF9");
    } else if (key === 90 && alt.Player.local.getSyncedMeta("IsCefOpen") == false) {
        alt.emitServer("Server:KeyHandler:PressZ");
    } else if (key === 222 && alt.Player.local.getSyncedMeta("IsCefOpen") == false) { //<-- Ä
        alt.emitServer("Server:KeyHandler:PressRagdoll");
    } else if (key === 192 && alt.Player.local.getSyncedMeta("IsCefOpen") == false) { //<-- Ö
        alt.emitServer("Server:KeyHandler:PressRagdoll2");
    }
});

alt.on('keydown', (key) => {
    if (key === 17 && alt.Player.local.getSyncedMeta("IsCefOpen") == false) { //STRG
        game.disableControlAction(0, 36, true);
        if (!game.isPlayerDead(alt.Player.local) && !game.isPedSittingInAnyVehicle(alt.Player.local.scriptID)) {
            if (!game.isPauseMenuActive()) {

                game.requestAnimSet("move_ped_crouched");
                if (!toggleCrouch) {
                    game.setPedMovementClipset(alt.Player.local.scriptID, "move_ped_crouched", 0.45);
                    toggleCrouch = true;
                } else {
                    game.clearPedTasks(alt.Player.local.scriptID);
                    game.resetPedMovementClipset(alt.Player.local.scriptID, 0.45);
                    toggleCrouch = false;
                }
            }
        }
    }
});

alt.onServer("Client:DoorManager:ManageDoor", (hash, pos, isLocked) => {
    if (hash != undefined && pos != undefined && isLocked != undefined) {
        game.setStateOfClosestDoorOfType(game.getHashKey(hash), pos.x, pos.y, pos.z, isLocked, 0, 0);
    }
});