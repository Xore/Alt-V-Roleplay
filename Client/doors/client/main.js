import * as alt from 'alt-client';
import * as natives from 'natives';

import { Doors } from '../shared/config';

const localPlayer = alt.Player.local;

let doorFound;

alt.onServer('doorLock:client:setState', (doorID, state) => {
    if (!Doors[doorID]) return;
    Doors[doorID].locked = state;
});

const toggleDoorLock = (doorState, index) => {
    if(Doors[index].requiredKey == 'KEY')
    {
        Doors[index].locking = true;
        natives.freezeEntityPosition(localPlayer, true);
        alt.emit('Client:HUD:sendNotification', 1, 1500, `${doorState ? 'Aufgeschlossen' : 'Abgeschlossen'}`);
        alt.emit("playHowl2d", "audio/doorlock.wav", 0.25);
        playAnimation("anim@heists@keycard@", "exit", 49, 1000);
        alt.setTimeout(() => {
            Doors[index].locking = false;
            natives.clearPedTasks(localPlayer);
            natives.freezeEntityPosition(localPlayer, false);
            alt.emitServer('doorLock:server:updateState', index, !doorState);
        }, 500);
    }else if (Doors[index].requiredKey != 'KEY')
    {
        // faking check ur faking key brah and then just do this below me
        
        Doors[index].locking = true;
        natives.freezeEntityPosition(localPlayer, true);
        alt.emit('Client:HUD:sendNotification', 1, 1500, `${doorState ? 'Aufgeschlossen' : 'Abgeschlossen'}`);
        alt.emit("playHowl2d", "audio/doorlock.wav", 0.25);
        playAnimation("anim@heists@keycard@", "exit", 49, 1000);
        alt.setTimeout(() => {
            Doors[index].locking = false;
            natives.clearPedTasks(localPlayer);
            natives.freezeEntityPosition(localPlayer, false);
            alt.emitServer('doorLock:server:updateState', index, !doorState);
        }, 500);
    }
};

const doorManager = () => {
    Doors.map((current, i) => {
        let distance;
        let maxDistance;
        let awayFromDoors = true;

        // Check distance between doors
        if (current.doors) {
            distance = localPlayer.pos.distanceTo(current.doors[0].objCoords);
        } else {
            distance = localPlayer.pos.distanceTo(current.objCoords);
        }

        if (current.distance) {
            maxDistance = current.distance;
        }

        if (distance > 3 && !doorFound) return;

        // See if has multiple doors
        if (current.doors) {
            for (let j = 0; j < current.doors.length; j++) {
                const currentDoor = current.doors[j];
                const doorHash =
                    currentDoor.objName === typeof Number ?
                    currentDoor.objName :
                    alt.hash(currentDoor.objName);
                if (!currentDoor.object ||
                    natives.doesEntityExist(currentDoor.object)
                ) {
                    currentDoor.object = natives.getClosestObjectOfType(
                        currentDoor.objCoords.x,
                        currentDoor.objCoords.y,
                        currentDoor.objCoords.z,
                        1.0,
                        doorHash,
                        false,
                        false,
                        false
                    );
                }
                natives.freezeEntityPosition(
                    currentDoor.object,
                    current.locked
                );
                if (
                    current.locked &&
                    currentDoor.objYaw &&
                    natives.getEntityRotation(currentDoor.object, 2).z !==
                    currentDoor.objYaw
                ) {
                    natives.setEntityRotation(
                        currentDoor.object,
                        0,
                        0,
                        currentDoor.objYaw,
                        2,
                        true
                    );
                }
            }
        } else {
            const doorHash =
                current.objName === typeof Number ?
                current.objName :
                alt.hash(current.objName);
            if (!current.object || natives.doesEntityExist(current.object)) {
                current.object = natives.getClosestObjectOfType(
                    current.objCoords.x,
                    current.objCoords.y,
                    current.objCoords.z,
                    1.0,
                    doorHash,
                    false,
                    false,
                    false
                );
            }
            natives.freezeEntityPosition(current.object, current.locked);
            if (
                current.locked &&
                current.objYaw &&
                natives.getEntityRotation(current.object, 2).z !==
                current.objYaw
            ) {
                natives.setEntityRotation(
                    current.object,
                    0,
                    0,
                    current.objYaw,
                    2,
                    true
                );
            }
        }

        if (distance < maxDistance) {
            awayFromDoors = false;
            doorFound = true;

            if (natives.isControlJustReleased(0, 38)) {
                toggleDoorLock(current.locked, i);
            }
        }

        if (awayFromDoors) {
            doorFound = false;
        }
    });
};

//Thread
alt.everyTick(() => {
    doorManager();
});

function playAnimation(animDict, animName, animFlag, animDuration) {
    if (animDict == undefined || animName == undefined || animFlag == undefined || animDuration == undefined) return;
    natives.requestAnimDict(animDict);
    let interval = alt.setInterval(() => {
        if (natives.hasAnimDictLoaded(animDict)) {
            alt.clearInterval(interval);
            natives.taskPlayAnim(alt.Player.local.scriptID, animDict, animName, 8.0, 1, animDuration, animFlag, 1, false, false, false);
        }
    }, 0);
}