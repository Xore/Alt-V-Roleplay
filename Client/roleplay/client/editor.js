import * as alt from 'alt-client';
import * as game from 'natives';
import * as NativeUI from './includes/NativeUI/NativeUI';
const MenuSettings = {
    TitleFont: NativeUI.Font.Monospace,
}

//
const menuEditor = new NativeUI.Menu("R* Editor", "", new NativeUI.Point(10, 10));
menuEditor.GetTitle().Scale = 0.9;
menuEditor.GetTitle().Font = MenuSettings.TitleFont;
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
menuEditor.SetRectangleBannerType(banner);
menuEditor.AddItem(new NativeUI.UIMenuItem("~g~Enable Editor", ""));
menuEditor.AddItem(new NativeUI.UIMenuItem("~r~Disable Editor", ""));
menuEditor.AddItem(new NativeUI.UIMenuItem("~g~Start Rec", ""));
menuEditor.AddItem(new NativeUI.UIMenuItem("~r~Stop Rec", ""));
//menuEditor.AddItem(new NativeUI.UIMenuItem("Button 5", ""));
menuEditor.ItemSelect.on((item) => {
    if (item.Text == '~g~Enable Editor') {
        enableRockstarEditor();
    } else if (item.Text == '~r~Disable Editor') {
        disableRockstarEditor();
    } else if (item.Text == '~g~Start Rec') {
        startRecording();
    } else if (item.Text == '~r~Stop Rec') {
        stopRecordingAndSaveClip();
    }
});
alt.on('keyup', (key) => {
    if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true || alt.Player.local.getSyncedMeta("IsUnconscious") == true || alt.Player.local.getSyncedMeta("IsCefOpen") == true || alt.Player.local.vehicle) return;
    if (key === 118) {
        if (menuEditor.Visible)
            menuEditor.Close();
        else
            menuEditor.Open();
    }
});
//

function startRecording() {
    game.startRecording(1);
}

function stopRecordingAndSaveClip() {
    game.stopRecordingAndSaveClip();
}

function enableRockstarEditor() {
    game.activateRockstarEditor();
    game.setPlayerRockstarEditorDisabled(false);

    const interval = setInterval(() => {
        if (game.isScreenFadedOut()) {
            game.doScreenFadeIn(1000);
            clearInterval(interval);
        }
    }, 1000);
}

function disableRockstarEditor() {
    game.setPlayerRockstarEditorDisabled(true);
}