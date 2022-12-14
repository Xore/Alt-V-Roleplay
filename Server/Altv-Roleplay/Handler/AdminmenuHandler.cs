using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;
using Newtonsoft.Json.Linq;

namespace Altv_Roleplay.Handler
{
    class AdminmenuHandler : IScript
    {

        [AsyncClientEvent("Server:AdminMenu:OpenMenu")]
        public void AdminmenuOpenMenu(IPlayer player)
        {
            try
            {
                if (player.AdminLevel() != 0) player.EmitLocked("Client:Adminmenu:OpenMenu");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:AdminMenu:CloseMenu")]
        public void AdminmenuCloseMenu(IPlayer player)
        {
            try
            {
                if (player.AdminLevel() != 0) player.EmitLocked("Client:Adminmenu:CloseMenu");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:AdminMenu:DoAction")]
        public void AdminmenuDoAction(IPlayer player, string action, string info, string addinfo, string inputvalue)
        {
            try
            {
                if (player.AdminLevel() != 0)
                {
                    switch (action)
                    {
                        // PLAYER
                        case "noclip":
                            player.EmitLocked("Client:AdminMenu:Noclip", info);

                            var text = "";
                            if (info == "on") text = " **NoClip** [ON]";
                            else text = " **NoClip** [OFF]";
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + text);
                            break;
                        case "unsichtbar":
                            if (player.Visible) player.Visible = false;
                            else player.Visible = true;

                            if (info == "on") text = " **Invisible** [ON]";
                            else text = " **Invisible** [OFF]";
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + text);
                            break;
                        case "adminoutfit":
                            SetAdminClothes(player, info);

                            if (info == "on") text = " **Adminoutfit** [ON]";
                            else text = " **Adminoutfit** [OFF]";
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + text);
                            break;
                        case "godmode":
                            player.EmitLocked("Client:AdminMenu:Godmode", info);

                            if (info == "on") text = " **Godmode** [ON]";
                            else text = " **Godmode** [OFF]";
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + text);
                            break;
                        case "heilen":
                            player.Health = 200;

                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sich **geheilt **");
                            break;
                        case "wiederbeleben":
                            DeathHandler.revive(player);
                            Alt.Emit("SaltyChat:SetPlayerAlive", player, true);

                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sich **wiederbelebt**.");
                            break;
                        // ONLINE
                        case "spieler_kicken":
                            var kickedPlayer = Alt.GetAllPlayers().ToList().FirstOrDefault(x => User.GetPlayerUsername(((ClassicPlayer)x).accountId) == addinfo);
                            if (string.IsNullOrWhiteSpace(inputvalue))
                            {
                                kickedPlayer.kickWithMessage("Du wurdest von einem Teammitglied gekickt.");
                                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat " + Characters.GetCharacterName((int)kickedPlayer.GetCharacterMetaId()) + " **gekickt**.");
                            }
                            else
                            {
                                kickedPlayer.kickWithMessage("Du wurdest von einem Teammitglied gekickt. Grund: " + inputvalue);
                                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat " + Characters.GetCharacterName((int)kickedPlayer.GetCharacterMetaId()) + " **gekickt**. Grund: " + inputvalue);
                            }
                            break;
                        case "spieler_bannen":
                            var bannedPlayer = Alt.GetAllPlayers().ToList().FirstOrDefault(x => User.GetPlayerUsername(((ClassicPlayer)x).accountId) == addinfo);
                            if (string.IsNullOrWhiteSpace(inputvalue))
                            {
                                User.SetPlayerBanned(((ClassicPlayer)bannedPlayer).accountId, true, $"Gebannt von {Characters.GetCharacterName(User.GetPlayerOnline(player))}");
                                if (bannedPlayer != null) bannedPlayer.Kick("Du wurdest von einem Teammitglied gebannt.");
                                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat " + Characters.GetCharacterName((int)bannedPlayer.GetCharacterMetaId()) + " **gebannt**.");
                            }
                            else
                            {
                                User.SetPlayerBanned(((ClassicPlayer)bannedPlayer).accountId, true, $"Gebannt von {Characters.GetCharacterName(User.GetPlayerOnline(player))}. Grund: " + inputvalue);
                                if (bannedPlayer != null) bannedPlayer.Kick("Du wurdest von einem Teammitglied gebannt. Grund: " + inputvalue);
                                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat " + Characters.GetCharacterName((int)bannedPlayer.GetCharacterMetaId()) + " **gebannt**. Grund: " + inputvalue);
                            }
                            break;
                        case "spieler_einfrieren":
                            var kickedPlayerr = Alt.GetAllPlayers().ToList().FirstOrDefault(x => User.GetPlayerUsername(((ClassicPlayer)x).accountId) == addinfo);
                            Alt.EmitAllClients("Client:AdminMenu:SetFreezed", kickedPlayerr, info);

                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat " + Characters.GetCharacterName((int)kickedPlayerr.GetCharacterMetaId()) + " **eingefroren**.");
                            break;
                        case "spieler_spectaten":
                            var spectatedPlayer = Alt.GetAllPlayers().ToList().FirstOrDefault(x => User.GetPlayerUsername(((ClassicPlayer)x).accountId) == addinfo);
                            player.Emit("Client:AdminMenu:Spectate", spectatedPlayer, info);

                            if (info == "on") { text = " **spectated** nun " + Characters.GetCharacterName((int)spectatedPlayer.GetCharacterMetaId()); Alt.EmitAllClients("Client:AdminMenu:SetInvisible", player, "on"); }
                            else { text = " hat aufgehört, " + Characters.GetCharacterName((int)spectatedPlayer.GetCharacterMetaId()) + " **spectaten**."; Alt.EmitAllClients("Client:AdminMenu:SetInvisible", player, "off"); }
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + text);
                            break;
                        case "spieler_heilen":
                            var HealedPlayer = Alt.GetAllPlayers().ToList().FirstOrDefault(x => User.GetPlayerUsername(((ClassicPlayer)x).accountId) == addinfo);
                            HealedPlayer.Health = 200;

                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sich zu " + Characters.GetCharacterName((int)HealedPlayer.GetCharacterMetaId()) + " **geheilt**.");
                            break;
                        case "spieler_wiederbeleben":
                            var RevivedPlayer = Alt.GetAllPlayers().ToList().FirstOrDefault(x => User.GetPlayerUsername(((ClassicPlayer)x).accountId) == addinfo);
                            DeathHandler.revive(RevivedPlayer);
                            Alt.Emit("SaltyChat:SetPlayerAlive", RevivedPlayer, true);

                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat " + Characters.GetCharacterName((int)RevivedPlayer.GetCharacterMetaId()) + " zu sich **wiederbelebt**.");
                            break;
                        case "tp_zu_spieler":
                            var TeleportToPlayer = Alt.GetAllPlayers().ToList().FirstOrDefault(x => User.GetPlayerUsername(((ClassicPlayer)x).accountId) == addinfo);
                            player.Position = new Position(TeleportToPlayer.Position.X, TeleportToPlayer.Position.Y, TeleportToPlayer.Position.Z);

                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sich zu " + Characters.GetCharacterName((int)TeleportToPlayer.GetCharacterMetaId()) + " **teleportiert**.");
                            break;
                        case "spieler_zu_mir_tp":
                            var TeleportPlayer = Alt.GetAllPlayers().ToList().FirstOrDefault(x => User.GetPlayerUsername(((ClassicPlayer)x).accountId) == addinfo);
                            TeleportPlayer.Position = new Position(player.Position.X, player.Position.Y, player.Position.Z);

                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat " + Characters.GetCharacterName((int)TeleportPlayer.GetCharacterMetaId()) + " zu sich **teleportiert**.");
                            break;
                        case "item_geben":
                            if (string.IsNullOrWhiteSpace(inputvalue)) break;
                            if (!ServerItems.ExistItem(ServerItems.ReturnNormalItemName(inputvalue))) { HUDHandler.SendNotification(player, 3, 2500, $"Itemname nicht gefunden: {inputvalue}"); break; }
                            var GiveItemPlayer = Alt.GetAllPlayers().ToList().FirstOrDefault(x => User.GetPlayerUsername(((ClassicPlayer)x).accountId) == addinfo);

                            CharactersInventory.AddCharacterItem((int)GiveItemPlayer.GetCharacterMetaId(), inputvalue, 1, "inventory");
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat " + Characters.GetCharacterName((int)GiveItemPlayer.GetCharacterMetaId()) + " das **Item " + inputvalue + " gegeben**.");
                            break;
                        case "adminlevel_geben":
                            if (string.IsNullOrWhiteSpace(inputvalue)) break;
                            var GiveAdminPlayer = Alt.GetAllPlayers().ToList().FirstOrDefault(x => User.GetPlayerUsername(((ClassicPlayer)x).accountId) == addinfo);
                            if (!Int32.TryParse(inputvalue, out int newinputvaluea)) { HUDHandler.SendNotification(player, 1, 2500, $"Du musst eine Zahl angeben"); break; }
                            if (player.AdminLevel() <= newinputvaluea) { HUDHandler.SendNotification(player, 3, 2500, $"Du darfst dieses Adminlevel nicht vergeben"); break; }
                            User.SetPlayerAdminLevel(((ClassicPlayer)GiveAdminPlayer).accountId, newinputvaluea);

                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat " + Characters.GetCharacterName((int)GiveAdminPlayer.GetCharacterMetaId()) + " **Adminlevel " + newinputvaluea + "** zugewiesen.");
                            break;
                        // MISC
                        case "zum_wegpunkt":
                            player.Emit("Client:AdminMenu:GetWaypointInfo");
                            break;
                        case "nametags":
                            player.Emit("Client:Adminmenu:ToggleNametags", info);
                            break;
                        case "spieler_auf_der_karte_anzeigen":
                            player.Emit("Client:Adminmenu:TogglePlayerBlips", info);
                            break;
                        // FARHEUG
                        case "fahrzeug_spawnen":
                            if (string.IsNullOrWhiteSpace(inputvalue)) break;
                            try
                            {
                                IVehicle veh = Alt.CreateVehicle(inputvalue, player.Position, player.Rotation);
                                veh.EngineOn = true;
                                veh.LockState = VehicleLockState.Unlocked;
                                player.Emit("Client:Utilities:setIntoVehicle", veh);
                                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sich das **Fahrzeug " + inputvalue + " gespawnt**");
                                break;
                            }
                            catch (Exception)
                            {
                                HUDHandler.SendNotification(player, 3, 2500, "Dieses Fahrzeug existiert nicht");
                                break;
                            }
                        case "reparieren":
                            if (player.Vehicle == null || !player.Vehicle.Exists) break;
                            ServerVehicles.SetVehicleEngineHealthy(player.Vehicle, true);
                            Alt.EmitAllClients("Client:Utilities:repairVehicle", player.Vehicle);
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Fahrzeug repariert**");
                            break;
                        case "fahrzeug_löschen":
                            if (player.Vehicle == null || !player.Vehicle.Exists) break;
                            player.Vehicle.Remove();

                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Fahrzeug gelöscht**");
                            break;
                        case "fahrzeugmotor_an_ausschalten":
                            if (player.Vehicle == null || !player.Vehicle.Exists) break;
                            if (!player.Vehicle.EngineOn) player.Vehicle.EngineOn = true;
                            else player.Vehicle.EngineOn = false;
                            break;
                        // PEDS
                        case "zurücksetzen":
                            if (!Characters.GetCharacterGender((int)player.GetCharacterMetaId())) player.Model = Alt.Hash("mp_m_freemode_01");
                            else player.Model = Alt.Hash("mp_f_freemode_01");

                            Characters.SetCharacterCorrectClothes(player);

                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Outfit zurückgesetzt**");
                            break;
                        case "a_c_boar":
                            player.Model = Alt.Hash(action);
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Model zu " + action + " geändert**");
                            break;
                        case "a_c_cat_01":
                            player.Model = Alt.Hash(action);
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Model zu " + action + " geändert**");
                            break;
                        case "a_c_chimp":
                            player.Model = Alt.Hash(action);
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Model zu " + action + " geändert**");
                            break;
                        case "a_c_chop":
                            player.Model = Alt.Hash(action);
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Model zu " + action + " geändert**");
                            break;
                        case "a_c_cow":
                            player.Model = Alt.Hash(action);
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Model zu " + action + " geändert**");
                            break;
                        case "a_c_coyote":
                            player.Model = Alt.Hash(action);
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Model zu " + action + " geändert**");
                            break;
                        case "a_c_crow":
                            player.Model = Alt.Hash(action);
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Model zu " + action + " geändert**");
                            break;
                        case "a_c_husky":
                            player.Model = Alt.Hash(action);
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Model zu " + action + " geändert**");
                            break;
                        case "a_c_mtlion":
                            player.Model = Alt.Hash(action);
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Model zu " + action + " geändert**");
                            break;
                        case "a_c_poodle":
                            player.Model = Alt.Hash(action);
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Model zu " + action + " geändert**");
                            break;
                        case "a_c_pug":
                            player.Model = Alt.Hash(action);
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Model zu " + action + " geändert**");
                            break;
                        case "a_c_rabbit_01":
                            player.Model = Alt.Hash(action);
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Model zu " + action + " geändert**");
                            break;
                        case "a_c_retriever":
                            player.Model = Alt.Hash(action);
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Model zu " + action + " geändert**");
                            break;
                        case "a_c_shepherd":
                            player.Model = Alt.Hash(action);
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Model zu " + action + " geändert**");
                            break;
                        case "a_c_westy":
                            player.Model = Alt.Hash(action);
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Model zu " + action + " geändert**");
                            break;
                        case "other":
                            if (string.IsNullOrWhiteSpace(inputvalue)) break;
                            try
                            {
                                player.Model = Alt.Hash(inputvalue);
                                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sein **Model zu " + inputvalue + " geändert**");
                                break;
                            }
                            catch (Exception)
                            {
                                HUDHandler.SendNotification(player, 3, 2500, "Dieses Model existiert nicht");
                                break;
                            }
                        // SERVER
                        case "ankündigung":
                            if (string.IsNullOrWhiteSpace(inputvalue)) break;
                            foreach (var client in Alt.GetAllPlayers())
                            {
                                if (client == null || !client.Exists) continue;
                                HUDHandler.SendNotification(client, 3, 2500, inputvalue);
                            }
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat eine Ankündigung gemacht: **" + inputvalue + "**");
                            break;
                        case "whitelist":
                            if (string.IsNullOrWhiteSpace(inputvalue)) break;
                            if (!User.ExistPlayerName(inputvalue)) { HUDHandler.SendNotification(player, 3, 2500, $"Benutzername {inputvalue} wurde nicht gefunden"); break; }
                            if (User.IsPlayerWhitelisted(inputvalue)) { HUDHandler.SendNotification(player, 3, 2500, $"Spieler {inputvalue} ist bereits gewhitelisted"); break; }
                            User.SetPlayerWhitelistState(User.GetPlayerAccountIdByUsername(inputvalue), true);
                            HUDHandler.SendNotification(player, 2, 2500, inputvalue + " wurde erfolgreich gewhitelisted");

                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat **" + inputvalue + " gewhitelistet**");
                            break;
                        case "fahrzeug_einparken":
                            if (string.IsNullOrWhiteSpace(inputvalue)) break;
                            var vehicle = Alt.GetAllVehicles().ToList().FirstOrDefault(x => x != null && x.Exists && x.HasVehicleId() && (int)x.GetVehicleId() > 0 && x.NumberplateText.ToLower() == inputvalue.ToLower());
                            if (vehicle == null) { HUDHandler.SendNotification(player, 3, 2500, $"Fahrzeug mit dem Kennzeichen {inputvalue} existiert nicht"); break; }
                            ServerVehicles.SetVehicleInGarage(vehicle, true, 1);
                            HUDHandler.SendNotification(player, 2, 2500, $"Fahrzeug mit dem Kennzeichen {inputvalue} in die Pillbox Garage eingeparkt");

                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat das **Fahrzeug mit dem Kennzeichen " + inputvalue + " eingeparkt**");
                            break;
                        case "alle_fahrzeuge_einparken":
                            int count = 0;
                            foreach (var veh in Alt.GetAllVehicles().ToList().Where(x => x != null && x.Exists && x.HasVehicleId()))
                            {
                                if (veh == null || !veh.Exists || !veh.HasVehicleId()) continue;
                                int currentGarageId = ServerVehicles.GetVehicleGarageId(veh);
                                if (currentGarageId <= 0) continue;
                                ServerVehicles.SetVehicleInGarage(veh, true, currentGarageId);
                                count++;
                            }

                            HUDHandler.SendNotification(player, 1, 2500, $"{count} Fahrzeuge eingeparkt");
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat ** alle Fahrzeuge (" + count + ") eingeparkt**");
                            break;
                        case "fahrzeuginhaber_finden":
                            if (string.IsNullOrWhiteSpace(inputvalue)) break;
                            var fvehicle = Alt.GetAllVehicles().ToList().FirstOrDefault(x => x != null && x.Exists && x.HasVehicleId() && (int)x.GetVehicleId() > 0 && x.NumberplateText.ToLower() == inputvalue.ToLower());
                            if (fvehicle == null) { HUDHandler.SendNotification(player, 3, 2500, $"Fahrzeug mit dem Kennzeichen {inputvalue} existiert nicht"); break; }
                            var ownerId = ServerVehicles.GetVehicleOwner(fvehicle);
                            var findvehownermsg = $"AccId: " + User.GetPlayerByCharId(ownerId).playerid + " | CharId: " + ownerId + " | Benutzername: " + User.GetPlayerUsername(User.GetPlayerByCharId(ownerId).playerid) + " | Name: " + Characters.GetCharacterName(ownerId);
                            HUDHandler.SendNotification(player, 1, 2500, findvehownermsg);
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " nach dem Besitzer des Fahrzeuges mit dem Kennzeichen " + inputvalue + " gesucht.\nResultat: **" + findvehownermsg + "**");
                            break;
                        case "hardwareid_zurücksetzen":
                            if (string.IsNullOrWhiteSpace(inputvalue)) break;
                            if (!User.ExistPlayerName(inputvalue)) { HUDHandler.SendNotification(player, 3, 2500, $"Benutzername {inputvalue} wurde nicht gefunden"); break; }
                            User.ResetPlayerHardwareID(User.GetPlayerAccountIdByUsername(inputvalue));
                            HUDHandler.SendNotification(player, 2, 2500, "Hardware-ID von " + inputvalue + " wurde erfolgreich zurückgesetzt");
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat die **Hardware-ID von " + inputvalue + " wurde zurückgesetzt**");
                            break;
                        case "socialclubid_zurücksetzen":
                            if (string.IsNullOrWhiteSpace(inputvalue)) break;
                            if (!User.ExistPlayerName(inputvalue)) { HUDHandler.SendNotification(player, 3, 2500, $"Benutzername {inputvalue} wurde nicht gefunden"); break; }
                            HUDHandler.SendNotification(player, 2, 2500, "Socialclub-ID von " + inputvalue + " wurde erfolgreich zurückgesetzt");
                            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat die **Socialclub-ID von " + inputvalue + " wurde zurückgesetzt**");
                            break;
                        // DEFAULT
                        default:
                            Console.WriteLine("Missing Adminmenu Action: " + action);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:AdminMenu:RequestAllOnlinePlayers")]
        public void RequestAllOnlinePlayers(IPlayer player)
        {
            try
            {
                if (player.AdminLevel() != 0)
                {
                    try
                    {
                        dynamic array = new JArray() as dynamic;
                        dynamic entry = new JObject();

                        foreach (var plr in Alt.GetAllPlayers().Where(p => p != null && p.Exists))
                        {
                            if (((ClassicPlayer)plr).accountId == 0 || (int)plr.GetCharacterMetaId() == 0 || Characters.GetCharacterName((int)plr.GetCharacterMetaId()) == "SYSTEM" || User.GetPlayerUsername(((ClassicPlayer)plr).accountId) == "Undefined") continue;
                            entry = new JObject();
                            entry.accid = ((ClassicPlayer)plr).accountId;
                            entry.charid = (int)plr.GetCharacterMetaId();
                            entry.fullname = Characters.GetCharacterName((int)plr.GetCharacterMetaId());
                            entry.username = User.GetPlayerUsername(((ClassicPlayer)plr).accountId);
                            array.Add(entry);
                        }

                        player.Emit("Client:AdminMenu:SendAllOnlinePlayers", array.ToString());
                    }
                    catch (Exception e)
                    {
                        Alt.Log($"{e}");
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:AdminMenu:TeleportWaypoint")]
        public void TeleportWaypoint(IPlayer player, int x, int y, int z)
        {
            try
            {
                if (player.AdminLevel() != 0)
                {
                    try
                    {
                        player.Position = new Position(x, y, z + 5);
                        DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat sich **zum Wegpunkt teleportiert**.");
                    }
                    catch (Exception e)
                    {
                        Alt.Log($"{e}");
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:AdminMenu:GetPlayer")]
        public void GetPlayer(IPlayer player, string reason, string username, string other)
        {
            try
            {
                if (player.AdminLevel() != 0)
                {
                    var GetPlayerPlayer = Alt.GetAllPlayers().ToList().FirstOrDefault(x => User.GetPlayerUsername(((ClassicPlayer)x).accountId) == username);
                    if (reason == "GetPlayerMeta") player.Emit("Client:Adminmenu:ReceiveMeta", GetPlayerPlayer);
                    else if (reason == "SetMeta") player.Emit("Client:Adminmenu:SetMetaDef", GetPlayerPlayer, other);
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static void SetAdminClothes(IPlayer player, string info)
        {
            int charId = User.GetPlayerOnline(player);
            var gun = WeaponModel.CarbineRifleMkII;
            if (info == "on")
            {
                if (!Characters.GetCharacterGender((int)player.GetCharacterMetaId()))
                {
                    //Männlich
                    player.EmitLocked("Client:SpawnArea:setCharClothes", 1, 125, 0);
                    player.EmitLocked("Client:SpawnArea:setCharClothes", 3, 16, 0);
                    player.EmitLocked("Client:SpawnArea:setCharClothes", 4, 130, 1);
                    player.EmitLocked("Client:SpawnArea:setCharClothes", 6, 24, 0);
                    player.EmitLocked("Client:SpawnArea:setCharClothes", 7, 155, 0);
                    player.EmitLocked("Client:SpawnArea:setCharClothes", 8, 191, 0);
                    player.EmitLocked("Client:SpawnArea:setCharClothes", 9, 62, 1);
                    player.EmitLocked("Client:SpawnArea:setCharClothes", 11, 336, 3);
                    player.EmitLocked("Client:SpawnArea:setCharAccessory", 0, 119, 0);
                }
                else
                {
                    //Weiblich
                    player.EmitLocked("Client:SpawnArea:setCharClothes", 1, 125, 0);
                    player.EmitLocked("Client:SpawnArea:setCharClothes", 3, 111, 0);
                    player.EmitLocked("Client:SpawnArea:setCharClothes", 4, 136, 1);
                    player.EmitLocked("Client:SpawnArea:setCharClothes", 6, 24, 0);
                    player.EmitLocked("Client:SpawnArea:setCharClothes", 7, 124, 0);
                    player.EmitLocked("Client:SpawnArea:setCharClothes", 8, 223, 0);
                    player.EmitLocked("Client:SpawnArea:setCharClothes", 9, 58, 1);
                    player.EmitLocked("Client:SpawnArea:setCharClothes", 11, 351, 3);
                    player.EmitLocked("Client:SpawnArea:setCharAccessory", 0, 118, 0);
                }
                player.GiveWeapon(gun, 250, true);
                player.AddWeaponComponent(WeaponModel.CarbineRifleMkII, 0x9D65907A);//GRIP
                player.AddWeaponComponent(WeaponModel.CarbineRifleMkII, 0x7BC4CDDC);//FLASHLIGHT
                player.AddWeaponComponent(WeaponModel.CarbineRifleMkII, 0x420FD713);//HOLOSIGHT
                player.AddWeaponComponent(WeaponModel.CarbineRifleMkII, 0x837445AA);//SUPPRESSOR
                Characters.SetCharacterArmor(charId, 100);
            }
            else if (info == "off")
            {
                player.RemoveWeapon(4208062921);
                Characters.SetCharacterCorrectClothes(player);
                Characters.SetCharacterArmor(charId, 0);
            }
        }
    }
}
