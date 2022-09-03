using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Handler;
using Altv_Roleplay.Model;
using Altv_Roleplay.models;
using Altv_Roleplay.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Altv_Roleplay.CarRental.Cayo
{
    public class Main : IScript
    {
        public partial class Minijob_Spots
        {
            public int id { get; set; }
            public IColShape depositShape { get; set; }
        }
        public static List<Minijob_Spots> MinijobSpots_ = new List<Minijob_Spots>();
        public static ClassicColshape startJobShape = (ClassicColshape)Alt.CreateColShapeSphere(Constants.Positions.CarRental_StartPos, 2f);

        public static void Initialize()
        {
            Alt.OnPlayerEnterVehicle += PlayerEnterVehicle;
            Alt.OnPlayerLeaveVehicle += PlayerExitVehicle;
            Alt.OnPlayerDisconnect += PlayerDisconnectedHandler;

            var data = new Server_Peds { model = "s_m_y_xmech_02_mp", posX = startJobShape.Position.X, posY = startJobShape.Position.Y, posZ = startJobShape.Position.Z - 1, rotation = 19f };
            ServerPeds.ServerPeds_.Add(data);

            startJobShape.Radius = 2f;


            foreach (var item in MinijobSpots_)
            {
                ((ClassicColshape)item.depositShape).Radius = 2.5f;
            }
        }

        private static void PlayerDisconnectedHandler(IPlayer player, string reason)
        {
            try
            {
                if (player == null || !player.Exists) return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                foreach (var veh in Alt.GetAllVehicles().Where(x => x.NumberplateText == $"RENT-{charId}").ToList())
                {
                    if (veh == null || !veh.Exists) continue;
                    ServerVehicles.RemoveVehiclePermanently(veh);
                    veh.Remove();
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        private static void PlayerExitVehicle(IVehicle vehicle, IPlayer player, byte seat)
        {
            try
            {
                if (player == null || vehicle == null || !player.Exists || !vehicle.Exists) return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                if (ServerVehicles.GetVehicleType(vehicle) != 2) return;
                if (ServerVehicles.GetVehicleOwner(vehicle) != charId) return;
                if (player.GetPlayerCurrentMinijob() == "None") return;
                if (player.GetPlayerCurrentMinijobStep() == "None") return;
                if (player.GetPlayerCurrentMinijob() != "Rent") return;
                if (player.GetPlayerCurrentMinijobStep() == "DRIVE_BACK_TO_START" && vehicle.Position.IsInRange(Constants.Positions.CarRental_VehOutPos, 10f))
                {
                    var model = vehicle.Model;
                    foreach (var veh in Alt.GetAllVehicles().Where(x => x.NumberplateText == $"RENT-{charId}").ToList())
                    {
                        if (veh == null || !veh.Exists) continue;
                        ServerVehicles.RemoveVehiclePermanently(veh);
                        veh.Remove();
                    }
                    player.SetPlayerCurrentMinijob("None");
                    player.SetPlayerCurrentMinijobRouteId(0);
                    player.SetPlayerCurrentMinijobStep("None");
                    player.SetPlayerCurrentMinijobActionCount(0);
                    //int rnd = 0;
                    //int rndExp = 0;
                    switch (model)
                    {
                        case 4084658662: //Winky
                            break;
                        case 4173521127: //Kamacho
                            break;
                        case 1802742206: //YougaC
                            break;
                    }
                    player.EmitLocked("Client:Minijob:RemoveJobMarker");
                    return;
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        private static void PlayerEnterVehicle(IVehicle vehicle, IPlayer player, byte seat)
        {
            try
            {
                if (player == null || vehicle == null || !player.Exists || !vehicle.Exists) return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                if (ServerVehicles.GetVehicleType(vehicle) != 2) return;
                if (ServerVehicles.GetVehicleOwner(vehicle) != charId) return;
                if (player.GetPlayerCurrentMinijob() == "None") return;
                if (player.GetPlayerCurrentMinijobStep() == "None") return;
                if (player.GetPlayerCurrentMinijob() != "Rent") return;
                if (player.GetPlayerCurrentMinijobStep() == "FirstStepInVehicle")
                {
                    player.SetPlayerCurrentMinijobActionCount(1);
                    return;
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:MinijobPilot:StartJob")]
        public void StartMiniJob(IPlayer player, int level)
        {
            try
            {
                if (player == null || !player.Exists || level <= 0) return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                if (player.GetPlayerCurrentMinijob() != "None") return;
                int serialNumber = new Random().Next(1, 10000);
                foreach (var veh in Alt.GetAllVehicles().ToList())
                {
                    if (veh == null || !veh.Exists) continue;
                    if (veh.Position.IsInRange(Constants.Positions.CarRental_VehOutPos, 8f)) { HUDHandler.SendNotification(player, 3, 2500, "Ausparkpunkt Blockiert."); return; }
                }
                switch (level)
                {
                    case 1:
                        if (CharactersInventory.GetCharacterItemAmount(charId, "Bargeld", "brieftasche") <= 250) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht genug Bargeld! 250$"); return; }
                        CharactersInventory.RemoveCharacterItemAmount(charId, "Bargeld", 250, "brieftasche");
                        ServerVehicles.CreateVehicle(4084658662, charId, 2, 0, false, 0, Constants.Positions.CarRental_VehOutPos, Constants.Positions.CarRental_VehOutRot, $"RENT-{charId}", 255, 255, 255, 0, serialNumber);
                        break;
                    case 2:
                        if (CharactersInventory.GetCharacterItemAmount(charId, "Bargeld", "brieftasche") <= 350) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht genug Bargeld! 350$"); return; }
                        CharactersInventory.RemoveCharacterItemAmount(charId, "Bargeld", 350, "brieftasche");
                        ServerVehicles.CreateVehicle(4173521127, charId, 2, 0, false, 0, Constants.Positions.CarRental_VehOutPos, Constants.Positions.CarRental_VehOutRot, $"RENT-{charId}", 255, 255, 255, 0, serialNumber);
                        break;
                    case 3:
                        if (CharactersInventory.GetCharacterItemAmount(charId, "Bargeld", "brieftasche") <= 550) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht genug Bargeld! 550$"); return; }
                        CharactersInventory.RemoveCharacterItemAmount(charId, "Bargeld", 550, "brieftasche");
                        ServerVehicles.CreateVehicle(1802742206, charId, 2, 0, false, 0, Constants.Positions.CarRental_VehOutPos, Constants.Positions.CarRental_VehOutRot, $"RENT-{charId}", 255, 255, 255, 0, serialNumber);
                        break;
                }
                player.SetPlayerCurrentMinijob("Rent");
                player.SetPlayerCurrentMinijobStep("FirstStepInVehicle");
                player.SetPlayerCurrentMinijobActionCount(0);
                player.EmitLocked("Client:Minijob:RemoveJobMarker");
                HUDHandler.SendNotification(player, 2, 2500, "Fahrzeug gemietet");
                return;
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void TryStartMinijob(IPlayer player)
        {
            try
            {
                if (player == null || !player.Exists || !((ClassicColshape)startJobShape).IsInRange((ClassicPlayer)player)) return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                if (player.GetPlayerCurrentMinijob() == "Rent")
                {
                    //Job abbrechen
                    foreach (var veh in Alt.GetAllVehicles().Where(x => x.NumberplateText == $"RENT-{charId}").ToList())
                    {
                        if (veh == null || !veh.Exists) continue;
                        ServerVehicles.RemoveVehiclePermanently(veh);
                        veh.Remove();
                    }
                    HUDHandler.SendNotification(player, 1, 2500, "Du hast das Fahrzeug zurückgegeben");
                    player.SetPlayerCurrentMinijob("None");
                    player.SetPlayerCurrentMinijobRouteId(0);
                    player.SetPlayerCurrentMinijobStep("None");
                    player.SetPlayerCurrentMinijobActionCount(0);
                    return;
                }
                else if (player.GetPlayerCurrentMinijob() == "None")
                {
                    //Levelauswahl anzeigen
                    if (!CharactersMinijobs.ExistCharacterMinijobEntry(charId, "Rent"))
                    {
                        CharactersMinijobs.CreateCharacterMinijobEntry(charId, "Rent");
                    }
                    player.EmitLocked("Client:MinijobPilot:openCEF");
                    return;
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
    }
}
