using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;
using System;

namespace Altv_Roleplay.Handler
{
    class GangHandler : IScript
    {
        [AsyncClientEvent("Server:GangStorage:StorageItem")]
        public void GangStorageStorageItem(IPlayer player, int gangId, int charId, string itemName, int amount, string fromContainer)
        {
            try
            {
                if (player == null || !player.Exists || gangId <= 0 || charId <= 0 || itemName == "" || itemName == "undefined" || amount <= 0 || fromContainer == "none" || fromContainer == "") return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                if (!ServerGangs.IsCharacterInAnyGang(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Du bist in keiner Fraktion."); return; }
                int cGangId = ServerGangs.GetCharacterGangId(charId);
                if (cGangId != gangId) { return; }
                if (!CharactersInventory.ExistCharacterItem(charId, itemName, fromContainer)) { HUDHandler.SendNotification(player, 3, 2500, "Diesen Gegenstand besitzt du nicht."); return; }
                if (CharactersInventory.GetCharacterItemAmount(charId, itemName, fromContainer) < amount) { HUDHandler.SendNotification(player, 3, 2500, "Du hast nicht genügend Gegenstände davon dabei."); return; }
                if (CharactersInventory.IsItemActive(player, itemName)) { HUDHandler.SendNotification(player, 3, 2500, "Ausgerüstete Gegenstände können nicht umgelagert werden."); return; }
                CharactersInventory.RemoveCharacterItemAmount(charId, itemName, amount, fromContainer);
                ServerGangs.AddServerGangStorageItem(gangId, charId, itemName, amount);
                DiscordLog.SendEmbed("frak", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " Eingelagert: " + itemName + " " + amount + "x | " + gangId);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:GangStorage:TakeItem")]
        public void GangStorageTakeItem(IPlayer player, int gangId, int charId, string itemName, int amount)
        {
            try
            {
                if (player == null || !player.Exists || gangId <= 0 || charId <= 0 || amount <= 0 || itemName == "" || itemName == "undefined") return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                if (!ServerGangs.IsCharacterInAnyGang(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Du bist in keiner Fraktion."); return; }
                int cGangId = ServerGangs.GetCharacterGangId(charId);
                if (cGangId != gangId) return;
                if (!ServerGangs.ExistServerGangStorageItem(gangId, charId, itemName)) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Der Gegenstand existiert im Spind nicht."); return; }
                if (ServerGangs.GetServerGangStorageItemAmount(gangId, charId, itemName) < amount) { HUDHandler.SendNotification(player, 3, 2500, "Fehler: Soviele Gegenstände sind nicht im Spind."); return; }
                float itemWeight = ServerItems.GetItemWeight(itemName) * amount;
                float invWeight = CharactersInventory.GetCharacterItemWeight(charId, "inventory");
                float backpackWeight = CharactersInventory.GetCharacterItemWeight(charId, "backpack");
                float schluesselWeight = CharactersInventory.GetCharacterItemWeight(charId, "schluessel");
                if (invWeight + itemWeight > 5f && backpackWeight + itemWeight > Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId))) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht genug Platz in deinen Taschen."); return; }
                ServerGangs.RemoveServerGangStorageItemAmount(gangId, charId, itemName, amount);
                DiscordLog.SendEmbed("frak", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " Ausgelagert: " + itemName + " " + amount + "x | " + gangId);

                if (itemName.Contains("Bargeld"))
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Du hast ({amount}x) {itemName}aus dem Lager genommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, amount, "brieftasche");
                    return;
                }
                else if (itemName.Contains("Ausweis "))
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Du hast ({amount}x) {itemName}aus dem Lager genommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, amount, "brieftasche");
                    return;
                }
                else if (itemName.Contains("EC-Karte "))
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Du hast ({amount}x) {itemName}aus dem Lager genommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, amount, "brieftasche");
                    return;
                }
                else if (itemName.Contains("Fahrzeugschluessel"))
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Du hast ({amount}x) {itemName}aus dem Lager genommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, amount, "schluessel");
                    return;
                }
                else if (itemName.Contains("Generalschluessel"))
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Du hast ({amount}x) {itemName}aus dem Lager genommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, amount, "schluessel");
                    return;
                }
                else if (invWeight + itemWeight <= 5f)
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Du hast ({amount}x) {itemName}aus dem Lager genommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, amount, "inventory");
                    return;
                }

                if (Characters.GetCharacterBackpack(charId) != -2 && backpackWeight + itemWeight <= Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId)))
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Du hast ({amount}x) {itemName}aus dem Lager genommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, amount, "backpack");
                    return;
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
    }
}
