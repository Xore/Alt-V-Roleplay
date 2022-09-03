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
subMenu.AddItem(new NativeUI.UIMenuItem("Uniform 3 -M-", ""));
subMenu.AddItem(new NativeUI.UIMenuItem("Zivil 1 -M-", ""));
//ON SELECT
subMenu.ItemSelect.on((item) => {
    if (item.Text == 'Uniform 1 -M-') {
        game.setPedComponentVariation(alt.Player.local.scriptID, 1, 52, 0, 0); //Mask
        game.setPedComponentVariation(alt.Player.local.scriptID, 3, 96, 0, 0); //Torso
        game.setPedComponentVariation(alt.Player.local.scriptID, 4, 130, 1, 0); //Leg
        game.setPedComponentVariation(alt.Player.local.scriptID, 6, 24, 0, 0); //Shoe
        game.setPedComponentVariation(alt.Player.local.scriptID, 7, 155, 0, 0); //Accessorie
        game.setPedComponentVariation(alt.Player.local.scriptID, 8, 188, 2, 0); //Undershirt
        game.setPedComponentVariation(alt.Player.local.scriptID, 9, 57, 1, 0); //Body Armor
        game.setPedComponentVariation(alt.Player.local.scriptID, 11, 336, 3, 0); //Tops
        game.setPedPropIndex(alt.Player.local.scriptID, 0, 156, 2, false); //Hats
    } else if (item.Text == 'Uniform 2 -M-') {
        game.setPedComponentVariation(alt.Player.local.scriptID, 1, 169, 0, 0); //Mask
        game.setPedComponentVariation(alt.Player.local.scriptID, 3, 96, 0, 0); //Torso
        game.setPedComponentVariation(alt.Player.local.scriptID, 4, 130, 1, 0); //Leg
        game.setPedComponentVariation(alt.Player.local.scriptID, 6, 24, 0, 0); //Shoe
        game.setPedComponentVariation(alt.Player.local.scriptID, 7, 155, 0, 0); //Accessorie
        game.setPedComponentVariation(alt.Player.local.scriptID, 8, 191, 2, 0); //Undershirt
        game.setPedComponentVariation(alt.Player.local.scriptID, 9, 59, 1, 0); //Body Armor
        game.setPedComponentVariation(alt.Player.local.scriptID, 11, 336, 3, 0); //Tops
        game.setPedPropIndex(alt.Player.local.scriptID, 0, 156, 2, false); //Hats
    } else if (item.Text == 'Uniform 3 -M-') {
        game.setPedComponentVariation(alt.Player.local.scriptID, 1, 169, 0, 0); //Mask
        game.setPedComponentVariation(alt.Player.local.scriptID, 7, 155, 0, 0); //Accessorie
        game.setPedComponentVariation(alt.Player.local.scriptID, 8, 191, 2, 0); //Undershirt
        game.setPedComponentVariation(alt.Player.local.scriptID, 9, 63, 1, 0); //Body Armor
    } else if (item.Text == 'Zivil 1 -M-') {
        game.setPedComponentVariation(alt.Player.local.scriptID, 1, 169, 0, 0); //Mask
        game.setPedComponentVariation(alt.Player.local.scriptID, 9, 55, 0, 0); //Body Armor
        game.setPedComponentVariation(alt.Player.local.scriptID, 8, 186, 0, 0); //Undershirt
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
subMenu2.AddItem(new NativeUI.UIMenuItem("Zivil 1 -F-", ""));
//ON SELECT
subMenu2.ItemSelect.on((item) => {
    if (item.Text == 'Uniform 1 -F-') {
        game.setPedComponentVariation(alt.Player.local.scriptID, 1, 52, 0, 0); //Mask
        game.setPedComponentVariation(alt.Player.local.scriptID, 3, 111, 0, 0); //Torso
        game.setPedComponentVariation(alt.Player.local.scriptID, 4, 136, 1, 0); //Leg
        game.setPedComponentVariation(alt.Player.local.scriptID, 6, 24, 0, 0); //Shoe
        game.setPedComponentVariation(alt.Player.local.scriptID, 7, 124, 0, 0); //Accessorie
        game.setPedComponentVariation(alt.Player.local.scriptID, 8, 223, 2, 0); //Undershirt
        game.setPedComponentVariation(alt.Player.local.scriptID, 9, 57, 0, 0); //Body Armor
        game.setPedComponentVariation(alt.Player.local.scriptID, 11, 351, 3, 0); //Tops
        game.setPedPropIndex(alt.Player.local.scriptID, 0, 155, 2, false); //Hats
    } else if (item.Text == 'Zivil 1 -F-') {
        game.setPedComponentVariation(alt.Player.local.scriptID, 1, 169, 0, 0); //Mask
        game.setPedComponentVariation(alt.Player.local.scriptID, 9, 53, 0, 0); //Body Armor
        game.setPedComponentVariation(alt.Player.local.scriptID, 8, 224, 0, 0); //Undershirt
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

alt.onServer("Client:FBI:openClotes", () => {
    if (alt.Player.local.getSyncedMeta("IsCefOpen") == false) {
        if (menu.Visible)
            menu.Close();
        else
            menu.Open();
    }
});