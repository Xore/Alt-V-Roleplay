using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.EntitySync;
using AltV.Net.EntitySync.ServerEvent;
using AltV.Net.EntitySync.SpatialPartitions;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Handler;
using Altv_Roleplay.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Altv_Roleplay.Model
{
    class ServerObjects : IScript
    {
        public static List<Server_Objects> ServerObjects_ = new List<Server_Objects>();
        public static void LoadAllObjects()
        {
            using (var db = new gtaContext())
            {
                ServerObjects_ = new List<Server_Objects>(db.Server_Objects);
                Alt.Log($"[SERVER] {ServerObjects_.Count} Objekte geladen");
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

            foreach (Server_Objects item in ServerObjects_)
            {
                item.prop = EntityStreamer.PropStreamer.Create(item.itemHash, item.pos, new System.Numerics.Vector3(item.rotX, item.rotY, item.rotZ), frozen: true, placeObjectOnGroundProperly: true);
            }
        }

        [AsyncClientEvent("Server:Objects:PlaceItem")]
        public async Task placeItemAsync(ClassicPlayer player, string itemHash, Position pos, Rotation rot)
        {
            if (player == null || !player.Exists) return;

            var item = new Server_Objects
            {
                itemHash = itemHash,
                pos = pos,
                rotX = rot.Pitch,
                rotY = rot.Roll,
                rotZ = rot.Yaw,
            };
            ServerObjects_.Add(item);
            using (gtaContext db = new gtaContext())
            {
                db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                db.Server_Objects.Add(item);
                db.SaveChanges();
            }

            player.EmitLocked("Client:Inventory:PlayAnimation", "amb@prop_human_bum_bin@base", "base", 10000, 1, false);
            HUDHandler.SendProgress(player, " ", "alert", 10000);
            await Task.Delay(10000);
            item.prop = EntityStreamer.PropStreamer.Create(itemHash, pos, new System.Numerics.Vector3(0, 0, rot.Yaw), player.Dimension, frozen: true, placeObjectOnGroundProperly: true);
            HUDHandler.SendBetterNotif(player, 2, 10, "Objekt", $"Objekt Platziert");
        }

        internal static void removeItem(IPlayer player, Server_Objects item)
        {
            if (player == null || !player.Exists || item == null) return;
            int charId = User.GetPlayerOnline(player);
            if (charId <= 0) return;
            item.prop.Delete();
            ServerObjects_.Remove(item);

            player.EmitLocked("Client:Inventory:PlayAnimation", "amb@world_human_gardener_plant@male@idle_a", "idle_a", 2000, 15, false);
            using (var db = new gtaContext())
            {
                db.Server_Objects.Remove(item);
                db.SaveChanges();
            }
        }
    }
}