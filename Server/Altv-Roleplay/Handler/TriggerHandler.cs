using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using System;

namespace Altv_Roleplay.Handler
{
    class TriggerHandler : IScript
    {
        [AsyncClientEvent("Server:Farming:StartProcessing")]
        public void FarmingStartProcessing(IPlayer player, string neededItem, string producedItem, int neededItemAmount, int producedItemAmount, int duration, string neededItemTWO, string neededItemTHREE, int neededItemTWOAmount, int neededItemTHREEAmount)
        {
            try
            {
                FarmingHandler.ProduceItem(player, neededItem, producedItem, neededItemAmount, producedItemAmount, duration, neededItemTWO, neededItemTHREE, neededItemTWOAmount, neededItemTHREEAmount);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
    }
}
