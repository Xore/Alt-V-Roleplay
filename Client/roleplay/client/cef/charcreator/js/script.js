    var gender = undefined;
    var headoverlaysdata = [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, 0];
    var headoverlaysopacitydata = [1, 10, 10, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0];
    var headoverlayscolordata = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
    var headoverlaysarray = [headoverlaysdata, headoverlaysopacitydata, headoverlayscolordata];
    var facefeaturesdata = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
    var headblendsdata = [0, 0, 0, 0, 0, 0, 0, 0, 0];
    var cam = "";
    var hatselect = -1,
        jacketselect = -1,
        legselect = -1,
        shoeselect = -1;
    var hats = ["-1/0", "2/0", "3/1", "4/0", "4/1", "5/0", "7/0", "7/2", "45/1", "44/1", "55/9"];
    var hatswomen = ["-1/0", "15/0", "54/0", "4/0", "4/1", "5/0", "14/0", "22/0", "58/0", "44/1", "55/9"];
    var jackets = ["0/0", "1/0", "6/0", "7/0", "12/0", "12/2", "17/4", "57/0", "61/3", "80/0", "82/2"];
    var tops = ["15/0", "1/0", "18/0", "23/0", "15/0", "15/0", "15/0", "15/0", "15/0", "15/0", "15/0"];
    var jacketswomen = ["0/0", "1/0", "6/0", "7/0", "2/0", "74/0", "316/0", "57/0", "111/0", "81/0", "86/0"];
    var topswomen = ["15/0", "0/0", "18/0", "23/0", "2/0", "2/0", "2/0", "1/0", "2/0", "2/0", "2/0"];
    var torso = [0, 0, 1, 1, 1, 1, 5, 1, 1, 11, 11];
    var torsowoman = [0, 5, 1, 7, 2, 24, 3, 6, 4, 11, 9];
    var legs = ["4/0", "5/0", "7/0", "12/0", "14/0", "42/0", "43/0", "47/0", "62/0", "63/0", "73/0"];
    var legswomen = ["4/0", "75/0", "7/0", "12/0", "14/0", "44/0", "43/0", "47/0", "0/0", "63/0", "71/0"];
    var shoes = ["-1/0", "5/0", "12/0", "14/0", "28/0", "32/0", "57/10", "76/0", "80/0", "66/0", "3/0"];
    var shoeswomen = ["-1/0", "5/0", "24/0", "14/0", "28/0", "32/0", "49/0", "68/0", "98/1", "95/0", "3/0"];
    var clothes = [
        [0, 0, 0],
        [0, 0, 0],
        [0, 0, 0],
        [0, 0, 0],
        [0, 0, 0],
        [-1, 0, 0],
        [0, 0, 0],
        [0, 0, 0],
        [0, 0, 0],
        [0, 0, 0],
        [0, 0, 0],
        [-1, 0, false],
        [0, 0, false],
        [0, 0, false],
        [0, 0, false],
        [0, 0, false]
    ];

    $(document).ready(function() {
        var date_input = $('input[name="birthday"]');
        var options = {
            format: "dd.mm.yyyy",
            startView: 2,
            weekStart: 1,
            defaultViewDate: {
                year: "1998",
                month: "0",
                day: "1"
            },
            language: "de",
            autoclose: true,
            todayHighlight: true
        };
        date_input.datepicker(options);

        $("#charrotationslider").hide();
        $("#SexArea").hide();
        $("#creatorarea").hide();
        $("#charcreatorsecondbox").hide();
        alt.emit("Client:Charcreator:cefIsReady");
    });

    $('#femalegender').on('click', function() {
        alt.emit('Client:Charcreator:ChangeGender', true);
        gender = true;
    });

    $("#malegender").click(function() {
        alt.emit('Client:Charcreator:ChangeGender', false);
        gender = false;
    });

    function clickdrop(current) {
        $(".dropdown-selector").prop("checked", false);
        $(current).prop("checked", true);
    }

    function ChangeHair(id, index) {
        switch (id) {
            case 0:
                headoverlaysarray[0][13] = document.getElementById("HairVariation").value;
                break;
            case 1:
                headoverlaysarray[2][13] = document.getElementById("HairColor1").value;
                break;
            case 2:
                headoverlaysarray[1][13] = document.getElementById("HairColor2").value;
                break;
            case 3:
                headoverlaysarray[0][1] = document.getElementById("BeardVariation").value;
                break;
            case 4:
                headoverlaysarray[2][1] = document.getElementById("BeardColor").value;
                break;
            case 5:
                headoverlaysarray[0][2] = document.getElementById("EyebrowsVariation").value;
                break;
            case 6:
                headoverlaysarray[2][2] = document.getElementById("EyebrowsColor").value;
                break;
            case 7:
                headoverlaysarray[1][1] = document.getElementById("BeardAlpha").value;
                break;
            case 8:
                headoverlaysarray[1][2] = document.getElementById("EyebrowsAlpha").value;
                break;
            case 9:
                headoverlaysarray[0][10] = document.getElementById("ChestHairVariation").value;
                break;
            case 10:
                headoverlaysarray[2][10] = document.getElementById("ChestHairColor").value;
                break;
            case 11:
                headoverlaysarray[1][10] = document.getElementById("ChestHairAlpha").value;
                break;
        }

        alt.emit('Client:Charcreator:UpdateHeadOverlays', JSON.stringify(headoverlaysarray));
        alt.emit('Client:Charcreator:UpdateHeadBlends', JSON.stringify(headblendsdata));
    }

    var fatherVar = document.getElementById("FatherVariation");
    var motherVar = document.getElementById("MotherVariation");
    var fatherSkinVar = document.getElementById("FatherSkin");
    var motherSkinVar = document.getElementById("MotherSkin");

    function ChangeHead(id, index) {
        switch (id) {
            case 0:
                headblendsdata[0] = fatherVar.value;
                if (fatherVar.value == "0") {
                    headblendsdata[0] = 0;
                } else if (fatherVar.value == "1") {
                    headblendsdata[0] = 1;
                } else if (fatherVar.value == "2") {
                    headblendsdata[0] = 2;
                } else if (fatherVar.value == "3") {
                    headblendsdata[0] = 3;
                } else if (fatherVar.value == "4") {
                    headblendsdata[0] = 4;
                } else if (fatherVar.value == "5") {
                    headblendsdata[0] = 5;
                } else if (fatherVar.value == "6") {
                    headblendsdata[0] = 6;
                } else if (fatherVar.value == "7") {
                    headblendsdata[0] = 7;
                } else if (fatherVar.value == "8") {
                    headblendsdata[0] = 8;
                } else if (fatherVar.value == "9") {
                    headblendsdata[0] = 9;
                } else if (fatherVar.value == "10") {
                    headblendsdata[0] = 10;
                } else if (fatherVar.value == "11") {
                    headblendsdata[0] = 11;
                } else if (fatherVar.value == "12") {
                    headblendsdata[0] = 12;
                } else if (fatherVar.value == "13") {
                    headblendsdata[0] = 13;
                } else if (fatherVar.value == "14") {
                    headblendsdata[0] = 14;
                } else if (fatherVar.value == "15") {
                    headblendsdata[0] = 15;
                } else if (fatherVar.value == "16") {
                    headblendsdata[0] = 16;
                } else if (fatherVar.value == "17") {
                    headblendsdata[0] = 17;
                } else if (fatherVar.value == "18") {
                    headblendsdata[0] = 18;
                } else if (fatherVar.value == "19") {
                    headblendsdata[0] = 19;
                } else if (fatherVar.value == "20") {
                    headblendsdata[0] = 20;
                } else if (fatherVar.value == "21") {
                    headblendsdata[0] = 42;
                } else if (fatherVar.value == "22") {
                    headblendsdata[0] = 43;
                } else if (fatherVar.value == "23") {
                    headblendsdata[0] = 44;
                }
                break;
            case 1:
                headblendsdata[1] = motherVar.value + 21;
                if (motherVar.value == "0") {
                    headblendsdata[1] = 21;
                } else if (motherVar.value == "1") {
                    headblendsdata[1] = 22;
                } else if (motherVar.value == "2") {
                    headblendsdata[1] = 23;
                } else if (motherVar.value == "3") {
                    headblendsdata[1] = 24;
                } else if (motherVar.value == "4") {
                    headblendsdata[1] = 25;
                } else if (motherVar.value == "5") {
                    headblendsdata[1] = 26;
                } else if (motherVar.value == "6") {
                    headblendsdata[1] = 27;
                } else if (motherVar.value == "7") {
                    headblendsdata[1] = 28;
                } else if (motherVar.value == "8") {
                    headblendsdata[1] = 29;
                } else if (motherVar.value == "9") {
                    headblendsdata[1] = 30;
                } else if (motherVar.value == "10") {
                    headblendsdata[1] = 31;
                } else if (motherVar.value == "11") {
                    headblendsdata[1] = 32;
                } else if (motherVar.value == "12") {
                    headblendsdata[1] = 33;
                } else if (motherVar.value == "13") {
                    headblendsdata[1] = 34;
                } else if (motherVar.value == "14") {
                    headblendsdata[1] = 35;
                } else if (motherVar.value == "15") {
                    headblendsdata[1] = 37;
                } else if (motherVar.value == "16") {
                    headblendsdata[1] = 38;
                } else if (motherVar.value == "17") {
                    headblendsdata[1] = 40;
                } else if (motherVar.value == "18") {
                    headblendsdata[1] = 41;
                } else if (motherVar.value == "19") {
                    headblendsdata[1] = 45;
                }
                break;
            case 2:
                headblendsdata[3] = document.getElementById("FatherMotherMix").value;
                break;
            case 3:
                headoverlaysarray[1][5] = document.getElementById("WangenBlushAlpha").value;
                break;
            case 4:
                headoverlaysarray[0][5] = document.getElementById("WangenBlushVariation").value;
                break;
            case 5:
                headoverlaysarray[2][5] = document.getElementById("WangenBlushColor").value;
                break;
            case 6:
                headoverlaysarray[1][4] = document.getElementById("MakeupAlpha").value;
                break;
            case 7:
                headoverlaysarray[0][4] = document.getElementById("MakeupVariation").value;
                break;
            case 8:
                headoverlaysarray[1][8] = document.getElementById("LipstickAlpha").value;
                break;
            case 9:
                headoverlaysarray[0][8] = document.getElementById("LipstickVariation").value;
                break;
            case 10:
                headoverlaysarray[2][8] = document.getElementById("LipstickColor").value;
                break;
            case 11:
                headoverlaysarray[1][0] = document.getElementById("BlemishesAlpha").value;
                break;
            case 12:
                headoverlaysarray[0][0] = document.getElementById("BlemishesIndex").value;
                break;
            case 13:
                headoverlaysarray[1][3] = document.getElementById("AgeingAlpha").value;
                break;
            case 14:
                headoverlaysarray[0][3] = document.getElementById("AgeingVariation").value;
                break;
            case 15:
                headoverlaysarray[1][6] = document.getElementById("ComplexionAlpha").value;
                break;
            case 16:
                headoverlaysarray[0][6] = document.getElementById("ComplexionIndex").value;
                break;
            case 17:
                headoverlaysarray[1][7] = document.getElementById("SunDamageAlpha").value;
                break;
            case 18:
                headoverlaysarray[0][7] = document.getElementById("SunDamageIndex").value;
                break;
            case 19:
                headoverlaysarray[1][9] = document.getElementById("MolesAlpha").value;
                break;
            case 20:
                headoverlaysarray[0][9] = document.getElementById("MolesIndex").value;
                break;
            case 21:
                headblendsdata[5] = fatherSkinVar.value;
                if (fatherSkinVar.value == "0") {
                    headblendsdata[5] = 0;
                } else if (fatherSkinVar.value == "1") {
                    headblendsdata[5] = 1;
                } else if (fatherSkinVar.value == "2") {
                    headblendsdata[5] = 2;
                } else if (fatherSkinVar.value == "3") {
                    headblendsdata[5] = 3;
                } else if (fatherSkinVar.value == "4") {
                    headblendsdata[5] = 4;
                } else if (fatherSkinVar.value == "5") {
                    headblendsdata[5] = 5;
                } else if (fatherSkinVar.value == "6") {
                    headblendsdata[5] = 6;
                } else if (fatherSkinVar.value == "7") {
                    headblendsdata[5] = 7;
                } else if (fatherSkinVar.value == "8") {
                    headblendsdata[5] = 8;
                } else if (fatherSkinVar.value == "9") {
                    headblendsdata[5] = 9;
                } else if (fatherSkinVar.value == "10") {
                    headblendsdata[5] = 10;
                } else if (fatherSkinVar.value == "11") {
                    headblendsdata[5] = 11;
                } else if (fatherSkinVar.value == "12") {
                    headblendsdata[5] = 12;
                } else if (fatherSkinVar.value == "13") {
                    headblendsdata[5] = 13;
                } else if (fatherSkinVar.value == "14") {
                    headblendsdata[5] = 14;
                } else if (fatherSkinVar.value == "15") {
                    headblendsdata[5] = 15;
                } else if (fatherSkinVar.value == "16") {
                    headblendsdata[5] = 16;
                } else if (fatherSkinVar.value == "17") {
                    headblendsdata[5] = 17;
                } else if (fatherSkinVar.value == "18") {
                    headblendsdata[5] = 18;
                } else if (fatherSkinVar.value == "19") {
                    headblendsdata[5] = 19;
                } else if (fatherSkinVar.value == "20") {
                    headblendsdata[5] = 20;
                } else if (fatherSkinVar.value == "21") {
                    headblendsdata[5] = 42;
                } else if (fatherSkinVar.value == "22") {
                    headblendsdata[5] = 43;
                } else if (fatherSkinVar.value == "23") {
                    headblendsdata[5] = 44;
                }
                break;
            case 22:
                headblendsdata[2] = motherSkinVar.value;
                if (motherSkinVar.value == "0") {
                    headblendsdata[2] = 21;
                } else if (motherSkinVar.value == "1") {
                    headblendsdata[2] = 22;
                } else if (motherSkinVar.value == "2") {
                    headblendsdata[2] = 23;
                } else if (motherSkinVar.value == "3") {
                    headblendsdata[2] = 24;
                } else if (motherSkinVar.value == "4") {
                    headblendsdata[2] = 25;
                } else if (motherSkinVar.value == "5") {
                    headblendsdata[2] = 26;
                } else if (motherSkinVar.value == "6") {
                    headblendsdata[2] = 27;
                } else if (motherSkinVar.value == "7") {
                    headblendsdata[2] = 28;
                } else if (motherSkinVar.value == "8") {
                    headblendsdata[2] = 29;
                } else if (motherSkinVar.value == "9") {
                    headblendsdata[2] = 30;
                } else if (motherSkinVar.value == "10") {
                    headblendsdata[2] = 31;
                } else if (motherSkinVar.value == "11") {
                    headblendsdata[2] = 32;
                } else if (motherSkinVar.value == "12") {
                    headblendsdata[2] = 33;
                } else if (motherSkinVar.value == "13") {
                    headblendsdata[2] = 34;
                } else if (motherSkinVar.value == "14") {
                    headblendsdata[2] = 35;
                } else if (motherSkinVar.value == "15") {
                    headblendsdata[2] = 37;
                } else if (motherSkinVar.value == "16") {
                    headblendsdata[2] = 38;
                } else if (motherSkinVar.value == "17") {
                    headblendsdata[2] = 40;
                } else if (motherSkinVar.value == "18") {
                    headblendsdata[2] = 41;
                } else if (motherSkinVar.value == "19") {
                    headblendsdata[2] = 45;
                }
                break;
            case 23:
                headblendsdata[4] = document.getElementById("FatherMotherSkinMix").value;
                break;
            case 24:
                headoverlaysarray[0][14] = document.getElementById("EyesColor").value;
                break;
        }

        alt.emit('Client:Charcreator:UpdateHeadOverlays', JSON.stringify(headoverlaysarray));
        alt.emit('Client:Charcreator:UpdateHeadBlends', JSON.stringify(headblendsdata));
    }

    function ChangeFace(id) {
        switch (id) {
            case 1:
                facefeaturesdata[6] = document.getElementById("EyebrowsHeight").value;
                break;
            case 2:
                facefeaturesdata[7] = document.getElementById("EyebrowsWidth").value;
                break;
            case 3:
                facefeaturesdata[11] = document.getElementById("EyesSize").value;
                break;
            case 4:
                facefeaturesdata[12] = document.getElementById("lipsfill").value;
                break;
            case 5:
                facefeaturesdata[18] = document.getElementById("chinShape").value;
                break;
            case 6:
                facefeaturesdata[16] = document.getElementById("chinPosition").value;
                break;
            case 7:
                facefeaturesdata[15] = document.getElementById("chinLength").value;
                break;
            case 8:
                facefeaturesdata[17] = document.getElementById("chinWidth").value;
                break;
            case 9:
                facefeaturesdata[13] = document.getElementById("jawWidth").value;
                break;
            case 10:
                facefeaturesdata[14] = document.getElementById("jawHeight").value;
                break;
            case 11:
                facefeaturesdata[8] = document.getElementById("cheekboneHeight").value;
                break;
            case 12:
                facefeaturesdata[9] = document.getElementById("cheekboneWidth").value;
                break;
            case 13:
                facefeaturesdata[10] = document.getElementById("cheeksWidth").value;
                break;
            case 14:
                facefeaturesdata[19] = document.getElementById("neckWidth").value;
                break;
            case 15:
                facefeaturesdata[4] = document.getElementById("noseTip").value;
                break;
            case 16:
                facefeaturesdata[1] = document.getElementById("noseHeight").value;
                break;
            case 17:
                facefeaturesdata[2] = document.getElementById("noseLength").value;
                break;
            case 18:
                facefeaturesdata[3] = document.getElementById("noseBridge").value;
                break;
            case 19:
                facefeaturesdata[5] = document.getElementById("noseShift").value;
                break;
            case 20:
                facefeaturesdata[0] = document.getElementById("noseWidth").value;
                break;
        }
        alt.emit('Client:Charcreator:UpdateFaceFeature', JSON.stringify(facefeaturesdata));
    }

    function hatselector(angle) {
        if (gender == "none") return;
        if (angle == "left") {
            if (hatselect == 0 || hatselect == -1) return;
        } else {
            if (hatselect == 10) return;
        }
        if (cam !== "hat") {
            alt.emit('Client:Creator:SetClothesCamProp');
        }
        cam = "hat";

        if (angle == "left") {
            hatselect = hatselect - 1;
        } else {
            hatselect = hatselect + 1;
        }

        if (gender == "male") {
            var hat = hats[hatselect].split('/');
        } else if (gender == "female") {
            var hat = hatswomen[hatselect].split('/');
        }

        var hatid = hat[0];
        var hattex = hat[1];

        if (gender == "male" && hatid == -1) {
            hatid = 11;
        } else if (gender == "female" && hatid == -1) {
            hatid = 57;
        }

        alt.emit('Client:Creator:UpdateProp', 0, hatid, hattex);
        clothes[11] = [hatid, hattex, true];
    }


    function shoeselector(angle) {
        if (gender == "none") return;
        if (angle == "left") {
            if (shoeselect == 0 || shoeselect == -1) return;
        } else {
            if (shoeselect == 10) return;
        }
        if (cam !== "shoe") {
            alt.emit('Client:Creator:SetClothesCamShoes');
        }
        cam = "shoe";

        if (angle == "left") {
            shoeselect = shoeselect - 1;
        } else {
            shoeselect = shoeselect + 1;
        }

        if (gender == "male") {
            var shoe = shoes[shoeselect].split('/');
        } else if (gender == "female") {
            var shoe = shoeswomen[shoeselect].split('/');
        }

        var shoeid = shoe[0];
        var shoetex = shoe[1];

        if (gender == "male" && shoeid == -1) {
            shoeid = 34;
        } else if (gender == "female" && shoeid == -1) {
            shoeid = 35;
        }

        alt.emit('Client:Creator:UpdateClothe', 6, shoeid, shoetex, 0);
        clothes[5] = [shoeid, shoetex, 0];
    }

    function legsselector(angle) {
        if (gender == "none") return;
        if (angle == "left") {
            if (legselect == 0 || legselect == -1) return;
        } else {
            if (legselect == 10) return;
        }
        if (cam !== "leg") {
            alt.emit('Client:Creator:SetClothesCamLegs');
        }
        cam = "leg";

        if (angle == "left") {
            legselect = legselect - 1;
        } else {
            legselect = legselect + 1;
        }

        if (gender == "male") {
            var leg = legs[legselect].split('/');
        } else if (gender == "female") {
            var leg = legswomen[legselect].split('/');
        }

        var legid = leg[0];
        var legtex = leg[1];

        alt.emit('Client:Creator:UpdateClothe', 4, legid, legtex, 0);
        clothes[3] = [legid, legtex, 0];
    }

    function jacketselector(angle) {
        if (gender == "none") return;
        if (angle == "left") {
            if (jacketselect == 0 || jacketselect == -1) return;
        } else {
            if (jacketselect == 10) return;
        }

        if (cam !== "jacket") {
            alt.emit('Client:Creator:SetClothesCamJacket');
        }
        cam = "jacket";

        if (angle == "left") {
            jacketselect = jacketselect - 1;
        } else {
            jacketselect = jacketselect + 1;
        }

        if (gender == "male") {
            var jacket = jackets[jacketselect].split('/');
            var top = tops[jacketselect].split('/');
        } else if (gender == "female") {
            var jacket = jacketswomen[jacketselect].split('/');
            var top = topswomen[jacketselect].split('/');
        }

        var jacketid = jacket[0];
        var jackettex = jacket[1];
        var topid = top[0];
        var toptex = top[1];

        alt.emit('Client:Creator:UpdateClothe', 11, jacketid, jackettex, 0);
        alt.emit('Client:Creator:UpdateClothe', 8, topid, toptex, 0);
        clothes[10] = [jacketid, jackettex, 0];
        clothes[7] = [topid, toptex, 0];

        if (gender == "male") {
            alt.emit('Client:Creator:UpdateClothe', 3, torso[jacketselect], 0, 0);
            clothes[2] = [torso[jacketselect], 0, 0];
        } else if (gender == "female") {
            alt.emit('Client:Creator:UpdateClothe', 3, torsowoman[jacketselect], 0, 0);
            clothes[2] = [torsowoman[jacketselect], 0, 0];
        }
    }

    function FinishCharCreator() {
        var vornameval = $("#vornameinput").val().replace(/^\s+|\s+$/g, "");
        var nachnameval = $("#nachnameinput").val().replace(/^\s+|\s+$/g, "");
        var birthday = $("#birthday").val();

        if (vornameval.length <= 2) {
            showError("Vorname leer oder zu kurz.");
            return;
        }

        if (nachnameval.length <= 3) {
            showError("Nachname leer oder zu kurz.");
            return;
        }

        if (birthday.length < 10 || !isValidDate(birthday)) {
            showError("Geburtsdatum ist ungültig.");
            return;
        }
        showError("none");
        savecharacter();
    }

    function isValidDate(date) {
        var temp = date.split('.');
        var d = new Date(temp[1] + '/' + temp[0] + '/' + temp[2]);
        return (d && (d.getMonth() + 1) == temp[1] && d.getDate() == Number(temp[0]) && d.getFullYear() == Number(temp[2]));
    }

    function savecharacter() {
        if (gender == undefined) return;
        if (gender == 0 || gender == false) {
            if (clothes[5][0] == -1) clothes[5] = [34, 0, 0];
            if (clothes[11][0] == -1) clothes[11] = [11, 0, false];
            clothes[12] = [0, 0, false];
            clothes[13] = [3, 0, false];
            clothes[14] = [2, 0, false];
            clothes[15] = [0, 0, false];
        } else if (gender == 1 || gender == true) {
            if (clothes[5][0] == -1) clothes[5] = [35, 0, 0];
            if (clothes[11][0] == -1) clothes[11] = [57, 0, false];
            clothes[12] = [12, 0, false];
            clothes[13] = [12, 0, false];
            clothes[14] = [1, 0, false];
            clothes[15] = [7, 0, false];
        }
        alt.emit('Client:Charcreator:SaveCharacter', $("#vornameinput").val(), $("#nachnameinput").val(), $("#birthday").val(), gender, facefeaturesdata.join(';'), headblendsdata.join(';'), headoverlaysarray[0].join(';'), headoverlaysarray[1].join(';'), headoverlaysarray[2].join(';'), JSON.stringify(clothes));
    }

    alt.on("CEF:Charcreator:showArea", (area) => {
        showArea(area);
    });

    alt.on("CEF:Charcreator:showError", (msg) => {
        showError(msg);
    });

    function showArea(area) {
        $("#charrotationslider").hide();
        $("#charerror").fadeOut(800);
        $(".charcreatorbox").fadeOut(800);
        $("#charcreatorsecondbox").fadeOut(800);
        $(".box").fadeOut(800, function() {
            $("#SexArea").hide();
            $("#creatorarea").hide();

            switch (area) {
                case "sexarea":
                    $(".box").fadeTo(800, 1, function() {});
                    $(".box").fadeIn(800);
                    $("#SexArea").fadeIn(800);
                    break;
                case "creatorarea":
                    $(".charcreatorbox").fadeTo(800, 1, function() {});
                    $("#charcreatorsecondbox").fadeTo(800, 1, function() {});
                    $(".charcreatorbox").fadeIn(800);
                    $("#charcreatorsecondbox").fadeIn(800);
                    $("#creatorarea").fadeIn(800);
                    $("#charrotationslider").fadeTo(800, 1, function() {});
                    $("#charrotationslider").fadeIn(800);
                    break;
            }
        });
    }

    function changeSliderRot() {
        alt.emit('Client:Charcreator:SetRotation', $("#charrotationslider").val());
    }

    function showError(msg) {
        if (msg == "none") {
            $("#charerror").fadeOut(800, function() {
                $("#charerror").hide();
            });
            return;
        }
        $("#charerror").text(msg);
        $("#charerror").fadeTo(800, 1, function() {});
        $("#charerror").fadeIn(800);
    }