import * as alt from 'alt';
import * as game from 'natives';

export let clothesStoreBrowser = null;

let opened = false;

let camera;
let camera2;
let isCameraBeingCreated = false;

alt.onServer("Client:HUD:CreateCEF", () => {
    if (clothesStoreBrowser == null) {
        clothesStoreBrowser = new alt.WebView("http://resource/client/cef/clothesstore/index.html");

        clothesStoreBrowser.on('Client:Clothesstore:PreviewCloth', (isProp, previewComponentId, previewDrawableId, previewTextureId) => {
            if (!isProp) game.setPedComponentVariation(alt.Player.local.scriptID, parseInt(previewComponentId), parseInt(previewDrawableId), parseInt(previewTextureId), 0);
            else game.setPedPropIndex(alt.Player.local.scriptID, parseInt(previewComponentId), parseInt(previewDrawableId), parseInt(previewTextureId), false);
        });

        clothesStoreBrowser.on('Client:Clothesstore:BuyCloth', (clothId, isProp) => {
            alt.emitServer("Server:Clothesstore:BuyCloth", clothId, isProp);
        });

        clothesStoreBrowser.on('Client:Clothesstore:SetPerfectTorso', (BestTorsoDrawable, BestTorsoTexture) => {
            alt.emitServer("Server:Clothesstore:SetPerfectTorso", BestTorsoDrawable, BestTorsoTexture);
        });
    }
});

alt.onServer("Client:Clothesstore:OpenMenu", (gender) => {
    if (!opened) {
        opened = true
        alt.showCursor(true);
        alt.toggleGameControls(false);
        clothesStoreBrowser.focus();

        isCameraBeingCreated = true;
        const gamePlayCamRot = game.getGameplayCamRot(0);
        const gamePlayCamPos = game.getGameplayCamCoord();
        const fov = 70;
        const fwd = game.getEntityForwardVector(alt.Player.local.scriptID);
        const pos = {...alt.Player.local.pos };
        const fwdPos = {
            x: pos.x + fwd.x * 1.75,
            y: pos.y + fwd.y * 1.75,
            z: pos.z + 0.2,
        };
        camera = game.createCamWithParams(
            'DEFAULT_SCRIPTED_CAMERA',
            gamePlayCamPos.x,
            gamePlayCamPos.y,
            gamePlayCamPos.z,
            0,
            0,
            0,
            fov,
            false,
            0,
        );
        game.setCamRot(camera, gamePlayCamRot.x, gamePlayCamRot.y, gamePlayCamRot.z, 0);

        camera2 = game.createCamWithParams(
            'DEFAULT_SCRIPTED_CAMERA',
            fwdPos.x,
            fwdPos.y,
            fwdPos.z,
            0,
            0,
            0,
            fov,
            false,
            0,
        );

        const easeTime = 750;
        game.pointCamAtEntity(camera2, alt.Player.local.scriptID, 0, 0, 0, false);
        game.setCamActiveWithInterp(camera2, camera, 500, 1, 1);
        game.renderScriptCams(true, true, 0, true, false, 0);
    } else {
        opened = false;
        alt.showCursor(false);
        alt.toggleGameControls(true);
        clothesStoreBrowser.unfocus();
        game.setCamActive(camera, false);
        game.setCamActive(camera2, false);
        game.destroyAllCams(true);
        game.renderScriptCams(false, false, 0, false, false, 0);
        camera = null;
        camera2 = null;
    }

    clothesStoreBrowser.emit("CEF:Clothesstore:OpenMenu", gender);
});