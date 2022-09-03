import * as alt from 'alt';
import * as native from 'natives';

var hotkey = 72; //Change key - https://keycode.info/
var handsUp = false;
var status = false;
const localPlayer = alt.Player.local;
var dict = "missminuteman_1ig_2";
native.requestAnimDict(dict);

alt.on('keydown', (key) => {
    if (key === hotkey && alt.Player.local.getSyncedMeta("IsCefOpen") == false) {
        handsUp = true;
    }
});

alt.on('keyup', (key) => {
    if (key === hotkey && alt.Player.local.getSyncedMeta("IsCefOpen") == false) {
        handsUp = false;
    }
});

alt.setInterval(() => {
    if (handsUp == true) {
        if (status == false) {
            native.taskPlayAnim(localPlayer.scriptID, dict, "handsup_enter", 8.0, 8.0, -1, 50, 0, false, false, false);
            status = true;
        }
    } else {
        if (status == true) {
            native.clearPedTasks(localPlayer.scriptID);
            status = false;
        }
    }
}, 10);