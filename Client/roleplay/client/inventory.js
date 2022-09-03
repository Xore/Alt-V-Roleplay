import * as alt from 'alt';
import * as game from 'natives';
import natives from 'natives';
//import registeredObjects from "objects";
import registeredObjects from './includes/objects';

const OBJECT_RANGE = 30;
const CHECK_INTERVAL = 1000
let currentExistingObjects = [];

let inventoryBrowser = null;
let lastInteract = 0;
let inventoryCEFOpen = false;


function canInteract() { return lastInteract + 1000 < Date.now() }

alt.on("Client:Inventory:Menu", () => {
    if (!inventoryCEFOpen) {
        openInventoryCEF(true);
        inventoryCEFOpen = true;
    } else { //Inv close
        closeInventoryCEF();
        inventoryCEFOpen = false;
    }
});

function UseItem(itemname, itemAmount, fromContainer) {
    alt.emitServer("Server:Inventory:UseItem", itemname, parseInt(itemAmount), fromContainer);
}

function DropItem(itemname, itemAmount, fromContainer) {
    alt.emitServer("Server:Inventory:DropItem", itemname, parseInt(itemAmount), fromContainer);
}

function switchItemToDifferentInv(itemname, itemAmount, fromContainer, toContainer) {
    alt.emitServer("Server:Inventory:switchItemToDifferentInv", itemname, parseInt(itemAmount), fromContainer, toContainer);
}

function GiveItem(itemname, itemAmount, fromContainer, targetPlayerID) {
    alt.emitServer("Server:Inventory:GiveItem", itemname, parseInt(itemAmount), fromContainer, parseInt(targetPlayerID));
}

alt.onServer("Client:Inventory:CreateInventory", (invArray, backpackSize, targetPlayerID) => {
    openInventoryCEF(false);
    alt.setTimeout(() => {
        if (inventoryBrowser != null) {
            inventoryBrowser.emit('CEF:Inventory:AddInventoryItems', invArray, backpackSize, targetPlayerID);
        }
    }, 800);
});

alt.onServer('Client:Inventory:AddInventoryItems', (invArray, backpackSize, targetPlayerID) => {
    if (inventoryBrowser != null) {
        inventoryBrowser.emit('CEF:Inventory:AddInventoryItems', invArray, backpackSize, targetPlayerID);
    }
});
alt.onServer("Client:Inventory:CreateInventory", (invArray, brieftascheSize, targetPlayerID) => {
    openInventoryCEF(false);
    alt.setTimeout(() => {
        if (inventoryBrowser != null) {
            inventoryBrowser.emit('CEF:Inventory:AddInventoryItems', invArray, brieftascheSize, targetPlayerID);
        }
    }, 800);
});

alt.onServer('Client:Inventory:AddInventoryItems', (invArray, brieftascheSize, targetPlayerID) => {
    if (inventoryBrowser != null) {
        inventoryBrowser.emit('CEF:Inventory:AddInventoryItems', invArray, brieftascheSize, targetPlayerID);
    }
});
alt.onServer("Client:Inventory:CreateInventory", (invArray, schluesselSize, targetPlayerID) => {
    openInventoryCEF(false);
    alt.setTimeout(() => {
        if (inventoryBrowser != null) {
            inventoryBrowser.emit('CEF:Inventory:AddInventoryItems', invArray, schluesselSize, targetPlayerID);
        }
    }, 800);
});

alt.onServer('Client:Inventory:AddInventoryItems', (invArray, schluesselSize, targetPlayerID) => {
    if (inventoryBrowser != null) {
        inventoryBrowser.emit('CEF:Inventory:AddInventoryItems', invArray, schluesselSize, targetPlayerID);
    }
});

alt.onServer('Client:Inventory:closeCEF', () => {
    closeInventoryCEF();
});

alt.onServer('Client:Inventory:PlayAnimation', (animDict, animName, duration, flag, lockpos) => {
    game.requestAnimDict(animDict);
    let interval = alt.setInterval(() => {
        if (game.hasAnimDictLoaded(animDict)) {
            alt.clearInterval(interval);
            game.taskPlayAnim(game.playerPedId(), animDict, animName, 8.0, 1, duration, flag, 1, lockpos, lockpos, lockpos);
        }
    }, 0);
});

alt.onServer('Client:Inventory:PlayWalking', (anim) => {
    game.requestAnimSet(anim);
    let interval = alt.setInterval(() => {
        if (game.hasAnimDictLoaded(anim)) {
            alt.clearInterval(interval);
            game.setPedMovementClipset(alt.Player.local.scriptID, anim, 0.2);
        }
    }, 0);
});

alt.onServer("Client:Inventory:PlayEffect", (name, duration) => {
    game.animpostfxPlay(name, duration, false);
});

alt.onServer("Client:Inventory:StopAnimation", () => {
    game.clearPedTasks(alt.Player.local.scriptID);
});

let openInventoryCEF = function(requestItems) {
    if (inventoryBrowser == null && alt.Player.local.getSyncedMeta("IsCefOpen") == false && alt.Player.local.getSyncedMeta("PLAYER_SPAWNED") == true) {
        alt.showCursor(true);
        alt.toggleGameControls(false);
        inventoryBrowser = new alt.WebView("http://resource/client/cef/inventory/index.html");
        inventoryBrowser.focus();
        alt.emitServer("Server:CEF:setCefStatus", true);
        inventoryBrowser.on("Client:Inventory:cefIsReady", () => {
            if (!requestItems) return;
            alt.emitServer("Server:Inventory:RequestInventoryItems");
        });
        inventoryBrowser.on("Client:Inventory:UseInvItem", UseItem);
        inventoryBrowser.on("Client:Inventory:DropInvItem", DropItem);
        inventoryBrowser.on("Client:Inventory:switchItemToDifferentInv", switchItemToDifferentInv);
        inventoryBrowser.on("Client:Inventory:giveItem", GiveItem);
        game.triggerScreenblurFadeIn(5); //TODO
    }
}

export function closeInventoryCEF() {
    if (inventoryBrowser != null) {
        inventoryBrowser.off("Client:Inventory:UseInvItem", UseItem);
        inventoryBrowser.off("Client:Inventory:DropInvItem", DropItem);
        inventoryBrowser.off("Client:Inventory:switchItemToDifferentInv", switchItemToDifferentInv);
        inventoryBrowser.off("Client:Inventory:giveItem", GiveItem);
        inventoryBrowser.unfocus();
        inventoryBrowser.destroy();
        inventoryBrowser = null;
        alt.showCursor(false);
        alt.toggleGameControls(true);
        alt.emitServer("Server:CEF:setCefStatus", false);
        game.triggerScreenblurFadeOut(5); //TODO
    }
}

/*ObjectAttacher*/
function outputMessage(message) {
    console.log('[ObjectAttacher] ' + message);
}

function getRegisteredObject(objectName) {
    if (registeredObjects[objectName]) {
        return registeredObjects[objectName];
    } else {
        outputMessage('Object is not registered: ' + objectName);
        return null;
    }
}

function resetAnimationOnLocalPlayer() {
    try {
        natives.clearPedTasks(alt.Player.local.scriptID);
    } catch (e) {
        outputMessage(e.message);
    }
}

function removeObjectFromPlayer(player) {
    try {
        let object = currentExistingObjects[player.id];
        if (object && natives.doesEntityExist(object)) {
            natives.detachEntity(object, true, true);
            natives.deleteObject(object);
            currentExistingObjects[player.id] = null;
            // Show weapon again
            //natives.setPedCurrentWeaponVisible(player.scriptID, true, true, true, true);
        }
    } catch (e) {
        outputMessage(e.message);
    }
}

function attachObjectToPlayer(player, boneId, objectName, positionX, positionY, positionZ, rotationX, rotationY, rotationZ) {
    try {
        // Remove existing object (if exists)
        removeObjectFromPlayer(player);

        let hashOfProp = natives.getHashKey(objectName);

        natives.requestModel(hashOfProp);
        const modelLoadInterval = alt.setInterval(() => {
            if (natives.hasModelLoaded(hashOfProp)) {
                alt.clearInterval(modelLoadInterval);
            }
        }, 100);

        let newObject = natives.createObject(hashOfProp, player.pos.x, player.pos.y, player.pos.z, true, true, true);

        // Release memory for model
        natives.setModelAsNoLongerNeeded(hashOfProp);

        let boneIndex = natives.getPedBoneIndex(player.scriptID, Number(boneId));

        if (newObject) {
            // Hide weapon before attaching object
            natives.setPedCurrentWeaponVisible(player.scriptID, false, true, true, true);

            natives.attachEntityToEntity(newObject, player.scriptID, boneIndex, Number(positionX), Number(positionY), Number(positionZ),
                Number(rotationX), Number(rotationY), Number(rotationZ), false, false, false, false, 1, true);

            currentExistingObjects[player.id] = newObject;
        } else {
            outputMessage('Object is null: ' + objectName);
        }
    } catch (e) {
        outputMessage(e.message);
    }
}

function attachRegisteredObjectToPlayer(player, objectData) {
    if (objectData) {
        attachObjectToPlayer(player, objectData.boneId, objectData.objectName, objectData.position.x, objectData.position.y, objectData.position.z,
            objectData.rotation.x, objectData.rotation.y, objectData.rotation.z);
    }
}

function attachRegisteredObjectToLocalPlayerSynced(objectName, objectData) {
    if (objectData) {
        attachRegisteredObjectToPlayer(alt.Player.local, objectData);
        alt.emitServer('objectAttacher:attachedObject', objectName);
    }
}

function detachObjectFromLocalPlayerSynced() {
    removeObjectFromPlayer(alt.Player.local);
    resetAnimationOnLocalPlayer();
    alt.emitServer('objectAttacher:detachedObject');
}

function attachRegisteredObjectToLocalPlayerAnimated(objectName, detachObjectAfterAnimation) {
    let registeredObject = getRegisteredObject(objectName);
    if (registeredObject) {
        attachRegisteredObjectToLocalPlayerSynced(objectName, registeredObject);
        playAnimationSequenceOnLocalPlayer(registeredObject.enterAnimation, registeredObject.exitAnimation, () => {
            if (detachObjectAfterAnimation) {
                detachObjectFromLocalPlayerSynced();
            }
        });
    }
}

function playAnimationOnLocalPlayer(animDictionary, animationName, animationFlag) {
    try {
        if (natives.doesAnimDictExist(animDictionary)) {
            natives.requestAnimDict(animDictionary);

            const animDictLoadInterval = alt.setInterval(() => {
                if (natives.hasAnimDictLoaded(animDictionary)) {
                    alt.clearInterval(animDictLoadInterval)
                }
            }, 100)

            natives.taskPlayAnim(alt.Player.local.scriptID, animDictionary, animationName, 8.0, 8.0, -1, Number(animationFlag), 1.0, false, false, false);
        } else {
            outputMessage('Animation dictionary does not exist');
        }
    } catch (e) {
        outputMessage(e.message);
    }
}

function playAnimationSequenceOnLocalPlayer(enterAnimation, exitAnimation, sequenceFinishedCallback) {
    let enterAnimationIsSet = enterAnimation && enterAnimation.dict && enterAnimation.name;
    let exitAnimationIsSet = exitAnimation && exitAnimation.dict && exitAnimation.name;

    let firstAnimation = null;
    let secondAnimation = null;

    // Only play animations that are completely set
    if (enterAnimationIsSet) {
        firstAnimation = enterAnimation;
        if (exitAnimationIsSet) {
            secondAnimation = exitAnimation;
        }
    } else if (exitAnimationIsSet) {
        firstAnimation = exitAnimation;
    }

    if (firstAnimation) {
        resetAnimationOnLocalPlayer();

        playAnimationOnLocalPlayer(firstAnimation.dict, firstAnimation.name, firstAnimation.flag);

        if (firstAnimation.durationMs && firstAnimation.durationMs > 0) {
            alt.setTimeout(() => {
                if (secondAnimation) {
                    playAnimationOnLocalPlayer(secondAnimation.dict, secondAnimation.name, secondAnimation.flag);

                    if (secondAnimation.durationMs && secondAnimation.durationMs > 0) {
                        alt.setTimeout(() => {
                            resetAnimationOnLocalPlayer();
                            sequenceFinishedCallback();
                        }, secondAnimation.durationMs);
                    }
                } else {
                    resetAnimationOnLocalPlayer();
                    sequenceFinishedCallback();
                }
            }, firstAnimation.durationMs);
        }
    }
}

// Interval for attaching and removing objects from remote players
alt.setInterval(() => {
    try {
        alt.Player.all.forEach(remotePlayer => {
            // Skip local player
            if (remotePlayer.id == alt.Player.local.id) {
                return;
            }

            let objectOfRemotePlayer = remotePlayer.getSyncedMeta('AttachedObject');

            if (objectOfRemotePlayer) {
                let isRemotePlayerInRange = remotePlayer.scriptID && remotePlayer.pos.isInRange(alt.Player.local.pos, OBJECT_RANGE);

                // Object not created yet?
                if (!currentExistingObjects[remotePlayer.id]) {
                    if (isRemotePlayerInRange) {
                        // Attach object to remote player
                        attachRegisteredObjectToPlayer(remotePlayer, getRegisteredObject(objectOfRemotePlayer));
                    }
                } else {
                    // Players is holding object, but is not in range anymore
                    if (!isRemotePlayerInRange) {
                        removeObjectFromPlayer(remotePlayer);
                    }
                }
            } else {
                // Remove object, if player was holding one before
                removeObjectFromPlayer(remotePlayer);
            }
        });
    } catch (e) {
        outputMessage(e.message);
    }
}, CHECK_INTERVAL);

alt.on('objectAttacher:attachObjectAnimated', (objectName, detachObjectAfterAnimation) => {
    attachRegisteredObjectToLocalPlayerAnimated(objectName, detachObjectAfterAnimation);
});

alt.onServer('Client:Inventory:AttachObject', (objectName, detachObjectAfterAnimation) => {
    attachRegisteredObjectToLocalPlayerAnimated(objectName, true);
});

alt.onServer('objectAttacher:attachObjectAnimated', (objectName, detachObjectAfterAnimation) => {
    attachRegisteredObjectToLocalPlayerAnimated(objectName, detachObjectAfterAnimation);
});

alt.on('objectAttacher:attachObject', (objectName) => {
    attachRegisteredObjectToLocalPlayerSynced(objectName, getRegisteredObject(objectName));
});

alt.onServer('objectAttacher:attachObject', (objectName) => {
    attachRegisteredObjectToLocalPlayerSynced(objectName, getRegisteredObject(objectName));
});

alt.on('objectAttacher:detachObject', () => {
    detachObjectFromLocalPlayerSynced();
});

alt.onServer('Client:Inventory:DetachObject', () => {
    detachObjectFromLocalPlayerSynced();
});