import * as alt from 'alt';
import * as native from "natives";



alt.setInterval(function() { CheckMap() }, 1000);

function CheckMap() {
    if (!alt.Player.local.vehicle) {
        native.displayRadar(false);
    } else {
        native.displayRadar(true);
        alt.beginScaleformMovieMethodMinimap('SETUP_HEALTH_ARMOUR');
        native.scaleformMovieMethodAddParamInt(3);
        native.endScaleformMovieMethod();
    }
}