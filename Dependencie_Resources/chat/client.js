import * as alt from 'alt';
import * as native from "natives";

let buffer = [];

let loaded = false;
let opened = false;
let hidden = false;

const view = new alt.WebView("http://resource/html/index.html");

function addMessage(name, text) {
    if (alt.Player.local.getSyncedMeta("ADMINLEVEL") <= 0) return;
    if (name) {
        view.emit('addMessage', name, text);
    } else {
        view.emit('addString', text);
    }
}

view.on('chatloaded', () => {
    for (const msg of buffer) {
        addMessage(msg.name, msg.text);
    }

    loaded = true;
})

view.on('chatmessage', (text) => {
    alt.emitServer('chat:message', text);
    opened = false;
    alt.toggleGameControls(true);
    alt.emit("Client:HUD:setCefStatus", false);
})

export function pushMessage(name, text) {
    if (!loaded) {
        buffer.push({ name, text });
    } else {
        addMessage(name, text);
    }
}

export function pushLine(text) {
    pushMessage(null, text);
}

alt.onServer('chat:message', pushMessage);

alt.on('keyup', (key) => {
    if (loaded) {
        if (!opened && key === 0x23 && alt.gameControlsEnabled()) {
            if (alt.Player.local.getSyncedMeta("ADMINLEVEL") <= 0) return;
            opened = true;
            view.emit('openChat', false);
            view.focus();
            alt.toggleGameControls(false);
            alt.emit("Client:HUD:setCefStatus", true);
        } else if (!opened && key === 0xBF && alt.gameControlsEnabled()) {
            if (alt.Player.local.getSyncedMeta("ADMINLEVEL") <= 0) return;
            opened = true;
            view.emit('openChat', true);
            view.focus();
            alt.toggleGameControls(false);
            alt.emit("Client:HUD:setCefStatus", true);
        } else if (opened && key == 0x1B) {
            opened = false;
            view.emit('closeChat');
            view.focus();
            alt.toggleGameControls(true);
            alt.emit("Client:HUD:setCefStatus", false);
        } else if (key == 0x76) {
            if (native.displayRadar(false));
            else if (native.displayRadar(true));
        }
    }
});