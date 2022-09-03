    $(document).ready(function() {
        alt.emit("Client:HUD:cefIsReady");
    });

    // Dynasty8
    function openDynasty8HUD(type, myItems, freeItems) {
        let myHtml = "",
            freeHtml = "",
            count = 0;
        myItems = JSON.parse(myItems);
        freeItems = JSON.parse(freeItems);
        if (type == "shops") {
            for (var i in freeItems) {
                freeHtml += "<li class='list-group-item'><p class='title'>Shop " + freeItems[i].id + "<span onclick='dynastyBuyShop(" + freeItems[i].id + ");'><i class='fas fa-dollar-sign'></i></span><span onclick='locatePosition(" + freeItems[i].pos.X + ", " + freeItems[i].pos.Y + ");'><i class='fas fa-map'></i></span></p>" +
                    "<div class='container'>";
                switch (freeItems[i].name) {
                    case "24-7 Supermarket":
                        freeHtml += "<img class='mainPic' src='../utils/img/dynasty8/247shop1.png'><img src='../utils/img/dynasty8/247shop1.png' onclick='dynasty8HPic(this, `247shop1.png`);'><img src='../utils/img/dynasty8/247shop2.png' onclick='dynasty8HPic(this, `247shop2.png`);'><img src='../utils/img/dynasty8/247shop3.png' onclick='dynasty8HPic(this, `247shop3.png`);'><img src='../utils/img/dynasty8/247shop4.png' onclick='dynasty8HPic(this, `247shop4.png`);'>";
                        break;
                    case "Robs Liquor":
                        freeHtml += "<img class='mainPic' src='../utils/img/dynasty8/rbshop1.png'><img src='../utils/img/dynasty8/rbshop1.png' onclick='dynasty8HPic(this, `rbshop1.png`);'><img src='../utils/img/dynasty8/rbshop2.png' onclick='dynasty8HPic(this, `rbshop2.png`);'><img src='../utils/img/dynasty8/rbshop3.png' onclick='dynasty8HPic(this, `rbshop3.png`);'><img src='../utils/img/dynasty8/rbshop4.png' onclick='dynasty8HPic(this, `rbshop4.png`);'>";
                        break;
                    case "Limited LTD Gasoline":
                        freeHtml += "<img class='mainPic' src='../utils/img/dynasty8/ltdshop1.png'><img src='../utils/img/dynasty8/ltdshop1.png' onclick='dynasty8HPic(this, `ltdshop1.png`);'><img src='../utils/img/dynasty8/ltdshop2.png' onclick='dynasty8HPic(this, `ltdshop2.png`);'><img src='../utils/img/dynasty8/ltdshop3.png' onclick='dynasty8HPic(this, `ltdshop3.png`);'><img src='../utils/img/dynasty8/ltdshop4.png' onclick='dynasty8HPic(this, `ltdshop4.png`);'>";
                        break;
                }
                freeHtml += "</div><br><p class='desc'>Umsatz: <font>" + freeItems[i].bank + "$</font></p><p class='desc'>Preis: <font>" + freeItems[i].price + "$</font></p></li>";
            }

            for (var i in myItems) {
                myHtml += "<li class='list-group-item'><p class='title'>Shop " + myItems[i].id + "<span onclick='dynastySellShop(" + myItems[i].id + ");'><i class='fas fa-dollar-sign'></i></span><span onclick='locatePosition(" + myItems[i].pos.X + ", " + myItems[i].pos.Y + ");'><i class='fas fa-map'></i></span></p><div class='container'>";
                switch (myItems[i].name) {
                    case "24-7 Supermarket":
                        myHtml += "<img class='mainPic' src='../utils/img/dynasty8/247shop4.png'>";
                        break;
                    case "Robs Liquor":
                        myHtml += "<img class='mainPic' src='../utils/img/dynasty8/rbshop1.png'>";
                        break;
                    case "Limited LTD Gasoline":
                        myHtml += "<img class='mainPic' src='../utils/img/dynasty8/ltdshop1.png'>";
                        break;
                }
                myHtml += "</div><p class='desc'>Umsatz: <font>" + myItems[i].bank + "$</font></p><p class='desc'>Verkaufspreis: <font>" + myItems[i].price / 2 + "$</font></p></li>";
            }

            $("#dynasty8HUD-Shops-List").html(freeHtml);
            $("#dynasty8HUD-Shops-MyList").html(myHtml);
            $("#dynasty8HUD-Shops").css("display", "flex");
            $("#dynasty8HUD-Shops").fadeTo(250, 1, function() {
                $("#dynasty8HUD-Shops").show();
            });
        } else if (type == "storages") {
            for (var i in freeItems) {
                freeHtml += "<li class='list-group-item'><p class='title'>Lagerhalle " + freeItems[i].id + "<span onclick='dynastyBuyStorage(" + freeItems[i].id + ");'><i class='fas fa-dollar-sign'></i></span><span onclick='locatePosition(" + freeItems[i].pos.X + ", " + freeItems[i].pos.Y + ");'><i class='fas fa-map'></i></span></p>" +
                    "<div class='container'>" +
                    "<img class='mainPic' src='../utils/img/dynasty8/storage1.png'><img src='../utils/img/dynasty8/storage1.png' onclick='dynasty8HPic(this, `storage1.png`);'><img src='../utils/img/dynasty8/storage2.png' onclick='dynasty8HPic(this, `storage2.png`);'><img src='../utils/img/dynasty8/storage3.png' onclick='dynasty8HPic(this, `storage3.png`);'><img src='../utils/img/dynasty8/storage4.png' onclick='dynasty8HPic(this, `storage4.png`);'>";
                freeHtml += "</div><br><p class='desc'>Lagerkapazität: <font>" + freeItems[i].maxSize + "kg</font></p><p class='desc'>Preis: <font>" + freeItems[i].price + "$</font></p></li>";
            }

            for (var i in myItems) {
                myHtml += "<li class='list-group-item'><p class='title'>Lagerhalle " + myItems[i].id + "<span onclick='dynastySellStorage(" + myItems[i].id + ");'><i class='fas fa-dollar-sign'></i></span><span onclick='locatePosition(" + myItems[i].pos.X + ", " + myItems[i].pos.Y + ");'><i class='fas fa-map'></i></span></p><div class='container'>" +
                    "<img class='mainPic' src='../utils/img/dynasty8/storage1.png'>";
                myHtml += "</div><p class='desc'>Lagerkapazität: <font>" + myItems[i].maxSize + "kg</font></p><p class='desc'>Verkaufspreis: <font>" + myItems[i].price / 2 + "$</font></p></li>";
            }

            $("#dynasty8HUD-Storages-List").html(freeHtml);
            $("#dynasty8HUD-Storages-MyList").html(myHtml);
            $("#dynasty8HUD-Storages").css("display", "flex");
            $("#dynasty8HUD-Storages").fadeTo(250, 1, function() {
                $("#dynasty8HUD-Storages").show();
            });
        }
    }

    function dynastyBuyShop(shopId) {
        if (shopId == undefined || shopId <= 0) return;
        alt.emit("Client:Dynasty:buyShop", shopId);
        closeDynasty8Cef('shops');
    }

    function dynastySellShop(shopId) {
        if (shopId == undefined || shopId <= 0) return;
        alt.emit("Client:Dynasty:sellShop", shopId);
        closeDynasty8Cef('shops');
    }

    function dynastyBuyStorage(storageId) {
        if (storageId == undefined || storageId <= 0) return;
        alt.emit("Client:Dynasty:buyStorage", storageId);
        closeDynasty8Cef('storages');
    }

    function dynastySellStorage(storageId) {
        if (storageId == undefined || storageId <= 0) return;
        alt.emit("Client:Dynasty:sellStorage", storageId);
        closeDynasty8Cef('storages');
    }

    function locatePosition(x, y) {
        alt.emit("Client:Utilities:locatePos", x, y);
    }

    function dynasty8HPic(htmlElem, newPic) {
        var parent = $(htmlElem).parent();
        var snew = $(parent).find("img.mainPic");
        $(snew).attr("src", `../utils/img/dynasty8/${newPic}`);
    }

    function closeDynasty8Cef(type) {
        alt.emit("Client:Dynasty8:destroy");
        switch (type) {
            case "shops":
                $("#dynasty8HUD-Shops").fadeTo(250, 0, function() {
                    $("#dynasty8HUD-Shops").css("display", "none");
                    $("#dynasty8HUD-Shops").hide();
                });
                break;
            case "storages":
                $("#dynasty8HUD-Storages").fadeTo(250, 0, function() {
                    $("#dynasty8HUD-Storages").css("display", "none");
                    $("#dynasty8HUD-Storages").hide();
                });
                break;
        }
    }

    //Alt On Events
    if ('alt' in window) {
        // Dynasty 8
        alt.on("CEF:Dynasty8:openDynasty8HUD", (type, myItems, freeItems) => {
            openDynasty8HUD(type, myItems, freeItems);
        });
    }