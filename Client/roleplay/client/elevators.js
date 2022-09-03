import * as alt from 'alt-client';
import * as game from 'natives';
import * as NativeUI from './includes/NativeUI/NativeUI';
const MenuSettings = {
    TitleFont: NativeUI.Font.Monospace,
}

//MD ELEVATORS START
const menuMD = new NativeUI.Menu("Fahrstuhl", "", new NativeUI.Point(10, 10));
menuMD.GetTitle().Scale = 0.9;
menuMD.GetTitle().Font = MenuSettings.TitleFont;
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
menuMD.SetRectangleBannerType(banner);
menuMD.AddItem(new NativeUI.UIMenuItem("Tiefgarage", ""));
menuMD.AddItem(new NativeUI.UIMenuItem("Erdgeschoss", ""));
menuMD.AddItem(new NativeUI.UIMenuItem("Stockwerk 1", ""));
menuMD.AddItem(new NativeUI.UIMenuItem("Stockwerk 2", ""));
menuMD.AddItem(new NativeUI.UIMenuItem("Helipad", ""));
menuMD.ItemSelect.on((item) => {
    if (item.Text == 'Tiefgarage') {
        alt.emitServer("Server:Elevator:MD", 1);
        game.playSoundFrontend(-1, "FAKE_ARRIVE", "MP_PROPERTIES_ELEVATOR_DOORS", true);
        menuMD.Close();
    } else if (item.Text == 'Erdgeschoss') {
        alt.emitServer("Server:Elevator:MD", 2);
        game.playSoundFrontend(-1, "FAKE_ARRIVE", "MP_PROPERTIES_ELEVATOR_DOORS", true);
        menuMD.Close();
    } else if (item.Text == 'Stockwerk 1') {
        alt.emitServer("Server:Elevator:MD", 3);
        game.playSoundFrontend(-1, "FAKE_ARRIVE", "MP_PROPERTIES_ELEVATOR_DOORS", true);
        menuMD.Close();
    } else if (item.Text == 'Stockwerk 2') {
        alt.emitServer("Server:Elevator:MD", 4);
        game.playSoundFrontend(-1, "FAKE_ARRIVE", "MP_PROPERTIES_ELEVATOR_DOORS", true);
        menuMD.Close();
    } else if (item.Text == 'Helipad') {
        alt.emitServer("Server:Elevator:MD", 5);
        game.playSoundFrontend(-1, "FAKE_ARRIVE", "MP_PROPERTIES_ELEVATOR_DOORS", true);
        menuMD.Close();
    }
});
alt.onServer("Client:Elevator:openMD", () => {
    if (alt.Player.local.getSyncedMeta("IsCefOpen") == false) {
        if (menuMD.Visible)
            menuMD.Close();
        else
            menuMD.Open();
    }
});
//MD ELEVATORS END

//FIB ELEVATORS START
const menuFIB = new NativeUI.Menu("Fahrstuhl", "", new NativeUI.Point(10, 10));
menuFIB.GetTitle().Scale = 0.9;
menuFIB.GetTitle().Font = MenuSettings.TitleFont;
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
menuFIB.SetRectangleBannerType(banner);
menuFIB.AddItem(new NativeUI.UIMenuItem("Tiefgarage", ""));
menuFIB.AddItem(new NativeUI.UIMenuItem("Erdgeschoss", ""));
menuFIB.AddItem(new NativeUI.UIMenuItem("Stockwerk 1", ""));
menuFIB.AddItem(new NativeUI.UIMenuItem("Helipad", ""));
menuFIB.ItemSelect.on((item) => {
    if (item.Text == 'Tiefgarage') {
        alt.emitServer("Server:Elevator:FIB", 1);
        game.playSoundFrontend(-1, "FAKE_ARRIVE", "MP_PROPERTIES_ELEVATOR_DOORS", true);
        menuFIB.Close();
    } else if (item.Text == 'Erdgeschoss') {
        alt.emitServer("Server:Elevator:FIB", 2);
        game.playSoundFrontend(-1, "FAKE_ARRIVE", "MP_PROPERTIES_ELEVATOR_DOORS", true);
        menuFIB.Close();
    } else if (item.Text == 'Stockwerk 1') {
        alt.emitServer("Server:Elevator:FIB", 3);
        game.playSoundFrontend(-1, "FAKE_ARRIVE", "MP_PROPERTIES_ELEVATOR_DOORS", true);
        menuFIB.Close();
    } else if (item.Text == 'Helipad') {
        alt.emitServer("Server:Elevator:FIB", 4);
        game.playSoundFrontend(-1, "FAKE_ARRIVE", "MP_PROPERTIES_ELEVATOR_DOORS", true);
        menuFIB.Close();
    }
});
alt.onServer("Client:Elevator:openFIB", () => {
    if (alt.Player.local.getSyncedMeta("IsCefOpen") == false) {
        if (menuFIB.Visible)
            menuFIB.Close();
        else
            menuFIB.Open();
    }
});
//FIB ELEVATORS END