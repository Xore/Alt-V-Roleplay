using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Handler;
using Altv_Roleplay.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Altv_Roleplay.Utils;
using System.Threading;

namespace Altv_Roleplay.Utils
{
    class HelperEvents : IScript
    {
        [AsyncClientEvent("Server:CEF:setCefStatus")]
        public async static Task ClientEvent_setCefStatus(IPlayer player, bool state)
        {
            try
            {
                if (player == null || !player.Exists) return;
                await AltAsync.Do(() =>
                {
                    player.SetSyncedMetaData("IsCefOpen", state);
                });
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Player:setPos")]
        public async Task ClientEvent_SetPos(IPlayer player, float X, float Y, float Z)
        {
            try
            {
                if (player == null || !player.Exists) return;
                player.Position = new Position(X, Y, Z);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void TriggerClientEvent(IPlayer player, string eventName, params object[] args)
        {
            try
            {
                if (player == null) return;
                if (Thread.CurrentThread.ManagedThreadId != Main.mainThreadId) player.EmitLocked(eventName, args);
                else player.Emit(eventName, args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
