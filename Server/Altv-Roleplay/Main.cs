using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Data;
using Altv_Roleplay.Model;
using Altv_Roleplay.Handler;
using System;
using System.Timers;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Net;
using System.IO;
using Altv_Roleplay.Utils;
using System.Linq;
using AltV.Net.Async;
using System.Threading.Tasks;
using Altv_Roleplay.Factories;
using Altv_Roleplay.models;
using System.Numerics;

namespace Altv_Roleplay
{
    public class Main : AsyncResource
    {
        public static int mainThreadId = Thread.CurrentThread.ManagedThreadId;
        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new AccountsFactory();
        }

        public override IBaseObjectFactory<IColShape> GetColShapeFactory()
        {
            return new ColshapeFactory();
        }

        public override IEntityFactory<IVehicle> GetVehicleFactory()
        {
            return new VehicleFactory();
        }

        public override void OnStart()
        {
            Environment.SetEnvironmentVariable("COMPlus_legacyCorruptedState­­ExceptionsPolicy", "1");

            //Datenbank laden
            Database.DatabaseHandler.ParkVehicles();
            Database.DatabaseHandler.LoadAllPlayers();
            Database.DatabaseHandler.LoadAllPlayerCharacters();
            Database.DatabaseHandler.LoadAllCharacterSkins();
            Database.DatabaseHandler.LoadAllCharacterBankAccounts();
            Database.DatabaseHandler.LoadAllCharacterLastPositions();
            Database.DatabaseHandler.LoadAllCharacterInventorys();
            Database.DatabaseHandler.LoadAllCharacterLicenses();
            Database.DatabaseHandler.LoadAllCharacterPermissions();
            Database.DatabaseHandler.LoadAllCharacterMinijobData();
            Database.DatabaseHandler.LoadAllCharacterPhoneChats();
            Database.DatabaseHandler.LoadAllCharacterWanteds();
            Database.DatabaseHandler.LoadAllServerBlips();
            Database.DatabaseHandler.LoadAllServerMarkers();
            Database.DatabaseHandler.LoadAllServerVehiclesGlobal();
            Database.DatabaseHandler.LoadAllServerAnimations();
            Database.DatabaseHandler.LoadAllServerAtMs();
            Database.DatabaseHandler.LoadAllServerBanks();
            Database.DatabaseHandler.LoadAllServerBankPapers();
            Database.DatabaseHandler.LoadAllServerItems();
            Database.DatabaseHandler.LoadAllServerPeds();
            Database.DatabaseHandler.LoadAllClothesShops();
            Database.DatabaseHandler.LoadAllServerShops();
            Database.DatabaseHandler.LoadAllServerShopItems();
            Database.DatabaseHandler.LoadAllServerBarbers();
            Database.DatabaseHandler.LoadAllServerTeleports();
            Database.DatabaseHandler.LoadAllGarages();
            Database.DatabaseHandler.LoadAllGarageSlots();
            Database.DatabaseHandler.LoadAllDroppedItems();
            Database.DatabaseHandler.LoadAllVehicleMods();
            Database.DatabaseHandler.LoadAllVehicles();
            Database.DatabaseHandler.LoadAllVehicleTrunkItems();
            Database.DatabaseHandler.LoadAllVehicleShops();
            Database.DatabaseHandler.LoadAllVehicleShopItems();
            Database.DatabaseHandler.LoadAllServerFarmingSpots();
            Database.DatabaseHandler.LoadAllServerFarmingProducers();
            Database.DatabaseHandler.LoadAllServerJobs();
            Database.DatabaseHandler.LoadAllServerLicenses();
            Database.DatabaseHandler.LoadAllServerFuelStations();
            Database.DatabaseHandler.LoadALlServerFuelStationSpots();
            Database.DatabaseHandler.LoadAllServerTabletAppData();
            Database.DatabaseHandler.LoadAllCharactersTabletApps();
            Database.DatabaseHandler.LoadAllServerTabletEvents();
            Database.DatabaseHandler.LoadAllServerTabletNotes();
            Database.DatabaseHandler.LoadAllServerFactions();
            Database.DatabaseHandler.LoadAllServerFactionRanks();
            Database.DatabaseHandler.LoadAllServerFactionMembers();
            Database.DatabaseHandler.LoadAllServerFactionStorageItems();
            Database.DatabaseHandler.LoadAllServerGangs();
            Database.DatabaseHandler.LoadAllServerGangRanks();
            Database.DatabaseHandler.LoadAllServerGangMembers();
            Database.DatabaseHandler.LoadAllServerGangStorageItems();
            Database.DatabaseHandler.LoadAllServerDoors();
            Database.DatabaseHandler.LoadAllServerHotels();
            Database.DatabaseHandler.LoadAllServerHouses();
            Database.DatabaseHandler.LoadAllServerMinijobBusdriverRoutes();
            Database.DatabaseHandler.LoadAllServerMinijobBusdriverRouteSpots();
            Database.DatabaseHandler.LoadAllServerMinijobGarbageSpots();
            Database.DatabaseHandler.LoadAllTattooStuff();
            Database.DatabaseHandler.LoadAllServerStorages();
            WeedPlantHandler.LoadAllWeedPots();
            ObjectHandler.LoadAllObjects();
            ClothesHelper.LoadClothesHelper();

            //Database.DatabaseHandler.RenewAll();

            Minijobs.Elektrolieferant.Main.Initialize();
            Minijobs.Müllmann.Main.Initialize();
            Minijobs.Busfahrer.Main.Initialize();
            //CarRental
            CarRental.Cayo.Main.Initialize();

            //Events registrieren
            Alt.OnColShape += ColAction;
            Alt.OnClient<IPlayer, string>("Server:Utilities:BanMe", banme);

            //Timer initialisieren
            System.Timers.Timer checkTimer = new System.Timers.Timer();
            System.Timers.Timer entityTimer = new System.Timers.Timer();
            System.Timers.Timer desireTimer = new System.Timers.Timer();
            System.Timers.Timer VehicleAutomaticParkFetchTimer = new System.Timers.Timer();
            checkTimer.Elapsed += new ElapsedEventHandler(TimerHandler.OnCheckTimer);
            entityTimer.Elapsed += new ElapsedEventHandler(TimerHandler.OnEntityTimer);
            desireTimer.Elapsed += new ElapsedEventHandler(TimerHandler.OnDesireTimer);
            VehicleAutomaticParkFetchTimer.Elapsed += new ElapsedEventHandler(TimerHandler.VehicleAutomaticParkFetch);
            checkTimer.Interval += 15000;
            entityTimer.Interval += 60000;
            desireTimer.Interval += 300000;
            VehicleAutomaticParkFetchTimer.Interval += 60000 * 5;
            checkTimer.Enabled = true;
            entityTimer.Enabled = true;
            desireTimer.Enabled = true;
            VehicleAutomaticParkFetchTimer.Enabled = true;

            // Dynasty8
            EntityStreamer.PedStreamer.Create("u_m_o_finguru_01", Constants.Positions.dynasty8_pedPositionShop, new System.Numerics.Vector3(0, 0, Constants.Positions.dynasty8_pedRotationShop), 0);
            EntityStreamer.MarkerStreamer.Create(EntityStreamer.MarkerTypes.MarkerTypeHorizontalSplitArrowCircle, Constants.Positions.dynasty8_positionShop, new System.Numerics.Vector3(1), color: new Rgba(255, 51, 51, 100), dimension: 0, streamRange: 20);
            EntityStreamer.HelpTextStreamer.Create("Drücke E um einen Shop zu erwerben oder deinen zu verkaufen.", Constants.Positions.dynasty8_positionShop, streamRange: 1);
            EntityStreamer.BlipStreamer.CreateStaticBlip("Dynasty8", 2, 0.5f, true, 642, Constants.Positions.dynasty8_pedPositionShop, 0);

            EntityStreamer.PedStreamer.Create("u_m_o_finguru_01", Constants.Positions.dynasty8_pedPositionStorage, new System.Numerics.Vector3(0, 0, Constants.Positions.dynasty8_pedRotationStorage), 0);
            EntityStreamer.MarkerStreamer.Create(EntityStreamer.MarkerTypes.MarkerTypeHorizontalSplitArrowCircle, Constants.Positions.dynasty8_positionStorage, new System.Numerics.Vector3(1), color: new Rgba(255, 51, 51, 100), dimension: 0, streamRange: 20);
            EntityStreamer.HelpTextStreamer.Create("Drücke E um eine Lagerhale zu erwerben oder deinen zu verkaufen.", Constants.Positions.dynasty8_positionStorage, streamRange: 1);

            // Shops
            foreach (var shop in ServerShops.ServerShops_)
            {
                if (shop.isBlipVisible)
                {
                    EntityStreamer.PedStreamer.Create(shop.pedModel, shop.pedPos, new System.Numerics.Vector3(0, 0, shop.pedRot), 0);
                    //if (shop.type == 0 || shop.type == 1 || shop.faction > 0) continue;
                    EntityStreamer.MarkerStreamer.Create(EntityStreamer.MarkerTypes.MarkerTypeHorizontalSplitArrowCircle, shop.managePos, new Vector3(1), color: new Rgba(255, 80, 80, 150), streamRange: 15);
                    EntityStreamer.BlipStreamer.CreateStaticBlip(shop.name, shop.blipColor, 0.5f, true, shop.blipSprite, shop.shopPos, 0);
                }
                else if (shop.isBlipVisible)
                {
                    EntityStreamer.PedStreamer.Create(shop.pedModel, shop.pedPos, new System.Numerics.Vector3(0, 0, shop.pedRot), 0);
                    //if (shop.type == 0 || shop.type == 1 || shop.faction > 0) continue;
                    EntityStreamer.MarkerStreamer.Create(EntityStreamer.MarkerTypes.MarkerTypeHorizontalSplitArrowCircle, shop.managePos, new Vector3(1), color: new Rgba(255, 80, 80, 150), streamRange: 15);
                    //EntityStreamer.BlipStreamer.CreateStaticBlip(shop.name, shop.blipColor, 0.8f, true, shop.blipSprite, shop.shopPos, 0);
                }
            }

            // Storage Rooms
            foreach (var storage in ServerStorages.ServerStorages_)
            {
                if (ServerStorages.GetOwner(storage.id) == 0)
                {
                    EntityStreamer.MarkerStreamer.Create(EntityStreamer.MarkerTypes.MarkerTypeVerticalCylinder, new Vector3(storage.entryPos.X, storage.entryPos.Y, storage.entryPos.Z - 1), new Vector3(1), color: new Rgba(255, 51, 51, 100), streamRange: 50);
                    EntityStreamer.HelpTextStreamer.Create("Du kannst diese Lagerhalle bei Dynasty-8 Erwerben.", storage.entryPos, streamRange: 2);
                    EntityStreamer.BlipStreamer.CreateStaticBlip("Freie Lagerhalle", 2, 0.25f, true, 50, storage.entryPos, 0);
                }
                else if (ServerStorages.GetOwner(storage.id) != 0)
                {
                    EntityStreamer.MarkerStreamer.Create(EntityStreamer.MarkerTypes.MarkerTypeVerticalCylinder, new Vector3(storage.entryPos.X, storage.entryPos.Y, storage.entryPos.Z - 1), new Vector3(1), color: new Rgba(255, 51, 51, 100), streamRange: 50);
                    EntityStreamer.HelpTextStreamer.Create("Die Lagerhalle wurde bereits Verkauft. ('E' um die Lagerhalle zu betreten. 'U' zum öffnen/schließen.)", storage.entryPos, streamRange: 2);
                    EntityStreamer.BlipStreamer.CreateStaticBlip("Lagerhalle", 0, 0.25f, true, 50, storage.entryPos, 0);
                }
            }

            // MD Wheelchairs
            //ServerVehicles.CreateVehicle(3116946227, 0, 2, 3, false, 0, new Position((float)222.989, (float)-801.046, (float)30.5436), new Rotation(0, 0, (float)0.44526514), $"LSFD", 255, 255, 255);

            Console.WriteLine($"Main-Thread = {Thread.CurrentThread.ManagedThreadId}");
        }

        private void banme(IPlayer player, string msg)
        {
            try
            {
                if (player == null || !player.Exists || player.AdminLevel() != 0) return;
                Alt.Log($"Ban Me: {player.Name} - {DateTime.Now.ToString()}");
                int charId = User.GetPlayerOnline(player);
                player.Kick("");
                if (charId <= 0) return;
                User.SetPlayerBanned(Characters.GetCharacterAccountId(charId), true, $"Grund: {msg}");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        private void ColAction(IColShape colShape, IEntity targetEntity, bool state)
        {
            if (colShape == null) return;
            if (!colShape.Exists) return;
            IPlayer client = targetEntity as IPlayer;
            if (client == null || !client.Exists) return;
            string colshapeName = colShape.GetColShapeName();
            long colshapeId = colShape.GetColShapeId();

            if(colshapeName == "Cardealer" && state == true)
            {
                long vehprice = colShape.GetColshapeCarDealerVehPrice();
                string vehname = colShape.GetColshapeCarDealerVehName();
                HUDHandler.SendNotification(client, 1, 2500, $"Name: {vehname}<br>Preis: {vehprice}$");
                return;
            }
            else if(colshapeName == "DoorShape" && state)
            {
                var doorData = ServerDoors.ServerDoors_.FirstOrDefault(x => x.id == (int)colshapeId);
                if (doorData == null) return;
                client.EmitLocked("Client:DoorManager:ManageDoor", doorData.hash, new Position(doorData.posX, doorData.posY, doorData.posZ), (bool)doorData.state);
            }
        }   

        public override void OnStop()
        {
            foreach (var player in Alt.GetAllPlayers().Where(p => p != null && p.Exists)) player.Kick("Server wird Neugestartet...");
            Alt.Log("Server ist gestoppt.");
        }        
    }
}
