using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;

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
            catch(Exception e)
            {
                Alt.Log($"{e}");
            }
        }
    }
}
