    $(document).ready(function() {
        alt.emit("Client:HUD:cefIsReady");
    });

    function isEmptyOrSpaces(str) {
        return str === null || str.match(/^ *$/) !== null;
    }

    function closeFactionStorageCEFBox() {
        $("#FactionStorage").fadeOut(250, function() {});
        alt.emit("Client:FactionStorage:destroyCEF");
    }

    function closeGangStorageCEFBox() {
        $("#GangStorage").fadeOut(250, function() {});
        alt.emit("Client:GangStorage:destroyCEF");
    }

    function SetFactionStorageCEFBoxContent(charId, factionId, type, invArray, storageArray) {
        var invHTML = "",
            storageHTML = "",
            invArray = JSON.parse(invArray),
            storageArray = JSON.parse(storageArray);

        for (var i in invArray) {
            invHTML += "<li class='list-group-item'>" +
                `<img src='../utils/img/inventory/${invArray[i].itemPicName}' onerror="if(!this.check) {this.check = true; this.src = '../utils/img/inventory/defaultErrorItem.png';}"><p>${invArray[i].itemName} (${invArray[i].itemAmount}x)</b></p><input type='number' placeholder='Anzahl' onkeypress='return event.charCode >= 48 && event.charCode <= 57'>` +
                "<button onclick='FactionStorageCEFAction(`storage`, " + factionId + ", " + charId + ", `" + type +
                "`, `" + invArray[i].itemLocation + "`, `" + invArray[i].itemName +
                "`, this);' type='button' class='btn btn-sm btn-primary'><i class='fas fa-check'></i></button></li>";
        }

        for (var i in storageArray) {
            storageHTML += "<li class='list-group-item'>" +
                `<img src='../utils/img/inventory/${storageArray[i].itemPicName}' onerror="if(!this.check) {this.check = true; this.src = '../utils/img/inventory/defaultErrorItem.png';}"><p>${storageArray[i].itemName} (${storageArray[i].amount}x)</b></p><input type='number' placeholder='Anzahl' onkeypress='return event.charCode >= 48 && event.charCode <= 57'>` +
                "<button onclick='FactionStorageCEFAction(`take`, " + factionId + ", " + charId + ", `" + type +
                "`, `" + storageArray[i].itemLocation + "`, `" + storageArray[i].itemName +
                "`, this);' type='button' class='btn btn-sm btn-primary'><i class='fas fa-check'></i></button></li>";
        }

        if (type == "faction") {
            $("#FactionBoxTitle").html(`LAGER`);
        } else if (type == "hotel") {
            $("#FactionBoxTitle").html(`LAGER`);
        } else if (type == "house") {
            $("#FactionBoxTitle").html(`LAGER`);
        }
        $("#InventoryBoxItems").html(invHTML);
        $("#FactionBoxItems").html(storageHTML);
        $("#FactionStorage").fadeTo(1000, 1, function() {});
    }

    function SetGangStorageCEFBoxContent(charId, gangId, type, invArray, storageArray) {
        var invHTML = "",
            storageHTML = "",
            invArray = JSON.parse(invArray),
            storageArray = JSON.parse(storageArray);

        for (var i in invArray) {
            invHTML += "<li class='list-group-item'>" +
                `<img src='../utils/img/inventory/${invArray[i].itemPicName}' onerror="if(!this.check) {this.check = true; this.src = '../utils/img/inventory/defaultErrorItem.png';}"><p>${invArray[i].itemName} (${invArray[i].itemAmount}x)</b></p><input type='number' placeholder='Anzahl' onkeypress='return event.charCode >= 48 && event.charCode <= 57'>` +
                "<button onclick='GangStorageCEFAction(`storage`, " + factionId + ", " + charId + ", `" + type +
                "`, `" + invArray[i].itemLocation + "`, `" + invArray[i].itemName +
                "`, this);' type='button' class='btn btn-sm btn-primary'><i class='fas fa-check'></i></button></li>";
        }

        for (var i in storageArray) {
            storageHTML += "<li class='list-group-item'>" +
                `<img src='../utils/img/inventory/${storageArray[i].itemPicName}' onerror="if(!this.check) {this.check = true; this.src = '../utils/img/inventory/defaultErrorItem.png';}"><p>${storageArray[i].itemName} (${storageArray[i].amount}x)</b></p><input type='number' placeholder='Anzahl' onkeypress='return event.charCode >= 48 && event.charCode <= 57'>` +
                "<button onclick='GangStorageCEFAction(`take`, " + factionId + ", " + charId + ", `" + type +
                "`, `" + storageArray[i].itemLocation + "`, `" + storageArray[i].itemName +
                "`, this);' type='button' class='btn btn-sm btn-primary'><i class='fas fa-check'></i></button></li>";
        }

        if (type == "gang") {
            $("#GangBoxTitle").html(`STASH`);
        }
        $("#GangInventoryBoxItems").html(invHTML);
        $("#GangBoxItems").html(storageHTML);
        $("#GangStorage").fadeTo(1000, 1, function() {});
    }

    function FactionStorageCEFAction(action, factionId, charId, type, fromContainer, itemName, htmlElem) {
        if (factionId <= 0 || charId <= 0 || type == null || type == undefined || type == "" || htmlElem == null ||
            itemName == undefined || itemName == "" || htmlElem == undefined) return;
        if (action != "take" && action != "storage") return;
        if (type != "hotel" && type != "faction" && type != "house") return;
        var inputElem = $(htmlElem).parent().find("input");
        var inputVal = $(inputElem).val();
        if (inputVal == "" || inputVal == "" || inputVal < 1) return;
        if (action == "storage" && fromContainer == undefined || action == "storage" && fromContainer == "none") return;
        alt.emit("Client:FactionStorage:FactionStorageAction", action, factionId, charId, type, itemName, inputVal,
            fromContainer);
        closeFactionStorageCEFBox();
    }

    function GangStorageCEFAction(action, gangId, charId, type, fromContainer, itemName, htmlElem) {
        if (gangId <= 0 || charId <= 0 || type == null || type == undefined || type == "" || htmlElem == null ||
            itemName == undefined || itemName == "" || htmlElem == undefined) return;
        if (action != "take" && action != "storage") return;
        if (type != "gang") return;
        var inputElem = $(htmlElem).parent().find("input");
        var inputVal = $(inputElem).val();
        if (inputVal == "" || inputVal == "" || inputVal < 1) return;
        if (action == "storage" && fromContainer == undefined || action == "storage" && fromContainer == "none") return;
        alt.emit("Client:GangStorage:GangStorageAction", action, gangId, charId, type, itemName, inputVal,
            fromContainer);
        closeGangStorageCEFBox();
    }

    function SetVehicleTrunkCEFBoxContent(charId, vehId, type, invArray, storageArray, currentweight, maxweight) {
        var invHTML = "",
            storageHTML = "",
            invArray = JSON.parse(invArray),
            storageArray = JSON.parse(storageArray);

        for (var i in invArray) {
            invHTML += "<li class='list-group-item'>" +
                `<img src='../utils/img/inventory/${invArray[i].itemPicName}' onerror="if(!this.check) {this.check = true; this.src = '../utils/img/inventory/defaultErrorItem.png';}"><p>${invArray[i].itemName} (${invArray[i].itemAmount}x)</b></p><input type='number' placeholder='Anzahl' onkeypress='return event.charCode >= 48 && event.charCode <= 57'>` +
                "<button onclick='VehicleTrunkCEFAction(`storage`, " + vehId + ", " + charId + ", `" + type +
                "`, `" + invArray[i].itemLocation + "`, `" + invArray[i].itemName +
                "`, this);' type='button' class='btn btn-sm btn-primary'><i class='fas fa-check'></i></button></li>";
        }

        for (var i in storageArray) {
            storageHTML += "<li class='list-group-item'>" +
                `<img src='../utils/img/inventory/${storageArray[i].itemPicName}' onerror="if(!this.check) {this.check = true; this.src = '../utils/img/inventory/defaultErrorItem.png';}"><p>${storageArray[i].itemName} (${storageArray[i].amount}x)</b></p><input type='number' placeholder='Anzahl' onkeypress='return event.charCode >= 48 && event.charCode <= 57'>` +
                "<button onclick='VehicleTrunkCEFAction(`take`, " + vehId + ", " + charId + ", `" + type +
                "`, `" + storageArray[i].itemLocation + "`, `" + storageArray[i].itemName +
                "`, this);' type='button' class='btn btn-sm btn-primary'><i class='fas fa-check'></i></button></li>";
        }

        var title, subtitle = "FAHRZEUG";
        if (type == "trunk") {
            title = "KOFFERRAUM";
            subtitle = `KOFFERRAUM (${currentweight.toFixed(2)}/${maxweight}kg)`
        } else if (type == "glovebox") {
            title = "HANDSCHUHFACH";
            subtitle = `HANDSCHUHFACH (${currentweight}/${maxweight} kg)`
        }

        $("#VehicleTrunkCEFBox-SubTitle").html(subtitle);
        $("#VehicleTrunkBoxTitle").html(title);
        $("#VehicleInventoryBoxItems").html(invHTML);
        $("#VehicleTrunkBoxItems").html(storageHTML);
        $("#VehicleTrunkCEFBox").fadeTo(1000, 1, function() {});
    }

    function VehicleTrunkCEFAction(action, vehId, charId, fromContainer, itemName, type, htmlElem) {
        if (vehId <= 0 || charId <= 0 || htmlElem == null || htmlElem == undefined || itemName == undefined ||
            itemName == "" || fromContainer == "" || type == undefined || type == null) return;
        if (action != "take" && action != "storage") return;
        if (type != "trunk" && type != "glovebox") return;
        var inputElem = $(htmlElem).parent().find("input");
        var inputVal = $(inputElem).val();
        if (inputVal == "" || inputVal < 1) return;
        if (action == "storage" && fromContainer == undefined || action == "storage" && fromContainer == "none") return;
        alt.emit("Client:VehicleTrunk:VehicleTrunkAction", action, vehId, charId, itemName, inputVal, fromContainer,
            type);
        closeVehicleTrunkCEFBox();
    }

    function closeVehicleTrunkCEFBox() {
        $("#VehicleTrunkCEFBox").fadeOut(250, function() {});
        alt.emit("Client:VehicleTrunk:destroyCEF");
    }

    function SetPlayerSearchInventoryCEFBoxContent(targetCharId, invArray) {
        var searchHTML = "",
            invArray = JSON.parse(invArray);

        for (var i in invArray) {
            searchHTML +=
                `<li class='list-group-item'><img src='../utils/img/inventory/${invArray[i].itemPicName}'><p class='name'><b>${invArray[i].itemName} (${invArray[i].itemAmount}x)</b></p><input type='number' placeholder='Anzahl' onkeypress='return event.charCode >= 48 && event.charCode <= 57' value='1'>` +
                "<button onclick='PlayerSearchCEFAction(" + targetCharId + ", `" + invArray[i].itemName + "`, `" +
                invArray[i].itemLocation +
                "`, this);' type='button' class='btn btn-sm btn-primary'><i class='fas fa-times'></i></button></li>";
        }

        $("#PlayerSearchInventoryCEFBox-List").html(searchHTML);
        $("#PlayerSearchInventoryCEFBox").fadeTo(1000, 1, function() {});
    }

    function PlayerSearchCEFAction(targetCharId, itemName, itemLocation, htmlElem) {
        if (targetCharId <= 0 || itemName == undefined || itemName == "" || htmlElem == null || htmlElem == undefined ||
            itemLocation == undefined || itemLocation == "") return;
        var inputElem = $(htmlElem).parent().find("input");
        var inputVal = $(inputElem).val();
        if (inputVal == "" || inputVal < 1) return;
        closePlayerSearchCEFBox();
        alt.emit("Client:PlayerSearch:TakeItem", targetCharId, itemName, itemLocation, inputVal);
    }

    function closePlayerSearchCEFBox() {
        $("#PlayerSearchInventoryCEFBox").fadeOut(250, function() {
            alt.emit("Client:PlayerSearch:destroyCEF");
        });
    }

    alt.on("CEF:FactionStorage:openCEF", (charId, factionId, type, invArray, storageArray) => {
        SetFactionStorageCEFBoxContent(charId, factionId, type, invArray, storageArray);
    });

    alt.on("CEF:GangStorage:openCEF", (charId, gangId, type, invArray, storageArray) => {
        SetGangStorageCEFBoxContent(charId, gangId, type, invArray, storageArray);
    });

    alt.on("CEF:VehicleTrunk:openCEF", (charId, vehID, type, invArray, storageArray, currentweight, maxweight) => {
        SetVehicleTrunkCEFBoxContent(charId, vehID, type, invArray, storageArray, currentweight, maxweight);
    });

    alt.on("CEF:PlayerSearch:openCEF", (targetCharId, invArray) => {
        SetPlayerSearchInventoryCEFBoxContent(targetCharId, invArray);
    });