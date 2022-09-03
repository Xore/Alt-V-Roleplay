using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Altv_Roleplay.Handler
{
    class FuelStationHandler : IScript
    {
        [AsyncClientEvent("Server:FuelStation:FuelVehicleAction")]
        public async void FuelVehicle(IPlayer player, int vID, int fuelstationId, string fueltype, int selectedLiterAmount, int selectedLiterPrice)
        {
            try
            {
                if (player == null || !player.Exists || vID == 0 || fuelstationId == 0 || fueltype == "" || selectedLiterAmount <= 0 || selectedLiterPrice == 0) return;
                long vehID = Convert.ToInt64(vID);
                int charId = User.GetPlayerOnline(player);
                if (vehID <= 0 || charId <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                var vehicle = Alt.GetAllVehicles().ToList().FirstOrDefault(x => x.GetVehicleId() == vehID);
                if (vehicle == null || !vehicle.Exists) { HUDHandler.SendNotification(player, 3, 2500, "Ein unerwarteter Fehler ist aufgetreten. [FEHLERCODE: FUEL-004]"); return; }
                if (ServerVehicles.GetVehicleType(vehicle) == 0)
                {
                    if (!CharactersInventory.ExistCharacterItem(charId, "Bargeld", "brieftasche")) { HUDHandler.SendNotification(player, 3, 2500, "Du hast nicht genügend Bargeld dabei."); return; }
                    if (CharactersInventory.GetCharacterItemAmount(charId, "Bargeld", "brieftasche") < (selectedLiterPrice * selectedLiterAmount)) { HUDHandler.SendNotification(player, 3, 2500, "Du hast nicht genügend Bargeld dabei."); return; }
                }
                if (!player.Position.IsInRange(vehicle.Position, 8f)) { HUDHandler.SendNotification(player, 3, 2500, "Du hast dich zu weit vom Fahrzeug entfernt."); return; }
                if (ServerVehicles.GetVehicleFuel(vehicle) >= ServerVehicles.GetVehicleFuelLimitOnHash(vehicle.Model)) { HUDHandler.SendNotification(player, 3, 2500, "Das Fahrzeug ist bereits voll getankt."); return; }
                var fuelStation = ServerFuelStations.ServerFuelStations_.FirstOrDefault(x => x.id == fuelstationId);
                if (fuelStation == null) { HUDHandler.SendNotification(player, 3, 2500, "Ein unerwarteter Fehler ist aufgetreten. [FEHLERCODE: FUEL-005]"); return; }
                int duration = 1000 * selectedLiterAmount;
                HUDHandler.SendProgress(player, "Fahrzeug wird betankt, bitte warten..", "alert", duration);
                await Task.Delay(duration);
                lock (player)
                {
                    if (!player.Position.IsInRange(vehicle.Position, 8f)) { HUDHandler.SendNotification(player, 3, 2500, "Du hast dich zu weit vom Fahrzeug entfernt."); return; }
                }
                float fuelVal = ServerVehicles.GetVehicleFuel(vehicle) + selectedLiterAmount;
                if (fuelVal > ServerVehicles.GetVehicleFuelLimitOnHash(vehicle.Model)) { fuelVal = ServerVehicles.GetVehicleFuelLimitOnHash(vehicle.Model); }
                if (ServerVehicles.GetVehicleType(vehicle) == 0)
                {
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Bargeld", (selectedLiterPrice * selectedLiterAmount), "brieftasche");
                }
                ServerVehicles.SetVehicleFuel(vehicle, fuelVal);
                if (ServerVehicles.GetVehicleFuelTypeOnHash(vehicle.Model) != fueltype) { ServerVehicles.SetVehicleEngineState(vehicle, false); ServerVehicles.SetVehicleEngineHealthy(vehicle, false); return; }
                ServerFuelStations.SetFuelStationBankMoney(fuelstationId, ServerFuelStations.GetFuelStationBankMoney(fuelstationId) + (selectedLiterPrice * selectedLiterAmount));
                ServerFuelStations.SetFuelStationAvailableLiters(fuelstationId, ServerFuelStations.GetFuelStationAvailableLiters(fuelstationId) - selectedLiterAmount);

                /*if (ServerFuelStations.GetFuelStationOwnerId(fuelstationId) != 0)
                {
                    ServerFuelStations.SetFuelStationAvailableLiters(fuelstationId, ServerFuelStations.GetFuelStationAvailableLiters(fuelstationId) - selectedLiterAmount);
                }*/
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
    }
}
