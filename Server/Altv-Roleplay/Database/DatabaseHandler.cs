using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.models;
using Altv_Roleplay.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Altv_Roleplay.Database
{
    internal class DatabaseHandler
    {
        static bool clothesNPCSpawned = false;
        internal static void LoadAllPlayers()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    User.Player = new List<Accounts>(db.Accounts);
                    Alt.Log($"{User.Player.Count} Spieler wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllIdentifiedVehicles()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerIdentifiedVehicles.ServerIdentifiedVehicles_ = new List<Server_IdentifiedVehicles>(db.Server_IdentifiedVehicles);
                }
                Alt.Log($"{ ServerIdentifiedVehicles.ServerIdentifiedVehicles_.Count} identifizierte Autos wurden geladen.");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");

            }
        }

        internal static void LoadAllServerStorages()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerStorages.ServerStorages_ = new List<Server_Storages>(db.Server_Storages);
                    Alt.Log($"{ServerStorages.ServerStorages_.Count} Server-Storages wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllTattooStuff()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerTattoos.ServerTattoos_ = new List<Server_Tattoos>(db.Server_Tattoos);
                    CharactersTattoos.CharactersTattoos_ = new List<Characters_Tattoos>(db.Characters_Tattoos);
                    ServerTattooShops.ServerTattooShops_ = new List<Server_Tattoo_Shops>(db.Server_Tattoo_Shops);
                }
                Alt.Log($"{CharactersTattoos.CharactersTattoos_.Count} Character-Tattoos wurden geladen.");
                Alt.Log($"{ServerTattoos.ServerTattoos_.Count} Server-Tattoos wurden geladen.");
                Alt.Log($"{ServerTattooShops.ServerTattooShops_.Count} Server-Tattoo-Shops wurden geladen.");

                foreach (var tattooShop in ServerTattooShops.ServerTattooShops_)
                {
                    ServerBlips.ServerBlips_.Add(new Server_Blips
                    {
                        name = $"{tattooShop.name}",
                        scale = 0.5f,
                        shortRange = true,
                        posX = tattooShop.pedX,
                        posY = tattooShop.pedY,
                        posZ = tattooShop.pedZ,
                        sprite = 75,
                        color = 0
                    });

                    ServerPeds.ServerPeds_.Add(new Server_Peds
                    {
                        model = tattooShop.pedModel,
                        posX = tattooShop.pedX,
                        posY = tattooShop.pedY,
                        posZ = tattooShop.pedZ,
                        rotation = tattooShop.pedRot
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        internal static void LoadAllPlayerCharacters()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    Characters.PlayerCharacters = new List<AccountsCharacters>(db.AccountsCharacters);
                    Alt.Log($"{Characters.PlayerCharacters.Count} Charakter wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllCharacterWanteds()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    CharactersWanteds.ServerWanteds_ = new List<Server_Wanteds>(db.Server_Wanteds);
                    Alt.Log($"{CharactersWanteds.ServerWanteds_.Count} Server-Wanted-Einträge wurden geladen.");

                    CharactersWanteds.CharactersWanteds_ = new List<Characters_Wanteds>(db.Characters_Wanteds);
                    Alt.Log($"{CharactersWanteds.CharactersWanteds_.Count} Characters-Wanted-Einträge wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllCharacterPhoneChats()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    CharactersPhone.CharactersPhoneChats_ = new List<CharactersPhoneChats>(db.CharactersPhoneChats);
                    Alt.Log($"{CharactersPhone.CharactersPhoneChats_.Count} Character-Phone-Chats wurden geladen.");

                    CharactersPhone.CharactersPhoneChatMessages_ = new List<CharactersPhoneChatMessages>(db.CharactersPhoneChatMessages);
                    Alt.Log($"{CharactersPhone.CharactersPhoneChatMessages_.Count} Character-Phone-Chat-Messages wurden geladen.");

                    CharactersPhone.CharactersPhoneContacts_ = new List<CharactersPhoneContacts>(db.CharactersPhoneContacts);
                    Alt.Log($"{CharactersPhone.CharactersPhoneContacts_.Count} Character-Phone-Contacts wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllCharacterMinijobData()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    CharactersMinijobs.CharactersMinijobsData_ = new List<Characters_Minijobs>(db.Characters_Minijobs);
                    Alt.Log($"{CharactersMinijobs.CharactersMinijobsData_.Count} Character-Minijob-Entrys wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllCharacterBankAccounts()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    CharactersBank.CharactersBank_ = new List<Characters_Bank>(db.Characters_Bank);
                    Alt.Log($"{CharactersBank.CharactersBank_.Count} Character Bank Accounts wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerShopItems()
        {
            try
            {
                var methPrice = new Random().Next(80, 350);
                using (var db = new gtaContext())
                {
                    ServerShops.ServerShopsItems_ = new List<Server_Shops_Items>(db.Server_Shops_Items);
                    foreach (var shopItem in ServerShops.ServerShopsItems_)
                    {
                        if (shopItem.itemName != "Methamphetamin") continue;
                        shopItem.itemPrice = methPrice;
                        db.Server_Shops_Items.Update(shopItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            try
            {
                var cokePrice = new Random().Next(100, 400);
                using (var db = new gtaContext())
                {
                    ServerShops.ServerShopsItems_ = new List<Server_Shops_Items>(db.Server_Shops_Items);
                    foreach (var shopItem in ServerShops.ServerShopsItems_)
                    {
                        if (shopItem.itemName != "Kokain-Pack") continue;
                        shopItem.itemPrice = cokePrice;
                        db.Server_Shops_Items.Update(shopItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            try
            {
                var weedPrice = new Random().Next(60, 200);
                using (var db = new gtaContext())
                {
                    ServerShops.ServerShopsItems_ = new List<Server_Shops_Items>(db.Server_Shops_Items);
                    foreach (var shopItem in ServerShops.ServerShopsItems_)
                    {
                        if (shopItem.itemName != "Weed-Pack") continue;
                        shopItem.itemPrice = weedPrice;
                        db.Server_Shops_Items.Update(shopItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            try
            {
                var glasPrice = new Random().Next(50, 100);
                using (var db = new gtaContext())
                {
                    ServerShops.ServerShopsItems_ = new List<Server_Shops_Items>(db.Server_Shops_Items);
                    foreach (var shopItem in ServerShops.ServerShopsItems_)
                    {
                        if (shopItem.itemName != "Glasscheibe") continue;
                        shopItem.itemPrice = glasPrice;
                        db.Server_Shops_Items.Update(shopItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            try
            {
                var passierteTomatenPrice = new Random().Next(10, 15);
                using (var db = new gtaContext())
                {
                    ServerShops.ServerShopsItems_ = new List<Server_Shops_Items>(db.Server_Shops_Items);
                    foreach (var shopItem in ServerShops.ServerShopsItems_)
                    {
                        if (shopItem.itemName != "Passierte-Tomaten") continue;
                        shopItem.itemPrice = passierteTomatenPrice;
                        db.Server_Shops_Items.Update(shopItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            try
            {
                var RosinenPrice = new Random().Next(6, 12);
                using (var db = new gtaContext())
                {
                    ServerShops.ServerShopsItems_ = new List<Server_Shops_Items>(db.Server_Shops_Items);
                    foreach (var shopItem in ServerShops.ServerShopsItems_)
                    {
                        if (shopItem.itemName != "Rosinen") continue;
                        shopItem.itemPrice = RosinenPrice;
                        db.Server_Shops_Items.Update(shopItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            try
            {
                var PopcornPrice = new Random().Next(6, 12);
                using (var db = new gtaContext())
                {
                    ServerShops.ServerShopsItems_ = new List<Server_Shops_Items>(db.Server_Shops_Items);
                    foreach (var shopItem in ServerShops.ServerShopsItems_)
                    {
                        if (shopItem.itemName != "Popcorn") continue;
                        shopItem.itemPrice = PopcornPrice;
                        db.Server_Shops_Items.Update(shopItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            try
            {
                var BretterPrice = new Random().Next(16, 25);
                using (var db = new gtaContext())
                {
                    ServerShops.ServerShopsItems_ = new List<Server_Shops_Items>(db.Server_Shops_Items);
                    foreach (var shopItem in ServerShops.ServerShopsItems_)
                    {
                        if (shopItem.itemName != "Bretter") continue;
                        shopItem.itemPrice = BretterPrice;
                        db.Server_Shops_Items.Update(shopItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            try
            {
                var KleidungPrice = new Random().Next(16, 25);
                using (var db = new gtaContext())
                {
                    ServerShops.ServerShopsItems_ = new List<Server_Shops_Items>(db.Server_Shops_Items);
                    foreach (var shopItem in ServerShops.ServerShopsItems_)
                    {
                        if (shopItem.itemName != "Kleidung") continue;
                        shopItem.itemPrice = KleidungPrice;
                        db.Server_Shops_Items.Update(shopItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            try
            {
                var ZiegelPrice = new Random().Next(10, 32);
                using (var db = new gtaContext())
                {
                    ServerShops.ServerShopsItems_ = new List<Server_Shops_Items>(db.Server_Shops_Items);
                    foreach (var shopItem in ServerShops.ServerShopsItems_)
                    {
                        if (shopItem.itemName != "Ziegel") continue;
                        shopItem.itemPrice = ZiegelPrice;
                        db.Server_Shops_Items.Update(shopItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            try
            {
                var ChickenPrice = new Random().Next(6, 22);
                using (var db = new gtaContext())
                {
                    ServerShops.ServerShopsItems_ = new List<Server_Shops_Items>(db.Server_Shops_Items);
                    foreach (var shopItem in ServerShops.ServerShopsItems_)
                    {
                        if (shopItem.itemName != "Chicken-Wings") continue;
                        shopItem.itemPrice = ChickenPrice;
                        db.Server_Shops_Items.Update(shopItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            try
            {
                var TabakPrice = new Random().Next(6, 16);
                using (var db = new gtaContext())
                {
                    ServerShops.ServerShopsItems_ = new List<Server_Shops_Items>(db.Server_Shops_Items);
                    foreach (var shopItem in ServerShops.ServerShopsItems_)
                    {
                        if (shopItem.itemName != "Zigaretten") continue;
                        shopItem.itemPrice = TabakPrice;
                        db.Server_Shops_Items.Update(shopItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            try
            {
                var OelPrice = new Random().Next(20, 42);
                using (var db = new gtaContext())
                {
                    ServerShops.ServerShopsItems_ = new List<Server_Shops_Items>(db.Server_Shops_Items);
                    foreach (var shopItem in ServerShops.ServerShopsItems_)
                    {
                        if (shopItem.itemName != "Raffiniertes-Oel") continue;
                        shopItem.itemPrice = OelPrice;
                        db.Server_Shops_Items.Update(shopItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            try
            {
                var EisenPrice = new Random().Next(25, 52);
                using (var db = new gtaContext())
                {
                    ServerShops.ServerShopsItems_ = new List<Server_Shops_Items>(db.Server_Shops_Items);
                    foreach (var shopItem in ServerShops.ServerShopsItems_)
                    {
                        if (shopItem.itemName != "Eisenbarren") continue;
                        shopItem.itemPrice = EisenPrice;
                        db.Server_Shops_Items.Update(shopItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            try
            {
                var WeinPrice = new Random().Next(16, 25);
                using (var db = new gtaContext())
                {
                    ServerShops.ServerShopsItems_ = new List<Server_Shops_Items>(db.Server_Shops_Items);
                    foreach (var shopItem in ServerShops.ServerShopsItems_)
                    {
                        if (shopItem.itemName != "Wein") continue;
                        shopItem.itemPrice = WeinPrice;
                        db.Server_Shops_Items.Update(shopItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            try
            {
                var DiaPrice = new Random().Next(100, 350);
                using (var db = new gtaContext())
                {
                    ServerShops.ServerShopsItems_ = new List<Server_Shops_Items>(db.Server_Shops_Items);
                    foreach (var shopItem in ServerShops.ServerShopsItems_)
                    {
                        if (shopItem.itemName != "Diamanten") continue;
                        shopItem.itemPrice = DiaPrice;
                        db.Server_Shops_Items.Update(shopItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            try
            {
                var GoldPrice = new Random().Next(60, 250);
                using (var db = new gtaContext())
                {
                    ServerShops.ServerShopsItems_ = new List<Server_Shops_Items>(db.Server_Shops_Items);
                    foreach (var shopItem in ServerShops.ServerShopsItems_)
                    {
                        if (shopItem.itemName != "Goldbarren") continue;
                        shopItem.itemPrice = GoldPrice;
                        db.Server_Shops_Items.Update(shopItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        internal static void LoadAllServerFarmingSpots()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerFarmingSpots.ServerFarmingSpots_ = new List<Server_Farming_Spots>(db.Server_Farming_Spots);
                    Alt.Log($"{ServerFarmingSpots.ServerFarmingSpots_.Count} Server-Farming-Spots wurden geladen.");
                }

                foreach (var spot in ServerFarmingSpots.ServerFarmingSpots_)
                {
                    ClassicColshape cols = (ClassicColshape)Alt.CreateColShapeSphere(new Position(spot.posX, spot.posY, spot.posZ), spot.range + 0.5f);
                    cols.SetColShapeName("Farmfield");
                    cols.SetColShapeId((long)spot.id);
                    cols.Radius = spot.range + 0.5f;
                    ServerFarmingSpots.ServerFarmingSpotsColshapes_.Add(cols);
                }

                var uniqueSpots = ServerFarmingSpots.ServerFarmingSpots_.GroupBy(x => x.itemName).Select(x => x.FirstOrDefault()).ToList();
                if (!uniqueSpots.Any()) return;
                uniqueSpots.ForEach(spot =>
                {
                    if (spot.isBlipVisible)
                    {
                        string blipName = $"{spot.itemName}";
                        var blipData = new Server_Blips
                        {
                            name = blipName,
                            color = spot.blipColor,
                            scale = 0.5f,
                            shortRange = true,
                            sprite = 164,
                            posX = spot.posX,
                            posY = spot.posY,
                            posZ = spot.posZ
                        };
                        ServerBlips.ServerBlips_.Add(blipData);
                    }
                });
            }
            catch (Exception e) { Alt.Log($"{e}"); }
        }

        internal static void LoadAllServerHotels()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerHotels.ServerHotels_ = new List<Server_Hotels>(db.Server_Hotels);
                    ServerHotels.ServerHotelsApartments_ = new List<Server_Hotels_Apartments>(db.Server_Hotels_Apartments);
                    ServerHotels.ServerHotelsStorage_ = new List<Server_Hotels_Storage>(db.Server_Hotels_Storages);
                    Alt.Log($"{ServerHotels.ServerHotels_.Count} Server-Hotels wurden geladen.");
                    Alt.Log($"{ServerHotels.ServerHotelsApartments_.Count} Server-Hotels-Apartments wurden geladen.");
                    Alt.Log($"{ServerHotels.ServerHotelsStorage_.Count} Server-Hotels-Storage-Items wurden geladen.");
                }

                foreach (var hotel in ServerHotels.ServerHotels_)
                {
                    //var markerData = new Server_Markers
                    //{
                    //    type = 1,
                    //    posX = hotel.posX,
                    //    posY = hotel.posY,
                    //    posZ = hotel.posZ - 0.2f,
                    //    scaleX = 1,
                    //    scaleY = 1,
                    //    scaleZ = 1,
                    //    red = 255,
                    //    green = 102,
                    //    blue = 102,
                    //    alpha = 150,
                    //    bobUpAndDown = true
                    //};

                    var blipData = new Server_Blips
                    {
                        name = $"{hotel.name}",
                        posX = hotel.posX,
                        posY = hotel.posY,
                        posZ = hotel.posZ,
                        scale = 0.5f,
                        shortRange = true,
                        sprite = 475,
                        color = 6
                    };
                    ServerBlips.ServerBlips_.Add(blipData);
                    //ServerBlips.ServerMarkers_.Add(markerData);
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerHouses()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerHouses.ServerHousesInteriors_ = new List<Server_Houses_Interiors>(db.Server_Houses_Interiors);
                    ServerHouses.ServerHousesStorage_ = new List<Server_Houses_Storage>(db.Server_Houses_Storages);
                    ServerHouses.ServerHousesRenter_ = new List<Server_Houses_Renter>(db.Server_Houses_Renters);
                    foreach (var house in db.Server_Houses)
                    {
                        ServerHouses.CreateHouse(house.id, house.interiorId, house.ownerId, house.street, house.price, house.maxRenters, house.rentPrice, house.isRentable, house.hasStorage, house.hasAlarm, house.hasBank, new Position(house.entranceX, house.entranceY, house.entranceZ), house.money);
                    }
                    foreach (var interior in ServerHouses.ServerHousesInteriors_)
                    {
                        //var exitData = new Server_Markers
                        //{
                        //    type = 1,
                        //    posX = interior.exitX,
                        //    posY = interior.exitY,
                        //    posZ = interior.exitZ,
                        //    scaleX = 1,
                        //    scaleY = 1,
                        //    scaleZ = 1,
                        //    red = 255,
                        //    green = 102,
                        //    blue = 102,
                        //    alpha = 150,
                        //    bobUpAndDown = false
                        //};
                        var storageData = new Server_Markers
                        {
                            type = 27,
                            posX = interior.storageX,
                            posY = interior.storageY,
                            posZ = interior.storageZ,
                            scaleX = 1,
                            scaleY = 1,
                            scaleZ = 1,
                            red = 255,
                            green = 102,
                            blue = 102,
                            alpha = 50,
                            bobUpAndDown = false
                        };
                        var manageData = new Server_Markers
                        {
                            type = 27,
                            posX = interior.manageX,
                            posY = interior.manageY,
                            posZ = interior.manageZ,
                            scaleX = 1,
                            scaleY = 1,
                            scaleZ = 1,
                            red = 255,
                            green = 102,
                            blue = 102,
                            alpha = 50,
                            bobUpAndDown = false
                        };

                        //ServerBlips.ServerMarkers_.Add(exitData);
                        ServerBlips.ServerMarkers_.Add(storageData);
                        ServerBlips.ServerMarkers_.Add(manageData);
                    }
                    Alt.Log($"{ServerHouses.ServerHouses_.Count} Server-Houses wurden geladen.");
                    Alt.Log($"{ServerHouses.ServerHousesInteriors_.Count} Server-House-Interior wurden geladen.");
                    Alt.Log($"{ServerHouses.ServerHousesStorage_.Count} Server-House-Storage-Items wurden geladen.");
                    Alt.Log($"{ServerHouses.ServerHousesRenter_.Count} Server-House-Renter wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerMinijobBusdriverRoutes()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    Minijobs.Busfahrer.Model.ServerMinijobBusdriverRoutes_ = new List<Server_Minijob_Busdriver_Routes>(db.Server_Minijob_Busdriver_Routes);
                    Alt.Log($"{Minijobs.Busfahrer.Model.ServerMinijobBusdriverRoutes_.Count} Server-Minijobs-BusDriver-Routes wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerDoors()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerDoors.ServerDoors_ = new List<Server_Doors>(db.Server_Doors);
                    Alt.Log($"{ServerDoors.ServerDoors_.Count} Server-Doors wurden geladen.");
                }

                foreach (var door in ServerDoors.ServerDoors_)
                {
                    ClassicColshape cols = (ClassicColshape)Alt.CreateColShapeSphere(new Position(door.posX, door.posY, door.posZ), 20f);
                    cols.SetColShapeName("DoorShape");
                    cols.SetColShapeId((long)door.id);
                    cols.Radius = 20f;
                    ServerDoors.ServerDoorsColshapes_.Add(cols);

                    if (door.type == "Door")
                    {
                        ClassicColshape lockCol = (ClassicColshape)Alt.CreateColShapeSphere(new Position(door.lockPosX, door.lockPosY, door.lockPosZ), 1.3f);
                        lockCol.SetColShapeName("DoorShape");
                        lockCol.SetColShapeId((long)door.id);
                        lockCol.Radius = 1.3f;
                        ServerDoors.ServerDoorsLockColshapes_.Add(lockCol);
                        continue;
                    }
                    else if (door.type == "Gate")
                    {
                        ClassicColshape lockCol = (ClassicColshape)Alt.CreateColShapeSphere(new Position(door.lockPosX, door.lockPosY, door.lockPosZ), 10f);
                        lockCol.SetColShapeName("DoorShape");
                        lockCol.SetColShapeId((long)door.id);
                        lockCol.Radius = 10f;
                        ServerDoors.ServerDoorsLockColshapes_.Add(lockCol);
                        continue;
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerFactions()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerFactions.ServerFactions_ = new List<Server_Factions>(db.Server_Factions);
                    ServerFactions.ServerFactionPositions_ = new List<Server_Faction_Positions>(db.Server_Faction_Positions);
                    ServerFactions.ServerFactionDispatches_ = new List<ServerFaction_Dispatch>(db.Server_Faction_Dispatch);
                    Alt.Log($"{ServerFactions.ServerFactions_.Count} Server-Factions wurden geladen.");
                    Alt.Log($"{ServerFactions.ServerFactionPositions_.Count} Server-Faction-Positions wurden geladen.");
                    Alt.Log($"{ServerFactions.ServerFactionDispatches_.Count} Server-Faction-Dispatches wurden geladen.");
                }

                foreach (var pos in ServerFactions.ServerFactionPositions_)
                {
                    if (pos.posType == "duty")
                    {
                        string model = "";
                        if (pos.factionId == 2) model = "s_m_y_cop_01";
                        else if (pos.factionId == 4) model = "s_m_y_construct_01";
                        var data = new Server_Peds
                        {
                            model = model,
                            posX = pos.posX,
                            posY = pos.posY,
                            posZ = pos.posZ,
                            rotation = pos.rotation
                        };
                        ServerPeds.ServerPeds_.Add(data);
                    }
                    else if (pos.posType == "storage")
                    {
                        var MarkerData = new Server_Markers
                        {
                            type = 27,
                            posX = pos.posX,
                            posY = pos.posY,
                            posZ = pos.posZ,
                            scaleX = 1,
                            scaleY = 1,
                            scaleZ = 1,
                            red = 224,
                            green = 58,
                            blue = 58,
                            alpha = 50,
                            bobUpAndDown = false
                        };
                        ServerBlips.ServerMarkers_.Add(MarkerData);
                    }

                    else if (pos.posType == "servicephone")
                    {
                        var markerData = new Server_Markers
                        {
                            type = 27,
                            posX = pos.posX,
                            posY = pos.posY,
                            posZ = pos.posZ,
                            scaleX = 1,
                            scaleY = 1,
                            scaleZ = 1,
                            red = 224,
                            green = 58,
                            blue = 58,
                            alpha = 50,
                            bobUpAndDown = false
                        };
                        ServerBlips.ServerMarkers_.Add(markerData);
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerGangs()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerGangs.ServerGangs_ = new List<Server_Gangs>(db.Server_Gangs);
                    ServerGangs.ServerGangPositions_ = new List<Server_Gang_Positions>(db.Server_Gang_Positions);
                    Alt.Log($"{ServerGangs.ServerGangs_.Count} Server-Gangs wurden geladen.");
                    Alt.Log($"{ServerGangs.ServerGangPositions_.Count} Server-Gang-Positions wurden geladen.");
                }

                foreach (var pos in ServerGangs.ServerGangPositions_)
                {
                    if (pos.posType == "storage")
                    {
                        var MarkerData = new Server_Markers
                        {
                            type = 27,
                            posX = pos.posX,
                            posY = pos.posY,
                            posZ = pos.posZ,
                            scaleX = 1,
                            scaleY = 1,
                            scaleZ = 1,
                            red = 224,
                            green = 58,
                            blue = 58,
                            alpha = 50,
                            bobUpAndDown = false
                        };
                        ServerBlips.ServerMarkers_.Add(MarkerData);
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerFactionRanks()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerFactions.ServerFactionRanks_ = new List<Server_Faction_Ranks>(db.Server_Faction_Ranks);
                    Alt.Log($"{ServerFactions.ServerFactionRanks_.Count} Server-Faction-Ranks wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerGangRanks()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerGangs.ServerGangRanks_ = new List<Server_Gang_Ranks>(db.Server_Gang_Ranks);
                    Alt.Log($"{ServerGangs.ServerGangRanks_.Count} Server-Gang-Ranks wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerFactionMembers()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerFactions.ServerFactionMembers_ = new List<Server_Faction_Members>(db.Server_Faction_Members);
                    Alt.Log($"{ServerFactions.ServerFactionMembers_.Count} Server-Faction-Member wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerGangMembers()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerGangs.ServerGangMembers_ = new List<Server_Gang_Members>(db.Server_Gang_Members);
                    Alt.Log($"{ServerGangs.ServerGangMembers_.Count} Server-Gang-Member wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerFactionStorageItems()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerFactions.ServerFactionStorageItems_ = new List<Server_Faction_Storage_Items>(db.Server_Faction_Storage_Items);
                    Alt.Log($"{ServerFactions.ServerFactionStorageItems_.Count} Server-Faction-Storage-Items wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerGangStorageItems()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerGangs.ServerGangStorageItems_ = new List<Server_Gang_Storage_Items>(db.Server_Gang_Storage_Items);
                    Alt.Log($"{ServerGangs.ServerGangStorageItems_.Count} Server-Gang-Storage-Items wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerTabletNotes()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    CharactersTablet.ServerTabletNotesData_ = new List<Server_Tablet_Notes>(db.Server_Tablet_Notes);
                    Alt.Log($"{CharactersTablet.ServerTabletNotesData_.Count} Server-Tablet-Notes wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerTabletEvents()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    CharactersTablet.ServerTabletEventsData_ = new List<Server_Tablet_Events>(db.Server_Tablet_Events);
                }

                foreach (var ev in CharactersTablet.ServerTabletEventsData_.Where(x => DateTime.Now.Subtract(Convert.ToDateTime(x.created)).TotalHours >= 168).ToList())
                {
                    CharactersTablet.ServerTabletEventsData_.Remove(ev);
                    using (gtaContext db = new gtaContext())
                    {
                        db.Server_Tablet_Events.Remove(ev);
                        db.SaveChanges();
                    }
                }
                Alt.Log($"{CharactersTablet.ServerTabletEventsData_.Count} Server-Tablet-Events wurden geladen.");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerTabletAppData()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    CharactersTablet.ServerTabletAppsData_ = new List<Server_Tablet_Apps>(db.Server_Tablet_Apps);
                    Alt.Log($"{CharactersTablet.ServerTabletAppsData_.Count} Server-Tablet-App-Datas wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllCharactersTabletApps()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    CharactersTablet.CharactersTabletApps_ = new List<Characters_Tablet_Apps>(db.Characters_Tablet_Apps);
                    Alt.Log($"{CharactersTablet.CharactersTabletApps_.Count} Character-Tablet-App Einträge wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerFuelStations()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerFuelStations.ServerFuelStations_ = new List<Server_Fuel_Stations>(db.Server_Fuel_Stations);
                    Alt.Log($"{ServerFuelStations.ServerFuelStations_.Count} Server-Tankstellen wurden geladen.");
                }
            }
            catch (Exception e) { Alt.Log($"{e}"); }
        }

        internal static void LoadALlServerFuelStationSpots()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerFuelStations.ServerFuelStationSpots_ = new List<Server_Fuel_Spots>(db.Server_Fuel_Spots);
                    Alt.Log($"{ServerFuelStations.ServerFuelStationSpots_.Count} Server-Tankstellen-Spots wurden geladen");
                }

                var uniqueSpots = ServerFuelStations.ServerFuelStationSpots_.GroupBy(x => x.fuelStationId).Select(x => x.FirstOrDefault()).ToList();
                if (!uniqueSpots.Any()) return;
                uniqueSpots.ForEach(spot =>
                {
                    //var blipData = new Server_Blips
                    //{
                    //    name = "Tankstelle",
                    //    color = 75,
                    //    scale = 0.75f,
                    //    shortRange = true,
                    //    sprite = 361,
                    //    posX = spot.posX,
                    //    posY = spot.posY,
                    //    posZ = spot.posZ
                    //};
                    //ServerBlips.ServerBlips_.Add(blipData);
                });
            }
            catch (Exception e) { Alt.Log($"{e}"); }
        }

        internal static void LoadAllServerFarmingProducers()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerFarmingSpots.ServerFarmingProducer_ = new List<Server_Farming_Producer>(db.Server_Farming_Producer);
                    Alt.Log($"{ServerFarmingSpots.ServerFarmingProducer_.Count} Server-Farming-Verarbeiter wurden geladen.");
                }

                foreach (var producer in ServerFarmingSpots.ServerFarmingProducer_)
                {
                    if (producer.isBlipVisible)
                    {
                        string blipName = $"{producer.blipName}";
                        ServerBlips.ServerBlips_.Add(new Server_Blips
                        {
                            name = blipName,
                            color = 2,
                            scale = 0.5f,
                            shortRange = true,
                            sprite = 365,
                            posX = producer.posX,
                            posY = producer.posY,
                            posZ = producer.posZ
                        });
                    }

                    ServerPeds.ServerPeds_.Add(new Server_Peds
                    {
                        model = $"{producer.pedModel}",
                        posX = producer.posX,
                        posY = producer.posY,
                        posZ = producer.posZ,
                        rotation = producer.pedRotation
                    });

                    ClassicColshape cols = (ClassicColshape)Alt.CreateColShapeSphere(new Position(producer.posX, producer.posY, producer.posZ), producer.range);
                    cols.SetColShapeName("Farmproducer");
                    cols.SetColShapeId((long)producer.id);
                    cols.Radius = producer.range;
                    ServerFarmingSpots.ServerFarmingProducerColshapes_.Add(cols);
                }
            }
            catch (Exception e) { Alt.Log($"{e}"); }
        }

        internal static void LoadAllVehicleShops()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerVehicleShops.ServerVehicleShops_ = new List<Server_Vehicle_Shops>(db.Server_Vehicle_Shops);
                    Alt.Log($"{ServerVehicleShops.ServerVehicleShops_.Count} Server-Vehicle-Shops wurden geladen.");
                }

                foreach (var shop in ServerVehicleShops.ServerVehicleShops_)
                {
                    string blipName = $"{shop.name}";
                    int blipSprite = 225;
                    int blipColor = 4;
                    if (shop.id == 27) { blipSprite = 43; }
                    if (shop.id == 28) { blipSprite = 90; }
                    var BlipData = new Server_Blips
                    {
                        name = blipName,
                        color = blipColor,
                        scale = 0.5f,
                        shortRange = true,
                        sprite = blipSprite,
                        posX = shop.pedX,
                        posY = shop.pedY,
                        posZ = shop.pedZ
                    };
                    if (shop.id != 1 && shop.id != 2 && shop.id != 3 && shop.id != 4 && shop.id != 5 && shop.id != 6 && shop.id != 32 && shop.id != 33 && shop.id != 36 && shop.id != 37 && shop.id != 38 && shop.id != 40)
                    {
                        ServerBlips.ServerBlips_.Add(BlipData);
                    }

                    var PedData = new Server_Peds
                    {
                        model = shop.pedModel,
                        posX = shop.pedX,
                        posY = shop.pedY,
                        posZ = shop.pedZ,
                        rotation = shop.pedRot
                    };
                    ServerPeds.ServerPeds_.Add(PedData);
                }
            }
            catch (Exception e) { Alt.Log($"{e}"); }
        }

        internal static void LoadAllVehicleShopItems()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerVehicleShops.ServerVehicleShopsItems_ = new List<Server_Vehicle_Shops_Items>(db.Server_Vehicle_Shops_Items);
                    Alt.Log($"{ServerVehicleShops.ServerVehicleShopsItems_.Count} Server-Vehicle-ShopItems wurden geladen.");
                }

                foreach (var veh in ServerVehicleShops.ServerVehicleShopsItems_.Where(x => x.isOnlyOnlineAvailable == false))
                {
                    //IVehicle altVeh = Alt.CreateVehicle((uint)veh.hash, new Position(veh.posX, veh.posY, veh.posZ), new Rotation(veh.rotX, veh.rotY, veh.rotZ)); //ToDo: Fahrzeug ggf. unzerstörbar machen & freezen
                    //altVeh.LockState = VehicleLockState.Locked;
                    //altVeh.EngineOn = false;
                    //altVeh.NumberplateText = "CARDEALER";
                    //altVeh.SetStreamSyncedMetaData("IsVehicleCardealer", true);
                    //
                    //ClassicColshape colShape = (ClassicColshape)Alt.CreateColShapeSphere(new Position(veh.posX, veh.posY, veh.posZ), 2.25f);
                    //colShape.ColshapeName = "Cardealer";
                    //colShape.CarDealerVehName = ServerVehicles.GetVehicleNameOnHash(veh.hash);
                    //colShape.CarDealerVehPrice = (long)veh.price;
                    //colShape.Radius = 2.25f;
                }
            }
            catch (Exception e) { Alt.Log($"{e}"); }
        }

        internal static void LoadAllServerJobs()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerJobs.ServerJobs_ = new List<Server_Jobs>(db.Server_Jobs);
                    Alt.Log($"{ServerJobs.ServerJobs_.Count} Server-Jobs wurden geladen.");
                }
            }
            catch (Exception e) { Alt.Log($"{e}"); }
        }

        internal static void LoadAllServerLicenses()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    CharactersLicenses.ServerLicenses_ = new List<Server_Licenses>(db.Server_Licenses);
                    Alt.Log($"{CharactersLicenses.ServerLicenses_.Count} Server-Licenses wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerShops()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerShops.ServerShops_ = new List<Server_Shops>(db.Server_Shops);
                    Alt.Log($"{ServerShops.ServerShops_.Count} Server-Shops wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerBarbers()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerBarbers.ServerBarbers_ = new List<Server_Barbers>(db.Server_Barbers);
                    Alt.Log($"{ServerBarbers.ServerBarbers_.Count} Server-Barbers wurden geladen.");
                }

                foreach (var barber in ServerBarbers.ServerBarbers_)
                {
                    var ServerBarberBlipData = new Server_Blips
                    {
                        name = "Friseur",
                        color = 0,
                        scale = 0.5f,
                        shortRange = true,
                        sprite = 71,
                        posX = barber.posX,
                        posY = barber.posY,
                        posZ = barber.posZ
                    };
                    ServerBlips.ServerBlips_.Add(ServerBarberBlipData);

                    var ServerBarberPedData = new Server_Peds
                    {
                        model = barber.pedModel,
                        posX = barber.pedX,
                        posY = barber.pedY,
                        posZ = barber.pedZ,
                        rotation = barber.pedRot
                    };
                    ServerPeds.ServerPeds_.Add(ServerBarberPedData);

                    //var ServerBarberMarkerData = new Server_Markers
                    //{
                    //    type = 27,
                    //    posX = barber.posX,
                    //    posY = barber.posY,
                    //    posZ = (float)(barber.posZ - 0.95),
                    //    scaleX = 1,
                    //    scaleY = 1,
                    //    scaleZ = 1,
                    //    red = 224,
                    //    green = 58,
                    //    blue = 58,
                    //    alpha = 150,
                    //    bobUpAndDown = false
                    //};
                    //ServerBlips.ServerMarkers_.Add(ServerBarberMarkerData);
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllCharacterInventorys()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    CharactersInventory.CharactersInventory_ = new List<Characters_Inventory>(db.Characters_Inventory);
                    Alt.Log($"{CharactersInventory.CharactersInventory_.Count} Charakter Inventar-Items wurden geladen.");
                }

                //Parallel.ForEach(ServerAllVehicles.ServerAllVehicles_, veh =>
                //{
                //    //HERE U CAN RUN/LOAD UR VEHICLE IN A PARRALEL WAY INSTEAD OF TASK
                //    //PARALLEL is an other forms of threading
                //});
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllCharacterLicenses()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    CharactersLicenses.CharactersLicenses_ = new List<Characters_Licenses>(db.Characters_Licenses);
                    Alt.Log($"{CharactersLicenses.CharactersLicenses_.Count} Character-Licenses wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllCharacterPermissions()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    Characters.CharactersPermissions = new List<Characters_Permissions>(db.Characters_Permissions);
                    Alt.Log($"{Characters.CharactersPermissions.Count} Character-Permissions wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllCharacterSkins()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    Characters.CharactersSkin = new List<Characters_Skin>(db.Characters_Skin);
                    Alt.Log($"{Characters.CharactersSkin.Count} Charakter-Skins wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllCharacterLastPositions()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    Characters.CharactersLastPos = new List<Characters_LastPos>(db.Characters_LastPos);
                    Alt.Log($"{Characters.CharactersLastPos.Count} Charakter Positionen wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerBlips()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerBlips.ServerBlips_ = new List<Server_Blips>(db.Server_Blips);
                    Alt.Log($"{ServerBlips.ServerBlips_.Count} Server-Blips wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerMarkers()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerBlips.ServerMarkers_ = new List<Server_Markers>(db.Server_Markers);
                    Alt.Log($"{ServerBlips.ServerMarkers_.Count} Server-Marker wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerPeds()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerPeds.ServerPeds_ = new List<Server_Peds>(db.Server_Peds);
                    Alt.Log($"{ServerPeds.ServerPeds_.Count} Server-Peds wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerVehiclesGlobal()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerAllVehicles.ServerAllVehicles_ = new List<Server_All_Vehicles>(db.Server_All_Vehicles);
                    Alt.Log($"{ServerAllVehicles.ServerAllVehicles_.Count} Server-All-Vehicles wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerAtMs()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerATM.ServerATM_ = new List<Server_ATM>(db.Server_ATM);
                    Alt.Log($"{ServerATM.ServerATM_.Count} Server-ATMs wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerAnimations()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerAnimations.ServerAnimations_ = new List<Server_Animations>(db.Server_Animations);
                    Alt.Log($"{ServerAnimations.ServerAnimations_.Count} Server-Animationen wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerMinijobBusdriverRouteSpots()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    foreach (var spot in db.Server_Minijob_Busdriver_Spots)
                    {
                        Minijobs.Busfahrer.Model.CreateMinijobRouteSpot(spot.id, spot.routeId, spot.spotId, new Position(spot.posX, spot.posY, spot.posZ));
                    }
                    Alt.Log($"{Minijobs.Busfahrer.Model.ServerMinijobBusdriverSpots_.Count} Server-Minijob-Busdriver-Spots wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerMinijobGarbageSpots()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    foreach (var spot in db.Server_Minijob_Garbage_Spots)
                    {
                        Minijobs.Müllmann.Model.CreateMinijobGarbageSpot(spot.id, spot.routeId, spot.spotId, new Position(spot.posX, spot.posY, spot.posZ));
                    }
                    Alt.Log($"{Minijobs.Müllmann.Model.ServerMinijobGarbageSpots_.Count} Server-Minijob-Garbage-Spots wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllVehicles()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    foreach (var veh in db.Server_Vehicles)
                    {
                        ServerVehicles.CreateServerVehicle(veh.id, veh.charid, (uint)(veh.hash), veh.vehType, veh.faction, veh.fuel, veh.KM, veh.engineState, veh.isEngineHealthy, true, veh.isInGarage, veh.garageId, new Position(veh.posX, veh.posY, veh.posZ), new Rotation(veh.rotX, veh.rotY, veh.rotZ), veh.plate, veh.lastUsage, veh.buyDate);
                    }
                    Alt.Log($"{ServerVehicles.ServerVehicles_.Count} Server-Vehicles wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllVehicleTrunkItems()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerVehicles.ServerVehicleTrunkItems_ = new List<Server_Vehicle_Items>(db.Server_Vehicle_Items);
                    Alt.Log($"{ServerVehicles.ServerVehicleTrunkItems_.Count} Server-Vehicle-Trunk-Items wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllVehicleMods()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    foreach (var m in db.Server_Vehicles_Mods)
                    {
                        ServerVehicles.AddVehicleModToList(m.id, m.vehId, m.colorPrimaryType, m.colorSecondaryType, m.spoiler, m.front_bumper, m.rear_bumper, m.side_skirt, m.exhaust, m.frame, m.grille, m.hood, m.fender, m.right_fender, m.roof, m.engine, m.brakes, m.transmission, m.horns, m.suspension, m.armor, m.turbo, m.xenon, m.wheel_type, m.wheels, m.wheelcolor, m.plate_holder, m.trim_design, m.ornaments, m.dial_design, m.steering_wheel, m.shift_lever, m.plaques, m.hydraulics, m.airfilter, m.window_tint, m.livery, m.plate, m.neon, m.neon_r, m.neon_g, m.neon_b, m.smoke_r, m.smoke_g, m.smoke_b, m.colorPearl, m.headlightColor, m.colorPrimary_r, m.colorPrimary_g, m.colorPrimary_b, m.colorSecondary_r, m.colorSecondary_g, m.colorSecondary_b, m.back_wheels, m.plate_vanity, m.door_interior, m.seats, m.rear_shelf, m.trunk, m.engine_block, m.strut_bar, m.arch_cover, m.antenna, m.exterior_parts, m.tank, m.rear_hydraulics, m.door, m.plate_color, m.interior_color, m.smoke);
                    }
                    Alt.Log($"{ServerVehicles.ServerVehiclesMod_.Count} Server-Vehicle-Mods wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllGarages()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerGarages.ServerGarages_ = new List<Server_Garages>(db.Server_Garages);
                    Alt.Log($"{ServerGarages.ServerGarages_.Count} Server-Garagen wurden geladen.");

                    foreach (var garage in ServerGarages.ServerGarages_)
                    {
                        if (garage.isBlipVisible)
                        {
                            string garageType = ""; int garageSprite = 0, garageColor = 0;
                            switch (garage.type)
                            {
                                case 0: garageType = "Garage"; garageSprite = 473; garageColor = 0; break;
                                case 1: garageType = "Bootsgarage"; garageSprite = 356; garageColor = 77; break;
                                case 2: garageType = "Flugzeuggarage"; garageSprite = 359; garageColor = 77; break;
                                case 3: garageType = "Helikoptergarage"; garageSprite = 360; garageColor = 77; break;
                            }

                            var ServerGarageBlipData = new Server_Blips
                            {
                                name = $"{garageType}: {garage.name}",
                                color = garageColor,
                                scale = 0.5f,
                                shortRange = true,
                                sprite = garageSprite,
                                posX = garage.posX,
                                posY = garage.posY,
                                posZ = garage.posZ
                            };
                            if (garage.id != 8 && garage.id != 9 && garage.id != 10 && garage.id != 11 && garage.id != 12 && garage.id != 13 && garage.id != 17 && garage.id != 18)
                            {
                                ServerBlips.ServerBlips_.Add(ServerGarageBlipData);
                            }
                        }

                        var ServerGaragePedData = new Server_Peds
                        {
                            model = "s_m_m_autoshop_02",
                            posX = garage.posX,
                            posY = garage.posY,
                            posZ = garage.posZ,
                            rotation = garage.rotation
                        };
                        ServerPeds.ServerPeds_.Add(ServerGaragePedData);
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllGarageSlots()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerGarages.ServerGarageSlots_ = new List<Server_Garage_Slots>(db.Server_Garage_Slots);
                    Alt.Log($"{ServerGarages.ServerGarageSlots_.Count} Server-Garagen Slots wurden geladen.");

                    foreach (var slot in ServerGarages.ServerGarageSlots_)
                    {
                        //var ServerGarageSlotMarkerData = new Server_Markers
                        //{
                        //    type = 30,
                        //    posX = slot.posX,
                        //    posY = slot.posY,
                        //    posZ = (float)(slot.posZ + 0.25),
                        //    scaleX = 1,
                        //    scaleY = 1,
                        //    scaleZ = 1,
                        //    red = 27,
                        //    green = 124,
                        //    blue = 227,
                        //    alpha = 75,
                        //    bobUpAndDown = false
                        //};
                        //ServerBlips.ServerMarkers_.Add(ServerGarageSlotMarkerData);
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllDroppedItems()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerDroppedItems.ServerDroppedItems_ = new List<Server_Dropped_Items>(db.Server_Dropped_Items);
                    Alt.Log($"{ServerDroppedItems.ServerDroppedItems_.Count} Server Dropped Items wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerATMs()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerATM.ServerATM_ = new List<Server_ATM>(db.Server_ATM);
                    Alt.Log($"{ServerATM.ServerATM_.Count} Server-ATMs wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerBanks()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerBanks.ServerBanks_ = new List<Server_Banks>(db.Server_Banks);
                    Alt.Log($"{ServerBanks.ServerBanks_.Count} Server-Banken wurden geladen.");
                    foreach (var bank in ServerBanks.ServerBanks_)
                    {
                        string bName = "Bank";
                        int bColor = 2,
                            bSprite = 500;
                        if (bank.zoneName == "Staatsbank") { bName = "Staatsbank"; bColor = 1; bSprite = 605; }
                        if (bank.zoneName != "Fraktion")
                        {
                            var ServerBankBlipData = new Server_Blips
                            {
                                name = bName,
                                color = bColor,
                                scale = 0.5f,
                                shortRange = true,
                                sprite = bSprite,
                                posX = bank.posX,
                                posY = bank.posY,
                                posZ = bank.posZ
                            };
                            ServerBlips.ServerBlips_.Add(ServerBankBlipData);
                        }

                        //var ServerBankMarkerData = new Server_Markers
                        //{
                        //    type = 27,
                        //    posX = bank.posX,
                        //    posY = bank.posY,
                        //    posZ = (float)(bank.posZ - 0.95),
                        //    scaleX = 1,
                        //    scaleY = 1,
                        //    scaleZ = 1,
                        //    red = 224,
                        //    green = 58,
                        //    blue = 58,
                        //    alpha = 150,
                        //    bobUpAndDown = false
                        //};
                        //ServerBlips.ServerMarkers_.Add(ServerBankMarkerData);
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerBankPapers()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerBankPapers.ServerBankPaper_ = new List<Server_Bank_Paper>(db.Server_Bank_Paper);
                    Alt.Log($"{ServerBankPapers.ServerBankPaper_.Count} Server-Bank-Papers geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerItems()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerItems.ServerItems_ = new List<Server_Items>(db.Server_Items);
                    Alt.Log($"{ServerItems.ServerItems_.Count} Server-Items wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllClothesShops()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerClothesShops.ServerClothesShops_ = new List<Server_Clothes_Shops>(db.Server_Clothes_Shops);
                    Alt.Log($"{ServerClothesShops.ServerClothesShops_.Count} Server-Clothes-Shops wurden geladen.");

                    var nonDistinct = new List<Server_Clothes_Shops_Items>(db.Server_Clothes_Shops_Items);
                    List<Server_Clothes_Shops_Items> distinct = nonDistinct.Distinct().ToList();

                    ServerClothesShops.ServerClothesShopsItems_ = new List<Server_Clothes_Shops_Items>(distinct);
                    Alt.Log($"{ServerClothesShops.ServerClothesShopsItems_.Count} Server-Clothes-Shop-Items wurden geladen.");
                }
                if (!clothesNPCSpawned)
                {

                    foreach (var cs in ServerClothesShops.ServerClothesShops_)
                    {
                        string blipName = "Kleiderladen";
                        int blipSprite = 73;
                        int blipColor = 0;
                        ServerBlips.ServerBlips_.Add(new Server_Blips
                        {
                            name = blipName,
                            color = blipColor,
                            scale = 0.5f,
                            sprite = blipSprite,
                            posX = cs.posX,
                            posY = cs.posY,
                            posZ = cs.posZ,
                            shortRange = true
                        });

                        ServerPeds.ServerPeds_.Add(new Server_Peds
                        {
                            model = $"{cs.pedModel}",
                            posX = cs.pedX,
                            posY = cs.pedY,
                            posZ = cs.pedZ,
                            rotation = cs.pedRot
                        });
                    }
                    clothesNPCSpawned = true;
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void LoadAllServerTeleports()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerItems.ServerTeleports_ = new List<Server_Teleports>(db.Server_Teleports);
                    Alt.Log($"{ServerItems.ServerTeleports_.Count} Server-Teleports wurden geladen.");

                    foreach (var teleport in ServerItems.ServerTeleports_)
                    {
                        var ServerTeleportsMarkerData = new Server_Markers
                        {
                            type = 27,
                            posX = teleport.posX,
                            posY = teleport.posY,
                            posZ = (float)(teleport.posZ - 0.95),
                            scaleX = 1,
                            scaleY = 1,
                            scaleZ = 1,
                            red = 224,
                            green = 58,
                            blue = 58,
                            alpha = 150,
                            bobUpAndDown = false
                        };
                        ServerBlips.ServerMarkers_.Add(ServerTeleportsMarkerData);
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void ParkVehicles()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    db.Accounts.ToList().ForEach(x => { x.Online = 0; });
                    db.SaveChanges();

                    foreach (var veh in db.Server_Vehicles)
                    {
                        if (!veh.isInGarage && DateTime.Now.Subtract(veh.lastUsage).TotalHours >= 96)
                        {
                            veh.isInGarage = true;
                            db.Server_Vehicles.Update(veh);
                        }

                        if (veh.vehType == 2 || veh.charid == 0)
                        {
                            var mod = ServerVehicles.ServerVehiclesMod_.FirstOrDefault(x => x.vehId == veh.id);
                            if (mod != null) { db.Server_Vehicles_Mods.Remove(mod); }
                            db.Server_Vehicles.Remove(veh);
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void RenewAll()
        {
            using (var db = new gtaContext())
            {
                Alt.Log($"License-Entrys vorher: {CharactersLicenses.CharactersLicenses_.Count}");
                Alt.Log($"TabletApp-Entrys vorher: {CharactersTablet.CharactersTabletApps_.Count}");
                foreach (var character in db.AccountsCharacters)
                {
                    if (!CharactersLicenses.ExistCharacterLicenseEntry(character.charId)) CharactersLicenses.CreateCharacterLicensesEntry(character.charId, false, false, false, false, false, false, false, false);
                    if (!CharactersTablet.ExistCharacterTabletAppEntry(character.charId)) CharactersTablet.CreateCharacterTabletAppEntry(character.charId, false, false, false, false, false, false, false, false);
                }
                Alt.Log($"----------------------------------------------------------------------");
                Alt.Log($"Characters insgesamt: {Characters.PlayerCharacters.Count}");
                Alt.Log($"License-Entrys nachher: {CharactersLicenses.CharactersLicenses_.Count}");
                Alt.Log($"TabletApp-Entrys nachher: {CharactersTablet.CharactersTabletApps_.Count}");
            }
        }
    }
}
