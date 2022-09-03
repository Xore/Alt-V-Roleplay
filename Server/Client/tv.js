import * as alt from 'alt';
import * as native from "natives";
import * as game from 'natives';

let tvactive = false;
let channels = [
    "PL_STD_CNT",
    "PL_STD_WZL",
    "PL_LO_CNT",
    "PL_LO_WZL",
    "PL_SP_WORKOUT",
    "PL_SP_INV",
    "PL_SP_INV_EXP",
    "PL_LO_RS",
    "PL_LO_RS_CUTSCENE",
    "PL_SP_PLSH1_INTRO",
    "PL_LES1_FAME_OR_SHAME",
    "PL_STD_WZL_FOS_EP2",
    "PL_MP_WEAZEL",
    "PL_MP_CCTV",
    "PL_CINEMA_ACTION",
    "PL_CINEMA_ARTHOUSE",
    "PL_CINEMA_MULTIPLAYER",
    "PL_WEB_HOWITZER",
    "PL_WEB_RANGERS"
]

function CreateNamedRenderTargetForModel(name, model) {
    let handle = 0;
    if (!native.isNamedRendertargetRegistered(name)) {
        native.registerNamedRendertarget(name, false);
    }
    if (!native.isNamedRendertargetLinked(model)) {
        native.linkNamedRendertarget(model);
    }
    if (native.isNamedRendertargetRegistered(name)) {
        handle = native.getNamedRendertargetRenderId(name);
    }
    return handle;
}

function drawTv() {
    native.registerScriptWithAudio(0);
    native.setTvChannelPlaylist(2, "PL_STD_CNT", false);
    native.setTvChannel(2);
    native.enableMovieSubtitles(true);
    tvactive = true;
}

alt.everyTick(function() {
    if (tvactive) {
        let model = native.getHashKey("prop_tv_flat_02"); // Jimmys TV
        let entity = native.getClosestObjectOfType(3356.028, 5693.5, 4.4, 20.0, model, false, false, false);
        let handle = CreateNamedRenderTargetForModel("tvscreen", model);

        native.setTvAudioFrontend(false);
        native.attachTvAudioToEntity(entity);
        native.setTextRenderId(handle);
        native.setScriptGfxDrawBehindPausemenu(true);
        native.drawTvChannel(0.5, 0.5, 1.0, 1.0, 0.0, 255, 255, 255, 255);
        native.setTextRenderId(native.getDefaultScriptRendertargetRenderId());
        native.setScriptGfxDrawBehindPausemenu(false);
    }
});