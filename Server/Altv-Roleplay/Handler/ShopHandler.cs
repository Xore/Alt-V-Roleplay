using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.models;
using Altv_Roleplay.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Altv_Roleplay.Handler
{
    class ShopHandler : IScript
    {
        #region Open Shop
        internal static void openShop(IPlayer player, Server_Shops shop)
        {
            try
            {
                if (player == null || !player.Exists) return;
                var allowed = true;
                int charId = User.GetPlayerOnline(player);

                if (charId <= 0) return;

                if (shop.faction > 0 && shop.faction != 0)
                {
                    if (!ServerFactions.IsCharacterInAnyFaction(charId))
                    {
                        HUDHandler.SendNotification(player, 3, 2500, "Kein Zugriff [1]");
                        allowed = false; return;
                    }
                    if (ServerFactions.GetCharacterFactionId(charId) != shop.faction)
                    {
                        HUDHandler.SendNotification(player, 3, 2500, $"Kein Zugriff [{shop.faction} - {ServerFactions.GetCharacterFactionId(charId)}]");
                        allowed = false; return;
                    }
                }

                if (shop.closed == 1)
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Sorry wir haben Geschlossen!");
                    return;
                }

                if (shop.stateClosed == 1)
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Sorry unser Shop wurde Zwangs Geschlossen!");
                    return;
                }

                if (shop.neededLicense != "None" && !CharactersLicenses.HasCharacterLicense(charId, shop.neededLicense))
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Du hast hier keinen Zugriff drauf.");
                    allowed = false;
                    return;
                }

                if (shop.isOnlySelling == false && allowed)
                {
                    string items = ServerShops.GetShopItems(shop.id);
                    player.EmitAsync("Client:Shop:shopBuyCEFCreateCEF", shop.id, items, shop.isOnlySelling);
                    return;
                }

                if (shop.isOnlySelling == true && allowed)
                {
                    string items = ServerShops.GetShopSellItems(charId, shop.id);
                    player.EmitAsync("Client:Shop:shopSellCEFCreateCEF", shop.id, items, shop.isOnlySelling);
                    return;
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        #endregion

        #region Buy Item
        [AsyncClientEvent("Server:Shop:buyItem")]
        public async Task ClientEvent_buyItem(ClassicPlayer player, int shopId, string itemName, int itemAmount)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0 || string.IsNullOrWhiteSpace(itemName) || shopId <= 0 || itemAmount <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                int charId = User.GetPlayerOnline(player);
                if (charId == 0) return;

                int shopType = ServerShops.GetShopType(shopId);
                int itemPrice = ServerShops.GetItemPrice(shopId, itemName) * itemAmount;
                //float itemWeight = ServerItems.GetItemWeight(ServerItems.GetNormalItemName(itemName)) * itemAmount;
                float itemWeight = ServerItems.GetItemWeight(itemName) * itemAmount;
                float invWeight = CharactersInventory.GetCharacterItemWeight(charId, "inventory");
                float backpackWeight = CharactersInventory.GetCharacterItemWeight(charId, "backpack");


                if (ServerShops.GetShopItemAmount(shopId, itemName) < itemAmount) { HUDHandler.SendNotification(player, 3, 2500, $"Soviele Gegenstände hat der Shop nicht auf Lager."); return; }
                if (invWeight + itemWeight > 5f && backpackWeight + itemWeight > Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId))) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht genug Platz in deinen Taschen."); return; }

                if (invWeight + itemWeight <= 5f)
                {
                    if (!CharactersInventory.ExistCharacterItem(charId, "Bargeld", "brieftasche") || CharactersInventory.GetCharacterItemAmount(charId, "Bargeld", "brieftasche") < itemPrice)
                    {
                        HUDHandler.SendNotification(player, 3, 2500, "Du hast nicht genügend Geld dabei.");
                        return;
                    }

                    if (itemName.Contains("Generalschluessel"))
                    {
                        HUDHandler.SendNotification(player, 2, 2500, $"Du hast {itemAmount}x {itemName} für {itemPrice} gekauft.");
                        CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "schluessel");
                        return;
                    }
                    if (itemName.Contains("Fahrzeugschluessel"))
                    {
                        HUDHandler.SendNotification(player, 2, 2500, $"Du hast {itemAmount}x {itemName} für {itemPrice} gekauft.");
                        CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "schluessel");
                        return;
                    }
                    if (itemName.Contains("Handschellenschluessel"))
                    {
                        HUDHandler.SendNotification(player, 2, 2500, $"Du hast {itemAmount}x {itemName} für {itemPrice} gekauft.");
                        CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "schluessel");
                        return;
                    }

                    if (shopType == 1)
                    {
                        CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "inventory");
                        CharactersInventory.RemoveCharacterItemAmount(charId, "Bargeld", itemPrice, "brieftasche");
                        HUDHandler.SendNotification(player, 2, 2500, $"Du hast {itemAmount}x {itemName} für {itemPrice}$ gekauft.");
                    }
                    if (shopType == 0)
                    {
                        CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "inventory");
                        CharactersInventory.RemoveCharacterItemAmount(charId, "Bargeld", itemPrice, "brieftasche");
                        ServerShops.RemoveShopItemAmount(shopId, itemName, itemAmount);
                        HUDHandler.SendNotification(player, 2, 2500, $"Du hast {itemAmount}x {itemName} für {itemPrice}$ gekauft.");
                    }
                    ServerShops.SetShopBankMoney(shopId, ServerShops.GetShopBankMoney(shopId) + itemPrice);
                    return;
                }

                if (Characters.GetCharacterBackpack(charId) != -2 && backpackWeight + itemWeight <= Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId)))
                {
                    if (!CharactersInventory.ExistCharacterItem(charId, "Bargeld", "brieftasche") || CharactersInventory.GetCharacterItemAmount(charId, "Bargeld", "brieftasche") < itemPrice)
                    {
                        HUDHandler.SendNotification(player, 3, 2500, "Du hast nicht genügend Geld dabei.");
                        return;
                    }

                    if (shopType == 1)
                    {
                        CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "backpack");
                        CharactersInventory.RemoveCharacterItemAmount(charId, "Bargeld", itemPrice, "brieftasche");
                        HUDHandler.SendNotification(player, 2, 2500, $"Du hast {itemAmount}x {itemName} für {itemPrice}$ gekauft.");
                    }
                    if (shopType == 0)
                    {
                        CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "backpack");
                        CharactersInventory.RemoveCharacterItemAmount(charId, "Bargeld", itemPrice, "brieftasche");
                        ServerShops.RemoveShopItemAmount(shopId, itemName, itemAmount);
                        HUDHandler.SendNotification(player, 2, 2500, $"Du hast {itemAmount}x {itemName} für {itemPrice}$ gekauft.");
                    }
                    ServerShops.SetShopBankMoney(shopId, ServerShops.GetShopBankMoney(shopId) + itemPrice);
                    return;
                }

                //ServerShops.RemoveShopItemAmount(shopId, itemName, itemAmount);
                //Alt.Log($"{(shopId, itemName, itemAmount)}");
                //ServerShops.SetShopBankMoney(shopId, ServerShops.GetShopBankMoney(shopId) + itemPrice);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
        #endregion

        #region Sell Item
        [AsyncClientEvent("Server:Shop:sellItem")]
        public async Task ClientEvent_sellItem(ClassicPlayer player, int shopId, string itemName, int itemAmount)
        {
            try
            {
                if (player == null || !player.Exists || player.accountId <= 0 || string.IsNullOrWhiteSpace(itemName) || shopId <= 0 || itemAmount <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                int charId = User.GetPlayerOnline(player);
                if (charId == 0) return;

                if (ServerShops.GetShopNeededLicense(shopId) != "None" && !Characters.HasCharacterPermission(charId, ServerShops.GetShopNeededLicense(shopId))) { HUDHandler.SendNotification(player, 3, 2500, "Du hast hier keinen Zugriff drauf."); return; }
                if (!CharactersInventory.ExistCharacterItem(charId, itemName, "inventory") && !CharactersInventory.ExistCharacterItem(charId, itemName, "backpack")) { HUDHandler.SendNotification(player, 3, 2500, "Diesen Gegenstand besitzt du nicht."); return; }
                int itemSellPrice = ServerShops.GetItemPrice(shopId, itemName); //Verkaufpreis pro Item
                int invItemAmount = CharactersInventory.GetCharacterItemAmount(charId, itemName, "inventory"); //Anzahl an Items im Inventar
                int backpackItemAmount = CharactersInventory.GetCharacterItemAmount(charId, itemName, "backpack"); //Anzahl an Items im Rucksack
                int schluesselItemAmount = CharactersInventory.GetCharacterItemAmount(charId, itemName, "schluessel"); //Anzahl an Items im Rucksack
                if (invItemAmount + backpackItemAmount < itemAmount) { HUDHandler.SendNotification(player, 3, 2500, "Soviele Gegenstände hast du nicht zum Verkauf dabei."); return; }

                var removeFromInventory = Math.Min(itemAmount, invItemAmount);
                if (removeFromInventory > 0)
                {
                    CharactersInventory.RemoveCharacterItemAmount(charId, itemName, removeFromInventory, "inventory");
                }

                var itemsLeft = itemAmount - removeFromInventory;
                if (itemsLeft > 0)
                {
                    CharactersInventory.RemoveCharacterItemAmount(charId, itemName, itemsLeft, "backpack");
                }

                HUDHandler.SendNotification(player, 2, 2500, $"Du hast {itemAmount}x {itemName} für {itemSellPrice * itemAmount}$ verkauft.");

                CharactersInventory.AddCharacterItem(charId, "Bargeld", itemAmount * itemSellPrice, "brieftasche");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
        #endregion

        #region Shop Manager
        internal static async void openShopManager(IPlayer player, int shopId)
        {
            try
            {
                if (player == null || !player.Exists || shopId <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;

                int shopClosed = ServerShops.GetShopStateClosed(shopId);
                if (shopClosed == 1)
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Dein Shop wurde Zwangs Geschlossen! Wende dich an die Justiz");
                    return;
                }

                await HelperEvents.ClientEvent_setCefStatus(player, true);
                string shopItems = ServerShops.GetShopItems(shopId);
                string inventoryItems = CharactersInventory.CharacterInventoryItems(charId);
                int shopCash = ServerShops.GetShopBankMoney(shopId);
                Global.mGlobal.VirtualAPI.TriggerClientEventSafe(player, "Client:Shop:openShopManager", shopId, inventoryItems, shopItems, shopCash);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [AsyncClientEvent("Server:Shop:setItemPrice")]
        public async Task ClientEvent_setItemPrice(IPlayer player, int shopId, string itemName, int itemPrice)
        {
            try
            {
                int charId = User.GetPlayerOnline(player);
                if (player == null || !player.Exists || charId <= 0 || shopId <= 0 || string.IsNullOrWhiteSpace(itemName) || itemPrice <= 0 || ServerShops.GetShopOwner(shopId) != charId || !ServerShops.ExistShopItem(shopId, itemName)) return;
                ServerShops.SetShopItemPrice(shopId, itemName, itemPrice);
                HUDHandler.SendNotification(player, 2, 2500, $"Du hast den Gegenstand {itemName} auf den Preis {itemPrice}$ gestellt.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [AsyncClientEvent("Server:Shop:depositShopItem")]
        public async Task ClientEvent_depositShopItem(IPlayer player, int shopId, string itemName, int itemAmount)
        {
            try
            {
                int charId = User.GetPlayerOnline(player);
                if (player == null || !player.Exists || charId <= 0 || shopId <= 0 || string.IsNullOrWhiteSpace(itemName) || itemAmount <= 0 || ServerShops.GetShopOwner(shopId) != charId || !CharactersInventory.ExistCharacterItem(charId, itemName, "inventory") || CharactersInventory.GetCharacterItemAmount(charId, itemName, "inventory") < itemAmount) return;

                if (itemName.Contains("Bargeld"))
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Das kannst du nicht zum Verkauf anbieten!");
                    return;
                }
                else if (itemName.Contains("Ausweis "))
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Das kannst du nicht zum Verkauf anbieten!");
                    return;
                }
                else if (itemName.Contains("EC-Karte "))
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Das kannst du nicht zum Verkauf anbieten!");
                    return;
                }
                else if (itemName.Contains("Fahrzeugschluessel"))
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Das kannst du nicht zum Verkauf anbieten!");
                    return;
                }
                else if (itemName.Contains("Generalschluessel"))
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Das kannst du nicht zum Verkauf anbieten!");
                    return;
                }

                CharactersInventory.RemoveCharacterItemAmount(charId, itemName, itemAmount, "inventory");
                ServerShops.AddShopItem(shopId, itemName, itemAmount);
                HUDHandler.SendNotification(player, 2, 2500, $"Du hast {itemAmount}x {itemName} zum Verkauf angeboten.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [AsyncClientEvent("Server:Shop:takeShopItem")]
        public async Task ClientEvent_takeShopItem(IPlayer player, int shopId, string itemName, int itemAmount)
        {
            try
            {
                int charId = User.GetPlayerOnline(player);
                if (player == null || !player.Exists || charId <= 0 || shopId <= 0 || string.IsNullOrWhiteSpace(itemName) || itemAmount <= 0 || ServerShops.GetShopOwner(shopId) != charId || !ServerShops.ExistShopItem(shopId, itemName) || ServerShops.GetShopItemAmount(shopId, itemName) < itemAmount) return;

                float itemWeight = ServerItems.GetItemWeight(itemName) * itemAmount;
                float invWeight = CharactersInventory.GetCharacterItemWeight(charId, "inventory");
                float backpackWeight = CharactersInventory.GetCharacterItemWeight(charId, "backpack");
                if (invWeight + itemWeight > 5f && backpackWeight + itemWeight > Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId))) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht genug Platz in deinen Taschen."); return; }
                ServerShops.RemoveShopItemAmount(shopId, itemName, itemAmount);
                if (itemName.Contains("Bargeld"))
                {
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast {itemAmount}x {itemName} aus dem Shop-Lager genommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "brieftasche");
                    return;
                }
                else if (itemName.Contains("Ausweis "))
                {
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast {itemAmount}x {itemName} aus dem Shop-Lager genommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "brieftasche");
                    return;
                }
                else if (itemName.Contains("EC-Karte "))
                {
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast {itemAmount}x {itemName} aus dem Shop-Lager genommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "brieftasche");
                    return;
                }
                else if (itemName.Contains("Fahrzeugschluessel"))
                {
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast {itemAmount}x {itemName} aus dem Shop-Lager genommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "schluessel");
                    return;
                }
                else if (itemName.Contains("Generalschluessel"))
                {
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast {itemAmount}x {itemName} aus dem Shop-Lager genommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "schluessel");
                    return;
                }
                else if (invWeight + itemWeight <= 5f)
                {
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast {itemAmount}x {itemName} aus dem Shop-Lager genommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "inventory");
                    return;
                }

                if (Characters.GetCharacterBackpack(charId) != -2 && backpackWeight + itemWeight <= Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId)))
                {
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast {itemAmount}x {itemName} aus dem Shop-Lager genommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "backpack");
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [AsyncClientEvent("Server:Shop:PayOut")]
        public void PayOutShop(IPlayer player, int shopId)
        {
            try
            {
                if (player == null || !player.Exists || shopId <= 0) return;
                int charid = User.GetPlayerOnline(player);
                if (charid == 0) return;
                var shopMoney = ServerShops.GetShopBankMoney(shopId);

                if (shopMoney <= 0) { HUDHandler.SendNotification(player, 3, 2500, "Soviel Geld ist nicht in der Kasse."); return; }

                ServerShops.SetShopBankMoney(shopId, shopMoney - shopMoney);
                CharactersInventory.AddCharacterItem(charid, "Bargeld", shopMoney, "brieftasche");
                HUDHandler.SendNotification(player, 2, 2500, $"Du hast erfolgreich {shopMoney}$ vom Konto abgebucht.");
                return;

            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Shop:CloseShop1")] //To-Do
        public void CloseShop1(IPlayer player, int shopId)
        {
            try
            {
                if (player == null || !player.Exists || shopId <= 0) return;
                int charid = User.GetPlayerOnline(player);
                if (charid == 0) return;

                ServerShops.SetShopClosed(shopId);

                HUDHandler.SendNotification(player, 2, 2500, $"Shop wurde Geschlossen.");
                return;

            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Shop:OpenShop1")] //To-Do
        public void OpenShop1(IPlayer player, int shopId)
        {
            try
            {
                if (player == null || !player.Exists || shopId <= 0) return;
                int charid = User.GetPlayerOnline(player);
                if (charid == 0) return;

                ServerShops.SetShopOpen(shopId);

                HUDHandler.SendNotification(player, 2, 2500, $"Shop wurde Geöffnet.");
                return;

            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        #endregion

        #region RobShop
        [AsyncClientEvent("Server:Shop:robShop")]
        public async Task robShop(ClassicPlayer player, int shopId)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0 || shopId <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }

                if (!player.Position.IsInRange(ServerShops.GetShopPosition(shopId), 5f)) { HUDHandler.SendNotification(player, 3, 2500, "Du bist zu weit entfernt."); return; }
                if (player.isRobbingAShop)
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Du raubst bereits einen Shop aus.");
                    return;
                }

                if (ServerShops.IsShopRobbedNow(shopId))
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Dieser Shop wird bereits ausgeraubt.");
                    return;
                }

                if (ServerFactions.GetFactionDutyMemberCount(2) + ServerFactions.GetFactionDutyMemberCount(12) < 2)
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Das Sicherheits System wurde erst Erneuert, Verschwinde!");
                    return;
                }

                if (ServerFactions.IsCharacterInAnyFaction(player.CharacterId) && ServerFactions.IsCharacterInFactionDuty(player.CharacterId) && ServerFactions.GetCharacterFactionId(player.CharacterId) == 2)
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Das kannst du nicht tun!");
                    return;
                }
                if (ServerFactions.IsCharacterInAnyFaction(player.CharacterId) && ServerFactions.IsCharacterInFactionDuty(player.CharacterId) && ServerFactions.GetCharacterFactionId(player.CharacterId) == 1)
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Das kannst du nicht tun!");
                    return;
                }
                if (ServerFactions.IsCharacterInAnyFaction(player.CharacterId) && ServerFactions.IsCharacterInFactionDuty(player.CharacterId) && ServerFactions.GetCharacterFactionId(player.CharacterId) == 3)
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Das kannst du nicht tun!");
                    return;
                }
                if (ServerFactions.IsCharacterInAnyFaction(player.CharacterId) && ServerFactions.IsCharacterInFactionDuty(player.CharacterId) && ServerFactions.GetCharacterFactionId(player.CharacterId) == 12)
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Das kannst du nicht tun!");
                    return;
                }
                /*if () ToDo : Check if Player is Armed
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Pfff.. vor dir habe ich keine Angst, Verschwinde!");
                    return;
                }*/

                ServerFactions.AddNewFactionDispatchNoName("Stiller Alarm", 2, "Ein aktiver Shopraub wurde gemeldet.", player.Position);
                ServerFactions.AddNewFactionDispatchNoName("Stiller Alarm", 12, "Ein aktiver Shopraub wurde gemeldet.", player.Position);

                foreach (var p in Alt.Server.GetPlayers().Where(x => x != null && x.Exists && x.GetCharacterMetaId() > 0).ToList())
                {
                    if (!ServerFactions.IsCharacterInAnyFaction((int)p.GetCharacterMetaId()) || !ServerFactions.IsCharacterInFactionDuty((int)p.GetCharacterMetaId()) || ServerFactions.GetCharacterFactionId((int)p.GetCharacterMetaId()) != 2 && ServerFactions.GetCharacterFactionId((int)p.GetCharacterMetaId()) != 12) continue;
                    HUDHandler.SendNotification(p, 3, 2500, "Ein stiller Alarm wurde ausgelöst.");
                }

                ServerShops.SetShopRobbedNow(shopId, true);
                player.isRobbingAShop = true;

                HUDHandler.SendProgress(player, "Du Raubst den Laden aus!", "alert", 300000);
                await Task.Delay(300000);//5 Minuten
                ServerShops.SetShopRobbedNow(shopId, false);
                if (player == null || !player.Exists) return;
                player.isRobbingAShop = false;

                if (!player.Position.IsInRange(ServerShops.GetShopPosition(shopId), 5f))
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Du bist zu weit entfernt, der Raub wurde abgebrochen.");
                    player.isRobbingAShop = false;
                    return;
                }

                int amount = new Random().Next(500, 5000);
                HUDHandler.SendNotification(player, 2, 2500, $"Shop ausgeraubt - du erhälst {amount}$.");
                CharactersInventory.AddCharacterItem(player.CharacterId, "Bargeld", amount, "brieftasche");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        #endregion

        #region VehicleShop

        internal static void OpenVehicleShop(IPlayer player, string shopname, int shopId)
        {
            if (player == null || !player.Exists || shopId <= 0) return;
            var array = ServerVehicleShops.GetVehicleShopItems(shopId);
            player.EmitLocked("Client:VehicleShop:OpenCEF", shopId, shopname, array);
        }

        [AsyncClientEvent("Server:VehicleShop:BuyVehicle")]
        public void BuyVehicle(IPlayer player, int shopid, string hash)
        {
            try
            {
                if (player == null || !player.Exists || shopid <= 0 || hash == "") return;
                long fHash = Convert.ToInt64(hash);
                int vehClass = ServerVehicles.VehicleClass(fHash);
                int charId = User.GetPlayerOnline(player);
                if (charId == 0 || fHash == 0) return;
                int Price = ServerVehicleShops.GetVehicleShopPrice(shopid, fHash);
                bool PlaceFree = true;
                Position ParkOut = ServerVehicleShops.GetVehicleShopOutPosition(shopid);
                Rotation RotOut = ServerVehicleShops.GetVehicleShopOutRotation(shopid);
                foreach (IVehicle veh in Alt.GetAllVehicles().ToList()) { if (veh.Position.IsInRange(ParkOut, 2f)) { PlaceFree = false; break; } }
                if (!PlaceFree) { HUDHandler.SendNotification(player, 3, 2500, $"Der Ausladepunkt ist belegt."); return; }
                int rnd = new Random().Next(100000, 999999);
                int serialNumber = new Random().Next(1, 10000);
                if (ServerVehicles.ExistServerVehiclePlate($"LS{rnd}")) { BuyVehicle(player, shopid, hash); return; }
                if (!CharactersInventory.ExistCharacterItem(charId, "Bargeld", "brieftasche") || CharactersInventory.GetCharacterItemAmount(charId, "Bargeld", "brieftasche") < Price) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht genügend Bargeld dabei ({Price}$)."); return; }
                CharactersInventory.RemoveCharacterItemAmount(charId, "Bargeld", Price, "brieftasche");
                if (shopid == 1)
                {
                    ServerVehicles.CreateVehicle(fHash, charId, 0, 2, false, 8, ParkOut, RotOut, $"LSPD", 255, 255, 255, vehClass, serialNumber);
                }
                else if (shopid == 2)
                {
                    ServerVehicles.CreateVehicle(fHash, charId, 0, 2, false, 9, ParkOut, RotOut, $"LSPD", 255, 255, 255, vehClass, serialNumber);
                }
                else if (shopid == 4)
                {
                    ServerVehicles.CreateVehicle(fHash, charId, 0, 3, false, 16, ParkOut, RotOut, $"LSFD", 255, 255, 255, vehClass, serialNumber);
                }
                else if (shopid == 5)
                {
                    ServerVehicles.CreateVehicle(fHash, charId, 0, 3, false, 21, ParkOut, RotOut, $"LSFD", 255, 255, 255, vehClass, serialNumber);
                }
                else if (shopid == 6)
                {
                    ServerVehicles.CreateVehicle(fHash, charId, 0, 14, false, 17, ParkOut, RotOut, $"BENNYS", 255, 255, 255, vehClass, serialNumber);
                }
                else if (shopid == 3)
                {
                    ServerVehicles.CreateVehicle(fHash, charId, 0, 2, false, 99, ParkOut, RotOut, $"LSPD", 255, 255, 255, vehClass, serialNumber);
                }
                else if (shopid == 33)
                {
                    ServerVehicles.CreateVehicle(fHash, charId, 0, 13, false, 21, ParkOut, RotOut, $"STARS", 255, 255, 255, vehClass, serialNumber);
                    CharactersInventory.AddCharacterItem(charId, $"Fahrzeugschluessel STARS", 2, "schluessel");
                }
                else if (shopid == 37)
                {
                    ServerVehicles.CreateVehicle(fHash, charId, 0, 4, false, 3, ParkOut, RotOut, $"ACLS", 168, 133, 50, vehClass, serialNumber);
                }
                else if (shopid == 38)
                {
                    ServerVehicles.CreateVehicle(fHash, charId, 0, 16, false, 3, ParkOut, RotOut, $"DCC", 168, 133, 50, vehClass, serialNumber);
                    CharactersInventory.AddCharacterItem(charId, $"Fahrzeugschluessel DCC", 2, "schluessel");
                }
                else if (shopid == 40)
                {
                    ServerVehicles.CreateVehicle(fHash, charId, 0, 12, false, 22, ParkOut, RotOut, $"FIB", 0, 0, 0, vehClass, serialNumber);
                }
                else
                {
                    ServerVehicles.CreateVehicle(fHash, charId, 0, 0, false, 1, ParkOut, RotOut, $"LS{rnd}", 255, 255, 255, vehClass, serialNumber);
                    CharactersInventory.AddCharacterItem(charId, $"Fahrzeugschluessel LS{rnd}", 2, "schluessel");
                    //CharactersInventory.AddCharacterItem(charId, $"Fahrzeugpapiere {serialNumber}", 2, "brieftasche");
                }
                HUDHandler.SendNotification(player, 2, 2500, $"Fahrzeug erfolgreich gekauft.");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static async void OpenVehicleShopManager(IPlayer player, string shopname, int shopId)
        {
            try
            {
                if (player == null || !player.Exists || shopId <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;

                int shopClosed = ServerShops.GetShopStateClosed(shopId);
                if (shopClosed == 1)
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Dein Shop wurde Zwangs Geschlossen! Wende dich an die Justiz");
                    return;
                }

                await HelperEvents.ClientEvent_setCefStatus(player, true);
                string shopItems = ServerShops.GetShopItems(shopId);
                string inventoryItems = CharactersInventory.CharacterInventoryItems(charId);
                int shopCash = ServerShops.GetShopBankMoney(shopId);
                Global.mGlobal.VirtualAPI.TriggerClientEventSafe(player, "Client:Shop:openShopManager", shopId, inventoryItems, shopItems, shopCash);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
        #endregion

        #region Clothes Shop
        public static void openClothesShop(ClassicPlayer player, int id)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0 || !ServerClothesShops.ExistClothesShop(id)) return;

                if (!player.HasData("clothesMenuOpen")) player.SetData("clothesMenuOpen", true);
                else
                {
                    Characters.SetCharacterCorrectClothes(player);
                    player.DeleteData("clothesMenuOpen");
                }

                player.EmitLocked("Client:Clothesstore:OpenMenu", Convert.ToInt32(Characters.GetCharacterGender(player.CharacterId)) + 1);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Clothesstore:BuyCloth")]
        public void buyClothesShopItem(ClassicPlayer player, int clothId, bool isProp)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0 || !ServerClothes.ExistClothes(clothId, Convert.ToInt32(Characters.GetCharacterGender(player.CharacterId)))) return;

                Characters.SwitchCharacterClothes(player, clothId, isProp);

                if (CharactersClothes.ExistCharacterClothes(player.CharacterId, clothId)) HUDHandler.SendNotification(player, 2, 1500, $"Du hast das Kleidungsstück angezogen.");
                else
                {
                    int price = ServerClothesShops.GetClothesPrice(player, clothId, isProp);
                    if (!CharactersInventory.ExistCharacterItem(player.CharacterId, "Bargeld", "brieftasche") || CharactersInventory.GetCharacterItemAmount(player.CharacterId, "Bargeld", "brieftasche") < price) HUDHandler.SendNotification(player, 2, 1500, $"Du hast nicht genug Geld, um dieses Kleidungsstück zu kaufen. (${price})"); ;
                    //if (!CharactersInventory.ExistCharacterItem(player.CharacterId, "Bargeld", "brieftasche") || CharactersInventory.GetCharacterItemAmount(player.CharacterId, "Bargeld", "brieftasche") < price)
                    //{
                    //    HUDHandler.SendNotification(player, 3, 2500, "Du hast nicht genügend Geld dabei.");
                    //    return;
                    //}
                    CharactersInventory.RemoveCharacterItemAmount(player.CharacterId, "Bargeld", price, "brieftasche");
                    HUDHandler.SendNotification(player, 2, 1500, $"Du hast dir das Kleidungsstück für ${price} gekauft.");
                    CharactersClothes.CreateCharacterOwnedClothes(player.CharacterId, clothId);
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Clothesstore:SetPerfectTorso")]
        public void ClothesshopSetPerfectTorso(ClassicPlayer player, int BestTorsoDrawable, int BestTorsoTexture)
        {
            try
            {
                int clothId = ServerClothes.GetClothesId(3, BestTorsoDrawable, BestTorsoTexture, Convert.ToInt32(Characters.GetCharacterGender(player.CharacterId)));
                if (player == null || !player.Exists || player.CharacterId <= 0 || !ServerClothes.ExistClothes(clothId, Convert.ToInt32(Characters.GetCharacterGender(player.CharacterId)))) return;

                Characters.SwitchCharacterClothes(player, clothId, false);
                CharactersClothes.CreateCharacterOwnedClothes(player.CharacterId, clothId);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        #endregion

        #region Tattoo Shop
        internal static void openTattooShop(ClassicPlayer player, Server_Tattoo_Shops tattooShop)
        {
            if (player == null || !player.Exists || player.CharacterId <= 0 || player.IsCefOpen() || tattooShop == null) return;
            LoginHandler.setCefStatus(player, true);
            int gender = Convert.ToInt32(Characters.GetCharacterGender(player.CharacterId));
            player.EmitAsync("Client:TattooShop:openShop", gender, tattooShop.id, CharactersTattoos.GetAccountOwnTattoos(player.CharacterId));
        }

        [AsyncClientEvent("Server:TattooShop:buyTattoo")]
        public void ClientEvent_buyTattoo(ClassicPlayer player, int shopId, int tattooId)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0 || shopId <= 0 || tattooId <= 0 || !ServerTattoos.ExistTattoo(tattooId) || CharactersTattoos.ExistAccountTattoo(player.CharacterId, tattooId) || !ServerTattooShops.ExistTattooShop(shopId)) return;
                int price = ServerTattoos.GetTattooPrice(tattooId);
                if (!CharactersInventory.ExistCharacterItem(player.CharacterId, "Bargeld", "brieftasche") || CharactersInventory.GetCharacterItemAmount(player.CharacterId, "Bargeld", "brieftasche") < price)
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Fehler: Du hast nicht genügend Geld dabei ({price}$).");
                    return;
                }
                CharactersInventory.RemoveCharacterItemAmount(player.CharacterId, "Bargeld", price, "brieftasche");
                ServerTattooShops.SetTattooShopBankMoney(shopId, ServerTattooShops.GetTattooShopBank(shopId) + price);
                CharactersTattoos.CreateNewEntry(player.CharacterId, tattooId);
                HUDHandler.SendNotification(player, 2, 2500, $"Du hast das Tattoo '{ServerTattoos.GetTattooName(tattooId)}' für {price}$ gekauft.");
                player.updateTattoos();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [AsyncClientEvent("Server:TattooShop:deleteTattoo")]
        public void ClientEvent_deleteTattoo(ClassicPlayer player, int tattooId)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0 || tattooId <= 0 || !CharactersTattoos.ExistAccountTattoo(player.CharacterId, tattooId)) return;
                CharactersTattoos.RemoveAccountTattoo(player.CharacterId, tattooId);
                HUDHandler.SendNotification(player, 2, 2500, $"Du hast das Tattoo '{ServerTattoos.GetTattooName(tattooId)}' erfolgreich entfernen lassen.");
                player.updateTattoos();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
        #endregion

        #region waschstrasse
        internal static async Task usewaschstrasse(IPlayer player)
        {
            try
            {

                IVehicle veh = player.Vehicle;
                if (player == null && !player.Exists && player.GetCharacterMetaId() == 0 && Characters.GetCharacterAccountId((int)player.GetCharacterMetaId()) == 0 && player.IsInVehicle == false) return;
                if (veh == null && !veh.Exists) return;

                var charId = (int)player.GetCharacterMetaId();
                var WashPrice = 50; //HIER ÄNDERN
                var zeit = 25000;    //HIER ÄNDERN
                bool engineState = ServerVehicles.GetVehicleEngineState(veh);
                int rnd1 = new Random().Next(50, 100);

                if (CharactersInventory.GetCharacterItemAmount(charId, "Bargeld", "brieftasche") <= WashPrice) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht genug Bargeld! {WashPrice}"); return; }
                if (veh.EngineOn == true) { HUDHandler.SendNotification(player, 3, 2500, $"Der Motor muss dafür aus sein!"); return; }
                HUDHandler.SendProgress(player, $"Das Fahrzeug wird gereinigt", "alert", zeit);
                await Task.Delay(zeit);
                if (rnd1 == 1) { HUDHandler.SendNotification(player, 3, 2500, $"Während des Waschens ist Wasser in den Motor gekommen!"); return; }
                if (rnd1 == 1) { HUDHandler.SendNotification(player, 3, 2500, $"Bitte Rufe einen Abschlepper, damit dein Fahrzeug repariert werden kann!"); return; }
                if (rnd1 == 1) { ServerVehicles.SetVehicleEngineHealthy(veh, false); return; }
                else
                {
                    HUDHandler.SendNotification(player, 2, 2500, $"Das Waschen ist abgeschlossen und das Fahrzeug ist wieder sauber!");
                    Alt.EmitAllClients("Client:Utilities:washVehicle", veh);
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Bargeld", WashPrice, "brieftasche");
                }

            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        #endregion
    }
}
