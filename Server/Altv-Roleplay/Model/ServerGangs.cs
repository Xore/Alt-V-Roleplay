using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Handler;
using Altv_Roleplay.models;
using Altv_Roleplay.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altv_Roleplay.Model
{
    class ServerGangs
    {
        public static List<Server_Gangs> ServerGangs_ = new List<Server_Gangs>();
        public static List<Server_Gang_Ranks> ServerGangRanks_ = new List<Server_Gang_Ranks>();
        public static List<Server_Gang_Members> ServerGangMembers_ = new List<Server_Gang_Members>();
        public static List<Server_Gang_Storage_Items> ServerGangStorageItems_ = new List<Server_Gang_Storage_Items>();
        public static List<Server_Gang_Positions> ServerGangPositions_ = new List<Server_Gang_Positions>();

        public static void CreateServerGangMember(int gangId, int charId, int rank)
        {
            try
            {
                if (gangId == 0 || charId == 0) return;
                var gangMemberData = ServerGangMembers_.FirstOrDefault(x => x.charId == charId);
                if (gangMemberData != null) return;
                var gangInviteData = new Server_Gang_Members
                {
                    charId = charId,
                    gangId = gangId,
                    rank = rank,
                    lastChange = DateTime.Now
                };
                ServerGangMembers_.Add(gangInviteData);
                using (gtaContext db = new gtaContext())
                {
                    db.Server_Gang_Members.Add(gangInviteData);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static bool ExistGang(int gangId)
        {
            return ServerGangs_.ToList().Exists(x => x.id == gangId);
        }

        internal static void UpdateCurrent(int gangId, int v)
        {
            throw new NotImplementedException();
        }

        public static void RemoveServerGangMember(int gangId, int charId)
        {
            try
            {
                if (gangId <= 0 || charId <= 0) return;
                var gangMemberData = ServerGangMembers_.FirstOrDefault(x => x.gangId == gangId && x.charId == charId);
                if (gangMemberData != null)
                {
                    ServerGangMembers_.Remove(gangMemberData);
                    using (gtaContext db = new gtaContext())
                    {
                        db.Server_Gang_Members.Remove(gangMemberData);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static void AddServerGangStorageItem(int gangId, int charId, string itemName, int itemAmount)
        {
            if (charId <= 0 || gangId <= 0 || itemName == "" || itemAmount <= 0) return;

            var itemData = new Server_Gang_Storage_Items
            {
                charId = charId,
                gangId = gangId,
                itemName = itemName,
                amount = itemAmount
            };

            try
            {
                var hasItem = ServerGangStorageItems_.FirstOrDefault(i => i.charId == charId && i.itemName == itemName && i.gangId == gangId);
                if (hasItem != null)
                {
                    //Item existiert, itemAmount erhöhen
                    hasItem.amount += itemAmount;
                    using (gtaContext db = new gtaContext())
                    {
                        var dbitem = db.Server_Gang_Storage_Items.FirstOrDefault(i => i.charId == charId && i.itemName == itemName && i.gangId == gangId);
                        if (dbitem != null)
                        {
                            dbitem.amount = dbitem.amount += itemAmount;
                        }
                        db.SaveChanges();
                    }
                }
                else
                {
                    //Existiert nicht, Item neu adden
                    ServerGangStorageItems_.Add(itemData);
                    using (gtaContext db = new gtaContext())
                    {
                        db.Server_Gang_Storage_Items.Add(itemData);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static int GetServerGangStorageItemAmount(int gangId, int charId, string itemName)
        {
            try
            {
                if (gangId <= 0 || charId <= 0 || itemName == "") return 0;
                var item = ServerGangStorageItems_.FirstOrDefault(x => x.gangId == gangId && /*x.charId == charId &&*/ x.itemName == itemName);
                if (item != null) return item.amount;
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return 0;
        }

        public static void RemoveServerGangStorageItemAmount(int gangId, int charId, string itemName, int itemAmount)
        {
            try
            {
                if (charId <= 0 || itemName == "" || itemAmount == 0 || gangId <= 0) return;
                var item = ServerGangStorageItems_.FirstOrDefault(i => /*i.charId == charId &&*/ i.itemName == itemName && i.gangId == gangId);
                if (item != null)
                {
                    using (gtaContext db = new gtaContext())
                    {
                        int prevAmount = item.amount;
                        item.amount -= itemAmount;
                        if (item.amount > 0)
                        {
                            db.Server_Gang_Storage_Items.Update(item);
                            db.SaveChanges();
                        }
                        else
                            RemoveServerGangStorageItem(gangId, charId, itemName);
                    }
                }
            }
            catch (Exception _) { Alt.Log($"{_}"); }
        }

        public static void RemoveServerGangStorageItem(int gangId, int charId, string itemName)
        {
            try
            {
                var item = ServerGangStorageItems_.FirstOrDefault(i => /*i.charId == charId &&*/ i.itemName == itemName && i.gangId == gangId);
                if (item != null)
                {
                    ServerGangStorageItems_.Remove(item);
                    using (gtaContext db = new gtaContext())
                    {
                        db.Server_Gang_Storage_Items.Remove(item);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static string GetGangFullName(int gangId)
        {
            if (gangId <= 0) return "Zivilist";
            string fullName = "Zivilist";
            var gangData = ServerGangs_.FirstOrDefault(x => x.id == gangId);
            if (gangData != null)
            {
                fullName = gangData.gangName;
            }
            return fullName;
        }

        public static string GetGangShortName(int gangId)
        {
            string shortCut = "Zivilist";
            if (gangId == 0) return shortCut;
            var gangData = ServerGangs_.FirstOrDefault(x => x.id == gangId);
            if (gangData != null)
            {
                shortCut = gangData.gangShort;
            }
            return shortCut;
        }

        public static string GetGangRankName(int gangId, int rankId)
        {
            if (gangId == 0 || rankId == 0) return "";
            var gangRankData = ServerGangRanks_.FirstOrDefault(x => x.gangId == gangId && x.rankId == rankId);
            if (gangRankData != null)
            {
                return gangRankData.name;
            }
            return "";
        }

        public static int GetCharacterGangId(int charId)
        {
            try
            {
                if (charId == 0) return 0;
                var gangMemberData = ServerGangMembers_.FirstOrDefault(x => x.charId == charId);
                if (gangMemberData != null)
                {
                    return gangMemberData.gangId;
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return 0;
        }

        public static int GetCharacterGangRank(int charId)
        {
            try
            {
                if (charId == 0) return 0;
                var gangMemberData = ServerGangMembers_.FirstOrDefault(x => x.charId == charId);
                if (gangMemberData != null)
                {
                    return gangMemberData.rank;
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return 0;
        }

        public static void SetCharacterGangRank(int charId, int newRank)
        {
            try
            {
                if (charId <= 0 || newRank <= 0) return;
                var gangMemberData = ServerGangMembers_.FirstOrDefault(x => x.charId == charId);
                if (gangMemberData != null)
                {
                gangMemberData.rank = newRank;
                    using (gtaContext db = new gtaContext())
                    {
                        db.Server_Gang_Members.Update(gangMemberData);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static int GetGangMaxRankCount(int gangId)
        {
            int rankCount = 0;
            try
            {
                if (gangId == 0) return rankCount;
                rankCount = ServerGangRanks_.Where(x => x.gangId == gangId).Count();
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return rankCount;
        }

        public static int GetServerGangMemberCount(int gangId)
        {
            int Count = 0;
            if (gangId == 0) return Count;
            Count = ServerGangMembers_.Where(x => x.gangId == gangId).Count();
            return Count;
        }

        public static int GetServerGangLeader(int gangId)
        {
            if (gangId == 0) return 0;
            var gangMemberData = ServerGangMembers_.FirstOrDefault(x => x.gangId == gangId && x.rank == GetGangMaxRankCount(gangId));
            if (gangMemberData != null)
            {
                return gangMemberData.charId;
            }
            return 0;
        }

        public static bool IsCharacterInAnyGang(int charId)
        {
            try
            {
                if (charId == 0) return false;
                var gangMemberData = ServerGangMembers_.FirstOrDefault(x => x.charId == charId);
                if (gangMemberData != null)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return false;
        }

        public static string GetServerGangManagerInfos(int gangId)
        {
            if (gangId == 0) return "";

            var items = ServerGangs_.Where(x => x.id == gangId).Select(x => new
            {
                gangId,
                x.gangName,
                gangOwner = Characters.GetCharacterName(GetServerGangLeader(gangId)),
                gangMemberCount = GetServerGangMemberCount(gangId),
            }).ToList();

            return JsonConvert.SerializeObject(items);
        }

        public static string GetServerGangMembers(int gangId)
        {
            if (gangId == 0) return "";

            var items = ServerGangMembers_.Where(x => x.gangId == gangId).Select(x => new
            {
                x.gangId,
                x.charId,
                charName = Characters.GetCharacterName(x.charId),
                x.rank,
                date = x.lastChange.ToString("dd.MM.yyy"),
            }).OrderByDescending(x => x.rank).ToList();

            return JsonConvert.SerializeObject(items);
        }

        public static string GetServerGangRanks(int gangId)
        {
            if (gangId == 0) return "";

            var items = ServerGangRanks_.Where(x => x.gangId == gangId).Select(x => new
            {
                x.gangId,
                x.rankId,
                rankName = x.name,
            }).OrderBy(x => x.rankId).ToList();

            return JsonConvert.SerializeObject(items);
        }

        public static string GetServerGangStorageItems(int gangId, int charId)
        {
            if (gangId <= 0 || charId <= 0) return "[]";
            var items = ServerGangStorageItems_.Where(x => x.gangId == gangId /*&& x.charId == charId*/).Select(x => new
            {
                x.id,
                //x.charId,
                x.gangId,
                x.itemName,
                x.amount,
                itemPicName = ServerItems.ReturnItemPicSRC(x.itemName),
            }).ToList();
            return JsonConvert.SerializeObject(items);
        }

        public static bool ExistServerGangStorageItem(int gangId, int charId, string itemName)
        {
            try
            {
                if (gangId <= 0 || charId <= 0 || itemName == "") return false;
                var storageData = ServerGangStorageItems_.FirstOrDefault(x => x.gangId == gangId /*&& x.charId == charId*/ && x.itemName == itemName);
                if (storageData != null) return true;
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return false;
        }

        public static bool ExistServerGangRankOnId(int gangId, int rankId)
        {
            try
            {
                if (gangId <= 0 || rankId <= 0) return false;
                var gangRankData = ServerGangRanks_.FirstOrDefault(x => x.gangId == gangId && x.rankId == rankId);
                if (gangRankData != null)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return false;
        }
    }
}
