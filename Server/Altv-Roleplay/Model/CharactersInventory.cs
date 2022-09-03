using AltV.Net;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altv_Roleplay.Model
{
    class CharactersInventory
    {
        public static List<Characters_Inventory> CharactersInventory_ = new List<Characters_Inventory>();

        public static string GetCharacterInventory(int charId)
        {
            if (charId <= 0) return "None";
            var existUser = User.GetPlayerByCharId(charId);
            if (existUser == null) return "None";

            if (existUser.playerid <= 0) return "None";
            var items = CharactersInventory_.ToList().Where(x => x.charId == charId).Select(x => new
            {
                itemName = x.itemName,
                itemAmount = x.itemAmount,
                itemPicName = ServerItems.ReturnItemPicSRC(x.itemName),
                itemWeight = ServerItems.GetItemWeight(x.itemName),
                itemLocation = x.itemLocation,
                isItemDroppable = ServerItems.IsItemDroppable(ServerItems.ReturnNormalItemName(x.itemName)),
                isItemUseable = ServerItems.IsItemUseable(ServerItems.ReturnNormalItemName(x.itemName)),
                isItemGiveable = ServerItems.IsItemGiveable(ServerItems.ReturnNormalItemName(x.itemName)),
            }).ToList();

            return JsonConvert.SerializeObject(items);
        }
        public static string CharacterInventoryItems(int charId)
        {
            if (charId <= 0) return "None";
            var existUser = User.GetPlayerByCharId(charId);
            if (existUser == null) return "None";

            if (existUser.playerid <= 0) return "None";
            var items = CharactersInventory_.ToList().Where(x => x.charId == charId && x.itemLocation == "inventory").Select(x => new
            {
                itemName = x.itemName,
                itemAmount = x.itemAmount,
                itemPicName = ServerItems.ReturnItemPicSRC(x.itemName),
                itemWeight = ServerItems.GetItemWeight(x.itemName),
                itemLocation = x.itemLocation,
                isItemDroppable = ServerItems.IsItemDroppable(ServerItems.ReturnNormalItemName(x.itemName)),
                isItemUseable = ServerItems.IsItemUseable(ServerItems.ReturnNormalItemName(x.itemName)),
                isItemGiveable = ServerItems.IsItemGiveable(ServerItems.ReturnNormalItemName(x.itemName)),
            }).ToList();

            return JsonConvert.SerializeObject(items);
        }
        public static string CharacterBackpackItems(int charId)
        {
            if (charId <= 0) return "None";
            var existUser = User.GetPlayerByCharId(charId);
            if (existUser == null) return "None";

            if (existUser.playerid <= 0) return "None";
            var items = CharactersInventory_.ToList().Where(x => x.charId == charId && x.itemLocation == "backpack").Select(x => new
            {
                itemName = x.itemName,
                itemAmount = x.itemAmount,
                itemPicName = ServerItems.ReturnItemPicSRC(x.itemName),
                itemWeight = ServerItems.GetItemWeight(x.itemName),
                itemLocation = x.itemLocation,
                isItemDroppable = ServerItems.IsItemDroppable(ServerItems.ReturnNormalItemName(x.itemName)),
                isItemUseable = ServerItems.IsItemUseable(ServerItems.ReturnNormalItemName(x.itemName)),
                isItemGiveable = ServerItems.IsItemGiveable(ServerItems.ReturnNormalItemName(x.itemName)),
            }).ToList();

            return JsonConvert.SerializeObject(items);
        }

        public static int GetCharacterBackpackItemCount(int charId)
        {
            if (charId <= 0) return 0;
            var existUser = User.GetPlayerByCharId(charId);

            if (existUser.playerid <= 0) return 0;
            return CharactersInventory_.ToList().Where(x => x.charId == charId && x.itemLocation == "backpack").Count();
        }

        public static void AddCharacterItem(int charId, string itemName, int itemAmount, string itemLocation)
        {
            if (charId == 0 || itemName == "" || itemLocation == "" || itemAmount == 0) return;

            var existUser = User.GetPlayerByCharId(charId);
            if (existUser == null) return;

            if (existUser.playerid <= 0) return;
            var itemData = new Characters_Inventory
            {
                charId = charId,
                itemName = itemName,
                itemAmount = itemAmount,
                itemLocation = itemLocation
            };

            try
            {
                var hasItem = CharactersInventory_.ToList().FirstOrDefault(i => i.charId == charId && i.itemName == itemName && i.itemLocation == itemLocation);
                if (hasItem != null)
                {
                    //Item existiert, itemAmount erhöhen
                    hasItem.itemAmount += itemAmount;

                    using (gtaContext db = new gtaContext())
                    {
                        var dbitem = db.Characters_Inventory.ToList().FirstOrDefault(i => i.charId == charId && i.itemName == itemName && i.itemLocation == itemLocation);
                        if (dbitem != null)
                        {
                            dbitem.itemAmount = dbitem.itemAmount += itemAmount;
                        }
                        db.SaveChanges();
                    }
                }
                else
                {
                    //Existiert nicht, Item neu adden
                    CharactersInventory_.Add(itemData);
                    using (gtaContext db = new gtaContext())
                    {
                        db.Characters_Inventory.Add(itemData);
                        db.SaveChanges();
                    }
                }

                if (itemName == "Bargeld")
                {
                    var tplayer = Alt.GetAllPlayers().ToList().FirstOrDefault(x => User.GetPlayerOnline(x) == charId);
                    tplayer.Emit("Client:HUD:updateMoney", CharactersInventory.GetCharacterItemAmount(User.GetPlayerOnline(tplayer), "Bargeld", "brieftasche"));
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
                return;
            }
        }

        public static void RemoveCharacterItemAmount(int charId, string itemName, int itemAmount, string itemLocation)
        {
            try
            {
                if (itemName == null || itemLocation == null) return;
                if (charId == 0 || itemName == "" || itemAmount == 0 || itemLocation == "") return;
                var existUser = User.GetPlayerByCharId(charId);
                if (existUser == null) return;

                if (existUser.playerid <= 0) return;
                var item = CharactersInventory_.FirstOrDefault(i => i.charId == charId && i.itemName == itemName && i.itemLocation == itemLocation);
                if (item != null)
                {
                    using (gtaContext db = new gtaContext())
                    {
                        int prevAmount = item.itemAmount;
                        item.itemAmount -= itemAmount;
                        if (item.itemAmount > 0)
                        {
                            db.Characters_Inventory.Update(item);
                            db.SaveChanges();
                        }
                        else
                        {
                            if (item != null)
                                if (itemName != null)
                                    if (itemLocation != null)
                                        RemoveCharacterItem(charId, itemName, itemLocation);
                        }
                    }
                }

                if (itemName == "Bargeld")
                {
                    var tplayer = Alt.GetAllPlayers().ToList().FirstOrDefault(x => User.GetPlayerOnline(x) == charId);
                    tplayer.Emit("Client:HUD:updateMoney", CharactersInventory.GetCharacterItemAmount(User.GetPlayerOnline(tplayer), "Bargeld", "brieftasche"));
                }
            }
            catch (Exception _) { Alt.Log($"{_}"); return; }
        }

        public static void RemoveCharacterItem2(int charId, string itemName)
        {
            try
            {
                if (itemName == null) return;
                if (charId <= 0) return;
                if (itemName == "" || itemName == "None") return;
                var existUser = User.GetPlayerByCharId(charId);
                if (existUser == null) return;

                if (existUser.playerid <= 0) return;
                var item = CharactersInventory_.FirstOrDefault(i => i.charId == charId && i.itemName == itemName);
                if (item != null)
                {
                    CharactersInventory_.Remove(item);
                    using (gtaContext db = new gtaContext())
                    {
                        db.Characters_Inventory.Remove(item);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
                return;
            }
        }

        public static void RemoveCharacterItemAmount2(int charId, string itemName, int itemAmount)
        {
            try
            {
                if (charId == 0 || itemName == "" || itemName == "None" || itemAmount == 0) return;
                var existUser = User.GetPlayerByCharId(charId);
                if (existUser == null) return;

                if (existUser.playerid <= 0) return;
                var item = CharactersInventory_.FirstOrDefault(i => i.charId == charId && i.itemName == itemName);
                if (item != null)
                {
                    using (gtaContext db = new gtaContext())
                    {
                        int prevAmount = item.itemAmount;
                        item.itemAmount -= itemAmount;
                        if (item.itemAmount > 0)
                        {
                            db.Characters_Inventory.Update(item);
                            db.SaveChanges();
                        }
                        else
                        {
                            if (item != null)
                                if (itemName != null)
                                    RemoveCharacterItem2(charId, itemName);
                        }
                    }
                }
            }
            catch (Exception _) { Alt.Log($"{_}"); return; }
        }

        public static void RemoveCharacterItem(int charId, string itemName, string itemLocation)
        {
            try
            {
                if (itemName == null || itemLocation == null) return;
                if (charId == 0 || itemName == "" || itemName == "None" || itemLocation == "" || itemLocation == "None") return;
                var existUser = User.GetPlayerByCharId(charId);
                if (existUser == null) return;

                if (existUser.playerid <= 0) return;
                var item = CharactersInventory_.FirstOrDefault(i => i.charId == charId && i.itemName == itemName && i.itemLocation == itemLocation);
                if (item != null)
                {
                    using (gtaContext db = new gtaContext())
                    {
                        db.Characters_Inventory.Remove(item);
                        db.SaveChanges();
                    }
                    if (item != null)
                        CharactersInventory_.Remove(item);
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
                return;
            }
        }

        public static bool ExistCharacterItem(int charId, string itemName, string itemLocation)
        {
            try
            {
                if (itemName == null || itemLocation == null) return false;
                if (charId == 0 || itemName == "" || itemName == "None" || itemLocation == "" || itemLocation == "None") return false;
                var existUser = User.GetPlayerByCharId(charId);
                if (existUser == null) return false;

                if (existUser.playerid <= 0) return false;
                var item = CharactersInventory_.ToList().FirstOrDefault(i => i.charId == charId && i.itemName == itemName && i.itemLocation == itemLocation);
                if (item != null) return true;
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
                return false;
            }
            return false;
        }

        public static void RenameCharactersItemName(string itemName, string newItemname)
        {
            try
            {
                if (itemName == null || newItemname == null) return;
                if (itemName == "" || newItemname == "") return;
                var items = CharactersInventory_.Where(i => i.itemName == itemName);
                if (items != null)
                {
                    foreach (var i in items.Where(x => x != null))
                    {
                        i.itemName = newItemname;
                        using (gtaContext db = new gtaContext())
                        {
                            db.Characters_Inventory.Update(i);
                            db.SaveChanges();
                        }
                    }
                }

                var factionStorageItems = ServerFactions.ServerFactionStorageItems_.Where(i => i.itemName == itemName);
                if (factionStorageItems != null)
                {
                    foreach (var i in factionStorageItems.Where(x => x != null))
                    {
                        i.itemName = newItemname;
                        using (gtaContext db = new gtaContext())
                        {
                            db.Server_Faction_Storage_Items.Update(i);
                            db.SaveChanges();
                        }
                    }
                }

                var vehicleItems = ServerVehicles.ServerVehicleTrunkItems_.Where(i => i.itemName == itemName);
                if (vehicleItems != null)
                {
                    foreach (var i in vehicleItems.Where(x => x != null))
                    {
                        i.itemName = newItemname;
                        using (gtaContext db = new gtaContext())
                        {
                            db.Server_Vehicle_Items.Update(i);
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
                return;
            }
        }

        public static int GetCharacterItemAmount(int charId, string itemName, string itemLocation)
        {
            try
            {
                if (itemName == null || itemLocation == null) return 0;
                if (charId == 0 || itemName == "" || itemName == "None" || itemLocation == "" || itemLocation == "None") return 0;
                var existUser = User.GetPlayerByCharId(charId);
                if (existUser == null) return 0;

                if (existUser.playerid <= 0) return 0;
                if (!CharactersInventory_.Any()) return 0;
                var item = CharactersInventory_.ToList().FirstOrDefault(i => i.charId == charId && i.itemName == itemName && i.itemLocation == itemLocation);
                if (item != null) return item.itemAmount;
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
                return 0;
            }
            return 0;
        }
        public static int GetCharacterItemAmount2(int charId, string itemName)
        {
            try
            {
                if (itemName == null || itemName == null) return 0;
                if (charId == 0 || itemName == "" || itemName == "None") return 0;
                var existUser = User.GetPlayerByCharId(charId);
                if (existUser == null) return 0;

                if (existUser.playerid <= 0) return 0;
                if (!CharactersInventory_.Any()) return 0;
                var item = CharactersInventory_.ToList().FirstOrDefault(i => i.charId == charId && i.itemName == itemName);
                if (item != null) return item.itemAmount;
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
                return 0;
            }
            return 0;
        }


        public static float GetCharacterItemWeight(int charId, string itemLocation)
        {
            try
            {
                if (itemLocation == null) return 0;
                if (charId == 0 || itemLocation == "" || itemLocation == "None") return 100f;
                if (charId <= 0 || string.IsNullOrWhiteSpace(itemLocation)) return 100f;
                var existUser = User.GetPlayerByCharId(charId);
                if (existUser == null) return 100f;

                if (existUser.playerid <= 0) return 100f;

                var item = CharactersInventory_.ToList().Where(i => i.charId == charId && i.itemLocation == itemLocation);
                if (item == null) return 100f;
                float invWeight = 0f;
                foreach (var i in item.Where(x => x != null && x.charId == charId))
                {
                    string iName = ServerItems.ReturnNormalItemName(i.itemName);
                    var serverItem = ServerItems.ServerItems_.ToList().FirstOrDefault(si => si.itemName == iName);
                    if (serverItem == null) continue;
                    invWeight += serverItem.itemWeight * i.itemAmount;
                }
                return invWeight;
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
                return 100f;
            }
        }

        public static bool IsItemActive(IPlayer player, string itemName)
        {
            try
            {
                if (player == null || !player.Exists) return false;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0 || itemName == "") return false;
                itemName = ServerItems.ReturnNormalItemName(itemName);
                if (itemName == "Rucksack" && Characters.GetCharacterBackpack(charId) == 31 || itemName == "Tasche" && Characters.GetCharacterBackpack(charId) == 45)
                {
                    return true;
                }
                else if ((string)Characters.GetCharacterWeapon(player, "PrimaryWeapon") == itemName || (string)Characters.GetCharacterWeapon(player, "SecondaryWeapon") == itemName || (string)Characters.GetCharacterWeapon(player, "SecondaryWeapon2") == itemName || (string)Characters.GetCharacterWeapon(player, "FistWeapon") == itemName)
                {
                    return true;
                }
                else if (itemName == "Smartphone" && Characters.IsCharacterPhoneEquipped(charId)) return true;
                return false;
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
                return false;
            }
        }
    }
}
