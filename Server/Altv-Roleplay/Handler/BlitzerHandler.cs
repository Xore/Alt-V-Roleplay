using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using Altv_Roleplay.Factories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Altv_Roleplay.Handler
{
    public partial class Server_Blitzer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string name { get; set; }
        public Position objectPos { get; set; }
        public Rotation objectRotation { get; set; }
        public Position colshapePos { get; set; }
        public float colshapeRadius { get; set; }
        public int speedLimit { get; set; }
    }

    public class BlitzerHandler : IScript
    {
        public static List<Server_Blitzer> ServerBlitzer_ = new List<Server_Blitzer>();

        public static void LoadBlitzer()
        {
            using (var db = new models.gtaContext())
            {
                ServerBlitzer_ = new List<Server_Blitzer>(db.Server_Blitzer);
                Alt.Log($"[SERVER] {ServerBlitzer_.Count()} Blitzer geladen");
            }

            foreach (Server_Blitzer blitzer in ServerBlitzer_.ToList())
            {
                ClassicColshape shape = (ClassicColshape)Alt.CreateColShapeSphere(blitzer.colshapePos, blitzer.colshapeRadius);
                shape.ColshapeId = blitzer.id;
                shape.ColshapeName = "Blitzer";
                shape.Radius = blitzer.colshapeRadius;
                EntityStreamer.PropStreamer.Create("prop_cctv_pole_04", blitzer.objectPos, blitzer.objectRotation, 0, placeObjectOnGroundProperly: true, frozen: true);
                // EntityStreamer.MarkerStreamer.Create(EntityStreamer.MarkerTypes.MarkerTypeVerticalCylinder, blitzer.colshapePos, new System.Numerics.Vector3(blitzer.colshapeRadius), color: new Rgba(150, 50, 50, 80));
            }
        }

        internal static void blitzerEntered(ClassicPlayer player, ulong blitzerId)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0 || blitzerId <= 0 || player.Vehicle == null || !player.IsInVehicle || player.Seat != 1) return;
                int speedLimit = GetSpeedLimit((int)blitzerId);
                player.Emit("Client:Blitzer:blitzerEntered", speedLimit, blitzerId);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [AsyncClientEvent("Server:Blitzer:giveTickets")]
        public void giveTickets(ClassicPlayer player, int vehicleSpeed, int blitzerId)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0 || player.Vehicle == null || !player.IsInVehicle || vehicleSpeed <= 0 || blitzerId <= 0) return;
                Server_Blitzer blitzer = ServerBlitzer_.ToList().FirstOrDefault(x => x.id == blitzerId);
                if (blitzer == null || vehicleSpeed <= blitzer.speedLimit) return;
                int difference = vehicleSpeed - blitzer.speedLimit;
                if (difference > 0 && difference < 26)
                {
                    // 1-25km/h Ticket
                    Model.CharactersWanteds.CreateCharacterWantedByName(player.CharacterId, "1-25km/h Geschwindigkeitsüberschreitung", "Blitzer");
                }
                else if (difference > 0 && difference < 51)
                {
                    // 25 - 50km/h Ticket
                    Model.CharactersWanteds.CreateCharacterWantedByName(player.CharacterId, "25-50km/h Geschwindigkeitsüberschreitung", "Blitzer");
                }
                else if (difference > 0 && difference < 100)
                {
                    // 50km/h - 99km/h
                    Model.CharactersWanteds.CreateCharacterWantedByName(player.CharacterId, "50-100km/h Geschwindigkeitsüberschreitung", "Blitzer");
                }
                else if (difference > 0 && difference > 100)
                {
                    // 100+ Ticket
                    Model.CharactersWanteds.CreateCharacterWantedByName(player.CharacterId, "100+ km/h Geschwindigkeitsüberschreitung", "Blitzer");
                }
                else return;
                HUDHandler.SendBetterNotif(player, 3, 10, "LSPD", $"Du bist {vehicleSpeed}km/h gefahren und wurdest geblitzt. Erlaubt: {blitzer.speedLimit - 10}km/h.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        #region Models
        public static int GetSpeedLimit(int blitzerId)
        {
            Server_Blitzer blitzer = ServerBlitzer_.ToList().FirstOrDefault(x => x.id == blitzerId);
            if (blitzer != null) return blitzer.speedLimit;
            return 0;
        }

        public static string GetName(int blitzerId)
        {
            Server_Blitzer blitzer = ServerBlitzer_.ToList().FirstOrDefault(x => x.id == blitzerId);
            if (blitzer != null) return blitzer.name;
            return "";
        }
        #endregion
    }
}
