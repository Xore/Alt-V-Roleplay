using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Handler;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;
using System;
using System.Globalization;

namespace Altv_Roleplay.Factions.LSFS
{
    class Functions : IScript
    {
        [AsyncClientEvent("Server:GivePlayerLicense:GiveLicense")]
        public void GiveLicense(IPlayer player, int targetCharId, string licShort)
        {
            try
            {
                if (player == null || !player.Exists || targetCharId <= 0 || licShort == "") return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das gefesselt machen?"); return; }
                if (!CharactersLicenses.ExistServerLicense(licShort)) { HUDHandler.SendNotification(player, 3, 2500, "Ein unerwarteter Fehler ist aufgetreten."); return; }
                if (CharactersLicenses.HasCharacterLicense(targetCharId, licShort)) { HUDHandler.SendNotification(player, 3, 2500, "Der Spieler hat diese Lizenz bereits."); return; }
                if (!CharactersBank.HasCharacterBankMainKonto(targetCharId)) { HUDHandler.SendNotification(player, 3, 2500, "Der Spieler besitzt kein Hauptkonto."); return; }
                int accNumber = CharactersBank.GetCharacterBankMainKonto(charId);
                int licPrice = CharactersLicenses.GetLicensePrice(licShort);
                if (CharactersBank.GetBankAccountLockStatus(accNumber)) { HUDHandler.SendNotification(player, 3, 2500, "Das Hauptkonto des Spielers ist gesperrt."); return; }
                CharactersBank.SetBankAccountMoney(accNumber, CharactersBank.GetBankAccountMoney(accNumber) - licPrice);
                ServerBankPapers.CreateNewBankPaper(accNumber, DateTime.Now.ToString("d", CultureInfo.CreateSpecificCulture("de-DE")), DateTime.Now.ToString("t", CultureInfo.CreateSpecificCulture("de-DE")), "Ausgehende Überweisung", "Fahrschule", $"Lizenzkauf: {CharactersLicenses.GetFullLicenseName(licShort)}", $"-{licPrice}$", "Bankeinzug");
                CharactersLicenses.SetCharacterLicense(targetCharId, licShort, true);
                ServerFactions.SetFactionBankMoney(5, ServerFactions.GetFactionBankMoney(5) + licPrice);
                HUDHandler.SendNotification(player, 1, 2500, $"Ihnen wurde die Lizenz '{CharactersLicenses.GetFullLicenseName(licShort)}' für eine Gebühr i.H.v. {licPrice}$ ausgestellt, diese wurde von Ihrem Hauptkonto abgebucht.");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
    }
}
