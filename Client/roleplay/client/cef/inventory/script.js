    var hasPlayerBackpack = false,
        lastselectedUIDName = undefined,
        lastselectedPlace = undefined,
        lastselectedPlayerID = 0,
        //curActiveSite = "inventory",
        curActiveSite = undefined,
        canDoItemInteract = true;

    $(document).ready(function() {
        setTimeout(() => {
            alt.emit("Client:Inventory:cefIsReady");
        }, 100);
    });

    function setInventoryActive() {
        curActiveSite = "inventory";
        return curActiveSite;
    }

    function setBackpackActive() {
        curActiveSite = "backpack";
        return curActiveSite;
    }

    function setWalletActive() {
        curActiveSite = "brieftasche";
        return curActiveSite;
    }

    function setKeysActive() {
        curActiveSite = "schluessel";
        return curActiveSite;
    }

    function openContextMenus(target) {
        isItemUseable = $(target).attr('data-isuseable');
        isItemDroppable = $(target).attr('data-isdroppable');
        isItemGiveable = $(target).attr('data-isgiveable');
        lastselectedPlace = $(target).attr('data-place');
        lastselectedUIDName = $(target).attr('data-uidname');

        if (isItemGiveable == "true" && lastselectedPlayerID != 0) {
            $("#customMenuGiveItem").show();
            $("#customMenuGiveItem").attr("onClick", "DoSomeItemAction(`giveitem`, `" + $(target).attr('data-place') + "`, `" + $(target).attr('data-uidname') + "`);");
        } else {
            $("#customMenuGiveItem").hide();
            $("#customMenuGiveItem").attr("onClick", "");
        }

        if (isItemUseable == "true") {
            $("#customMenuUseItem").show();
            $("#customMenuUseItem").attr("onClick", "DoSomeItemAction(`use`, `" + $(target).attr('data-place') + "`, `" + $(target).attr('data-uidname') + "`);");
        } else {
            $("#customMenuUseItem").hide();
            $("#customMenuUseItem").attr("onClick", "");
        }

        if (isItemDroppable == "true") {
            $("#customMenuDropItem").show();
            $("#customMenuDropItem").attr("onClick", "DoSomeItemAction(`drop`, `" + $(target).attr('data-place') + "`, `" + $(target).attr('data-uidname') + "`);");
        } else {
            $("#customMenuDropItem").hide();
            $("#customMenuDropItem").attr("onClick", "");
        }

        if (curActiveSite == "inventory") {
            $("#customMenuSwitchBagItem-Text").html("In den Rucksack");
            $("#customMenuSwitchBagItem").attr("onClick", "DoSomeItemAction(`switchToBackpack`, `" + $(target).attr('data-place') + "`, `" + $(target).attr('data-uidname') + "`);");
        } else if (curActiveSite == "backpack") {
            $("#customMenuSwitchBagItem-Text").html("Ins Inventar");
            $("#customMenuSwitchBagItem").attr("onClick", "DoSomeItemAction(`switchToInventory`, `" + $(target).attr('data-place') + "`, `" + $(target).attr('data-uidname') + "`);");
        }
        if (curActiveSite == "brieftasche") {
            $("customMenuSwitchBagItem").hide();
        }
        if (curActiveSite == "schluessel") {
            $("#customMenuSwitchBagItem").hide();
        }

        if (hasPlayerBackpack == true) {
            $("#customMenuSwitchBagItem").show();
        } else if (hasPlayerBackpack == false) {
            $("#customMenuSwitchBagItem").hide();
        }

        $(".custom-menu").finish().show(100).css({
            //top: event.pageY + "px",
            //left: event.pageX + "px"
            opacity: 1,
        });
    }

    function changeSite(site) {
        lastselectedPlace = undefined;
        lastselectedUIDName = undefined;
        $(".custom-menu").hide();
        $("#customMenuUseItem").hide();
        $("#customMenuUseItem").attr("onClick", "");
        $("#customMenuDropItem").hide();
        $("#customMenuDropItem").attr("onClick", "");
        $("#customMenuSwitchBagItem").hide();
        $(`#${curActiveSite}SiteTitle`).removeClass("active");
        $(`#schluesselSiteItemList`).fadeOut(400);
        $(`#brieftascheSiteItemList`).fadeOut(400);

        $(`#${site}SiteTitle`).addClass("active");
        setTimeout(() => {
            $(`#${site}SiteItemList`).fadeIn(400);
        }, 400);
        curActiveSite = site;
    }


    function SetInventoryInformations(invArray, backpackSize) {
        invArray = JSON.parse(invArray);
        let inventoryWeight = 0.0,
            backpackWeight = 0.0,
            brieftascheWeight = 0.0,
            schluesselWeight = 0.0,
            invHTML = "",
            backpackHTML = "";
        brieftascheHTML = "";
        schluesselHTML = "";
        for (var i in invArray) {
            let displayName = invArray[i].itemName;
            if (invArray[i].itemLocation == "inventory") {
                invHTML += "<li class='list-group-item invitem' draggable='true' data-uidname='" + invArray[i].itemName + "' data-isgiveable='" + invArray[i].isItemGiveable + "' data-isuseable='" + invArray[i].isItemUseable + "' data-isdroppable='" + invArray[i].isItemDroppable + "' data-place='inventory' onclick='openContextMenus(this);setInventoryActive();'>" +
                    `<img src='../utils/img/inventory/${invArray[i].itemPicName}' onerror="if(!this.check) {this.check = true; this.src = '../utils/img/inventory/defaultErrorItem.png';}"><p>${displayName} (${invArray[i].itemAmount}x)</p>`;
                invHTML += "</li>";

                inventoryWeight += (invArray[i].itemWeight * invArray[i].itemAmount);
            } else if (invArray[i].itemLocation == "backpack") {
                backpackHTML += "<li class='list-group-item invitem' draggable='true' data-uidname='" + invArray[i].itemName + "' data-isgiveable='" + invArray[i].isItemGiveable + "' data-isuseable='" + invArray[i].isItemUseable + "' data-isdroppable='" + invArray[i].isItemDroppable + "' data-place='backpack' onclick='openContextMenus(this);setBackpackActive();'>" +
                    `<img src='../utils/img/inventory/${invArray[i].itemPicName}' onerror="if(!this.check) {this.check = true; this.src = '../utils/img/inventory/defaultErrorItem.png';}"><p>${displayName} (${invArray[i].itemAmount}x)</p>`;
                backpackHTML += "</li>";
                backpackWeight += (invArray[i].itemWeight * invArray[i].itemAmount);
            } else if (invArray[i].itemLocation == "brieftasche") {
                brieftascheHTML += "<li class='list-group-item invitem' draggable='true' data-uidname='" + invArray[i].itemName + "' data-isgiveable='" + invArray[i].isItemGiveable + "' data-isuseable='" + invArray[i].isItemUseable + "' data-isdroppable='" + invArray[i].isItemDroppable + "' data-place='brieftasche' onclick='openContextMenus(this);setWalletActive();'>" +
                    `<img src='../utils/img/inventory/${invArray[i].itemPicName}' onerror="if(!this.check) {this.check = true; this.src = '../utils/img/inventory/defaultErrorItem.png';}"><p>${displayName} (${invArray[i].itemAmount}x)</p>`;
                brieftascheHTML += "</li>";
                brieftascheWeight += (invArray[i].itemWeight * invArray[i].itemAmount);
            } else if (invArray[i].itemLocation == "schluessel") {
                schluesselHTML += "<li class='list-group-item invitem' draggable='true' data-uidname='" + invArray[i].itemName + "' data-isgiveable='" + invArray[i].isItemGiveable + "' data-isuseable='" + invArray[i].isItemUseable + "' data-isdroppable='" + invArray[i].isItemDroppable + "' data-place='schluessel' onclick='openContextMenus(this);setKeysActive();'>" +
                    `<img src='../utils/img/inventory/${invArray[i].itemPicName}' onerror="if(!this.check) {this.check = true; this.src = '../utils/img/inventory/defaultErrorItem.png';}"><p>${displayName} (${invArray[i].itemAmount}x)</p>`;
                schluesselHTML += "</li>";
                schluesselWeight += (invArray[i].itemWeight * invArray[i].itemAmount);
            }
        }

        if (backpackSize > 0) {
            hasPlayerBackpack = true;
            $("#backpackHeadContainer").attr('onClick', 'changeSite(`backpack`);');
            $("#backpackSiteTitle").html(`Rucksack (${backpackWeight.toFixed(2)}/${backpackSize}kg)`);
        } else if (backpackSize <= 0) {
            hasPlayerBackpack = false;
            $("#backpackHeadContainer").attr('onClick', '');
            $("#backpackSiteTitle").html("Kein Rucksack");
        }

        $("#inventoryMaxWeight").html("5");
        $("#inventoryWeight").html(inventoryWeight.toFixed(2));
        $("#backpackMaxWeight").html(backpackSize);
        $("#inventorySiteItemList").html(invHTML);
        $("#backpackSiteItemList").html(backpackHTML);
        $("#brieftascheSiteItemList").html(brieftascheHTML);
        $("#schluesselSiteItemList").html(schluesselHTML);
        $('#inventorySiteItemList, #backpackSiteItemList, #schluesselSiteItemList, #brieftascheSiteItemList').each(function() {
            $(this).find('li').sort(function(a, b) {
                return $(a).text() < $(b).text() ? -1 : 1;
            }).appendTo(this);
        })
    }


    function DoSomeItemAction(action, fromContainer, uiditemname) {
        var itemAmount = $("#SelectedItemAmount").val();
        if (!canDoItemInteract) return;
        canDoItemInteract = false;
        switch (action) {
            case "use":
                alt.emit("Client:Inventory:UseInvItem", uiditemname, itemAmount, fromContainer);
                break;
            case "drop":
                alt.emit("Client:Inventory:DropInvItem", uiditemname, itemAmount, fromContainer);
                break;
            case "switchToBackpack":
                alt.emit("Client:Inventory:switchItemToDifferentInv", uiditemname, itemAmount, fromContainer, "backpack");
                break;
            case "switchToInventory":
                alt.emit("Client:Inventory:switchItemToDifferentInv", uiditemname, itemAmount, fromContainer, "inventory");
                break;
            case "giveitem":
                if (lastselectedPlace == 0) return;
                alt.emit("Client:Inventory:giveItem", uiditemname, itemAmount, fromContainer, lastselectedPlayerID);
                break;
        }
        $(".custom-menu").hide();
        setTimeout(() => {
            lastselectedPlayerID = 0;
            canDoItemInteract = true;
        }, 1000);
    }

    alt.on("CEF:Inventory:AddInventoryItems", (invArray, backpackSize, targetPlayer) => {
        lastselectedPlayerID = parseInt(targetPlayer);
        SetInventoryInformations(invArray, backpackSize);
    });

    alt.on("CEF:Inventory:playInvSound", (filePath) => {
        playInvAudio(filePath);
        setTimeout(() => {
            stopInvAudio();
        }, 10000);
    })

    function playInvAudio(path) {
        InvAudio = new Audio(path);
        InvAudio.play();
    }