import * as alt from 'alt';
import * as game from 'natives';
let lastInteract = 0;
let toggleCrouch = false;


// Gamepad Controls (XBOX 360)
var SeatbeltC;
var EngineC;
var CycleSirensC;
var ToggleEmergencyC;
var ToggleSirensC;


// Keyboard UP Controls
var CycleSirenUpK = 14;
var ToggleSirensUpK = 18;
var Phone33UpK = 33;
var Phone34UpK = 34;
var FingerPointUpK = 66;
var UseAndHonkUpK = 69;
var HandsupKeyUp = 72;
var InventoryUpK = 73;
var KleidungUpK = 75;
var RadioChatterUpK = 78;
var TuningNUpK = 78;
var ToggleLights = 81;
var UHandlerUpK = 85;
var InteractionUpK = 88;
var MegaphoneUpK = 90;
var ClearAnimationUpK = 96;
var AnimationMenuUpK = 113;
var Tablet73UpK = 115;
var F9HandlerUpK = 117;
var Ragdoll2UpK = 192;
var Ragdoll1UpK = 222;


// Keyboard DOWN Controls
var DuckHandlerDownK = 17;
var FingerPointDownK = 66;
var UseAndHonkUpK = 69;
var GKeyDownK = 71;
var Key72DownK = 72;
var RadioChatterDownK = 78;
var TuningDownK = 78;
var XKeyDownK = 88;
var KKeyDownK = 75;
var ZKeyDownK = 90;
var VoiceRangeDownK = 220;


 // alt.everyTick(() => {
	 // if (alt.Player.local.vehicle === null) { return; }
	 // game.disableControlAction(0, 79, false);	
	 // game.disableControlAction(0, 80, false);	
	 // // R3
	 // if(game.isDisabledControlJustReleased(0, 79)){
		 // if (!canInteract()) return;
		 // alt.emitServer("Server:Vehicle:ToggleSeatbelt");
	 // }	
	 // // B 
	 // if(game.isDisabledControlJustReleased(0, 80)){
		 // if (!canInteract()) return;
		 // alt.emitServer("Server:Raycast:ToggleVehicleEngine", alt.Player.local.vehicle);
	 // }	
// });

alt.everyTick(() => {
	 if (alt.Player.local.vehicle === null) { return; }
	 if (game.getVehicleClass(alt.Player.local.vehicle.scriptID) != 18) { return; } // Emergency cars
	 game.disableControlAction(0, CycleSirenUpK, false);
	 if(game.isDisabledControlJustReleased(0, CycleSirenUpK)){
		alt.emit("Client:Sirens:14Pressed");
	 }
	 // game.disableControlAction(0, 85, false);	
	 // game.disableControlAction(0, 85, false);	
	 // //DPAD LEFT CYCLE SIRENS
	 // if(game.isDisabledControlJustReleased(0, 85)){
		 // alt.emit("Client:Sirens:14Pressed");
	 // }
	 // // LT
	 // if(game.isDisabledControlJustReleased(0, 77)){
		 // alt.emit("Client:Sirens:toggleSirens");	
	 // }
});

alt.on('keyup', (key) => {
    if (alt.Player.local.getSyncedMeta("HasHandcuffs") === true || alt.Player.local.getSyncedMeta("HasRopeCuffs") === true || alt.Player.local.getSyncedMeta("IsUnconscious") === true) { return; }
	if(alt.Player.local.getSyncedMeta("ChatOpen")) { return; }
	
	//  CEF SHIT
	if (key === InventoryUpK) {
		alt.emit("Client:Inventory:Menu");
    } else if (key === RadioChatterUpK) {
		alt.emit("Client:Phone:KeyN");
    }else if (key === Tablet73UpK ) {
		alt.emit("Client:Tablet:Key0x73");
    }else if (key === Phone34UpK) {
		alt.emit("Client:Phone:Key34");
    } else if (key === MegaphoneUpK) {
        if (!(alt.Player.local.vehicle && alt.Player.local.scriptID === game.getPedInVehicleSeat(alt.Player.local.vehicle.scriptID, -1, false) && game.getVehicleClass(alt.Player.local.vehicle.scriptID) === 18)) { return; }
		alt.emit("Client:Hud:ToggleMegaphone");
    }
	
	
	//  other shit not CEF related
	if (alt.Player.local.getSyncedMeta("IsCefOpen")) { return; }
	
	
    if (key === UseAndHonkUpK) {
        alt.emitServer("Server:KeyHandler:PressE");
    }else if (key === CycleSirenUpK ){
		if (alt.Player.local.vehicle === null) { return; }
		if (game.getVehicleClass(alt.Player.local.vehicle.scriptID) != 18) { return; } // Emergency cars
		alt.emit("Client:Sirens:14Pressed");
    } else if (key === UHandlerUpK) {
        alt.emitServer("Server:KeyHandler:PressU");
    } else if (key === F9HandlerUpK) {
        alt.emitServer("Server:KeyHandler:PressF9");
    } else if (key === Ragdoll1UpK) { //<-- Ä
		if (game.isPedSittingInAnyVehicle(alt.Player.local.scriptID)) return;
        alt.emitServer("Server:KeyHandler:PressRagdoll");
    } else if (key === Ragdoll2UpK) { //<-- Ö
		if (game.isPedSittingInAnyVehicle(alt.Player.local.scriptID)) return;
        alt.emitServer("Server:KeyHandler:PressRagdoll2");
    } else if (key === InteractionUpK) {
		alt.emit("Client:Hud:XMenu");
    } else if (key === KleidungUpK) {
		alt.emit("Client:Hud:KMenu");
    } else if (key === Phone33UpK) {
		alt.emit("Client:Phone:Key33");
    } else if (key === TuningNUpK) {
		alt.emit("Client:Tuning:KeyN");
    } else if (key === ClearAnimationUpK) { //Numpad0 
		if (game.isPedSittingInAnyVehicle(alt.Player.local.scriptID)) return;
        game.clearPedTasks(alt.Player.local.scriptID);
    } else if (key === AnimationMenuUpK) {
		if (game.isPedSittingInAnyVehicle(alt.Player.local.scriptID)) return;
		alt.emit("Client:Animation:Key113");
    } else if (key === FingerPointUpK) {
		if (game.isPedSittingInAnyVehicle(alt.Player.local.scriptID)) return;
		alt.emit("Client:Fingerpoint:TogglePoint");
    } else if (key === HandsupKeyUp){
		if (game.isPedSittingInAnyVehicle(alt.Player.local.scriptID)) return;
		alt.emit("Client:Handsup:ToggleHandsup");
	} else if (key === ToggleSirensUpK) { // ALT
		alt.emit("Client:ELS:ToggleSiren");
    } else if (key === ToggleLights) {
		alt.emit("Client:ELS:ToggleLights");
    }
});

alt.on('keydown', (key) => {
    if (alt.Player.local.getSyncedMeta("HasHandcuffs") === true || alt.Player.local.getSyncedMeta("HasRopeCuffs") === true || alt.Player.local.getSyncedMeta("IsUnconscious") === true) { return; }
	if(alt.Player.local.getSyncedMeta("ChatOpen") === true){ return };
	
	if(key === RadioChatterDownK) {	
		alt.emit("Client:Phone:KeyNDown");
	} else if (key === MegaphoneUpK) {		
		if (!(game.isPedSittingInAnyVehicle(alt.Player.local.scriptID))) { alt.emitServer("Server:KeyHandler:PressZ"); }
        if (!(alt.Player.local.vehicle && alt.Player.local.scriptID === game.getPedInVehicleSeat(alt.Player.local.vehicle.scriptID, -1, false) && game.getVehicleClass(alt.Player.local.vehicle.scriptID) === 18)) { return; }
		alt.emit("Client:Hud:ToggleMegaphone");
    }
	
	if (alt.Player.local.getSyncedMeta("IsCefOpen")) { return; }

    if (key === DuckHandlerDownK) { //STRG
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
    }else if (key === XKeyDownK) {
		alt.emit("Client:HUD:XKeyDown");
    }else if (key === KKeyDownK) {
		alt.emit("Client:HUD:KKeyDown");
    }else if (key === VoiceRangeDownK) {
        alt.emit("SaltyChat:ToggleRange");
    }else if (key === GKeyDownK) {
		alt.emit("Client:Passenger:GKeyDown");
	}else if (key === FingerPointDownK) {
		if (game.isPedSittingInAnyVehicle(alt.Player.local.scriptID)) return;
		alt.emit("Client:Fingerpoint:TogglePoint");		
    }else if (key === TuningDownK) {
		alt.emit("Client:Tuning:KeyNDown");
    }
});

alt.onServer("Client:DoorManager:ManageDoor", (hash, pos, isLocked) => {
    if (hash != undefined && pos != undefined && isLocked != undefined) {
        game.setStateOfClosestDoorOfType(game.getHashKey(hash), pos.x, pos.y, pos.z, isLocked, 0, 0);
    }
});