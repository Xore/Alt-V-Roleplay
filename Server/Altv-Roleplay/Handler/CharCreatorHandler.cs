using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using System;
using System.Globalization;

namespace Altv_Roleplay.Handler
{
    class CharCreatorHandler : IScript
    {
        [AsyncClientEvent("Server:Charcreator:CreateCEF")]
        public void CreateCefBrowser(IPlayer client)
        {
            if (client == null || !client.Exists) return;
            if (((ClassicPlayer)client).accountId <= 0) client.Kick("");
            client.EmitLocked("Client:Charcreator:CreateCEF");
            client.Position = new Position((float)402.778, (float)-996.9758, (float)-98);
            client.Rotation = new Rotation(0, 0, (float)3.1168559);
        }

        [AsyncClientEvent("Server:Charcreator:CreateCharacter")]
        public void CreateCharacter(IPlayer client, string charname, string birthdate, bool gender, string facefeaturesarray, string headblendsdataarray, string headoverlaysarray1, string headoverlaysarray2, string headoverlaysarray3)
        {
            if (client == null || !client.Exists) return;
            if (Characters.ExistCharacterName(charname))
            {
                client.EmitLocked("Client:Charcreator:showError", "Der eingegebene Charaktername ist bereits vergeben.");
                return;
            }
            //ToDo: Abfrage ob Umlaute oder Sonderzeichen im namen sind, falls ja => error

            Characters.CreatePlayerCharacter(client, charname, birthdate, gender, facefeaturesarray, headblendsdataarray, headoverlaysarray1, headoverlaysarray2, headoverlaysarray3);
            client.EmitLocked("Client:Charcreator:DestroyCEF");
            LoginHandler.CreateLoginBrowser(client);
        }

        [AsyncClientEvent("Server:Barber:finishBarber")]
        public void finishBarber(IPlayer player, string headoverlayarray1, string headoverlayarray2, string headoverlayarray3)
        {
            if (player == null || !player.Exists) return;
            int charId = User.GetPlayerOnline(player);
            if (charId == 0 || headoverlayarray1.Length == 0 || headoverlayarray2.Length == 0 || headoverlayarray3.Length == 0) return;
            if (!CharactersInventory.ExistCharacterItem(charId, "Bargeld", "brieftasche") || CharactersInventory.GetCharacterItemAmount(charId, "Bargeld", "brieftasche") < 50) { HUDHandler.SendNotification(player, 3, 5000, $"Du hast nicht genug Bargeld dabei (50$)."); SetCorrectCharacterSkin(player); return; }
            CharactersInventory.RemoveCharacterItemAmount(charId, "Bargeld", 50, "brieftasche");
            HUDHandler.SendNotification(player, 1, 5000, "Nice Cut G!");
            Characters.SetCharacterHeadOverlays(charId, headoverlayarray1, headoverlayarray2, headoverlayarray3);

            float[] headoverlays1 = Array.ConvertAll(headoverlayarray1.Split(';'), param => float.Parse(param, CultureInfo.InvariantCulture));
            float[] headoverlays2 = Array.ConvertAll(headoverlayarray2.Split(';'), param => float.Parse(param, CultureInfo.InvariantCulture));
            float[] headoverlays3 = Array.ConvertAll(headoverlayarray3.Split(';'), param => float.Parse(param, CultureInfo.InvariantCulture));

            player.SetHeadOverlayColor(1, 1, (byte)headoverlays3[1], 1);
            player.SetHeadOverlayColor(2, 1, (byte)headoverlays3[2], 1);
            player.SetHeadOverlayColor(5, 2, (byte)headoverlays3[5], 1);
            player.SetHeadOverlayColor(8, 2, (byte)headoverlays3[8], 1);
            player.SetHeadOverlayColor(10, 1, (byte)headoverlays3[10], 1);
            player.SetEyeColor((ushort)headoverlays1[14]);
            player.SetHeadOverlay(0, (byte)headoverlays1[0], headoverlays2[0]);
            player.SetHeadOverlay(1, (byte)headoverlays1[1], headoverlays2[1]);
            player.SetHeadOverlay(2, (byte)headoverlays1[2], headoverlays2[2]);
            player.SetHeadOverlay(3, (byte)headoverlays1[3], headoverlays2[3]);
            player.SetHeadOverlay(4, (byte)headoverlays1[4], headoverlays2[4]);
            player.SetHeadOverlay(5, (byte)headoverlays1[5], headoverlays2[5]);
            player.SetHeadOverlay(6, (byte)headoverlays1[6], headoverlays2[6]);
            player.SetHeadOverlay(7, (byte)headoverlays1[7], headoverlays2[7]);
            player.SetHeadOverlay(8, (byte)headoverlays1[8], headoverlays2[8]);
            player.SetHeadOverlay(9, (byte)headoverlays1[9], headoverlays2[9]);
            player.SetHeadOverlay(10, (byte)headoverlays1[10], headoverlays2[10]);

            player.HairColor = (byte)headoverlays3[13];
            player.HairHighlightColor = (byte)headoverlays2[13];
            player.SetClothes(2, (ushort)headoverlays1[13], 0, 2);
        }

        [AsyncClientEvent("Server:Barber:RequestCurrentSkin")]
        public void SetCorrectCharacterSkin(IPlayer player)
        {
            if (player == null || !player.Exists) return;
            int charid = User.GetPlayerOnline(player);
            if (charid == 0) return;
            Characters.SetCharacterSkin(player);
        }
    }
}
