import * as alt from 'alt-client';
import * as game from 'natives';
import * as NativeUI from './includes/NativeUI/NativeUI';
import { setClothes } from './utilities.js';
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
subMenu.AddItem(new NativeUI.UIMenuItem("Standard Uniform -M-", ""));
subMenu.AddItem(new NativeUI.UIMenuItem("Standard Uniform 1 -M-", ""));
subMenu.AddItem(new NativeUI.UIMenuItem("Swat Uniform -M-", ""));
//ON SELECT
subMenu.ItemSelect.on((item) => {
    if (item.Text == 'Standard Uniform -M-') {
        /*game.setPedComponentVariation(alt.Player.local.scriptID, 3, 30, 0, 2); //Torso
        game.setPedComponentVariation(alt.Player.local.scriptID, 4, 31, 0, 2); //Legs
        game.setPedComponentVariation(alt.Player.local.scriptID, 6, 60, 0, 2); //Shoes
        game.setPedComponentVariation(alt.Player.local.scriptID, 8, 186, 0, 2); //Undershirts
        game.setPedComponentVariation(alt.Player.local.scriptID, 9, 63, 0, 2); //Armors
        game.setPedComponentVariation(alt.Player.local.scriptID, 11, 55, 0, 2); //Tops
        game.setPedPropIndex(alt.Player.local.scriptID, 0, 142, 0, true); //Hat*/
        setDlcClothes(3238703990, 11, 336, 3);
        setClothes(alt.Player.local.scriptID, 3, 30, 0); //Torso
        setClothes(alt.Player.local.scriptID, 4, 31, 0); //Legs
        setClothes(alt.Player.local.scriptID, 6, 60, 0); //Shoes
        setClothes(alt.Player.local.scriptID, 8, 122, 0); //Undershirts
        setClothes(alt.Player.local.scriptID, 9, 63, 0); //Armors
        setClothes(alt.Player.local.scriptID, 11, 336, 3); //Tops
    } else if (item.Text == 'Standard Uniform 1 -M-') {
        setClothes(alt.Player.local.scriptID, 3, 38, 0); //Torso
        setClothes(alt.Player.local.scriptID, 4, 31, 0); //Legs
        setClothes(alt.Player.local.scriptID, 6, 60, 0); //Shoes
        setClothes(alt.Player.local.scriptID, 8, 122, 0); //Undershirts
        setClothes(alt.Player.local.scriptID, 9, 63, 0); //Armors
        setClothes(alt.Player.local.scriptID, 11, 316, 0); //Tops
        SetProps(alt.Player.local.scriptID, 0, 142, 0); //Hat
    } else if (item.Text == 'Swat Uniform -M-') {
        setClothes(alt.Player.local.scriptID, 3, 30, 0); //Torso
        setClothes(alt.Player.local.scriptID, 4, 31, 0); //Legs
        setClothes(alt.Player.local.scriptID, 6, 60, 0); //Shoes
        setClothes(alt.Player.local.scriptID, 8, 122, 0); //Undershirts
        setClothes(alt.Player.local.scriptID, 9, 61, 0); //Armors
        setClothes(alt.Player.local.scriptID, 11, 336, 3); //Tops
        SetProps(alt.Player.local.scriptID, 0, 150, 0); //Hat
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
subMenu2.AddItem(new NativeUI.UIMenuItem("Standard Uniform -F-", ""));
subMenu2.AddItem(new NativeUI.UIMenuItem("Standard Uniform 1 -F-", ""));
subMenu2.AddItem(new NativeUI.UIMenuItem("Swat Uniform -F-", ""));
//ON SELECT
subMenu2.ItemSelect.on((item) => {
    if (item.Text == 'Standard Uniform -F-') {
        setClothes(alt.Player.local.scriptID, 3, 30, 0); //Torso
        setClothes(alt.Player.local.scriptID, 4, 31, 0); //Legs
        setClothes(alt.Player.local.scriptID, 6, 60, 0); //Shoes
        setClothes(alt.Player.local.scriptID, 8, 186, 0); //Undershirts
        setClothes(alt.Player.local.scriptID, 9, 63, 0); //Armors
        setClothes(alt.Player.local.scriptID, 11, 55, 0); //Tops
    } else if (item.Text == 'Standard Uniform 1 -F-') {
        setClothes(alt.Player.local.scriptID, 3, 30, 0); //Torso
        setClothes(alt.Player.local.scriptID, 4, 31, 0); //Legs
        setClothes(alt.Player.local.scriptID, 6, 60, 0); //Shoes
        setClothes(alt.Player.local.scriptID, 8, 186, 0); //Undershirts
        setClothes(alt.Player.local.scriptID, 9, 63, 0); //Armors
        setClothes(alt.Player.local.scriptID, 11, 55, 0); //Tops
    } else if (item.Text == 'Swat Uniform -F-') {
        setClothes(alt.Player.local.scriptID, 3, 30, 0); //Torso
        setClothes(alt.Player.local.scriptID, 4, 31, 0); //Legs
        setClothes(alt.Player.local.scriptID, 6, 60, 0); //Shoes
        setClothes(alt.Player.local.scriptID, 8, 186, 0); //Undershirts
        setClothes(alt.Player.local.scriptID, 9, 63, 0); //Armors
        setClothes(alt.Player.local.scriptID, 11, 55, 0); //Tops
    }
});

/*alt.on('keyup', (key) => {
    if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true || alt.Player.local.getSyncedMeta("IsUnconscious") == true || alt.Player.local.getSyncedMeta("IsCefOpen") == true || alt.Player.local.vehicle) return;
    if (key === 113) {
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