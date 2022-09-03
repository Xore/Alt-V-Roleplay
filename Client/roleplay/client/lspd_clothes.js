import * as alt from 'alt-client';
import * as game from 'natives';
import * as NativeUI from './includes/NativeUI/NativeUI';
const MenuSettings = {
    TitleFont: NativeUI.Font.Monospace,
}

const menu = new NativeUI.Menu("Kleiderkammer", "", new NativeUI.Point(10, 10));
menu.GetTitle().Scale = 0.9;
menu.GetTitle().Font = MenuSettings.TitleFont;
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
menu.SetRectangleBannerType(banner);
//MALE UNIFORMS
let menuItem = new NativeUI.UIMenuItem("Uniformen M", "");
menu.AddItem(menuItem);
const subMenu = new NativeUI.Menu("Uniformen M", "", new NativeUI.Point(10, 10));
subMenu.Visible = false;
subMenu.GetTitle().Scale = 0.9;
subMenu.GetTitle().Font = MenuSettings.TitleFont;
menu.AddSubMenu(subMenu, menuItem);
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
subMenu.SetRectangleBannerType(banner);
var player = alt.Player.local.scriptID;
//ADD ITEM
subMenu.AddItem(new NativeUI.UIMenuItem("Uniform 1 -M-", ""));
subMenu.AddItem(new NativeUI.UIMenuItem("Uniform 2 -M-", ""));
subMenu.AddItem(new NativeUI.UIMenuItem("Zivil 1 -M-", ""));
subMenu.AddItem(new NativeUI.UIMenuItem("SWAT -M-", ""));
//ON SELECT
subMenu.ItemSelect.on((item) => {
    if (item.Text == 'Uniform 1 -M-') {
        game.setPedComponentVariation(alt.Player.local.scriptID, 3, 30, 0, 0); //Torso
        game.setPedComponentVariation(alt.Player.local.scriptID, 4, 31, 0, 0); //Leg
        game.setPedComponentVariation(alt.Player.local.scriptID, 6, 25, 0, 0); //Shoe
        game.setPedComponentVariation(alt.Player.local.scriptID, 8, 185, 2, 0); //Undershirt
        game.setPedComponentVariation(alt.Player.local.scriptID, 9, 63, 0, 0); //Body Armor
        game.setPedComponentVariation(alt.Player.local.scriptID, 11, 385, 0, 0); //Tops
    } else if (item.Text == 'Uniform 2 -M-') {
        game.setPedComponentVariation(alt.Player.local.scriptID, 3, 96, 0, 0); //Torso
        game.setPedComponentVariation(alt.Player.local.scriptID, 4, 31, 0, 0); //Leg
        game.setPedComponentVariation(alt.Player.local.scriptID, 6, 25, 0, 0); //Shoe
        game.setPedComponentVariation(alt.Player.local.scriptID, 8, 185, 2, 0); //Undershirt
        game.setPedComponentVariation(alt.Player.local.scriptID, 9, 63, 0, 0); //Body Armor
        game.setPedComponentVariation(alt.Player.local.scriptID, 11, 386, 0, 0); //Tops
    } else if (item.Text == 'Zivil 1 -M-') {
        game.setPedComponentVariation(alt.Player.local.scriptID, 7, 125, 0, 0); //Accessorie
        game.setPedComponentVariation(alt.Player.local.scriptID, 8, 122, 0, 0); //Undershirt
    } else if (item.Text == 'SWAT -M-') {
        game.setPedComponentVariation(alt.Player.local.scriptID, 1, 52, 0, 0); //Mask
        game.setPedComponentVariation(alt.Player.local.scriptID, 3, 96, 0, 0); //Torso
        game.setPedComponentVariation(alt.Player.local.scriptID, 4, 31, 0, 0); //Leg
        game.setPedComponentVariation(alt.Player.local.scriptID, 6, 25, 0, 0); //Shoe
        game.setPedComponentVariation(alt.Player.local.scriptID, 7, 155, 0, 0); //Accessorie
        game.setPedComponentVariation(alt.Player.local.scriptID, 8, 191, 0, 0); //Undershirt
        game.setPedComponentVariation(alt.Player.local.scriptID, 9, 59, 0, 0); //Body Armor
        game.setPedComponentVariation(alt.Player.local.scriptID, 11, 336, 3, 0); //Tops
        game.setPedPropIndex(alt.Player.local.scriptID, 0, 156, 0, false); //Hats
    }
});

//FEMALE UNIFORMS
let menuItem2 = new NativeUI.UIMenuItem("Uniformen F", "");
menu.AddItem(menuItem2);
const subMenu2 = new NativeUI.Menu("Uniformen F", "", new NativeUI.Point(10, 10));
subMenu2.Visible = false;
subMenu2.GetTitle().Scale = 0.9;
subMenu2.GetTitle().Font = MenuSettings.TitleFont;
menu.AddSubMenu(subMenu2, menuItem2);
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
subMenu2.SetRectangleBannerType(banner);
//ADD ITEM
subMenu2.AddItem(new NativeUI.UIMenuItem("Uniform 1 -F-", ""));
subMenu2.AddItem(new NativeUI.UIMenuItem("Uniform 2 -F-", ""));
subMenu2.AddItem(new NativeUI.UIMenuItem("Zivil 1 -F-", ""));
subMenu2.AddItem(new NativeUI.UIMenuItem("SWAT -F-", ""));
//ON SELECT
subMenu2.ItemSelect.on((item) => {
    if (item.Text == 'Uniform 1 -F-') {
        game.setPedComponentVariation(alt.Player.local.scriptID, 3, 33, 0, 0); //Torso
        game.setPedComponentVariation(alt.Player.local.scriptID, 4, 30, 0, 0); //Leg
        game.setPedComponentVariation(alt.Player.local.scriptID, 6, 25, 0, 0); //Shoe
        game.setPedComponentVariation(alt.Player.local.scriptID, 8, 222, 0, 0); //Undershirt
        game.setPedComponentVariation(alt.Player.local.scriptID, 9, 56, 0, 0); //Body Armor
        game.setPedComponentVariation(alt.Player.local.scriptID, 11, 409, 0, 0); //Tops
    } else if (item.Text == 'Uniform 2 -F-') {
        game.setPedComponentVariation(alt.Player.local.scriptID, 3, 111, 0, 0); //Torso
        game.setPedComponentVariation(alt.Player.local.scriptID, 4, 30, 0, 0); //Leg
        game.setPedComponentVariation(alt.Player.local.scriptID, 6, 25, 0, 0); //Shoe
        game.setPedComponentVariation(alt.Player.local.scriptID, 8, 222, 0, 0); //Undershirt
        game.setPedComponentVariation(alt.Player.local.scriptID, 9, 56, 0, 0); //Body Armor
        game.setPedComponentVariation(alt.Player.local.scriptID, 11, 410, 0, 0); //Tops
    } else if (item.Text == 'Zivil 1 -F-') {
        game.setPedComponentVariation(alt.Player.local.scriptID, 7, 95, 0, 0); //Accessorie
        game.setPedComponentVariation(alt.Player.local.scriptID, 8, 152, 0, 0); //Undershirt
    } else if (item.Text == 'SWAT -F-') {
        game.setPedComponentVariation(alt.Player.local.scriptID, 1, 52, 0, 0); //Mask
        game.setPedComponentVariation(alt.Player.local.scriptID, 3, 111, 0, 0); //Torso
        game.setPedComponentVariation(alt.Player.local.scriptID, 4, 30, 0, 0); //Leg
        game.setPedComponentVariation(alt.Player.local.scriptID, 6, 25, 0, 0); //Shoe
        game.setPedComponentVariation(alt.Player.local.scriptID, 7, 124, 0, 0); //Accessorie
        game.setPedComponentVariation(alt.Player.local.scriptID, 8, 223, 0, 0); //Undershirt
        game.setPedComponentVariation(alt.Player.local.scriptID, 9, 58, 0, 0); //Body Armor
        game.setPedComponentVariation(alt.Player.local.scriptID, 11, 351, 3, 0); //Tops
        game.setPedPropIndex(alt.Player.local.scriptID, 0, 155, 0, false); //Hats
    }
});

/*alt.on('keyup', (key) => {
    if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true || alt.Player.local.getSyncedMeta("IsUnconscious") == true || alt.Player.local.getSyncedMeta("IsCefOpen") == true || alt.Player.local.vehicle) return;
    if (key === 114) {
        if (menu.Visible)
            menu.Close();
        else
            menu.Open();
    }
});*/

alt.onServer("Client:LSPD:openClotes", () => {
    if (alt.Player.local.getSyncedMeta("IsCefOpen") == false) {
        if (menu.Visible)
            menu.Close();
        else
            menu.Open();
    }
});