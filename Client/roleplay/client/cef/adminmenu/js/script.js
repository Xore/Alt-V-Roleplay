    let adminmenu_selectedfield = "player";
    let adminmenu_selectedmenu = "main";

    let activatedmodulespushed = false;
    let selecteduser = "None";

    const adminmenu_main_items = ["player", "online", "misc", "fahrzeug", "peds", "server"];
    const adminmenu_player_items = ["noclip", "unsichtbar", "adminoutfit", "godmode", "heilen", "wiederbeleben"];
    const adminmenu_player_items_type = ["onoff", "onoff", "onoff", "onoff", "button", "button"];
    var adminmenu_online_items = [];
    const adminmenu_online_action_items = ["spieler_kicken", "spieler_bannen", "spieler_einfrieren", "spieler_spectaten", "spieler_heilen", "spieler_wiederbeleben", "tp_zu_spieler", "spieler_zu_mir_tp", "item_geben", "adminlevel_geben"];
    const adminmenu_online_action_items_type = ["button", "button", "onoff", "onoff", "button", "button", "button", "button", "button", "button"];
    const adminmenu_misc_items = ["zum_wegpunkt", "nametags", "spieler_auf_der_karte_anzeigen"];
    const adminmenu_misc_items_type = ["button", "onoff", "onoff"];
    const adminmenu_fahrzeug_items = ["fahrzeug_spawnen", "reparieren", "fahrzeug_löschen"];
    const adminmenu_fahrzeug_items_type = ["button", "button", "button"];
    const adminmenu_peds_items = ["zurücksetzen", "a_c_boar", "a_c_cat_01", "a_c_chimp", "a_c_chop", "a_c_cow", "a_c_coyote", "a_c_crow", "a_c_husky", "a_c_mtlion", "a_c_poodle", "a_c_pug", "a_c_rabbit_01", "a_c_retriever", "a_c_shepherd", "a_c_westy", "other"];
    const adminmenu_peds_items_type = ["button", "button", "button", "button", "button", "button", "button", "button", "button", "button", "button", "button", "button", "button", "button", "button", "button"];
    const adminmenu_server_items = ["ankündigung", "whitelist", "fahrzeug_einparken", "alle_fahrzeuge_einparken", "hardwareid_zurücksetzen", "socialclubid_zurücksetzen"];
    const adminmenu_server_items_type = ["button", "button", "button", "button", "button", "button"];

    let activatedmodules_player = [];
    let activatedmodules_online_action = [];
    let activatedmodules_misc = [];
    let activatedmodules_fahrzeug = [];
    let activatedmodules_peds = [];
    let activatedmodules_server = [];

    function openadminmenu() {
        $(`.adminmenu-main`).show();
        if (!activatedmodulespushed) {

            activatedmodulespushed = true;
            adminmenu_main_items.forEach(a => {
                if (a == "online") a = "online_action";
                let html = "";

                eval("adminmenu_" + a + "_items").forEach(b => {
                    eval("activatedmodules_" + a).push(0);
                    let addinfo = "";
                    if (eval("adminmenu_" + a + "_items_type")[eval("adminmenu_" + a + "_items").indexOf(b)] == "onoff") addinfo = "[AUS]";

                    html += `<li class="${eval("adminmenu_" + a + "_items_type")[eval("adminmenu_" + a + "_items").indexOf(b)]} adminmenu-main-list-item adminmenu-${a}-list-item-${b} off">${b.toUpperCase().replace(/_/g, " ")} ${addinfo}</li>`;
                })

                $(`#adminmenu-${a}-list`).html(html);
            });
        }
    };

    function closeadminmenu() {
        $(`.adminmenu-main`).hide();
    };

    function ChangeSelectedField(UpOrDown) {
        if (UpOrDown == 'up') {
            if (adminmenu_selectedmenu == "main" && adminmenu_selectedfield != adminmenu_main_items[0] && adminmenu_selectedfield != "inputfield") {
                $(`.adminmenu-main-list-item`).removeClass("selected");
                $(`.adminmenu-main-list-item-${adminmenu_main_items[adminmenu_main_items.indexOf(adminmenu_selectedfield) - 1]}`).addClass("selected");
                adminmenu_selectedfield = adminmenu_main_items[adminmenu_main_items.indexOf(adminmenu_selectedfield) - 1];
            } else if (adminmenu_selectedfield == "inputfield") {
                alt.emit("Client:AdminMenu:SetGameControls", false);
                $(`.adminmenu-input`).blur();
                $(`.adminmenu-input`).removeClass("selectedinput");
                if (adminmenu_selectedmenu == "main") {
                    $(`.adminmenu-main-list-item-${adminmenu_main_items[adminmenu_main_items.length - 1]}`).addClass("selected");
                    adminmenu_selectedfield = adminmenu_main_items[adminmenu_main_items.length - 1];
                    if (adminmenu_main_items.length > 8) $(".adminmenu-main-list").scrollTop($(".adminmenu-main-list").height());
                } else if (adminmenu_selectedmenu == "onlineaction") {
                    $(`.adminmenu-online_action-list-item-${adminmenu_online_action_items[adminmenu_online_action_items.length - 1]}`).addClass("selected");
                    adminmenu_selectedfield = adminmenu_online_action_items[adminmenu_online_action_items.length - 1];
                    if (adminmenu_online_action_items.length > 8) $(".adminmenu-online_action-list").scrollTop($(".adminmenu-online_action-list").height());
                } else {
                    $(`.adminmenu-${adminmenu_selectedmenu}-list-item-${eval("adminmenu_" + adminmenu_selectedmenu + "_items")[eval("adminmenu_" + adminmenu_selectedmenu + "_items").length - 1]}`).addClass("selected");
                    adminmenu_selectedfield = eval("adminmenu_" + adminmenu_selectedmenu + "_items")[eval("adminmenu_" + adminmenu_selectedmenu + "_items").length - 1];
                    if (eval("adminmenu_" + adminmenu_selectedmenu + "_items").length > 8) $(`.adminmenu-${adminmenu_selectedmenu}-list`).scrollTop($(`.adminmenu-${adminmenu_selectedmenu}-list`).height());
                }
            } else if (adminmenu_selectedfield == adminmenu_main_items[0]) {
                $(`.adminmenu-main-list-item`).removeClass("selected");
                $(`.adminmenu-input`).addClass("selectedinput");
                adminmenu_selectedfield = "inputfield";
                $(`.adminmenu-input`).focus();
                alt.emit("Client:AdminMenu:SetGameControls", true);
            } else {
                if (adminmenu_selectedmenu == "onlineaction" && adminmenu_selectedfield != adminmenu_online_action_items[0]) {
                    $(`.adminmenu-main-list-item`).removeClass("selected");
                    $(`.${"adminmenu-online_action-list-item"}-${eval("adminmenu_online_action_items")[eval("adminmenu_online_action_items").indexOf(adminmenu_selectedfield) - 1]}`).addClass("selected");
                    adminmenu_selectedfield = eval("adminmenu_online_action_items")[eval("adminmenu_online_action_items").indexOf(adminmenu_selectedfield) - 1];
                    if (eval("adminmenu_online_action_items").indexOf(adminmenu_selectedfield) < (eval("adminmenu_online_action_items").length - 8)) {
                        document.getElementById(`adminmenu-online_action-list`).scrollBy(0, -3.45 * window.innerHeight / 100);
                    }
                } else if (adminmenu_selectedmenu == "onlineaction" && adminmenu_selectedfield == adminmenu_online_action_items[0]) {
                    $(`.adminmenu-main-list-item`).removeClass("selected");
                    $(`.adminmenu-input`).addClass("selectedinput");
                    adminmenu_selectedfield = "inputfield";
                    $(`.adminmenu-input`).focus();
                    alt.emit("Client:AdminMenu:SetGameControls", true);
                } else if (adminmenu_selectedmenu != "onlineaction") {
                    adminmenu_main_items.forEach(b => {
                        if (adminmenu_selectedmenu == b && adminmenu_selectedfield != eval("adminmenu_" + b + "_items")[0]) {
                            $(`.adminmenu-main-list-item`).removeClass("selected");
                            $(`.${"adminmenu-" + b + "-list-item"}-${eval("adminmenu_" + b + "_items")[eval("adminmenu_" + b + "_items").indexOf(adminmenu_selectedfield) - 1]}`).addClass("selected");
                            adminmenu_selectedfield = eval("adminmenu_" + b + "_items")[eval("adminmenu_" + b + "_items").indexOf(adminmenu_selectedfield) - 1];
                            if (eval("adminmenu_" + b + "_items").indexOf(adminmenu_selectedfield) < (eval("adminmenu_" + b + "_items").length - 8)) {
                                document.getElementById(`adminmenu-${b}-list`).scrollBy(0, -3.45 * window.innerHeight / 100);
                            }
                        } else if (adminmenu_selectedmenu == b && adminmenu_selectedfield == eval("adminmenu_" + b + "_items")[0]) {
                            $(`.adminmenu-main-list-item`).removeClass("selected");
                            $(`.adminmenu-input`).addClass("selectedinput");
                            adminmenu_selectedfield = "inputfield";
                            $(`.adminmenu-input`).focus();
                            alt.emit("Client:AdminMenu:SetGameControls", true);
                        }
                    })
                }
            }
        } else {
            if (adminmenu_selectedfield == "inputfield") {
                alt.emit("Client:AdminMenu:SetGameControls", false);
                $(`.adminmenu-input`).blur();
                $(`.adminmenu-input`).removeClass("selectedinput");
                if (adminmenu_selectedmenu == "main") {
                    $(`.adminmenu-main-list-item-${adminmenu_main_items[0]}`).addClass("selected");
                    adminmenu_selectedfield = adminmenu_main_items[0];
                } else if (adminmenu_selectedmenu == "onlineaction") {
                    $(`.adminmenu-online_action-list-item-${adminmenu_online_action_items[0]}`).addClass("selected");
                    adminmenu_selectedfield = adminmenu_online_action_items[0];
                } else {
                    $(`.adminmenu-${adminmenu_selectedmenu}-list-item-${eval("adminmenu_" + adminmenu_selectedmenu + "_items")[0]}`).addClass("selected");
                    adminmenu_selectedfield = eval("adminmenu_" + adminmenu_selectedmenu + "_items")[0];
                }
            } else if (adminmenu_selectedmenu == "main" && adminmenu_selectedfield != adminmenu_main_items[adminmenu_main_items.length - 1] && adminmenu_selectedfield != "inputfield") {
                $(`.adminmenu-main-list-item`).removeClass("selected");
                $(`.adminmenu-main-list-item-${adminmenu_main_items[adminmenu_main_items.indexOf(adminmenu_selectedfield) + 1]}`).addClass("selected");
                adminmenu_selectedfield = adminmenu_main_items[adminmenu_main_items.indexOf(adminmenu_selectedfield) + 1];
            } else if (adminmenu_selectedmenu == "main" && adminmenu_selectedfield == adminmenu_main_items[adminmenu_main_items.length - 1] && adminmenu_selectedfield != "inputfield") {
                alt.emit("Client:AdminMenu:SetGameControls", true);
                $(`.adminmenu-input`).focus();
                $(`.adminmenu-main-list-item`).removeClass("selected");
                $(`.adminmenu-input`).addClass("selectedinput");
                adminmenu_selectedfield = "inputfield";
            } else if (adminmenu_selectedfield != "inputfield") {
                if (adminmenu_selectedmenu == "onlineaction" && adminmenu_selectedfield != adminmenu_online_action_items[adminmenu_online_action_items.length - 1]) {
                    $(`.adminmenu-main-list-item`).removeClass("selected");
                    $(`.${"adminmenu-online_action-list-item"}-${eval("adminmenu_online_action_items")[eval("adminmenu_online_action_items").indexOf(adminmenu_selectedfield) + 1]}`).addClass("selected");
                    adminmenu_selectedfield = eval("adminmenu_online_action_items")[eval("adminmenu_online_action_items").indexOf(adminmenu_selectedfield) + 1];
                    if (eval("adminmenu_online_action_items").indexOf(adminmenu_selectedfield) > 8) {
                        document.getElementById(`adminmenu-online_action-list`).scrollBy(0, 3.45 * window.innerHeight / 100);
                    }
                } else if (adminmenu_selectedmenu == "onlineaction" && adminmenu_selectedfield == adminmenu_online_action_items[adminmenu_online_action_items.length - 1]) {
                    alt.emit("Client:AdminMenu:SetGameControls", true);
                    $(`.adminmenu-input`).focus();
                    $(`.adminmenu-main-list-item`).removeClass("selected");
                    $(`.adminmenu-input`).addClass("selectedinput");
                    adminmenu_selectedfield = "inputfield";
                } else if (adminmenu_selectedmenu != "onlineaction") {
                    adminmenu_main_items.forEach(b => {
                        if (adminmenu_selectedmenu == b && adminmenu_selectedfield != eval("adminmenu_" + b + "_items")[eval("adminmenu_" + b + "_items").length - 1]) {
                            $(`.adminmenu-main-list-item`).removeClass("selected");
                            $(`.${"adminmenu-" + b + "-list-item"}-${eval("adminmenu_" + b + "_items")[eval("adminmenu_" + b + "_items").indexOf(adminmenu_selectedfield) + 1]}`).addClass("selected");
                            adminmenu_selectedfield = eval("adminmenu_" + b + "_items")[eval("adminmenu_" + b + "_items").indexOf(adminmenu_selectedfield) + 1];
                            if (eval("adminmenu_" + b + "_items").indexOf(adminmenu_selectedfield) > 8) {
                                document.getElementById(`adminmenu-${b}-list`).scrollBy(0, 3.45 * window.innerHeight / 100);
                            }
                        } else if (adminmenu_selectedmenu == b && adminmenu_selectedfield == eval("adminmenu_" + b + "_items")[eval("adminmenu_" + b + "_items").length - 1]) {
                            alt.emit("Client:AdminMenu:SetGameControls", true);
                            $(`.adminmenu-input`).focus();
                            $(`.adminmenu-main-list-item`).removeClass("selected");
                            $(`.adminmenu-input`).addClass("selectedinput");
                            adminmenu_selectedfield = "inputfield";
                        }
                    })
                }
            }
        }
    }

    function ChangeSelectedMenu() {
        if (adminmenu_selectedmenu == "main") {
            adminmenu_main_items.forEach(a => {
                if (adminmenu_selectedfield == a) {
                    adminmenu_selectedmenu = a;
                    $(`.adminmenu-list`).hide();
                    $(`.${"adminmenu-" + a + "-list"}`).show();
                    adminmenu_selectedfield = eval("adminmenu_" + a + "_items")[0];
                    $(`.adminmenu-main-list-item`).removeClass("selected");
                    $(`.${"adminmenu-" + adminmenu_selectedmenu + "-list-item-" + adminmenu_selectedfield}`).addClass("selected");
                    $(`#adminmenu-${adminmenu_selectedmenu}-list`).scrollTop(0);
                    $(`.adminmenu-main-title`).html(`Adminmenü > ${a.charAt(0).toUpperCase() + a.slice(1)}`);
                    if (adminmenu_selectedmenu == "online") alt.emit("Client:AdminMenu:RequestAllOnlinePlayers");
                }
            })
        } else {
            if (adminmenu_selectedmenu == "online") {
                $(`.adminmenu-list`).hide();
                $(`.${"adminmenu-online_action-list"}`).show();
                selecteduser = adminmenu_selectedfield;
                $(`.adminmenu-main-title`).html(`Adminmenü > ${adminmenu_selectedmenu.charAt(0).toUpperCase() + adminmenu_selectedmenu.slice(1)} > ${selecteduser.charAt(0).toUpperCase() + selecteduser.slice(1)}`);
                adminmenu_selectedmenu = "onlineaction";
                adminmenu_selectedfield = adminmenu_online_action_items[0];
                $(`.adminmenu-main-list-item`).removeClass("selected");
                $(`.${"adminmenu-online_action-list-item-" + adminmenu_selectedfield}`).addClass("selected");
                $(`#adminmenu-${adminmenu_selectedmenu}-list`).scrollTop(0);
                alt.emit("Client:AdminMenu:GetPlayerMeta", selecteduser, adminmenu_online_action_items);
            } else {
                adminmenu_main_items.forEach(a => {
                    eval("adminmenu_" + a + "_items").forEach(b => {
                        if (adminmenu_selectedfield == b) {
                            ChangeSelectedMenuMain(a, b);
                        }
                    })
                })
                adminmenu_online_action_items.forEach(c => {
                    if (adminmenu_selectedfield == c) {
                        ChangeSelectedMenuMain("online_action", c);
                    }
                })
            }
        }
    }

    function ReturnSelectedMenu() {
        if (selecteduser != "None") {
            selecteduser = "None";
            adminmenu_selectedmenu = "online";
            $(`.adminmenu-main-title`).html(`Adminmenü > ${adminmenu_selectedmenu.charAt(0).toUpperCase() + adminmenu_selectedmenu.slice(1)}`);
            alt.emit("Client:AdminMenu:RequestAllOnlinePlayers");
            $(`.adminmenu-list`).hide();
            $(`.adminmenu-online-list`).show();
        } else {
            $(`.adminmenu-main-list-item`).removeClass("selected");
            $(`.adminmenu-main-list-item-player`).addClass("selected");
            $(`.adminmenu-list`).hide();
            $(`.adminmenu-main-list`).show();
            adminmenu_selectedmenu = "main";
            adminmenu_selectedfield = "player";
            $(`.adminmenu-main-title`).html(`Adminmenü`);
        }
    }

    function UseAllOnlinePlayerArray(AllOnlinePlayerArray) {
        adminmenu_online_items = [];
        var html = "";

        AllOnlinePlayerArray = JSON.parse(AllOnlinePlayerArray);

        for (var i in AllOnlinePlayerArray) {
            html += `<li class="adminmenu-main-list-item adminmenu-online-list-item-${AllOnlinePlayerArray[i].username} off">${AllOnlinePlayerArray[i].accid} | ${AllOnlinePlayerArray[i].charid} | ${AllOnlinePlayerArray[i].username} | ${AllOnlinePlayerArray[i].fullname}</li>`;
            adminmenu_online_items.push(AllOnlinePlayerArray[i].username);
        }

        $("#adminmenu-online-list").html(html);

        adminmenu_selectedfield = eval("adminmenu_online_items")[0];
        $(`.adminmenu-main-list-item`).removeClass("selected");
        $(`.${"adminmenu-" + adminmenu_selectedmenu + "-list-item-" + adminmenu_selectedfield}`).addClass("selected");
    }

    function ChangeSelectedMenuMain(a, b) {
        if ($(`.${"adminmenu-" + a + "-list-item-" + b}`).hasClass("onoff")) {
            if (eval("activatedmodules_" + a)[eval("adminmenu_" + a + "_items").indexOf(b)] == 0) {
                eval("activatedmodules_" + a)[eval("adminmenu_" + a + "_items").indexOf(b)] = 1;
                $(`.${"adminmenu-" + a + "-list-item-" + b}`).text(b.toUpperCase() + " [AN]");
                $(`.${"adminmenu-" + a + "-list-item-" + b}`).addClass("on");
                if (a == "online_action") alt.emit("Client:AdminMenu:DoAction", adminmenu_selectedfield, "on", selecteduser, $(`.adminmenu-input`).val());
                else alt.emit("Client:AdminMenu:DoAction", adminmenu_selectedfield, "on", "none", $(`.adminmenu-input`).val());
            } else {
                eval("activatedmodules_" + a)[eval("adminmenu_" + a + "_items").indexOf(b)] = 0;
                $(`.${"adminmenu-" + a + "-list-item-" + b}`).text(b.toUpperCase() + " [AUS]");
                $(`.${"adminmenu-" + a + "-list-item-" + b}`).addClass("off");
                if (a == "online_action") alt.emit("Client:AdminMenu:DoAction", adminmenu_selectedfield, "off", selecteduser, $(`.adminmenu-input`).val());
                else alt.emit("Client:AdminMenu:DoAction", adminmenu_selectedfield, "off", "none", $(`.adminmenu-input`).val());
            }

            if (adminmenu_selectedmenu == "onlineaction" && selecteduser != "None") {
                alt.emit("Client:AdminMenu:SetMeta", selecteduser, b);
            }
        } else if ($(`.${"adminmenu-" + a + "-list-item-" + b}`).hasClass("button")) { if (a == "online_action") alt.emit("Client:AdminMenu:DoAction", adminmenu_selectedfield, "button", selecteduser, $(`.adminmenu-input`).val());
            else alt.emit("Client:AdminMenu:DoAction", adminmenu_selectedfield, "button", "none", $(`.adminmenu-input`).val()); }
    }

    function receiveMeta(metaarray) {
        metaarray.forEach(metaarrayelement => {
            if (adminmenu_online_action_items_type[adminmenu_online_action_items.indexOf(adminmenu_online_action_items[metaarray.indexOf(metaarrayelement)])] == "button") return;
            if (metaarrayelement.includes("yeno")) {
                $(`.${"adminmenu-online_action-list-item-" + adminmenu_online_action_items[metaarray.indexOf(metaarrayelement)]}`).text(adminmenu_online_action_items[metaarray.indexOf(metaarrayelement)].toUpperCase().replace(/_/g, " ") + " [AUS]");
                activatedmodules_online_action[adminmenu_online_action_items.indexOf(metaarray.indexOf(metaarrayelement))] = 0;
            } else if (metaarrayelement.includes("yaes")) {
                $(`.${"adminmenu-online_action-list-item-" + adminmenu_online_action_items[metaarray.indexOf(metaarrayelement)]}`).text(adminmenu_online_action_items[metaarray.indexOf(metaarrayelement)].toUpperCase().replace(/_/g, " ") + " [AN]");
                activatedmodules_online_action[adminmenu_online_action_items.indexOf(metaarray.indexOf(metaarrayelement))] = 1;
            }
        })
    }

    alt.on("CEF:AdminMenu:OpenMenu", () => {
        openadminmenu();
    });
    alt.on("CEF:AdminMenu:CloseMenu", () => {
        closeadminmenu();
    });
    alt.on("CEF:AdminMenu:ChangeSelectedField", (UpOrDown) => {
        ChangeSelectedField(UpOrDown);
    });
    alt.on("CEF:AdminMenu:ChangeSelectedMenu", () => {
        ChangeSelectedMenu();
    });
    alt.on("CEF:AdminMenu:ReturnSelectedMenu", () => {
        ReturnSelectedMenu();
    });
    alt.on("CEF:AdminMenu:AllOnlinePlayerArray", (AllOnlinePlayerArray) => {
        UseAllOnlinePlayerArray(AllOnlinePlayerArray);
    });
    alt.on("CEF:Adminmenu:ReceiveMeta", (metaarray) => {
        receiveMeta(metaarray);
    });