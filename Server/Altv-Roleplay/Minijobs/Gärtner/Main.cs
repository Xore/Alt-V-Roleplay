using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Handler;
using Altv_Roleplay.Model;
using Altv_Roleplay.models;
using Altv_Roleplay.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Altv_Roleplay.Minijobs.Gärtner
{
    public partial class Gärtner_Pos
    {
        public int id { get; set; }
        public Position pos { get; set; }
        public IColShape colshape { get; set; } = null;
    }

    class Main : IScript
    {
        public static List<Gärtner_Pos> Spots_ = new List<Gärtner_Pos>() {
            new Gärtner_Pos { id = 1, pos = new Position(-1320.0923f, 38.202198f, 52.504395f)},
            new Gärtner_Pos { id = 2, pos = new Position(-1308.2241f, 30.67253f, 51.864136f)},
            new Gärtner_Pos { id = 3, pos = new Position(-1293.9165f, 28.852749f, 50.60034f)},
            new Gärtner_Pos { id = 4, pos = new Position(-1276.3649f, 27.468134f, 47.93811f)},
            new Gärtner_Pos { id = 5, pos = new Position(-1255.7935f, 22.931868f, 47.06189f)},
            new Gärtner_Pos { id = 6, pos = new Position(-1240.1538f, 12.553846f, 46.640625f)},
            new Gärtner_Pos { id = 7, pos = new Position(-1224.2902f, 2.967033f, 46.775513f)},
            new Gärtner_Pos { id = 8, pos = new Position(-1207.9121f, -4.7208786f, 46.62378f)},
            new Gärtner_Pos { id = 9, pos = new Position(-1190.0835f, -13.687912f, 45.815063f)},
            new Gärtner_Pos { id = 10, pos = new Position(-1174.8132f, -24.474724f, 44.719727f)},
            new Gärtner_Pos { id = 11, pos = new Position(-1158.5934f, -37.054947f, 44.248047f)},
            new Gärtner_Pos { id = 12, pos = new Position(-1142.3605f, -50.10989f, 43.99524f)},
            new Gärtner_Pos { id = 13, pos = new Position(-1133.4857f, -42.83077f, 44.70288f)},
            new Gärtner_Pos { id = 14, pos = new Position(-1136.0967f, -26.88791f, 46.590088f)},
            new Gärtner_Pos { id = 15, pos = new Position(-1143.0593f, -8.953846f, 47.365234f)},
            new Gärtner_Pos { id = 16, pos = new Position(-1151.5385f, 5.5384617f, 47.988647f)},
            new Gärtner_Pos { id = 17, pos = new Position(-1158.9495f, 16.734066f, 49.235474f)},
            new Gärtner_Pos { id = 18, pos = new Position(-1167.7715f, 28.958242f, 50.01062f)},
            new Gärtner_Pos { id = 19, pos = new Position(-1179.1384f, 41.485714f, 51.341797f)},
            new Gärtner_Pos { id = 20, pos = new Position(-1194.2109f, 49.767033f, 52.31909f)},
            new Gärtner_Pos { id = 21, pos = new Position(-1216.1802f, 58.035164f, 51.695557f)},
            new Gärtner_Pos { id = 22, pos = new Position(-1236.712f, 65.18242f, 51.577637f)},
            new Gärtner_Pos { id = 23, pos = new Position(-1254.0923f, 61.107693f, 50.280273f)},
            new Gärtner_Pos { id = 24, pos = new Position(-1274.4396f, 53.564835f, 50.12854f)},
            new Gärtner_Pos { id = 25, pos = new Position(-1290.8572f, 49.31868f, 50.313965f)},
            new Gärtner_Pos { id = 26, pos = new Position(-1306.2593f, 47.604397f, 51.628174f)},
            new Gärtner_Pos { id = 27, pos = new Position(-1316.7693f, 44.057144f, 52.504395f)},
        };
        public static readonly Position startMarkerPos = new Position(-1333.56f, 38.492f, 52.594f);
        public static readonly Position vehOutPos = new Position(-1327.43f, 37.094f, 52.987f);
        public static readonly Rotation vehOutRot = new Rotation(0f, 0f, -1.534f);
        public static ClassicColshape startJobShape = (ClassicColshape)Alt.CreateColShapeSphere(startMarkerPos, 2f);

        public static void Initiliaze()
        {
            Alt.Log("[SERVER] Lade Minijob: Gärtner");
            Alt.OnColShape += ColshapeEnterExitHandler;
            Alt.OnPlayerEnterVehicle += PlayerEnterVehicle;
            Alt.OnPlayerLeaveVehicle += PlayerExitVehicle;

            //EntityStreamer.PedStreamer.Create("a_f_o_ktown_01", new Position(-1333.6615f, 39.375f, 52.594f), new System.Numerics.Vector3(0, 0, -173.3897f), 0);
            var data = new Server_Peds { model = "a_f_o_ktown_01", posX = -1333.6615f, posY = 39.375f, posZ = 52.594f, rotation = -173.3897f };
            ServerPeds.ServerPeds_.Add(data);

            EntityStreamer.MarkerStreamer.Create(EntityStreamer.MarkerTypes.MarkerTypeVerticalCylinder, startMarkerPos, new System.Numerics.Vector3(1), color: new Rgba(150, 50, 50, 150), dimension: 0);

            EntityStreamer.BlipStreamer.CreateStaticBlip("Gärtnerei", 0, 0.7f, true, 33, startMarkerPos, 0);
            startJobShape.Radius = 2f;
            Alt.OnPlayerDisconnect += PlayerDisconnectedHandler;

            foreach (Gärtner_Pos spot in Spots_)
                spot.colshape = Alt.CreateColShapeSphere(spot.pos, 2f);
        }

        private static void PlayerDisconnectedHandler(IPlayer player, string reason)
        {
            try
            {
                if (player == null || !player.Exists) return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                foreach (var veh in Alt.GetAllVehicles().Where(x => x.NumberplateText == $"GN-{charId}").ToList())
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

        private static async void PlayerExitVehicle(IVehicle vehicle, IPlayer player, byte seat)
        {
            try
            {
                if (player == null || !player.Exists || ((ClassicPlayer)player).CharacterId <= 0 || vehicle == null || !vehicle.Exists || ((ClassicVehicle)vehicle).VehicleId <= 0) return;
                if (player.GetPlayerCurrentMinijob() != "Gärtner" || player.GetPlayerCurrentMinijobStep() != "DRIVE_BACK_TO_START" || ServerVehicles.GetVehicleType(vehicle) != 2 || ServerVehicles.GetVehicleOwner(vehicle) != ((ClassicPlayer)player).CharacterId || !vehicle.Position.IsInRange(vehOutPos, 5f)) return;
                player.Emit("Client:Minijob:RemoveJobMarker");
                foreach (ClassicVehicle veh in Alt.GetAllVehicles().Cast<ClassicVehicle>().Where(x => x != null && x.Exists && x.NumberplateText == $"GN-{((ClassicPlayer)player).CharacterId}").ToList())
                {
                    if (veh == null || !veh.Exists) continue;
                    ServerVehicles.RemoveVehiclePermanently(veh);
                    await Task.Delay(5000);
                    veh.Remove();
                }

                int givenMoney = new Random().Next(500, 1500);
                int charId = User.GetPlayerOnline(player);
                player.SetPlayerCurrentMinijob("None");
                player.SetPlayerCurrentMinijobStep("None");
                player.SetPlayerCurrentMinijobActionCount(0);
                player.SetPlayerCurrentMinijobRouteId(0);
                CharactersInventory.AddCharacterItem(charId, "Bargeld", givenMoney, "brieftasche");
                HUDHandler.SendBetterNotif(player, 0, 10, "Gärtnerei", $"Du hast den Job erfolgreich abgeschlossen. Hier hast du {givenMoney}$!");
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        private static void ColshapeEnterExitHandler(IColShape colShape, IEntity targetEntity, bool state)
        {
            try
            {
                if (colShape == null || !colShape.Exists) return;
                ClassicPlayer player = targetEntity as ClassicPlayer;
                if (player == null || !player.Exists || player.CharacterId <= 0) return;
                if (colShape == startJobShape && state)
                {
                    if (player.GetPlayerCurrentMinijob() == "Gärtner") { HUDHandler.SendBetterNotif(player, 0, 10, "Gärtnerei", "Drücke E um den Gärtner Minijob zu beenden."); }
                    else if (player.GetPlayerCurrentMinijob() == "None") { HUDHandler.SendBetterNotif(player, 0, 10, "Gärtnerei", "Drücke E um den Gärtner Minijob zu starten."); }
                    else if (player.GetPlayerCurrentMinijob() != "None") { HUDHandler.SendBetterNotif(player, 0, 10, "Gärtnerei", "Du bist bereits in einem Minijob."); }
                    return;
                }

                if (player.GetPlayerCurrentMinijob() != "Gärtner" || player.GetPlayerCurrentMinijobActionCount() <= 0) return;
                if (player.GetPlayerCurrentMinijobStep() == "DRIVE_TO_NEXT_SPOT" && state && player.IsInVehicle)
                {
                    Gärtner_Pos spot = Spots_.ToList().FirstOrDefault(x => x.id == (int)player.GetPlayerCurrentMinijobActionCount());
                    if (spot == null || colShape != spot.colshape) return;
                    player.Emit("Client:Minijob:RemoveJobMarker");
                    if ((int)player.GetPlayerCurrentMinijobActionCount() < Spots_.ToList().Count())
                    {
                        // neuer Punkt
                        player.SetPlayerCurrentMinijobActionCount(player.GetPlayerCurrentMinijobActionCount() + 1);
                        Gärtner_Pos newSpot = Spots_.ToList().FirstOrDefault(x => x.id == (int)player.GetPlayerCurrentMinijobActionCount());
                        if (newSpot == null) return;
                        player.Emit("Client:Minijob:CreateJobMarker", "Minijob: Spot", 3, 515, 1, newSpot.pos.X, newSpot.pos.Y, newSpot.pos.Z, false);
                        HUDHandler.SendBetterNotif(player, 0, 10, "Gärtnerei", "An Spot angekommen, fahre nun zum nächsten Spot.");
                        return;
                    }
                    else if ((int)player.GetPlayerCurrentMinijobActionCount() >= Spots_.ToList().Count())
                    {
                        // Zurück zum Anfang.
                        HUDHandler.SendBetterNotif(player, 0, 10, "Gärtnerei", "An letzten Spot angekommen, fahre den Rasenmäher nun zurück zum Startpunkt und stelle ihn dort ab, wo du ihn bekommen hast.");
                        player.SetPlayerCurrentMinijobStep("DRIVE_BACK_TO_START");
                        player.Emit("Client:Minijob:CreateJobMarker", "Minijob: Fahrzeugabgabe", 3, 634, 1, vehOutPos.X, vehOutPos.Y, vehOutPos.Z - 0.5f, false);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        private static void PlayerEnterVehicle(IVehicle vehicle, IPlayer player, byte seat)
        {
            try
            {
                if (vehicle == null || !vehicle.Exists || ((ClassicVehicle)vehicle).VehicleId <= 0 || player == null || !player.Exists || ((ClassicPlayer)player).CharacterId <= 0) return;
                if (player.GetPlayerCurrentMinijob() != "Gärtner" || player.GetPlayerCurrentMinijobStep() != "FirstStepInVehicle" || ServerVehicles.GetVehicleType(vehicle) != 2 || ServerVehicles.GetVehicleOwner(vehicle) != ((ClassicPlayer)player).CharacterId) return;
                Gärtner_Pos spot = Spots_.ToList().FirstOrDefault(x => x.id == (int)player.GetPlayerCurrentMinijobActionCount());
                if (spot == null) return;
                player.SetPlayerCurrentMinijobStep("DRIVE_TO_NEXT_SPOT");
                player.Emit("Client:Minijob:CreateJobMarker", "Minijob: Spot", 3, 515, 1, spot.pos.X, spot.pos.Y, spot.pos.Z, false);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        internal static void StartMinijob(ClassicPlayer player)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0 || !(startJobShape).IsInRange(player)) return;
                if (player.GetPlayerCurrentMinijob() == "Gärtner")
                {
                    // Abbrechen
                    foreach (ClassicVehicle veh in Alt.GetAllVehicles().Cast<ClassicVehicle>().Where(x => x.NumberplateText == $"GN-{player.CharacterId}").ToList())
                    {
                        if (veh == null || !veh.Exists) continue;
                        ServerVehicles.RemoveVehiclePermanently(veh);
                        veh.Remove();
                    }

                    HUDHandler.SendBetterNotif(player, 0, 10, "Gärtnerei", "Du hast den Job beendet.");
                    player.SetPlayerCurrentMinijob("None");
                    player.SetPlayerCurrentMinijobRouteId(0);
                    player.SetPlayerCurrentMinijobStep("None");
                    player.SetPlayerCurrentMinijobActionCount(0);
                    player.Emit("Client:Minijob:RemoveJobMarker");
                    return;
                }
                else if (player.GetPlayerCurrentMinijob() == "None")
                {
                    // Starten
                    foreach (ClassicVehicle veh in Alt.GetAllVehicles().Cast<ClassicVehicle>().ToList())
                    {
                        if (veh == null || !veh.Exists) continue;
                        if (veh.Position.IsInRange(vehOutPos, 5f)) { HUDHandler.SendBetterNotif(player, 0, 10, "Gärtnerei", "Der Ausparkpunkt ist blockiert."); return; }
                    }
                    int serialNumber = new Random().Next(1, 10000);
                    ServerVehicles.CreateVehicle(1783355638, player.CharacterId, 2, 0, false, 0, vehOutPos, vehOutRot, $"GN-{player.CharacterId}", 255, 255, 255, 0, serialNumber, false, DateTime.Now, false);
                    player.SetPlayerCurrentMinijob("Gärtner");
                    player.SetPlayerCurrentMinijobStep("FirstStepInVehicle");
                    player.SetPlayerCurrentMinijobActionCount(1);
                    HUDHandler.SendBetterNotif(player, 0, 10, "Gärtnerei", "Du hast den Job begonnen. Wir haben dir einen Rasenmäher ausgeparkt, steige ein.");
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}