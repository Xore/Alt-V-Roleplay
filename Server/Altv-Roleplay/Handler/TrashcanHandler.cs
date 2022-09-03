using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Resources.Chat.Api;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Altv_Roleplay.Handler
{
    public partial class Server_Trashcan_Item
    {
        public string itemName { get; set; }
        public int itemAmount { get; set; }
    }

    public partial class Server_Trashcans
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public Position pos { get; set; }
        public List<Server_Trashcan_Item> items { get; set; } = new List<Server_Trashcan_Item>();
        public float maxSize { get; set; }
    }

    public class TrashcanHandler : IScript
    {
        public static List<Server_Trashcans> ServerTrashcans_ = new List<Server_Trashcans>();

        [Command("createtrashcan")]
        public void TrashCanCMD(ClassicPlayer player, int maxSize)
        {
            if (player == null || !player.Exists) return;
            Server_Trashcans can = new Server_Trashcans
            {
                pos = player.Position,
                items = new List<Server_Trashcan_Item>(),
                maxSize = (float)maxSize
            };

            using (var db = new gtaContext())
            {
                db.Server_Trashcans.Add(can);
                db.SaveChanges();
            }
            ServerTrashcans_.Add(can);
            HUDHandler.SendBetterNotif(player, 1, 10, "Admin", "Mülltonne erstellt.");
        }

        public static void Load()
        {
            using (var db = new gtaContext())
            {
                ServerTrashcans_ = new List<Server_Trashcans>(db.Server_Trashcans);
                Alt.Log($"[SERVER] {ServerTrashcans_.Count} Mülltonnen geladen");
            }

            foreach (Server_Trashcans can in ServerTrashcans_)
            {
                EntityStreamer.MarkerStreamer.Create(EntityStreamer.MarkerTypes.MarkerTypeVerticalCylinder, new System.Numerics.Vector3(can.pos.X, can.pos.Y, can.pos.Z - 1), new System.Numerics.Vector3(1), color: new Rgba(150, 50, 50, 150));
            }
        }

        [AsyncClientEvent("Server:Trashcan:switchItemToTrashcan")]
        public void switchItemToTrashcan(ClassicPlayer player, int trashcanId, string itemName, int itemAmount, string fromContainer)
        {
            try
            {
                // Inventory -> Trashcan
                if (player == null || !player.Exists || player.CharacterId <= 0 || itemName.Length <= 0 || itemAmount <= 0 || trashcanId <= 0) return;
                if (itemAmount > CharactersInventory.GetCharacterItemAmount(player.CharacterId, itemName, fromContainer))
                {
                    HUDHandler.SendBetterNotif(player, 3, 10, "Mülltonne", "Soviele Gegenstände hast du nicht dabei.");
                    return;
                }

                float itemWeight = ServerItems.GetItemWeight(itemName) * itemAmount;
                if (GetWeight(trashcanId) + itemWeight > GetMaxSize(trashcanId))
                {
                    HUDHandler.SendBetterNotif(player, 3, 10, "Mülltonne", $"Soviel Platz hat diese Mülltonne nicht (maximal: {GetMaxSize(trashcanId)}kg).");
                    return;
                }

                AddItem(trashcanId, itemName, itemAmount);
                CharactersInventory.RemoveCharacterItemAmount(player.CharacterId, itemName, itemAmount, fromContainer);
                HUDHandler.SendBetterNotif(player, 2, 10, "Mülltonne", $"Du hast {itemAmount}x {itemName} in die Mülltonne gelegt.");
            }
            catch (Exception e)
            {
                Alt.Log(e.ToString());
            }
        }

        [AsyncClientEvent("Server:Trashcan:switchItemToInventory")]
        public void switchItemToInventory(ClassicPlayer player, int trashcanId, string itemName, int itemAmount)
        {
            try
            {
                // Trashcan -> Inventory
                if (player == null || !player.Exists || player.CharacterId <= 0 || itemName.Length <= 0 || itemAmount <= 0 || trashcanId <= 0) return;
                if (itemAmount > GetItemAmount(trashcanId, itemName))
                {
                    HUDHandler.SendBetterNotif(player, 3, 10, "Mülltonne", "Soviele Gegenstände sind nicht in der Mülltonne.");
                    return;
                }
                float itemWeight = ServerItems.GetItemWeight(itemName) * itemAmount;
                if (CharactersInventory.GetCharacterItemWeight(player.CharacterId, "inventory") + itemWeight > 15f)
                {
                    HUDHandler.SendBetterNotif(player, 3, 10, "Mülltonne", "Fehler: Du hast nicht genügend Platz für diese Gegenstände.");
                    return;
                }

                RemoveItemAmount(trashcanId, itemName, itemAmount);
                CharactersInventory.AddCharacterItem(player.CharacterId, itemName, itemAmount, "inventory");
                HUDHandler.SendBetterNotif(player, 2, 10, "Mülltonne", $"Du hast {itemAmount}x {itemName} aus der Mülltonne entnommen.");
            }
            catch (Exception e)
            {
                Alt.Log(e.ToString());
            }
        }

        #region Functions
        public static void RemoveItemAmount(int trashcanId, string itemName, int itemAmount)
        {
            try
            {
                if (trashcanId <= 0 || itemName.Length <= 0 || itemAmount <= 0) return;
                Server_Trashcans can = ServerTrashcans_.FirstOrDefault(x => x.id == trashcanId);
                if (can == null) return;
                Server_Trashcan_Item item = can.items.FirstOrDefault(x => x.itemName == itemName);
                if (item == null) return;
                item.itemAmount -= itemAmount;
                if (item.itemAmount <= 0)
                    can.items.Remove(item);

                using (var db = new gtaContext())
                {
                    db.Server_Trashcans.Update(can);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static void AddItem(int trashcanId, string itemName, int itemAmount)
        {
            try
            {
                if (trashcanId <= 0 || itemName.Length <= 0 || itemAmount <= 0) return;
                Server_Trashcans can = ServerTrashcans_.FirstOrDefault(x => x.id == trashcanId);
                if (can == null || can.items == null) return;
                Server_Trashcan_Item item = can.items.FirstOrDefault(x => x.itemName == itemName);
                if (item == null)
                {
                    // Existiert nicht, hinzufügen.
                    can.items.Add(new Server_Trashcan_Item
                    {
                        itemName = itemName,
                        itemAmount = itemAmount
                    });
                }
                else item.itemAmount += itemAmount;

                using (var db = new gtaContext())
                {
                    db.Server_Trashcans.Update(can);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static float GetWeight(int trashcanId)
        {
            float weight = 0f;
            Server_Trashcans can = ServerTrashcans_.ToList().FirstOrDefault(x => x.id == trashcanId);
            if (can == null || can.items == null) return weight;
            foreach (var item in can.items)
                weight += ServerItems.GetItemWeight(item.itemName) * item.itemAmount;

            return weight;
        }

        public static int GetItemAmount(int trashcanId, string itemName)
        {
            int amount = 0;
            Server_Trashcans can = ServerTrashcans_.ToList().FirstOrDefault(x => x.id == trashcanId);
            if (can != null && can.items != null)
            {
                Server_Trashcan_Item item = can.items.ToList().FirstOrDefault(x => x.itemName == itemName);
                if (item != null) amount = item.itemAmount;
            }
            return amount;
        }

        public static float GetMaxSize(int trashcanId)
        {
            float maxSize = 0f;
            Server_Trashcans can = ServerTrashcans_.ToList().FirstOrDefault(x => x.id == trashcanId);
            if (can != null) maxSize = can.maxSize;
            return maxSize;
        }
        #endregion
    }
}
