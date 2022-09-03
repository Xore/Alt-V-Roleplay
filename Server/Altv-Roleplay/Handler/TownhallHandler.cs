using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;
using System;

namespace Altv_Roleplay.Handler
{
    class TownhallHandler : IScript
    {
        [AsyncClientEvent("Server:HUD:sendIdentityCardApplyForm")]
        public void sendIdentityCardApplyForm(IPlayer player, string birthplace)
        {
            if (player == null || !player.Exists) return;
            int charId = User.GetPlayerOnline(player);
            if (charId == 0 || birthplace == "") return;
            Characters.SetCharacterBirthplace(charId, birthplace);
            Characters.setCharacterAccState(charId, 1);
            CharactersInventory.AddCharacterItem(charId, $"Ausweis {Characters.GetCharacterName(charId)}", 1, "brieftasche");
            HUDHandler.SendNotification(player, 2, 2500, "Du hast dir erfolgreich deinen Personalausweis beantragt.");
        }

        internal static void tryCreateIdentityCardApplyForm(IPlayer player)
        {
            if (player == null || !player.Exists) return;
            int charId = User.GetPlayerOnline(player);
            if (charId == 0) return;
            var charname = Characters.GetCharacterName(charId);
            var birthdate = Characters.GetCharacterBirthdate(charId);
            var adress = $"{Characters.GetCharacterStreet(charId)}";
            var curBirthpl = Characters.GetCharacterBirthplace(charId);
            bool gender = Characters.GetCharacterGender(charId);
            player.EmitLocked("Client:HUD:createIdentityCardApplyForm", charname, gender, adress, birthdate, curBirthpl);
        }

        internal static void openHouseSelector(IPlayer player)
        {
            try
            {
                if (player == null || !player.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                if (charId <= 0) return;
                string info = ServerHouses.GetAllCharacterHouses(charId);
                player.EmitLocked("Client:Townhall:openHouseSelector", info);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void openGivePlayerLicenseCEF(IPlayer player)
        {
            try
            {
                if (player == null || !player.Exists) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                var licArray = CharactersLicenses.GetCharacterLicenses(charId);
                player.EmitLocked("Client:GivePlayerLicense:openCEF", charId, licArray);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
    }
}
