using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using AltV.Net.Resources.Chat.Api;
using Altv_Roleplay.Database;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.models;
using Altv_Roleplay.Utils;
using System;
using System.IO;
using System.Linq;

namespace Altv_Roleplay.Handler
{
    public class Commands : IScript
    {
        #region Utils
        [Command("resethwidbyid")]
        public void CMD_ResetHwIdById(IPlayer player, int accountId)
        {
            try
            {
                if (player == null || !player.Exists) return;
                if (player.GetCharacterMetaId() <= 0 || player.AdminLevel() <= 2) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
                if (!User.ExistPlayerById(accountId)) { HUDHandler.SendNotification(player, 4, 2500, "Der Spieler existiert nicht."); return; }
                User.ResetPlayerHardwareID(accountId);
                HUDHandler.SendNotification(player, 1, 2500, $"Hardware-ID zurückgesetzt (Acc-ID: {accountId}).");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat die **Hardware-ID von " + accountId + " wurde zurückgesetzt**");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [Command("resethwidbyname")]
        public void CMD_ResetHwIdByName(IPlayer player, string username)
        {
            try
            {
                if (player == null || !player.Exists) return;
                if (player.GetCharacterMetaId() <= 0 || player.AdminLevel() <= 2) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
                if (User.GetPlayerAccountIdByUsername(username) == 0) { HUDHandler.SendNotification(player, 4, 2500, "Der Spieler existiert nicht."); return; }
                int accountId = User.GetPlayerAccountIdByUsername(username);
                HUDHandler.SendNotification(player, 1, 2500, $"Hardware-ID zurückgesetzt (Acc-ID: {accountId}).");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat die **Hardware-ID von " + accountId + " wurde zurückgesetzt**");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [Command("getaccidbyname")]
        public void CMD_GetAccIdByName(IPlayer player, string username)
        {
            try
            {
                if (player == null || !player.Exists) return;
                if (player.GetCharacterMetaId() <= 0 || player.AdminLevel() <= 2) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
                if (User.GetPlayerAccountIdByUsername(username) == 0) { HUDHandler.SendNotification(player, 4, 2500, "Der Spieler existiert nicht."); return; }

                HUDHandler.SendNotification(player, 1, 2500, $"Hardware-ID zurückgesetzt (Acc-ID: {User.GetPlayerAccountIdByUsername(username)}).");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [Command("pos")]
        public void PosCMD(IPlayer player)
        {
            if (player == null || !player.Exists) return;
            if (player.AdminLevel() <= 2) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
            player.SendChatMessage($"{player.Position.ToString()}");
            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " Pos:" + player.Position.ToString());
        }
        [Command("pos2")]
        public static void PosCMD(IPlayer player, string coordName)
        {
            if (player.AdminLevel() < 2)
            {
                HUDHandler.SendNotification(player, 4, 5000, "Keine Rechte.");
                return;
            }

            if (coordName == null)
            {
                HUDHandler.SendNotification(player, 4, 5000, "Kein Namen angegeben!");
                return;
            }

            Position playerPosGet = player.Position;
            Rotation playerRotGet = player.Rotation;

            StreamWriter coordsFile;
            if (!File.Exists("SavedCoords.txt"))
            {
                coordsFile = new StreamWriter("SavedCoords.txt");
            }
            else
            {
                coordsFile = File.AppendText("SavedCoords.txt");
            }
            HUDHandler.SendNotification(player, 1, 8000, "Saved");
            coordsFile.WriteLine("| " + coordName + " | " + playerPosGet.ToString() + " | " + playerRotGet.ToString());
            coordsFile.Close();
        }
        [Command("kick")]
        public static void cmd_KICK(IPlayer player, int charId)
        {
            try
            {
                if (player == null || !player.Exists || charId <= 0 || player.AdminLevel() <= 1) return;
                var targetP = Alt.GetAllPlayers().ToList().FirstOrDefault(x => x != null && x.Exists && User.GetPlayerOnline(x) == charId);
                if (targetP == null) return;
                targetP.Kick("");
                HUDHandler.SendNotification(player, 2, 2500, $"Spieler mit Char-ID {charId} Erfolgreich gekickt.");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat Spieler " + charId + " Gekickt");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [Command("ban")]
        public static void cmd_BAn(IPlayer player, int accId)
        {
            try
            {
                if (player == null || !player.Exists || accId <= 0 || player.AdminLevel() <= 2) return;
                User.SetPlayerBanned(accId, true, $"Gebannt von {Characters.GetCharacterName(User.GetPlayerOnline(player))}");
                var targetP = Alt.GetAllPlayers().ToList().FirstOrDefault(x => x != null && x.Exists && User.GetPlayerAccountId(x) == accId);
                if (targetP != null) targetP.Kick("");
                HUDHandler.SendNotification(player, 2, 2500, $"Spieler mit ID {accId} Erfolgreich gebannt.");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat Spieler " + accId + " Gebannt");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [Command("unban")]
        public static void CMD_Unban(ClassicPlayer player, int accId)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0 || accId <= 0 || player.AdminLevel() <= 3) return;
                User.SetPlayerBanned(accId, false, "");
                HUDHandler.SendNotification(player, 2, 2500, $"Spieler mit ID {accId} Erfolgreich entbannt.");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat Spieler " + accId + " Entbannt");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        #endregion
        #region Set
        [Command("setdim")]
        public void setDimCMD(IPlayer player, int charId, int dimension)
        {
            if (player == null || !player.Exists || charId == 0) return;
            if (player.AdminLevel() <= 0) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }

            var targetPlayer = Alt.GetAllPlayers().FirstOrDefault(x => (int)x.GetCharacterMetaId() == charId);
            if (targetPlayer != null)
            {
                targetPlayer.Dimension = dimension;
                targetPlayer.SetStreamSyncedMetaData("dimension", targetPlayer.Dimension);
                Alt.Emit("SaltyChat:PlayerChangeDimension", targetPlayer);
                player.SendChatMessage($"Send player{targetPlayer.Name} to: {dimension}");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat Spieler " + targetPlayer + " Dimension auf" + dimension + "Gesetzt");
            }
        }
        [Command("setatm")]
        public void SetATMCmd(IPlayer player, string name)
        {
            try
            {
                if (player == null || !player.Exists) return;
                if (player.AdminLevel() < 3) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
                ServerATM.CreateNewATM(player, 0, player.Position, name);
                HUDHandler.SendNotification(player, 2, 2500, $"ATM mit dem Namen > {name} erfolgreich gesetzt.");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " ATM:" + name + " Pos:" + player.Position + " Erstellt");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
                HUDHandler.SendNotification(player, 3, 2500, $"ein fehler ist aufgetreten!");
            }
        }
        /*[Command("setstore")] ###ToDo###
        public void SetstoreCmd(IPlayer player, int shopid, string shopname)
        {
            try
            {
                int charId = User.GetPlayerOnline(player);
                if (player.AdminLevel() < 6)
                {
                    HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte.");
                    return;
                }
                ServerShops.CreateServerShop(player, shopid, shopname, player.Position);
                HUDHandler.SendNotification(player, 2, 2500, $"Shop gesetzt > {shopname} - ID: {shopid}");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " Shop:" + shopname + "" + shopid + " Erstellt");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
                HUDHandler.SendNotification(player, 4, 2500, $"ein fehler ist aufgetreten!");
            }
        }*/
        #endregion
        #region Items
        [Command("cash")]
        public void GiveItemCMD(IPlayer player, int itemAmount)
        {
            if (player == null || !player.Exists) return;
            if (player.AdminLevel() <= 8) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
            ulong charId = player.GetCharacterMetaId();
            if (charId <= 0) return;
            CharactersInventory.AddCharacterItem((int)charId, "Bargeld", itemAmount, "brieftasche");
            HUDHandler.SendNotification(player, 2, 2500, $"{itemAmount}$ erhalten (Bargeld).");
            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " Cash:" + itemAmount + "");
        }
        [Command("wep")]
        public void wCMD(IPlayer player, WeaponModel wp)
        {
            if (player.AdminLevel() <= 8) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
            try
            {
                if (player == null || !player.Exists) return;
                player.GiveWeapon(wp, 9999, true);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [Command("giveitem")]
        public void GiveItemCMD(IPlayer player, string itemName, int itemAmount)
        {
            if (player == null || !player.Exists) return;
            if (player.AdminLevel() <= 8) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
            if (!ServerItems.ExistItem(ServerItems.ReturnNormalItemName(itemName))) { HUDHandler.SendNotification(player, 4, 2500, $"Itemname nicht gefunden: {itemName}"); return; }
            ulong charId = player.GetCharacterMetaId();
            if (charId <= 0) return;
            CharactersInventory.AddCharacterItem((int)charId, itemName, itemAmount, "inventory");
            HUDHandler.SendNotification(player, 2, 2500, $"Gegenstand '{itemName}' ({itemAmount}x) erhalten.");
            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " GiveItem:" + itemName + "" + itemAmount + "");
        }
        #endregion
        #region Cars
        [Command("car")]
        public void heyCMD(IPlayer player, string model)
        {
            if (player == null || !player.Exists) return;
            if (player.AdminLevel() <= 1) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
            if (player.Vehicle != null && player.Vehicle.Exists) player.Vehicle.Remove();
            IVehicle veh = Alt.CreateVehicle(model, player.Position, player.Rotation);
            veh.EngineOn = true;
            veh.LockState = VehicleLockState.Unlocked;
            veh.NumberplateText = "RENTAL";
            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " Car:" + model + " Pos:" + player.Position + "");
        }
        [Command("rentcar")]
        public void rentcar(IPlayer player)// <-- ToDo
        {
            if (player == null || !player.Exists) return;
            if (player.AdminLevel() <= 1) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
            if (player.Vehicle != null && player.Vehicle.Exists) player.Vehicle.Remove();
            IVehicle veh = Alt.CreateVehicle(4192631813, player.Position, player.Rotation);
            veh.EngineOn = true;
            veh.LockState = VehicleLockState.Unlocked;
            veh.NumberplateText = "RENTAL";
            //DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " Car:" + model + " Pos:" + player.Position + "");
        }
        [Command("dv")]
        public void delcarCMD(IPlayer player)
        {
            try
            {
                if (player == null || !player.Exists || player.Vehicle == null || !player.Vehicle.Exists) return;
                if (player.AdminLevel() <= 1) return;
                else
                {
                    if (player.IsInVehicle)
                    {
                        HUDHandler.SendNotification(player, 2, 2500, "Du hast ein Fahrzeug gelöscht.");
                        player.Vehicle.Remove();
                    }
                    else HUDHandler.SendNotification(player, 4, 2500, "Du bist in keinem Fahrzeug.");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [Command("dvr")]
        public void DeleteVehicleRange(IPlayer player, float range = 25)
        {
            try
            {
                int charId = User.GetPlayerOnline(player);
                if (player.AdminLevel() < 6)
                {
                    HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte.");
                    return;
                }
                int count = 0;
                foreach (IVehicle veh in Alt.GetAllVehicles())
                {
                    if (veh.Position.Distance(player.Position) <= range)
                    {
                        if (veh == null || !veh.Exists || !veh.HasVehicleId())
                            continue;
                        Alt.RemoveVehicle(veh);
                        count++;
                    }
                }
                HUDHandler.SendNotification(player, 2, 2500, $"{count} Fahrzeuge Gelöscht.");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " DVR:" + count + "");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
                HUDHandler.SendNotification(player, 4, 2500, $"ein fehler ist aufgetreten!");
            }
        }
        [Command("fix")]
        public void FixVehicle(IPlayer player)
        {
            try
            {
                if (player == null || !player.Exists || !player.IsInVehicle)
                {
                    HUDHandler.SendNotification(player, 4, 2500, "Du bist in Keinem Auto.");
                    return;
                }
                if (player.AdminLevel() <= 3)
                {
                    HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte.");
                    return;
                }
                int charId = User.GetPlayerOnline(player);
                string charName = Characters.GetCharacterName(charId);
                ServerVehicles.SetVehicleEngineHealthy(player.Vehicle, true);
                Alt.EmitAllClients("Client:Utilities:repairVehicle", player.Vehicle);
                HUDHandler.SendNotification(player, 2, 2500, $"Das Auto wieder repariert.");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
                HUDHandler.SendNotification(player, 4, 2500, $"ein fehler ist aufgetreten!");
            }
        }
        [Command("park")]
        public static void CMD_parkVehicleById(IPlayer player, int vehId)
        {
            try
            {
                if (player == null || !player.Exists || player.AdminLevel() <= 8 || vehId <= 0) return;
                var vehicle = Alt.GetAllVehicles().ToList().FirstOrDefault(x => x != null && x.Exists && x.HasVehicleId() && (int)x.GetVehicleId() == vehId);
                if (vehicle == null) return;
                ServerVehicles.SetVehicleInGarage(vehicle, true, 1);
                HUDHandler.SendNotification(player, 2, 2500, $"Fahrzeug {vehId} in Garage 1(Pillbox) eingeparkt");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " Car:" + vehId + " Gecleart");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [Command("parkall")]
        public static void CMD_ParkALlVehs(IPlayer player)
        {
            try
            {
                if (player == null || !player.Exists || player.AdminLevel() <= 8) return;
                int count = 0;
                foreach (var veh in Alt.Server.GetVehicles().ToList().Where(x => x != null && x.Exists && x.HasVehicleId()))
                {
                    if (veh == null || !veh.Exists || !veh.HasVehicleId()) continue;
                    int currentGarageId = ServerVehicles.GetVehicleGarageId(veh);
                    if (currentGarageId <= 0) continue;
                    ServerVehicles.SetVehicleInGarage(veh, true, currentGarageId);
                    count++;
                }

                HUDHandler.SendNotification(player, 2, 2500, $"{count} Fahrzeuge eingeparkt.");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " Cars:" + count + " Gecleart");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [Command("parkplate", true)]
        public static void CMD_parkVehicle(IPlayer player, string plate)
        {
            try
            {
                if (player == null || !player.Exists || player.AdminLevel() <= 1 || string.IsNullOrWhiteSpace(plate)) return;
                var vehicle = Alt.GetAllVehicles().ToList().FirstOrDefault(x => x != null && x.Exists && x.HasVehicleId() && (int)x.GetVehicleId() > 0 && x.NumberplateText.ToLower() == plate.ToLower());
                if (vehicle == null) return;
                ServerVehicles.SetVehicleInGarage(vehicle, true, 1);
                HUDHandler.SendNotification(player, 2, 2500, $"Fahrzeug mit dem Kennzeichen {plate} in Garage 1 (Pillbox) eingeparkt");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " Car:" + plate + " Gecleart");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [Command("vehpos")]
        public void vehPos(IPlayer player)
        {
            if (player == null || !player.Exists || !player.IsInVehicle) return;
            if (player.AdminLevel() <= 8) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
            player.SendChatMessage($"{player.Vehicle.Position.ToString()}");
            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " CarPos:" + player.Vehicle.Position.ToString() + "");
        }
        [Command("carHash")]
        public static void CMD_GetCarHash(IPlayer player, string car)
        {
            try
            {
                if (player.AdminLevel() <= 9) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
                if (car == null) { HUDHandler.SendNotification(player, 4, 2500, "Fahrzeugname nicht angegeben!"); return; }
                IVehicle testVehicle = Alt.CreateVehicle(car, player.Position, player.Rotation);

                if (testVehicle == null)
                {
                    HUDHandler.SendNotification(player, 4, 2500, $"Falsches Fahrzeug.");
                }
                else
                {
                    testVehicle.Remove();
                    uint hashedCar = Alt.Hash(car);
                    player.SendChatMessage($"Fahrzeug: {car}, Hash: {hashedCar}");
                    DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " Car:" + car + " Hash:" + hashedCar + "");
                }

            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [Command("power")]
        public void PowerCMD(IPlayer player, int engineMultiplier = -1)
        {
            if (player.AdminLevel() <= 9)
            {
                return;
            }

            if (!player.IsInVehicle)
            {
                HUDHandler.SendNotification(player, 4, 2500, "Du bist in keinem Fahrzeug");
                return;
            }

            if (engineMultiplier < 0 || engineMultiplier > 100)
            {
                HUDHandler.SendNotification(player, 4, 2500, "Wert angeben 0-100");
                return;
            }
            player.Vehicle.SetStreamSyncedMetaData("EnginePowerMultiplier", engineMultiplier);
            HUDHandler.SendNotification(player, 1, 2500, $"Power {engineMultiplier}");
            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " CarPower:" + engineMultiplier + "");
        }

        [Command("setveh")]
        public void setVeh(ClassicPlayer player, string name, string plate)
        {
            try
            {

                if (player == null || !player.Exists || player.AdminLevel() < 9) return;

                if (ServerVehicles.ExistServerVehiclePlate(plate)) { HUDHandler.SendNotification(player, 4, 2500, $"Dieses Kennzeichen existiert bereits."); return; }

                long fHash = Alt.Hash(name);
                int charId = User.GetPlayerOnline(player);
                int vehClass = ServerVehicles.VehicleClass(fHash);
                int serialNumber = new Random().Next(1, 10000);
                if (charId == 0 || fHash == 0) return;
                ServerVehicles.CreateVehicle(fHash, charId, 0, 0, false, 0, player.Position, player.Rotation, $"{plate}", 0, 0, 0, vehClass, serialNumber);
                CharactersInventory.AddCharacterItem(charId, $"Fahrzeugschluessel {plate}", 2, "schluessel");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " SetVeh:" + Alt.Hash(name) + "" + plate + "");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        #endregion
        #region AdminStuff
        [Command("setadmin")]
        public static void CMD_Giveadmin(IPlayer player, int accId, int adminLevel)
        {
            try
            {
                if (player == null || !player.Exists) return;
                User.SetPlayerAdminLevel(accId, adminLevel);
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " SetAdmin:" + accId + " AdminLevel:" + adminLevel + "");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [Command("killchar")]
        public void KillCharacter(IPlayer player, int charId)
        {
            if (player.AdminLevel() <= 2) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
            try
            {
                if (player == null || !player.Exists) return;
                var chars = Characters.PlayerCharacters.FirstOrDefault(p => p.charId == charId);

                if (chars != null)
                {
                    chars.death = true;
                    using (gtaContext db = new gtaContext())
                    {
                        var entry = db.AccountsCharacters.Find(chars.charId);
                        if (entry != null)
                        {
                            entry.death = true;
                        }
                        db.SaveChanges();


                    }
                    var targetP = Alt.GetAllPlayers().ToList().FirstOrDefault(x => x != null && x.Exists && User.GetPlayerOnline(x) == charId);
                    if (targetP == null) return;
                    targetP.Kick("");
                    HUDHandler.SendNotification(player, 2, 2500, $"Spieler mit Char-ID {charId} Erfolgreich gekickt.");
                    DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " hat Spieler " + charId + " Gekickt");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [Command("bring", false)]
        public void GetHereCMD(IPlayer player, int targetId)
        {
            if (player.AdminLevel() <= 2) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
            try
            {
                if (player == null || !player.Exists) return;
                if (targetId <= 0 || targetId.ToString().Length <= 0)
                {
                    player.SendChatMessage("Benutzung: /gethere charId");
                    return;
                }
                string targetCharName = Characters.GetCharacterName(targetId);
                if (targetCharName.Length <= 0)
                {
                    HUDHandler.SendNotification(player, 4, 2500, $"Warnung: Die angegebene Character-ID wurde nicht gefunden ({targetId}).");
                    return;
                }
                if (!Characters.ExistCharacterName(targetCharName))
                {
                    HUDHandler.SendNotification(player, 4, 2500, $"Warnung: Der angegebene Charaktername wurde nicht gefunden ({targetCharName} - ID: {targetId}).");
                    return;
                }
                var targetPlayer = Alt.GetAllPlayers().FirstOrDefault(x => x != null && x.Exists && x.GetCharacterMetaId() == (ulong)targetId);
                if (targetPlayer == null || !targetPlayer.Exists) { HUDHandler.SendNotification(player, 4, 2500, "Spieler ist nicht online."); return; }
                HUDHandler.SendNotification(targetPlayer, 1, 2500, $"{Characters.GetCharacterName((int)player.GetCharacterMetaId())} hat dich zu Ihm teleportiert.");
                HUDHandler.SendNotification(player, 1, 2500, $"Du hast den Spieler {Characters.GetCharacterName((int)targetPlayer.GetCharacterMetaId())} zu dir teleportiert.");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " PlayerTP:" + targetPlayer + " >" + player + "");
                targetPlayer.Position = player.Position + new Position(0, 0, 1);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [Command("goto", false)]
        public void GotoCMD(IPlayer player, int targetId)
        {
            if (player.AdminLevel() <= 2) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
            try
            {
                if (player == null || !player.Exists) return;
                if (targetId <= 0 || targetId.ToString().Length <= 0)
                {
                    player.SendChatMessage("Benutzung: /goto charId");
                    HUDHandler.SendNotification(player, 2, 2500, "Benutzung: /goto charId");
                    return;
                }
                string targetCharName = Characters.GetCharacterName(targetId);
                if (targetCharName.Length <= 0)
                {
                    HUDHandler.SendNotification(player, 4, 2500, $"Die angegebene Character-ID wurde nicht gefunden ({targetId}).");
                    return;
                }
                if (!Characters.ExistCharacterName(targetCharName))
                {
                    HUDHandler.SendNotification(player, 4, 2500, $"Der angegebene Charaktername wurde nicht gefunden ({targetCharName} - ID: {targetId}).");
                    return;
                }
                var targetPlayer = Alt.GetAllPlayers().FirstOrDefault(x => x != null && x.Exists && x.GetCharacterMetaId() == (ulong)targetId);
                if (targetPlayer == null || !targetPlayer.Exists) { HUDHandler.SendNotification(player, 3, 2500, "Spieler ist nicht online."); return; }
                HUDHandler.SendNotification(targetPlayer, 1, 2500, $"{Characters.GetCharacterName((int)player.GetCharacterMetaId())} hat sich zu dir teleportiert.");
                HUDHandler.SendNotification(player, 1, 2500, $"Du hast dich zu dem Spieler {Characters.GetCharacterName((int)targetPlayer.GetCharacterMetaId())} teleportiert.");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " PlayerTP:" + player + " >" + targetPlayer + "");
                player.Position = targetPlayer.Position + new Position(0, 0, 1);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [Command("faction")]
        public void FactionCMD(IPlayer player, int charId, int id)
        {
            try
            {
                if (player == null || !player.Exists || player.GetCharacterMetaId() <= 0) return;
                if (player.AdminLevel() <= 8) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
                if (ServerFactions.IsCharacterInAnyFaction(charId))
                {
                    ServerFactions.RemoveServerFactionMember(ServerFactions.GetCharacterFactionId(charId), charId);
                }

                ServerFactions.CreateServerFactionMember(id, charId, ServerFactions.GetFactionMaxRankCount(id), charId);
                HUDHandler.SendNotification(player, 2, 2500, $"{charId} - {id}");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " CharID:" + charId + " Faction:" + id + "");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [Command("gang")]
        public void GangCMD(IPlayer player, int charId, int id)
        {
            try
            {
                if (player == null || !player.Exists || player.GetCharacterMetaId() <= 0) return;
                if (player.AdminLevel() <= 8) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
                if (ServerGangs.IsCharacterInAnyGang(charId))
                {
                    ServerGangs.RemoveServerGangMember(ServerGangs.GetCharacterGangId(charId), charId);
                }

                ServerGangs.CreateServerGangMember(id, charId, ServerGangs.GetGangMaxRankCount(id));
                HUDHandler.SendNotification(player, 2, 2500, $"{charId} - {id}");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " CharID:" + charId + " Gang:" + id + "");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [Command("revive")]
        public void ReviveTargetCMD(IPlayer player, int targetId)
        {
            if (player == null || !player.Exists) return;
            if (player.AdminLevel() <= 2) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
            string charName = Characters.GetCharacterName(targetId);
            if (!Characters.ExistCharacterName(charName)) return;
            var tp = Alt.GetAllPlayers().FirstOrDefault(x => x != null && x.Exists && x.GetCharacterMetaId() == (ulong)targetId);
            if (tp != null)
            {
                tp.Health = 200;
                DeathHandler.revive(tp);
                Alt.Emit("SaltyChat:SetPlayerAlive", tp, true);
                player.SendChatMessage($"Du hast den Spieler {charName} wiederbelebt.");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " PlayerRevive:" + player + " > Player:" + targetId + "");
                return;
            }
            player.SendChatMessage($"Der Spieler {charName} ist nicht online.");
        }
        [Command("support", true)]
        public void supportCMD(IPlayer player, string msg)
        {
            try
            {
                if (player == null || !player.Exists || User.GetPlayerOnline(player) <= 0) return;
                foreach (var admin in Alt.GetAllPlayers().Where(x => x != null && x.Exists && x.AdminLevel() > 0))
                {
                    admin.SendChatMessage($"[SUPPORT] {Characters.GetCharacterName(User.GetPlayerOnline(player))} (ID: {User.GetPlayerOnline(player)}) benötigt Support: {msg}");
                    DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " ID:" + User.GetPlayerOnline(player) + " benötigt Support:" + msg + "");
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [Command("ReloadVeh")]
        public static void VEHS_RELOAD(IPlayer player)
        {
            if (player == null || !player.Exists) return;
            if (player.AdminLevel() < 9) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
            int charId = User.GetPlayerOnline(player);
            if (charId <= 0) return;

            DatabaseHandler.LoadAllServerVehiclesGlobal();
            //Log
            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " ID:" + User.GetPlayerOnline(player) + " VehShops Reload");
            HUDHandler.SendNotification(player, 2, 2500, "Vehicles Reloaded");
        }

        [Command("ReloadVehShops")]
        public static void VEH_RELOAD(IPlayer player)
        {
            if (player == null || !player.Exists) return;
            if (player.AdminLevel() < 9) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
            int charId = User.GetPlayerOnline(player);
            if (charId <= 0) return;
            //SHOPS/Dealer
            DatabaseHandler.LoadAllVehicleShops();
            DatabaseHandler.LoadAllVehicleShopItems();
            //Log
            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " ID:" + User.GetPlayerOnline(player) + " VehShops Reload");
            HUDHandler.SendNotification(player, 2, 2500, "Vehicle Shops Reloaded");
        }
        [Command("ReloadDB")]
        public static void CMD_RELOAD(IPlayer player)
        {
            if (player == null || !player.Exists) return;
            if (player.AdminLevel() < 9) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
            int charId = User.GetPlayerOnline(player);
            if (charId <= 0) return;
            //SHOPS/Dealer
            DatabaseHandler.LoadAllServerShops();
            DatabaseHandler.LoadAllServerShopItems();
            DatabaseHandler.LoadAllClothesShops();
            //Farming
            DatabaseHandler.LoadAllServerFarmingProducers();
            DatabaseHandler.LoadAllServerFarmingSpots();
            DatabaseHandler.LoadAllServerFarmingProducers();
            //Doors
            DatabaseHandler.LoadAllServerDoors();
            //Items
            DatabaseHandler.LoadAllServerItems();
            //Log
            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " ID:" + User.GetPlayerOnline(player) + " DB Reload");
            HUDHandler.SendNotification(player, 2, 2500, "DB Reloaded");
        }
        [Command("ReloadFactions")]
        public static void Factions_RELOAD(IPlayer player)
        {
            if (player == null || !player.Exists) return;
            if (player.AdminLevel() < 9) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
            int charId = User.GetPlayerOnline(player);
            if (charId <= 0) return;
            //Faction
            DatabaseHandler.LoadAllServerFactions();
            DatabaseHandler.LoadAllServerFactionRanks();
            DatabaseHandler.LoadAllServerFactionMembers();
            DatabaseHandler.LoadAllServerFactionStorageItems();
            //Gang
            DatabaseHandler.LoadAllServerGangs();
            DatabaseHandler.LoadAllServerGangRanks();
            DatabaseHandler.LoadAllServerGangMembers();
            //Log
            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " ID:" + User.GetPlayerOnline(player) + " DB Reload");
            HUDHandler.SendNotification(player, 2, 2500, "Factions/Gangs Reloaded");
        }
        [Command("ReloadGarage")]
        public static void Garage_RELOAD(IPlayer player)
        {
            if (player == null || !player.Exists) return;
            if (player.AdminLevel() < 9) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
            int charId = User.GetPlayerOnline(player);
            if (charId <= 0) return;
            //Garage
            DatabaseHandler.LoadAllGarages();
            DatabaseHandler.LoadAllGarageSlots();
            DatabaseHandler.LoadAllServerDoors();
            //Log
            DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " ID:" + User.GetPlayerOnline(player) + " DB Reload");
            HUDHandler.SendNotification(player, 2, 2500, "Garage Reloaded");
        }
        [Command("tppos")]
        public void TpPosCMD(IPlayer player, float X, float Y, float Z)
        {
            try
            {
                if (player == null || !player.Exists) return;
                if (player.AdminLevel() <= 2) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " TP-Pos >" + new Position(X, Y, Z) + "");
                player.Position = new Position(X, Y, Z);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [Command("msg", true)]
        public void announceCMD(IPlayer player, string msg)
        {
            try
            {
                if (player == null || !player.Exists) return;
                if (player.AdminLevel() <= 2) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }

                /*foreach (var client in Alt.GetAllPlayers())
                {
                    if (client == null || !client.Exists) continue;
                    HUDHandler.SendNotification(client, 1, 6500, msg);
                }*/
                foreach (var p in Alt.Server.GetPlayers().Where(x => x != null && x.Exists && x.GetCharacterMetaId() > 0).ToList())
                {
                    HUDHandler.SendNotification(p, 1, 6500, msg);
                }
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " Ankündigung: **" + msg + "**");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [Command("ptwp", false)]
        public void GetHereCMD(IPlayer player, int targetId, int x, int y, int z)
        {
            if (player.AdminLevel() <= 2) { HUDHandler.SendNotification(player, 4, 2500, "Keine Rechte."); return; }
            try
            {
                if (player == null || !player.Exists) return;
                if (targetId <= 0 || targetId.ToString().Length <= 0)
                {
                    player.SendChatMessage("Benutzung: /ptwp charId");
                    HUDHandler.SendNotification(player, 1, 2500, "Benutzung: /ptwp charId");
                    return;
                }
                string targetCharName = Characters.GetCharacterName(targetId);
                if (targetCharName.Length <= 0)
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Die angegebene Character-ID wurde nicht gefunden ({targetId}).");
                    return;
                }
                if (!Characters.ExistCharacterName(targetCharName))
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Der angegebene Charaktername wurde nicht gefunden ({targetCharName} - ID: {targetId}).");
                    return;
                }
                var targetPlayer = Alt.GetAllPlayers().FirstOrDefault(x => x != null && x.Exists && x.GetCharacterMetaId() == (ulong)targetId);
                player.Emit("Client:AdminMenu:GetWaypointInfo");
                if (targetPlayer == null || !targetPlayer.Exists) { HUDHandler.SendNotification(player, 3, 2500, "Spieler ist nicht online."); return; }
                HUDHandler.SendNotification(targetPlayer, 2, 2500, $"{Characters.GetCharacterName((int)player.GetCharacterMetaId())} hat dich teleportiert.");
                HUDHandler.SendNotification(player, 2, 2500, $"Du hast den Spieler {Characters.GetCharacterName((int)targetPlayer.GetCharacterMetaId())} zum Waypoint teleportiert.");
                DiscordLog.SendEmbed("adminmenu", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " PTWP:" + targetPlayer + " Waypoint");
                targetPlayer.Position = new Position(x, y, z + 5);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        #endregion
    }
}
