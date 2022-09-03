using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Handler;
using Altv_Roleplay.models;
using Altv_Roleplay.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace Altv_Roleplay.Model
{
    class ServerDroppedItems
    {
        public static List<Server_Dropped_Items> ServerDroppedItems_ = new List<Server_Dropped_Items>();

        public static void AddItem(string itemName, int amount, Position pos, DateTime droppedTime, int dimension)
        {
            try
            {

                Server_Dropped_Items item = new Server_Dropped_Items
                {
                    itemName = itemName,
                    itemAmount = amount,
                    pos = pos,
                    droppedTimestamp = droppedTime,
                    dimension = dimension,
                    prop = EntityStreamer.PropStreamer.Create("prop_cs_heist_bag_02", pos, new System.Numerics.Vector3(0), dimension: dimension, frozen: true, streamRange: 100),
                    textLabel = EntityStreamer.TextLabelStreamer.Create($"'{amount}x {itemName}'", pos, dimension, streamRange: 3),
                };

                ServerDroppedItems_.Add(item);
                using (gtaContext db = new gtaContext())
                {
                    db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    db.Server_Dropped_Items.Add(item);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void RemoveItem(Server_Dropped_Items item)
        {
            try
            {
                if (item == null) return;
                if (item.prop != null)
                    EntityStreamer.PropStreamer.Delete(item.prop);

                if (item.textLabel != null)
                    EntityStreamer.TextLabelStreamer.DestroyDynamicTextLabel(item.textLabel);

                ServerDroppedItems_.Remove(item);
                using (gtaContext db = new gtaContext())
                {
                    db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    db.Server_Dropped_Items.Remove(item);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public static void ManipulateItem(Server_Dropped_Items item, int amount)
        {
            if (item == null) return;
            try
            {
                using (gtaContext db = new gtaContext())
                {
                    db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    db.Server_Dropped_Items.Find(item).itemAmount = item.itemAmount - amount;
                    db.Server_Dropped_Items.Find(item).textLabel = EntityStreamer.TextLabelStreamer.Create($"'{item.itemAmount - amount}x {item.itemName}'", item.pos, item.dimension, streamRange: 3);
                    db.SaveChanges();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        internal static void takeItem(ClassicPlayer player, Server_Dropped_Items droppedItem)
        {
            if (droppedItem == null || !ServerDroppedItems_.Exists(x => x == droppedItem)) return;
            int charId = User.GetPlayerOnline(player);
            if (charId <= 0) return;


            float totalWeight = ServerItems.GetItemWeight(droppedItem.itemName) * droppedItem.itemAmount;
            float singleWeight = ServerItems.GetItemWeight(droppedItem.itemName);
            float invWeight = CharactersInventory.GetCharacterItemWeight(charId, "inventory");
            float backpackWeight = CharactersInventory.GetCharacterItemWeight(charId, "backpack");
            float backpackSize = Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId));
            int canTakeInv = 0;
            int canTakeBack = 0;
            int tmpOnGround = droppedItem.itemAmount;

            if (droppedItem.itemName.StartsWith("Bargeld"))
            {
                RemoveItem(droppedItem);
                HUDHandler.SendNotification(player, 1, 2500, $"Du hast {droppedItem.itemAmount}x {droppedItem.itemName} aufgehoben.");
                CharactersInventory.AddCharacterItem(charId, droppedItem.itemName, droppedItem.itemAmount, "brieftasche");
                return;
            }
            else if (droppedItem.itemName.StartsWith("Ausweis "))
            {
                RemoveItem(droppedItem);
                HUDHandler.SendNotification(player, 1, 2500, $"Du hast {droppedItem.itemAmount}x {droppedItem.itemName} aufgehoben.");
                CharactersInventory.AddCharacterItem(charId, droppedItem.itemName, droppedItem.itemAmount, "brieftasche");
                return;
            }
            else if (droppedItem.itemName.StartsWith("EC-Karte "))
            {
                RemoveItem(droppedItem);
                HUDHandler.SendNotification(player, 1, 2500, $"Du hast {droppedItem.itemAmount}x {droppedItem.itemName} aufgehoben.");
                CharactersInventory.AddCharacterItem(charId, droppedItem.itemName, droppedItem.itemAmount, "brieftasche");
                return;
            }
            else if (droppedItem.itemName.Contains("schluessel"))
            {
                RemoveItem(droppedItem);
                HUDHandler.SendNotification(player, 1, 2500, $"Du hast {droppedItem.itemAmount}x {droppedItem.itemName} aufgehoben.");
                CharactersInventory.AddCharacterItem(charId, droppedItem.itemName, droppedItem.itemAmount, "schluessel");
                return;
            }
            else if (invWeight + totalWeight <= 15f)
            {
                RemoveItem(droppedItem);
                HUDHandler.SendNotification(player, 1, 2500, $"Du hast {droppedItem.itemAmount}x {droppedItem.itemName} aufgehoben.");
                CharactersInventory.AddCharacterItem(charId, droppedItem.itemName, droppedItem.itemAmount, "inventory");
                return;
            }

            if (Characters.GetCharacterBackpack(charId) != -2 && backpackWeight + totalWeight <= Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId)))
            {
                RemoveItem(droppedItem);
                HUDHandler.SendNotification(player, 1, 2500, $"Du hast {droppedItem.itemAmount}x {droppedItem.itemName} aufgehoben.");
                CharactersInventory.AddCharacterItem(charId, droppedItem.itemName, droppedItem.itemAmount, "backpack");
                return;
            }


            canTakeInv = (int)((15f - invWeight) / singleWeight);
            canTakeBack = (int)((backpackSize - backpackWeight) / singleWeight);
            int summed = 0;
            bool invTrue = false;
            bool backTrue = false;
            if (canTakeInv < droppedItem.itemAmount)
            {
                invTrue = true;
                tmpOnGround -= canTakeInv;
                summed += canTakeInv;
            }
            if (canTakeBack < droppedItem.itemAmount)
            {
                backTrue = true;
                tmpOnGround -= canTakeBack;
                summed += canTakeBack;
            }

            if (droppedItem.itemAmount < canTakeBack)
            {
                backTrue = true;
                summed += tmpOnGround;
                tmpOnGround = 0;
            }

            if (canTakeInv > 0 && tmpOnGround == 1 || canTakeInv > 0 && droppedItem.itemAmount == 1)
            {
                RemoveItem(droppedItem);
                HUDHandler.SendNotification(player, 1, 2500, $"Du hast 1x {droppedItem.itemName} aufgehoben.");
                CharactersInventory.AddCharacterItem(charId, droppedItem.itemName, 1, "inventory");
                return;
            }
            else if (canTakeBack > 0 && tmpOnGround == 1 || canTakeBack > 0 && droppedItem.itemAmount == 1)
            {
                RemoveItem(droppedItem);
                HUDHandler.SendNotification(player, 1, 2500, $"Du hast 1x {droppedItem.itemName} aufgehoben.");
                CharactersInventory.AddCharacterItem(charId, droppedItem.itemName, 1, "backpack");
                return;
            }
            else if (canTakeBack == 0 && canTakeInv == 0)
            {
                float tmp = backpackWeight;
                tmp += singleWeight;
                if (tmp == backpackSize)
                {
                    RemoveItem(droppedItem);
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast 1x {droppedItem.itemName} aufgehoben.");
                    CharactersInventory.AddCharacterItem(charId, droppedItem.itemName, 1, "backpack");
                    return;
                }

                tmp = invWeight;
                tmp += singleWeight;
                if (tmp <= 15.000001f)
                {
                    RemoveItem(droppedItem);
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast 1x {droppedItem.itemName} aufgehoben.");
                    CharactersInventory.AddCharacterItem(charId, droppedItem.itemName, 1, "inventory");
                    return;
                }


                HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht genügend Platz in deinen Taschen.");
                return;
            }
            else if (canTakeBack < 0)
            {
                HUDHandler.SendNotification(player, 3, 2500, $"Du hast scheinbar zu viel in deinem Rucksack.");
                Console.WriteLine($"Der Character mit der ID {player.CharacterId} und der Account ID {player.accountId} hat zu viel im Rucksack");
                return;
            }
            else if (canTakeInv < 0)
            {
                HUDHandler.SendNotification(player, 3, 2500, $"Du hast scheinbar zu viel in deinem Inventar.");
                Console.WriteLine($"Der Character mit der ID {player.CharacterId} und der Account ID {player.accountId} hat zu viel im Inventar");
                return;
            }
            else if (tmpOnGround <= 0)
            {
                RemoveItem(droppedItem);
                return;
            }
            else
            {
                RemoveItem(droppedItem);

                if (tmpOnGround > 0)
                    AddItem(droppedItem.itemName, tmpOnGround, droppedItem.pos, DateTime.Now, droppedItem.dimension);

                HUDHandler.SendNotification(player, 1, 2500, $"Du hast {summed}x {droppedItem.itemName} aufgehoben.");
                if (invTrue)
                    CharactersInventory.AddCharacterItem(charId, droppedItem.itemName, canTakeInv, "inventory");

                if (backTrue)
                    CharactersInventory.AddCharacterItem(charId, droppedItem.itemName, canTakeBack, "backpack");

            }
        }
    }
}