using AltV.Net;
using AltV.Net.Data;
using AltV.Net.EntitySync;
using AltV.Net.EntitySync.ServerEvent;
using AltV.Net.EntitySync.SpatialPartitions;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Timers;

namespace Altv_Roleplay.Handler
{
    public partial class Object
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int remainingMinutes { get; set; }
        public int dimension { get; set; }
        public Position position { get; set; }

        [NotMapped]
        public EntityStreamer.Prop prop { get; set; } = null;

        [NotMapped]
        public EntityStreamer.PlayerLabel textLabel { get; set; } = null;
    }

    class ObjectHandler : IScript
    {
        public static List<Object> Objects_ = new List<Object>();
        public static readonly string Object1 = "prop_barrier_work05";
        public static void LoadAllObjects()
        {
            using (var db = new gtaContext())
            {
                Objects_ = new List<Object>(db.Objects);
                Alt.Log($"{Objects_.Count} Objects geladen..");
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

            //System.Timers.Timer ObjectTimer = new System.Timers.Timer();
            //ObjectTimer.Interval = 10000;
            //ObjectTimer.Elapsed += ObjectTimerElapsed;
            //ObjectTimer.Enabled = true;
        }

        /*internal static void removeItem(IPlayer player, Object Object)
        {
            if (player == null || !player.Exists || Object == null) return;
            int charId = User.GetPlayerOnline(player);
            if (charId <= 0) return;
            Object.prop.Delete();
            Object.textLabel.Delete();
            Objects_.Remove(Object);
            using (var db = new gtaContext())
            {
                db.Objects.Remove(Object);
                db.SaveChanges();
            }
        }*/

        internal static void removeItem(ClassicPlayer player, Object Object)
        {
            if (player == null || !player.Exists || Object == null) return;
            int charId = User.GetPlayerOnline(player);
            if (charId <= 0) return;
            Object.prop.Delete();
            Object.textLabel.Delete();

            using (var db = new gtaContext())
            {
                var dbObject = db.Objects.Find(Object);
                if (dbObject == null) return;
                db.Objects.Remove(dbObject);
                db.SaveChanges();
            }
            Objects_.Remove(Object);
        }

        private static void ObjectTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                foreach (Object item in Objects_.Where(x => x.remainingMinutes > 0))
                {
                    using (gtaContext db = new gtaContext())
                    {
                        db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                        var dbItem = db.Objects.Find(item);
                        if (dbItem == null) return;
                        db.Objects.Remove(dbItem);
                        db.SaveChanges();
                    }
                    Objects_.Remove(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void PlaceNewBarrier(ClassicPlayer player)
        {
            if (player == null || !player.Exists) return;
            Object item = new Object
            {
                dimension = player.Dimension,
                position = new Position(player.Position.X, player.Position.Y, player.Position.Z - 1),
                remainingMinutes = 15,
                prop = EntityStreamer.PropStreamer.Create(Object1, new Position(player.Position.X, player.Position.Y, player.Position.Z - 1), new Position(), player.Dimension, frozen: true),
            };

            Objects_.Add(item);
            using (var db = new gtaContext())
            {
                db.Objects.Add(item);
                db.SaveChanges();
            }
            HUDHandler.SendNotification(player, 1, 2500, "Du hast etwas Platziert, Drücke 'Z' um dieses Objekt wieder zu Entfernen");
        }
    }
}
