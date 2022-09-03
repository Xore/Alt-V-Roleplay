import alt from "alt"
import native from "natives"
import * as NativeUI from './includes/NativeUI/NativeUI';
import { Menu, UIMenuItem, UIMenuListItem, UIMenuCheckboxItem, UIMenuSliderItem, BadgeStyle, Point, ItemsCollection, Color, ListItem } from "./nativeui/nativeui.min.js"
import { AvailableWalks } from './includes/emotes.js';

const configFile = "/client/config.json"
const keysForBindingDisplay = ["-"]
const keysForBinding = [0]
const menus = {}
let keyBindings = {}
let config = null
let selectedIndex = 0
let selectedKey = 0
let lastAnimation = null

const loadConfig = () => {
    const exists = alt.File.exists(configFile)

    if (!exists) {
        //alt.logError("[Animations Menü] Datei config.json existiert im client Order nicht!")
        return false
    }

    try {
        config = JSON.parse(alt.File.read(configFile))
    } catch (error) {
        //alt.logError("[Animations Menü] Fehler beim laden der config.json! (Format richtig?)")
        return false
    }

    return true
}

const createMenu = () => {
    const MenuSettings = {
        TitleFont: NativeUI.Font.Monospace,
    }
    const mainMenu = new Menu("Animationsmenu", "", new Point(10, 10))
    mainMenu.AddItem(new UIMenuItem("~r~Stop", "", 0))
    var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
    mainMenu.SetRectangleBannerType(banner);
    mainMenu.GetTitle().Scale = 0.9;
    mainMenu.GetTitle().Font = MenuSettings.TitleFont;
    mainMenu.GetTitle().DropShadow = true;

    let menuWalkItem = new NativeUI.UIMenuItem("Gehstile", "");
    mainMenu.AddItem(menuWalkItem);
    const WalkMenu = new Menu("Gehstile", "", new Point(10, 10))
    var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
    WalkMenu.SetRectangleBannerType(banner);
    WalkMenu.Visible = false;
    WalkMenu.GetTitle().Scale = 0.9;
    WalkMenu.GetTitle().Font = MenuSettings.TitleFont;
    mainMenu.AddSubMenu(WalkMenu, menuWalkItem);
    AvailableWalks.forEach(element => {
        let WalkItem = new NativeUI.UIMenuItem(element.name, "");
        WalkMenu.AddItem(WalkItem);
    });
    WalkMenu.ItemSelect.on((item, selectedItemIndex) => {
        if (item instanceof NativeUI.UIMenuItem && selectedItemIndex < AvailableWalks.length) {

            let SelectedWalk = AvailableWalks[selectedItemIndex];

            playWalking(SelectedWalk.dict);
        } else {
            //alt.log("[ItemSelect] " + selectedItemIndex, selectedItem.Text);
        }
    });

    function playWalking(anim) {
        native.requestAnimSet(anim);
        let interval = alt.setInterval(() => {
            if (native.hasAnimDictLoaded(anim)) {
                alt.clearInterval(interval);
                native.setPedMovementClipset(alt.Player.local.scriptID, anim, 0.2);
            }
        }, 0);
    }

    for (const index in config.animations) {
        const animation = config.animations[index]
        let savedIndex = 0

        for (const key in keyBindings) {
            if (index != keyBindings[key]) continue

            for (const i in keysForBinding) {
                if (keysForBinding[i] != key) continue

                savedIndex = i
                break
            }
            break
        }

        if (animation[5] == undefined)
            continue

        let animationMenu = menus[animation[5]]
        if (animationMenu == undefined) {
            animationMenu = new Menu("Animationsmenu", "", new Point(10, 10))
            var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
            animationMenu.SetRectangleBannerType(banner);
            animationMenu.GetTitle().Scale = 0.9;
            animationMenu.GetTitle().Font = MenuSettings.TitleFont;
            animationMenu.GetTitle().DropShadow = true;
            animationMenu.AddItem(new UIMenuListItem(animation[0], animation[1], new ItemsCollection(keysForBindingDisplay), savedIndex, index))
            menus[animation[5]] = animationMenu

            const categorieItem = new UIMenuItem(animation[5])
            mainMenu.AddItem(categorieItem)
            mainMenu.AddSubMenu(animationMenu, categorieItem)

            animationMenu.ItemSelect.on(async(item, index) => {
                const animation = config.animations[item.Data]
                await playAnimation(animation)
            })

            animationMenu.ListChange.on((item, index) => {
                selectedIndex = item.Data
                selectedKey = keysForBinding[index]
            })

            mainMenu.ItemSelect.on((item, index) => {
                if (item.Data == 0) {
                    playAnimation(null)
                }
            })
        } else {
            animationMenu.AddItem(new UIMenuListItem(animation[0], animation[1], new ItemsCollection(keysForBindingDisplay), savedIndex, index))
        }
    }

    alt.on("keydown", async key => {
        if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true || alt.Player.local.getSyncedMeta("IsUnconscious") == true || alt.Player.local.getSyncedMeta("IsCefOpen") == true) return;
        if (key == config.openKey) {
            if (mainMenu.Visible) {
                mainMenu.Close() //wenn submenu auf und f5 dann geht main menu auf
            } else {
                mainMenu.Open()
                for (const k in menus) {
                    menus[k].Close()
                }
            }
        } else if (key == config.saveKey) {
            saveAnimation(selectedKey, selectedIndex)
            notify("Animation ~g~Gespeichert")
        } else {
            const index = keyBindings[key]
            if (!index) {
                return
            }

            const animation = config.animations[index]
            if (animation) {
                await playAnimation(animation)
            }
        }
    })
    alt.on('keyup', (key) => {
        if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true || alt.Player.local.getSyncedMeta("IsUnconscious") == true || alt.Player.local.getSyncedMeta("IsCefOpen") == true) return;
        if (key == 96) { //Numpad0 
            native.clearPedTasks(alt.Player.local.scriptID);
            native.resetPedMovementClipset(alt.Player.local.scriptID, 0);
        }
    });
}

const notify = message => {
    native.beginTextCommandThefeedPost("STRING")
    native.addTextComponentSubstringPlayerName(message)
    native.endTextCommandThefeedPostTicker(false, false)
}

const saveAnimation = (key, index) => {
    for (const k in keyBindings) {
        if (keyBindings[k] == index) {
            delete keyBindings[k]
        }
    }

    keyBindings[key] = index
    alt.LocalStorage.set("keyBindings", keyBindings)
    alt.LocalStorage.save();
}

const loadKeyBindings = () => {
    let bindings = alt.LocalStorage.get("keyBindings")

    if (bindings != null) {
        keyBindings = bindings
    }

    for (let i = 1; i < 10; i++) {
        keysForBinding.push([0x60 + i])
        keysForBindingDisplay.push(i)
    }
}

const initDebugCommands = () => {
    alt.on("consoleCommand", async(name, ...args) => {
        if (name == "anim") {
            await playAnimation(["", "", args[0], args[1], args[2]])
                //notify("Animation ~g~gestartet")
        } else if (name == "stop") {
            native.clearPedTasks(alt.Player.local.scriptID)
                //notify("Animation ~r~gestoppt")
        }
    })
}

const playAnimation = async animation => {
    if (lastAnimation) {
        native.stopAnimTask(alt.Player.local.scriptID, lastAnimation[2], lastAnimation[3], lastAnimation[4])
    }

    if (animation == null) {
        //notify("Animation ~r~gestoppt")
        return
    }

    await loadAnimation(animation[2])
    native.taskPlayAnim(alt.Player.local.scriptID, animation[2], animation[3], 8, -4, -1, animation[4], 0, false, false, false)
    native.removeAnimDict(animation[2])
    lastAnimation = animation
        //notify("Animation ~g~gestartet")
}

const loadAnimation = async(animDict) => {
    return new Promise((resolve, reject) => {
        native.requestAnimDict(animDict)

        const interval = alt.setInterval(() => {
            if (native.hasAnimDictLoaded(animDict)) {
                alt.clearInterval(interval)
                return resolve(true)
            }
        }, 0)
    })
}

const loaded = loadConfig()

if (loaded) {
    loadKeyBindings()
    createMenu()

    if (config.debug) {
        //alt.logWarning("[Animations Menü] Debug Modus ist an!")
        initDebugCommands()
    }
}