using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.EntitySync;
using AltV.Net.EntitySync.ServerEvent;
using AltV.Net.EntitySync.SpatialPartitions;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.models;
using Altv_Roleplay.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Timers;

namespace Altv_Roleplay.Handler
{
    public partial class WeedPot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int remainingMinutes { get; set; }
        public int dimension { get; set; }
        public int water { get; set; }
        public bool hasFertilizer { get; set; }
        public Position position { get; set; }
        public int state { get; set; } // 1 = Benötigt Wasser, 2 = Wächst, 3 = Erntebereit, 4 = tot.

        [NotMapped]
        public EntityStreamer.Prop prop { get; set; } = null;

        [NotMapped]
        public EntityStreamer.PlayerLabel textLabel { get; set; } = null;
    }

    class WeedPlantHandler : IScript
    {
        public static List<WeedPot> WeedPots_ = new List<WeedPot>();
        public static readonly string smallObject = "bkr_prop_weed_01_small_01a";
        public static readonly string midObject = "bkr_prop_weed_med_01a";
        public static readonly string bigObject = "bkr_prop_weed_lrg_01a";
        public static void LoadAllWeedPots()
        {
            using (var db = new gtaContext())
            {
                WeedPots_ = new List<WeedPot>(db.WeedPot);
                Alt.Log($"{WeedPots_.Count} Weedpots geladen..");
                AltEntitySync.Init(7, (threadId) => 200, (threadId) => false,
                (threadCount, repository) => new ServerEventNetworkLayer(threadCount, repository),
                (entity, threadCount) => entity.Type,
                (entityId, entityType, threadCount) => entityType,
                (threadId) =>
                {
                    return threadId switch
                    {
                        // Marker
                        0 => new LimitedGrid3(50_000, 50_000, 75, 10_000, 10_000, 64),
                        // Text
                        1 => new LimitedGrid3(50_000, 50_000, 75, 10_000, 10_000, 32),
                        // Props
                        2 => new LimitedGrid3(50_000, 50_000, 100, 10_000, 10_000, 1500),
                        // Help Text
                        3 => new LimitedGrid3(50_000, 50_000, 100, 10_000, 10_000, 1),
                        // Blips
                        4 => new EntityStreamer.GlobalEntity(),
                        // Dynamic Blip
                        5 => new LimitedGrid3(50_000, 50_000, 175, 10_000, 10_000, 200),
                        // Ped
                        6 => new LimitedGrid3(50_000, 50_000, 175, 10_000, 10_000, 64),
                        _ => new LimitedGrid3(50_000, 50_000, 175, 10_000, 10_000, 115),
                    };
                },
            new IdProvider());
            }

            foreach (WeedPot pot in WeedPots_)
            {
                var potposi = new Position(pot.position.X, pot.position.Y, pot.position.Z);
                switch (pot.state)
                {
                    /*case 4:
                        pot.prop = EntityStreamer.PropStreamer.Create(smallObject, pot.position, new Position(), pot.dimension, frozen: true);
                        pot.textLabel = EntityStreamer.TextLabelStreamer.Create($"Weed Pflanze\nStatus: zerstört.\nDrücke Z zum entfernen.", new Position(pot.position.X, pot.position.Y, pot.position.Z + 1), pot.dimension, streamRange: 5);
                        break;*/
                    case 3:
                        pot.prop = EntityStreamer.PropStreamer.Create(bigObject, new Position(pot.position.X, pot.position.Y, pot.position.Z - 2.5f), new Position(), pot.dimension, frozen: true);
                        pot.textLabel = EntityStreamer.TextLabelStreamer.Create($"Weed Pflanze\nStatus: erntebereit.\nDrücke E zum ernten.", new Position(pot.position.X, pot.position.Y, pot.position.Z + 1), pot.dimension, streamRange: 3 / 2);
                        EntityStreamer.MarkerStreamer.Create(EntityStreamer.MarkerTypes.MarkerTypeHorizontalCircleSkinny, potposi, new System.Numerics.Vector3(1), color: new Rgba(255, 51, 51, 100), dimension: pot.dimension, streamRange: 3 / 2);
                        break;
                    case 2:
                        pot.prop = EntityStreamer.PropStreamer.Create(midObject, new Position(pot.position.X, pot.position.Y, pot.position.Z - 2.5f), new Position(), pot.dimension, frozen: true);
                        pot.textLabel = EntityStreamer.TextLabelStreamer.Create($"Weed Pflanze\nStatus: Wachstum.\nDünger: Ja\nWachsdauer: {pot.remainingMinutes}", new Position(pot.position.X, pot.position.Y, pot.position.Z + 1), pot.dimension, streamRange: 3 / 2);
                        EntityStreamer.MarkerStreamer.Create(EntityStreamer.MarkerTypes.MarkerTypeHorizontalCircleSkinny, potposi, new System.Numerics.Vector3(1), color: new Rgba(255, 51, 51, 100), dimension: pot.dimension, streamRange: 3 / 2);
                        break;
                    case 1:
                        pot.prop = EntityStreamer.PropStreamer.Create(smallObject, pot.position, new Position(), pot.dimension, frozen: true);
                        pot.textLabel = EntityStreamer.TextLabelStreamer.Create($"Weed Pflanze\nStatus: benötigt Wasser.", new Position(pot.position.X, pot.position.Y, pot.position.Z + 1), pot.dimension, streamRange: 3 / 2);
                        EntityStreamer.MarkerStreamer.Create(EntityStreamer.MarkerTypes.MarkerTypeHorizontalCircleSkinny, potposi, new System.Numerics.Vector3(1), color: new Rgba(255, 51, 51, 100), dimension: pot.dimension, streamRange: 3 / 2);
                        break;
                }
            }

            System.Timers.Timer weedPotTimer = new System.Timers.Timer();
            weedPotTimer.Interval = 10000;
            weedPotTimer.Elapsed += WeedPotTimerElapsed;
            weedPotTimer.Enabled = true;
        }

        internal static void harvestPot(IPlayer player, WeedPot weedPot)
        {
            if (player == null || !player.Exists || weedPot == null) return;
            int charId = User.GetPlayerOnline(player);
            if (charId <= 0) return;
            int amount = new Random().Next(10, 35);
            if (weedPot.hasFertilizer) amount = new Random().Next(15, 45);
            weedPot.prop.Delete();
            weedPot.textLabel.Delete();
            WeedPots_.Remove(weedPot);

            player.EmitLocked("Client:Inventory:PlayAnimation", "amb@world_human_gardener_plant@male@idle_a", "idle_a", 2000, 15, false);

            /*HUDHandler.SendNotification(player, $"Du hast die Pflanze geerntet und {amount} Knollen erhalten.", "success", 2500);
            CharactersInventory.AddCharacterItem(charId, "Weed", amount, "inventory");
            CharactersInventory.AddCharacterItem(charId, "Blumentopf", 1, "inventory");*/

            float weedWeight = ServerItems.GetItemWeight("Weed") * amount;
            float potWeight = ServerItems.GetItemWeight("Blumentopf");
            float invWeight = CharactersInventory.GetCharacterItemWeight(charId, "inventory");
            float backpackWeight = CharactersInventory.GetCharacterItemWeight(charId, "backpack");
            if (invWeight + weedWeight + potWeight > 15.02f && backpackWeight + weedWeight + potWeight > Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId)))
            {
                HUDHandler.SendNotification(player, 3, 2500, $"Deine Taschen sind voll.");
                return;
            }

            if (invWeight + weedWeight + potWeight <= 15.02f)
            {
                HUDHandler.SendNotification(player, 2, 2500, $"Du hast die Pflanze geerntet und {amount} Knollen erhalten.");
                CharactersInventory.AddCharacterItem(charId, "Weed", amount, "inventory");
                CharactersInventory.AddCharacterItem(charId, "Blumentopf", 1, "inventory");
            }
            else
            {

                if (Characters.GetCharacterBackpack(charId) != -2 && backpackWeight + weedWeight + potWeight <= Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId)))
                {
                    HUDHandler.SendNotification(player, 2, 2500, $"Du hast die Pflanze geerntet und {amount} Knollen erhalten.");
                    CharactersInventory.AddCharacterItem(charId, "Weed", amount, "inventory");
                    CharactersInventory.AddCharacterItem(charId, "Blumentopf", 1, "inventory");
                }
                else
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Es ist ein Fehler aufgetreten.");
                    return;
                }

            }
            using (var db = new gtaContext())
            {
                db.WeedPot.Remove(weedPot);
                db.SaveChanges();
            }
        }

        internal static void removePot(IPlayer player, WeedPot weedPot)
        {
            if (player == null || !player.Exists || weedPot == null) return;
            int charId = User.GetPlayerOnline(player);
            if (charId <= 0) return;
            weedPot.prop.Delete();
            weedPot.textLabel.Delete();
            WeedPots_.Remove(weedPot);
            //CharactersInventory.AddCharacterItem(charId, "Blumentopf", 1, "inventory");
            //HUDHandler.SendNotification(player, $"Du hast die Pflanze entfernt.", "info", 2500);
            using (var db = new gtaContext())
            {
                db.WeedPot.Remove(weedPot);
                db.SaveChanges();
            }
        }

        private static void WeedPotTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                foreach (WeedPot pot in WeedPots_.Where(x => x.state == 2 && x.remainingMinutes > 0))
                {
                    lock (pot)
                    {
                        //pot.water -= 2;
                        pot.remainingMinutes -= 1;
                        UpdatePotLabelAndObject(pot);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void UpdatePotLabelAndObject(WeedPot pot)
        {
            if (pot == null) return;
            lock (pot)
            {
                /*if (pot.water >= 100 || pot.water < 0)
                {
                    pot.state = 4;
                    pot.prop.Delete();
                    pot.prop = EntityStreamer.PropStreamer.Create(smallObject, pot.position, new Position(), pot.dimension, frozen: true);
                    pot.textLabel.SetText($"Weed Pflanze\nStatus: zerstört.\nDrücke Z zum entfernen.");
                    return;
                }*/

                if (pot.remainingMinutes <= 0)
                {
                    pot.state = 3;
                    pot.prop.Delete();
                    pot.prop = EntityStreamer.PropStreamer.Create(bigObject, new Position(pot.position.X, pot.position.Y, pot.position.Z - 2.5f), new Position(), pot.dimension, frozen: true);
                    pot.textLabel.SetText($"Weed Pflanze\nStatus: erntebereit.\nDrücke E zum ernten.");
                    return;
                }

                string hasFertilizer = pot.hasFertilizer == true ? "Ja" : "Nein";
                pot.textLabel.SetText($"Weed Pflanze\nStatus: Wachstum.\nDünger: {hasFertilizer}\nWachsdauer: {pot.remainingMinutes}");
            }

            using (var db = new gtaContext())
            {
                db.WeedPot.Update(pot);
                db.SaveChanges();
            }
        }

        public static void PlaceNewWeedpot(ClassicPlayer player)
        {
            if (player == null || !player.Exists) return;
            WeedPot pot = new WeedPot
            {
                dimension = player.Dimension,
                position = new Position(player.Position.X, player.Position.Y, player.Position.Z - 1),
                remainingMinutes = 120,
                state = 1,
                water = 0,
                hasFertilizer = false,
                prop = EntityStreamer.PropStreamer.Create(smallObject, new Position(player.Position.X, player.Position.Y, player.Position.Z - 1), new Position(), player.Dimension, frozen: true),
                textLabel = EntityStreamer.TextLabelStreamer.Create($"Weed Pflanze\nStatus: benötigt Wasser.", new Position(player.Position.X, player.Position.Y, player.Position.Z), player.Dimension, streamRange: 3 / 2),
            };

            WeedPots_.Add(pot);
            using (var db = new gtaContext())
            {
                db.WeedPot.Add(pot);
                db.SaveChanges();
            }
            HUDHandler.SendNotification(player, 1, 2500, "Du hast eine Weedpflanze angepflanzt. Gieße diese mit Wasser, damit der Wachsvorgang beginnt.");
        }

        public static void fertilizeNearestPot(ClassicPlayer player)
        {
            if (player == null || !player.Exists) return;
            WeedPot pot = WeedPots_.FirstOrDefault(x => player.Position.IsInRange(x.position, 1.5f));
            if (pot == null || pot.state != 2 || pot.hasFertilizer || pot.remainingMinutes < 30) return;
            pot.hasFertilizer = true;
            pot.remainingMinutes -= 15;
            string hasFertilizer = pot.hasFertilizer == true ? "Ja" : "Nein";
            pot.textLabel.SetText($"Weed Pflanze\nStatus: Wachstum.\nDünger: {hasFertilizer}\nWachsdauer: {pot.remainingMinutes}");
        }


        public static void fillNearestPotWithWater(ClassicPlayer player)
        {
            if (player == null || !player.Exists) return;
            int charId = User.GetPlayerOnline(player);
            WeedPot pot = WeedPots_.FirstOrDefault(x => player.Position.IsInRange(x.position, 1.5f));
            if (pot == null || pot.state == 3 || pot.state == 4) return;

            if (pot.water > 0)
            {
                HUDHandler.SendNotification(player, 3, 2500, "Die Pflanze wurde bereits Gewässter.");
                return;
            }

            string hasFertilizer = pot.hasFertilizer == true ? "Ja" : "Nein";

            lock (pot)
            {
                pot.water += 20;
                /*if (pot.water > 100)
                {
                    pot.state = 4;
                    pot.prop.Delete();
                    pot.prop = EntityStreamer.PropStreamer.Create(smallObject, pot.position, new Position(), pot.dimension, frozen: true);
                    pot.textLabel.SetText($"Weed Pflanze\nStatus: zerstört.\nDrücke Z zum entfernen.");
                }
                else*/
                if (pot.water <= 100 && pot.state == 1)
                {
                    pot.state = 2;
                    pot.prop.Delete();
                    pot.prop = EntityStreamer.PropStreamer.Create(midObject, new Position(pot.position.X, pot.position.Y, pot.position.Z - 2.5f), new Position(), pot.dimension, frozen: true);
                    pot.textLabel.SetText($"Weed Pflanze\nStatus: Wachstum.\nDünger: {hasFertilizer}\nWachsdauer: {pot.remainingMinutes}");
                    HUDHandler.SendNotification(player, 1, 2500, "Die Pflanze fängst nun an zu wachsen. Mit Dünger kannst du die Wachstumsdauer verkürzen.");
                    return;
                }
                else if (pot.water <= 100 && pot.state == 2)
                {
                    pot.textLabel.SetText($"Weed Pflanze\nStatus: Wachstum.\nDünger: {hasFertilizer}\nWachsdauer: {pot.remainingMinutes}");
                    return;
                }
            }
            using (var db = new gtaContext())
            {
                db.WeedPot.Update(pot);
                db.SaveChanges();
            }
        }
    }
}
