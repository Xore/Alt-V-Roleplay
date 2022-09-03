using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;
using Newtonsoft.Json;

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

        #region Login (WBB)
        private const string Key = "J@McQfTjWnZr4u7x"; //128-Character Key
        private const string RequestUrl = "https://forum.nightout-gaming.de/Wbb-Verify.php";  //URL to PHP-File

        private static readonly HttpClient Client = new HttpClient();

        public enum LoginStatusCode
        {
            Error = 0,
            KeyWrong = 1,
            DataMissing = 2,
            Success = 10,
            WrongPasswordUsername = 11
        }

        public class LoginUserData
        {
            public int UserId { get; set; }
            public string Username { get; set; }
            public bool Banned { get; set; }
            public string BanReason { get; set; }
            public bool Whitelisted { get; set; }
        }

        public class LoginResponse
        {
            public LoginStatusCode StatusCode { get; set; }
            public LoginUserData UserData { get; set; }

            public LoginResponse(LoginStatusCode statusCode, LoginUserData userData)
            {
                StatusCode = statusCode;
                UserData = userData;
            }
        }

        private static async Task<LoginResponse> MakePostRequest(string requestUrl, string username, string password, string key)
        {

            var values = new Dictionary<string, string>
            {
                { "Username", username.ToLower() },
                { "Password", password },
                { "Key", key }
            };

            var content = new FormUrlEncodedContent(values);
            var response = await Client.PostAsync(requestUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<LoginResponse>(responseString);
        }


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

            LoginResponse loginInfo;
            Console.WriteLine($"ValidateLoginCredentials - Thread = {Thread.CurrentThread.ManagedThreadId}");

            loginInfo = MakePostRequest(RequestUrl, username, password, Key).Result;

            switch (loginInfo.StatusCode)
            {
                case LoginStatusCode.Success:
                    ((ClassicPlayer)client).accountId = loginInfo.UserData.UserId;

                    if (User.ExistPlayerName(username.ToLower()))
                    {
                        // nothing
                        User.SetPlayerIP(client, username.ToLower());
                        User.SetPlayerHardwareID(client, username.ToLower());
                    }
                    else
                    {
                        User.CreatePlayerAccount(client, username.ToLower());
                        User.SetPlayerHardwareID(client, username.ToLower());
                    }


                    if (loginInfo.UserData.Banned)
                    {
                        if (User.IsPlayerBanned(client))
                        {
                            client.EmitLocked("Client:Login:showError", "Du wurdest gebannt. Grund: {0}", loginInfo.UserData.BanReason);
                        }
                        else
                        {
                            User.SetPlayerBanned(client, true, loginInfo.UserData.BanReason);
                            client.EmitLocked("Client:Login:showError", "Du wurdest gebannt. Grund: {0}", loginInfo.UserData.BanReason);
                        }

                    }
                    else
                    {
                        if (User.IsPlayerBanned(client))
                        {
                            User.SetPlayerBanned(client, false, "");
                        }
                    }

                    if (!User.ExistPlayerName(username.ToLower()))
                    {
                        client.EmitLocked("Client:Login:showError", "Dieser Account wurde nicht gefunden. Erstelle dir einen Account bei uns im Forum.");
                    }

                    if (loginInfo.UserData.Whitelisted)
                    {
                        //User.CreatePlayerAccount(client, username);
                        client.Dimension = (short)User.GetPlayerAccountId(client);
                        client.EmitLocked("Client:Login:SaveLoginCredentialsToStorage", username, password);
                        User.SetPlayerOnline(client, 1);
                        SendDataToCharselectorArea(client);
                        stopwatch.Stop();
                        if (stopwatch.Elapsed.Milliseconds > 30) //Alt.Log($"ValidateLoginCredentials benötigte {stopwatch.Elapsed.Milliseconds}ms");
                            return;
                    }
                    else
                    {
                        client.EmitLocked("Client:Login:showError", "Du musst dich erst Whitelisten lassen!");
                    }
                    break;

                case LoginStatusCode.WrongPasswordUsername:
                    client.EmitLocked("Client:Login:showError", "Der Benutzername oder das Passwort stimmen nicht überein.");
                    break;

                case LoginStatusCode.DataMissing:
                    client.EmitLocked("Client:Login:showError", "Trage deinen Benutzernamen oder Passwort ein.");
                    new LoginResponse(LoginStatusCode.DataMissing, null);
                    break;

                case LoginStatusCode.KeyWrong:
                    client.EmitLocked("Client:Login:showError", "Der Login Service ist nicht erreichbar.");
                    new LoginResponse(LoginStatusCode.KeyWrong, null);
                    break;

                default:
                    new LoginResponse(LoginStatusCode.Error, null);
                    break;
            }
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
            User.SetPlayerOnline(client, charid); //Online Feld = CharakterID
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
            client.EmitLocked("Client:SpawnArea:setCharSkin", Characters.GetCharacterSkin("facefeatures", charid), Characters.GetCharacterSkin("headblendsdata", charid), Characters.GetCharacterSkin("headoverlays", charid));
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
            //WeatherHandler.SetRealTime(client); //Echtzeit setzen
            Characters.SetCharacterCorrectClothes(client);
            Characters.SetCharacterLastLogin(charid, DateTime.Now);
            Characters.SetCharacterCurrentFunkFrequence(charid, null);
            Alt.Emit("SaltyChat:EnablePlayer", client, (int)charid);
            client.EmitLocked("SaltyChat_OnConnected");
            //client.SetSyncedMetaData("NAME", User.GetPlayerUsername(((ClassicPlayer)client).accountId) + " | " + Characters.GetCharacterName((int)client.GetCharacterMetaId()));
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
            if (stopwatch.Elapsed.Milliseconds > 30) //Alt.Log($"{charid} - CharacterSelectedSpawnPlace benötigte {stopwatch.Elapsed.Milliseconds}ms");
                await Task.Delay(5000);
            Model.ServerTattoos.GetAllTattoos(client);
        }
    }
}
