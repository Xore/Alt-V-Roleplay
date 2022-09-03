using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Altv_Roleplay.Handler
{
    class LoginHandler : IScript
    {
        [ScriptEvent(ScriptEventType.PlayerConnect)]
        public void OnPlayerConnect_Handler(ClassicPlayer player, string reason)
        {
            if (player == null || !player.Exists) return;
            player.SetSyncedMetaData("PLAYER_SPAWNED", false);
            player.SetSyncedMetaData("ADMINLEVEL", 0);
            player.SetPlayerIsCuffed("handcuffs", false);
            player.SetPlayerIsCuffed("ropecuffs", false);
            setCefStatus(player, false);
            player.SetPlayerCurrentMinijob("None");
            player.SetPlayerCurrentMinijobRouteId(0);
            player.SetPlayerCurrentMinijobStep("None");
            player.SetPlayerCurrentMinijobActionCount(0);
            player.SetPlayerFarmingActionMeta("None");
            User.SetPlayerOnline(player, 0);
            player.EmitLocked("Client:Pedcreator:spawnPed", ServerPeds.GetAllServerPeds());
            CreateLoginBrowser(player);
        }



        [ScriptEvent(ScriptEventType.PlayerDisconnect)]
        public void OnPlayerDisconnected_Handler(ClassicPlayer player, string reason)
        {
            try
            {
                if (player == null) return;
                if (User.GetPlayerOnline(player) != 0) Characters.SetCharacterLastPosition(User.GetPlayerOnline(player), player.Position, player.Dimension);
                User.SetPlayerOnline(player, 0);
                Characters.SetCharacterCurrentFunkFrequence(player.CharacterId, null);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:CEF:setCefStatus")]
        public static void setCefStatus(IPlayer player, bool status)
        {
            if (player == null || !player.Exists) return;
            AltAsync.Do(() => player.SetSyncedMetaData("IsCefOpen", status));
        }

        public static void CreateLoginBrowser(IPlayer client)
        {
            if (client == null || !client.Exists) return;
            client.Model = 0x3D843282;
            client.Dimension = 10000;
            client.Position = new Position(-1398, -555, 16);
            client.EmitLocked("Client:Login:CreateCEF"); //Login triggern
        }

        #region Login
        [AsyncClientEvent("Server:Login:ValidateLoginCredentials")]
        public void ValidateLoginCredentials(ClassicPlayer client, string username, string password)
        {
            if (client == null || !client.Exists) return;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                client.EmitLocked("Client:Login:showError", "Eines der Felder wurde nicht ordnungsgemäß ausgefüllt.");
                return;
            }

            if (!User.ExistPlayerName(username))
            {
                client.EmitLocked("Client:Login:showError", "Der eingegebene Benutzername wurde nicht gefunden.");
                
                DiscordLog.SendEmbed("login", "NightOut-Admin | Log", "\nUsername: " + username + "\nSocialID: " + client.SocialClubId + "\nIP: " + client.Ip + "\nHWID: " + client.HardwareIdHash + "\nDer eingegebene Benutzername wurde nicht gefunden.");
                return;
            }

            if (!BCrypt.Net.BCrypt.Verify(password, User.GetPlayerPassword(username)))
            {
                client.EmitLocked("Client:Login:showError", "Das eingegebene Passwort ist falsch.");
                
                DiscordLog.SendEmbed("login", "NightOut-Admin | Log", "\nUsername: " + username + "\nSocialID: " + client.SocialClubId + "\nIP: " + client.Ip + "\nHWID: " + client.HardwareIdHash + "\nDas eingegebene Passwort ist falsch.");
                return;
            }

            /*if(User.GetPlayerSocialclubId(username) != client.SocialClubId)*/
            if (User.GetPlayerSocialclubId(username) != 0) { if (User.GetPlayerSocialclubId(username) != client.SocialClubId) { client.EmitLocked("Client:Login:showError", "Fehler bei der Anmeldung (Fehlercode 508)."); return; } }
            else { User.SetPlayerSocialID(client); }

            if (!User.IsPlayerWhitelisted(username))
            {
                //client.EmitLocked("Client:Login:showError", "Dieser Benutzeraccount wurde noch nicht im Support aktiviert.");
                var pl = User.GetPlayerAccountIdByUsername(username);
                if (User.GetPlayerWhitelistTime(pl) == 0)
                {
                    client.EmitLocked("Client:Login:showArea", "whitelist");
                }
                else
                {
                    //client.EmitLocked("Client:Login:showArea", "login");
                    client.EmitLocked("Client:Login:showError", "Du hast die Whitelist leider nicht bestanden, nach dem nächsten Server Restart kannst du es nochmal versuchen!");
                }

                DiscordLog.SendEmbed("login", "NightOut-Admin | Log", "\nUsername: " + username + "\nSocialID: " + client.SocialClubId + "\nIP: " + client.Ip + "\nHWID: " + client.HardwareIdHash + "\nDieser Benutzeraccount wurde noch nicht im Support aktiviert.");
                return;
            }

            if (User.GetPlayerHardwareID(client) != 0) { if (User.GetPlayerHardwareID(client) != client.HardwareIdHash) { client.EmitLocked("Client:Login:showError", "Fehler bei der Anmeldung (Fehlercode 187)."); return; } }
            else { User.SetPlayerHardwareID(client); }

            if (User.IsPlayerBanned(client))
            {
                client.EmitLocked("Client:Login:showError", "Dieser Benutzeraccount wurde gebannt, im Support melden.");
                
                DiscordLog.SendEmbed("login", "NightOut-Admin | Log", "\nUsername: " + username + "\nSocialID: " + client.SocialClubId + "\nIP: " + client.Ip + "\nHWID: " + client.HardwareIdHash + "\nDieser Benutzeraccount wurde gebannt, im Support melden.");
                return;
            }

            client.EmitLocked("Client:Login:SaveLoginCredentialsToStorage", username, password);
            User.SetPlayerOnline(client, 0);
            lock (client)
            {
                if (client == null || !client.Exists) return;
                client.accountId = (short)User.GetPlayerAccountId(client);
                client.Dimension = (short)User.GetPlayerAccountId(client);
            }

            SendDataToCharselectorArea(client);
            
            DiscordLog.SendEmbed("login", "NightOut-Admin | Log", "\nUsername: " + username + "\nSocialID: " + client.SocialClubId + "\nIP: " + client.Ip + "\nHWID: " + client.HardwareIdHash + "\nErfolgreich eingeloggt.");
            stopwatch.Stop();
            if (stopwatch.Elapsed.Milliseconds > 30) Alt.Log($"ValidateLoginCredentials benötigte {stopwatch.Elapsed.Milliseconds}ms");
        }
        #endregion

        public static void SendDataToCharselectorArea(IPlayer client)
        {
            if (client == null || !client.Exists) return;
            var charArray = Characters.GetPlayerCharacters(client);
            client.Position = new Position((float)402.778, (float)-996.9758, (float)-98);
            if (client.AdminLevel() > 3)
            {
                client.EmitLocked("Client:Charselector:setMaxChars", 3);
            }

            client.EmitLocked("Client:Charselector:sendCharactersToCEF", charArray);

            client.EmitLocked("Client:Login:showArea", "charselect");
        }

        [AsyncClientEvent("Server:Charselector:spawnChar")]
        public async void CharacterSelectedSpawnPlace(ClassicPlayer client, string spawnstr, string charcid)
        {
            if (client == null || !client.Exists || spawnstr == null || charcid == null || client.accountId <= 0 || User.GetPlayerAccountId(client) <= 0) return;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int charid = Convert.ToInt32(charcid);
            if (charid <= 0) return;
            if (User.GetPlayerAccountId(client) != Characters.GetCharacterAccountId(charid))
            {
                client.Kick("Login Fehler!");
                return;
            }

            string charName = Characters.GetCharacterName(charid);
            User.SetPlayerOnline(client, charid); 
            lock (client)
            {
                if (client == null || !client.Exists) return;
                client.CharacterId = charid;
            }

            if (Characters.GetCharacterFirstJoin(charid) && Characters.GetCharacterFirstSpawnPlace(client, charid) == "unset")
            {
                Characters.SetCharacterFirstSpawnPlace(client, charid, spawnstr);
                CharactersInventory.AddCharacterItem(charid, "Bargeld", 10000, "brieftasche");
                CharactersInventory.AddCharacterItem(charid, "Sandwich", 3, "inventory");
                CharactersInventory.AddCharacterItem(charid, "Smartphone", 1, "inventory");
                CharactersInventory.AddCharacterItem(charid, "Wasser", 3, "inventory");

                if (!Characters.GetCharacterGender((int)client.GetCharacterMetaId()))
                {
                    //Männlich
                    Characters.SwitchCharacterClothes(client, 5048, false);
                    Characters.SwitchCharacterClothes(client, 7761, false);
                    Characters.SwitchCharacterClothes(client, 4860, false);
                    CharactersClothes.CreateCharacterOwnedClothes(client.CharacterId, 5048);
                    CharactersClothes.CreateCharacterOwnedClothes(client.CharacterId, 7761);
                    CharactersClothes.CreateCharacterOwnedClothes(client.CharacterId, 4860);
                }
                else
                {
                    //Weiblich
                    Characters.SwitchCharacterClothes(client, 17018, false);
                    Characters.SwitchCharacterClothes(client, 21383, false);
                    Characters.SwitchCharacterClothes(client, 27501, false);
                    CharactersClothes.CreateCharacterOwnedClothes(client.CharacterId, 17018);
                    CharactersClothes.CreateCharacterOwnedClothes(client.CharacterId, 21383);
                    CharactersClothes.CreateCharacterOwnedClothes(client.CharacterId, 27501);
                }

                switch (spawnstr)
                {
                    case "lsairport":
                        Characters.CreateCharacterLastPos(charid, Constants.Positions.SpawnPos_Airport, 0);
                        break;
                    case null:
                        Characters.CreateCharacterLastPos(charid, Constants.Positions.SpawnPos_Airport, 0);
                        break;
                }
            }

            lock (client)
            {
                if (client == null || !client.Exists) return;
                if (Characters.GetCharacterGender(charid)) client.Model = 0x9C9EFFD8;
                else client.Model = 0x705E61F2;
            }

            client.EmitLocked("Client:ServerBlips:LoadAllBlips", ServerBlips.GetAllServerBlips());
            client.EmitLocked("Client:ServerMarkers:LoadAllMarkers", ServerBlips.GetAllServerMarkers());
            
            Position dbPos = Characters.GetCharacterLastPosition(charid);
            lock (client)
            {
                if (client == null || !client.Exists) return;
                client.Position = dbPos;
                client.Spawn(dbPos, 0);
            }
            if (Characters.GetCharacterPedName(client.CharacterId) != "none" && !String.IsNullOrWhiteSpace(Characters.GetCharacterPedName(client.CharacterId))) client.Model = Alt.Hash(Characters.GetCharacterPedName(client.CharacterId));
            lock (client)
            {
                if (client == null || !client.Exists) return;
                client.Dimension = Characters.GetCharacterLastDimension(charid);
                client.Health = (ushort)(Characters.GetCharacterHealth(charid) + 100);
                client.Armor = (ushort)Characters.GetCharacterArmor(charid);
            }
            HUDHandler.CreateHUDBrowser(client); //HUD erstellen
            
            Characters.SetCharacterCorrectClothes(client);
            Characters.SetCharacterSkin(client);
            Characters.SetCharacterLastLogin(charid, DateTime.Now);
            Characters.SetCharacterCurrentFunkFrequence(charid, null);
            Alt.Emit("SaltyChat:EnablePlayer", client, (int)charid);
            client.EmitLocked("SaltyChat_OnConnected");
            
            if (Characters.IsCharacterUnconscious(charid))
            {
                lock (client)
                {
                    if (client == null || !client.Exists) return;
                    client.Spawn(dbPos, 0);
                    client.EmitAsync("Client:Ragdoll:SetPedToRagdoll", true, 0);
                    client.EmitAsync("Client:Deathscreen:openCEF");
                    client.SetPlayerIsUnconscious(true);
                }

            }
            if (Characters.IsCharacterFastFarm(charid))
            {
                var fastFarmTime = Characters.GetCharacterFastFarmTime(charid) * 60000;
                client.EmitLocked("Client:Inventory:PlayEffect", "DrugsMichaelAliensFight", fastFarmTime);
                HUDHandler.SendNotification(client, 1, 2500, $"Du bist noch ziemlich drauf..");
            }
            ServerAnimations.RequestAnimationMenuContent(client);
            if (Characters.IsCharacterPhoneEquipped(charid) && CharactersInventory.ExistCharacterItem(charid, "Smartphone", "inventory") && CharactersInventory.GetCharacterItemAmount(charid, "Smartphone", "inventory") > 0)
            {
                client.EmitLocked("Client:Smartphone:equipPhone", true, Characters.GetCharacterPhonenumber(charid), Characters.IsCharacterPhoneFlyModeEnabled(charid), Characters.GetCharacterPhoneWallpaper(charid));
                Characters.SetCharacterPhoneEquipped(charid, true);
            }
            else if (!Characters.IsCharacterPhoneEquipped(charid) || !CharactersInventory.ExistCharacterItem(charid, "Smartphone", "inventory") || CharactersInventory.GetCharacterItemAmount(charid, "Smartphone", "inventory") <= 0)
            {
                client.EmitLocked("Client:Smartphone:equipPhone", false, Characters.GetCharacterPhonenumber(charid), Characters.IsCharacterPhoneFlyModeEnabled(charid), Characters.GetCharacterPhoneWallpaper(charid));
                Characters.SetCharacterPhoneEquipped(charid, false);
            }
            SmartphoneHandler.RequestLSPDIntranet(client);
            setCefStatus(client, false);
            client.SetStreamSyncedMetaData("sharedUsername", $"{charName} ({Characters.GetCharacterAccountId(charid)})");
            client.SetSyncedMetaData("ADMINLEVEL", client.AdminLevel());
            client.SetSyncedMetaData("PLAYER_SPAWNED", true);

            if (Characters.IsCharacterInJail(charid))

            {
                int jailTimes = Characters.GetCharacterJailTime(charid);
                HUDHandler.SendNotification(client, 3, 2500, $"Du befindest dich noch {Characters.GetCharacterJailTime(charid)} Minuten im Gefängnis.");
                HUDHandler.SendNotification(client, 3, jailTimes, $"Du bist noch in Haft!");
                lock (client)
                {
                    if (client == null || !client.Exists) return;
                    client.Position = new Position(1691.4594f, 2565.7056f, 45.556763f);
                }
                if (Characters.GetCharacterGender(charid) == false)
                {
                    client.EmitLocked("Client:SpawnArea:setCharClothes", 3, 0, 0);//Torso
                    client.EmitLocked("Client:SpawnArea:setCharClothes", 4, 3, 7);//Legs
                    client.EmitLocked("Client:SpawnArea:setCharClothes", 6, 1, 1);//Shoes
                    client.EmitLocked("Client:SpawnArea:setCharClothes", 8, 15, 0);//Undershirt
                    client.EmitLocked("Client:SpawnArea:setCharClothes", 11, 22, 0);//Tops
                }
                else
                {
                    client.EmitLocked("Client:SpawnArea:setCharClothes", 3, 4, 0);//Torso
                    client.EmitLocked("Client:SpawnArea:setCharClothes", 4, 3, 15);//Legs
                    client.EmitLocked("Client:SpawnArea:setCharClothes", 6, 1, 6);//Shoes
                    client.EmitLocked("Client:SpawnArea:setCharClothes", 8, 15, 0);//Undershirt
                    client.EmitLocked("Client:SpawnArea:setCharClothes", 11, 118, 0);//Tops
                }
            }
            client.updateTattoos();
            stopwatch.Stop();
            if (stopwatch.Elapsed.Milliseconds > 30) 
                await Task.Delay(5000);
            Model.ServerTattoos.GetAllTattoos(client);
        }
    }
}
