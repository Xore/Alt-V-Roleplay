using AltV.Net;
using AltV.Net.Async;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;

namespace Altv_Roleplay.Handler
{
    class DynastyHandler : IScript
    {
        [AsyncClientEvent("Server:Dynasty:buyShop")]
        public void buyShop(ClassicPlayer player, int shopId)
        {
            int charId = User.GetPlayerOnline(player);
            if (player == null || !player.Exists || charId <= 0 || shopId <= 0 || ServerShops.GetShopOwner(shopId) != 0) return;
            int price = ServerShops.GetShopPrice(shopId);
            if (ServerShops.GetShopsOwned(charId) >= 1) { HUDHandler.SendNotification(player, 3, 2500, "Dir gehört bereits 1 Geschäft!"); return; }
            if (!CharactersInventory.ExistCharacterItem(charId, "Bargeld", "brieftasche")) { HUDHandler.SendNotification(player, 3, 2500, "Du hast nicht genügend Bargeld dabei."); return; }
            if (CharactersInventory.GetCharacterItemAmount(charId, "Bargeld", "brieftasche") < price) { HUDHandler.SendNotification(player, 3, 2500, "Du hast nicht genügend Bargeld dabei."); return; }
            CharactersInventory.RemoveCharacterItemAmount(charId, "Bargeld", price, "brieftasche");
            ServerShops.SetShopOwner(shopId, charId);
            HUDHandler.SendNotification(player, 2, 2500, $"Du hast den Shop {shopId} für {price}$ gekauft.");
        }

        [AsyncClientEvent("Server:Dynasty:sellShop")]
        public void sellShop(ClassicPlayer player, int shopId)
        {
            int charId = User.GetPlayerOnline(player);
            if (player == null || !player.Exists || charId <= 0 || shopId <= 0 || ServerShops.GetShopOwner(shopId) != charId) return;
            int price = ServerShops.GetShopPrice(shopId) / 2;
            CharactersInventory.AddCharacterItem(charId, "Bargeld", price, "brieftasche");
            ServerShops.SetShopOwner(shopId, 0);
            HUDHandler.SendNotification(player, 2, 2500, $"Du hast den Shop {shopId} für {price}$ verkauft.");
        }

        [AsyncClientEvent("Server:Dynasty:buyStorage")]
        public void buyStorage(ClassicPlayer player, int storageId)
        {
            if (player == null || !player.Exists || User.GetPlayerOnline(player) <= 0 || !ServerStorages.ExistStorage(storageId) || ServerStorages.GetOwner(storageId) != 0) return;
            int price = ServerStorages.GetPrice(storageId);
            int charId = User.GetPlayerOnline(player);
            if (!CharactersInventory.ExistCharacterItem(charId, "Bargeld", "brieftasche")) { HUDHandler.SendNotification(player, 3, 2500, "Du hast nicht genügend Bargeld dabei."); return; }
            if (CharactersInventory.GetCharacterItemAmount(charId, "Bargeld", "brieftasche") < price) { HUDHandler.SendNotification(player, 3, 2500, "Du hast nicht genügend Bargeld dabei."); return; }
            CharactersInventory.RemoveCharacterItemAmount(charId, "Bargeld", price, "brieftasche");
            ServerStorages.SetOwner(storageId, User.GetPlayerOnline(player));
            ServerStorages.SetSecondOwner(storageId, 0);
            HUDHandler.SendNotification(player, 2, 2500, $"Du hast die Lagerhalle {storageId} für {price}$ gekauft.");
        }

        [AsyncClientEvent("Server:Dynasty:sellStorage")]
        public void sellStorage(ClassicPlayer player, int storageId)
        {
            if (player == null || !player.Exists || User.GetPlayerOnline(player) <= 0 || !ServerStorages.ExistStorage(storageId) || ServerStorages.GetOwner(storageId) != User.GetPlayerOnline(player)) return;
            int price = ServerStorages.GetPrice(storageId) / 2;
            int charId = User.GetPlayerOnline(player);
            CharactersInventory.AddCharacterItem(charId, "Bargeld", price, "brieftasche");
            ServerStorages.SetOwner(storageId, 0);
            ServerStorages.SetSecondOwner(storageId, 0);
            ServerStorages.SetStorageLocked(storageId, true);
            HUDHandler.SendNotification(player, 2, 2500, $"Du hast die Lagerhalle {storageId} für {price}$ verkauft.");
        }
    }
}