using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Altv_Roleplay.Handler
{
    #region models
    public partial class Server_Car_Rentals
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string name { get; set; }
        public Position pedPos { get; set; }
        public float pedRot { get; set; }
        public Position parkOutPos { get; set; }
        public Rotation parkOutRot { get; set; }
    }

    public partial class Server_Car_Rentals_Vehicles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int carRentalId { get; set; }
        public long hash { get; set; }
        public int pricePerDay { get; set; }
        public Position vehPos { get; set; }
        public float vehRot { get; set; }
    }
    #endregion

    public class CarRentalHandler : IScript
    {
        public static List<Server_Car_Rentals> ServerCarRentals_ = new List<Server_Car_Rentals>();
        public static List<Server_Car_Rentals_Vehicles> ServerCarRentalsVehicles_ = new List<Server_Car_Rentals_Vehicles>();

        public static void Load()
        {
            using (var db = new models.gtaContext())
            {
                ServerCarRentals_ = new List<Server_Car_Rentals>(db.Server_Car_Rentals);
                ServerCarRentalsVehicles_ = new List<Server_Car_Rentals_Vehicles>(db.Server_Car_Rentals_Vehicles);
                Alt.Log($"[SERVER] {ServerCarRentals_.Count} Fahrzeugvermietungen geladen");
                Alt.Log($"[SERVER] {ServerCarRentalsVehicles_.Count} Fahrzeugvermietungen-Fahrzeuge geladen");
            }

            foreach (Server_Car_Rentals rental in ServerCarRentals_.ToList())
            {
                EntityStreamer.BlipStreamer.CreateStaticBlip("Fahrzeugvermietung", 9, 0.75f, true, 225, rental.pedPos, 0);
                EntityStreamer.PedStreamer.Create("ig_car3guy1", rental.pedPos, new System.Numerics.Vector3(0, 0, rental.pedRot), 0);
            }

            foreach (Server_Car_Rentals_Vehicles rentalVeh in ServerCarRentalsVehicles_.ToList())
            {
                IVehicle veh = Alt.CreateVehicle((uint)rentalVeh.hash, rentalVeh.vehPos, new Rotation(0, 0, rentalVeh.vehRot));
                veh.LockState = AltV.Net.Enums.VehicleLockState.Locked;
                veh.EngineOn = false;
                veh.NumberplateText = "CARRENTAL";
                veh.SetStreamSyncedMetaData("IsVehicleCardealer", true);

                ClassicColshape colShape = (ClassicColshape)Alt.CreateColShapeSphere(rentalVeh.vehPos, 2.25f);
                colShape.ColshapeName = "Carrental";
                colShape.CarDealerVehName = ServerVehicles.GetVehicleNameOnHash(rentalVeh.hash);
                colShape.CarDealerVehPrice = rentalVeh.pricePerDay;
                colShape.Radius = 2.25f;
            }
        }

        [ClientEvent("Server:CarRental:rentVehicle")]
        public void rentVehicle(ClassicPlayer player, int rentalId, string hash, int tage)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0 || rentalId <= 0 || hash.Length <= 0 || tage <= 0) return;
                long fHash = Convert.ToInt64(hash);
                if (fHash <= 0) return;
                Server_Car_Rentals rental = ServerCarRentals_.ToList().FirstOrDefault(x => x.id == rentalId);
                if (rental == null) return;
                bool PlaceFree = true;
                foreach (IVehicle veh in Alt.GetAllVehicles().ToList()) { if (veh.Position.IsInRange(rental.parkOutPos, 2f)) { PlaceFree = false; break; } }
                if (!PlaceFree) { HUDHandler.SendBetterNotif(player, 3, 10, "Auto Vermietung", $"Der Ausparkpunkt ist belegt."); return; }
                Server_Car_Rentals_Vehicles rentalVeh = ServerCarRentalsVehicles_.ToList().FirstOrDefault(x => x.carRentalId == rentalId && x.hash == fHash);
                if (rentalVeh == null) return;
                int rnd = new Random().Next(100000, 999999);
                if (ServerVehicles.ExistServerVehiclePlate($"LS{rnd}")) { rentVehicle(player, rentalId, hash, tage); return; }
                int Price = rentalVeh.pricePerDay * tage;
                if (!CharactersInventory.ExistCharacterItem(player.CharacterId, "Bargeld", "brieftasche") || CharactersInventory.GetCharacterItemAmount(player.CharacterId, "Bargeld", "brieftasche") < Price) { HUDHandler.SendBetterNotif(player, 3, 10, "Auto Vermietung", $"Du hast nicht genügend Bargeld dabei ({Price}$)."); return; }
                CharactersInventory.RemoveCharacterItemAmount(player.CharacterId, "Bargeld", Price, "brieftasche");
                DateTime expireRental = DateTime.Now.AddDays(tage);
                int serialNumber = new Random().Next(1, 10000);
                int vehClass = ServerVehicles.VehicleClass(fHash);
                ServerVehicles.CreateVehicle(fHash, player.CharacterId, 0, 0, false, 1, rental.parkOutPos, rental.parkOutRot, $"LS{rnd}", 255, 255, 255, vehClass, serialNumber, true, expireRental, false);
                CharactersInventory.AddCharacterItem(player.CharacterId, $"Fahrzeugschluessel LS{rnd}", 2, "schluessel");
                HUDHandler.SendBetterNotif(player, 3, 10, "Auto Vermietung", $"Fahrzeug erfolgreich für {tage} Tage gemietet ({Price}$).");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static string GetCarRentalVehicle(int rentalId)
        {
            var rentalVehicles = ServerCarRentalsVehicles_.ToList().Where(x => x.carRentalId == rentalId).Select(x => new
            {
                x.hash,
                name = Model.ServerVehicles.GetVehicleNameOnHash(x.hash),
                price = x.pricePerDay,
            }).OrderBy(x => x.name).ToList();

            return Newtonsoft.Json.JsonConvert.SerializeObject(rentalVehicles);
        }
    }
}