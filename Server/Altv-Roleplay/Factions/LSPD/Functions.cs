using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Handler;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;

namespace Altv_Roleplay.Factions.LSPD
{
    class Functions : IScript
    {
        [AsyncClientEvent("Server:Tablet:LSPDAppSearchPerson")]
        public void LSPDAppSearchPerson(IPlayer player, string targetCharname)
        {
            try
            {
                if (player == null || !player.Exists || targetCharname == "") return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                if(player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                if(!ServerFactions.IsCharacterInAnyFaction(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Du bist in keiner Fraktion."); return; }
                if(ServerFactions.GetCharacterFactionId(charId) != 2 && ServerFactions.GetCharacterFactionId(charId) != 1 && ServerFactions.GetCharacterFactionId(charId) != 12) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Du bist nicht im L.S.P.D. oder der Justiz angestellt."); return; }
                if (!ServerFactions.IsCharacterInFactionDuty(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Du bist nicht im Dienst."); return; }
                if (!Characters.ExistCharacterName(targetCharname)) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Der eingegebene Name wurde nicht gefunden."); return; }
                int targetCharId = Characters.GetCharacterIdFromCharName(targetCharname);
                if (targetCharId <= 0) return;
                string charName = Characters.GetCharacterName(targetCharId);
                string gender = "Unbekannt";
                string birthdate = Characters.GetCharacterBirthdate(targetCharId);
                string birthplace = Characters.GetCharacterBirthplace(targetCharId);
                string address = $"{Characters.GetCharacterStreet(targetCharId)}";
                string mainBankAccount = "Nicht vorhanden";
                string firstJoinDate = $"{Characters.GetCharacterFirstJoinDate(targetCharId).ToString("d", CultureInfo.CreateSpecificCulture("de-DE"))}";
                if(CharactersBank.HasCharacterBankMainKonto(targetCharId)) { mainBankAccount = $"{CharactersBank.GetCharacterBankMainKonto(targetCharId)}"; }
                if(Characters.GetCharacterGender(targetCharId)) { gender = "Weiblich"; }
                else { gender = "Männlich"; }
                player.EmitLocked("Client:Tablet:SetLSPDAppPersonSearchData", charName, gender, birthdate, birthplace, address, mainBankAccount, firstJoinDate);
                HUDHandler.SendNotification(player, 1, 2500, $"Personenabfrage durchgeführt: {charName}.");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Tablet:LSPDAppSearchVehiclePlate")]
        public void LSPDAppSearchVehiclePlate(IPlayer player, string targetPlate)
        {
            try
            {
                var basePlate = new[] { "LSPD", "DOJ", "LSFD", "ACLS", "LSF", "FIB", "LSF-", "LSPD-", "DOJ-", "LSFD-", "ACLS-", "FIB-", "LS", "LS-", "EL-", "MM-", "PL-", "SWAT", "S.W.A.T", "SWAT-", "NOOSE", "N.O.O.S.E", "WP", "WP-", "WALKERS", "WALKERS-", "BENNYS" };
                if (player == null || !player.Exists || targetPlate == "") return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                if (!ServerFactions.IsCharacterInAnyFaction(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Du bist in keiner Fraktion."); return; }
                if (ServerFactions.GetCharacterFactionId(charId) != 2 && ServerFactions.GetCharacterFactionId(charId) != 1 && ServerFactions.GetCharacterFactionId(charId) != 12) { HUDHandler.SendNotification(player, 3, 2500, "Du bist nicht im L.S.P.D. oder der Justiz angestellt."); return; }
                if (!ServerFactions.IsCharacterInFactionDuty(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Du bist nicht im Dienst."); return; }
                if (!ServerVehicles.ExistServerVehiclePlate(targetPlate)) { HUDHandler.SendNotification(player, 3, 2500, "Das angegebene Kennzeichen wurde nicht gefunden."); return; }
                int vehicleId = ServerVehicles.GetVehicleIdByPlate(targetPlate);
                if (vehicleId <= 0) return;
                int ownerId = ServerVehicles.GetVehicleOwnerById(vehicleId);
                if (ownerId <= 0) return;
                string owner = Characters.GetCharacterName(ownerId);
                string vehName = ServerVehicles.GetVehicleNameOnHash(ServerVehicles.GetVehicleHashById(vehicleId));
                string manufactor = ServerVehicles.GetVehicleManufactorOnHash(ServerVehicles.GetVehicleHashById(vehicleId));
                string buyDate = $"{ServerVehicles.GetVehicleBuyDate(vehicleId).ToString("d", CultureInfo.CreateSpecificCulture("de-DE"))}";
                string trunk = $"{ServerVehicles.GetVehicleTrunkCapacityOnHash(ServerVehicles.GetVehicleHashById(vehicleId))}kg";
                string maxfuel = $"{ServerVehicles.GetVehicleFuelLimitOnHash(ServerVehicles.GetVehicleHashById(vehicleId))}";
                string fuelType = ServerVehicles.GetVehicleFuelTypeOnHash(ServerVehicles.GetVehicleHashById(vehicleId));


                for (var i = 0; i < basePlate.Length; i++)
                {
                    if (targetPlate.StartsWith(basePlate[i])) { ownerId = 0; break; }
                }
                player.EmitLocked("Client:Tablet:SetLSPDAppSearchVehiclePlateData", owner, vehName, manufactor, buyDate, trunk, maxfuel, fuelType);
                HUDHandler.SendNotification(player, 1, 2500, $"Fahrzeugabfrage durchgeführt: {targetPlate}");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [AsyncClientEvent("Server:Tablet:LSPDAppSearchVehicleOwner")]
        public void LSPDAppSearchVehicleOwnerName(IPlayer player, string firstname, string lastname)
        {
            try
            {
                if (player == null || !player.Exists || lastname == "" || firstname == "") return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                if (!ServerFactions.IsCharacterInAnyFaction(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Du bist in keiner Fraktion."); return; }
                if (ServerFactions.GetCharacterFactionId(charId) != 2 && ServerFactions.GetCharacterFactionId(charId) != 1 && ServerFactions.GetCharacterFactionId(charId) != 12) { HUDHandler.SendNotification(player, 3, 2500, "Du bist nicht im L.S.P.D. oder der Justiz angestellt."); return; }

                string vehiclePlate = ServerIdentifiedVehicles.getEntryByName(firstname, lastname);

                if (vehiclePlate == "None")
                    return;

                int vehicleId = ServerVehicles.GetVehicleIdByPlate(vehiclePlate);
                if (vehicleId <= 0) return;
                string owner = ServerIdentifiedVehicles.getOwnerByPlate(vehiclePlate);
                string vehName = ServerVehicles.GetVehicleNameOnHash(ServerVehicles.GetVehicleHashById(vehicleId));
                string manufactor = ServerVehicles.GetVehicleManufactorOnHash(ServerVehicles.GetVehicleHashById(vehicleId));
                string buyDate = $"{ServerVehicles.GetVehicleBuyDate(vehicleId).ToString("d", CultureInfo.CreateSpecificCulture("de-DE"))}";
                string trunk = $"{ServerVehicles.GetVehicleTrunkCapacityOnHash(ServerVehicles.GetVehicleHashById(vehicleId))}kg";
                string tax = $"{ServerAllVehicles.GetVehicleTaxes(ServerVehicles.GetVehicleHashById(vehicleId))}$";
                string maxfuel = $"{ServerVehicles.GetVehicleFuelLimitOnHash(ServerVehicles.GetVehicleHashById(vehicleId))}";
                string fuelType = ServerVehicles.GetVehicleFuelTypeOnHash(ServerVehicles.GetVehicleHashById(vehicleId));


                player.EmitLocked("Client:Tablet:SetLSPDAppSearchVehiclePlateData", owner, vehName, manufactor, buyDate, trunk, maxfuel, tax, fuelType);
                HUDHandler.SendNotification(player, 1, 2500, $"Fahrzeugabfrage durchgeführt: {firstname} {lastname}");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }        
        }

        [AsyncClientEvent("Server:Tablet:LSPDAppSearchVehiclePlateNew")]
        public void LSPDAppSearchVehiclePlateNew(IPlayer player, string targetPlate)
        {
            try
            {
                if (player == null || !player.Exists || targetPlate == "") return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                if (!ServerFactions.IsCharacterInAnyFaction(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Du bist in keiner Fraktion."); return; }
                if (ServerFactions.GetCharacterFactionId(charId) != 2 && ServerFactions.GetCharacterFactionId(charId) != 1 && ServerFactions.GetCharacterFactionId(charId) != 12) { HUDHandler.SendNotification(player, 3, 2500, "Du bist nicht im L.S.P.D. oder der Justiz angestellt."); return; }
                if (!ServerFactions.IsCharacterInFactionDuty(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Du bist nicht im Dienst."); return; }
                if (!ServerVehicles.ExistServerVehiclePlate(targetPlate)) { HUDHandler.SendNotification(player, 3, 2500, "Das angegebene Kennzeichen wurde nicht gefunden."); return; }
                int vehicleId = ServerVehicles.GetVehicleIdByPlate(targetPlate);
                if (vehicleId <= 0) return;
                string owner = ServerIdentifiedVehicles.getOwnerByPlate(targetPlate);
                string vehName = ServerVehicles.GetVehicleNameOnHash(ServerVehicles.GetVehicleHashById(vehicleId));
                string manufactor = ServerVehicles.GetVehicleManufactorOnHash(ServerVehicles.GetVehicleHashById(vehicleId));
                string buyDate = $"{ServerVehicles.GetVehicleBuyDate(vehicleId).ToString("d", CultureInfo.CreateSpecificCulture("de-DE"))}";
                string trunk = $"{ServerVehicles.GetVehicleTrunkCapacityOnHash(ServerVehicles.GetVehicleHashById(vehicleId))}kg";
                string tax = $"{ServerAllVehicles.GetVehicleTaxes(ServerVehicles.GetVehicleHashById(vehicleId))}$";
                string maxfuel = $"{ServerVehicles.GetVehicleFuelLimitOnHash(ServerVehicles.GetVehicleHashById(vehicleId))}";
                string fuelType = ServerVehicles.GetVehicleFuelTypeOnHash(ServerVehicles.GetVehicleHashById(vehicleId));

                if (owner == "None")
                    return;
                

                player.EmitLocked("Client:Tablet:SetLSPDAppSearchVehiclePlateData", owner, vehName, manufactor, buyDate, trunk, maxfuel, tax, fuelType);
                HUDHandler.SendNotification(player, 1, 2500, $"Fahrzeugabfrage durchgeführt: {targetPlate}");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }


        [AsyncClientEvent("Server:Tablet:LSPDAppSearchLicense")]
        public void LSPDAppSearchLicense(IPlayer player, string targetCharname)
        {
            try
            {
                if (player == null || !player.Exists || targetCharname == "") return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                if (!ServerFactions.IsCharacterInAnyFaction(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Du bist in keiner Fraktion."); return; }
                if (ServerFactions.GetCharacterFactionId(charId) != 2 && ServerFactions.GetCharacterFactionId(charId) != 1 && ServerFactions.GetCharacterFactionId(charId) != 12) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Du bist nicht im L.S.P.D. oder der Justiz angestellt."); return; }
                if (!ServerFactions.IsCharacterInFactionDuty(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Du bist nicht im Dienst."); return; }
                if (!Characters.ExistCharacterName(targetCharname)) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Der eingegebene Name wurde nicht gefunden."); return; }
                int targetCharId = Characters.GetCharacterIdFromCharName(targetCharname);
                if (targetCharId <= 0) return;
                string charName = Characters.GetCharacterName(targetCharId);
                string licArray = CharactersLicenses.GetCharacterLicenses(targetCharId);
                player.EmitLocked("Client:Tablet:SetLSPDAppLicenseSearchData", charName, licArray);
                HUDHandler.SendNotification(player, 1, 2500, $"Lizenzabfrage durchgeführt: {charName}.");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Tablet:LSPDAppTakeLicense")]
        public void LSPDAppTakeLicense(IPlayer player, string targetCharname, string licName)
        {
            try
            {
                if (player == null || !player.Exists || targetCharname == "" || licName == "") return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                if (!ServerFactions.IsCharacterInAnyFaction(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Du bist in keiner Fraktion."); return; }
                if (ServerFactions.GetCharacterFactionId(charId) != 2 && ServerFactions.GetCharacterFactionId(charId) != 12) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Du bist nicht im L.S.P.D. angestellt."); return; }
                if (!ServerFactions.IsCharacterInFactionDuty(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Du bist nicht im Dienst."); return; }
                if (!Characters.ExistCharacterName(targetCharname)) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Der eingegebene Name wurde nicht gefunden."); return; }
                //Alt.Log($"{CharactersLicenses.GetFullLicenseName(licName)}");
                if (CharactersLicenses.GetFullLicenseName(licName) == "None") return;
                int targetCharId = Characters.GetCharacterIdFromCharName(targetCharname);
                if (targetCharId <= 0) return;
                if(!CharactersLicenses.HasCharacterLicense(targetCharId, licName)) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Der Spieler hat diese Lizenz nicht mehr."); return; }
                CharactersLicenses.SetCharacterLicense(targetCharId, licName, false);
                HUDHandler.SendNotification(player, 1, 2500, $"{targetCharname} wurde die Lizenz {CharactersLicenses.GetFullLicenseName(licName)} entzogen.");
                LSPDAppSearchLicense(player, targetCharname);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
    }
}
