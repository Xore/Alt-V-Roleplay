import * as alt from 'alt-client';
import * as game from 'natives';
import * as NativeUI from './includes/NativeUI/NativeUI';
import { AvailableDances, AvailableAnimations, AvailableWalks, AvailableAnimals, AvailableLays, AvailableSits } from './includes/newemotes.js';
let prop = null;
const MenuSettings = {
    TitleFont: NativeUI.Font.Monospace,
}
const menu = new NativeUI.Menu("Animationsmenu", "", new NativeUI.Point(10, 10));
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
menu.SetRectangleBannerType(banner);
menu.GetTitle().Scale = 0.9;
menu.GetTitle().Font = MenuSettings.TitleFont;
menu.GetTitle().DropShadow = true;
menu.AddItem(new NativeUI.UIMenuItem("~r~Stop", ""));

let menuDanceItem = new NativeUI.UIMenuItem("Tanze", "Alle verfügbaren Tanze.");
menu.AddItem(menuDanceItem);

const DanceMenu = new NativeUI.Menu("Tanze", "Tanz:", new NativeUI.Point(10, 10));
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
DanceMenu.SetRectangleBannerType(banner);
DanceMenu.Visible = false;
DanceMenu.GetTitle().Scale = 0.9;
DanceMenu.GetTitle().Font = MenuSettings.TitleFont;

menu.AddSubMenu(DanceMenu, menuDanceItem);

AvailableDances.forEach(element => {
    let DanceItem = new NativeUI.UIMenuItem(element.name, "Alle verfügbaren Tanze.");
    DanceMenu.AddItem(DanceItem);
});

let menuWalkItem = new NativeUI.UIMenuItem("Gehstile", "Alle verfügbaren Gehstile.");
menu.AddItem(menuWalkItem);

const WalkMenu = new NativeUI.Menu("Gehstile", "Gehstil:", new NativeUI.Point(10, 10));
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
WalkMenu.SetRectangleBannerType(banner);
WalkMenu.Visible = false;
WalkMenu.GetTitle().Scale = 0.9;
WalkMenu.GetTitle().Font = MenuSettings.TitleFont;

menu.AddSubMenu(WalkMenu, menuWalkItem);

AvailableWalks.forEach(element => {
    let WalkItem = new NativeUI.UIMenuItem(element.name, "Alle verfügbaren Gehstile.");
    WalkMenu.AddItem(WalkItem);
});

let menuAniamtionItem = new NativeUI.UIMenuItem("Animationen", "Alle verfügbaren Animationen.");
menu.AddItem(menuAniamtionItem);

const AnimationMenu = new NativeUI.Menu("Animationen", "Animation:", new NativeUI.Point(10, 10));
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
AnimationMenu.SetRectangleBannerType(banner);
AnimationMenu.Visible = false;
AnimationMenu.GetTitle().Scale = 0.9;
AnimationMenu.GetTitle().Font = MenuSettings.TitleFont;

menu.AddSubMenu(AnimationMenu, menuAniamtionItem);

AvailableAnimations.forEach(element => {
    let DanceItem = new NativeUI.UIMenuItem(element.name, "Alle verfügbaren Animationen.");
    AnimationMenu.AddItem(DanceItem);
});

let menuSitItem = new NativeUI.UIMenuItem("Sitz Animationen", "Alle verfügbaren Sitz Animationen.");
menu.AddItem(menuSitItem);

const SitMenu = new NativeUI.Menu("Sitzen", "Liege:", new NativeUI.Point(10, 10));
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
SitMenu.SetRectangleBannerType(banner);
SitMenu.Visible = false;
SitMenu.GetTitle().Scale = 0.9;
SitMenu.GetTitle().Font = MenuSettings.TitleFont;

menu.AddSubMenu(SitMenu, menuSitItem);

AvailableSits.forEach(element => {
    let SitItem = new NativeUI.UIMenuItem(element.name, "Alle verfügbaren Sitz Animationen.");
    SitMenu.AddItem(SitItem);
});

let menuLayItem = new NativeUI.UIMenuItem("Liege Animationen", "Alle verfügbaren Liege Animationen.");
menu.AddItem(menuLayItem);

const LayMenu = new NativeUI.Menu("Liegen", "Liege:", new NativeUI.Point(10, 10));
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
LayMenu.SetRectangleBannerType(banner);
LayMenu.Visible = false;
LayMenu.GetTitle().Scale = 0.9;
LayMenu.GetTitle().Font = MenuSettings.TitleFont;

menu.AddSubMenu(LayMenu, menuLayItem);

AvailableLays.forEach(element => {
    let LayItem = new NativeUI.UIMenuItem(element.name, "Alle verfügbaren Liege Animationen.");
    LayMenu.AddItem(LayItem);
});

let menuAnimalItem = new NativeUI.UIMenuItem("Tier Animationen", "Alle verfügbaren Tier Animationen.");
menu.AddItem(menuAnimalItem);
const AnimalMenu = new NativeUI.Menu("Tier", "Tier:", new NativeUI.Point(10, 10));
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
AnimalMenu.SetRectangleBannerType(banner);
AnimalMenu.Visible = false;
AnimalMenu.GetTitle().Scale = 0.9;
AnimalMenu.GetTitle().Font = MenuSettings.TitleFont;

menu.AddSubMenu(AnimalMenu, menuAnimalItem);

AvailableAnimals.forEach(element => {
    let AnimalItem = new NativeUI.UIMenuItem(element.name, "Alle verfügbaren Tier Animationen.");
    AnimalMenu.AddItem(AnimalItem);
});

menu.ItemSelect.on((item, selectedItemIndex) => {
    if (item instanceof NativeUI.UIMenuItem && item.Text == "~r~Stop") {
        game.clearPedTasks(alt.Player.local.scriptID);
        game.resetPedMovementClipset(alt.Player.local.scriptID, 0);
        if (!prop || prop == null) return;
        alt.setTimeout(() => {
            game.detachEntity(prop, true, false);
            game.deleteObject(prop);
            prop = null;
        }, 800);
    } else {

    }
});

DanceMenu.ItemSelect.on((item, selectedItemIndex) => {
    if (item instanceof NativeUI.UIMenuItem && selectedItemIndex < AvailableDances.length) {

        let SelectedDance = AvailableDances[selectedItemIndex];


        playAnimation(SelectedDance.dict, SelectedDance.anim, 1, 300000);
    } else {
        //alt.log("[ItemSelect] " + selectedItemIndex, selectedItem.Text);
    }
});

AnimationMenu.ItemSelect.on((item, selectedItemIndex) => {
    if (item instanceof NativeUI.UIMenuItem && selectedItemIndex < AvailableAnimations.length) {

        let SelectedAnimation = AvailableAnimations[selectedItemIndex];

        playAnimation(SelectedAnimation.dict, SelectedAnimation.anim, 1, SelectedAnimation.EmoteDuration);
    } else {
        //alt.log("[ItemSelect] " + selectedItemIndex, selectedItem.Text);
    }
});

LayMenu.ItemSelect.on((item, selectedItemIndex) => {
    if (item instanceof NativeUI.UIMenuItem && selectedItemIndex < AvailableLays.length) {

        let SelectedLay = AvailableLays[selectedItemIndex];

        playAnimation(SelectedLay.dict, SelectedLay.anim, 1, SelectedLay.EmoteDuration);
    } else {
        //alt.log("[ItemSelect] " + selectedItemIndex, selectedItem.Text);
    }
});

SitMenu.ItemSelect.on((item, selectedItemIndex) => {
    if (item instanceof NativeUI.UIMenuItem && selectedItemIndex < AvailableSits.length) {

        let SelectedSit = AvailableSits[selectedItemIndex];

        playAnimation(SelectedSit.dict, SelectedSit.anim, 1, SelectedSit.EmoteDuration);
    } else {
        //alt.log("[ItemSelect] " + selectedItemIndex, selectedItem.Text);
    }
});

AnimalMenu.ItemSelect.on((item, selectedItemIndex) => {
    if (item instanceof NativeUI.UIMenuItem && selectedItemIndex < AvailableAnimals.length) {

        let SelectedAnimal = AvailableAnimals[selectedItemIndex];

        playAnimation(SelectedAnimal.dict, SelectedAnimal.anim, 1, SelectedAnimal.EmoteDuration);
    } else {
        //alt.log("[ItemSelect] " + selectedItemIndex, selectedItem.Text);
    }
});

WalkMenu.ItemSelect.on((item, selectedItemIndex) => {
    if (item instanceof NativeUI.UIMenuItem && selectedItemIndex < AvailableWalks.length) {

        let SelectedWalk = AvailableWalks[selectedItemIndex];

        playWalking(SelectedWalk.dict);
    } else {
        //alt.log("[ItemSelect] " + selectedItemIndex, selectedItem.Text);
    }
});

//Keys
alt.on('keyup', (key) => {
    if (key == 96) { //Numpad0 
        game.clearPedTasks(alt.Player.local.scriptID);
    }
});
alt.on('keyup', (key) => {
    if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true || alt.Player.local.getSyncedMeta("IsUnconscious") == true || alt.Player.local.getSyncedMeta("IsCefOpen") == true) return;
    if (key === 113) {
        if (menu.Visible || DanceMenu.Visible || AnimationMenu.Visible) {
            menu.Close();
            DanceMenu.Close();
            AnimationMenu.Close();
        } else {
            menu.Open();
        }
    }
});
//PlayDemShit
function playWalking(anim) {
    if (anim == undefined) return;
    if (anim == "normal") {
        game.resetPedMovementClipset(alt.Player.local.scriptID, 0);
        return;
    }
    game.requestAnimSet(anim);
    let interval = alt.setInterval(() => {
        if (game.hasAnimDictLoaded(anim)) {
            alt.clearInterval(interval);
            game.setPedMovementClipset(alt.Player.local.scriptID, anim, 0.2);
        }
    }, 0);
}

function playAnimation(animDict, animName, animFlag, animDuration) {
    if (animDict == undefined || animName == undefined || animFlag == undefined || animDuration == undefined) return;
    game.requestAnimDict(animDict);
    let interval = alt.setInterval(() => {
        if (game.hasAnimDictLoaded(animDict)) {
            alt.clearInterval(interval);
            game.taskPlayAnim(alt.Player.local.scriptID, animDict, animName, 8.0, 1, animDuration, animFlag, 1, false, false, false);
        }
    }, 0);
}