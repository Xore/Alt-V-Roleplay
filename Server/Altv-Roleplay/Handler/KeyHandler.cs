
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.models;
using Altv_Roleplay.Utils;

namespace Altv_Roleplay.Handler
{
    class KeyHandler : IScript
    {
        [AsyncClientEvent("Server:KeyHandler:PressE")]
        public void PressE(IPlayer player)
        {

            lock (player)
            {
                if (player == null || !player.Exists) return;
                int charId = User.GetPlayerOnline(player);
                if (charId == 0) return;

                ClassicColshape farmCol = (ClassicColshape)ServerFarmingSpots.ServerFarmingSpotsColshapes_.FirstOrDefault(x => ((ClassicColshape)x).IsInRange((ClassicPlayer)player));
                if (farmCol != null && !player.IsInVehicle)
                {
                    if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                    if (player.GetPlayerFarmingActionMeta() != "None") return;
                    var farmColData = ServerFarmingSpots.ServerFarmingSpots_.FirstOrDefault(x => x.id == (int)farmCol.GetColShapeId());

                    if (farmColData != null)
                    {
                        if (farmColData.neededItemToFarm != "None")
                        {
                            if (!CharactersInventory.ExistCharacterItem(charId, farmColData.neededItemToFarm, "inventory") && !CharactersInventory.ExistCharacterItem(charId, farmColData.neededItemToFarm, "backpack")) { HUDHandler.SendNotification(player, 1, 2500, $"Zum Farmen benötigst du: {farmColData.neededItemToFarm}."); return; }
                        }
                        player.SetPlayerFarmingActionMeta("farm");
                        FarmingHandler.FarmFieldAction(player, farmColData.itemName, farmColData.itemMinAmount, farmColData.itemMaxAmount, farmColData.animation, farmColData.duration);
                        return;
                    }
                }

                ClassicColshape farmProducerCol = (ClassicColshape)ServerFarmingSpots.ServerFarmingProducerColshapes_.FirstOrDefault(x => ((ClassicColshape)x).IsInRange((ClassicPlayer)player));
                if (farmProducerCol != null && !player.IsInVehicle)
                {
                    if (player.GetPlayerFarmingActionMeta() != "None") { HUDHandler.SendNotification(player, 3, 2500, $"Warte einen Moment."); return; }
                    var farmColData = ServerFarmingSpots.ServerFarmingProducer_.FirstOrDefault(x => x.id == (int)farmProducerCol.GetColShapeId());
                    if (farmColData != null)
                    {
                        //FarmingHandler.ProduceItem(player, farmColData.neededItem, farmColData.producedItem, farmColData.neededItemAmount, farmColData.producedItemAmount, farmColData.duration);
                        FarmingHandler.openFarmingCEF(player, farmColData.neededItem, farmColData.producedItem, farmColData.neededItemAmount, farmColData.producedItemAmount, farmColData.duration, farmColData.neededItemTWO, farmColData.neededItemTHREE, farmColData.neededItemTWOAmount, farmColData.neededItemTHREEAmount);
                        return;
                    }
                }

                WeedPot weedPot = WeedPlantHandler.WeedPots_.FirstOrDefault(x => player.Position.IsInRange(x.position, 2f) && x.state == 3);
                if (weedPot != null && !player.IsInVehicle)
                {
                    //Ernten
                    WeedPlantHandler.harvestPot(player, weedPot);
                    //WeedPlantHandler.removePot(player, weedPot);
                    return;
                }

                if (((ClassicColshape)Minijobs.Elektrolieferant.Main.startJobShape).IsInRange((ClassicPlayer)player))
                {
                    Minijobs.Elektrolieferant.Main.StartMinijob(player);
                    return;
                }

                if (((ClassicColshape)Minijobs.Müllmann.Main.startJobShape).IsInRange((ClassicPlayer)player))
                {
                    Minijobs.Müllmann.Main.StartMinijob(player);
                    return;
                }

                if (((ClassicColshape)Minijobs.Busfahrer.Main.startJobShape).IsInRange((ClassicPlayer)player))
                {
                    Minijobs.Busfahrer.Main.TryStartMinijob(player);
                    return;
                }

                if (((ClassicColshape)CarRental.Cayo.Main.startJobShape).IsInRange((ClassicPlayer)player))
                {
                    CarRental.Cayo.Main.TryStartMinijob(player);
                    return;
                }

                var houseEntrance = ServerHouses.ServerHouses_.FirstOrDefault(x => ((ClassicColshape)x.entranceShape).IsInRange((ClassicPlayer)player));
                if (houseEntrance != null && player.Dimension == 0)
                {
                    HouseHandler.openEntranceCEF(player, houseEntrance.id);
                    return;
                }

                var hotelPos = ServerHotels.ServerHotels_.FirstOrDefault(x => player.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 2f));
                if (hotelPos != null && !player.IsInVehicle)
                {
                    HotelHandler.openCEF(player, hotelPos);
                    return;
                }

                if (player.Dimension >= 5000)
                {
                    int houseInteriorCount = ServerHouses.GetMaxInteriorsCount();
                    for (var i = 1; i <= houseInteriorCount; i++)
                    {
                        if (i > houseInteriorCount || i <= 0) continue;
                        if ((player.Dimension >= 5000 && player.Dimension < 10000) && player.Position.IsInRange(ServerHouses.GetInteriorExitPosition(i), 2f))
                        {
                            //Apartment Leave
                            HotelHandler.LeaveHotel(player);
                            return;
                        }
                        else if ((player.Dimension >= 5000 && player.Dimension < 10000) && player.Position.IsInRange(ServerHouses.GetInteriorStoragePosition(i), 2f))
                        {
                            //Apartment Storage
                            HotelHandler.openStorage(player);
                            return;
                        }
                        else if (player.Dimension >= 10000 && player.Position.IsInRange(ServerHouses.GetInteriorExitPosition(i), 2f))
                        {
                            //House Leave
                            HouseHandler.LeaveHouse(player, i);
                            return;
                        }
                        else if (player.Dimension >= 10000 && player.Position.IsInRange(ServerHouses.GetInteriorStoragePosition(i), 2f))
                        {
                            //House Storage
                            HouseHandler.openStorage(player);
                            return;
                        }
                        else if (player.Dimension >= 10000 && player.Position.IsInRange(ServerHouses.GetInteriorManagePosition(i), 2f))
                        {
                            //Hausverwaltung
                            HouseHandler.openManageCEF(player);
                            return;
                        }
                    }
                }

                var teleportsPos = ServerItems.ServerTeleports_.FirstOrDefault(x => player.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 1.5f));
                if (teleportsPos != null && !player.IsInVehicle)
                {
                    player.Position = new Position(teleportsPos.targetX, teleportsPos.targetY, teleportsPos.targetZ + 0.5f);
                    return;
                }

                var shop = ServerShops.ServerShops_.ToList().FirstOrDefault(x => player.Position.IsInRange(x.shopPos, 2.5f));
                if (shop != null && !player.IsInVehicle)
                {
                    ShopHandler.openShop(player, shop);
                    return;
                }

                var shopManage = ServerShops.ServerShops_.ToList().FirstOrDefault(x => x.owner != 0 && x.owner == charId && player.Position.IsInRange(x.managePos, 1.5f) && x.type == 0);
                if (shopManage != null && !player.IsInVehicle)
                {
                    ShopHandler.openShopManager(player, shopManage.id);
                    return;
                }

                if (player.Position.IsInRange(Constants.Positions.dynasty8_positionShop, 2f))
                {
                    player.Emit("Client:Dynasty8:create", "shops", ServerShops.GetAccountShops(User.GetPlayerOnline(player)), ServerShops.GetFreeShops());
                    return;
                }

                if (player.Position.IsInRange(Constants.Positions.dynasty8_positionStorage, 2f))
                {
                    player.Emit("Client:Dynasty8:create", "storages", ServerStorages.GetAccountStorages(User.GetPlayerOnline(player)), ServerStorages.GetFreeStorages());
                    return;
                }

                var fish = FishingHandler.sandyFishPositions.ToList().FirstOrDefault(x => player.Position.IsInRange(x.position, 2.5f));
                if (fish != null && !player.IsInVehicle)
                {
                    FishingHandler.startFishing((ClassicPlayer)player);
                    return;
                }

                /*if (player.Position.IsInRange(FishingHandler.fishPositions.Sandy1, 2f))
                {
                    FishingHandler.startFishing((ClassicPlayer)player);
                    return;
                }*/

                Server_Storages storageEntry = ServerStorages.ServerStorages_.ToList().FirstOrDefault(x => player.Position.IsInRange(x.entryPos, 2f) && !x.isLocked);
                if (storageEntry != null && !player.IsInVehicle)
                {
                    player.Dimension = storageEntry.id;
                    player.Position = Constants.Positions.storage_ExitPosition;
                    return;
                }

                if (player.Position.IsInRange(Constants.Positions.storage_ExitPosition, 2f) && player.Dimension != 0)
                {
                    Server_Storages storage = ServerStorages.ServerStorages_.ToList().FirstOrDefault(x => x.id == player.Dimension);
                    if (storage == null || storage.entryPos == new Position(0, 0, 0) || storage.isLocked) return;
                    player.Position = storage.entryPos;
                    player.Dimension = 0;
                    return;
                }

                if (player.Position.IsInRange(Constants.Positions.storage_InvPosition, 2.5f) && player.Dimension != 0)
                {
                    StorageHandler.openStorage((ClassicPlayer)player);
                    return;
                }
                if (player.Position.IsInRange(Constants.Positions.storage_LSPDInvPosition, 2.5f) && ServerStorages.ExistStorage(player.Dimension))
                {
                    StorageHandler.openStorage2((ClassicPlayer)player);
                    return;
                }

                var garagePos = ServerGarages.ServerGarages_.FirstOrDefault(x => player.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 2f));
                if (garagePos != null && !player.IsInVehicle)
                {
                    GarageHandler.OpenGarageCEF(player, garagePos.id);
                    return;
                }

                var clothesShopPos = ServerClothesShops.ServerClothesShops_.FirstOrDefault(x => player.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 2f));
                if (clothesShopPos != null && !player.IsInVehicle)
                {
                    ShopHandler.openClothesShop((ClassicPlayer)player, clothesShopPos.id);
                    return;
                }

                var vehicleShopPos = ServerVehicleShops.ServerVehicleShops_.FirstOrDefault(x => player.Position.IsInRange(new Position(x.pedX, x.pedY, x.pedZ), 2f));
                if (vehicleShopPos != null && !player.IsInVehicle)
                {
                    if (vehicleShopPos.neededLicense != "None" && !Characters.HasCharacterPermission(charId, vehicleShopPos.neededLicense)) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast hier keinen Zugriff drauf."); return; }
                    if (vehicleShopPos.id == 1 && ServerFactions.GetCharacterFactionId(charId) != 2) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast hier keinen Zugriff drauf."); return; }
                    if (vehicleShopPos.id == 2 && ServerFactions.GetCharacterFactionId(charId) != 2) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast hier keinen Zugriff drauf."); return; }
                    if (vehicleShopPos.id == 3 && ServerFactions.GetCharacterFactionId(charId) != 2) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast hier keinen Zugriff drauf."); return; }
                    if (vehicleShopPos.id == 4 && ServerFactions.GetCharacterFactionId(charId) != 3) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast hier keinen Zugriff drauf."); return; }
                    if (vehicleShopPos.id == 5 && ServerFactions.GetCharacterFactionId(charId) != 3) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast hier keinen Zugriff drauf."); return; }
                    if (vehicleShopPos.id == 6 && ServerFactions.GetCharacterFactionId(charId) != 14) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast hier keinen Zugriff drauf."); return; }
                    if (vehicleShopPos.id == 32 && ServerFactions.GetCharacterFactionId(charId) != 13) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast hier keinen Zugriff drauf."); return; }
                    if (vehicleShopPos.id == 33 && ServerFactions.GetCharacterFactionId(charId) != 13) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast hier keinen Zugriff drauf."); return; }
                    if (vehicleShopPos.id == 36 && ServerFactions.GetCharacterFactionId(charId) != 15) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast hier keinen Zugriff drauf."); return; }
                    if (vehicleShopPos.id == 37 && ServerFactions.GetCharacterFactionId(charId) != 4) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast hier keinen Zugriff drauf."); return; }
                    if (vehicleShopPos.id == 38 && ServerFactions.GetCharacterFactionId(charId) != 16) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast hier keinen Zugriff drauf."); return; }
                    if (vehicleShopPos.id == 28 && ServerFactions.GetCharacterFactionId(charId) != 17) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast hier keinen Zugriff drauf."); return; }
                    if (vehicleShopPos.id == 40 && ServerFactions.GetCharacterFactionId(charId) != 12) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast hier keinen Zugriff drauf."); return; }
                    ShopHandler.OpenVehicleShop(player, vehicleShopPos.name, vehicleShopPos.id);
                    return;
                }

                var waschPos = player.Position.IsInRange(Constants.Positions.Waschstrasse, 7.5f);
                var waschPos2 = player.Position.IsInRange(Constants.Positions.Waschstrasse2, 7.5f);
                if (waschPos != false && player.IsInVehicle)
                {
                    ShopHandler.usewaschstrasse(player);
                    return;
                }
                if (waschPos2 != false && player.IsInVehicle)
                {
                    ShopHandler.usewaschstrasse(player);
                    return;
                }

                var bankPos = ServerBanks.ServerBanks_.FirstOrDefault(x => player.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 1f));
                if (bankPos != null && !player.IsInVehicle)
                {
                    if (bankPos.zoneName == "Fraktion")
                    {
                        if (!ServerFactions.IsCharacterInAnyFaction(charId)) return;
                        if (ServerFactions.GetCharacterFactionRank(charId) != ServerFactions.GetFactionMaxRankCount(ServerFactions.GetCharacterFactionId(charId)) && ServerFactions.GetCharacterFactionRank(charId) != ServerFactions.GetFactionMaxRankCount(ServerFactions.GetCharacterFactionId(charId)) - 1) { return; }
                        player.EmitLocked("Client:FactionBank:createCEF", "faction", ServerFactions.GetCharacterFactionId(charId), ServerFactions.GetFactionBankMoney(ServerFactions.GetCharacterFactionId(charId)));
                        return;
                    }
                    else
                    {
                        var bankArray = CharactersBank.GetCharacterBankAccounts(charId);
                        player.EmitLocked("Client:Bank:createBankAccountManageForm", bankArray, bankPos.zoneName);
                        return;
                    }
                }

                var barberPos = ServerBarbers.ServerBarbers_.FirstOrDefault(x => player.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 2f));
                if (barberPos != null && !player.IsInVehicle)
                {
                    player.EmitLocked("Client:Barber:barberCreateCEF", Characters.GetCharacterHeadOverlays(charId));
                    return;
                }

                if (player.Position.IsInRange(Constants.Positions.VehicleLicensing_Position, 3f))
                {
                    if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                    VehicleHandler.OpenLicensingCEF(player);
                    return;
                }

                if (player.Position.IsInRange(Constants.Positions.VehicleKey_Position, 3f))
                {
                    if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                    VehicleHandler.OpenKeyCEF(player);
                    return;
                }

                if (player.Position.IsInRange(Constants.Positions.VehicleSell_Position, 3f))
                {
                    if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                    VehicleHandler.OpenSellCEF(player);
                    return;
                }

                if (player.Position.IsInRange(Constants.Positions.LicensePoint, 3f))
                {
                    if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                    TownhallHandler.openGivePlayerLicenseCEF(player);
                    return;
                }

                if (ServerFactions.IsCharacterInAnyFaction(charId))
                {
                    int factionId = ServerFactions.GetCharacterFactionId(charId);
                    var factionDutyPos = ServerFactions.ServerFactionPositions_.FirstOrDefault(x => x.factionId == factionId && x.posType == "duty" && player.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 2f));
                    if (factionDutyPos != null && !player.IsInVehicle)
                    {
                        bool isDuty = ServerFactions.IsCharacterInFactionDuty(charId);
                        ServerFactions.SetCharacterInFactionDuty(charId, !isDuty);
                        if (isDuty)
                        {
                            HUDHandler.SendNotification(player, 1, 6500, "Du hast dich erfolgreich vom Dienst Abgemeldet.");
                            ServerFactions.sendMsg(factionId, $"{Characters.GetCharacterName(charId)} hat sich vom Dienst Abgemeldet");
                        }
                        else
                        {
                            HUDHandler.SendNotification(player, 1, 6500, "Du hast dich erfolgreich zum Dienst Gemeldet.");
                            ServerFactions.sendMsg(factionId, $"{Characters.GetCharacterName(charId)} hat sich in den Dienst Gemeldet");
                        }
                        if (factionId == 2 || factionId == 12) SmartphoneHandler.RequestLSPDIntranet((ClassicPlayer)player);
                        return;
                    }

                    var factionStoragePos = ServerFactions.ServerFactionPositions_.FirstOrDefault(x => x.factionId == factionId && x.posType == "storage" && player.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 2f));
                    if (factionStoragePos != null && !player.IsInVehicle)
                    {
                        if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                        bool isDuty = ServerFactions.IsCharacterInFactionDuty(charId);
                        if (isDuty)
                        {
                            var factionStorageContent = ServerFactions.GetServerFactionStorageItems(factionId, charId); //Fraktionsspind Items
                            var CharacterInvArray = CharactersInventory.GetCharacterInventory(charId); //Spieler Inventar
                            player.EmitLocked("Client:FactionStorage:openCEF", charId, factionId, "faction", CharacterInvArray, factionStorageContent);
                            return;
                        }
                    }

                    var factionServicePhonePos = ServerFactions.ServerFactionPositions_.ToList().FirstOrDefault(x => x.factionId == factionId && x.posType == "servicephone" && player.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 2f));
                    if (factionServicePhonePos != null && !player.IsInVehicle && ServerFactions.IsCharacterInFactionDuty(charId))
                    {
                        int activeLeitstelle = ServerFactions.GetCurrentServicePhoneOwner(factionId);

                        if (activeLeitstelle <= 0)
                        {
                            ServerFactions.UpdateCurrentServicePhoneOwner(factionId, charId);
                            ServerFactions.sendMsg(factionId, $"{Characters.GetCharacterName(charId)} hat das Leitstellentelefon deiner Fraktion übernommen.");
                            return;
                        }
                        if (activeLeitstelle != charId)
                        {
                            HUDHandler.SendNotification(player, 1, 2500, $"Die Leitstelle ist aktuell vom Mitarbeiter {Characters.GetCharacterName(activeLeitstelle)} besetzt.");
                            return;
                        }
                        if (activeLeitstelle == charId)
                        {
                            ServerFactions.UpdateCurrentServicePhoneOwner(factionId, 0);
                            ServerFactions.sendMsg(factionId, $"{Characters.GetCharacterName(charId)} hat das Leitstellentelefon deiner Fraktion abgelegt.");
                            return;
                        }
                    }
                }

                if (ServerGangs.IsCharacterInAnyGang(charId))
                {
                    int gangId = ServerGangs.GetCharacterGangId(charId);
                    var gangStoragePos = ServerGangs.ServerGangPositions_.FirstOrDefault(x => x.gangId == gangId && x.posType == "storage" && player.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 2f));
                    if (gangStoragePos != null && !player.IsInVehicle)
                    {
                        if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                        bool isGang = ServerGangs.IsCharacterInAnyGang(charId);
                        if (isGang)
                        {
                            var gangStorageContent = ServerGangs.GetServerGangStorageItems(gangId, charId); //Fraktionsspind Items
                            var CharacterInvArray = CharactersInventory.GetCharacterInventory(charId); //Spieler Inventar
                            player.EmitLocked("Client:GangStorage:openCEF", charId, gangId, "gang", CharacterInvArray, gangStorageContent);
                            return;
                        }
                    }
                }

                if (player.Position.IsInRange(Constants.Positions.TownhallHouseSelector, 2.5f))
                {
                    TownhallHandler.openHouseSelector(player);
                    return;
                }

                if (player.Position.IsInRange(Constants.Positions.IdentityCardApply, 2.5f) && Characters.GetCharacterAccState(charId) == 0 && !player.IsInVehicle) //Rathaus IdentityCardApply
                {
                    TownhallHandler.tryCreateIdentityCardApplyForm(player);
                    return;
                }

                var tattooShop = ServerTattooShops.ServerTattooShops_.ToList().FirstOrDefault(x => x.owner != 0 && player.Position.IsInRange(new Position(x.pedX, x.pedY, x.pedZ), 2.5f));
                if (tattooShop != null && !player.IsInVehicle)
                {
                    ShopHandler.openTattooShop((ClassicPlayer)player, tattooShop);
                    return;
                }
            }
        }

        [AsyncClientEvent("Server:KeyHandler:PressU")]
        public void PressU(IPlayer player)
        {
            try
            {
                lock (player)
                {
                    if (player == null || !player.Exists) return;
                    int charId = User.GetPlayerOnline(player);
                    if (charId <= 0) return;
                    if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }

                    ClassicColshape serverDoorLockCol = (ClassicColshape)ServerDoors.ServerDoorsLockColshapes_.FirstOrDefault(x => ((ClassicColshape)x).IsInRange((ClassicPlayer)player));
                    if (serverDoorLockCol != null)
                    {
                        var doorColData = ServerDoors.ServerDoors_.FirstOrDefault(x => x.id == (int)serverDoorLockCol.GetColShapeId());
                        if (doorColData != null)
                        {
                            string doorKey = doorColData.doorKey;
                            string doorKey2 = doorColData.doorKey2;
                            if (doorKey == null || doorKey2 == null) return;
                            if (!CharactersInventory.ExistCharacterItem(charId, doorKey, "inventory") && !CharactersInventory.ExistCharacterItem(charId, doorKey, "schluessel") && !CharactersInventory.ExistCharacterItem(charId, doorKey2, "inventory") && !CharactersInventory.ExistCharacterItem(charId, doorKey2, "schluessel")) return;

                            if (!doorColData.state) { HUDHandler.SendNotification(player, 1, 2500, "Tür abgeschlossen."); }
                            else { HUDHandler.SendNotification(player, 1, 2500, "Tür aufgeschlossen."); }
                            doorColData.state = !doorColData.state;
                            Alt.EmitAllClients("Client:DoorManager:ManageDoor", doorColData.hash, new Position(doorColData.posX, doorColData.posY, doorColData.posZ), (bool)doorColData.state);
                            return;
                        }
                    }

                    if (player.Dimension >= 5000)
                    {
                        int houseInteriorCount = ServerHouses.GetMaxInteriorsCount();
                        for (var i = 1; i <= houseInteriorCount; i++)
                        {
                            if (player.Dimension >= 5000 && player.Dimension < 10000 && player.Position.IsInRange(ServerHouses.GetInteriorExitPosition(i), 2f))
                            {
                                //Hotel abschließen / aufschließen
                                if (player.Dimension - 5000 <= 0) continue;
                                int apartmentId = player.Dimension - 5000;
                                int hotelId = ServerHotels.GetHotelIdByApartmentId(apartmentId);
                                if (hotelId <= 0 || apartmentId <= 0) continue;
                                if (!ServerHotels.ExistHotelApartment(hotelId, apartmentId)) { HUDHandler.SendNotification(player, 4, 2500, "Ein unerwarteter Fehler ist aufgetreten [HOTEL-001]."); return; }
                                if (ServerHotels.GetApartmentOwner(hotelId, apartmentId) != charId) { HUDHandler.SendNotification(player, 4, 2500, "Du hast keinen Schlüssel."); return; }
                                HotelHandler.LockHotel(player, hotelId, apartmentId);
                                return;
                            }
                            else if (player.Dimension >= 10000 && player.Position.IsInRange(ServerHouses.GetInteriorExitPosition(i), 2f))
                            {
                                //Haus abschließen / aufschließen
                                if (player.Dimension - 10000 <= 0) continue;
                                int houseId = player.Dimension - 10000;
                                if (houseId <= 0) continue;
                                if (!ServerHouses.ExistHouse(houseId)) { HUDHandler.SendNotification(player, 4, 2500, "Ein unerwarteter Fehler ist aufgetreten [HOUSE-001]."); return; }
                                if (ServerHouses.GetHouseOwner(houseId) != charId && !ServerHouses.IsCharacterRentedInHouse(charId, houseId)) { HUDHandler.SendNotification(player, 3, 2500, "Dieses Haus gehört nicht dir und / oder du bist nicht eingemietet."); return; }
                                HouseHandler.LockHouse(player, houseId);
                                return;
                            }
                        }
                    }

                    var houseEntrance = ServerHouses.ServerHouses_.FirstOrDefault(x => ((ClassicColshape)x.entranceShape).IsInRange((ClassicPlayer)player));
                    if (houseEntrance != null)
                    {
                        HouseHandler.LockHouse(player, houseEntrance.id);
                    }

                    Server_Storages storage = ServerStorages.ServerStorages_.FirstOrDefault(x => player.Position.IsInRange(x.entryPos, 2f) && (x.owner == User.GetPlayerOnline(player) || x.secondOwner == User.GetPlayerOnline(player) || x.factionid == ServerFactions.GetCharacterFactionId(charId)));
                    if (storage != null && !player.IsInVehicle && player.Dimension == 0)
                    {
                        storage.isLocked = !storage.isLocked;
                        if (storage.isLocked) HUDHandler.SendNotification(player, 1, 2500, "Du hast die Lagerhalle abgeschlossen.");
                        else HUDHandler.SendNotification(player, 1, 2500, "Du hast die Lagerhalle aufgeschlossen.");
                        return;
                    }

                    if (player.Position.IsInRange(Constants.Positions.storage_ExitPosition, 2f) && player.Dimension != 0 && (ServerStorages.GetOwner(player.Dimension) == User.GetPlayerOnline(player) || ServerStorages.GetSecondOwner(player.Dimension) == User.GetPlayerOnline(player)))
                    {
                        Server_Storages storages = ServerStorages.ServerStorages_.FirstOrDefault(x => x.id == player.Dimension);
                        if (storages == null) return;
                        storages.isLocked = !storages.isLocked;
                        if (storages.isLocked) HUDHandler.SendNotification(player, 1, 2500, "Du hast die Lagerhalle abgeschlossen.");
                        else HUDHandler.SendNotification(player, 1, 2500, "Du hast die Lagerhalle aufgeschlossen.");
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [AsyncClientEvent("Server:KeyHandler:PressZ")]
        public void PressZ(IPlayer player)
        {
            models.Server_Dropped_Items droppedItem = ServerDroppedItems.ServerDroppedItems_.ToList().FirstOrDefault(x => player.Position.IsInRange(x.pos, 2f));
            if (droppedItem != null && !player.IsInVehicle)
            {
                lock (droppedItem)
                    ServerDroppedItems.takeItem((ClassicPlayer)player, droppedItem);
                player.EmitLocked("Client:Inventory:PlayAnimation", "anim@mp_snowball", "pickup_snowball", 500, 1, false);
                return;
            }

            Object objects = ObjectHandler.Objects_.FirstOrDefault(x => player.Position.IsInRange(x.position, 2f));
            if (objects != null && !player.IsInVehicle)
            {
                lock (objects)
                ObjectHandler.removeItem((ClassicPlayer)player, objects);
                return;
            }
        }
        [AsyncClientEvent("Server:KeyHandler:PressRagdoll")]
        public void PressRagdoll(IPlayer player)
        {
            player.EmitLocked("Client:Ragdoll:SetPedToRagdoll", true, 0);
        }
        [AsyncClientEvent("Server:KeyHandler:PressRagdoll2")]
        public void PressRagdoll2(IPlayer player)
        {
            player.EmitLocked("Client:Ragdoll:SetPedToRagdoll", false, 0);
        }
    }
}