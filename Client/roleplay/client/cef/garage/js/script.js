    var curGarageId = undefined;

    $(document).ready(function() {
        alt.emit("Client:HUD:cefIsReady");
        setTimeout(function() {
            $("#drinkbox").fadeTo(1000, 1, function() {});
            //$("#voicebox").css("background", "#36b1468a");
            $("#voicebox").fadeTo(1000, 1, function() {});
            $(".money").fadeTo(500, 1, function() {});
        }, 1000);
    });

    function SetGarageCEFListContent(garagename, garageInArray, garageOutArray) {
        var garageHTML = "",
            garageInArray = JSON.parse(garageInArray),
            garageOutArray = JSON.parse(garageOutArray);

        for (var i in garageInArray) {
            garageHTML += "<li class='list-group-item green'><button class='parkin' onclick='GarageCEFBoxDoAction(`storage`, `" + garageInArray[i].vehid + "`);' style='color:green;'>EINPARKEN <i class='fas fa-chevron-left'></i></button>" + `<span class='vehname'>Fahrzeug: ${garageInArray[i].name}</span><span class='vehtank'>Tank: ${garageInArray[i].tank}</span><span class='vehkz'>Kennzeichen: ${garageInArray[i].plate}</span></li><hr>`;
        }

        for (var i in garageOutArray) {
            garageHTML += "<li class='list-group-item red'><button class='parkout' onclick='GarageCEFBoxDoAction(`take`, `" + garageOutArray[i].vehid + "`);' style='color:red;'>AUSPARKEN <i class='fas fa-chevron-right'></i></button>" + `<span class='vehname'>Fahrzeug: ${garageOutArray[i].name}</span><span class='vehtank'>Tank: ${garageOutArray[i].tank}</span><span class='vehkz'>Kennzeichen: ${garageOutArray[i].plate}</span></li><hr>`;
        }

        $("#GarageCEFBOX-title").html(`${garagename}`);
        $("#GarageCEFBox-VehList").html(garageHTML);
        $("#GarageCEFBox").fadeTo(1000, 1, function() {});
    }

    function GarageCEFBoxDoAction(action, vehID) {
        //action storage = einparken, take = ausparken
        if (action == "" || vehID == 0) return;
        alt.emit("Client:Garage:DoAction", curGarageId, action, vehID);
        GarageCEFBoxdestroy();
    }

    function GarageCEFBoxdestroy() {
        $("#GarageCEFBox").fadeOut(1, function() {
            $("#GarageCEFBox").hide();
            curGarageId = undefined;
        });
        alt.emit("Client:Garage:destroyGarageCEF");
    }

    alt.on("CEF:General:hideAllCEFs", (hideCursor) => {
        $("#GarageCEFBox").fadeOut(1, function() {
            $("#GarageCEFBox").hide();
            curGarageId = undefined;
        });
    });

    alt.on("CEF:Garage:OpenGarage", (garageId, garagename, garageInArray, garageOutArray) => {
        curGarageId = garageId;
        SetGarageCEFListContent(garagename, garageInArray, garageOutArray);
    });