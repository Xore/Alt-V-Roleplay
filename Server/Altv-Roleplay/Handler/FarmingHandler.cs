using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using AltV.Net.Resources.Chat.Api;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.models;
using Altv_Roleplay.Utils;

namespace Altv_Roleplay.Handler
{
    class FarmingHandler : IScript
    {
        #region FARM
        internal static async void FarmFieldAction(IPlayer player, string itemName, int itemMinAmount, int itemMaxAmount, string animation, int duration)
        {
            if (player == null || !player.Exists || itemName == "" || itemMinAmount == 0 || itemMaxAmount == 0 || animation == "") return;
            int charId = User.GetPlayerOnline(player);
            if (charId <= 0) return;
            if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
            InventoryHandler.InventoryAnimation(player, animation, duration);

            ClassicColshape farmCol = (ClassicColshape)ServerFarmingSpots.ServerFarmingSpotsColshapes_.FirstOrDefault(x => ((ClassicColshape)x).IsInRange((ClassicPlayer)player));
            var farmColData = ServerFarmingSpots.ServerFarmingSpots_.FirstOrDefault(x => x.id == (int)farmCol.GetColShapeId());
            if (farmColData.neededItemToFarm == "Spitzhacke")
            {
                player.EmitLocked("Client:Inventory:AttachObject", "pickaxe");
            }

            await Task.Delay(duration + 1250);
            lock (player)
            {
                player.SetPlayerFarmingActionMeta("None");
                InventoryHandler.DetachObject(player);
            }
            int rndItemAmount = new Random().Next(itemMinAmount, itemMaxAmount);
            //Doppelte Menge aufsammeln
            if (Characters.IsCharacterFastFarm(charId)) rndItemAmount += 1;
            float itemWeight = ServerItems.GetItemWeight(itemName) * rndItemAmount;
            float invWeight = CharactersInventory.GetCharacterItemWeight(charId, "inventory");
            float backpackWeight = CharactersInventory.GetCharacterItemWeight(charId, "backpack");
            if (invWeight + itemWeight > 15f && backpackWeight + itemWeight > Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId))) { HUDHandler.SendNotification(player, 3, 2500, $"Deine Taschen sind voll."); return; }

            if (invWeight + itemWeight <= 15f)
            {
                HUDHandler.SendNotification(player, 1, 2500, $"Du hast {itemName} ({rndItemAmount}x) gesammelt.");
                CharactersInventory.AddCharacterItem(charId, itemName, rndItemAmount, "inventory");
                return;
            }

            if (Characters.GetCharacterBackpack(charId) != -2 && backpackWeight + itemWeight <= Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId)))
            {
                HUDHandler.SendNotification(player, 1, 2500, $"Du hast {itemName} ({rndItemAmount}x) gesammelt.");
                CharactersInventory.AddCharacterItem(charId, itemName, rndItemAmount, "backpack");
                return;
            }

        }
        #endregion
        #region PRODUCER_CEF
        public static void openFarmingCEF(IPlayer player, string neededItem, string producedItem, int neededItemAmount, int producedItemAmount, int duration, string neededItemTWO, string neededItemTHREE, int neededItemTWOAmount, int neededItemTHREEAmount)
        {
            try
            {
                player.SetCEFOpen(true);
                player.EmitLocked("Client:Farming:createCEF", neededItem, producedItem, neededItemAmount, producedItemAmount, duration, neededItemTWO, neededItemTHREE, neededItemTWOAmount, neededItemTHREEAmount);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        #endregion

        #region PRODUCER_NEW
        internal static async void ProduceItem(IPlayer player, string neededItem, string producedItem, int neededItemAmount, int producedItemAmount, int duration, string neededItemTWO, string neededItemTHREE, int neededItemTWOAmount, int neededItemTHREEAmount)
        {
            try
            {
                if(player.IsProducing() == true)
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Es ist ein Fehler aufgetreten.");
                    player.SetIsProducing(false);
                    player.SetCEFOpen(false);
                    return;
                }

                player.SetIsProducing(true);
                //  check if player, neededItem, producedItem, Amount for both and duration  is set - also check if player exists 
                //  otherwise return
                if (player == null || !player.Exists || neededItem == "" || producedItem == "" || neededItemAmount == 0 || producedItemAmount == 0 || duration < 0) return;

                //  get and set charID
                int charID = User.GetPlayerOnline(player);

                //  init vars needed for later
                //  could init them later on but who cares :^)
                bool item2Set = false;
                bool item3Set = false;
                int giveInv = 0;
                int giveBack = 0;
                int canCreate = 0;
                int canCreate2 = 0;
                int canCreate3 = 0;

                //  check if charID is set
                //  otherwise return
                if (charID == 0) return;
                //  check if the producer needs more than one items
                //  if so - set the specific bool to true
                // additionally init more vars to use later
                if (neededItemTWO != "none")
                {
                    item2Set = true;
                }

                if (neededItemTHREE != "none")
                {
                    item3Set = true;
                }


                //  check if initial item is in backpack or inventory
                //  otherwise send notification to player and return
                if (!CharactersInventory.ExistCharacterItem(charID, neededItem, "inventory") && !CharactersInventory.ExistCharacterItem(charID, neededItem, "backpack"))
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht die richtigen Gegenstände, um " + neededItem + " zu verarbeiten.");
                    player.SetIsProducing(false);
                    player.SetCEFOpen(false);
                    return;
                };

                //  check if second item is in backpack or inventory
                //  otherwise send notification to player and return
                if (item2Set)
                {
                    if (!CharactersInventory.ExistCharacterItem(charID, neededItemTWO, "inventory") && !CharactersInventory.ExistCharacterItem(charID, neededItemTWO, "backpack"))
                    {
                        HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht die richtigen Gegenstände, um " + neededItemTWO + " zu verarbeiten.");
                        player.SetIsProducing(false);
                        player.SetCEFOpen(false);
                        return;
                    };
                }
                //  check if third item is in backpack or inventory
                //  otherwise send notification to player and return
                if (item3Set)
                {
                    if (!CharactersInventory.ExistCharacterItem(charID, neededItemTHREE, "inventory") && !CharactersInventory.ExistCharacterItem(charID, neededItemTHREE, "backpack"))
                    {
                        HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht die richtigen Gegenstände, um " + neededItemTHREE + " zu verarbeiten.");
                        player.SetIsProducing(false);
                        player.SetCEFOpen(false);
                        return;
                    };
                }

                //  check how much of the initial item are in backpack / inventory
                //  set vars with amount and calulate them together (why?)
                int invAmount = CharactersInventory.GetCharacterItemAmount(charID, neededItem, "inventory");
                int backAmount = CharactersInventory.GetCharacterItemAmount(charID, neededItem, "backpack");
                int sumAmount = invAmount + backAmount;
               
                if (invAmount <= 0 && backAmount <= 0)
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Es ist ein Fehler aufgetreten.");
                    player.SetIsProducing(false);
                    player.SetCEFOpen(false);
                    return;
                }


                if (sumAmount < neededItemAmount)
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht die nötigen Gegenstände dabei, um {neededItem} zu verarbeiten!");
                    player.SetIsProducing(false);
                    player.SetCEFOpen(false);
                    return;
                }



                if (sumAmount >= neededItemAmount)
                {
                    int availableNeededItems = sumAmount / neededItemAmount;
                    canCreate = availableNeededItems;
                }

                //  check how much of the second item are in backpack / inventory
                //  set vars with amount and calulate them together (why?)
                if (item2Set)
                {
                    int inv2Amount = CharactersInventory.GetCharacterItemAmount(charID, neededItemTWO, "inventory");
                    int back2Amount = CharactersInventory.GetCharacterItemAmount(charID, neededItemTWO, "backpack");
                    int sum2Amount = inv2Amount + back2Amount;

                    if (inv2Amount <= 0 && back2Amount <= 0)
                    {
                        HUDHandler.SendNotification(player, 3, 2500, $"Es ist ein Fehler aufgetreten.");
                        player.SetIsProducing(false);
                        player.SetCEFOpen(false);
                        return;
                    }


                    if (sum2Amount < neededItemTWOAmount)
                    {
                        HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht die nötigen Gegenstände dabei, um {neededItemTWO} zu verarbeiten!");
                        player.SetIsProducing(false);
                        player.SetCEFOpen(false);
                        return;
                    }


                    //  calculate how much could be created out of the initial item


                    if (sum2Amount >= neededItemTWOAmount)
                    {
                        int availableNeededItems2 = sum2Amount / neededItemTWOAmount;
                        canCreate2 = availableNeededItems2;
                    }
                }

                //  check how much of the third item are in backpack / inventory
                //  set vars with amount and calulate them together (why?)
                if (item3Set)
                {
                    int inv3Amount = CharactersInventory.GetCharacterItemAmount(charID, neededItemTHREE, "inventory");
                    int back3Amount = CharactersInventory.GetCharacterItemAmount(charID, neededItemTHREE, "backpack");
                    int sum3Amount = inv3Amount + back3Amount;

                    if (inv3Amount <= 0 && back3Amount <= 0)
                    {
                        HUDHandler.SendNotification(player, 3, 2500, $"Es ist ein Fehler aufgetreten.");
                        player.SetIsProducing(false);
                        player.SetCEFOpen(false);
                        return;
                    }


                    if (sum3Amount < neededItemTHREEAmount)
                    {
                        HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht die nötigen Gegenstände dabei, um {neededItemTHREE} zu verarbeiten!");
                        player.SetIsProducing(false);
                        player.SetCEFOpen(false);
                        return;
                    }



                    if (sum3Amount >= neededItemTHREEAmount)
                    {
                        int availableNeededItems3 = sum3Amount / neededItemTHREEAmount;
                        canCreate3 = availableNeededItems3;
                    }
                }

                //  get the loweset number of all three
                int[] fuckthisShitamenaIchhasseEsundSO = new[] { canCreate };

                if (item2Set)
                    fuckthisShitamenaIchhasseEsundSO.Append(canCreate2);

                if (item3Set)
                    fuckthisShitamenaIchhasseEsundSO.Append(canCreate3);

                int canMinCreate = fuckthisShitamenaIchhasseEsundSO.Min();

                //  calc that shit again                
                int finalAmount = canMinCreate * producedItemAmount;
                int finalNeeded = canMinCreate * neededItemAmount;
                int finalNeeded2 = canMinCreate * neededItemTWOAmount;
                int finalNeeded3 = canMinCreate * neededItemTHREEAmount;

                //  weight check
                float itemWeight = ServerItems.GetItemWeight(producedItem);
                float invWeight = CharactersInventory.GetCharacterItemWeight(charID, "inventory");
                float backWeight = CharactersInventory.GetCharacterItemWeight(charID, "backpack");


                if (itemWeight == 100f)
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Es ist ein Fehler aufgetreten.");
                    player.SetIsProducing(false);
                    player.SetCEFOpen(false);
                    return;
                }

                if (invWeight + itemWeight > 15.02f || backWeight + itemWeight > Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charID)))
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht genug Platz in deinen Taschen.");
                    player.SetIsProducing(false);
                    player.SetCEFOpen(false);
                    return;
                }

                //  check location
                player.SetPlayerFarmingActionMeta("produce");
                int finalDuration = canMinCreate * duration;

                HUDHandler.SendProgress(player, $"Verarbeitung von {neededItem} wurde gestartet...", "alert", finalDuration);
                await Task.Delay(finalDuration);

                //  positioning check 
                Position ProducerPos = player.Position;
                if (!player.Position.IsInRange(ProducerPos, 3f))
                {
                    HUDHandler.SendNotification(player, 3, 2500, $"Du hast dich zu weit entfernt.");
                    player.SetIsProducing(false);
                    player.SetCEFOpen(false);
                    player.SetPlayerFarmingActionMeta("None");
                    return;
                }


                //  run as long as we need to delete items
                while (finalNeeded > 0)
                {
                    //  get how much we got from dat shit
                    int tmpInv = CharactersInventory.GetCharacterItemAmount(charID, neededItem, "inventory");
                    int tmpBack = CharactersInventory.GetCharacterItemAmount(charID, neededItem, "backpack");

                    //  if we got something in our inventory traverse down
                    if (tmpInv > 0)
                    {
                        //  if the needed size is bigger than we got traverse first branch otherwise second
                        if (finalNeeded > tmpInv)
                        {
                            //  remove the max items possible from our inventory
                            CharactersInventory.RemoveCharacterItemAmount(charID, neededItem, tmpInv, "inventory");
                            // update the needed items to be deleted
                            finalNeeded -= tmpInv;
                        }
                        else
                        {
                            CharactersInventory.RemoveCharacterItemAmount(charID, neededItem, finalNeeded, "inventory");
                            finalNeeded = 0;
                        }
                        //  if we got something in our backpack traverse down - same as above
                    }
                    else if (tmpBack > 0)
                    {
                        if (finalNeeded > tmpBack)
                        {
                            CharactersInventory.RemoveCharacterItemAmount(charID, neededItem, tmpBack, "backpack");
                            finalNeeded -= tmpBack;
                        }
                        else
                        {
                            CharactersInventory.RemoveCharacterItemAmount(charID, neededItem, finalNeeded, "backpack");
                            finalNeeded = 0;
                        }
                    }
                }

                //  if second item is set
                if (item2Set)
                {
                    //  run as long as we need to delete items
                    while (finalNeeded2 > 0 )
                    {
                        //  get how much we got from dat shit
                        int tmpInv = CharactersInventory.GetCharacterItemAmount(charID, neededItemTWO, "inventory");
                        int tmpBack = CharactersInventory.GetCharacterItemAmount(charID, neededItemTWO, "backpack");

                        //  if we got something in our inventory traverse down
                        if (tmpInv > 0 )
                        {
                            //  if the needed size is bigger than we got traverse first branch otherwise second
                            if (finalNeeded2 > tmpInv)
                            {
                                //  remove the max items possible from our inventory
                                CharactersInventory.RemoveCharacterItemAmount(charID, neededItemTWO, tmpInv, "inventory");
                                // update the needed items to be deleted
                                finalNeeded2 -= tmpInv;
                            }
                            else
                            {
                                CharactersInventory.RemoveCharacterItemAmount(charID, neededItemTWO, finalNeeded2, "inventory");
                                finalNeeded2 = 0;
                            }
                            //  if we got something in our backpack traverse down - same as above
                        }
                        else if (tmpBack > 0)
                        {
                            if (finalNeeded2 > tmpBack)
                            {
                                CharactersInventory.RemoveCharacterItemAmount(charID, neededItemTWO, tmpBack, "backpack");
                                finalNeeded2 -= tmpBack;
                            }
                            else
                            {
                                CharactersInventory.RemoveCharacterItemAmount(charID, neededItemTWO, finalNeeded2, "backpack");
                                finalNeeded2 = 0;
                            }
                        }
                    }
                }

                //  if third item is set
                if (item3Set)
                {
                    //  run as long as we need to delete items
                    while (finalNeeded3 > 0)
                    {
                        //  get how much we got from dat shit
                        int tmpInv = CharactersInventory.GetCharacterItemAmount(charID, neededItemTHREE, "inventory");
                        int tmpBack = CharactersInventory.GetCharacterItemAmount(charID, neededItemTHREE, "backpack");

                        //  if we got something in our inventory traverse down
                        if (tmpInv > 0)
                        {
                            //  if the needed size is bigger than we got traverse first branch otherwise second
                            if (finalNeeded3 > tmpInv)
                            {
                                //  remove the max items possible from our inventory
                                CharactersInventory.RemoveCharacterItemAmount(charID, neededItemTHREE, tmpInv, "inventory");
                                // update the needed items to be deleted
                                finalNeeded3 -= tmpInv;
                            }
                            else
                            {
                                CharactersInventory.RemoveCharacterItemAmount(charID, neededItemTHREE, finalNeeded3, "inventory");
                                finalNeeded3 = 0;
                            }
                            //  if we got something in our backpack traverse down - same as above
                        }
                        else if (tmpBack > 0)
                        {
                            if (finalNeeded3 > tmpBack)
                            {
                                CharactersInventory.RemoveCharacterItemAmount(charID, neededItemTHREE, tmpBack, "backpack");
                                finalNeeded3 -= tmpBack;
                            }
                            else
                            {
                                CharactersInventory.RemoveCharacterItemAmount(charID, neededItemTHREE, finalNeeded3, "backpack");
                                finalNeeded3 = 0;
                            }
                        }
                    }
                }


                while (finalAmount > 0)
                {
                    //  dry calc number that can be added to inventory
                    bool finished = false;
                    giveInv = 0;
                    giveBack = 0;

                    //  check how much items you can put in the inventory until its full
                    while (!finished && finalAmount > 0)
                    {
                        itemWeight = ServerItems.GetItemWeight(producedItem);
                        invWeight = CharactersInventory.GetCharacterItemWeight(charID, "inventory");

                        //  if current weight of inventory plus itemweight is above 15f its finished - if inventoryweight is same as 15f its finished
                        if (invWeight + itemWeight > 15f || invWeight == 15f)
                            finished = true;

                        //  if inventoryweight plus itemweight is less than 15f give item to inventory and remove one from finalamount to add
                        if (invWeight + itemWeight < 15f)
                        {
                            giveInv += 1;
                            finalAmount -= 1;
                        }
                    }

                    bool finished2 = false;
                    while (!finished2 && finalAmount > 0)
                    {
                        itemWeight = ServerItems.GetItemWeight(producedItem);
                        backWeight = CharactersInventory.GetCharacterItemWeight(charID, "backpack");
                        float backSize = Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charID));

                        //  if current weight of bag plus itemweight is above backSIze its finished - if backweight is same as backsize its finished
                        if (backWeight + itemWeight > backSize || backWeight == backSize)
                            finished2 = true;


                        //  if backWeight plus itemweight is less than backSize give item to backpack and remove one from finalamount to add
                        if (backWeight + itemWeight <= backSize)
                        {
                            giveBack += 1;
                            finalAmount -= 1;
                        }
                    }

                    //  something went wrong and finalAmount didnt went down to zero - release this shit
                    if (finished && finished2)
                        break;
                }

                CharactersInventory.AddCharacterItem(charID, producedItem, giveInv, "inventory");
                CharactersInventory.AddCharacterItem(charID, producedItem, giveBack, "backpack");
                lock (player)
                {
                    player.SetPlayerFarmingActionMeta("None");
                    HUDHandler.SendNotification(player, 2, 2500, $"Verarbeitung von {neededItem} ist nun abgeschlossen.");
                }
                player.SetIsProducing(false);
                player.SetCEFOpen(false);
                player.SetPlayerFarmingActionMeta("None");
            }
            catch (Exception e)
            {
                player.SetIsProducing(false);
                player.SetCEFOpen(false);
                player.SetPlayerFarmingActionMeta("None");
                Alt.Log($"{e}");
            }
        }
        #endregion
    }
}