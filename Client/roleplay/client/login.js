import * as alt from 'alt';
import * as game from 'natives';

let loginBrowser = null;
let loginCam = null;
let loginPedHandle = null;
let loginModelHash = null;

alt.onServer('Client:Login:CreateCEF', () => {
    if (loginBrowser == null) {
        loginCam = game.createCamWithParams('DEFAULT_SCRIPTED_CAMERA', -1398, -555, 36, 0, 0, -173, 50, true, 2);
        game.setCamActive(loginCam, true);
        game.renderScriptCams(true, false, 0, true, false, 0);
        game.freezeEntityPosition(alt.Player.local.scriptID, true);
        alt.showCursor(true);
        alt.toggleGameControls(false);
        loginBrowser = new alt.WebView("http://resource/client/cef/login/index.html");
        loginBrowser.focus();
        loginBrowser.on("Client:Login:cefIsReady", () => {
            alt.setTimeout(() => {
                if (alt.LocalStorage.get("username")) {
                    loginBrowser.emit("CEF:Login:setStorage", alt.LocalStorage.get("username"), alt.LocalStorage.get("password"));
                }
                loginBrowser.emit("CEF:Login:showArea", "login");
            }, 2000);
        });

        loginBrowser.on("Client:Login:sendLoginDataToServer", (name, password) => {
            alt.emitServer("Server:Login:ValidateLoginCredentials", name, password);
        });

        loginBrowser.on("Client:Register:sendRegisterDataToServer", (name, email, password, passwordrepeat) => {
            alt.emitServer("Server:Register:RegisterNewPlayer", name, email, password, passwordrepeat);
        });

        loginBrowser.on("Client:Charcreator:OpenCreator", () => {
            alt.emitServer("Server:Charcreator:CreateCEF");
            destroyLoginBrowser();
        });

        loginBrowser.on("Client:Login:DestroyCEF", () => {
            destroyLoginBrowser();
        });

        loginBrowser.on("Client:Charselector:KillCharacter", (charid) => {
            alt.emitServer("Server:Charselector:KillCharacter", charid);
        });

        loginBrowser.on("Client:Charselector:spawnChar", (charid, spawnstr) => {
            alt.emitServer("Server:Charselector:spawnChar", spawnstr, charid);
        });
    }
});

alt.onServer("Client:SpawnArea:setCharSkin", (facefeaturearray, headblendsarray, headoverlayarray) => {
    let facefeatures = JSON.parse(facefeaturearray);
    let headblends = JSON.parse(headblendsarray);
    let headoverlays = JSON.parse(headoverlayarray);

    game.setPedHeadBlendData(alt.Player.local.scriptID, parseInt(headblends[0]), parseInt(headblends[1]), 0, parseInt(headblends[2]), parseInt(headblends[5]), 0, parseFloat(headblends[3]), parseInt(headblends[4]), 0, false);
    game.setPedHeadOverlayColor(alt.Player.local.scriptID, 1, 1, parseInt(headoverlays[2][1]), 1);
    game.setPedHeadOverlayColor(alt.Player.local.scriptID, 2, 1, parseInt(headoverlays[2][2]), 1);
    game.setPedHeadOverlayColor(alt.Player.local.scriptID, 5, 2, parseInt(headoverlays[2][5]), 1);
    game.setPedHeadOverlayColor(alt.Player.local.scriptID, 8, 2, parseInt(headoverlays[2][8]), 1);
    game.setPedHeadOverlayColor(alt.Player.local.scriptID, 10, 1, parseInt(headoverlays[2][10]), 1);
    game.setPedEyeColor(alt.Player.local.scriptID, parseInt(headoverlays[0][14]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 0, parseInt(headoverlays[0][0]), parseInt(headoverlays[1][0]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 1, parseInt(headoverlays[0][1]), parseFloat(headoverlays[1][1]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 2, parseInt(headoverlays[0][2]), parseFloat(headoverlays[1][2]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 3, parseInt(headoverlays[0][3]), parseInt(headoverlays[1][3]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 4, parseInt(headoverlays[0][4]), parseInt(headoverlays[1][4]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 5, parseInt(headoverlays[0][5]), parseInt(headoverlays[1][5]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 6, parseInt(headoverlays[0][6]), parseInt(headoverlays[1][6]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 7, parseInt(headoverlays[0][7]), parseInt(headoverlays[1][7]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 8, parseInt(headoverlays[0][8]), parseInt(headoverlays[1][8]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 9, parseInt(headoverlays[0][9]), parseInt(headoverlays[1][9]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 10, parseInt(headoverlays[0][10]), parseInt(headoverlays[1][10]));
    game.setPedComponentVariation(alt.Player.local.scriptID, 2, parseInt(headoverlays[0][13]), 0, 0);
    game.setPedHairColor(alt.Player.local.scriptID, parseInt(headoverlays[2][13]), parseInt(headoverlays[1][13]));

    for (let i = 0; i < 20; i++) {
        game.setPedFaceFeature(alt.Player.local.scriptID, i, parseFloat(facefeatures[i]));
    }
});

alt.onServer("Client:SpawnArea:setCharClothes", (componentId, drawableId, textureId) => {
    //alt.log(`Component: ${componentId} - drawable: ${drawableId} - texture: ${textureId}`);
    game.setPedComponentVariation(alt.Player.local.scriptID, parseInt(componentId), parseInt(drawableId), parseInt(textureId), 0);
});

alt.onServer("Client:SpawnArea:setCharAccessory", (componentId, drawableId, textureId) => {
    game.setPedPropIndex(alt.Player.local.scriptID, componentId, drawableId, textureId, false);
});

alt.onServer("Client:SpawnArea:clearCharAccessory", (componentId) => {
    game.clearPedProp(alt.Player.local.scriptID, componentId);
});

alt.onServer("Client:Charselector:ViewCharacter", (gender, facefeaturearray, headblendsarray, headoverlayarray) => {
    spawnCharSelectorPed(gender, facefeaturearray, headblendsarray, headoverlayarray);
});

alt.onServer("Client:Login:SaveLoginCredentialsToStorage", (name, password) => {
    alt.LocalStorage.set('username', name);
    alt.LocalStorage.set('password', password);
    alt.LocalStorage.save();
});

alt.onServer("Client:Login:showError", (msg) => {
    if (loginBrowser != null) {
        loginBrowser.emit("CEF:Login:showError", msg);
    }
});

alt.onServer("Client:Login:showArea", (area) => {
    if (loginBrowser != null) {
        loginBrowser.emit("CEF:Login:showArea", area);
        if (area == "charselect") {
            if (loginCam != null) {
                game.renderScriptCams(false, false, 0, true, false, 0);
                game.setCamActive(loginCam, false);
                game.destroyCam(loginCam, true);
                loginCam = null;
            }
            game.setEntityAlpha(alt.Player.local.scriptID, 0, 0);
            loginCam = game.createCamWithParams('DEFAULT_SCRIPTED_CAMERA', 402.7, -1003, -98.6, 0, 0, 358, 18, true, 2);
            game.setCamActive(loginCam, true);
            game.renderScriptCams(true, false, 0, true, false, 0);
        }
    }
});

alt.onServer("Client:Charselector:sendCharactersToCEF", (chars) => {
    if (loginBrowser != null) {
        loginBrowser.emit("CEF:Charselector:sendCharactersToCEF", chars)
    }
});

let destroyLoginBrowser = function() {
    if (loginBrowser != null) {
        loginBrowser.destroy();
    }
    loginBrowser = null;
    game.renderScriptCams(false, false, 0, true, false, 0);
    game.setCamActive(loginCam, false);
    if (loginCam != null) {
        game.destroyCam(loginCam, true);
    }
    if (loginPedHandle != null) {
        game.deletePed(loginPedHandle);
        loginPedHandle = null;
    }
    loginCam = null;
    alt.showCursor(false);
    alt.toggleGameControls(true);
    game.freezeEntityPosition(alt.Player.local.scriptID, false);
    game.setEntityAlpha(alt.Player.local.scriptID, 255, 0);
}

function spawnCharSelectorPed(gender, facefeaturearray, headblendsarray, headoverlayarray) {
    let facefeatures = JSON.parse(facefeaturearray);
    let headblends = JSON.parse(headblendsarray);
    let headoverlays = JSON.parse(headoverlayarray);

    if (loginPedHandle != null) {
        game.deletePed(loginPedHandle);
        loginPedHandle = null;
    }

    if (gender == true) {
        loginModelHash = game.getHashKey('mp_f_freemode_01');
        game.requestModel(loginModelHash);
    } else if (gender == false) {
        loginModelHash = game.getHashKey('mp_m_freemode_01');
        game.requestModel(loginModelHash);
    }
    alt.setTimeout(function() {
        if (game.hasModelLoaded(loginModelHash)) {
            loginPedHandle = game.createPed(4, loginModelHash, 402.778, -996.9758, -100.01465, 0, false, true);
            game.setEntityAlpha(loginPedHandle, 255, 0);
            game.setEntityHeading(loginPedHandle, 180.0);
            game.setEntityInvincible(loginPedHandle, true);
            game.disablePedPainAudio(loginPedHandle, true);
            game.freezeEntityPosition(loginPedHandle, true);
            game.taskSetBlockingOfNonTemporaryEvents(loginPedHandle, true);

            game.setPedHeadBlendData(loginPedHandle, headblends[0], headblends[1], 0, headblends[2], headblends[5], 0, headblends[3], headblends[4], 0, 0);
            game.setPedHeadOverlayColor(loginPedHandle, 1, 1, parseInt(headoverlays[2][1]), 1);
            game.setPedHeadOverlayColor(loginPedHandle, 2, 1, parseInt(headoverlays[2][2]), 1);
            game.setPedHeadOverlayColor(loginPedHandle, 5, 2, parseInt(headoverlays[2][5]), 1);
            game.setPedHeadOverlayColor(loginPedHandle, 8, 2, parseInt(headoverlays[2][8]), 1);
            game.setPedHeadOverlayColor(loginPedHandle, 10, 1, parseInt(headoverlays[2][10]), 1);
            game.setPedEyeColor(loginPedHandle, parseInt(headoverlays[0][14]));
            game.setPedHeadOverlay(loginPedHandle, 0, parseInt(headoverlays[0][0]), parseInt(headoverlays[1][0]));
            game.setPedHeadOverlay(loginPedHandle, 1, parseInt(headoverlays[0][1]), parseFloat(headoverlays[1][1]));
            game.setPedHeadOverlay(loginPedHandle, 2, parseInt(headoverlays[0][2]), parseFloat(headoverlays[1][2]));
            game.setPedHeadOverlay(loginPedHandle, 3, parseInt(headoverlays[0][3]), parseInt(headoverlays[1][3]));
            game.setPedHeadOverlay(loginPedHandle, 4, parseInt(headoverlays[0][4]), parseInt(headoverlays[1][4]));
            game.setPedHeadOverlay(loginPedHandle, 5, parseInt(headoverlays[0][5]), parseInt(headoverlays[1][5]));
            game.setPedHeadOverlay(loginPedHandle, 6, parseInt(headoverlays[0][6]), parseInt(headoverlays[1][6]));
            game.setPedHeadOverlay(loginPedHandle, 7, parseInt(headoverlays[0][7]), parseInt(headoverlays[1][7]));
            game.setPedHeadOverlay(loginPedHandle, 8, parseInt(headoverlays[0][8]), parseInt(headoverlays[1][8]));
            game.setPedHeadOverlay(loginPedHandle, 9, parseInt(headoverlays[0][9]), parseInt(headoverlays[1][9]));
            game.setPedHeadOverlay(loginPedHandle, 10, parseInt(headoverlays[0][10]), parseInt(headoverlays[1][10]));
            game.setPedComponentVariation(loginPedHandle, 2, parseInt(headoverlays[0][13]), 0, 0);
            game.setPedHairColor(loginPedHandle, parseInt(headoverlays[2][13]), parseInt(headoverlays[1][13]));

            for (let i = 0; i < 20; i++) {
                game.setPedFaceFeature(loginPedHandle, i, parseFloat(facefeatures[i]));
            }
        }
    }, 200);
}

alt.on('connectionComplete', () => {
    loadallIPLsAndInteriors();
    alt.setStat('stamina', 100);
    alt.setStat('strength', 100);
    alt.setStat('lung_capacity', 100);
    alt.setStat('wheelie_ability', 100);
    alt.setStat('flying_ability', 100);
    alt.setStat('shooting_ability', 100);
    alt.setStat('stealth_ability', 100);
});

function loadallIPLsAndInteriors() {
    // CASINO
    alt.requestIpl('vw_casino_penthouse');
    alt.requestIpl('hei_dlc_windows_casino');
    alt.requestIpl('vw_casino_carpark');
    alt.requestIpl('vw_casino_garage');
    alt.requestIpl('vw_casino_main');
    alt.requestIpl('hei_dlc_windows_casino');
    alt.requestIpl('hei_dlc_casino_door');

    // CLOSE OPEN DOORS
    game.doorControl(3687927243, -1149.709, -1521.088, 10.78267, true, 0.0, 50.0, 0.0); // VESPUCCI HOUSE
    game.doorControl(520341586, -14.868921, -1441.1823, 31.193226, true, 0.0, 50.0, 0.0); // FRANKLIN'S OLD HOUSE
    game.doorControl(159994461, -816.716, 179.09796, 72.82738, true, 0.0, 50.0, 0.0); // MICHAELS HOUSE 
    game.doorControl(2608952911, -816.1068, 177.51086, 72.82738, true, 0.0, 50.0, 0.0); // MICHAELS HOUSE 
    game.doorControl(2731327123, -806.28174, 186.02461, 72.62405, true, 0.0, 50.0, 0.0); // MICHAELS HOUSE 
    game.doorControl(2840207166, -793.3943, 180.50746, 73.04045, true, 0.0, 50.0, 0.0); // MICHAELS HOUSE 
    game.doorControl(2840207166, -796.5657, 177.22139, 73.04045, true, 0.0, 50.0, 0.0); // MICHAELS HOUSE 
    game.doorControl(1245831483, -794.1853, 182.56801, 73.04045, true, 0.0, 50.0, 0.0); // MICHAELS HOUSE 
    game.doorControl(1245831483, -794.5051, 178.01237, 73.04045, true, 0.0, 50.0, 0.0); // MICHAELS HOUSE 
    game.doorControl(308207762, 7.518359, 539.5268, 176.17764, true, 0.0, 50.0, 0.0); // FRANKLIN'S NEW HOUSE
    game.doorControl(1145337974, 1273.8154, -1720.6969, 54.92143, true, 0.0, 50.0, 0.0); // LESTER'S HOUSE
    game.doorControl(132154435, 1972.769, 3815.366, 33.663258, true, 0.0, 50.0, 0.0); // TREVOR'S HOUSE

    // REQUEST
    //apartments
    alt.requestIpl('apa_v_mp_h_01_a');
    alt.requestIpl('apa_v_mp_h_01_c');
    alt.requestIpl('apa_v_mp_h_01_b');
    alt.requestIpl('apa_v_mp_h_02_a');
    alt.requestIpl('apa_v_mp_h_02_c');
    alt.requestIpl('apa_v_mp_h_02_b');
    alt.requestIpl('apa_v_mp_h_03_a');
    alt.requestIpl('apa_v_mp_h_03_c');
    alt.requestIpl('apa_v_mp_h_03_b');
    alt.requestIpl('apa_v_mp_h_04_a');
    alt.requestIpl('apa_v_mp_h_04_c');
    alt.requestIpl('apa_v_mp_h_04_b');
    alt.requestIpl('apa_v_mp_h_05_a');
    alt.requestIpl('apa_v_mp_h_05_c');
    alt.requestIpl('apa_v_mp_h_05_b');
    alt.requestIpl('apa_v_mp_h_06_a');
    alt.requestIpl('apa_v_mp_h_06_c');
    alt.requestIpl('apa_v_mp_h_06_b');
    alt.requestIpl('apa_v_mp_h_07_a');
    alt.requestIpl('apa_v_mp_h_07_c');
    alt.requestIpl('apa_v_mp_h_07_b');
    alt.requestIpl('apa_v_mp_h_08_a');
    alt.requestIpl('apa_v_mp_h_08_c');
    alt.requestIpl('apa_v_mp_h_08_b');
    //arcadius
    alt.requestIpl('ex_dt1_02_office_02b');
    alt.requestIpl('ex_dt1_02_office_02c');
    alt.requestIpl('ex_dt1_02_office_02a');
    alt.requestIpl('ex_dt1_02_office_01a');
    alt.requestIpl('ex_dt1_02_office_01b');
    alt.requestIpl('ex_dt1_02_office_01c');
    alt.requestIpl('ex_dt1_02_office_03a');
    alt.requestIpl('ex_dt1_02_office_03b');
    alt.requestIpl('ex_dt1_02_office_03c');
    //lombank
    alt.requestIpl('ex_sm_13_office_02b');
    alt.requestIpl('ex_sm_13_office_02c');
    alt.requestIpl('ex_sm_13_office_02a');
    alt.requestIpl('ex_sm_13_office_01a');
    alt.requestIpl('ex_sm_13_office_01b');
    alt.requestIpl('ex_sm_13_office_01c');
    alt.requestIpl('ex_sm_13_office_03a');
    alt.requestIpl('ex_sm_13_office_03b');
    alt.requestIpl('ex_sm_13_office_03c');
    //mazebank
    alt.requestIpl('ex_dt1_11_office_02b');
    alt.requestIpl('ex_dt1_11_office_02c');
    alt.requestIpl('ex_dt1_11_office_02a');
    alt.requestIpl('ex_dt1_11_office_01a');
    alt.requestIpl('ex_dt1_11_office_01b');
    alt.requestIpl('ex_dt1_11_office_01c');
    alt.requestIpl('ex_dt1_11_office_03a');
    alt.requestIpl('ex_dt1_11_office_03b');
    alt.requestIpl('ex_dt1_11_office_03c');
    //mazebankwest
    alt.requestIpl('ex_sm_15_office_02b');
    alt.requestIpl('ex_sm_15_office_02c');
    alt.requestIpl('ex_sm_15_office_02a');
    alt.requestIpl('ex_sm_15_office_01a');
    alt.requestIpl('ex_sm_15_office_01b');
    alt.requestIpl('ex_sm_15_office_01c');
    alt.requestIpl('ex_sm_15_office_03a');
    alt.requestIpl('ex_sm_15_office_03b');
    alt.requestIpl('ex_sm_15_office_03c');
    //clubs/warehouses
    alt.requestIpl('bkr_biker_interior_placement_interior_0_biker_dlc_int_01_milo');
    alt.requestIpl('bkr_biker_interior_placement_interior_1_biker_dlc_int_02_milo');
    alt.requestIpl('bkr_biker_interior_placement_interior_2_biker_dlc_int_ware01_milo');
    alt.requestIpl('bkr_biker_interior_placement_interior_5_biker_dlc_int_ware04_milo');
    alt.requestIpl('bkr_biker_interior_placement_interior_6_biker_dlc_int_ware05_milo');
    alt.requestIpl('ex_exec_warehouse_placement_interior_1_int_warehouse_s_dlc_milo');
    alt.requestIpl('ex_exec_warehouse_placement_interior_0_int_warehouse_m_dlc_milo');
    alt.requestIpl('ex_exec_warehouse_placement_interior_2_int_warehouse_l_dlc_milo');
    alt.requestIpl('biker_dlc_int_03');
    alt.requestIpl('imp_impexp_interior_placement_interior_1_impexp_intwaremed_milo_');
    //special
    alt.requestIpl('DES_stilthouse_rebuild');
    alt.requestIpl('FINBANK');
    alt.requestIpl('v_factory1');
    alt.requestIpl('v_factory2');
    alt.requestIpl('v_factory3');
    alt.removeIpl('CS1_02_cf_offmission');
    alt.requestIpl('CS1_02_cf_onmission1');
    alt.requestIpl('CS1_02_cf_onmission2');
    alt.requestIpl('CS1_02_cf_onmission3');
    alt.requestIpl('CS1_02_cf_onmission4');
    alt.requestIpl('SP1_10_real_interior');
    alt.requestIpl('TrevorsTrailerTidy');
    alt.requestIpl('post_hiest_unload');
    alt.requestIpl('FIBlobby');
    alt.requestIpl('Carwash_with_spinners');
    alt.requestIpl('bkr_bi_id1_23_door');
    alt.requestIpl('lr_cs6_08_grave_closed');
    alt.requestIpl('methtrailer_grp1');
    alt.requestIpl('bkr_bi_hw1_13_int');
    //Pillbox Hill Hospital
    alt.requestIpl("rc12b_default");
    alt.requestIpl("rc12b_hospitalinterior");
    alt.requestIpl("rc12b_hospitalinterior_lod");
    alt.requestIpl("rc12b_destroyed");

    alt.requestIpl('BH1_47_JoshHse_UnBurnt');
    alt.requestIpl('bh1_47_joshhse_unburnt_lod');
    //PDM
    alt.requestIpl('shr_int');
    //
    alt.requestIpl('ferris_finale_Anim');
    alt.requestIpl('hei_dlc_windows_casino');
    alt.requestIpl('golfflags');
    alt.removeIpl('racetrack01');
    alt.requestIpl('railing_start');
    //FARMHOUSE
    alt.removeIpl('farm_burnt');
    alt.removeIpl('farm_burnt_lod');
    alt.removeIpl('farm_burnt_props');
    alt.removeIpl('farmint_cap');
    alt.removeIpl('farmint_cap_lod');
    alt.requestIpl('farm');
    alt.requestIpl('farmint');
    alt.requestIpl('farm_lod');
    alt.requestIpl('farm_props');
    alt.requestIpl('des_farmhs_startimap');
    alt.requestIpl('cs3_05_water_grp2');
    alt.requestIpl('cs3_05_water_grp1');
    alt.requestIpl('trv1_trail_start');
    alt.requestIpl('canyonrvrshallow');
    alt.requestIpl('canyonrvrdeep');
    alt.requestIpl('canyonriver01');
    //
    alt.requestIpl('id2_14_during_door');
    alt.requestIpl('id2_14_during1');
    alt.requestIpl('facelobby');
    alt.requestIpl('facelobby_lod');
    alt.requestIpl('v_tunnel_hole');
    alt.requestIpl('v_tunnel_hole_lod');
    //ACTIVATE
    game.activateInteriorEntitySet(game.getInteriorAtCoordsWithType(1051.491, -3196.536, -39.14842, 'biker_dlc_int_ware02'), 'all');
    game.activateInteriorEntitySet(game.getInteriorAtCoordsWithType(1093.6, -3196.6, -38.99841, 'biker_dlc_int_ware03'), 'all');
    game.activateInteriorEntitySet(game.getInteriorAtCoordsWithType(982.0083, -100.8747, 74.84512, 'biker_dlc_int_03'), 'all');
    game.activateInteriorEntitySet(game.getInteriorAtCoordsWithType(-38.62, -1099.01, 27.31, 'v_carshowroom'), 'csr_beforeMission');
    //game.activateInteriorEntitySet(game.getInteriorAtCoordsWithType(-38.62, -1099.01, 27.31, 'v_carshowroom'), 'shutter_closed');

    // REMOVE
    alt.removeIpl('CS3_07_MPGates');

    //Security DLC
    alt.requestIpl('sf_int_placement_sec_interior_7_dlc_warehouse_sec_milo_')
    alt.requestIpl('sf_int_placement_sec_interior_0_dlc_office_sec_milo_')
    alt.requestIpl('sf_int_placement_sec_interior_1_dlc_studio_sec_milo_')
    alt.requestIpl('sf_int_placement_sec_interior_2_dlc_garage_sec_milo_')
    alt.requestIpl('sf_plaque_kt1_08')
    alt.requestIpl('sf_plaque_kt1_05')
    alt.requestIpl('sf_plaque_hw1_08')
    alt.requestIpl('sf_plaque_bh1_05')
    alt.requestIpl('sf_phones')
    alt.requestIpl('sf_musicrooftop')
    alt.requestIpl('sf_mansionroof')
    alt.requestIpl('sf_franklin')
    alt.requestIpl('sf_billboards')
    alt.requestIpl('sf_heli_crater')
    alt.requestIpl('sf_yacht_02_int')
    alt.requestIpl('sf_yacht_02')
    alt.requestIpl('sf_yacht_01_int')
    alt.requestIpl('sf_yacht_01')
    alt.requestIpl('sf_fixeroffice_kt1_08')
    alt.requestIpl('sf_fixeroffice_kt1_05')
    alt.requestIpl('sf_fixeroffice_hw1_08')
    alt.requestIpl('sf_fixeroffice_bh1_05')
    alt.requestIpl('sf_dlc_fixer_hanger_door')

    //COKE-LAB
    let CocaineInteriorID = game.getInteriorAtCoords(1093.6, -3196.6, -38.99841);
    if (game.isValidInterior(CocaineInteriorID)) {
        alt.requestIpl('bkr_biker_interior_placement_interior_4_biker_dlc_int_ware03_milo');
        game.activateInteriorEntitySet(CocaineInteriorID, "set_up");
        game.activateInteriorEntitySet(CocaineInteriorID, "equipment_upgrade");
        game.activateInteriorEntitySet(CocaineInteriorID, "coke_press_upgrade");
        game.activateInteriorEntitySet(CocaineInteriorID, "production_upgrade");
        game.activateInteriorEntitySet(CocaineInteriorID, "table_equipment_upgrade");
        game.activateInteriorEntitySet(CocaineInteriorID, "security_high");
        game.activateInteriorEntitySet(CocaineInteriorID, "coke_cut_04");
        game.activateInteriorEntitySet(CocaineInteriorID, "coke_cut_05");
        game.refreshInterior(CocaineInteriorID);
    }
    //WEED-LAB
    let WeedInteriorID = game.getInteriorAtCoords(1051.491, -3196.536, -39.14842);
    if (game.isValidInterior(WeedInteriorID)) {
        alt.requestIpl('bkr_biker_interior_placement_interior_3_biker_dlc_int_ware02_milo');
        game.activateInteriorEntitySet(WeedInteriorID, "weed_upgrade_equip");
        game.activateInteriorEntitySet(WeedInteriorID, "weed_security_upgrade");
        game.activateInteriorEntitySet(WeedInteriorID, "light_growtha_stage23_upgrade");
        game.activateInteriorEntitySet(WeedInteriorID, "weed_hosea");
        game.activateInteriorEntitySet(WeedInteriorID, "light_growthb_stage23_upgrade");
        game.activateInteriorEntitySet(WeedInteriorID, "weed_hoseb");
        game.activateInteriorEntitySet(WeedInteriorID, "light_growthc_stage23_upgrade");
        game.activateInteriorEntitySet(WeedInteriorID, "weed_hosec");
        game.activateInteriorEntitySet(WeedInteriorID, "light_growthd_stage23_upgrade");
        game.activateInteriorEntitySet(WeedInteriorID, "weed_hosed");
        game.activateInteriorEntitySet(WeedInteriorID, "light_growthe_stage23_upgrade");
        game.activateInteriorEntitySet(WeedInteriorID, "weed_hosee");
        game.activateInteriorEntitySet(WeedInteriorID, "light_growthf_stage23_upgrade");
        game.activateInteriorEntitySet(WeedInteriorID, "weed_hosef");
        game.activateInteriorEntitySet(WeedInteriorID, "light_growthg_stage23_upgrade");
        game.activateInteriorEntitySet(WeedInteriorID, "weed_hoseg");
        game.activateInteriorEntitySet(WeedInteriorID, "light_growthh_stage23_upgrade");
        game.activateInteriorEntitySet(WeedInteriorID, "weed_hoseh");
        game.activateInteriorEntitySet(WeedInteriorID, "light_growthi_stage23_upgrade");
        game.activateInteriorEntitySet(WeedInteriorID, "weed_hosei");
        game.activateInteriorEntitySet(WeedInteriorID, "weed_set_up");
        game.activateInteriorEntitySet(WeedInteriorID, "weed_drying");
        game.activateInteriorEntitySet(WeedInteriorID, "weed_chairs");
        game.activateInteriorEntitySet(WeedInteriorID, "weed_production");
        game.refreshInterior(WeedInteriorID);
    }

    //METH-LAB
    let MethInteriorID = game.getInteriorAtCoords(1009.5, -3196.6, -38.99682);
    if (game.isValidInterior(MethInteriorID)) {
        alt.requestIpl('bkr_biker_interior_placement_interior_2_biker_dlc_int_ware01_milo');
        game.activateInteriorEntitySet(MethInteriorID, "meth_lab_setup");
        game.activateInteriorEntitySet(MethInteriorID, "meth_lab_upgrade");
        game.activateInteriorEntitySet(MethInteriorID, "meth_lab_security_high");
        game.activateInteriorEntitySet(MethInteriorID, "meth_lab_production");
        game.refreshInterior(MethInteriorID);
    }

    //BikerDocumentForgery
    let BikerDocumentForgery = game.getInteriorAtCoords(1165, -3196.6, -39.01306);
    if (game.isValidInterior(BikerDocumentForgery)) {
        alt.requestIpl('bkr_biker_interior_placement_interior_6_biker_dlc_int_ware05_milo');
        game.activateInteriorEntitySet(BikerDocumentForgery, "interior_upgrade");
        game.activateInteriorEntitySet(BikerDocumentForgery, "equipment_upgrade");
        game.activateInteriorEntitySet(BikerDocumentForgery, "security_high");
        game.activateInteriorEntitySet(BikerDocumentForgery, "chair01");
        game.activateInteriorEntitySet(BikerDocumentForgery, "chair02");
        game.activateInteriorEntitySet(BikerDocumentForgery, "chair03");
        game.activateInteriorEntitySet(BikerDocumentForgery, "chair04");
        game.activateInteriorEntitySet(BikerDocumentForgery, "chair05");
        game.activateInteriorEntitySet(BikerDocumentForgery, "chair06");
        game.activateInteriorEntitySet(BikerDocumentForgery, "chair07");
        game.activateInteriorEntitySet(BikerDocumentForgery, "production");
        game.activateInteriorEntitySet(BikerDocumentForgery, "set_up");
        game.activateInteriorEntitySet(BikerDocumentForgery, "clutter");
        game.refreshInterior(BikerDocumentForgery);
    }

    //ClubHaus1
    let ClubHaus1 = game.getInteriorAtCoords(1107.04, -3157.399, -37.51859);
    if (game.isValidInterior(ClubHaus1)) {
        alt.requestIpl('bkr_biker_interior_placement_interior_0_biker_dlc_int_01_milo');
        game.activateInteriorEntitySet(ClubHaus1, "walls_02");
        game.activateInteriorEntitySet(ClubHaus1, "furnishings_02");
        game.activateInteriorEntitySet(ClubHaus1, "decorative_02");
        game.activateInteriorEntitySet(ClubHaus1, "mural_07");
        game.activateInteriorEntitySet(ClubHaus1, "no_gun_locker");
        game.activateInteriorEntitySet(ClubHaus1, "mod_booth");
        game.activateInteriorEntitySet(ClubHaus1, "id_stash1");
        game.activateInteriorEntitySet(ClubHaus1, "id_stash2");
        game.activateInteriorEntitySet(ClubHaus1, "id_stash3");
        game.refreshInterior(ClubHaus1);
    }

    //ClubHaus2
    let ClubHaus2 = game.getInteriorAtCoords(998.4809, -3164.711, -38.90733);
    if (game.isValidInterior(ClubHaus2)) {
        alt.requestIpl('bkr_biker_interior_placement_interior_1_biker_dlc_int_02_milo');
        game.activateInteriorEntitySet(ClubHaus2, "walls_01");
        game.activateInteriorEntitySet(ClubHaus2, "lower_walls_default");
        game.activateInteriorEntitySet(ClubHaus2, "furnishings_01");
        game.activateInteriorEntitySet(ClubHaus2, "decorative_02");
        game.activateInteriorEntitySet(ClubHaus2, "mural_09");
        game.activateInteriorEntitySet(ClubHaus2, "gun_locker");
        game.activateInteriorEntitySet(ClubHaus2, "mod_booth");
        game.refreshInterior(ClubHaus2);
    }

    //Doomsday
    let Doomsday = game.getInteriorAtCoords(345.00000000, 4842.00000000, -60.00000000);
    if (game.isValidInterior(Doomsday)) {
        alt.requestIpl('xm_x17dlc_int_placement_interior_33_x17dlc_int_02_milo_');
        game.activateInteriorEntitySet(Doomsday, "set_int_02_decal_01");
        game.activateInteriorEntitySet(Doomsday, "set_int_02_shell");
        game.activateInteriorEntitySet(Doomsday, "set_int_02_lounge3");
        game.activateInteriorEntitySet(Doomsday, "set_int_02_sleep3");
        game.activateInteriorEntitySet(Doomsday, "set_int_02_security");
        game.activateInteriorEntitySet(Doomsday, "set_int_02_cannon");
        game.activateInteriorEntitySet(Doomsday, "Set_Int_02_outfit_serverfarm");
        game.activateInteriorEntitySet(Doomsday, "Set_Int_02_outfit_iaa");
        game.activateInteriorEntitySet(Doomsday, "set_int_02_trophy_iaa");
        game.activateInteriorEntitySet(Doomsday, "set_int_02_trophy_sub");
        game.refreshInterior(Doomsday);
    }

    //CasPent
    let CasPent = game.getInteriorAtCoords(976.636, 70.295, 115.164);
    if (game.isValidInterior(CasPent)) {
        alt.requestIpl('vw_casino_penthouse');
        game.activateInteriorEntitySet(CasPent, "Set_Pent_Tint_Shell");
        game.activateInteriorEntitySet(CasPent, "Set_Pent_Pattern_01");
        game.activateInteriorEntitySet(CasPent, "Set_Pent_Spa_Bar_Open");
        game.activateInteriorEntitySet(CasPent, "Set_Pent_Media_Bar_Open");
        game.activateInteriorEntitySet(CasPent, "Set_Pent_Dealer");
        game.activateInteriorEntitySet(CasPent, "Set_Pent_Arcade_Modern");
        game.activateInteriorEntitySet(CasPent, "Set_Pent_Bar_Clutter");
        game.activateInteriorEntitySet(CasPent, "Set_Pent_Clutter_01");
        game.activateInteriorEntitySet(CasPent, "Set_Pent_Clutter_02");
        game.activateInteriorEntitySet(CasPent, "Set_Pent_Clutter_03");
        game.activateInteriorEntitySet(CasPent, "set_pent_bar_party_1");
        game.refreshInterior(CasPent);
    }

    //YACHT
    alt.requestIpl("apa_yacht_grp12_3");
    alt.requestIpl("apa_yacht_grp12_3_int");
    alt.requestIpl("apa_yacht_grp12_3_lod");
    alt.removeIpl("apa_yacht_grp12_1");
    alt.removeIpl("apa_yacht_grp12_1_int");
    alt.removeIpl("apa_yacht_grp12_1_lod");
    alt.removeIpl("apa_yacht_grp12_2");
    alt.removeIpl("apa_yacht_grp12_2_int");
    alt.removeIpl("apa_yacht_grp12_2_lod");
    alt.removeIpl("apa_ch2_superyacht");
    alt.removeIpl('smboat');
    alt.removeIpl('smboat_lod');

    let YachtInteriorID = game.getInteriorAtCoords(-2000.0, 1113.211, -25.36243);
    if (game.isValidInterior(YachtInteriorID)) {
        game.activateInteriorEntitySet(YachtInteriorID, "apa_mp_apa_yacht_radar_01a");
        game.activateInteriorEntitySet(YachtInteriorID, "apa_mp_apa_yacht_option3");
        game.activateInteriorEntitySet(YachtInteriorID, "apa_mp_apa_yacht_o3_rail_a");
        game.activateInteriorEntitySet(YachtInteriorID, "apa_mp_apa_yacht_launcher_01a");
        game.activateInteriorEntitySet(YachtInteriorID, "apa_mp_apa_y3_l2b");
        game.activateInteriorEntitySet(YachtInteriorID, "apa_mp_apa_yacht_jacuzzi_ripple1");
        game.refreshInterior(YachtInteriorID);
    }

    let TunerInteriorID = game.getInteriorAtCoords(810.46170000, -962.07540000, 25.30295000);
    if (game.isValidInterior(TunerInteriorID)) {

        alt.requestIpl("tunergarage_milo_");

        game.setInteriorEntitySetColor(TunerInteriorID, "entity_set_style_7", 9);
        game.setInteriorEntitySetColor(TunerInteriorID, "entity_set_style_7", 9);

        game.activateInteriorEntitySet(TunerInteriorID, "entity_set_style_1"); // Default Design
        game.deactivateInteriorEntitySet(TunerInteriorID, "entity_set_style_2"); // White Design
        game.deactivateInteriorEntitySet(TunerInteriorID, "entity_set_style_3"); // Dark Design
        game.deactivateInteriorEntitySet(TunerInteriorID, "entity_set_style_4"); // Concrete Design
        game.deactivateInteriorEntitySet(TunerInteriorID, "entity_set_style_5"); // Home Design
        game.deactivateInteriorEntitySet(TunerInteriorID, "entity_set_style_6"); // Street Design
        game.deactivateInteriorEntitySet(TunerInteriorID, "entity_set_style_7"); // Japan Design
        game.deactivateInteriorEntitySet(TunerInteriorID, "entity_set_style_8"); // Color Design
        game.deactivateInteriorEntitySet(TunerInteriorID, "entity_set_style_9"); // Race Design


        game.activateInteriorEntitySet(TunerInteriorID, "entity_set_bedroom"); // With Bed room
        game.deactivateInteriorEntitySet(TunerInteriorID, "entity_set_bedroom_empty"); // Bed room is clean

        game.activateInteriorEntitySet(TunerInteriorID, "entity_set_table"); // nothing
        game.activateInteriorEntitySet(TunerInteriorID, "entity_set_thermal"); // nothing
        game.activateInteriorEntitySet(TunerInteriorID, "entity_set_tints"); // railing
        game.activateInteriorEntitySet(TunerInteriorID, "entity_set_train"); // nothing
        game.activateInteriorEntitySet(TunerInteriorID, "entity_set_laptop"); // nothing
        game.activateInteriorEntitySet(TunerInteriorID, "entity_set_lightbox"); // lights ceiling
        game.activateInteriorEntitySet(TunerInteriorID, "entity_set_plate"); // nothing
        game.activateInteriorEntitySet(TunerInteriorID, "entity_set_cabinets"); // nothing
        game.activateInteriorEntitySet(TunerInteriorID, "entity_set_chalkboard"); // panel at the top of two rooms in front
        game.deactivateInteriorEntitySet(TunerInteriorID, "entity_set_box_clutter"); // Box

        game.deactivateInteriorEntitySet(TunerInteriorID, "entity_set_car_lift_cutscene"); // Carlift Cutscene
        game.deactivateInteriorEntitySet(TunerInteriorID, "entity_set_car_lift_default"); // Carlift Default
        game.deactivateInteriorEntitySet(TunerInteriorID, "entity_set_car_lift_purchase"); // Carlift Purchase

        game.activateInteriorEntitySet(TunerInteriorID, "entity_set_scope"); // nothing
        game.activateInteriorEntitySet(TunerInteriorID, "entity_set_cut_seats"); // Seats in corner
        game.activateInteriorEntitySet(TunerInteriorID, "entity_set_def_table"); // nothing
        game.activateInteriorEntitySet(TunerInteriorID, "entity_set_container"); // nothing
        game.activateInteriorEntitySet(TunerInteriorID, "entity_set_virus"); // nothing
        game.activateInteriorEntitySet(TunerInteriorID, "entity_set_bombs"); // nothing

        game.refreshInterior(TunerInteriorID);
    }

    let CarMeetInteriorID = game.getInteriorAtCoords(-2000.0, 1113.211, -25.36243);
    if (game.isValidInterior(CarMeetInteriorID)) {
        game.activateInteriorEntitySet(CarMeetInteriorID, "entity_set_meet_crew"); // nothing
        game.activateInteriorEntitySet(CarMeetInteriorID, "entity_set_meet_lights"); // activate every light
        game.activateInteriorEntitySet(CarMeetInteriorID, "entity_set_meet_lights_cheap"); // activate every cheap light
        game.activateInteriorEntitySet(CarMeetInteriorID, "entity_set_player"); // nothing
        game.activateInteriorEntitySet(CarMeetInteriorID, "entity_set_test_crew"); // nothing
        game.activateInteriorEntitySet(CarMeetInteriorID, "entity_set_test_lights"); // activate every light on the test race
        game.activateInteriorEntitySet(CarMeetInteriorID, "entity_set_test_lights_cheap"); // activate every cheap light on the test race
        game.activateInteriorEntitySet(CarMeetInteriorID, "entity_set_time_trial"); // activate the white traces on the ground

        game.refreshInterior(CarMeetInteriorID);
    }
}