    var type = 1,
        radius = '18em',
        start = -90,
        globalInteractMenuAction = "close",
        globalAnimationMenuAction = "close",
        globalInteractMenuType = "none",
        globalPlayerBillType = undefined,
        globalPlayerBilltargetCharId = undefined,
        globalRecievePlayerBillType = undefined,
        globalRecievePlayerBillFactionCompanyId = undefined,
        globalRecievePlayerBillAmount = undefined,
        globalRecievePlayerBillReason = undefined,
        globalRecievePlayerBillFactionName = undefined,
        globalRecievePlayerBillGivenCharacterId = undefined,
        playerHasLowHealth = false,
        curVehShopId = undefined,
        globalFuelstationBenzinPrice = undefined,
        globalFuelstationDieselPrice = undefined,
        globalFuelstationStromPrice = undefined,
        globalFuelstationKerosinPrice = undefined,
        globalShopId = undefined,
        targetMessageUser,
        allowChatKeyUp = false,
        selectedChatId = 0,
        globalBarberHeadoverlaysData = undefined,
        globalBarberHeadoverlaysOpacityData = undefined,
        globalBarberHeadoverlaysColorData = undefined,
        globalBarberheadoverlaysarray = [globalBarberHeadoverlaysData, globalBarberHeadoverlaysOpacityData,
            globalBarberHeadoverlaysColorData
        ];

    $(document).ready(function() {
        alt.emit("Client:HUD:cefIsReady");
        setTimeout(function() {
            $("#drinkbox").fadeTo(1000, 1, function() {});
            //$("#voicebox").css("background", "#36b1468a");
            $("#voicebox").fadeTo(1000, 1, function() {});
            $(".money").fadeTo(500, 1, function() {});
        }, 1000);
    });

    $(function() {
        $('[data-toggle="tooltip"]').tooltip()
    });

    $('.nav-tabs > li > a').click(function() {
        $('.nav-tabs > li.active').removeClass('active');
        $(this).parent().parent().addClass('active');
    });
    // Money HUD
    function updateMoneyHUD(currentMoney) {
        $("#moneybar").html(`${numberWithCommas(currentMoney)} $`);
    }

    function updateHealth(health) {
        document.getElementById("healthbar").innerHTML = health + "%";
    }

    function numberWithCommas(x) {
        let numberwithComma = x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        return numberwithComma.replace(/,/g, '.');
    }

    function closeChangeVehOwnerHUD() {
        $("#changeVehOwnerHUD").fadeTo(250, 0, function() {
            $("#changeVehOwnerHUD").hide();
            $("#changeVehOwnerHUD-value").val('');
            alt.emit("Client:Vehicle:closeChangeVehOwnerHUD");
        });
    }

    function showChangeOwnerHUD(vehName) {
        $("changeVehOwnerHUD-desc").html(`Fahrzeug: '${vehName}'`);
        $("#changeVehOwnerHUD").fadeTo(100, 1, function() {
            $("#changeVehOwnerHUD").show();
        });
    }

    function showChangeOwnerHUD(vehName) {
        $("changeVehOwnerHUD-desc").html(`Fahrzeug: '${vehName}'`);
        $("#changeVehOwnerHUD").fadeTo(100, 1, function() {
            $("#changeVehOwnerHUD").show();
        });
    }

    function changeVehOwner() {
        let value = $("#changeVehOwnerHUD-value").val();
        if (value == undefined || value.length <= 0) return;
        alt.emit("Client:Vehicle:changeVehOwner", value);
        closeChangeVehOwnerHUD();
    }
    // Rotation HUD
    function rotatePlayer(htmlElem) {
        let value = $(htmlElem).val();
        alt.emit("Client:Utilities:setRotation", value);
    }

    //Tattoo HUD
    let tattooShopId = undefined,
        globalTattooArray = [],
        ownTattoos = [];

    function SetTattooShopItems(array) {
        array = JSON.parse(array);
        for (var i in array) globalTattooArray.push(array[i]);
    }

    function openTattooHUD(shopId, ownTattoosJSON) {
        tattooShopId = shopId;
        ownTattoosJSON = JSON.parse(ownTattoosJSON);
        for (var i in ownTattoosJSON) ownTattoos.push(ownTattoosJSON[i]);

        $("#TattooShopHUD-TattooList").hide();
        $("#TattooShopHUD-TattooList").html("");
        $("#TattooShopHUD-HomeList").show();
        $("#TattooShopHUD").fadeTo(250, 1, function() {
            $("#TattooShopHUD").show();
            $("#RotationHUD").fadeTo(1, 1, function() {
                $("#RotationHUD").show();
            });
        });
    }

    function closeTattooShopCef() {
        alt.emit("Client:TattooShop:closeShop");
        $("#TattooShopHUD").fadeTo(250, 0, function() {
            $("#TattooShopHUD").hide();
            $("#RotationHUD").fadeTo(1, 0, function() {
                $("#RotationHUD").hide();
            });
            $("#TattooShopHUD-TattooList").hide();
            $("#TattooShopHUD-TattooList").html("");
            $("#TattooShopHUD-HomeList").show();
            tattooShopId = undefined;
            ownTattoos = [];
        });
    }

    function openTattooCategory(category) {
        let html = "";
        for (var i in globalTattooArray) {
            if (globalTattooArray[i].part != category) continue;
            html += "<li class='list-group-item'><span>" + globalTattooArray[i].name + "</span><span>" +
                globalTattooArray[i].price + "$</span><button onclick='previewTattoo(`" + globalTattooArray[i]
                .nameHash + "`, `" + globalTattooArray[i].collection +
                "`);' class='blue btn btn-success'><i class='far fa-eye'></i></button><button onclick='buyTattoo(" +
                globalTattooArray[i].id +
                ");' class='btn btn-success'><i class='fas fa-dollar-sign'></i></button></li>";
        }

        if (category == "ownTattoos") {
            for (var i in ownTattoos)
                html += "<li class='list-group-item'><span>" + ownTattoos[i].name +
                "</span><span></span><button onclick='deleteTattoo(" + ownTattoos[i].tattooId +
                ");' class='btn btn-danger'><i class='fas fa-times'></i></button></li>";
        }

        $("#TattooShopHUD-TattooList").html(html);
        $("#TattooShopHUD-HomeList").hide();
        $("#TattooShopHUD-TattooList").show();
    }

    function deleteTattoo(id) {
        if (id <= 0 || id == undefined) return;
        alt.emit("Client:TattooShop:deleteTattoo", id);
        closeTattooShopCef();
    }

    function buyTattoo(id) {
        if (id <= 0 || id == undefined || tattooShopId <= 0 || tattooShopId == undefined) return;
        alt.emit("Client:TattooShop:buyTattoo", tattooShopId, id);
        closeTattooShopCef();
    }

    function previewTattoo(hash, collection) {
        if (hash == undefined || collection == undefined) return;
        alt.emit("Client:TattooShop:previewTattoo", hash, collection);
    }

    function ShowNotification(notificationtype, message, time) {
        toastr.options.progressBar = true;
        switch (notificationtype) {
            case 0:
                break;
            case 1:
                //Info Notification
                toastr.info(message, "", {
                    timeOut: time
                });
                break;
            case 2:
                //Success Notification
                toastr.success(message, "", {
                    timeOut: time
                });
                break;
            case 3:
                //Warning Notification
                toastr.warning(message, "", {
                    timeOut: time
                });
                break;
            case 4:
                //Error Notification
                toastr.error(message, "", {
                    timeOut: time
                });
                break;
        }
    }

    let neededItemLet, producedItemLet, neededItemAmountLet, produceditemAmountLet, durationLet, neededItemTWOLet,
        neededItemTHREELet, neededItemTWOAmountLet, neededItemTHREEAmountLet = "";

    function FarmingCreateCEF(neededItem, producedItem, neededItemAmount, producedItemAmount, duration, neededItemTWO,
        neededItemTHREE, neededItemTWOAmount, neededItemTHREEAmount) {
        $("#ProcessingBox").fadeTo(1000, 1, function() {});

        document.getElementById("ProcessingBoxButtonBoxes").innerHTML = `     <!--<hr class="ProcessingBoxLinie">-->
        <div class="ProcessingBoxIconBox ProcessingBoxHammerBox">
            <a onclick="startProcessing()">HERSTELLEN</a>
        </div> `;

        if (neededItemTWO != "none") {
            if (neededItemTHREE == "none") {
                document.getElementById("ProcessingBoxNeededItems").innerHTML = `    <div class="ProcessingBoxNeededItem ProcessingBoxNeededItemOne">
                    <img class="ProcessingBoxNeededItemIMG" src="../utils/img/${neededItem}.png">
            ${neededItemAmount}
            </div>
            <div class="ProcessingBoxNeededItem ProcessingBoxNeededItemTwo">
                <img class="ProcessingBoxNeededItemIMG" src="../utils/img/${neededItemTWO}.png">
            ${neededItemTWOAmount}
            </div>
            `;
            } else {
                document.getElementById("ProcessingBoxNeededItems").innerHTML = `    <div class="ProcessingBoxNeededItem ProcessingBoxNeededItemOne">
                    <img class="ProcessingBoxNeededItemIMG" src="../utils/img/${neededItem}.png">
            ${neededItemAmount}
            </div>
            <div class="ProcessingBoxNeededItem ProcessingBoxNeededItemTwo">
                <img class="ProcessingBoxNeededItemIMG" src="../utils/img/${neededItemTWO}.png">
            ${neededItemTWOAmount}
            </div>
            <div class="ProcessingBoxNeededItem ProcessingBoxNeededItemThree">
                <img class="ProcessingBoxNeededItemIMG" src="../utils/img/${neededItemTHREE}.png">
            ${neededItemTHREEAmount}
            </div>
            `;
            }
        } else {
            document.getElementById("ProcessingBoxNeededItems").innerHTML = `    <div class="ProcessingBoxNeededItem ProcessingBoxNeededItemOne">
                <img class="ProcessingBoxNeededItemIMG" src="../utils/img/${neededItem}.png">
         ${neededItemAmount}
          </div>
         `;

        }
        document.getElementById("ProcessingBoxProducedItem").innerHTML = `    <div class="ProcessingBoxNeededItem ProcessingBoxProducedItem">
            <img class="ProcessingBoxNeededItemIMG" src="../utils/img/${producedItem}.png">
        ${producedItemAmount}
        </div>
         `;
        neededItemLet = neededItem;
        producedItemLet = producedItem;
        neededItemAmountLet = neededItemAmount;
        produceditemAmountLet = producedItemAmount;
        durationLet = duration;
        neededItemTWOLet = neededItemTWO;
        neededItemTHREELet = neededItemTHREE;
        neededItemTWOAmountLet = neededItemTWOAmount;
        neededItemTHREEAmountLet = neededItemTHREEAmount;
    }

    function startProcessing() {
        alt.emit("Client:Farming:StartProcessing", neededItemLet, producedItemLet, neededItemAmountLet,
            produceditemAmountLet, durationLet, neededItemTWOLet, neededItemTHREELet, neededItemTWOAmountLet,
            neededItemTHREEAmountLet);
        closeProcessingMenuCEF();
    }

    function closeProcessingMenuCEF() {
        $("#ProcessingBox").fadeOut(100, function() {
            $("#ProcessingBox").hide();
            alt.emit("Client:Farming:closeCEF");
        });
    }


    function SetPlayerHUDInVehicle(state) {
        $("#locationbox").hide();
        $("#locationtext").hide();

        if (state == false) {
            anime({
                targets: '#hud',
                bottom: 280
            });
        } else {
            anime({
                targets: '#hud',
                bottom: 480
            });
            $("#locationbox").fadeTo(1, 1, function() {});
            $("#locationtext").fadeTo(1, 1, function() {});
        }
    }

    function updateVoiceHUD(state) {
        $("#voicemic").attr("src", "../utils/img/micmute.png");
        switch (state) {
            case 0:
                document.getElementById("micbar").innerHTML = "Stumm";
                break;
            case 3:
                document.getElementById("micbar").innerHTML = "Flüstern";
                break;
            case 8:
                document.getElementById("micbar").innerHTML = "Normal";
                break;
            case 15:
                document.getElementById("micbar").innerHTML = "Rufen";
                break;
            case 32:
                document.getElementById("micbar").innerHTML = "Schreien";
                break;
        }
    }

    function updateSpeakState(state) {
        if (state) {
            $("#micicon").css("color", "rgb(47, 120, 255)");
        } else {
            $("#micicon").css("color", "rgb(255, 255, 255)");
        }
    }

    function updateDesireHUD(food, thirst) {
        document.getElementById("foodbar").innerHTML = food + "%";
        document.getElementById("thirstbar").innerHTML = thirst + "%";
    }

    function updateStreetLocation(loc) {
        $("#locationtext").html(loc);
    }

    /* Interaktionsmenü Zeug */
    var interactItems = null;

    function toggleInterActionMenu(state, type, itemArray) {
        var interactHTML = itemArray;
        if (state == true) {
            globalInteractMenuType = type;
            $("#InteractionMenu-List").html(interactHTML);
            interactItems = $("li.interactitem");

            interactItems.mouseleave(function() {
                globalInteractMenuAction = "close";
                $("#InteractionMenu-SelectedTitle").html("Schließen");
            });

            interactItems.mouseenter(function() {
                globalInteractMenuAction = $(this).attr('data-action');
                interactString = $(this).attr('data-actionstring');
                $("#InteractionMenu-SelectedTitle").html($(this).attr('data-actionstring'));
            });
            $("#InteractionMenu-List").show();

            transformInteractMenuItems();
        } else {
            $("#InteractionMenu-List").hide();
        }
    }

    function toggleClothesRadialMenu(state, itemArray) {
        var interactHTML = itemArray;
        if (state == true) {
            $("#InteractionMenu-List").html(interactHTML);
            interactItems = $("li.interactitem");

            interactItems.mouseleave(function() {
                globalAnimationMenuAction = "close";
                $("#InteractionMenu-SelectedTitle").html("Schließen");
            });

            interactItems.mouseenter(function() {
                globalAnimationMenuAction = $(this).attr('data-action');
                interactString = $(this).attr('data-actionstring');
                $("#InteractionMenu-SelectedTitle").html($(this).attr('data-actionstring'));
            });
            $("#InteractionMenu-List").show();

            transformInteractMenuItems();
        } else {
            $("#InteractionMenu-List").hide();
        }
    }

    function transformInteractMenuItems() {
        //Items richtig im Kreis anordnen  
        var $items = $('li.interactitem:not(:first-child)'),
            numberOfItems = (type === 1) ? $items.length : $items.length - 1,
            slice = 360 * type / numberOfItems;

        $items.each(function(i) {
            var $self = $(this),
                rotate = slice * i + start,
                rotateReverse = rotate * -1;

            $self.css({
                'transform': 'rotate(' + rotate + 'deg) translate(' + radius + ') rotate(' +
                    rotateReverse + 'deg)'
            });
        });
    }

    function CreateIdentityCardApplyForm(charname, gender, adress, birthdate) {
        $("#identityCardApplyFormCharname").val(charname);
        if (gender == true || gender == "true") {
            gender = "Weiblich";
        } else if (gender == false || gender == "false") {
            gender = "Männlich";
        }
        $("#identityCardApplyFormGender").val(gender);
        $("#identityCardApplyFormAdress").val(adress);
        $("#identityCardApplyFormBirthdate").val(birthdate);
        $("#identityCardApplyForm").fadeTo(1000, 1, function() {});
    }

    function SendIdentityCardApplyForm() {
        var birthplace = $("#identityCardApplyFormBirthplace").val().replace(/^\s+|\s+$/g, "");
        if (birthplace.length <= 0) {
            return;
        }

        alt.emit("Client:HUD:sendIdentityCardApplyForm", birthplace);
        $("#identityCardApplyForm").fadeTo(1000, 0, function() {
            $("#identityCardApplyForm").hide();
        });
    }

    function BarberChangeHair(id) {
        switch (id) {
            case 0:
                globalBarberHeadoverlaysData[13] = document.getElementById("BarberHairVariation").value;
                if (document.getElementById("BarberHairVariation").value == "23") {
                    globalBarberHeadoverlaysData[13] = 22;
                }
                break;
            case 1:
                globalBarberHeadoverlaysColorData[13] = document.getElementById("BarberHairColor1").value;
                break;
            case 2:
                globalBarberHeadoverlaysOpacityData[13] = document.getElementById("BarberHairColor2").value;
                break;
            case 3:
                globalBarberHeadoverlaysData[1] = document.getElementById("BarberBeardVariation").value;
                break;
            case 4:
                globalBarberHeadoverlaysColorData[1] = document.getElementById("BarberBeardColor").value;
                break;
            case 5:
                globalBarberHeadoverlaysData[2] = document.getElementById("BarberEyebrowsVariation").value;
                break;
            case 6:
                globalBarberHeadoverlaysColorData[2] = document.getElementById("BarberEyebrowsColor").value;
                break;
            case 7:
                globalBarberHeadoverlaysData[4] = document.getElementById("BarberMakeupVariation").value;
                break;
            case 8:
                globalBarberHeadoverlaysOpacityData[4] = document.getElementById("BarberMakeupAlpha").value;
                break;
            case 9:
                globalBarberHeadoverlaysData[8] = document.getElementById("BarberLipstickVariation").value;
                break;
            case 10:
                globalBarberHeadoverlaysColorData[8] = document.getElementById("BarberLipstickColor").value;
                break;
            case 11:
                globalBarberHeadoverlaysData[5] = document.getElementById("BarberWangenBlushVariation").value;
                break;
            case 12:
                globalBarberHeadoverlaysColorData[5] = document.getElementById("BarberWangenBlushColor").value;
                break;
        }
        alt.emit("Client:Barber:UpdateHeadOverlays", JSON.stringify(globalBarberheadoverlaysarray));
    }

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

    function BarberBuyHairStyles() {
        $("#BarberBuyButton").prop("disabled", true);
        alt.emit("Client:Barber:finishBarber", globalBarberheadoverlaysarray[0].join(';'), globalBarberheadoverlaysarray[1].join(';'), globalBarberheadoverlaysarray[2].join(';'));
        barberCEFBoxdestroy();
        setTimeout(() => {
            $("#BarberBuyButton").prop("disabled", false);
        }, 2500);
    }

    function CloseBarberCEF() {
        alt.emit("Client:Barber:RequestCurrentSkin");
        barberCEFBoxdestroy();
    }

    function barberCEFBoxdestroy() {
        $("#barberCEFBox").fadeOut(500, function() {
            $("#barberCEFBox").hide();
        });
        alt.emit("Client:Barber:destroyBarberCEF");
    }

    function clickdropBarber(current) {
        $(".dropdown-selector").prop("checked", false);
        $(current).prop("checked", true);
    }

    function SetVehicleShopCEFListContent(shopid, shopname, itemarray) {
        var vehshophtml = "",
            itemarray = JSON.parse(itemarray);

        for (var i in itemarray) {
            vehshophtml += `<li class='list-group-item'><img src='../utils/img/vehicles/${itemarray[i].hash}.png'><span><b>Fahrzeugname: </b>${itemarray[i].name}</span><br><span><b>Hersteller: </b>${itemarray[i].manufactor}</span><br>` +
                `<span><b>Treibstoff: </b>${itemarray[i].fueltype}</span><br><span><b>Tankgröße: </b>${itemarray[i].maxfuel} Liter</span><br><span><b>Kofferraum: </b>${itemarray[i].trunkcapacity}kg</span><br>` +
                `<span><b>Sitzplätze: </b>${itemarray[i].seats}</span><br><span><b>Preis: </b>${itemarray[i].price}$</span>` +
                "<span class='buybtn' onclick='VehicleShopCEFBuyItem(`" + itemarray[i].hash + "`);'><i class='fas fa-dollar-sign'></i></span></li>";
        }

        $("#VehicleShopCEFBox-title").html(`${shopname}`);
        $("#VehicleShopCEFBox-StoreList").html(vehshophtml);
        $("#VehicleShopCEFBox").fadeTo(1000, 1, function() {});
    }

    function VehicleShopCEFBuyItem(hash) {
        alt.emit("Client:VehicleShop:BuyVehicle", curVehShopId, hash);
        CloseVehicleShopCEF();
    }

    function CloseVehicleShopCEF() {
        $("#VehicleShopCEFBox").fadeOut(500, function() {
            $("#VehicleShopCEFBox").hide();
            curVehShopId = undefined;
        });
        alt.emit("Client:VehicleShop:destroyVehicleShopCEF");
    }

    var globalHotelApartmentsArray = [];

    function setHotelApartmentsItems(array) {
        array = JSON.parse(array);
        for (var i in array) {
            globalHotelApartmentsArray.push(array[i]);
        }
    }

    function openHotelRentCEF(hotelname) {
        var hotelHTML = "";
        for (var i in globalHotelApartmentsArray) {
            hotelHTML +=
                `<li class='list-group-item'><p class='title'>Zimmer ${globalHotelApartmentsArray[i].apartmentId}</p>`;

            if (globalHotelApartmentsArray[i].ownerId >= 1) {
                hotelHTML +=
                    `<p class='rentstate'><i class='fas fa-bed'></i>Mietstatus: <font style='color: #e83838'>Belegt</font></p>` +
                    `<p class='rentstate'><i class='fas fa-user'></i>Mieter: <font>${globalHotelApartmentsArray[i].ownerName}</font></p>`;
            } else {
                hotelHTML +=
                    `<p class='rentstate'><i class='fas fa-bed'></i>Mietstatus: <font style='color: #38e876'>Frei</font></p>` +
                    `<p class='rentstate'><i class='fas fa-user'></i>Mieter: <font>-/-</font></p>`;
            }

            hotelHTML +=
                `<p class='rentstate'><i class='far fa-clock'></i>Mietdauer: <font>${globalHotelApartmentsArray[i].maxRentHours} Std.</font></p>` +
                `<p class='rentstate'><i class='fas fa-dollar-sign'></i>Mietpreis: <font>${globalHotelApartmentsArray[i].rentPrice}$</font></p>` +
                `<button type='button' class='btn btn-sm btn-danger' onclick='LockHotel("${globalHotelApartmentsArray[i].hotelId}", "${globalHotelApartmentsArray[i].apartmentId}");'><i class='fas fa-lock'></i></button>` +
                `<button type='button' class='btn btn-sm btn-danger' onclick='RentHotel("${globalHotelApartmentsArray[i].hotelId}", "${globalHotelApartmentsArray[i].apartmentId}");'><i class='fas fa-dollar-sign'></i></button>` +
                `<button type='button' class='btn btn-sm btn-danger' onclick='EnterHotel("${globalHotelApartmentsArray[i].hotelId}", "${globalHotelApartmentsArray[i].apartmentId}");'><i class='fas fa-sign-in-alt'></i></button></li>`;
        }
        $("#HotelManageCEFBox-ApartmentList").html(hotelHTML);
        $("#HotelManageCEFBox-HotelName").html(`Hotel: ${hotelname}`);
        $("#HotelManageCEFBox").fadeTo(1000, 1, function() {});
    }

    function EnterHotel(hotelId, apartmentId) {
        if (hotelId <= 0 || apartmentId <= 0) return;
        closeHotelManageCEFBox();
        alt.emit("Client:Hotel:EnterHotel", hotelId, apartmentId);
    }

    function RentHotel(hotelId, apartmentId) {
        if (hotelId <= 0 || apartmentId <= 0) return;
        closeHotelManageCEFBox();
        alt.emit("Client:Hotel:RentHotel", hotelId, apartmentId);
    }

    function LockHotel(hotelId, apartmentId) {
        if (hotelId <= 0 || apartmentId <= 0) return;
        closeHotelManageCEFBox();
        alt.emit("Client:Hotel:LockHotel", hotelId, apartmentId);
    }

    function closeHotelManageCEFBox() {
        $("#HotelManageCEFBox").fadeOut(500, function() {
            $("#HotelManageCEFBox").hide();
            alt.emit("Client:Hotel:destroyCEF");
        });
        globalHotelApartmentsArray = [];
    }

    function openHouseManageCEF(houseInfoArray, renterArray) {
        var renterHTML = "",
            upgradeHTML = "",
            houseInfoArray = JSON.parse(houseInfoArray),
            renterArray = JSON.parse(renterArray),
            houseId = 0;

        for (var i in houseInfoArray) {
            houseId = houseInfoArray[i].id;
            if (houseId <= 0 || houseId == undefined) continue;
            if (houseInfoArray[i].ownerId <= 0) continue;
            $("#HouseManageCEFBox-TresorInput").val('');
            $("#HouseManageCEFBox-TresorInput").attr('placeholder', `${houseInfoArray[i].money}`);
            $("#HouseManageCEFBox-rentPriceInput").val('');
            $("#HouseManageCEFBox-rentPriceInput").attr('placeholder', `${houseInfoArray[i].rentPrice}`);
            $('#HouseManageCEFBox-setRentPriceBtn').removeAttr('onClick');
            $('#HouseManageCEFBox-AllowRentersBtn').removeAttr('onClick');
            $('#HouseManageCEFBox-DenyRentersBtn').removeAttr('onClick');
            $('#HouseManageCEFBox-TresorDepositBtn').removeAttr('onClick');
            $('#HouseManageCEFBox-TresorWithdrawBtn').removeAttr('onClick');
            $('#HouseManageCEFBox-setRentPriceBtn').attr('onClick',
                `HouseManageSetNewRentPrice(${houseInfoArray[i].id}, ${houseInfoArray[i].rentPrice});`);
            $('#HouseManageCEFBox-AllowRentersBtn').attr('onClick',
                `HouseManageSetRentState(${houseInfoArray[i].id}, 'true');`);
            $('#HouseManageCEFBox-DenyRentersBtn').attr('onClick',
                `HouseManageSetRentState(${houseInfoArray[i].id}, 'false');`);
            $('#HouseManageCEFBox-TresorDepositBtn').attr('onClick',
                `HouseManageTresorAction(${houseInfoArray[i].id}, 'deposit');`);
            $('#HouseManageCEFBox-TresorWithdrawBtn').attr('onClick',
                `HouseManageTresorAction(${houseInfoArray[i].id}, 'withdraw');`);
            $("#HouseManageCEFBox-TresorAmount").html(`${houseInfoArray[i].money}$`);
            if (houseInfoArray[i].hasStorage) {
                upgradeHTML += `<li class='list-group-item'><p>Lagerraum - 1500$</p></li>`;
            } else {
                upgradeHTML +=
                    `<li class='list-group-item'><p>Lagerraum - 1500$</p><button type='button' onclick='HouseManageBuyUpgrade(${houseInfoArray[i].id}, "storage");' class='btn btn-sm btn-success'><i class='fas fa-dollar-sign'></i></button></li>`;
            }

            if (houseInfoArray[i].hasAlarm) {
                upgradeHTML += `<li class='list-group-item'><p>Alarmanlage - 500$</p></li>`;
            } else {
                upgradeHTML +=
                    `<li class='list-group-item'><p>Alarmanlage - 500$</p><button type='button' onclick='HouseManageBuyUpgrade(${houseInfoArray[i].id}, "alarm");' class='btn btn-sm btn-success'><i class='fas fa-dollar-sign'></i></button></li>`;
            }

            if (houseInfoArray[i].hasBank) {
                upgradeHTML += `<li class='list-group-item'><p>Tresor - 250$</p></li>`;
            } else {
                upgradeHTML +=
                    `<li class='list-group-item'><p>Tresor - 250$</p><button type='button' onclick='HouseManageBuyUpgrade(${houseInfoArray[i].id}, "bank");' class='btn btn-sm btn-success'><i class='fas fa-dollar-sign'></i></button></li>`;
            }
        }

        for (var i in renterArray) {
            if (renterArray[i].charId <= 0 || renterArray[i].charId == undefined) continue;
            renterHTML +=
                `<li class='list-group-item'><p>${renterArray[i].renterName}</p><button type='button' onclick='HouseManageRemoveRenter(${houseId}, ${renterArray[i].charId});' class='btn btn-sm btn-danger'><i class='fas fa-times'></i></button></li>`;
        }

        $("#HouseManageCEFBox-RenterList").html(renterHTML);
        $("#HouseManageCEFBox-UpgradeList").html(upgradeHTML);
        $("#HouseManageCEFBox").fadeTo(1000, 1, function() {});
    }

    function HouseManageTresorAction(houseId, action) {
        if (houseId <= 0 || houseId == undefined) return;
        if (action != "withdraw" && action != "deposit") return;
        var inputVal = $("#HouseManageCEFBox-TresorInput").val();
        if (inputVal == undefined || inputVal <= 0) return;
        closeHouseManageCEFBox();
        alt.emit("Client:HouseManage:TresorAction", houseId, action, inputVal);
    }

    function HouseManageBuyUpgrade(houseId, upgrade) {
        if (houseId <= 0) return;
        if (upgrade != "alarm" && upgrade != "storage" && upgrade != "bank") return;
        closeHouseManageCEFBox();
        alt.emit("Client:HouseManage:BuyUpgrade", houseId, upgrade);
    }

    function HouseManageRemoveRenter(houseId, renterId) {
        if (houseId <= 0 || houseId == undefined || renterId <= 0 || renterId == undefined) return;
        closeHouseManageCEFBox();
        alt.emit("Client:HouseManage:RemoveRenter", houseId, renterId);
    }

    function HouseManageSetRentState(houseId, rentState) {
        if (houseId <= 0 || houseId == undefined || rentState == undefined) return;
        closeHouseManageCEFBox();
        alt.emit("Client:HouseManage:setRentState", houseId, rentState);
    }

    function HouseManageSetNewRentPrice(houseId, currentPrice) {
        if (houseId <= 0 || houseId == undefined) return;
        var inputVal = $("#HouseManageCEFBox-rentPriceInput").val();
        if (inputVal <= 0 || inputVal == undefined || inputVal == currentPrice) return;
        closeHouseManageCEFBox();
        alt.emit("Client:HouseManage:setRentPrice", houseId, inputVal);
    }

    function closeHouseManageCEFBox() {
        $("#HouseManageCEFBox").fadeOut(500, function() {
            $("#HouseManageCEFBox").hide();
            alt.emit("Client:HouseManage:destroyCEF");
        });
    }

    function openHouseEntranceCEF(charId, houseInfoArray, isRentedIn) {
        var houseHTML = "",
            houseInfoArray = JSON.parse(houseInfoArray);

        for (var i in houseInfoArray) {
            var color = "red";
            var isRentable = "nicht mietbar";
            $("#HouseEntranceCEFBox-Cut").removeClass("green");
            $("#HouseEntranceCEFBox-HouseName").removeClass("green");
            if (houseInfoArray[i].isRentable == true) {
                isRentable = "mietbar";
            }
            if (houseInfoArray[i].ownerId <= 0) {
                color = "green";
                houseHTML += "<div class='container'><p class='green'>Eigentümer: <font>Kein Besitzer</font></p></div>";
                $("#HouseEntranceCEFBox-Cut").addClass("green");
                $("#HouseEntranceCEFBox-HouseName").addClass("green");
            } else {
                houseHTML +=
                    `<div class='container'><p class='red'>Eigentümer: <font>${houseInfoArray[i].ownerName}</font></p></div>`;
            }

            houseHTML +=
                `<div class='container'><p class='${color}'>Aktuelle Mieter: <font>${houseInfoArray[i].renterCount}</font></p></div>` +
                `<div class='container'><p class='${color}'>Maximale Mieter: <font>${houseInfoArray[i].maxRenters}</font></p></div>` +
                `<div class='container'><p class='${color}'>Mietpreis: <font>${houseInfoArray[i].rentPrice}$</font></p></div>` +
                `<div class='container'><p class='${color}'>Mietstatus: <font>${isRentable}</font></p></div>` +
                `<div class='container'><p class='${color}'>Kaufpreis: <font>${houseInfoArray[i].price}$</font></p></div>`;


            if (houseInfoArray[i].ownerId <= 0) {
                houseHTML +=
                    `<button type="button" onclick='HouseEntranceBuyHouse(${houseInfoArray[i].id});' class="btn btn-sm btn-danger green">Kaufen</button>`;
            }

            if (houseInfoArray[i].isRentable && !isRentedIn) {
                houseHTML +=
                    `<button type="button" onclick='HouseEntranceRentHouse(${houseInfoArray[i].id});' class="btn btn-sm btn-danger">Einmieten</button>`;
            }

            if (isRentedIn) {
                houseHTML +=
                    `<button type="button" onclick='HouseEntranceUnrentHouse(${houseInfoArray[i].id});' class="btn btn-sm btn-danger">Ausmieten</button>`;
            }

            if (!houseInfoArray[i].isLocked) {
                houseHTML +=
                    `<button type="button" onclick='HouseEntranceEnterHouse(${houseInfoArray[i].id});' class="btn btn-sm btn-danger">Betreten</button>`;
            }

            $("#HouseEntranceCEFBox-HouseName").html(`${houseInfoArray[i].street}`);
        }

        $("#HouseEntranceCEFBox-List").html(houseHTML);
        $("#HouseEntranceCEFBox").fadeTo(1000, 1, function() {});
    }

    function HouseEntranceUnrentHouse(houseId) {
        if (houseId <= 0) return;
        closeHouseEntranceCEFBox();
        alt.emit("Client:HouseEntrance:UnrentHouse", houseId);
    }

    function HouseEntranceRentHouse(houseId) {
        if (houseId <= 0) return;
        closeHouseEntranceCEFBox();
        alt.emit("Client:HouseEntrance:RentHouse", houseId);
    }

    function HouseEntranceBuyHouse(houseId) {
        if (houseId <= 0) return;
        closeHouseEntranceCEFBox();
        alt.emit("Client:HouseEntrance:BuyHouse", houseId);
    }

    function HouseEntranceEnterHouse(houseId) {
        if (houseId <= 0) return;
        closeHouseEntranceCEFBox();
        alt.emit("Client:HouseEntrance:EnterHouse", houseId);
    }

    function closeHouseEntranceCEFBox() {
        $("#HouseEntranceCEFBox").fadeOut(500, function() {
            $("#HouseEntranceCEFBox").hide();
            alt.emit("Client:HouseEntrance:destroyCEF");
        });
    }

    function closeTownHallHouseSelector() {
        $("#TownhallHouseSelectorBox").fadeOut(500, function() {
            $("#TownhallHouseSelectorBox").hide();
            alt.emit("Client:Townhall:destroyHouseSelector");
        });
    }

    function openTownhallHouseSelector(houseArray) {
        var html = "",
            houseArray = JSON.parse(houseArray);

        for (var i in houseArray) {
            html +=
                `<li class='list-group-item'><span>${houseArray[i].street}</span><button type='button' onclick='HouseSelectorSelectHouse(${houseArray[i].id});' class='btn btn-sm btn-success'><i class='fas fa-check'></i></button><button type='button' onclick='HouseSelectorSellHouse(${houseArray[i].id});' class='btn btn-sm btn-danger'><i class='fas fa-dollar-sign'></i></button>`;
        }

        $("#TownHallHouseSelector-List").html(html);
        $("#TownhallHouseSelectorBox").fadeTo(1000, 1, function() {});
    }

    function HouseSelectorSelectHouse(houseId) {
        if (houseId <= 0 || houseId == undefined) return;
        closeTownHallHouseSelector();
        alt.emit("Client:House:setMainHouse", houseId);
    }

    function HouseSelectorSellHouse(houseId) {
        if (houseId <= 0 || houseId == undefined) return;
        closeTownHallHouseSelector();
        alt.emit("Client:House:SellHouse", houseId);
    }

    function isEmptyOrSpaces(str) {
        return str === null || str.match(/^ *$/) !== null;
    }

    function SetFuelstationListContent(fuelStationId, stationName, owner, maxFuelAmount, availableLiter, fuelArray,
        vehID) {
        var fuelstationHTML = "",
            fuelArray = JSON.parse(fuelArray),
            maxFuelAmount = parseInt(maxFuelAmount);

        for (var i in fuelArray) {
            if (fuelArray[i].fueltype == "Benzin") {
                globalFuelstationBenzinPrice = fuelArray[i].fuelPrice;

                fuelstationHTML += "<li class='list-group-item' onclick='VehicleFuelStationFuelVehicle(" + vehID +
                    ", " +
                    fuelStationId + ", `" + fuelArray[i].fueltype + "`);'>" +
                    `<i style='position:absolute;left:15px;color:white;' class='fal fa-gas-pump fa-5x'></i><p id='VehicleFuelStationCEFBox-${fuelArray[i].fueltype}Price'>${fuelArray[i].fueltype} Tanken</p></li>`;

            } else if (fuelArray[i].fueltype == "Diesel") {
                globalFuelstationDieselPrice = fuelArray[i].fuelPrice;

                fuelstationHTML += "<li class='list-group-item' onclick='VehicleFuelStationFuelVehicle(" + vehID +
                    ", " +
                    fuelStationId + ", `" + fuelArray[i].fueltype + "`);'>" +
                    `<i style='position:absolute;left:15px;color:white;' class='fal fa-gas-pump fa-5x'></i><p id='VehicleFuelStationCEFBox-${fuelArray[i].fueltype}Price'>${fuelArray[i].fueltype} Tanken</p></li>`;

            } else if (fuelArray[i].fueltype == "Strom") {
                globalFuelstationStromPrice = fuelArray[i].fuelPrice;

                fuelstationHTML += "<li class='list-group-item' onclick='VehicleFuelStationFuelVehicle(" + vehID +
                    ", " +
                    fuelStationId + ", `" + fuelArray[i].fueltype + "`);'>" +
                    `<i style='position:absolute;left:15px;color:white;' class='fal fa-charging-station fa-5x'></i><p id='VehicleFuelStationCEFBox-${fuelArray[i].fueltype}Price'>${fuelArray[i].fueltype} Aufladen</p></li>`;

            } else if (fuelArray[i].fueltype == "Kerosin") {
                globalFuelstationKerosinPrice = fuelArray[i].fuelPrice;

                fuelstationHTML += "<li class='list-group-item' onclick='VehicleFuelStationFuelVehicle(" + vehID +
                    ", " +
                    fuelStationId + ", `" + fuelArray[i].fueltype + "`);'>" +
                    `<i style='position:absolute;left:15px;color:white;' class='fal fa-gas-pump fa-5x'></i><p id='VehicleFuelStationCEFBox-${fuelArray[i].fueltype}Price'>${fuelArray[i].fueltype} Tanken</p></li>`;

            }
        }

        $("#VehicleFuelstationCEFBox-List").html(fuelstationHTML);
        $("#VehicleFuelstationCEFBox-stationName").html(`${stationName}`);
        //$("#VehicleFuelstationCEFBox-ownerName").html(`${owner}`);
        $("#VehicleFuelstationCEFBox-availableLiter").html(`${availableLiter} Liter`);
        $("#VehicleFuelStationCEFBox-Out").html(`1`);
        $("#VehicleFuelStationCEFBox-LiterSlider").val(`1`);
        $("#VehicleFuelStationCEFBox-LiterSlider").attr({
            "max": maxFuelAmount,
            "min": 1
        });
        $("#VehicleFuelstationCEFBox").fadeTo(1000, 1, function() {});
    }

    function VehicleFuelStationFuelVehicle(vehID, fuelStationId, fueltype) {
        if (fuelStationId == 0 || fuelStationId == undefined || fueltype == "" || fueltype == undefined || fueltype ==
            "undefined" || vehID == 0 || vehID == undefined || vehID == "undefined") return;
        var selectedLiterAmount = $("#VehicleFuelStationCEFBox-LiterSlider").val();
        var selectedLiterPrice = 0;
        if (fueltype == "Benzin") {
            selectedLiterPrice = globalFuelstationBenzinPrice;
        } else if (fueltype == "Diesel") {
            selectedLiterPrice = globalFuelstationDieselPrice;
        } else if (fueltype == "Strom") {
            selectedLiterPrice = globalFuelstationStromPrice;
        } else if (fueltype == "Kerosin") {
            selectedLiterPrice = globalFuelstationKerosinPrice;
        }

        alt.emit("Client:FuelStation:FuelVehicleAction", vehID, fuelStationId, fueltype, selectedLiterAmount,
            selectedLiterPrice);
        closeFuelstationCEF();
    }

    function VehicleFuelStationCEFBoxCalculatePrices() {
        var selectedLiterAmount = $("#VehicleFuelStationCEFBox-LiterSlider").val(),
            benzinPrice = selectedLiterAmount * globalFuelstationBenzinPrice,
            dieselPrice = selectedLiterAmount * globalFuelstationDieselPrice,
            stromPrice = selectedLiterAmount * globalFuelstationStromPrice,
            kerosinPrice = selectedLiterAmount * globalFuelstationKerosinPrice;

        if ($("#VehicleFuelStationCEFBox-BenzinPrice").length > 0) {
            $("#VehicleFuelStationCEFBox-BenzinPrice").html(`BENZIN TANKEN <br>(${benzinPrice}$)`);
        }

        if ($("#VehicleFuelStationCEFBox-DieselPrice").length > 0) {
            $("#VehicleFuelStationCEFBox-DieselPrice").html(`DIESEL TANKEN <br>(${dieselPrice}$)`);
        }

        if ($("#VehicleFuelStationCEFBox-StromPrice").length > 0) {
            $("#VehicleFuelStationCEFBox-StromPrice").html(`STROM AUFLADEN <br>(${stromPrice}$)`);
        }

        if ($("#VehicleFuelStationCEFBox-KerosinPrice").length > 0) {
            $("#VehicleFuelStationCEFBox-KerosinPrice").html(`KEROSIN TANKEN <br>(${kerosinPrice}$)`);
        }
    }

    function closeFuelstationCEF() {
        $("#VehicleFuelstationCEFBox").fadeOut(250, function() {});
        alt.emit("Client:FuelStation:destroyCEF");
    }

    $("#GivePlayerBillCEFBox-FinishBtn").click(function() {
        if (globalPlayerBillType != "faction" && globalPlayerBillType != "company" && globalPlayerBillType !=
            "sellcar") return;
        if (globalPlayerBillType == undefined || globalPlayerBillType == null) return;
        if (globalPlayerBilltargetCharId == undefined || globalPlayerBilltargetCharId == null ||
            globalPlayerBilltargetCharId <= 0) return;
        var selectedMoneyAmount = $("#GivePlayerBillCEFBox-AmountInput").val();
        var selectedReason = $("#GivePlayerBillCEFBox-ReasonInput").val().replace(/^\s+|\s+$/g, "");
        if (selectedReason.length <= 0) {
            return;
        }
        if (selectedMoneyAmount < 1 || selectedMoneyAmount == "") return;

        alt.emit("Client:GivePlayerBill:giveBill", globalPlayerBillType, globalPlayerBilltargetCharId,
            selectedReason, selectedMoneyAmount);
        closeGivePlayerBillCEFBox();
    });

    function closeGivePlayerBillCEFBox() {
        globalPlayerBillType = undefined;
        globalPlayerBilltargetCharId = undefined;
        $("#GivePlayerBillCEFBox").fadeOut(250, function() {});
        alt.emit("Client:GivePlayerBill:destroyCEF");
    }

    function RecievePlayerBillCEFBoxAction(action) {
        if (action != "bar" && action != "bank" && action != "decline") return;
        if (globalRecievePlayerBillType == undefined) return;
        if (globalRecievePlayerBillType != "faction" && globalRecievePlayerBillType != "company" &&
            globalRecievePlayerBillType != "sellcar") return;
        if (globalRecievePlayerBillFactionCompanyId == undefined || globalRecievePlayerBillFactionCompanyId <= 0)
            return;
        if (globalRecievePlayerBillAmount == undefined || globalRecievePlayerBillAmount <= 0) return;
        if (globalRecievePlayerBillReason == undefined || globalRecievePlayerBillReason == "") return;
        if (globalRecievePlayerBillFactionName == undefined || globalRecievePlayerBillFactionName == "") return;
        if (globalRecievePlayerBillGivenCharacterId == undefined || globalRecievePlayerBillGivenCharacterId <= 0)
            return;
        alt.emit("Client:PlayerBill:BillAction", action, globalRecievePlayerBillType,
            globalRecievePlayerBillFactionCompanyId, globalRecievePlayerBillAmount, globalRecievePlayerBillReason,
            globalRecievePlayerBillGivenCharacterId);
        closeReviecePlayerBillCEFBox();
    }

    function closeReviecePlayerBillCEFBox() {
        globalRecievePlayerBillType = undefined;
        globalRecievePlayerBillFactionCompanyId = undefined;
        globalRecievePlayerBillAmount = undefined;
        globalRecievePlayerBillReason = undefined;
        globalRecievePlayerBillFactionName = undefined;
        globalRecievePlayerBillGivenCharacterId = undefined;
        $("#RecievePlayerBillCEFBox").fadeOut(250, function() {});
        alt.emit("Client:RecievePlayerBill:destroyCEF");
    }

    function SetVehicleLicensingCEFBoxContent(vehArray) {
        var licenseHTML = "",
            vehArray = JSON.parse(vehArray);

        for (var i in vehArray) {
            licenseHTML +=
                `<li class='list-group-item'><div class='container left'><p class='title'>FAHRZEUGNAME</p><p class='name'>${vehArray[i].vehName}</p></div>` +
                `<div class='container mid'><p class='title'>KENNZEICHEN</p><p class='name'>${vehArray[i].vehPlate}</p></div>` +
                `<div class='container right'><p class='title'>FAHRZEUG ANMELDEN?</p>` +
                "<button type='button' onclick='VehicleLicensingCEFAction(`anmelden`, " + vehArray[i].vehId + ", `" +
                vehArray[i].vehPlate +
                "`, this);' class='btn btn-sm btn-anmelden'><i class='fas fa-check'></i></button>" +
                "<button type='button' onclick='VehicleLicensingCEFAction(`abmelden`, " + vehArray[i].vehId + ", `" +
                vehArray[i].vehPlate +
                "`, this);' class='btn btn-sm btn-abmelden'><i class='fas fa-times'></i></button></div></li>";
        }

        $("#VehicleLicensingCEFBox-List").html(licenseHTML);
        $("#VehicleLicensingCEFBox").fadeTo(1000, 1, function() {});
    }

    function SetVehicleKeyCEFBoxContent(vehArray) {
        var licenseHTML = "",
            vehArray = JSON.parse(vehArray);

        for (var i in vehArray) {
            licenseHTML +=
                `<li class='list-group-item'><div class='container left'><p class='title'>FAHRZEUGNAME</p><p class='name'>${vehArray[i].vehName}</p></div>` +
                `<div class='container mid'><p class='title'>KENNZEICHEN</p><p class='name'>${vehArray[i].vehPlate}</p></div>` +
                `<div class='container right'><p class='title'></p>` +
                "<button type='button' onclick='VehicleKeyCEFAction(`remake`, " + vehArray[i].vehId + ", `" + vehArray[
                    i].vehPlate +
                "`, this);' class='btn btn-sm btn-remake'>Schlüssel Nachmachen (250$)</i></button></li>";
        }

        $("#VehicleKeyCEFBox-List").html(licenseHTML);
        $("#VehicleKeyCEFBox").fadeTo(1000, 1, function() {});
    }

    function SetVehicleSellCEFBoxContent(vehArray) {
        var licenseHTML = "",
            vehArray = JSON.parse(vehArray);

        for (var i in vehArray) {
            licenseHTML +=
                `<li class='list-group-item'><div class='container left'><p class='title'>FAHRZEUGNAME</p><p class='name'>${vehArray[i].vehName}</p></div>` +
                `<div class='container mid'><p class='title'>KENNZEICHEN</p><p class='name'>${vehArray[i].vehPlate}</p></div>` +
                `<div class='container right'><p class='title'></p>` +
                "<button type='button' onclick='VehicleSellCEFAction(`verkaufen`, " + vehArray[i].vehId +
                ", this);' class='btn btn-sm btn-sell'>Fahrzeug Verkaufen</i></button></li>";
        }

        $("#VehicleSellCEFBox-List").html(licenseHTML);
        $("#VehicleSellCEFBox").fadeTo(1000, 1, function() {});
    }

    function VehicleLicensingCEFAction(action, vehId, vehPlate, htmlElem) {
        if (vehId <= 0 || vehPlate == "" || htmlElem == undefined || vehPlate == undefined || vehId == undefined ||
            action == undefined || action == "") return;
        if (action != "anmelden" && action != "abmelden") return;
        //var inputElem = $(htmlElem).parent().find("input");
        var inputElem = document.getElementById('PlateText');
        var inputVal = $(inputElem).val().replace(/^\s+|\s+$/g, "");

        closeVehicleLicensingCEFBox();
        if (action == "anmelden") {
            if (inputVal == "" || inputVal.length <= 0) return;
            var newPlate = inputVal.replace(" ", "-");
            alt.emit("Client:VehicleLicensing:LicensingAction", action, vehId, vehPlate, newPlate.toUpperCase());
        } else if (action == "abmelden") {
            alt.emit("Client:VehicleLicensing:LicensingAction", action, vehId, vehPlate, "");
        }
    }

    function VehicleKeyCEFAction(action, vehId, vehPlate, charId, itemName, itemAmount, htmlElem) {
        if (vehId <= 0 || vehPlate == "" || vehPlate == undefined || vehId == undefined) {
            return;
        }
        alt.emit("Client:VehicleKey:KeyAction", "remake", vehId, vehPlate);
    }

    function VehicleSellCEFAction(action, vehId, charId, htmlElem) {
        if (vehId <= 0 || vehId == undefined) {
            return;
        }
        alt.emit("Client:VehicleSell:SellAction", "verkaufen", vehId);
    }

    function closeVehicleLicensingCEFBox() {
        $("#VehicleLicensingCEFBox").fadeOut(250, function() {
            alt.emit("Client:VehicleLicensing:destroyCEF");
        });
    }

    function closeVehicleKeyCEFBox() {
        $("#VehicleKeyCEFBox").fadeOut(250, function() {
            alt.emit("Client:VehicleKey:destroyCEF");
        });
    }

    function closeVehicleSellCEFBox() {
        $("#VehicleSellCEFBox").fadeOut(250, function() {
            alt.emit("Client:VehicleSell:destroyCEF");
        });
    }

    function SetGivePlayerLicenseCEFContent(targetCharId, licArray) {
        var licHTML = "",
            licArray = JSON.parse(licArray);

        for (var i in licArray) {
            if (!licArray[i].PKW) {
                licHTML +=
                    `<li class='list-group-item' onclick='GivePlayerLicenseCEFAction(${targetCharId}, "pkw");'><p>C (${licArray[i].PKWPrice}$)</p></li>`;
            }

            if (!licArray[i].LKW) {
                licHTML +=
                    `<li class='list-group-item' onclick='GivePlayerLicenseCEFAction(${targetCharId}, "lkw");'><p>B (${licArray[i].LKWPrice}$)</p></li>`;
            }

            if (!licArray[i].Bike) {
                licHTML +=
                    `<li class='list-group-item'onclick='GivePlayerLicenseCEFAction(${targetCharId}, "bike");'><p>M (${licArray[i].BikePrice}$)</p></li>`;
            }

            if (!licArray[i].Boat) {
                licHTML +=
                    `<li class='list-group-item' onclick='GivePlayerLicenseCEFAction(${targetCharId}, "boat");'><p>BL (${licArray[i].BoatPrice}$)</p></li>`;
            }

            if (!licArray[i].Fly) {
                licHTML +=
                    `<li class='list-group-item' onclick='GivePlayerLicenseCEFAction(${targetCharId}, "fly");'><p>FL (${licArray[i].FlyPrice}$)</p></li>`;
            }

            if (!licArray[i].Helicopter) {
                licHTML +=
                    `<li class='list-group-item' onclick='GivePlayerLicenseCEFAction(${targetCharId}, "helicopter");'><p>HL (${licArray[i].HelicopterPrice}$)</p></li>`;
            }

            if (!licArray[i].PassengerTransport) {
                licHTML +=
                    `<li class='list-group-item' onclick='GivePlayerLicenseCEFAction(${targetCharId}, "passengertransport");'><p>C-CDL (${licArray[i].PassengerTransportPrice}$)</p></li>`;
            }

            if (!licArray[i].weaponlicense) {
                licHTML +=
                    `<li class='list-group-item' onclick='GivePlayerLicenseCEFAction(${targetCharId}, "weaponlicense");'><p>Waffenschein (${licArray[i].weaponlicensePrice}$)</p></li>`;
            }
        }

        $("#GivePlayerLicenseCEFBox-List").html(licHTML);
        $("#GivePlayerLicenseCEFBox").fadeTo(1000, 1, function() {});
    }

    function GivePlayerLicenseCEFAction(targetCharId, licname) {
        if (targetCharId <= 0 || licname.length <= 0 || licname == "") return;
        closeGivePlayerLicenseCEFBox();
        alt.emit("Client:GivePlayerLicense:GiveLicense", targetCharId, licname);
    }

    function closeGivePlayerLicenseCEFBox() {
        $("#GivePlayerLicenseCEFBox").fadeOut(250, function() {
            alt.emit("Client:GivePlayerLicense:destroyCEF");
        });
    }

    function MinijobPilotCEFSelectJob(level) {
        if (level <= 0) return;
        closeMinijobPilotCEFBox();
        alt.emit("Client:MinijobPilot:SelectJob", level);
    }

    function closeMinijobPilotCEFBox() {
        $("#Minijob-PilotCEFBox").fadeOut(50, function() {
            alt.emit("Client:MinijobPilot:destroyCEF");
        });
    }

    function SetMinijobBusDriverListContent(routeArray) {
        var busHTML = "",
            routeArray = JSON.parse(routeArray);

        for (var i in routeArray) {
            busHTML +=
                `<div class='container' onclick='MinijobBusDriverStartRoute(${routeArray[i].routeId});'><img src='../utils/img/vehicles/${routeArray[i].hash}.png'><p class='title'>${routeArray[i].routeName}</p><p class='subtitle'>Geschätzte Fahrtdauer: <font>${routeArray[i].neededTime}</font></p>` +
                `<p class='subtitle'>Entlohnung: <font>${routeArray[i].paycheck}$</font></p></div>`;
        }
        $("#Minijob-BusDriverCEFBox-List").html(busHTML);
        $("#Minijob-BusDriverCEFBox").fadeTo(1000, 1, function() {});
    }

    function MinijobBusDriverStartRoute(routeId) {
        if (routeId <= 0) return;
        closeMinijobBusDriverCEFBox();
        alt.emit("Client:MinijobBusdriver:StartJob", routeId);
    }

    function closeMinijobBusDriverCEFBox() {
        $("#Minijob-BusDriverCEFBox").fadeOut(50, function() {
            alt.emit("Client:MinijobBusdriver:destroyCEF");
        });
    }

    function unixToLatest(unix, type) {
        unix = unix * 1000;
        let date = new Date(unix),
            today = new Date(),
            str,
            dateDay = date.getDate(),
            dateMonth = date.getMonth(),
            todayDay = today.getDate(),
            todayMonth = today.getMonth();

        if (dateDay == todayDay && dateMonth == todayMonth) {
            if (type == null) {
                unix = unix / 1000;
                str = unixToTime(unix);
            } else if (type == "day") {
                str = "Heute";
            }
        } else {
            date.setHours(0);
            date.setMinutes(0);
            date.setSeconds(0);
            today.setHours(0);
            today.setMinutes(0);
            today.setSeconds(0);
            today.setMilliseconds(0);

            let unixDate = date.getTime();
            let unixToday = today.getTime();
            let diffdays = (unixToday - unixDate) / (1000 * 60 * 60 * 24);

            if (diffdays == 1) {
                str = "Gestern";
            } else {
                dateMonth++;
                str = checkTime(dateDay) + "." + checkTime(dateMonth) + ".";
            }
        }
        return (str);
    }

    function unixToTime(unix) {
        let date = new Date(unix * 1000);
        hour = checkTime(date.getHours()),
            minute = checkTime(date.getMinutes());
        let str = hour + ":" + minute;
        return (str);
    }

    function playAnimation(animDict, animName, animFlag, animDuration) {
        if (animDict == undefined || animName == undefined || animFlag == undefined || animDuration == undefined)
            return;
        alt.emit("Client::playAnimation", animDict, animName, parseInt(animFlag), parseInt(animDuration));
    }

    //Sounds

    alt.on("CEF:Sound:PlayOnce", (path) => {
        playAudio(path);
    });

    // MONEY
    alt.on("CEF:HUD:updateMoney", (money) => {
        updateMoneyHUD(money);
    });

    alt.on("CEF:HUD:updateHealth", (health) => {
        updateHealth(health);
    });

    //Alt On Events

    alt.on("CEF:Farming:createCEF", (neededItem, producedItem, neededItemAmount, producedItemAmount, duration,
        neededItemTWO, neededItemTHREE, neededItemTWOAmount, neededItemTHREEAmount) => {
        FarmingCreateCEF(neededItem, producedItem, neededItemAmount, producedItemAmount, duration,
            neededItemTWO, neededItemTHREE, neededItemTWOAmount, neededItemTHREEAmount);
    });

    alt.on("CEF:HUD:sendNotification", (type, time, message) => {
        ShowNotification(type, message, time);
    });

    alt.on("CEF:HUD:updateStreetLocation", (loc) => {
        updateStreetLocation(loc);
    });

    alt.on("CEF:HUD:updateDesireHUD", (food, drink) => {
        updateDesireHUD(food, drink);
    });

    alt.on("CEF:HUD:updateSpeakState", (state) => {
        updateSpeakState(state);
    });

    alt.on("CEF:HUD:updateHUDVoice", (state) => {
        updateVoiceHUD(state);
    });

    alt.on("CEF:HUD:updateHUDPosInVeh", (state) => {
        SetPlayerHUDInVehicle(state);
    });

    alt.on("CEF:HUD:createIdentityCardApplyForm", (charname, gender, adress, birthdate) => {
        CreateIdentityCardApplyForm(charname, gender, adress, birthdate);
    });

    alt.on("CEF:Shop:shopCEFBoxCreateCEF", (itemArray, shopId, isOnlySelling) => {
        shopCEFBoxCreateCEF(itemArray, shopId, isOnlySelling);
    });

    alt.on("CEF:General:hideAllCEFs", (hideCursor) => {
        $("#identityCardApplyForm").fadeOut(300, 0, function() {
            $("#identityCardApplyForm").hide();
        });

        $("#shopCEFBox").fadeOut(500, function() {
            $("#shopCEFBox").hide();
        });

        $("#barberCEFBox").fadeOut(500, function() {
            $("#barberCEFBox").hide();
        });

        $("#VehicleShopCEFBox").fadeOut(500, function() {
            $("#VehicleShopCEFBox").hide();
            curVehShopId = undefined;
        });

        $("#VehicleFuelstationCEFBox").fadeOut(250, function() {
            $("#VehicleFuelstationCEFBox").hide();
        });

        globalPlayerBillType = undefined;
        globalPlayerBilltargetCharId = undefined;
        $("#GivePlayerBillCEFBox").fadeOut(250, function() {
            $("#GivePlayerBillCEFBox").hide();
        });

        globalRecievePlayerBillType = undefined;
        globalRecievePlayerBillFactionCompanyId = undefined;
        globalRecievePlayerBillAmount = undefined;
        globalRecievePlayerBillReason = undefined;
        globalRecievePlayerBillFactionName = undefined;
        globalRecievePlayerBillGivenCharacterId = undefined;
        $("#RecievePlayerBillCEFBox").fadeOut(250, function() {
            $("#RecievePlayerBillCEFBox").hide();
        });
        $("#GivePlayerLicenseCEFBox").fadeOut(250, function() {
            $("#GivePlayerLicenseCEFBox").hide();
        });
        $("#Minijob-PilotCEFBox").fadeOut(50, function() {
            $("#Minijob-PilotCEFBox").hide();
        });
        $("#Minijob-BusDriverCEFBox").fadeOut(50, function() {
            $("#Minijob-BusDriverCEFBox").hide();
        });
        globalHotelApartmentsArray = [];
        $("#HotelManageCEFBox").fadeOut(500, function() {
            $("#HotelManageCEFBox").hide();
        });
        $("#HouseEntranceCEFBox").fadeOut(500, function() {
            $("#HouseEntranceCEFBox").hide();
        });
        $("#HouseManageCEFBox").fadeOut(500, function() {
            $("#HouseManageCEFBox").hide();
        });
        $("#TownhallHouseSelectorBox").fadeOut(500, function() {
            $("#TownhallHouseSelectorBox").hide();
        });
    });

    alt.on("CEF:Barber:barberCEFBoxCreateCEF", (data, opacitydata, colordata) => {
        globalBarberHeadoverlaysData = data;
        globalBarberHeadoverlaysOpacityData = opacitydata;
        globalBarberHeadoverlaysColorData = colordata;
        globalBarberheadoverlaysarray = [globalBarberHeadoverlaysData, globalBarberHeadoverlaysOpacityData, globalBarberHeadoverlaysColorData];

        //Daten werden visuell eingetragen
        document.getElementById("BarberHairVariation").value = globalBarberHeadoverlaysData[13];
        document.getElementById("BarberHairVariationOutput").innerHTML = globalBarberHeadoverlaysData[13];
        document.getElementById("BarberHairColor1").value = globalBarberHeadoverlaysColorData[13];
        document.getElementById("BarberHairColor1Output").innerHTML = globalBarberHeadoverlaysColorData[13];
        document.getElementById("BarberHairColor2").value = globalBarberHeadoverlaysOpacityData[13];
        document.getElementById("BarberHairColor2Output").innerHTML = globalBarberHeadoverlaysOpacityData[13];
        document.getElementById("BarberEyebrowsVariation").value = globalBarberHeadoverlaysData[2];
        document.getElementById("BarberEyebrowsVariationOutput").innerHTML = globalBarberHeadoverlaysData[2];
        document.getElementById("BarberEyebrowsColor").value = globalBarberHeadoverlaysColorData[2];
        document.getElementById("BarberEyebrowsColorOutput").innerHTML = globalBarberHeadoverlaysColorData[2];
        document.getElementById("BarberBeardVariation").value = globalBarberHeadoverlaysData[1];
        document.getElementById("BarberBeardVariationOutput").innerHTML = globalBarberHeadoverlaysData[1];
        document.getElementById("BarberBeardColor").value = globalBarberHeadoverlaysColorData[1];
        document.getElementById("BarberBeardColorOutput").innerHTML = globalBarberHeadoverlaysColorData[1];
        document.getElementById("BarberMakeupVariation").value = globalBarberHeadoverlaysData[4];
        document.getElementById("BarberMakeupVariationOutput").innerHTML = globalBarberHeadoverlaysData[4];
        document.getElementById("BarberMakeupAlpha").value = globalBarberHeadoverlaysOpacityData[4];
        document.getElementById("BarberMakeupAlphaOutput").innerHTML = globalBarberHeadoverlaysOpacityData[4];
        document.getElementById("BarberLipstickVariation").value = globalBarberHeadoverlaysData[8];
        document.getElementById("BarberLipstickVariationOutput").innerHTML = globalBarberHeadoverlaysData[8];
        document.getElementById("BarberLipstickColor").value = globalBarberHeadoverlaysColorData[8];
        document.getElementById("BarberLipstickColorOutput").innerHTML = globalBarberHeadoverlaysColorData[8];
        document.getElementById("BarberWangenBlushVariation").value = globalBarberHeadoverlaysData[5];
        document.getElementById("BarberWangenBlushVariationOutput").innerHTML = globalBarberHeadoverlaysData[5];
        document.getElementById("BarberWangenBlushColor").value = globalBarberHeadoverlaysColorData[5];
        document.getElementById("BarberWangenBlushColorOutput").innerHTML = globalBarberHeadoverlaysColorData[5];
        $("#barberCEFBox").fadeTo(500, 1, function() {});
    });

    alt.on("CEF:InteractionMenu:toggleInteractionMenu", (state, type, itemArray) => {
        toggleInterActionMenu(state, type, itemArray);
    });

    alt.on("CEF:ClothesRadial:toggleInteractionMenu", (state, itemArray) => {
        toggleClothesRadialMenu(state, itemArray);
    });

    alt.on("CEF:ClothesRadial:requestAction", () => {
        alt.emit("Client:ClothesRadial:giveRequestedAction", globalAnimationMenuAction);
    });

    alt.on("CEF:InteractionMenu:requestAction", () => {
        alt.emit("Client:InteractionMenu:giveRequestedAction", globalInteractMenuType,
            globalInteractMenuAction);
    });

    alt.on("CEF:AnimationMenu:requestAction", () => {
        alt.emit("Client:AnimationMenu:giveRequestedAction", globalAnimationMenuAction);
    });

    alt.on("CEF:AnimationMenuPage2:requestAction", () => {
        alt.emit("Client:AnimationMenuPage2:giveRequestedAction", globalAnimationMenuAction);
    });

    alt.on("CEF:AnimationMenuPage3:requestAction", () => {
        alt.emit("Client:AnimationMenuPage3:giveRequestedAction", globalAnimationMenuAction);
    });

    alt.on("CEF:VehicleShop:SetListContent", (shopid, shopname, itemarray) => {
        SetVehicleShopCEFListContent(shopid, shopname, itemarray);
        curVehShopId = shopid;
    });

    alt.on("CEF:FuelStation:OpenCEF", (fuelStationId, stationName, owner, maxFuel, availableLiter, fuelArray,
        vehID) => {
        SetFuelstationListContent(fuelStationId, stationName, owner, maxFuel, availableLiter, fuelArray, vehID);
    });

    alt.on("CEF:Hotel:openCEF", (hotelname) => {
        openHotelRentCEF(hotelname);
    });

    alt.on("CEF:Hotel:setApartmentItems", (array) => {
        setHotelApartmentsItems(array);
    });

    alt.on("CEF:HouseEntrance:openCEF", (charId, houseArray, isRentedIn) => {
        openHouseEntranceCEF(charId, houseArray, isRentedIn);
    });

    alt.on("CEF:GivePlayerBill:openCEF", (type, targetCharId) => {
        globalPlayerBillType = type;
        globalPlayerBilltargetCharId = targetCharId;
        $("#GivePlayerBillCEFBox-AmountInput").val("");
        $("#GivePlayerBillCEFBox-ReasonInput").val("");
        $("#GivePlayerBillCEFBox").fadeTo(1000, 1, function() {});
    });

    alt.on("CEF:RecievePlayerBill:openCEF", (type, factionCompanyId, moneyAmount, reason, factionCompanyName,
        charId) => {
        globalRecievePlayerBillType = type;
        globalRecievePlayerBillFactionCompanyId = factionCompanyId;
        globalRecievePlayerBillAmount = moneyAmount;
        globalRecievePlayerBillReason = reason;
        globalRecievePlayerBillFactionName = factionCompanyName;
        globalRecievePlayerBillGivenCharacterId = charId;
        $("#RecievePlayerBillCEFBox-NameInput").val(`${factionCompanyName} (${factionCompanyId})`);
        $("#RecievePlayerBillCEFBox-AmountInput").val(`${moneyAmount}`);
        $("#RecievePlayerBillCEFBox-ReasonInput").val(`${reason}`);
        $("#RecievePlayerBillCEFBox").fadeTo(1000, 1, function() {});
    });

    alt.on("CEF:VehicleLicensing:openCEF", (vehArray) => {
        SetVehicleLicensingCEFBoxContent(vehArray);
    });

    alt.on("CEF:VehicleKey:openCEF", (vehArray) => {
        SetVehicleKeyCEFBoxContent(vehArray);
    });

    alt.on("CEF:VehicleSell:openCEF", (vehArray) => {
        SetVehicleSellCEFBoxContent(vehArray);
    });

    alt.on("CEF:GivePlayerLicense:SetGivePlayerLicenseCEFContent", (targetCharId, licArray) => {
        SetGivePlayerLicenseCEFContent(targetCharId, licArray);
    });

    alt.on("CEF:MinijobBusdriver:openCEF", (routeArray) => {
        SetMinijobBusDriverListContent(routeArray);
    });

    alt.on("CEF:MinijobPilot:openCEF", () => {
        $("#Minijob-PilotCEFBox").fadeTo(250, 1, function() {});
    });

    alt.on("CEF:Deathscreen:openCEF", () => {
        openDeathscreenCEF();
    });

    alt.on("CEF:Deathscreen:closeCEF", () => {
        closeDeathscreen();
    });

    alt.on("CEF:HouseManage:openCEF", (houseInfoArray, renterArray) => {
        openHouseManageCEF(houseInfoArray, renterArray);
    });

    alt.on("CEF:IdentityCard:showIdentityCard", (type, infoArray) => {
        showIdentityCard(type, infoArray);
    });
    alt.on("CEF:IdentityCard:showPoliceId", (type, infoArray) => {
        showPoliceId(type, infoArray);
    });
    alt.on("CEF:IdentityCard:showFIBId", (fib) => {
        showFibId(fib);
    });

    alt.on("CEF:Townhall:openHouseSelector", (houseArray) => {
        openTownhallHouseSelector(houseArray);
    });

    //Tattoo Shop
    alt.on("CEF:TattooShop:openShop", (shopId, ownTattoosJSON) => {
        openTattooHUD(shopId, ownTattoosJSON);
    });

    alt.on("CEF:TattooShop:sendItemsToClient", (array) => {
        SetTattooShopItems(array);
    });

    alt.on("CEF:Vehicle:showChangeOwnerHUD", (vehName) => {
        showChangeOwnerHUD(vehName);
    });