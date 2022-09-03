using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Altv_Roleplay.Handler
{
    class FishingHandler : IScript
    {
        public static List<sandyFishPosition> sandyFishPositions = new List<sandyFishPosition>
        {
            new sandyFishPosition { position = new Position(713.5121f, 4092.6594f, 34.72229f)},
            new sandyFishPosition { position = new Position(1299.1648f, 4215.9033f, 33.89673f)},
            new sandyFishPosition { position = new Position(1340.3473f, 4225.319f, 33.913574f)},
            new sandyFishPosition { position = new Position(1999.6879f, 3958.4966f, 31.672485f)},
            new sandyFishPosition { position = new Position(1732.8264f, 3985.6616f, 31.97583f)},
            new sandyFishPosition { position = new Position(902.0967f, 3703.2131f, 31.352417f)},
            new sandyFishPosition { position = new Position(369.03296f, 3637.5957f, 31.807373f)},
            new sandyFishPosition { position = new Position(-118.57582f, 3819.9297f, 31.21753f)},
            new sandyFishPosition { position = new Position(-146.08351f, 3902.967f, 31.402954f)},
            new sandyFishPosition { position = new Position(3373.2659f, 5183.486f, 1.4439697f)},
            new sandyFishPosition { position = new Position(3223.4504f, 5332.299f, 2.050537f)},
        };
        public partial class sandyFishPosition
        {
            public Position position { get; set; }
        }
        public static List<paletoFishPosition> paletoFishPositions = new List<paletoFishPosition>
        {
            new paletoFishPosition { position = new Position(1554.3561f, 6661.345f, 1.5281982f)},
            new paletoFishPosition { position = new Position(1496.7296f, 6634.7734f, 2.0168457f)},
            new paletoFishPosition { position = new Position(1257.2968f, 6619.464f, 1.258667f)},
            new paletoFishPosition { position = new Position(51.573627f, 7254.6196f, 1.2248535f)},
            new paletoFishPosition { position = new Position(-276.93628f, 6639.4023f, 7.543579f)},
            new paletoFishPosition { position = new Position(-374.98022f, 6519.02f, 1.4776611f)},
            new paletoFishPosition { position = new Position(-1612.5758f, 5262.5933f, 3.9714355f)},
            new paletoFishPosition { position = new Position(-1612.5758f, 5262.5933f, 3.9714355f)},
        };
        public partial class paletoFishPosition
        {
            public Position position { get; set; }
        }
        public static List<lsFishPosition> lsFishPositions = new List<lsFishPosition>
        {
            new lsFishPosition { position = new Position(-3290.1362f, 1126.2197f, 0.90478516f)},
            new lsFishPosition { position = new Position(-3428.3735f, 964.95825f, 8.335571f)},
            new lsFishPosition { position = new Position(-3058.1934f, 10.140659f, 0.7363281f)},
            new lsFishPosition { position = new Position(-1851.033f, -1249.7406f, 8.605103f)},
            new lsFishPosition { position = new Position(2839.609f, -641.5648f, 1.4439697f)},
            new lsFishPosition { position = new Position(2832.8176f, -611.55164f, 1.1237793f)},
            new lsFishPosition { position = new Position(2840.1362f, -723.6f, 1.1911621f)},
            new lsFishPosition { position = new Position(2827.3713f, -771.5209f, 2.168457f)},
        };
        public partial class lsFishPosition
        {
            public Position position { get; set; }
        }

        [AsyncClientEvent("Server:KeyHandler:PressE")]
        public void PressE(IPlayer player)
        {
            lock (player)
            {
                var fish = FishingHandler.sandyFishPositions.ToList().FirstOrDefault(x => player.Position.IsInRange(x.position, 2.5f));
                if (fish != null && !player.IsInVehicle)
                {
                    startFishingSandy((ClassicPlayer)player);
                    return;
                }
            }

        }

        internal static async Task startFishingSandy(ClassicPlayer player)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0) return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;

                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                if (player.IsFishing() == true) { HUDHandler.SendNotification(player, 3, 2500, "Du bist schon am Angeln!"); return; }
                if (!CharactersInventory.ExistCharacterItem(charId, "Angel", "inventory")) { HUDHandler.SendNotification(player, 3, 2500, "Du hast keine Angel dabei.."); return; }
                if (!CharactersInventory.ExistCharacterItem(charId, "Koeder", "inventory")) { HUDHandler.SendNotification(player, 3, 2500, "Du hast keine Köder dabei.."); return; }

                player.SetIsFishing(true);
                player.EmitLocked("objectAttacher:attachObjectAnimated", "fishing-rod", true);
                CharactersInventory.RemoveCharacterItemAmount(charId, "Koeder", 1, "inventory");

                HUDHandler.SendProgress(player, "Erstmal paar Fische Fangen!", "alert", 210000);
                await Task.Delay(210000);

                InventoryHandler.DetachObject(player);
                player.SetIsFishing(false);


                Random r = new Random();
                string[] fish = { "Hecht", "Forelle", "Brasse", "Zander", "Karpfen" };
                string itemName = fish[r.Next(0, fish.Length)];

                float itemWeight = ServerItems.GetItemWeight(itemName);
                float invWeight = CharactersInventory.GetCharacterItemWeight(charId, "inventory");
                float backpackWeight = CharactersInventory.GetCharacterItemWeight(charId, "backpack");
                if (invWeight + itemWeight > 5f && backpackWeight + itemWeight > Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId))) { HUDHandler.SendNotification(player, 3, 2500, $"Deine Taschen sind voll."); return; }
                if (invWeight + itemWeight <= 5f)
                {
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast einen {itemName} gefangen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, 1, "inventory");
                    return;
                }

                if (Characters.GetCharacterBackpack(charId) != -2 && backpackWeight + itemWeight <= Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId)))
                {
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast einen {itemName} gefangen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, 1, "backpack");
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
