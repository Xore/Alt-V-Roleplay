using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Altv_Roleplay.Handler
{
    class RaycastHandler : IScript
    {
        [AsyncClientEvent("Server:InteractionMenu:GetMenuVehicleItems")]
        public void GetMenuVehicleItems(IPlayer player, string type, IVehicle veh)
        {
            try
            {
                if (type != "vehicleIn" && type != "vehicleOut") return;
                if (veh == null || !veh.Exists || player == null || !player.Exists) return;
                long vehID = veh.GetVehicleId();
                int charId = (int)player.GetCharacterMetaId();
                //Alt.Log($"GetVehicleItems: {vehID} - {charId}");
                bool vehTrunkIsOpen = veh.GetVehicleTrunkState(); //false = zu || true = offen
                if (charId <= 0 || vehID <= 0) return;
                var interactHTML = "";
                interactHTML += "<li><p id='InteractionMenu-SelectedTitle'>Schließen</p></li><li class='interactitem' data-action='close' data-actionstring='Schließen'><img src='../utils/img/cancel.png'></li>";
                if (type == "vehicleOut")
                {
                    interactHTML += "<li class='interactitem' id='InteractionMenu-vehtoggleLock' data-action='vehtoggleLock' data-actionstring='Fahrzeug auf/abschließen'><img src='../utils/img/vehlock.png'></li>";
                    interactHTML += "<li class='interactitem' id='InteractionMenu-vehFuelVehicle' data-action='vehFuelVehicle' data-actionstring='Fahrzeug tanken'><img src='../utils/img/vehfuel.png'></li>";


                    if (!ServerVehicles.GetVehicleLockState(veh))
                    {
                        interactHTML += "<li class='interactitem' id='InteractionMenu-vehOpenCloseTrunk' data-action='vehOpenCloseTrunk' data-actionstring='Kofferraum öffnen/schließen'><img src='../utils/img/trunk.png'></li>";
                        interactHTML += "<li class='interactitem' id='InteractionMenu-vehOpenCloseHood' data-action='vehOpenCloseHood' data-actionstring='Motorhaube öffnen/schließen'><img src='../utils/img/hood.png'></li>";
                    }

                    if (vehTrunkIsOpen)
                    {
                        interactHTML += "<li class='interactitem' id='InteractionMenu-vehViewTrunkContent' data-action='vehViewTrunkContent' data-actionstring='Kofferraum ansehen'><img src='../utils/img/viewtrunk.png'></li>";
                    }

                    if (CharactersInventory.ExistCharacterItem(charId, "Reparaturkit", "inventory") || CharactersInventory.ExistCharacterItem(charId, "Reparaturkit", "backpack"))
                    {
                        interactHTML += "<li class='interactitem' id='InteractionMenu-vehRepair' data-action='vehRepair' data-actionstring='Fahrzeug reparieren'><img src='../utils/img/repair.png'></li>";
                    }

                    if (CharactersInventory.ExistCharacterItem(charId, "Lappen", "inventory"))
                    {
                        interactHTML += "<li class='interactitem' id='InteractionMenu-vehClear' data-action='vehClear' data-actionstring='Fahrzeug Waschen'><img src='../utils/img/Lappen.png'></li>";
                    }

                    if (ServerFactions.IsCharacterInAnyFaction(charId) && ServerFactions.IsCharacterInFactionDuty(charId) && ServerFactions.GetCharacterFactionId(charId) == 14)
                    {
                        if (veh.Position.IsInRange(Constants.Positions.BennyPosition, 25f) && veh.GetVehicleId() > 0)
                        {
                            interactHTML += "<li class='interactitem' id='InteractionMenu-vehTuning' data-action='vehTuning' data-actionstring='Fahrzeug modifizieren'><img src='../utils/img/vehTuning.png'></li>";
                        }
                    }

                    if (ServerFactions.IsCharacterInAnyFaction(charId) && ServerFactions.IsCharacterInFactionDuty(charId) && ServerFactions.GetCharacterFactionId(charId) == 4 && ServerFactions.GetCharacterFactionId(charId) == 2 && ServerFactions.GetCharacterFactionId(charId) == 12)
                    {
                        if (veh.Position.IsInRange(Constants.Positions.AutoClubLosSantos_StoreVehPosition, 5f) && veh.GetVehicleId() > 0)
                        {
                            interactHTML += "<li class='interactitem' id='InteractionMenu-towVehicle' data-action='vehTow' data-actionstring='Fahrzeug verwahren'><img src='../utils/img/towvehicle.png'></li>";
                        }
                    }

                    if (ServerFactions.IsCharacterInAnyFaction(charId) && ServerFactions.IsCharacterInFactionDuty(charId) && ServerFactions.GetCharacterFactionId(charId) == 13)
                    {
                        if (veh.Position.IsInRange(Constants.Positions.WP_Position_01, 25f) && veh.GetVehicleId() > 0)
                        {
                            interactHTML += "<li class='interactitem' id='InteractionMenu-vehTuning' data-action='vehTuning' data-actionstring='Fahrzeug modifizieren'><img src='../utils/img/vehTuning.png'></li>";
                        }
                    }
                    if (ServerFactions.IsCharacterInAnyFaction(charId) && ServerFactions.IsCharacterInFactionDuty(charId) && ServerFactions.GetCharacterFactionId(charId) == 15)
                    {
                        if (veh.Position.IsInRange(Constants.Positions.WP_Position_03, 25f) && veh.GetVehicleId() > 0)
                        {
                            interactHTML += "<li class='interactitem' id='InteractionMenu-vehTuning' data-action='vehTuning' data-actionstring='Fahrzeug modifizieren'><img src='../utils/img/vehTuning.png'></li>";
                        }
                    }
                    if (ServerVehicles.GetVehicleOwner(veh) != charId && CharactersInventory.ExistCharacterItem(charId, "Dietrich", "inventory"))
                    {
                        interactHTML += "<li class='interactitem' id='InteractionMenu-vehBreakVehicle' data-action='vehBreakVehicle' data-actionstring='Fahrzeug aufbrechen'><img src='../utils/img/Dietrich.png'></li>";
                    }
                }
                else if (type == "vehicleIn")
                {
                    interactHTML += "<li class='interactitem' id='InteractionMenu-vehtoggleLock' data-action='vehtoggleLock' data-actionstring='Fahrzeug auf/abschließen'><img src='../utils/img/vehlock.png'></li>";
                    interactHTML += "<li class='interactitem' id='InteractionMenu-vehtoggleEngine' data-action='vehtoggleEngine' data-actionstring='Motor an/ausmachen'><img src='../utils/img/vehengine.png'></li>";
                    interactHTML += "<li class='interactitem' id='InteractionMenu-seatbelt' data-action='seatbelt' data-actionstring='An/Abschnallen'><img src='../utils/img/seatbelt.png'></li>";

                    if (player.IsInVehicle && (player.Seat == 1 || player.Seat == 2) && ServerVehicles.GetVehicleType(veh) != 2)
                    {
                        interactHTML += "<li class='interactitem' id='InteractionMenu-vehViewGloveboxContent' data-action='vehViewGloveboxContent' data-actionstring='Handschuhfach ansehen'><img src='../utils/img/viewglovebox.png'></li>";
                    }

                    if (ServerVehicles.GetVehicleOwner(veh) == charId && ServerVehicles.GetVehicleType(veh) == 0)
                    {
                        interactHTML += "<li class='interactitem' id='InteractionMenu-vehchangeowner' data-action='vehchangeowner' data-actionstring='Fahrzeug überschreiben'><img src='../utils/img/vehlock.png'></li>";
                    }

                    if (ServerVehicles.GetVehicleOwner(veh) != charId && CharactersInventory.ExistCharacterItem(charId, "Kabelbaum", "inventory"))
                    {
                        interactHTML += "<li class='interactitem' id='InteractionMenu-vehbreakEngine' data-action='vehbreakEngine' data-actionstring='Fahrzeug kurzschliessen'><img src='../utils/img/zange.png'></li>";
                    }
                }

                player.EmitLocked("Client:RaycastMenu:SetMenuItems", type, interactHTML);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:InteractionMenu:GetMenuPlayerItems")]
        public void GetMenuPlayerItems(IPlayer player, string type, IPlayer targetPlayer)
        {
            try
            {
                if (player == null || !player.Exists || type != "player" || targetPlayer == null || !targetPlayer.Exists) return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                var interactHTML = "";
                interactHTML += "<li><p id='InteractionMenu-SelectedTitle'>Schließen</p></li><li class='interactitem' data-action='close' data-actionstring='Schließen'><img src='../utils/img/cancel.png'></li>";
                interactHTML += "<li class='interactitem' id='InteractionMenu-playersupportId' data-action='playersupportId' data-actionstring='Support ID anzeigen'><img src='../utils/img/playersupportid.png'></li>";
                interactHTML += "<li class='interactitem' id='InteractionMenu-playergiveItem' data-action='playergiveItem' data-actionstring='Gegenstand geben'><img src='../utils/img/order.png'></li>";
                //interactHTML += "<li class='interactitem' id='InteractionMenu-showIdCard' data-action='showIdCard' data-actionstring='Ausweis zeigen'><img src='../utils/img/inventory/Ausweis.png'></li>";
                //Ausweis {Characters.GetCharacterName(charId)}
                #region Documents
                //Identification & Licenses
                if (CharactersInventory.ExistCharacterItem(charId, $"Ausweis {Characters.GetCharacterName(charId)}", "brieftasche"))
                {
                    interactHTML += "<li class='interactitem' id='InteractionMenu-showIdentityCard' data-action='showIdentityCard' data-actionstring='Ausweis zeigen'><img src='../utils/img/inventory/Ausweis.png'></li>";
                }
                if (CharactersInventory.ExistCharacterItem(charId, $"Fuehrerschein {Characters.GetCharacterName(charId)}", "brieftasche"))
                {
                    interactHTML += "<li class='interactitem' id='InteractionMenu-showDriversLic' data-action='showDriversLic' data-actionstring='Führerschein zeigen'><img src='../utils/img/inventory/Fuehrerschein.png'></li>";
                }
                if (CharactersInventory.ExistCharacterItem(charId, $"Waffenschein {Characters.GetCharacterName(charId)}", "brieftasche"))
                {
                    interactHTML += "<li class='interactitem' id='InteractionMenu-showWepLic' data-action='showWepLic' data-actionstring='Waffenschein zeigen'><img src='../utils/img/inventory/Waffenschein.png'></li>";
                }
                //Faction Shit
                if (ServerFactions.IsCharacterInAnyFaction(charId) && ServerFactions.IsCharacterInFactionDuty(charId))
                {
                    interactHTML += "<li class='interactitem' id='InteractionMenu-showFactionCard' data-action='showFactionCard' data-actionstring='Dienstausweis zeigen'><img src='../utils/img/inventory/Fraktion.png'></li>";
                }
                if (ServerFactions.IsCharacterInAnyFaction(charId) && ServerFactions.IsCharacterInFactionDuty(charId) && ServerFactions.GetCharacterFactionId(charId) == 2)
                {
                    interactHTML += "<li class='interactitem' id='InteractionMenu-showPoliceCard' data-action='showPoliceCard' data-actionstring='Dienstausweis zeigen'><img src='../utils/img/inventory/Dienstausweis.png'></li>";
                }
                if (ServerFactions.IsCharacterInAnyFaction(charId) && ServerFactions.IsCharacterInFactionDuty(charId) && ServerFactions.GetCharacterFactionId(charId) == 12)
                {
                    interactHTML += "<li class='interactitem' id='InteractionMenu-showFIBCard' data-action='showFIBCard' data-actionstring='Dienstausweis zeigen'><img src='../utils/img/inventory/Dienstausweis.png'></li>";
                }
                if (ServerFactions.IsCharacterInAnyFaction(charId) && ServerFactions.IsCharacterInFactionDuty(charId) && ServerFactions.GetCharacterFactionId(charId) == 3)
                {
                    interactHTML += "<li class='interactitem' id='InteractionMenu-showLSMDCard' data-action='showLSMDCard' data-actionstring='Dienstausweis zeigen'><img src='../utils/img/inventory/Medic.png'></li>";
                }
                #endregion

                if (CharactersInventory.ExistCharacterItem(charId, "Handschellenschluessel", "schluessel"))
                {
                    interactHTML += "<li class='interactitem' id='InteractionMenu-playerGiveTakeHandcuffs' data-action='playerGiveTakeHandcuffs' data-actionstring='Handschellen an/ablegen'><img src='../utils/img/inventory/Handschellenschluessel.png'></li>";
                }

                if (CharactersInventory.ExistCharacterItem(charId, "Dietrich", "inventory") || CharactersInventory.ExistCharacterItem(charId, "Dietrich", "backpack"))
                {
                    interactHTML += "<li class='interactitem' id='InteractionMenu-playerGiveTakeHandcuffs' data-action='playerGiveTakeHandcuffs' data-actionstring='Handschellen an/ablegen'><img src='../utils/img/inventory/Dietrich.png'></li>";
                }

                if (CharactersInventory.ExistCharacterItem(charId, "Handschellen", "inventory") || CharactersInventory.ExistCharacterItem(charId, "Handschellen", "backpack"))
                {
                    interactHTML += "<li class='interactitem' id='InteractionMenu-playerGiveTakeHandcuffs' data-action='playerGiveTakeHandcuffs' data-actionstring='Handschellen an/ablegen'><img src='../utils/img/inventory/Handschellen.png'></li>";
                }

                if (CharactersInventory.ExistCharacterItem(charId, "Kabelbinder", "inventory") || CharactersInventory.ExistCharacterItem(charId, "Kabelbinder", "backpack"))
                {
                    interactHTML += "<li class='interactitem' id='InteractionMenu-playerGiveTakeRopeCuffs' data-action='playerGiveTakeRopeCuffs' data-actionstring='Spieler fesseln/entfesseln'><img src='../utils/img/inventory/kabelbinder.png'></li>";
                }

                if (targetPlayer.HasPlayerRopeCuffs())
                {
                    interactHTML += "<li class='interactitem' id='InteractionMenu-playerGiveTakeRopeCuffs' data-action='playerGiveTakeRopeCuffs' data-actionstring='Spieler fesseln/entfesseln'><img src='../utils/img/inventory/Messer.png'></li>";
                }

                if (targetPlayer.HasPlayerHandcuffs() || targetPlayer.HasPlayerRopeCuffs() || Characters.IsCharacterUnconscious((int)targetPlayer.GetCharacterMetaId()))
                {
                    interactHTML += "<li class='interactitem' id='InteractionMenu-playerSearchInventory' data-action='playerSearchInventory' data-actionstring='Spieler durchsuchen'><img src='../utils/img/searchbag.png'></li>";
                }

                if (ServerFactions.IsCharacterInAnyFaction(charId) && ServerFactions.IsCharacterInFactionDuty(charId))
                {
                    interactHTML += "<li class='interactitem' id='InteractionMenu-playerGiveBill' data-action='playergiveFactionBill' data-actionstring='Rechnung ausstellen (Fraktion)'><img src='../utils/img/bill.png'></li>";

                    if ((ServerFactions.GetCharacterFactionId(charId) == 2 || ServerFactions.GetCharacterFactionId(charId) == 12) && player.Position.IsInRange(Constants.Positions.Arrest_Position, 5f) && targetPlayer.Position.IsInRange(Constants.Positions.Arrest_Position, 5f))
                    {
                        interactHTML += "<li class='interactitem' id='InteractionMenu-playerJail' data-action='playerJail' data-actionstring='Spieler inhaftieren'><img src='../utils/img/jail.png'></li>";
                    }

                    if (ServerFactions.GetCharacterFactionId(charId) == 3 && Characters.IsCharacterUnconscious((int)targetPlayer.GetCharacterMetaId()))
                    {
                        interactHTML += "<li class='interactitem' id='InteractionMenu-playerRevive' data-action='playerRevive' data-actionstring='Spieler wiederbeleben'><img src='../utils/img/revive.png'></li>";
                    }

                    if (ServerFactions.GetCharacterFactionId(charId) == 3 && targetPlayer.Health < 200)
                    {
                        interactHTML += "<li class='interactitem' id='InteractionMenu-HealPlayer' data-action='healPlayer' data-actionstring='Spieler heilen'><img src='../utils/img/inventory/Heilen.png'></li>";
                    }
                }

                if (ServerFactions.IsCharacterInAnyFaction(charId) && ServerFactions.IsCharacterInFactionDuty(charId) && ServerFactions.GetCharacterFactionId(charId) == 5)
                {
                    interactHTML += "<li class='interactitem' id='InteractionMenu-playerGiveLicense' data-action='playerGiveLicense' data-actionstring='Lizenz ausstellen (Fahrschule)'><img src='../utils/img/drivingschool.png'></li>";
                }

                player.EmitLocked("Client:RaycastMenu:SetMenuItems", type, interactHTML);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Raycast:jailPlayer")]
        public void jailPlayer(ClassicPlayer player, ClassicPlayer targetPlayer)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0 || targetPlayer == null || !targetPlayer.Exists || targetPlayer.CharacterId <= 0 || !ServerFactions.IsCharacterInAnyFaction(player.CharacterId) || !ServerFactions.IsCharacterInFactionDuty(player.CharacterId) || (ServerFactions.GetCharacterFactionId(player.CharacterId) != 2 && ServerFactions.GetCharacterFactionId(player.CharacterId) != 12) || Characters.IsCharacterInJail(targetPlayer.CharacterId)) return;
                HUDHandler.SendNotification(player, 1, 2500, "Person wurde Inhaftiert.");
                HUDHandler.SendNotification(player, 1, 2500, $"Du wurdest Inhaftiert.");
                targetPlayer.Position = new Position(1716.5143f, 2587.0154f, 53.010254f);
                if (Characters.GetCharacterGender(targetPlayer.CharacterId) == false)
                {
                    targetPlayer.EmitLocked("Client:SpawnArea:setCharClothes", 0, 0, 0);//ToDo-Clothes
                }
                else
                {
                    targetPlayer.EmitLocked("Client:SpawnArea:setCharClothes", 0, 0, 0);//ToDo-Clothes
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Raycast:OpenVehicleFuelMenu")]
        public void OpenVehicleFuelMenu(IPlayer player, IVehicle veh)
        {
            try
            {
                if (player == null || !player.Exists || veh == null || !veh.Exists) return;
                int charId = User.GetPlayerOnline(player);
                long vehID = veh.GetVehicleId();
                if (charId <= 0 || vehID <= 0 || player.IsInVehicle) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                if (!player.Position.IsInRange(veh.Position, 15f)) { HUDHandler.SendNotification(player, 3, 2500, "Du hast dich zu weit vom Fahrzeug entfernt."); return; }
                if (ServerVehicles.GetVehicleEngineState(veh)) { HUDHandler.SendNotification(player, 3, 2500, "Der Motor vom Fahrzeug muss ausgeschaltet sein."); return; }
                var fuelSpot = ServerFuelStations.ServerFuelStationSpots_.FirstOrDefault(x => veh.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 25f));
                if (fuelSpot == null) { HUDHandler.SendNotification(player, 3, 2500, "Das Fahrzeug befindet sich an keiner Tankstelle. [FEHLERCODE: 001]"); return; }
                int fuelStationId = ServerFuelStations.GetFuelSpotParentStation(fuelSpot.id);
                if (fuelStationId == 0) { HUDHandler.SendNotification(player, 3, 2500, "Ein unerwarteter Fehler ist aufgetreten. [FEHLERCODE: 002]"); return; }
                int availableLiter = ServerFuelStations.GetFuelStationAvailableLiters(fuelStationId);
                if (availableLiter < 1) { HUDHandler.SendNotification(player, 3, 2500, "Diese Tankstelle hat keinen Treibstoff mehr auf Lager."); return; }
                var fuelArray = ServerFuelStations.GetFuelStationAvailableFuel(fuelStationId);
                string stationName = ServerFuelStations.GetFuelStationName(fuelStationId);
                string ownerName = ServerFuelStations.GetFuelStationOwnerName(ServerFuelStations.GetFuelStationOwnerId(fuelStationId));
                var maxFuel = ServerVehicles.GetVehicleFuelLimitOnHash(veh.Model);
                var curFuel = Convert.ToInt32(ServerVehicles.GetVehicleFuel(veh));
                maxFuel -= curFuel;

                player.EmitLocked("Client:FuelStation:OpenCEF", fuelStationId, stationName, ownerName, maxFuel, availableLiter, fuelArray, vehID);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [AsyncClientEvent("Server:Raycast:ReportVehicle")]
        public static void VehicleGetPosition(IPlayer player, IVehicle veh)
        {
            //var myVehicle = (ClassicVehicle)veh;
            //if (myVehicle == null || !myVehicle.Exists) return;
            player.Emit("Client:drawMarkerVehicle", veh.Position);
            return;
        }
        [AsyncClientEvent("Server:Raycast:ReportPlayer")]
        public static void PlayerGetPosition(IPlayer player, IPlayer target)
        {
            //if (target == null || !target.Exists) return;

            player.Emit("Client:drawMarkerPlayer", target.Position);
            return;
        }

        [AsyncClientEvent("Server:Raycast:LockVehicle")]
        public void LockVehicle(IPlayer player, IVehicle veh)
        {
            if (player == null || !player.Exists || veh == null || !veh.Exists) return;
            int charId = User.GetPlayerOnline(player);
            long vehID = veh.GetVehicleId();
            string vehPlate = veh.NumberplateText;
            if (charId <= 0 || vehID <= 0) return;
            if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
            if (!player.Position.IsInRange(veh.Position, 8f)) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast dich zu weit vom Fahrzeug entfernt."); return; }
            if (ServerVehicles.GetVehicleFactionId(veh) == 0 && ServerVehicles.GetVehicleType(veh) == 0 && !CharactersInventory.ExistCharacterItem(charId, "Fahrzeugschluessel " + vehPlate, "schluessel")) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast keinen Schlüssel für dieses Fahrzeug."); return; }
            else if (ServerVehicles.GetVehicleFactionId(veh) != 0 && vehPlate.Contains(ServerFactions.GetFactionShortName(ServerVehicles.GetVehicleFactionId(veh))))
            {
                string factionPlate = ServerFactions.GetFactionShortName(ServerVehicles.GetVehicleFactionId(veh));
                if (!CharactersInventory.ExistCharacterItem(charId, "Fahrzeugschluessel " + factionPlate, "schluessel")) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast keinen Schlüssel für dieses Fahrzeug ({factionPlate})."); return; }
            }
            else if (ServerVehicles.GetVehicleType(veh) == 1)
            {
                if (ServerVehicles.GetVehicleFactionId(veh) == 0) return;
                string factionPlate = ServerFactions.GetFactionShortName(ServerVehicles.GetVehicleFactionId(veh));
                if (!vehPlate.Contains(factionPlate)) return;
                if (!CharactersInventory.ExistCharacterItem(charId, "Fahrzeugschluessel " + factionPlate, "schluessel")) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast keinen Schlüssel für dieses Fahrzeug ({factionPlate})."); return; }
            }
            else if (ServerVehicles.GetVehicleType(veh) == 2 && ServerVehicles.GetVehicleOwner(veh) != charId) { HUDHandler.SendNotification(player, 3, 2500, "Du hast keinen Schlüssel."); return; }
            else if (ServerVehicles.GetVehicleFactionId(veh) != 0 && !vehPlate.Contains(ServerFactions.GetFactionShortName(ServerVehicles.GetVehicleFactionId(veh))) && !CharactersInventory.ExistCharacterItem(charId, "Fahrzeugschluessel " + vehPlate, "inventory")) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast keinen Schlüssel für dieses Fahrzeug."); return; }

            bool LockState = ServerVehicles.GetVehicleLockState(veh);
            ServerVehicles.SetVehicleLockState(veh, !LockState);
            //player.EmitLocked("Client:Inventory:PlayAnimation", "anim@mp_player_intupperface_palm", "fob_click_fp", 1000, 49, false);
            if (LockState == true)
            {
                HUDHandler.SendNotification(player, 1, 2500, "Du hast das Fahrzeug aufgeschlossen.");
            }
            else
            {
                //player.EmitLocked("Client:Inventory:PlayAnimation", "anim@mp_player_intupperface_palm", "fob_click_fp", 1000, 49, false);
                HUDHandler.SendNotification(player, 1, 2500, "Du hast das Fahrzeug abgeschlossen.");
                veh.SetVehicleTrunkState(false);
                Global.mGlobal.VirtualAPI.TriggerClientEventSafe(player, "Client:Vehicles:ToggleDoorState", veh, 5, false);
            }
        }

        [AsyncClientEvent("Server:Raycast:ToggleVehicleEngine")]
        public void ToggleVehicleEngine(IPlayer player, IVehicle veh)
        {
            if (player == null || !player.Exists || veh == null || !veh.Exists) return;
            int charId = User.GetPlayerOnline(player);
            long vehID = veh.GetVehicleId();
            string vehPlate = veh.NumberplateText;
            if (charId <= 0 || vehID <= 0 || player.Seat != 1) return;
            if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
            if (!player.Position.IsInRange(veh.Position, 8f)) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast dich zu weit vom Fahrzeug entfernt."); return; }
            if (ServerVehicles.GetVehicleFactionId(veh) == 0 && ServerVehicles.GetVehicleType(veh) == 0 && !CharactersInventory.ExistCharacterItem(charId, "Fahrzeugschluessel " + vehPlate, "schluessel")) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast keinen Schlüssel für dieses Fahrzeug."); return; }
            else if (ServerVehicles.GetVehicleFactionId(veh) != 0 && vehPlate.Contains(ServerFactions.GetFactionShortName(ServerVehicles.GetVehicleFactionId(veh))))
            {
                string factionPlate = ServerFactions.GetFactionShortName(ServerVehicles.GetVehicleFactionId(veh));
                if (!CharactersInventory.ExistCharacterItem(charId, "Fahrzeugschluessel " + factionPlate, "schluessel")) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast keinen Schlüssel für dieses Fahrzeug ({factionPlate})."); return; }
            }
            else if (ServerVehicles.GetVehicleType(veh) == 1)
            {
                if (ServerVehicles.GetVehicleFactionId(veh) == 0) return;
                string factionPlate = ServerFactions.GetFactionShortName(ServerVehicles.GetVehicleFactionId(veh));
                if (!vehPlate.Contains(factionPlate)) return;
                if (!CharactersInventory.ExistCharacterItem(charId, "Fahrzeugschluessel " + factionPlate, "schluessel")) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast keinen Schlüssel für dieses Fahrzeug ({factionPlate})."); return; }
            }
            else if (ServerVehicles.GetVehicleType(veh) == 2 && ServerVehicles.GetVehicleOwner(veh) != charId) { HUDHandler.SendNotification(player, 3, 2500, "Du hast keinen Schlüssel."); return; }
            else if (ServerVehicles.GetVehicleFactionId(veh) != 0 && !vehPlate.Contains(ServerFactions.GetFactionShortName(ServerVehicles.GetVehicleFactionId(veh))) && !CharactersInventory.ExistCharacterItem(charId, "Fahrzeugschluessel " + vehPlate, "schluessel")) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast keinen Schlüssel für dieses Fahrzeug."); return; }

            bool engineState = ServerVehicles.GetVehicleEngineState(veh);
            if (engineState == false && !ServerVehicles.IsVehicleEngineHealthy(veh)) { HUDHandler.SendNotification(player, 3, 2500, "Dieses Fahrzeug hat einen Motorschaden."); return; }
            if (engineState == false && ServerVehicles.GetVehicleFuel(veh) <= 0) { HUDHandler.SendNotification(player, 3, 2500, "Dieses Fahrzeug hat keinen Treibstoff mehr."); return; }
            ServerVehicles.SetVehicleEngineState(veh, !engineState);
            if (engineState == true) { HUDHandler.SendNotification(player, 1, 2500, "Du hast den Motor ausgeschaltet."); }
            else { HUDHandler.SendNotification(player, 1, 2500, "Du hast den Motor eingeschaltet."); }
        }

        [AsyncClientEvent("Server:Raycast:OpenCloseVehicleTrunk")]
        public void OpenCloseVehicleTrunk(IPlayer player, IVehicle veh)
        {
            try
            {
                if (player == null || !player.Exists || veh == null || !veh.Exists) return;
                int charId = User.GetPlayerOnline(player);
                long vehID = veh.GetVehicleId();
                string vehPlate = veh.NumberplateText;
                if (charId <= 0 || vehID <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                if (!player.Position.IsInRange(veh.Position, 5f)) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast dich zu weit vom Fahrzeug entfernt."); return; }
                if (ServerVehicles.GetVehicleLockState(veh)) { HUDHandler.SendNotification(player, 1, 2500, "Das Fahrzeug ist abgeschlossen."); return; }
                bool isTrunkOpened = veh.GetVehicleTrunkState(); //false = Zu | True = offen
                if (!isTrunkOpened)
                {
                    veh.SetVehicleTrunkState(true);
                    Alt.EmitAllClients("Client:Vehicles:ToggleDoorState", veh, 5, true);
                    HUDHandler.SendNotification(player, 1, 2500, "Du hast den Kofferraum geöffnet.");
                    return;
                }
                else
                {
                    veh.SetVehicleTrunkState(false);
                    Alt.EmitAllClients("Client:Vehicles:ToggleDoorState", veh, 5, false);
                    HUDHandler.SendNotification(player, 1, 2500, "Du hast den Kofferraum geschlossen");
                    return;
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Raycast:OpenCloseVehicleHood")]
        public void OpenCloseVehicleHood(IPlayer player, IVehicle veh)
        {
            try
            {
                if (player == null || !player.Exists || veh == null || !veh.Exists) return;
                int charId = User.GetPlayerOnline(player);
                long vehID = veh.GetVehicleId();
                string vehPlate = veh.NumberplateText;
                if (charId <= 0 || vehID <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                if (!player.Position.IsInRange(veh.Position, 5f)) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast dich zu weit vom Fahrzeug entfernt."); return; }
                if (ServerVehicles.GetVehicleLockState(veh)) { HUDHandler.SendNotification(player, 1, 2500, "Das Fahrzeug ist abgeschlossen."); return; }
                bool isHoodOpened = veh.GetVehicleHoodState(); //false = Zu | True = offen
                if (!isHoodOpened)
                {
                    veh.SetVehicleHoodState(true);
                    Alt.EmitAllClients("Client:Vehicles:ToggleDoorState", veh, 4, true);
                    HUDHandler.SendNotification(player, 1, 2500, "Du hast die Motorhaube geöffnet.");
                    return;
                }
                else
                {
                    veh.SetVehicleHoodState(false);
                    Alt.EmitAllClients("Client:Vehicles:ToggleDoorState", veh, 4, false);
                    HUDHandler.SendNotification(player, 1, 2500, "Du hast die Motorhaube geschlossen");
                    return;
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Raycast:ViewVehicleTrunk")]
        public void ViewVehicleTrunk(ClassicPlayer player, ClassicVehicle veh)
        {
            try
            {
                if (player == null || !player.Exists || veh == null || !veh.Exists || player.CharacterId <= 0 || veh.VehicleId <= 0) return;
                if (player.IsInVehicle) return;
                if (!player.Position.IsInRange(veh.Position, 5f)) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast dich zu weit vom Fahrzeug entfernt."); return; }
                if (ServerVehicles.GetVehicleLockState(veh)) { HUDHandler.SendNotification(player, 1, 2500, "Das Fahrzeug ist abgeschlossen."); return; }
                bool isTrunkOpened = veh.GetVehicleTrunkState(); //false = Zu | True = offen
                if (!isTrunkOpened) { HUDHandler.SendNotification(player, 1, 2500, "Der Kofferraum ist zu."); return; }
                var characterInvArray = CharactersInventory.GetCharacterInventory(player.CharacterId); //Inventar Items
                var vehicleTrunkArray = ServerVehicles.GetVehicleTrunkItems(veh.VehicleId, false); //Kofferraum Items
                var curVehWeight = ServerVehicles.GetVehicleVehicleTrunkWeight(veh.VehicleId, false);
                var maxVehWeight = ServerVehicles.GetVehicleTrunkCapacityOnHash(veh.Model);
                player.EmitLocked("Client:VehicleTrunk:openCEF", player.CharacterId, veh.VehicleId, "trunk", characterInvArray, vehicleTrunkArray, curVehWeight, maxVehWeight); //trunk oder glovebox
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Raycast:ViewVehicleGlovebox")]
        public void ViewVehicleGlovebox(ClassicPlayer player, ClassicVehicle veh)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0 || veh == null || !veh.Exists || veh.VehicleId <= 0) return;
                if (!player.IsInVehicle) return;
                if (player.Seat != 1 && player.Seat != 2) return;
                var characterInvArray = CharactersInventory.GetCharacterInventory(player.CharacterId); //Inventar Items
                var vehicleGloveboxArray = ServerVehicles.GetVehicleTrunkItems(veh.VehicleId, true); //Handschuhfach Items
                var curVehWeight = ServerVehicles.GetVehicleVehicleTrunkWeight(veh.VehicleId, true);
                var maxVehWeight = 5f;
                //Alt.Log($"{player.Name} ({player.CharacterId}) öffnet Handschuhfach von {veh.VehicleId}: {characterInvArray} ||| {vehicleGloveboxArray}");
                player.EmitLocked("Client:VehicleTrunk:openCEF", player.CharacterId, veh.VehicleId, "glovebox", characterInvArray, vehicleGloveboxArray, curVehWeight, maxVehWeight); //trunk oder glovebox
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Raycast:showPlayerSupportId")]
        public void showPlayerSupportId(IPlayer player, IPlayer targetPlayer)
        {
            if (player == null || !player.Exists || targetPlayer == null || !targetPlayer.Exists) return;
            int targetCharId = User.GetPlayerOnline(targetPlayer);
            if (targetCharId == 0) return;
            HUDHandler.SendNotification(player, 1, 2500, $"Die Charakter-ID des Spielers lautet: {targetCharId}");
        }

        [AsyncClientEvent("Server:Raycast:givePlayerItemRequest")]
        public void givePlayerItemRequest(IPlayer player, IPlayer targetPlayer)
        {
            if (player == null || !player.Exists || targetPlayer == null || !targetPlayer.Exists) return;
            int charId = User.GetPlayerOnline(player);
            int targetCharId = User.GetPlayerOnline(targetPlayer);
            if (charId <= 0 || targetCharId <= 0) return;
            if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
            player.EmitLocked("Client:Inventory:CreateInventory", CharactersInventory.GetCharacterInventory(User.GetPlayerOnline(player)), Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(User.GetPlayerOnline(player))), targetCharId);
        }

        [AsyncClientEvent("Server:Raycast:OpenGivePlayerBillCEF")]
        public void OpenGivePlayerBillCEF(IPlayer player, IPlayer targetPlayer, string type) //Types:  faction
        {
            try
            {
                if (player == null || !player.Exists || targetPlayer == null || !targetPlayer.Exists) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                if (type != "faction") return;
                int charId = User.GetPlayerOnline(player);
                int targetCharId = User.GetPlayerOnline(targetPlayer);
                if (charId <= 0 || targetCharId <= 0) return;
                if (type == "faction")
                {
                    if (!ServerFactions.IsCharacterInAnyFaction(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Du bist in keiner Fraktion."); return; }
                    if (!ServerFactions.IsCharacterInFactionDuty(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Du bist nicht im Dienst."); return; }
                    int factionId = ServerFactions.GetCharacterFactionId(charId);
                    if (factionId <= 0) return;
                    player.EmitLocked("Client:GivePlayerBill:openCEF", "faction", targetCharId);
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Raycast:openGivePlayerLicenseCEF")]
        public void openGivePlayerLicenseCEF(IPlayer player, IPlayer targetPlayer)
        {
            try
            {
                if (player == null || !player.Exists || targetPlayer == null || !targetPlayer.Exists) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                int charId = User.GetPlayerOnline(player);
                int targetCharId = User.GetPlayerOnline(targetPlayer);
                if (charId <= 0 || targetCharId <= 0) return;
                if (!ServerFactions.IsCharacterInAnyFaction(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Du bist in keiner Fraktion."); return; }
                if (!ServerFactions.IsCharacterInFactionDuty(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Du bist nicht im Dienst."); return; }
                if (ServerFactions.GetCharacterFactionId(charId) != 5) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Du bist kein Teil der Fahrschule."); return; }
                var licArray = CharactersLicenses.GetCharacterLicenses(targetCharId);
                player.EmitLocked("Client:GivePlayerLicense:openCEF", targetCharId, licArray);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:PlayerBill:giveBill")]
        public void PlayerBillGiveBill(IPlayer player, string type, string reason, int targetCharId, int moneyAmount) //Types:  faction
        {
            try
            {
                if (player == null || !player.Exists || targetCharId <= 0 || moneyAmount <= 0 || reason == null || reason == "") return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                if (type != "faction") return;
                int charId = User.GetPlayerOnline(player);
                if (charId == 0) return;
                var targetPlayer = Alt.GetAllPlayers().ToList().FirstOrDefault(x => x.GetCharacterMetaId() == (ulong)targetCharId);
                if (targetPlayer == null || !targetPlayer.Exists) return;
                int factionCompanyId = 0;
                string factionCompanyName = "None";
                if (type == "faction")
                {
                    if (!ServerFactions.IsCharacterInAnyFaction(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Du bist in keiner Fraktion"); return; }
                    if (!ServerFactions.IsCharacterInFactionDuty(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Du bist nicht im Dienst."); return; }
                    factionCompanyId = ServerFactions.GetCharacterFactionId(charId);
                    factionCompanyName = ServerFactions.GetFactionFullName(factionCompanyId);
                }
                if (factionCompanyId <= 0 || factionCompanyName == "None" || factionCompanyName == "") return;
                targetPlayer.EmitLocked("Client:RecievePlayerBill:openCEF", type, factionCompanyId, moneyAmount, reason, factionCompanyName, charId);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:PlayerBill:BillAction")]
        public void PlayerBillAction(IPlayer player, string action, string type, int factionCompanyId, int moneyAmount, string reason, int givenBillOwnerCharId)
        {
            try
            {
                if (player == null || !player.Exists || action == "" || type == "" || factionCompanyId <= 0 || moneyAmount <= 0 || reason == "" || givenBillOwnerCharId <= 0) return;
                if (type != "faction" && type != "company") return;
                if (action != "bar" && action != "bank" && action != "decline") return;
                int targetCharId = User.GetPlayerOnline(player);
                if (targetCharId <= 0) return;
                var givenBillPlayer = Alt.GetAllPlayers().ToList().FirstOrDefault(x => x.GetCharacterMetaId() == (ulong)givenBillOwnerCharId);
                int charId = User.GetPlayerOnline(player);
                if (givenBillPlayer == null || !givenBillPlayer.Exists) return;
                string factionCompanyName = "None";
                if (type == "faction") { factionCompanyName = ServerFactions.GetFactionFullName(factionCompanyId); }
                if (factionCompanyName == "None" || factionCompanyName == "" || factionCompanyName == "Zivilist") return;
                DateTime dateTime = DateTime.Now;
                if (action == "bar")
                {
                    if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                    if (!CharactersInventory.ExistCharacterItem(targetCharId, "Bargeld", "brieftasche")) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht genügend Bargeld dabei ({moneyAmount}$)."); HUDHandler.SendNotification(givenBillPlayer, 3, 2500, "Die Person hat nicht genügend Bargeld dabei."); return; }
                    if (CharactersInventory.GetCharacterItemAmount(targetCharId, "Bargeld", "brieftasche") < moneyAmount) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht genügend Bargeld dabei ({moneyAmount}$)."); HUDHandler.SendNotification(givenBillPlayer, 3, 2500, "Die Person hat nicht genügend Bargeld dabei."); return; }
                    CharactersInventory.RemoveCharacterItemAmount(targetCharId, "Bargeld", moneyAmount, "brieftasche");
                    CharactersInventory.AddCharacterItem(charId, "Bargeld", moneyAmount / 2, "brieftasche");
                }
                else if (action == "bank")
                {
                    if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                    if (!CharactersBank.HasCharacterBankMainKonto(targetCharId)) { HUDHandler.SendNotification(player, 3, 2500, "Du hast noch kein Hauptkonto in der Bank gesetzt."); HUDHandler.SendNotification(givenBillPlayer, 3, 2500, "Die Person hat noch kein Hauptkonto gesetzt."); return; }
                    int accountNumber = CharactersBank.GetCharacterBankMainKonto(targetCharId);
                    if (accountNumber <= 0) return;
                    if (CharactersBank.GetBankAccountLockStatus(accountNumber)) { HUDHandler.SendNotification(player, 3, 2500, "Dein Hauptkonto ist aktuell gesperrt."); HUDHandler.SendNotification(givenBillPlayer, 3, 2500, "Das Hauptkonto der Person ist gesperrt."); return; }
                    if (CharactersBank.GetBankAccountMoney(accountNumber) < moneyAmount) { HUDHandler.SendNotification(player, 3, 2500, $"Dein Bankkonto ist nicht ausreichend gedeckt ({moneyAmount}$)."); HUDHandler.SendNotification(givenBillPlayer, 3, 2500, "Die Person hat nicht genügend Geld auf ihrem Bankkonto."); return; }
                    CharactersBank.SetBankAccountMoney(accountNumber, CharactersBank.GetBankAccountMoney(accountNumber) - moneyAmount);
                    ServerBankPapers.CreateNewBankPaper(accountNumber, dateTime.ToString("dd.MM.yyyy"), dateTime.ToString("HH.mm"), "Ausgehende Überweisung", $"{factionCompanyName}", $"Rechnungskartenzahlung", $"-{moneyAmount}$", "Online Banking");
                }
                else if (action == "decline")
                {
                    HUDHandler.SendNotification(givenBillPlayer, 4, 2500, $"Die Person hat die Rechnung i.H.v. {moneyAmount}$ abgelehnt.");
                    HUDHandler.SendNotification(player, 4, 2500, $"Du hast die Rechnung i.H.v. {moneyAmount}$ abgelehnt.");
                    return;
                }

                if (type == "faction")
                {
                    ServerFactions.SetFactionBankMoney(factionCompanyId, ServerFactions.GetFactionBankMoney(factionCompanyId) + moneyAmount);
                }

                HUDHandler.SendNotification(player, 2, 2500, $"Du hast die Rechnung i.H.v. {moneyAmount}$ bezahlt (Zahlungsart: {action}).");
                HUDHandler.SendNotification(givenBillPlayer, 2, 2500, $"Die Person hat die Rechnung i.H.v. {moneyAmount}$ bezahlt (Zahlungsart: {action}).");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Raycast:GiveTakeHandcuffs")]
        public void GiveTakeHandcuffs(IPlayer player, IPlayer targetPlayer)
        {
            try
            {
                if (player == null || targetPlayer == null || !player.Exists || !targetPlayer.Exists) return;
                if (!player.Position.IsInRange(targetPlayer.Position, 3f)) { HUDHandler.SendNotification(player, 3, 2500, "Du bist zu weit entfernt."); return; }
                int charId = User.GetPlayerOnline(player);
                int targetCharId = User.GetPlayerOnline(targetPlayer);
                if (charId <= 0 || targetCharId <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                bool hasTargetHandcuffs = targetPlayer.HasPlayerHandcuffs();
                bool hasTargetRopeCuffs = targetPlayer.HasPlayerRopeCuffs();

                if (hasTargetRopeCuffs) { HUDHandler.SendNotification(player, 3, 2500, "Die Person ist gefesselt."); return; }

                if (hasTargetHandcuffs)
                {
                    //TargetPlayer hat Handschellen.
                    if (!CharactersInventory.ExistCharacterItem(charId, "Handschellenschluessel", "schluessel")) { HUDHandler.SendNotification(player, 3, 2500, "Du hast keinen Schlüssel."); return; }
                    InventoryHandler.StopAnimation(targetPlayer, "mp_arresting", "sprint");
                    targetPlayer.SetPlayerIsCuffed("handcuffs", false);
                    HUDHandler.SendNotification(targetPlayer, 3, 2500, "Dir wurden die Handschellen abgenommen.");
                    float itemWeight = ServerItems.GetItemWeight("Handschellen");
                    float invWeight = CharactersInventory.GetCharacterItemWeight(charId, "inventory");
                    float backpackWeight = CharactersInventory.GetCharacterItemWeight(charId, "backpack");
                    if (invWeight + itemWeight > 5f && backpackWeight + itemWeight > Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId))) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht genug Platz in deinen Taschen."); return; }
                    if (invWeight + itemWeight <= 5f)
                    {
                        HUDHandler.SendNotification(player, 3, 2500, $"Handschellen abgenommen.");
                        CharactersInventory.AddCharacterItem(charId, "Handschellen", 1, "inventory");
                    }

                    if (Characters.GetCharacterBackpack(charId) != -2 && backpackWeight + itemWeight <= Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId)))
                    {
                        HUDHandler.SendNotification(player, 3, 2500, $"Handschellen abgenommen.");
                        CharactersInventory.AddCharacterItem(charId, "Handschellen", 1, "backpack");
                    }
                    return;
                }
                else
                {
                    //TargetPlayer hat keine Handschellen.
                    if (!CharactersInventory.ExistCharacterItem(charId, "Handschellen", "inventory") && !CharactersInventory.ExistCharacterItem(charId, "Handschellen", "backpack")) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Du hast keine Handschellen."); return; }
                    if (CharactersInventory.ExistCharacterItem(charId, "Handschellen", "inventory") && CharactersInventory.GetCharacterItemAmount(charId, "Handschellen", "inventory") > 0) { CharactersInventory.RemoveCharacterItemAmount(charId, "Handschellen", 1, "inventory"); }
                    else if (CharactersInventory.ExistCharacterItem(charId, "Handschellen", "backpack") && CharactersInventory.GetCharacterItemAmount(charId, "Handschellen", "backpack") > 0) { CharactersInventory.RemoveCharacterItemAmount(charId, "Handschellen", 1, "backpack"); }
                    InventoryHandler.InventoryAnimation(targetPlayer, "handcuffs", -1);
                    targetPlayer.SetPlayerIsCuffed("handcuffs", true);
                    targetPlayer.GiveWeapon(WeaponModel.Fist, 0, true);
                    HUDHandler.SendNotification(targetPlayer, 1, 2500, "Dir wurden Handschellen angelegt.");
                    HUDHandler.SendNotification(player, 1, 2500, "Handschellen angelegt.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Raycast:GiveTakeRopeCuffs")]
        public void GiveTakeRopeCuffs(IPlayer player, IPlayer targetPlayer)
        {
            try
            {
                if (player == null || targetPlayer == null || !player.Exists || !targetPlayer.Exists) return;
                if (!player.Position.IsInRange(targetPlayer.Position, 3f)) { HUDHandler.SendNotification(player, 3, 2500, "Du bist zu weit entfernt."); return; }
                int charId = User.GetPlayerOnline(player);
                int targetCharId = User.GetPlayerOnline(targetPlayer);
                if (charId <= 0 || targetCharId <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                bool hasTargetHandCuffs = targetPlayer.HasPlayerHandcuffs();
                bool hasTargetRopeCuffs = targetPlayer.HasPlayerRopeCuffs();

                if (hasTargetHandCuffs) { HUDHandler.SendNotification(player, 3, 2500, "Der Spieler ist bereits Gefesselt."); return; }

                if (hasTargetRopeCuffs)
                {
                    //TargetPlayer hat Seilfesseln.
                    if (!CharactersInventory.ExistCharacterItem(charId, "Messer", "inventory") && !CharactersInventory.ExistCharacterItem(charId, "Springmesser", "inventory")) { HUDHandler.SendNotification(player, 3, 2500, "Du hast kein Messer dabei."); return; }
                    InventoryHandler.StopAnimation(targetPlayer, "mp_arresting", "sprint");
                    targetPlayer.SetPlayerIsCuffed("ropecuffs", false);
                    HUDHandler.SendNotification(targetPlayer, 1, 2500, "Dir wurden die Kabelbinder abgenommen.");

                    float itemWeight = ServerItems.GetItemWeight("Kabelbinder");
                    float invWeight = CharactersInventory.GetCharacterItemWeight(charId, "inventory");
                    float backpackWeight = CharactersInventory.GetCharacterItemWeight(charId, "backpack");
                    if (invWeight + itemWeight > 5f && backpackWeight + itemWeight > Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId))) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht genug Platz in deinen Taschen."); return; }
                    if (invWeight + itemWeight <= 5f)
                    {
                        HUDHandler.SendNotification(player, 1, 2500, $"Kabelbinder abgenommen.");
                        CharactersInventory.AddCharacterItem(charId, "Kabelbinder", 1, "inventory");
                    }

                    if (Characters.GetCharacterBackpack(charId) != -2 && backpackWeight + itemWeight <= Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId)))
                    {
                        HUDHandler.SendNotification(player, 1, 2500, $"Kabelbinder abgenommen.");
                        CharactersInventory.AddCharacterItem(charId, "Kabelbinder", 1, "backpack");
                    }
                    return;
                }
                else
                {
                    //TargetPlayer hat keine Seilfesseln.
                    if (!CharactersInventory.ExistCharacterItem(charId, "Kabelbinder", "inventory") && !CharactersInventory.ExistCharacterItem(charId, "Kabelbinder", "backpack")) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Du hast kein Kabelbinder dabei."); return; }
                    if (CharactersInventory.ExistCharacterItem(charId, "Kabelbinder", "inventory") && CharactersInventory.GetCharacterItemAmount(charId, "Kabelbinder", "inventory") > 0) { CharactersInventory.RemoveCharacterItemAmount(charId, "Kabelbinder", 1, "inventory"); }
                    else if (CharactersInventory.ExistCharacterItem(charId, "Kabelbinder", "backpack") && CharactersInventory.GetCharacterItemAmount(charId, "Kabelbinder", "backpack") > 0) { CharactersInventory.RemoveCharacterItemAmount(charId, "Kabelbinder", 1, "backpack"); }
                    InventoryHandler.InventoryAnimation(targetPlayer, "handcuffs", -1);
                    targetPlayer.SetPlayerIsCuffed("ropecuffs", true);
                    targetPlayer.GiveWeapon(WeaponModel.Fist, 0, true);
                    HUDHandler.SendNotification(targetPlayer, 1, 2500, "Du wurdest mit einem Kabelbinder gefesselt.");
                    HUDHandler.SendNotification(player, 1, 2500, "Kabelbinder angelegt.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Raycast:SearchPlayerInventory")]
        public void SearchPlayerInventory(IPlayer player, IPlayer targetPlayer)
        {
            try
            {
                if (player == null || targetPlayer == null || !player.Exists || !targetPlayer.Exists) return;
                if (!player.Position.IsInRange(targetPlayer.Position, 3f)) { HUDHandler.SendNotification(player, 3, 2500, "Du bist zu weit entfernt."); return; }
                int charId = User.GetPlayerOnline(player);
                int targetCharId = User.GetPlayerOnline(targetPlayer);
                if (charId <= 0 || targetCharId <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                if (!targetPlayer.HasPlayerHandcuffs() && !targetPlayer.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Dieser Spieler ist nicht gefesselt."); return; }
                var targetInvArray = CharactersInventory.GetCharacterInventory(targetCharId); //Inventar Items des zu durchsuchenden Spielers
                player.EmitLocked("Client:PlayerSearch:openCEF", targetCharId, targetInvArray);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:KeyHandler:PressF9")]
        public void PressF9(IPlayer player)
        {
            Alt.Emit("PlayerLoggedIn", player, player.GetCharacterMetaId());
            player.EmitLocked("SaltyChat:EnablePlayer");
        }
        #region Show Documents

        [AsyncClientEvent("Server:Raycast:showIdentityCard")]
        public void showIdentityCard(IPlayer player, IPlayer targetPlayer)
        {
            try
            {
                if (player == null || targetPlayer == null || !player.Exists || !targetPlayer.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                int targetId = (int)targetPlayer.GetCharacterMetaId();
                if (charId <= 0 || targetId <= 0) return;
                if (Characters.GetCharacterAccState(charId) <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 5000, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                var data = "[]";

                data = Characters.GetCharacterInformations(charId);
                if (data == null || data == "[]") return;
                player.EmitLocked("Client:IdentityCard:showIdentityCard", "perso", data);
                targetPlayer.EmitLocked("Client:IdentityCard:showIdentityCard", "perso", data);
                InventoryHandler.InventoryAnimation(player, "give", 0);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        //Needs to be Tested
        [AsyncClientEvent("Server:Raycast:showDriversLic")]
        public void showDriversLic(IPlayer player, IPlayer targetPlayer)
        {
            try
            {
                if (player == null || targetPlayer == null || !player.Exists || !targetPlayer.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                int targetId = (int)targetPlayer.GetCharacterMetaId();
                if (charId <= 0 || targetId <= 0) return;
                if (Characters.GetCharacterAccState(charId) <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 5000, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                var data = "[]";

                data = CharactersLicenses.GetDriverLicenses(charId);
                if (data == null || data == "[]") return;
                player.EmitLocked("Client:IdentityCard:showDriversLic", "drivers", data);
                targetPlayer.EmitLocked("Client:IdentityCard:showDriversLic", "drivers", data);
                InventoryHandler.InventoryAnimation(player, "give", 0);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        //ToDo
        [AsyncClientEvent("Server:Raycast:showWepLic")]
        public void showWepLic(IPlayer player, IPlayer targetPlayer)
        {
            try
            {
                if (player == null || targetPlayer == null || !player.Exists || !targetPlayer.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                int targetId = (int)targetPlayer.GetCharacterMetaId();
                if (charId <= 0 || targetId <= 0) return;
                if (Characters.GetCharacterAccState(charId) <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 5000, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                var data = "[]";

                data = Characters.GetCharacterInformations(charId);
                if (data == null || data == "[]") return;
                player.EmitLocked("Client:IdentityCard:showWepLic", "wep", data);
                targetPlayer.EmitLocked("Client:IdentityCard:showWepLic", "wep", data);
                InventoryHandler.InventoryAnimation(player, "give", 0);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [AsyncClientEvent("Server:Raycast:showFactionCard")]
        public void showFactionCard(IPlayer player, IPlayer targetPlayer)
        {
            try
            {
                if (player == null || targetPlayer == null || !player.Exists || !targetPlayer.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                int targetId = (int)targetPlayer.GetCharacterMetaId();
                if (charId <= 0 || targetId <= 0) return;
                if (Characters.GetCharacterAccState(charId) <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 5000, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                var data = "[]";

                data = Characters.GetCharacterFactionInformations(charId);
                if (data == null || data == "[]") return;
                player.EmitLocked("Client:IdentityCard:showFactionCard", "faction", data);
                targetPlayer.EmitLocked("Client:IdentityCard:showFactionCard", "faction", data);
                InventoryHandler.InventoryAnimation(player, "give", 0);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [AsyncClientEvent("Server:Raycast:showPoliceCard")]
        public void showPoliceCard(IPlayer player, IPlayer targetPlayer)
        {
            try
            {
                if (player == null || targetPlayer == null || !player.Exists || !targetPlayer.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                int targetId = (int)targetPlayer.GetCharacterMetaId();
                if (charId <= 0 || targetId <= 0) return;
                if (Characters.GetCharacterAccState(charId) <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 5000, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                var data = "[]";

                data = Characters.GetCharacterFactionInformations(charId);
                if (data == null || data == "[]") return;
                player.EmitLocked("Client:IdentityCard:showPoliceCard", "police", data);
                targetPlayer.EmitLocked("Client:IdentityCard:showPoliceCard", "police", data);
                InventoryHandler.InventoryAnimation(player, "give", 0);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [AsyncClientEvent("Server:Raycast:showFIBCard")]
        public void showFIBCard(IPlayer player, IPlayer targetPlayer)
        {
            try
            {
                if (player == null || targetPlayer == null || !player.Exists || !targetPlayer.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                int targetId = (int)targetPlayer.GetCharacterMetaId();
                if (charId <= 0 || targetId <= 0) return;
                if (Characters.GetCharacterAccState(charId) <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 5000, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                var data = "[]";

                data = Characters.GetCharacterFactionInformations(charId);
                if (data == null || data == "[]") return;
                player.EmitLocked("Client:IdentityCard:showFIBCard", "fib", data);
                targetPlayer.EmitLocked("Client:IdentityCard:showFIBCard", "fib", data);
                InventoryHandler.InventoryAnimation(player, "give", 0);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [AsyncClientEvent("Server:Raycast:showLSMDCard")]
        public void showLSMDCard(IPlayer player, IPlayer targetPlayer)
        {
            try
            {
                if (player == null || targetPlayer == null || !player.Exists || !targetPlayer.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                int targetId = (int)targetPlayer.GetCharacterMetaId();
                if (charId <= 0 || targetId <= 0) return;
                if (Characters.GetCharacterAccState(charId) <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 5000, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                var data = "[]";

                data = Characters.GetCharacterFactionInformations(charId);
                if (data == null || data == "[]") return;
                player.EmitLocked("Client:IdentityCard:showLSMDCard", "lsmd", data);
                targetPlayer.EmitLocked("Client:IdentityCard:showLSMDCard", "lsmd", data);
                InventoryHandler.InventoryAnimation(player, "give", 0);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        #endregion
        [AsyncClientEvent("Server:Raycast:BreakVehicleEngine")]
        public async Task BreakVehicleEngine(IPlayer player, IVehicle veh)
        {
            if (player == null || !player.Exists || veh == null || !veh.Exists) return;
            int charId = User.GetPlayerOnline(player);
            long vehID = veh.GetVehicleId();
            if (charId <= 0 || vehID <= 0 || player.Seat != 1) return;
            if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
            if (!player.Position.IsInRange(veh.Position, 8f)) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast dich zu weit vom Fahrzeug entfernt."); return; }
            if (!CharactersInventory.ExistCharacterItem(charId, "Kabelbaum", "inventory")) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast keinen Dietrich um das Fahrzeug aufzubrechen"); return; }

            bool engineState = ServerVehicles.GetVehicleEngineState(veh);
            if (engineState == false && !ServerVehicles.IsVehicleEngineHealthy(veh)) { HUDHandler.SendNotification(player, 1, 2500, "Dieses Fahrzeug hat einen Motorschaden."); return; }
            if (engineState == false && ServerVehicles.GetVehicleFuel(veh) <= 0) { HUDHandler.SendNotification(player, 1, 2500, "Dieses Fahrzeug hat keinen Treibstoff mehr."); return; }

            if (engineState == false)
            {

                HUDHandler.SendProgress(player, "Fahrzeug wird Kurzgeschlossen", "alert", 15000);
                await Task.Delay(15000);

                int rnd = new Random().Next(0, 100);

                if (rnd <= 50)
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Kurzschließen Fehlgeschlagen.");
                    CharactersInventory.RemoveCharacterItemAmount2(charId, "Kabelbaum", 1);
                    return;
                }
                else if (rnd <= 100 && rnd > 50)
                {
                    ServerVehicles.SetVehicleEngineState(veh, !engineState);
                    HUDHandler.SendNotification(player, 2, 2500, "Du hast das Fahrzeug kurzgeschlossen.");
                    CharactersInventory.RemoveCharacterItemAmount2(charId, "Kabelbaum", 1);
                }

            }
            else if (engineState == true)
            {
                HUDHandler.SendNotification(player, 1, 2500, "Du hast den Motor ausgeschaltet.");
            }
        }

        [AsyncClientEvent("Server:Raycast:BreakVehicle")]
        public async Task BreakVehicle(IPlayer player, IVehicle veh)
        {
            if (player == null || !player.Exists || veh == null || !veh.Exists) return;
            int charId = User.GetPlayerOnline(player);
            long vehID = veh.GetVehicleId();
            string vehPlate = veh.NumberplateText;
            if (charId <= 0 || vehID <= 0) return;
            if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
            if (!player.Position.IsInRange(veh.Position, 8f)) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast dich zu weit vom Fahrzeug entfernt."); return; }
            if (!CharactersInventory.ExistCharacterItem(charId, "Dietrich", "inventory")) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast keinen Dietrich um das Fahrzeug aufzubrechen"); return; }


            bool LockState = ServerVehicles.GetVehicleLockState(veh);
            if (LockState == true)
            {
                player.EmitLocked("Client:Inventory:PlayAnimation", "anim@amb@clubhouse@tutorial@bkr_tut_ig3@", "machinic_loop_mechandplayer", 15000, 49, false);
                HUDHandler.SendProgress(player, "Fahrzeug wird Aufgebrochen", "alert", 15000);
                await Task.Delay(15000);
                int rnd = new Random().Next(0, 100);

                if (ServerFactions.GetFactionDutyMemberCount(2) + ServerFactions.GetFactionDutyMemberCount(12) < 2)
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Die Karre ist ziemlich gut gesichert...");
                    return;
                }

                if (rnd <= 50)
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Dietrich abgebrochen. Aufbrechen Fehlgeschlagen.");
                    CharactersInventory.RemoveCharacterItemAmount2(charId, "Dietrich", 1);
                    return;
                }
                else if (rnd <= 100 && rnd > 50)
                {
                    ServerVehicles.SetVehicleLockState(veh, !LockState);
                    HUDHandler.SendNotification(player, 2, 2500, "Du hast das Fahrzeug aufgebrochen.");
                    CharactersInventory.RemoveCharacterItemAmount2(charId, "Dietrich", 1);
                    ServerFactions.AddNewFactionDispatchNoName("Stiller Alarm", 2, "Ein Fahrzeug Diebstahl wurde gemeldet.", player.Position);
                    ServerFactions.AddNewFactionDispatchNoName("Stiller Alarm", 12, "Ein Fahrzeug Diebstahl wurde gemeldet.", player.Position);
                    foreach (var p in Alt.Server.GetPlayers().Where(x => x != null && x.Exists && x.GetCharacterMetaId() > 0).ToList())
                    {
                        if (!ServerFactions.IsCharacterInAnyFaction((int)p.GetCharacterMetaId()) || !ServerFactions.IsCharacterInFactionDuty((int)p.GetCharacterMetaId()) || ServerFactions.GetCharacterFactionId((int)p.GetCharacterMetaId()) != 2) continue;
                        HUDHandler.SendNotification(p, 3, 3500, "Ein Fahrzeug Diebstahl wurde gemeldet.");
                    }
                }
            }
            else
            {
                HUDHandler.SendNotification(player, 1, 2500, "Das Fahrzeug ist bereits offen...");
                return;
            }
        }
        [AsyncClientEvent("Server:Raycast:showChangeOwnerHUD")]
        public void showChangeOwnerHUD(ClassicPlayer player, ClassicVehicle veh)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0 || veh == null || !veh.Exists || veh.VehicleId <= 0 || ServerVehicles.GetVehicleOwner(veh) != player.CharacterId) return;
                player.EmitLocked("Client:Vehicle:showChangeOwnerHUD", ServerAllVehicles.GetVehicleNameOnHash(veh.Model));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        [AsyncClientEvent("Server:Vehicle:changeVehOwner")]
        public void changeVehOwner(ClassicPlayer player, int targetId)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0 || targetId <= 0 || player.Vehicle == null || !player.Vehicle.Exists) return;
                ClassicVehicle veh = (ClassicVehicle)player.Vehicle;
                /*                if (veh == null || !veh.Exists || veh.VehicleId <= 0 || ServerVehicles.GetVehicleOwner(veh) != player.CharacterId) return;
                */
                ClassicPlayer targetPlayer = (ClassicPlayer)Alt.GetAllPlayers().ToList().FirstOrDefault(x => x != null && x.Exists && ((ClassicPlayer)x).CharacterId == targetId);
                if (targetPlayer == null || !targetPlayer.Exists)
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Die eingegebene Spieler-ID ist nicht online.");
                    return;
                }

                if (!targetPlayer.Position.IsInRange(player.Position, 13f))
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Der ausgewählte Spieler ist nicht im Umfeld.");
                    return;
                }

                if (veh == null || !veh.Exists || veh.VehicleId <= 0 || ServerVehicles.GetVehicleOwner(veh) != player.CharacterId)
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Nicht dein Fahrzeug! / Fahrzeug nicht erfasst!");
                    return;
                }

                models.Server_Vehicles dbVeh = Model.ServerVehicles.ServerVehicles_.FirstOrDefault(x => x.id == veh.VehicleId);
                if (dbVeh == null) return;

                //entfernen
                var keyAmount = CharactersInventory.GetCharacterItemAmount(player.CharacterId, $"Fahrzeugschluessel {veh.NumberplateText}", "schluessel");
                CharactersInventory.RemoveCharacterItemAmount(player.CharacterId, $"Fahrzeugschluessel {veh.NumberplateText}", keyAmount, "schluessel");
                //CharactersInventory.RemoveCharacterItem(player.CharacterId, $"Fahrzeugschluessel {veh.NumberplateText}", "schluessel");

                //geben
                CharactersInventory.AddCharacterItem(targetId, $"Fahrzeugschluessel {veh.NumberplateText}", keyAmount, "schluessel");

                //Notification
                HUDHandler.SendNotification(player, 2, 2500, "Du hast das Fahrzeug erfolgreich überschrieben.");
                HUDHandler.SendNotification(targetPlayer, 1, 2500, $"{Characters.GetCharacterName(player.CharacterId)} hat dir sein Fahrzeug überschrieben.");

                dbVeh.charid = targetId;
                using (var db = new models.gtaContext())
                {
                    db.Server_Vehicles.Update(dbVeh);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
