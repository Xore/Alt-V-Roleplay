var isPersoIdentityCardInUse = false,
    isDriversLicInUse = false,
    isWepLicInUse = false,
    isFactionCardInUse = false,
    isPoliceCardInUse = false,
    isFIBCardInUse = false,
    isLSMDCardInUse = false,
    isVehRegInUse = false;

$(document).ready(function() {
    alt.emit("Client:HUD:cefIsReady");

});

function showIdentityCard(type, infoArray) {
    if (isPersoIdentityCardInUse || isPersoIdentityCardInUse || infoArray == "[]" || infoArray == undefined) return;
    if (type != "perso") return;
    if (type == "perso") isPersoIdentityCardInUse = true;


    var persoHTML = "",
        infoArray = JSON.parse(infoArray),
        colorCode = "#fff";

    for (var i in infoArray) {
        if (type == "perso") {
            $("#UserIdCard-name").html(`${infoArray[i].charname}`);
            $("#UserIdCard-address").html(`${infoArray[i].address}`);
            $("#UserIdCard-birthdate").html(`${infoArray[i].birthdate}`);
            $("#UserIdCard-citizenid").html(`${infoArray[i].citizenid}`);
            $("#UserIdCard-signature").html(`${infoArray[i].charname}`);
            $("#UserIdCard-joinInfo").html(`Los Santos`);

            $(function() {
                $("#UserIdCard").show();
                setTimeout(() => {
                    $("#UserIdCard").hide(1000);
                    isPersoIdentityCardInUse = false;
                }, 10000);
            });
        }
    }
}

function showDriversLic(type, infoArray) {
    if (isDriversLicInUse || isDriversLicInUse || infoArray == "[]" || infoArray == undefined) return;
    if (type != "drivers") return;
    if (type == "drivers") isDriversLicInUse = true;


    var driversHTML = "",
        infoArray = JSON.parse(infoArray),
        colorCode = "#fff";

    for (var i in infoArray) {
        if (type == "drivers") {
            $("#DriverLic-name").html(`${infoArray[i].charname}`);
            $("#DriverLic-birthdate").html(`${infoArray[i].birthdate}`);
            $("#DriverLic-signature").html(`${infoArray[i].charname}`);
            $("#DriverLic-pkwName").html(`${infoArray[i].pkwName}`);
            $("#DriverLic-lkwName").html(`${infoArray[i].lkwName}`);
            $("#DriverLic-bikeName").html(`${infoArray[i].bikeName}`);
            $("#DriverLic-boatName").html(`${infoArray[i].boatName}`);
            $("#DriverLic-flyName").html(`${infoArray[i].flyName}`);
            $("#DriverLic-heliName").html(`${infoArray[i].heliName}`);
            $("#DriverLic-passengerTransportName").html(`${infoArray[i].passengerTransportName}`);


            $(function() {
                $("#DriverLic").show();
                setTimeout(() => {
                    $("#DriverLic").hide(1000);
                    isDriversLicInUse = false;
                }, 10000);
            });
        }
    }
}

function showWepLic(type, infoArray) {
    if (isWepLicInUse || isWepLicInUse || infoArray == "[]" || infoArray == undefined) return;
    if (type != "wep") return;
    if (type == "wep") isWepLicInUse = true;


    var wepHTML = "",
        infoArray = JSON.parse(infoArray),
        colorCode = "#fff";

    for (var i in infoArray) {
        if (type == "wep") {
            $("#WeaponsLic-name").html(`${infoArray[i].charname}`);
            $("#WeaponsLic-birthdate").html(`${infoArray[i].birthdate}`);
            $("#WeaponsLic-signature").html(`${infoArray[i].charname}`);

            $(function() {
                $("#WeaponsLic").show();
                setTimeout(() => {
                    $("#WeaponsLic").hide(1000);
                    isWepLicInUse = false;
                }, 10000);
            });
        }
    }
}

function showFactionCard(type, infoArray) {
    if (isFactionCardInUse || isFactionCardInUse || infoArray == "[]" || infoArray == undefined) return;
    if (type != "faction") return;
    if (type == "faction") isFactionCardInUse = true;


    var factionHTML = "",
        infoArray = JSON.parse(infoArray),
        colorCode = "#fff";

    for (var i in infoArray) {
        if (type == "faction") {
            $("#FactionCard-factionname").html(`${infoArray[i].factionname}`);
            $("#FactionCard-name").html(`${infoArray[i].charname}`);
            $("#FactionCard-rankname").html(`${infoArray[i].rankname}`);
            $("#FactionCard-serviceNumber").html(`${infoArray[i].serviceNumber}`);
            $("#FactionCard-citizenid").html(`${infoArray[i].citizenid}`);
            $("#FactionCard-signature").html(`${infoArray[i].charname}`);

            $(function() {
                $("#FactionCard").show();
                setTimeout(() => {
                    $("#FactionCard").hide(1000);
                    isFactionCardInUse = false;
                }, 10000);
            });
        }
    }
}

function showPoliceCard(type, infoArray) {
    if (isPoliceCardInUse || isPoliceCardInUse || infoArray == "[]" || infoArray == undefined) return;
    if (type != "police") return;
    if (type == "police") isPoliceCardInUse = true;


    var policeHTML = "",
        infoArray = JSON.parse(infoArray),
        colorCode = "#fff";

    for (var i in infoArray) {
        if (type == "police") {
            $("#PoliceCard-name").html(`${infoArray[i].charname}`);
            $("#PoliceCard-rankname").html(`${infoArray[i].rankname}`);
            $("#PoliceCard-serviceNumber").html(`${infoArray[i].serviceNumber}`);
            $("#PoliceCard-citizenid").html(`${infoArray[i].citizenid}`);
            $("#PoliceCard-signature").html(`${infoArray[i].charname}`);

            $(function() {
                $("#PoliceCard").show();
                setTimeout(() => {
                    $("#PoliceCard").hide(1000);
                    isPoliceCardInUse = false;
                }, 10000);
            });
        }
    }
}

function showFIBCard(type, infoArray) {
    if (isFIBCardInUse || isFIBCardInUse || infoArray == "[]" || infoArray == undefined) return;
    if (type != "fib") return;
    if (type == "fib") isFIBCardInUse = true;


    var fibHTML = "",
        infoArray = JSON.parse(infoArray),
        colorCode = "#fff";

    for (var i in infoArray) {
        if (type == "fib") {
            $("#FIBCard-name").html(`${infoArray[i].charname}`);
            $("#FIBCard-rankname").html(`${infoArray[i].rankname}`);
            $("#FIBCard-serviceNumber").html(`${infoArray[i].serviceNumber}`);
            $("#FIBCard-citizenid").html(`${infoArray[i].citizenid}`);
            $("#FIBCard-signature").html(`${infoArray[i].charname}`);

            $(function() {
                $("#FIBCard").show();
                setTimeout(() => {
                    $("#FIBCard").hide(1000);
                    isFIBCardInUse = false;
                }, 10000);
            });
        }
    }
}

function showLSMDCard(type, infoArray) {
    if (isLSMDCardInUse || isLSMDCardInUse || infoArray == "[]" || infoArray == undefined) return;
    if (type != "lsmd") return;
    if (type == "lsmd") isLSMDCardInUse = true;


    var lsmdHTML = "",
        infoArray = JSON.parse(infoArray),
        colorCode = "#fff";

    for (var i in infoArray) {
        if (type == "lsmd") {
            $("#LSMDCard-name").html(`${infoArray[i].charname}`);
            $("#LSMDCard-rankname").html(`${infoArray[i].rankname}`);
            $("#LSMDCard-serviceNumber").html(`${infoArray[i].serviceNumber}`);
            $("#LSMDCard-citizenid").html(`${infoArray[i].citizenid}`);
            $("#LSMDCard-signature").html(`${infoArray[i].charname}`);

            $(function() {
                $("#LSMDCard").show();
                setTimeout(() => {
                    $("#LSMDCard").hide(1000);
                    isLSMDCardInUse = false;
                }, 10000);
            });
        }
    }
}

function showVehReg(type, infoArray) {
    if (isVehRegInUse || isVehRegInUse || infoArray == "[]" || infoArray == undefined) return;
    if (type != "veh") return;
    if (type == "veh") isLSMDCardInUse = true;


    var lsmdHTML = "",
        infoArray = JSON.parse(infoArray),
        colorCode = "#fff";

    for (var i in infoArray) {
        if (type == "veh") {
            $("#vehReg-name").html(`${infoArray[i].ownerName}`);
            $("#vehReg-signature").html(`${infoArray[i].ownerName}`);
            $("#vehReg-vehName").html(`${infoArray[i].vehname}`);
            $("#vehReg-manufactor").html(`${infoArray[i].manufactor}`);
            $("#vehReg-serial").html(`${infoArray[i].serialNumber}`);
            $("#vehReg-plate").html(`${infoArray[i].plate}`);
            $("#vehReg-buyDate").html(`${infoArray[i].buyDate}`);

            $(function() {
                $("#vehReg").show();
                setTimeout(() => {
                    $("#vehReg").hide(1000);
                    isVehRegInUse = false;
                }, 10000);
            });
        }
    }
}

alt.on("CEF:IdentityCard:showIdentityCard", (type, infoArray) => {
    showIdentityCard(type, infoArray);
});
alt.on("CEF:IdentityCard:showDriversLic", (type, infoArray) => {
    showDriversLic(type, infoArray);
});
alt.on("CEF:IdentityCard:showWepLic", (type, infoArray) => {
    showWepLic(type, infoArray);
});
alt.on("CEF:IdentityCard:showFactionCard", (type, infoArray) => {
    showFactionCard(type, infoArray);
});
alt.on("CEF:IdentityCard:showPoliceCard", (type, infoArray) => {
    showPoliceCard(type, infoArray);
});
alt.on("CEF:IdentityCard:showFIBCard", (type, infoArray) => {
    showFIBCard(type, infoArray);
});
alt.on("CEF:IdentityCard:showLSMDCard", (type, infoArray) => {
    showLSMDCard(type, infoArray);
});