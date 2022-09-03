import * as alt from 'alt-client';
import * as game from 'natives';
import * as NativeUI from './includes/NativeUI/NativeUI';
const MenuSettings = {
    TitleFont: NativeUI.Font.Monospace,
}
const menu = new NativeUI.Menu("Animationen", "", new NativeUI.Point(10, 10));
menu.GetTitle().Scale = 0.9;
menu.GetTitle().Font = MenuSettings.TitleFont;
menu.AddItem(new NativeUI.UIMenuItem("~r~Abbrechen", ""));
menu.ItemSelect.on((item) => {
    if (item.Text == '~r~Abbrechen') {
        game.clearPedTasks(alt.Player.local.scriptID);
    }
});
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
menu.SetRectangleBannerType(banner);
//SITZEN
let menuItem = new NativeUI.UIMenuItem("Sitzen", "");
menu.AddItem(menuItem);
const subMenu = new NativeUI.Menu("Sitzen", "", new NativeUI.Point(10, 10));
subMenu.Visible = false;
subMenu.GetTitle().Scale = 0.9;
subMenu.GetTitle().Font = MenuSettings.TitleFont;
menu.AddSubMenu(subMenu, menuItem);
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
subMenu.SetRectangleBannerType(banner);
//ADD ITEM
subMenu.AddItem(new NativeUI.UIMenuItem("Sitzen 1", ""));
subMenu.AddItem(new NativeUI.UIMenuItem("Sitzen 2", ""));
subMenu.AddItem(new NativeUI.UIMenuItem("Sitzen 3", ""));
subMenu.AddItem(new NativeUI.UIMenuItem("Sitzen 4", ""));
subMenu.AddItem(new NativeUI.UIMenuItem("Sitzen 5", ""));
subMenu.AddItem(new NativeUI.UIMenuItem("Sitzen 6", ""));
//ON SELECT
subMenu.ItemSelect.on((item) => {
    if (item.Text == 'Sitzen 1') {
        playAnimation("anim@heists@fleeca_bank@ig_7_jetski_owner", "owner_idle", 1, -1);
    } else if (item.Text == 'Sitzen 2') {
        playAnimation("amb@lo_res_idles@", "world_human_picnic_female_lo_res_base", 1, -1);
    } else if (item.Text == 'Sitzen 3') {
        playAnimation("switch@michael@sitting", "idle", 1, -1);
    } else if (item.Text == 'Sitzen 4') {
        playAnimation("missfam2leadinoutmcs3", "onboat_leadin_pornguy_a", 1, -1);
    } else if (item.Text == 'Sitzen 5') {
        playAnimation("timetable@reunited@ig_10", "base_amanda", 1, -1);
    } else if (item.Text == 'Sitzen 6') {
        playAnimation("switch@trevor@floyd_crying", "console_end_loop_floyd", 1, -1);
    }
});
//LIEGEN
let menuItem2 = new NativeUI.UIMenuItem("Liegen", "");
menu.AddItem(menuItem2);
const subMenu2 = new NativeUI.Menu("Liegen", "", new NativeUI.Point(10, 10));
subMenu2.Visible = false;
subMenu2.GetTitle().Scale = 0.9;
subMenu2.GetTitle().Font = MenuSettings.TitleFont;
menu.AddSubMenu(subMenu2, menuItem2);
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
subMenu2.SetRectangleBannerType(banner);
//ADD ITEM
subMenu2.AddItem(new NativeUI.UIMenuItem("Liegen 1", ""));
subMenu2.AddItem(new NativeUI.UIMenuItem("Liegen 2", ""));
subMenu2.AddItem(new NativeUI.UIMenuItem("Liegen 3", ""));
subMenu2.AddItem(new NativeUI.UIMenuItem("Liegen 4", ""));
subMenu2.AddItem(new NativeUI.UIMenuItem("Liegen 5", ""));
subMenu2.AddItem(new NativeUI.UIMenuItem("Liegen 6", ""));
subMenu2.AddItem(new NativeUI.UIMenuItem("Liegen 7", ""));
//ON SELECT
subMenu2.ItemSelect.on((item) => {
    if (item.Text == 'Liegen 1') {
        playAnimation("amb@world_human_sunbathe@male@back@base", "base", 1, -1);
    } else if (item.Text == 'Liegen 2') {
        playAnimation("amb@world_human_sunbathe@male@front@base", "base", 1, -1);
    } else if (item.Text == 'Liegen 3') {
        playAnimation("amb@world_human_sunbathe@male@front@idle_a", "idle_a", 1, -1);
    } else if (item.Text == 'Liegen 4') {
        playAnimation("amb@lo_res_idles@", "world_human_bum_slumped_right_lo_res_base", 1, -1);
    } else if (item.Text == 'Liegen 5') {
        playAnimation("timetable@amanda@drunk@idle_a", "idle_pinot", 1, -1);
    } else if (item.Text == 'Liegen 6') {
        playAnimation("misssolomon_5@end", "dead_black_ops", 1, -1);
    } else if (item.Text == 'Liegen 7') {
        playAnimation("missfinale_c1@", "lying_dead_player0", 1, -1);
    }
});
//STEHEN
let menuItem3 = new NativeUI.UIMenuItem("Stehen", "");
menu.AddItem(menuItem3);
const subMenu3 = new NativeUI.Menu("Stehen", "", new NativeUI.Point(10, 10));
subMenu3.Visible = false;
subMenu3.GetTitle().Scale = 0.9;
subMenu3.GetTitle().Font = MenuSettings.TitleFont;
menu.AddSubMenu(subMenu3, menuItem3);
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
subMenu3.SetRectangleBannerType(banner);
//ADD ITEM
subMenu3.AddItem(new NativeUI.UIMenuItem("Arme Verschränken", ""));
subMenu3.AddItem(new NativeUI.UIMenuItem("Security", ""));
subMenu3.AddItem(new NativeUI.UIMenuItem("Anlehnen", ""));
subMenu3.AddItem(new NativeUI.UIMenuItem("Anlehnen2", ""));
subMenu3.AddItem(new NativeUI.UIMenuItem("Arrogant", ""));
subMenu3.AddItem(new NativeUI.UIMenuItem("Eingebildet", ""));
subMenu3.AddItem(new NativeUI.UIMenuItem("Depressiv", ""));
subMenu3.AddItem(new NativeUI.UIMenuItem("Salutieren", ""));
subMenu3.AddItem(new NativeUI.UIMenuItem("Nachdenken", ""));
//ON SELECT
subMenu3.ItemSelect.on((item) => {
    if (item.Text == 'Arme Verschränken') {
        playAnimation("anim@heists@heist_corona@single_team", "single_team_loop_boss", 50, -1);
    } else if (item.Text == 'Security') {
        playAnimation("mini@strip_club@idles@bouncer@idle_a", "idle_a", 50, -1);
    } else if (item.Text == 'Anlehnen') {
        playAnimation("amb@world_human_leaning@male@wall@back@foot_up@idle_a", "idle_a", 1, -1);
    } else if (item.Text == 'Anlehnen2') {
        playAnimation("amb@world_human_leaning@male@wall@back@legs_crossed@idle_a", "idle_c", 1, -1);
    } else if (item.Text == 'Arrogant') {
        playAnimation("missmic_3_ext@leadin@mic_3_ext", "_leadin_trevor", 50, -1);
    } else if (item.Text == 'Eingebildet') {
        playAnimation("mp_move@prostitute@m@hooker", "idle", 50, -1);
    } else if (item.Text == 'Depressiv') {
        playAnimation("amb@world_human_bum_standing@depressed@idle_a", "idle_c", 50, -1);
    } else if (item.Text == 'Salutieren') {
        playAnimation("anim@mp_player_intuppersalute", "idle_a", 50, -1);
    } else if (item.Text == 'Nachdenken') {
        playAnimation("amb@world_human_leaning@female@wall@back@hand_up@idle_a", "idle_a", 50, -1);
    }
});
//Knien
let menuItem4 = new NativeUI.UIMenuItem("Knien", "");
menu.AddItem(menuItem4);
const subMenu4 = new NativeUI.Menu("Knien", "", new NativeUI.Point(10, 10));
subMenu4.Visible = false;
subMenu4.GetTitle().Scale = 0.9;
subMenu4.GetTitle().Font = MenuSettings.TitleFont;
menu.AddSubMenu(subMenu4, menuItem4);
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
subMenu4.SetRectangleBannerType(banner);
//ADD ITEM
subMenu4.AddItem(new NativeUI.UIMenuItem("Knien", ""));
subMenu4.AddItem(new NativeUI.UIMenuItem("Auf die Knie", ""));
subMenu4.AddItem(new NativeUI.UIMenuItem("Auf die Knie 2", ""));
subMenu4.AddItem(new NativeUI.UIMenuItem("Verzweifelt Knien", ""));
//ON SELECT
subMenu4.ItemSelect.on((item) => {
    if (item.Text == 'Knien') {
        playAnimation("amb@medic@standing@kneel@base", "base", 1, 300000);
    } else if (item.Text == 'Auf die Knie') {
        playAnimation("missheist_jewel", "manageress_kneel_loop", 1, -1);
    } else if (item.Text == 'Auf die Knie 2') {
        playAnimation("random@arrests", "kneeling_arrest_idle", 1, -1);
    } else if (item.Text == 'Anlehnen2') {
        playAnimation("amb@world_human_leaning@male@wall@back@legs_crossed@idle_a", "kneeling_arrest_idle", 1, -1);
    } else if (item.Text == 'Verzweifelt Knien') {
        playAnimation("missfra2", "lamar_base_idle", 1, -1);
    }
});
//GangSigns
let menuItem5 = new NativeUI.UIMenuItem("Gangsigns", "");
menu.AddItem(menuItem5);
const subMenu5 = new NativeUI.Menu("Gangsigns", "", new NativeUI.Point(10, 10));
subMenu5.Visible = false;
subMenu5.GetTitle().Scale = 0.9;
subMenu5.GetTitle().Font = MenuSettings.TitleFont;
menu.AddSubMenu(subMenu5, menuItem5);
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
subMenu5.SetRectangleBannerType(banner);
//ADD ITEM
subMenu5.AddItem(new NativeUI.UIMenuItem("Gangsign 1", ""));
subMenu5.AddItem(new NativeUI.UIMenuItem("Gangsign 2", ""));
//ON SELECT
subMenu5.ItemSelect.on((item) => {
    if (item.Text == 'Gangsign 1') {
        playAnimation("mp_player_int_uppergang_sign_b", "mp_player_int_gang_sign_b_exit", 50, -1);
    } else if (item.Text == 'Gangsign 2') {
        playAnimation("mp_player_int_uppergang_sign_a", "mp_player_int_gang_sign_a", 50, -1);
    }
});
//Tänze
let menuItem6 = new NativeUI.UIMenuItem("Tänze", "");
menu.AddItem(menuItem6);
const subMenu6 = new NativeUI.Menu("Tanze", "", new NativeUI.Point(10, 10));
subMenu6.Visible = false;
subMenu6.GetTitle().Scale = 0.9;
subMenu6.GetTitle().Font = MenuSettings.TitleFont;
menu.AddSubMenu(subMenu6, menuItem6);
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
subMenu6.SetRectangleBannerType(banner);
//ADD ITEM
subMenu6.AddItem(new NativeUI.UIMenuItem("Strip 1", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Strip 2", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Strip 3", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Strip 4", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Strip 5", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Strip 6", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Ghetto", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Tao 1", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Tao 2", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Stepptanz 1", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Po wackeln", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Po wackeln 2", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Zumba 1", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Zumba 2", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Zumba 3", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Zumba 4", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Zumba 5", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Geiles lied", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Feminines Tanzen", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Tanzfaul", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Luftgitarre", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Banging Tunes", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Onkel Disco", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Der Fisch", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Herzrasen", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Snap", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Raise", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Salsa", ""));
subMenu6.AddItem(new NativeUI.UIMenuItem("Cats Cradle", ""));
//ON SELECT
subMenu6.ItemSelect.on((item) => {
    if (item.Text == 'Strip 1') {
        playAnimation("oddjobs@assassinate@multi@yachttarget@lapdance", "yacht_ld_f", 1, -1);
    } else if (item.Text == 'Strip 2') {
        playAnimation("mini@strip_club@private_dance@idle", "priv_dance_idle", 1, -1);
    } else if (item.Text == 'Strip 3') {
        playAnimation("mini@strip_club@private_dance@part1", "priv_dance_p1", 1, -1);
    } else if (item.Text == 'Strip 4') {
        playAnimation("mini@strip_club@private_dance@part2", "priv_dance_p2", 1, -1);
    } else if (item.Text == 'Strip 5') {
        playAnimation("mini@strip_club@private_dance@part3", "priv_dance_p3", 1, -1);
    } else if (item.Text == 'Strip 6') {
        playAnimation("mp_am_stripper", "lap_dance_girl", 1, -1);
    } else if (item.Text == 'Ghetto') {
        playAnimation("missfbi3_sniping", "dance_m_default", 1, -1);
    } else if (item.Text == 'Tao 1') {
        playAnimation("misschinese2_crystalmazemcs1_cs", "dance_loop_tao", 1, -1);
    } else if (item.Text == 'Tao 2') {
        playAnimation("misschinese2_crystalmazemcs1_ig", "dance_loop_tao", 1, -1);
    } else if (item.Text == 'Stepptanz 1') {
        playAnimation("special_ped@mountain_dancer@base", "base", 1, -1);
    } else if (item.Text == 'Po wackeln') {
        playAnimation("switch@trevor@mocks_lapdance", "001443_01_trvs_28_idle_stripper", 1, -1);
    } else if (item.Text == 'Po wackeln 2') {
        playAnimation("switch@trevor@mocks_lapdance", "001443_01_trvs_28_exit_stripper", 1, -1);
    } else if (item.Text == 'Zumba 1') {
        playAnimation("timetable@tracy@ig_5@idle_a", "idle_a", 1, -1);
    } else if (item.Text == 'Zumba 2') {
        playAnimation("timetable@tracy@ig_5@idle_a", "idle_b", 1, -1);
    } else if (item.Text == 'Zumba 3') {
        playAnimation("timetable@tracy@ig_5@idle_a", "idle_c", 1, -1);
    } else if (item.Text == 'Zumba 4') {
        playAnimation("timetable@tracy@ig_5@idle_b", "idle_d", 1, -1);
    } else if (item.Text == 'Zumba 5') {
        playAnimation("timetable@tracy@ig_5@idle_b", "idle_e", 1, -1);
    } else if (item.Text == 'Geiles lied') {
        playAnimation("amb@world_human_cheering@female_b", "base", 1, -1);
    } else if (item.Text == 'Feminines Tanzen') {
        playAnimation("amb@world_human_jog_standing@female@idle_a", "idle_a", 1, -1);
    } else if (item.Text == 'Tanzfaul') {
        playAnimation("amb@world_human_partying@female@partying_beer@base", "base", 1, -1);
    } else if (item.Text == 'Luftgitarre') {
        playAnimation("anim@mp_player_intcelebrationfemale@air_guitar", "air_guitar", 1, -1);
    } else if (item.Text == 'Banging Tunes') {
        playAnimation("anim@mp_player_intcelebrationmale@banging_tunes", "banging_tunes", 1, -1);
    } else if (item.Text == 'Onkel Disco') {
        playAnimation("anim@mp_player_intcelebrationmale@uncle_disco", "uncle_disco", 1, -1);
    } else if (item.Text == 'Der Fisch') {
        playAnimation("anim@mp_player_intcelebrationmale@find_the_fish", "find_the_fish", 1, -1);
    } else if (item.Text == 'Herzrasen') {
        playAnimation("anim@mp_player_intcelebrationmale@heart_pumping", "heart_pumping", 1, -1);
    } else if (item.Text == 'Snap') {
        playAnimation("anim@mp_player_intcelebrationmale@oh_snap", "oh_snap", 1, -1);
    } else if (item.Text == 'Raise') {
        playAnimation("anim@mp_player_intcelebrationmale@raise_the_roof", "raise_the_roof", 1, -1);
    } else if (item.Text == 'Salsa') {
        playAnimation("anim@mp_player_intcelebrationmale@salsa_roll", "salsa_roll", 1, -1);
    } else if (item.Text == 'Cats Cradle') {
        playAnimation("anim@mp_player_intcelebrationmale@cats_cradle", "cats_cradle", 1, -1);
    }
});
//Sport
let menuItem7 = new NativeUI.UIMenuItem("Sport", "");
menu.AddItem(menuItem7);
const subMenu7 = new NativeUI.Menu("Sport", "", new NativeUI.Point(10, 10));
subMenu7.Visible = false;
subMenu7.GetTitle().Scale = 0.9;
subMenu7.GetTitle().Font = MenuSettings.TitleFont;
menu.AddSubMenu(subMenu7, menuItem7);
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
subMenu7.SetRectangleBannerType(banner);
//ADD ITEM
subMenu7.AddItem(new NativeUI.UIMenuItem("Yoga 1", ""));
subMenu7.AddItem(new NativeUI.UIMenuItem("Yoga 2", ""));
subMenu7.AddItem(new NativeUI.UIMenuItem("Yoga 3", ""));
subMenu7.AddItem(new NativeUI.UIMenuItem("Yoga 4", ""));
subMenu7.AddItem(new NativeUI.UIMenuItem("Yoga 5", ""));
subMenu7.AddItem(new NativeUI.UIMenuItem("Liegestütze 1", ""));
subMenu7.AddItem(new NativeUI.UIMenuItem("Liegestütze 2", ""));
subMenu7.AddItem(new NativeUI.UIMenuItem("Situps", ""));
subMenu7.AddItem(new NativeUI.UIMenuItem("Flex", ""));
subMenu7.AddItem(new NativeUI.UIMenuItem("Flex 2", ""));
subMenu7.AddItem(new NativeUI.UIMenuItem("Klimmzüge", ""));
//ON SELECT
subMenu7.ItemSelect.on((item) => {
    if (item.Text == 'Yoga 1') {
        playAnimation("rcmepsilonism3", "ep_3_rcm_marnie_meditating", 1, -1);
    } else if (item.Text == 'Yoga 2') {
        playAnimation("rcmepsilonism3", "base_loop", 1, -1);
    } else if (item.Text == 'Yoga 3') {
        playAnimation("rcmfanatic1maryann_stretchidle_b", "idle_e", 1, -1);
    } else if (item.Text == 'Yoga 4') {
        playAnimation("timetable@amanda@ig_4", "ig_4_idle", 1, -1);
    } else if (item.Text == 'Yoga 5') {
        playAnimation("amb@world_human_yoga@female@base", "base_c", 1, -1);
    } else if (item.Text == 'Liegestütze 1') {
        playAnimation("rcmfanatic3", "ef_3_rcm_loop_maryann", 1, -1);
    } else if (item.Text == 'Liegestütze 2') {
        playAnimation("amb@world_human_push_ups@male@base", "base", 1, -1);
    } else if (item.Text == 'Situps') {
        playAnimation("amb@world_human_sit_ups@male@base", "base", 1, -1);
    } else if (item.Text == 'Flex') {
        playAnimation("amb@world_human_muscle_flex@arms_at_side@base", "base", 1, -1);
    } else if (item.Text == 'Flex 2') {
        playAnimation("amb@world_human_muscle_flex@arms_in_front@idle_a", "idle_b", 1, -1);
    } else if (item.Text == 'Klimmzüge') {
        playAnimation("amb@prop_human_muscle_chin_ups@male@base", "base", 1, -1);
    }
});
//Job
let menuItem8 = new NativeUI.UIMenuItem("Job", "");
menu.AddItem(menuItem8);
const subMenu8 = new NativeUI.Menu("Job", "", new NativeUI.Point(10, 10));
subMenu8.Visible = false;
subMenu8.GetTitle().Scale = 0.9;
subMenu8.GetTitle().Font = MenuSettings.TitleFont;
menu.AddSubMenu(subMenu8, menuItem8);
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
subMenu8.SetRectangleBannerType(banner);
//ADD ITEM
subMenu8.AddItem(new NativeUI.UIMenuItem("Begutachten", ""));
subMenu8.AddItem(new NativeUI.UIMenuItem("Reparieren", ""));
subMenu8.AddItem(new NativeUI.UIMenuItem("Putzen", ""));
//ON SELECT
subMenu8.ItemSelect.on((item) => {
    if (item.Text == 'Begutachten') {
        playAnimation("oddjobs@taxi@gyn@", "idle_b_ped", 1, 300000);
    } else if (item.Text == 'Reparieren') {
        playAnimation("amb@world_human_vehicle_mechanic@male@base", "base", 1, 300000);
    } else if (item.Text == 'Putzen') {
        playAnimation("timetable@maid@cleaning_window@base", "base", 1, 300000);
    }
});
//Sonstige
let menuItem9 = new NativeUI.UIMenuItem("Sonstige", "");
menu.AddItem(menuItem9);
const subMenu9 = new NativeUI.Menu("Sonstige", "", new NativeUI.Point(10, 10));
subMenu9.Visible = false;
subMenu9.GetTitle().Scale = 0.9;
subMenu9.GetTitle().Font = MenuSettings.TitleFont;
menu.AddSubMenu(subMenu9, menuItem9);
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
subMenu9.SetRectangleBannerType(banner);
//ADD ITEM

subMenu9.AddItem(new NativeUI.UIMenuItem("Peace", ""));
subMenu9.AddItem(new NativeUI.UIMenuItem("2 Bier", ""));
subMenu9.AddItem(new NativeUI.UIMenuItem("Fuck you", ""));
subMenu9.AddItem(new NativeUI.UIMenuItem("Daumen hoch", ""));
subMenu9.AddItem(new NativeUI.UIMenuItem("Winken", ""));
subMenu9.AddItem(new NativeUI.UIMenuItem("Facepalm", ""));
subMenu9.AddItem(new NativeUI.UIMenuItem("Pinkeln", ""));
subMenu9.AddItem(new NativeUI.UIMenuItem("Notieren", ""));
//ON SELECT
subMenu9.ItemSelect.on((item) => {
    if (item.Text == 'Peace') {
        playAnimation("anim@mp_player_intincarpeacestd@ds@", "idle_a", 50, -1);
    } else if (item.Text == '2 Bier') {
        playAnimation("amb@code_human_in_car_mp_actions@v_sign@bodhi@rps@base", "idle_a", 50, -1);
    } else if (item.Text == 'Fuck you') {
        playAnimation("anim@mp_player_intselfiethe_bird", "idle_a", 50, -1);
    } else if (item.Text == 'Daumen hoch') {
        playAnimation("anim@mp_player_intupperthumbs_up", "idle_a", 50, -1);
    } else if (item.Text == 'Winken') {
        playAnimation("anim@mp_player_intupperwave", "idle_a", 50, -1);
    } else if (item.Text == 'Facepalm') {
        playAnimation("anim@mp_player_intupperface_palm", "idle_a", 50, -1);
    } else if (item.Text == 'Pinkeln') {
        playAnimation("missbigscore1switch_trevor_piss", "piss_loop", 50, -1);
    } else if (item.Text == 'Notieren') {
        playAnimation("amb@world_human_clipboard@male@idle_a", "idle_c", 50, -1);
    }
});
//Gehstile
let menuItem10 = new NativeUI.UIMenuItem("Gehstile", "");
menu.AddItem(menuItem10);
const subMenu10 = new NativeUI.Menu("Gehstile", "", new NativeUI.Point(10, 10));
subMenu10.Visible = false;
subMenu10.GetTitle().Scale = 0.9;
subMenu10.GetTitle().Font = MenuSettings.TitleFont;
menu.AddSubMenu(subMenu10, menuItem10);
var banner = new NativeUI.ResRectangle(new NativeUI.Point(0, 0), new NativeUI.Size(0, 0), new NativeUI.Color(0, 0, 0, 220));
subMenu10.SetRectangleBannerType(banner);
//ADD ITEM
subMenu10.AddItem(new NativeUI.UIMenuItem("Normal", ""));
subMenu10.AddItem(new NativeUI.UIMenuItem("Verletzt", ""));
subMenu10.AddItem(new NativeUI.UIMenuItem("Arrogant", ""));
subMenu10.AddItem(new NativeUI.UIMenuItem("Casual", ""));
subMenu10.AddItem(new NativeUI.UIMenuItem("Casual 2", ""));
subMenu10.AddItem(new NativeUI.UIMenuItem("Zuversichtlich", ""));
subMenu10.AddItem(new NativeUI.UIMenuItem("Betrunken", ""));
subMenu10.AddItem(new NativeUI.UIMenuItem("Gangster 1", ""));
subMenu10.AddItem(new NativeUI.UIMenuItem("Gangster 2", ""));
subMenu10.AddItem(new NativeUI.UIMenuItem("Cop 1", ""));
subMenu10.AddItem(new NativeUI.UIMenuItem("Cop 2", ""));
//ON SELECT
subMenu10.ItemSelect.on((item) => {
    if (item.Text == 'Verletzt') {
        playWalking("move_m@injured");
    } else if (item.Text == 'Arrogant') {
        playWalking("move_f@arrogant@a");
    } else if (item.Text == 'Casual') {
        playWalking("move_m@casual@a");
    } else if (item.Text == 'Casual 2') {
        playWalking("move_m@casual@d");
    } else if (item.Text == 'Zuversichtlich') {
        playWalking("move_m@confident");
    } else if (item.Text == 'Betrunken') {
        playWalking("move_m@drunk@a");
    } else if (item.Text == 'Gangster 1') {
        playWalking("move_m@gangster@generic");
    } else if (item.Text == 'Gangster 2') {
        playWalking("move_m@gangster@ng");
    } else if (item.Text == 'Cop 1') {
        playWalking("move_m@business@a");
    } else if (item.Text == 'Cop 2') {
        playWalking("move_m@business@b");
    } else if (item.Text == 'Normal') {
        game.resetPedMovementClipset(alt.Player.local.scriptID);
    }
});

alt.on('keyup', (key) => {
    if (alt.Player.local.getSyncedMeta("HasHandcuffs") == true || alt.Player.local.getSyncedMeta("HasRopeCuffs") == true || alt.Player.local.getSyncedMeta("IsUnconscious") == true || alt.Player.local.getSyncedMeta("IsCefOpen") == true) return;
    if (key === 113) {
        if (menu.Visible)
            menu.Close();
        else
            menu.Open();
    }
});

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

function playWalking(anim) {
    if (anim == undefined) return;
    if (anim == "normal") {
        game.resetPedMovementClipset(alt.Player.local.scriptID);
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

alt.onServer('Client:Animation:PlayAnimation', (animDict, animName, duration, flag, lockpos) => {
    game.requestAnimDict(animDict);
    let interval = alt.setInterval(() => {
        if (game.hasAnimDictLoaded(animDict)) {
            alt.clearInterval(interval);
            game.taskPlayAnim(game.playerPedId(), animDict, animName, 8.0, 1, duration, flag, 1, lockpos, lockpos, lockpos);
        }
    }, 0);
});

alt.on('keyup', (key) => {
    if (key == 96) { //Numpad0 
        game.clearPedTasks(alt.Player.local.scriptID);
    }
});