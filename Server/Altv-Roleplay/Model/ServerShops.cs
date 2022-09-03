using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Handler;
using Altv_Roleplay.models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Altv_Roleplay.Model
{
    class ServerShops
    {
        public static List<Server_Shops> ServerShops_ = new List<Server_Shops>();
        public static List<Server_Shops_Items> ServerShopsItems_ = new List<Server_Shops_Items>();

        public static int GetShopItemAmount(int shopId, string itemName)
        {
            Server_Shops_Items shopItem = ServerShopsItems_.ToList().FirstOrDefault(x => x.shopId == shopId && x.itemName == itemName);
            if (shopItem != null) return shopItem.itemAmount;
            return 0;
        }

        public static bool ExistShopItem(int shopId, string itemName)
        {
            Server_Shops_Items shopItem = ServerShopsItems_.ToList().FirstOrDefault(x => x.shopId == shopId && x.itemName == itemName);
            if (shopItem != null) return true;
            return false;
        }

        public static void RemoveShopItemAmount(int shopId, string itemName, int itemAmount)
        {
            try
            {
                if (shopId <= 0 || string.IsNullOrWhiteSpace(itemName) || itemAmount <= 0) return;
                Server_Shops_Items shopItem = ServerShopsItems_.FirstOrDefault(x => x.shopId == shopId && x.itemName == itemName);
                if (shopItem == null) return;
                shopItem.itemAmount -= itemAmount;
                if (shopItem.itemAmount > 0)
                {
                    //Item existiert noch, updaten.
                    using (gtaContext db = new gtaContext())
                    {
                        db.Server_Shops_Items.Update(shopItem);
                        db.SaveChanges();
                    }
                }
                else
                {
                    //Item existiert nicht mehr, löschen.
                    ServerShopsItems_.Remove(shopItem);
                    using (gtaContext db = new gtaContext())
                    {
                        db.Server_Shops_Items.Remove(shopItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void AddShopItem(int shopId, string itemName, int itemAmount)
        {
            try
            {
                if (shopId <= 0 || string.IsNullOrWhiteSpace(itemName) || itemAmount <= 0) return;
                Server_Shops_Items existItem = ServerShopsItems_.FirstOrDefault(x => x.shopId == shopId && x.itemName == itemName);
                if(existItem != null)
                {
                    //Item existiert, Anzahl erhöhen.
                    existItem.itemAmount += itemAmount;
                    using (gtaContext db = new gtaContext())
                    {
                        db.Server_Shops_Items.Update(existItem);
                        db.SaveChanges();
                    }
                }
                else
                {
                    //Item existiert nicht, neu erstellen.
                    Server_Shops_Items item = new Server_Shops_Items
                    {
                        itemName = itemName,
                        itemPrice = 99999,
                        itemAmount = itemAmount,
                        shopId = shopId
                    };

                    ServerShopsItems_.Add(item);
                    using (gtaContext db = new gtaContext())
                    {
                        db.Server_Shops_Items.Add(item);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void SetShopItemPrice(int shopId, string itemName, int newPrice)
        {
            try
            {
                Server_Shops_Items shopItem = ServerShopsItems_.FirstOrDefault(x => x.shopId == shopId && x.itemName == itemName);
                if (shopItem == null || shopItem.itemPrice == newPrice) return;
                shopItem.itemPrice = newPrice;
                using (gtaContext db = new gtaContext())
                {
                    db.Server_Shops_Items.Update(shopItem);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetShopOwner(int shopId)
        {
            Server_Shops shop = ServerShops_.ToList().FirstOrDefault(x => x.id == shopId);
            if (shop != null) return shop.owner;
            return 0;
        }

        public static int GetShopPrice(int shopId)
        {
            Server_Shops shop = ServerShops_.ToList().FirstOrDefault(x => x.id == shopId);
            if (shop != null) return shop.price;
            return 0;
        }

        public static void SetShopBankMoney(int shopId, int money)
        {
            try
            {
                Server_Shops shop = ServerShops_.FirstOrDefault(x => x.id == shopId);
                if (shop == null) return;
                shop.bank = money;
                using (gtaContext db = new gtaContext())
                {
                    db.Server_Shops.Update(shop);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void SetShopOwner(int shopId, int owner)
        {
            try
            {
                Server_Shops shop = ServerShops_.FirstOrDefault(x => x.id == shopId);
                if (shop == null) return;
                shop.owner = owner;
                using (gtaContext db = new gtaContext())
                {
                    db.Server_Shops.Update(shop);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetShopBankMoney(int shopId)
        {
            Server_Shops shop = ServerShops_.ToList().FirstOrDefault(x => x.id == shopId);
            if (shop != null) return shop.bank;
            return 0;
        }

        public static string GetShopItems(int shopId)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(ServerShopsItems_.ToList().Where(x => x.shopId == shopId).Select(x => new
            {
                x.itemName,
                x.itemPrice,
                x.itemAmount,
                itemPic = ServerItems.ReturnItemPicSRC(x.itemName),
            }).ToList());
            return json;
        }

        public static string GetShopSellItems(int charId, int shopId)
        {
            if (charId == 0 || shopId == 0) return "";
            var json = System.Text.Json.JsonSerializer.Serialize(ServerShopsItems_.Where(x => x.shopId == shopId && (CharactersInventory.ExistCharacterItem(charId, x.itemName, "inventory") == true || CharactersInventory.ExistCharacterItem(charId, x.itemName, "backpack") == true)).Select(x => new
            {
                x.itemName,
                x.itemPrice,
                itemAmount = CharactersInventory.GetCharacterItemAmount(charId, x.itemName, "inventory") + CharactersInventory.GetCharacterItemAmount(charId, x.itemName, "backpack"),
                itemPic = ServerItems.ReturnItemPicSRC(x.itemName),
            }).ToList());

            return json;
        }

        public static string GetFreeShops()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(ServerShops_.ToList().Where(x => x.owner == 0 && x.faction == 0 && x.type == 0 && x.isOnlySelling == false).Select(x => new
            {
                x.id,
                x.price,
                pos = x.shopPos,
                x.name,
                x.bank,
            }).OrderBy(x => x.id).ToList());
        }

        public static string GetAccountShops(int charId)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(ServerShops_.ToList().Where(x => x.owner == charId).Select(x => new
            {
                x.id,
                x.price,
                x.name,
                pos = x.shopPos,
                x.bank,
            }).OrderBy(x => x.id).ToList());
        }

        public static string GetAllShops()
        {
            var items = ServerShops_.Where(x => x.owner > 0 && x.faction == 0 && x.type == 0 && x.isOnlySelling == false).Select(x => new
            {
                x.id,
                x.name,
                owner = Characters.GetCharacterName(x.owner),
                x.bank,
                pos = GetShopPosition(x.id),
            }).ToList();

            return JsonConvert.SerializeObject(items);
        }

        public static Position GetShopPosition(int shopId)
        {
            Server_Shops shop = ServerShops_.ToList().FirstOrDefault(x => x.id == shopId);
            if (shop != null) return shop.shopPos;
            return new Position(0, 0, 0);
        }

        public static bool IsShopRobbedNow(int shopId)
        {
            try
            {
                Server_Shops shop = ServerShops_.ToList().FirstOrDefault(x => x.id == shopId);
                if (shop != null) return shop.isRobbedNow;
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return false;
        }

        public static void SetShopRobbedNow(int shopId, bool isRobbedNow)
        {
            try
            {
                Server_Shops shop = ServerShops_.ToList().FirstOrDefault(x => x.id == shopId);
                if (shop == null) return;
                shop.isRobbedNow = isRobbedNow;
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static string GetShopNeededLicense(int shopId)
        {
            if (shopId <= 0) return "";
            Server_Shops shop = ServerShops_.ToList().FirstOrDefault(x => x.id == shopId);
            if (shop != null)
            {
                return shop.neededLicense;
            }
            return "";
        }

        public static int GetShopType(int shopId)
        {
            Server_Shops shopItem = ServerShops_.ToList().FirstOrDefault(x => x.id == shopId);
            if (shopItem != null) return shopItem.type;
            return 0;
        }

        public static int GetShopFaction(int shopId)
        {
            Server_Shops shopItem = ServerShops_.ToList().FirstOrDefault(x => x.id == shopId);
            if (shopItem != null) return shopItem.faction;
            return 0;
        }

        public static int GetItemPrice(int shopId, string itemName)
        {
            Server_Shops_Items shopItem = ServerShopsItems_.ToList().FirstOrDefault(x => x.shopId == shopId && x.itemName == itemName);
            if (shopItem != null) return shopItem.itemPrice;
            return 0;
        }

        public static int GetShopsOwned(int charId)
        {
            try
            {
                if (charId <= 0) return 0;
                var shop = ServerShops_.Where(x => x.owner == charId);
                if (shop != null) return shop.Count();
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return 0;
        }
    }
}
