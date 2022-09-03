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
    class InventoryHandler : IScript
    {
        [AsyncClientEvent("Server:Inventory:RequestInventoryItems")]
        public static void RequestInventoryItems(IPlayer player)
        {
            try
            {
                if (player == null || !player.Exists) return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                string invArray = CharactersInventory.GetCharacterInventory(User.GetPlayerOnline(player));
                player.EmitLocked("Client:Inventory:AddInventoryItems", invArray, Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(User.GetPlayerOnline(player))), 0);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Inventory:closeCEF")]
        public void CloseInventoryCEF(IPlayer player)
        {
            if (player == null || !player.Exists) return;
            player.EmitLocked("Client:Inventory:closeCEF");
        }

        [AsyncClientEvent("Server:Inventory:switchItemToDifferentInv")]
        public void switchItemToDifferentInv(ClassicPlayer player, string itemname, int itemAmount, string fromContainer, string toContainer)
        {
            try
            {
                if (player == null || !player.Exists || itemname == "" || itemAmount <= 0 || fromContainer == "" || toContainer == "" || User.GetPlayerOnline(player) == 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                int charId = player.CharacterId;
                string normalName = ServerItems.ReturnNormalItemName(itemname);
                float itemWeight = ServerItems.GetItemWeight(itemname) * itemAmount;
                float invWeight = CharactersInventory.GetCharacterItemWeight(charId, "inventory");
                float backpackWeight = CharactersInventory.GetCharacterItemWeight(charId, "backpack");
                if (!CharactersInventory.ExistCharacterItem(charId, itemname, fromContainer)) return;

                if (toContainer == "inventory") { if (invWeight + itemWeight > 15f) { HUDHandler.SendNotification(player, 3, 2500, $"Soviel Platz hast du im Inventar nicht."); return; } }
                else if (toContainer == "backpack") { if (backpackWeight + itemWeight > Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId))) { HUDHandler.SendNotification(player, 3, 2500, $"Soviel Platz hast du in deinen Taschen / deinem Rucksack nicht."); return; } }

                if (CharactersInventory.GetCharacterItemAmount(charId, itemname, fromContainer) < itemAmount) { HUDHandler.SendNotification(player, 3, 2500, "Die angegebene Item-Anzahl ist größer als die Anzahl der Items die du mit dir trägst."); return; }
                if (itemname == "Rucksack" || itemname == "Tasche" || normalName == "Ausweis" || normalName == "Bargeld" || normalName == "Smartphone" || normalName == "EC-Karte" || normalName == "Fahrzeugschluessel") { HUDHandler.SendNotification(player, 3, 2500, "Diesen Gegenstand kannst du nicht in deinen Rucksack / deine Tache legen."); return; }
                CharactersInventory.RemoveCharacterItemAmount(charId, itemname, itemAmount, fromContainer);
                CharactersInventory.AddCharacterItem(charId, itemname, itemAmount, toContainer);
                HUDHandler.SendNotification(player, 1, 2500, $"Du hast {itemAmount}x {itemname} verschoben.");
                RequestInventoryItems(player);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Inventory:UseItem")]
        public void UseItem(ClassicPlayer player, string itemname, int itemAmount, string fromContainer)
        {
            try
            {
                string ECData = null,
                    CarKeyData = null;
                if (player == null || !player.Exists || itemname == "" || itemAmount <= 0 || fromContainer == "" || User.GetPlayerOnline(player) == 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                if (ServerItems.IsItemUseable(ServerItems.ReturnNormalItemName(itemname)) == false) { HUDHandler.SendNotification(player, 3, 2500, $"Dieser Gegenstand ist nicht benutzbar ({itemname})!"); return; }
                int charId = player.CharacterId;
                if (charId <= 0 || CharactersInventory.ExistCharacterItem(charId, itemname, fromContainer) == false) return;
                if (CharactersInventory.GetCharacterItemAmount(charId, itemname, fromContainer) < itemAmount) { HUDHandler.SendNotification(player, 3, 2500, $"Die angegeben zu nutzende Anzahl ist nicht vorhanden ({itemname})!"); return; }
                if (itemname.Contains("EC-Karte")) { string[] SplittedItemName = itemname.Split(' '); ECData = itemname.Replace("EC-Karte ", ""); itemname = "EC-Karte"; }
                else if (itemname.Contains("Fahrzeugschluessel")) { string[] SplittedItemName = itemname.Split(' '); CarKeyData = itemname.Replace("Fahrzeugschluessel ", ""); itemname = "Autoschluessel"; }

                if (ServerItems.IsItemDesire(itemname))
                {
                    CharactersInventory.RemoveCharacterItemAmount(charId, itemname, itemAmount, fromContainer);
                    Characters.SetCharacterHunger(charId, Characters.GetCharacterHunger(charId) + ServerItems.GetItemDesireFood(itemname) * itemAmount);
                    Characters.SetCharacterThirst(charId, Characters.GetCharacterThirst(charId) + ServerItems.GetItemDesireDrink(itemname) * itemAmount);
                    player.EmitLocked("Client:HUD:UpdateDesire", Characters.GetCharacterHunger(charId), Characters.GetCharacterThirst(charId)); //HUD updaten
                }

                else if (itemname == "Beamtenschutzweste")
                {
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Beamtenschutzweste", 1, fromContainer);
                    Characters.SetCharacterArmor(charId, 100);
                    player.Armor = 100;
                }
                else if (itemname == "Weste")
                {
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Weste", 1, fromContainer);
                    Characters.SetCharacterArmor(charId, 100);
                    player.Armor = 100;
                    if (Characters.GetCharacterGender(charId)) player.EmitLocked("Client:SpawnArea:setCharClothes", 9, 17, 2);
                    else player.EmitLocked("Client:SpawnArea:setCharClothes", 9, 15, 2);
                }
                if (itemname == "Rucksack" || itemname == "Tasche")
                {
                    if (fromContainer == "backpack") { HUDHandler.SendNotification(player, 3, 2500, "Kleidungen & Taschen können nicht aus dem Rucksack aus benutzt werden."); return; }
                    if (Characters.GetCharacterBackpack(charId) == 31)
                    {
                        if (itemname == "Rucksack")
                        {
                            if (CharactersInventory.GetCharacterBackpackItemCount(charId) == 0)
                            {
                                Characters.SetCharacterBackpack(player, "None");
                                HUDHandler.SendNotification(player, 1, 2500, "Du hast deinen Rucksack ausgezogen.");
                            }
                            else { HUDHandler.SendNotification(player, 3, 2500, "Du hast zuviele Sachen im Rucksack, du kannst deinen Rucksack nicht ablegen."); return; }
                        }
                        else
                        {
                            HUDHandler.SendNotification(player, 3, 2500, "Du hast bereits eine Tasche angelegt, lege diese vorher ab um deinen Rucksack anzulegen.");
                            return;
                        }
                    }
                    else if (Characters.GetCharacterBackpack(charId) == 45)
                    {
                        if (itemname == "Tasche")
                        {
                            if (CharactersInventory.GetCharacterBackpackItemCount(charId) == 0)
                            {
                                Characters.SetCharacterBackpack(player, "None");
                                HUDHandler.SendNotification(player, 1, 2500, "Du hast deine Tasche ausgezogen.");
                            }
                            else { HUDHandler.SendNotification(player, 3, 2500, "Du hast zuviele Sachen in deiner Tasche, du kannst deine Tasche nicht ablegen."); return; }
                        }
                        else
                        {
                            HUDHandler.SendNotification(player, 3, 2500, "Du hast bereits einen Rucksack angelegt, lege diesen vorher ab um deine Tasche anzulegen.");
                            return;
                        }
                    }
                    else if (Characters.GetCharacterBackpack(charId) == 0)
                    {
                        Characters.SetCharacterBackpack(player, itemname);
                        if (itemname == "Rucksack")
                        {
                            HUDHandler.SendNotification(player, 1, 2500, "Du hast deinen Rucksack angezogen");
                        }
                        else
                        {
                            HUDHandler.SendNotification(player, 1, 2500, "Du hast deine Tasche angezogen.");
                        }
                    }
                }
                else if (itemname == "Kokain")
                {
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Kokain", 1, fromContainer);
                    player.EmitLocked("Client:Inventory:PlayEffect", "DeadlineNeon", 900000);
                    player.EmitLocked("Client:Inventory:PlayAnimation", "anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01", 2000, 49, false);
                    player.EmitLocked("Client:Inventory:PlayWalking", "move_m@quick");
                    Characters.SetCharacterFastFarm(charId, true, 15);
                    Characters.SetCharacterArmor(charId, 15);
                    player.Armor = +15;

                }
                else if (itemname == "Joint")
                {
                    if (!CharactersInventory.ExistCharacterItem(charId, "Feuerzeug", fromContainer)) { HUDHandler.SendNotification(player, 3, 2500, "Du hast kein Feuerzeug dabei.."); return; }
                    if (CharactersInventory.GetCharacterItemAmount(charId, "Joint", fromContainer) < 1) { return; }
                    ushort currentHealth = player.Health;
                    currentHealth += 5;
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Joint", 1, fromContainer);
                    player.EmitLocked("Client:Inventory:AttachObject", "joint");
                    player.EmitLocked("Client:Inventory:PlayEffect", "ChopVision", 900000);
                    player.EmitLocked("Client:Inventory:PlayWalking", "move_m@gangster@generic");
                }
                else if (itemname == "Zigaretten")
                {
                    if (!CharactersInventory.ExistCharacterItem(charId, "Zigaretten", fromContainer)) { return; }
                    if (CharactersInventory.GetCharacterItemAmount(charId, "Zigaretten", fromContainer) < 1) { return; }
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Zigaretten", 1, fromContainer);
                    CharactersInventory.AddCharacterItem(charId, $"Zigarette", 15, fromContainer);
                }
                else if (itemname == "Zigarette")
                {
                    if (!CharactersInventory.ExistCharacterItem(charId, "Feuerzeug", fromContainer)) { HUDHandler.SendNotification(player, 3, 2500, "Du hast kein Feuerzeug dabei.."); return; }
                    if (CharactersInventory.GetCharacterItemAmount(charId, "Zigarette", fromContainer) < 1) { return; }
                    ushort currentHealth = player.Health;
                    currentHealth -= 1;
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Zigarette", 1, fromContainer);
                    player.EmitLocked("Client:Inventory:AttachObject", "cigarette");
                }
                else if (itemname == "Methamphetamin")
                {
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Methamphetamin", 1, fromContainer);
                    player.EmitLocked("Client:Inventory:PlayEffect", "DMT_flight", 900000);
                    player.EmitLocked("Client:Inventory:PlayWalking", "move_characters@michael@fire");
                }
                else if (itemname == "Whisky")
                {
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Whisky", 1, fromContainer);
                    player.EmitLocked("Client:Inventory:PlayEffect", "BikerFilter", 270000);
                    player.EmitLocked("Client:Inventory:PlayWalking", "move_m@drunk@verydrunk");
                }
                else if (itemname == "Tequila")
                {
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Tequila", 1, fromContainer);
                    player.EmitLocked("Client:Inventory:PlayEffect", "BikerFilter", 270000);
                    player.EmitLocked("Client:Inventory:PlayWalking", "move_m@drunk@verydrunk");
                }
                else if (itemname == "Bier")
                {
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Bier", 1, fromContainer);
                    player.EmitLocked("Client:Inventory:PlayEffect", "BikerFilter", 150000);
                    player.EmitLocked("Client:Inventory:PlayWalking", "move_m@drunk@a");
                }
                else if (itemname == "Vodka")
                {
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Vodka", 1, fromContainer);
                    player.EmitLocked("Client:Inventory:PlayEffect", "BikerFilter", 300000);
                    player.EmitLocked("Client:Inventory:PlayWalking", "move_m@drunk@verydrunk");
                }
                else if (itemname == "Wein")
                {
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Wein", 1, fromContainer);
                    player.EmitLocked("Client:Inventory:PlayEffect", "BikerFilter", 210000);
                    player.EmitLocked("Client:Inventory:PlayWalking", "move_m@drunk@a");
                }
                else if (itemname == "EC-Karte")
                {
                    var atmPos = ServerATM.ServerATM_.FirstOrDefault(x => player.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 1f));
                    if (atmPos == null || player.IsInVehicle) { HUDHandler.SendNotification(player, 3, 2500, "Du bist an keinem ATM oder sitzt in einem Auto."); return; }
                    int usingAccountNumber = Convert.ToInt32(ECData);
                    if (CharactersBank.GetBankAccountLockStatus(usingAccountNumber)) { if (CharactersInventory.ExistCharacterItem(charId, "EC-Karte " + usingAccountNumber, "inventory")) { CharactersInventory.RemoveCharacterItemAmount(charId, "EC-Karte " + usingAccountNumber, 1, "inventory"); } HUDHandler.SendNotification(player, 3, 2500, $"Ihre EC-Karte wurde einzogen da diese gesperrt ist."); return; }
                    player.EmitLocked("Client:ATM:BankATMcreateCEF", CharactersBank.GetBankAccountPIN(usingAccountNumber), usingAccountNumber, atmPos.zoneName);
                }
                else if (ServerItems.GetItemType(itemname) == "weapon")
                {                    
                    if (itemname.Contains("Munitionsbox"))
                    {                        
                        string wName = itemname.Replace(" Munitionsbox", "");
                        CharactersInventory.RemoveCharacterItemAmount(charId, itemname, itemAmount, fromContainer);
                        CharactersInventory.AddCharacterItem(charId, $"{wName} Munition", 30 * itemAmount, fromContainer);
                    }
                    else if (itemname.Contains("Munition")) { WeaponHandler.EquipCharacterWeapon(player, "Ammo", itemname, itemAmount, fromContainer); }
                    else { WeaponHandler.EquipCharacterWeapon(player, "Weapon", itemname, 0, fromContainer); }
                }
                #region NOT_WORKING???
                else if (itemname == "Micro-Kiste")
                {
                    if (!CharactersInventory.ExistCharacterItem(charId, "Brecheisen", fromContainer)) { HUDHandler.SendNotification(player, 3, 2500, "Du hast kein Brecheisen dabei.."); return; }
                    if (CharactersInventory.GetCharacterItemAmount(charId, "Micro-Kiste", fromContainer) < 1) { return; }
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Micro-Kiste", 1, fromContainer);
                    CharactersInventory.AddCharacterItem(charId, $"Micro-SMG", 1, fromContainer);
                    { CharactersInventory.AddCharacterItem(charId, $"Micro-SMG Munitionsbox", 5, fromContainer); }
                }
                else if (itemname == "50.Pistol-Kiste")
                {
                    if (!CharactersInventory.ExistCharacterItem(charId, "Brecheisen", fromContainer)) { HUDHandler.SendNotification(player, 3, 2500, "Du hast kein Brecheisen dabei.."); return; }
                    if (CharactersInventory.GetCharacterItemAmount(charId, "50.Pistol-Kiste", fromContainer) < 1) { return; }
                    CharactersInventory.RemoveCharacterItemAmount(charId, "50.Pistol-Kiste", 1, fromContainer);
                    CharactersInventory.AddCharacterItem(charId, $"50.Pistol", 1, fromContainer);
                    CharactersInventory.AddCharacterItem(charId, $"50.Pistol Munitionsbox", 5, fromContainer);
                }
                else if (itemname == "Gusenberg-Kiste")
                {
                    if (!CharactersInventory.ExistCharacterItem(charId, "Brecheisen", fromContainer)) { HUDHandler.SendNotification(player, 3, 2500, "Du hast kein Brecheisen dabei.."); return; }
                    if (CharactersInventory.GetCharacterItemAmount(charId, "Gusenberg-Kiste", fromContainer) < 1) { return; }
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Gusenberg-Kiste", 1, fromContainer);
                    CharactersInventory.AddCharacterItem(charId, $"Gusenberg", 1, fromContainer);
                    CharactersInventory.AddCharacterItem(charId, $"Gusenberg Munitionsbox", 5, fromContainer);
                }
                else if (itemname == "Heavy-Pistol-Kiste")
                {
                    if (!CharactersInventory.ExistCharacterItem(charId, "Brecheisen", fromContainer)) { HUDHandler.SendNotification(player, 3, 2500, "Du hast kein Brecheisen dabei.."); return; }
                    if (CharactersInventory.GetCharacterItemAmount(charId, "Heavy-Pistol-Kiste", fromContainer) < 1) { return; }
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Heavy-Pistol-Kiste", 1, fromContainer);
                    CharactersInventory.AddCharacterItem(charId, $"Heavy-Pistol", 1, fromContainer);
                    CharactersInventory.AddCharacterItem(charId, $"Heavy-Pistol Munitionsbox", 5, fromContainer);
                }
                else if (itemname == "Compact-Kiste")
                {
                    if (!CharactersInventory.ExistCharacterItem(charId, "Brecheisen", fromContainer)) { HUDHandler.SendNotification(player, 3, 2500, "Du hast kein Brecheisen dabei.."); return; }
                    if (CharactersInventory.GetCharacterItemAmount(charId, "Compact-Kiste", fromContainer) < 1) { return; }
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Compact-Kiste", 1, fromContainer);
                    CharactersInventory.AddCharacterItem(charId, $"Compact", 1, fromContainer);
                    CharactersInventory.AddCharacterItem(charId, $"Compact Munitionsbox", 5, fromContainer);
                }
                #endregion
                else if (itemname == "Verbandskasten")
                {
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Verbandskasten", 1, fromContainer);
                    Characters.SetCharacterHealth(charId, 200);
                    player.Health = 200;
                }
                else if (itemname == "Verband")
                {
                    ushort currentHealth = player.Health; //get currentHealth of player
                    currentHealth += 35; //adds 35 health to currentHealth
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Verband", 1, fromContainer);
                    Characters.SetCharacterHealth(charId, currentHealth);
                    player.Health = currentHealth;
                }
                else if (itemname == "Pflaster")
                {
                    ushort currentHealth = player.Health; //get currentHealth of player
                    currentHealth += 15; //adds 25 health to currentHealth
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Pflaster", 1, fromContainer);
                    Characters.SetCharacterHealth(charId, currentHealth);
                    player.Health = currentHealth;
                }
                else if (itemname == "Tabletten")
                {
                    ushort currentHealth = player.Health; //get currentHealth of player
                    currentHealth += 25; //adds 25 health to currentHealth
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Tabletten", 1, fromContainer);
                    Characters.SetCharacterHealth(charId, currentHealth);
                    player.Health = currentHealth;
                }
                else if (itemname == "Benzinkanister" && player.IsInVehicle && player.Vehicle.Exists)
                {
                    if (ServerVehicles.GetVehicleFuel(player.Vehicle) >= ServerVehicles.GetVehicleFuelLimitOnHash(player.Vehicle.Model)) { HUDHandler.SendNotification(player, 3, 2500, "Der Tank ist bereits voll."); return; }
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Benzinkanister", 1, fromContainer);
                    ServerVehicles.SetVehicleFuel(player.Vehicle, ServerVehicles.GetVehicleFuel(player.Vehicle) + 15.0f);
                    HUDHandler.SendNotification(player, 2, 2500, "Du hast das Fahrzeug erfolgreich aufgetankt.");
                }
                else if (itemname == "Kokain-Pack")
                {
                    if (!CharactersInventory.ExistCharacterItem(charId, "Kokain-Pack", fromContainer)) { return; }
                    if (CharactersInventory.GetCharacterItemAmount(charId, "Kokain-Pack", fromContainer) < 1) { return; }
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Kokain-Pack", 1, fromContainer);
                    if (!CharactersInventory.ExistCharacterItem(charId, "Baggy", fromContainer)) { HUDHandler.SendNotification(player, 3, 2500, "Du hast keine Baggy's dabei."); return; }
                    if (CharactersInventory.GetCharacterItemAmount(charId, "Baggy", fromContainer) < 1) { return; }
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Baggy", 1, fromContainer);
                    CharactersInventory.AddCharacterItem(charId, $"Kokain", 10, fromContainer);
                }
                else if (itemname == "Papes")
                {
                    if (!CharactersInventory.ExistCharacterItem(charId, "Papes", fromContainer)) { return; }
                    if (CharactersInventory.GetCharacterItemAmount(charId, "Papes", fromContainer) < 1) { return; }
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Papes", 1, fromContainer);
                    if (!CharactersInventory.ExistCharacterItem(charId, "Weed", fromContainer)) { HUDHandler.SendNotification(player, 3, 2500, "Du hast kein Weed dabei."); return; }
                    if (CharactersInventory.GetCharacterItemAmount(charId, "Weed", fromContainer) < 1) { return; }
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Weed", 1, fromContainer);
                    CharactersInventory.AddCharacterItem(charId, $"Joint", 1, fromContainer);
                }
                else if (itemname == "Schweißbrenner")
                {
                    var atmPos = ServerATM.ServerATM_.FirstOrDefault(x => player.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 1f));
                    int usingAccountNumber = Convert.ToInt32(ECData);
                    if (atmPos != null)
                    {
                        _ = RobberyHandler.breakUpATM(player, new Position(atmPos.posX, atmPos.posY, atmPos.posZ));
                        return;
                    }

                    if (player.Position.IsInRange(Handler.RobberyHandler.bankRobPosition, 2f))
                    {
                        _ = RobberyHandler.breakUpBank(player);
                        return;
                    }

                    var house = ServerHouses.ServerHouses_.FirstOrDefault(x => x.ownerId > 0 && x.isLocked && ((ClassicColshape)x.entranceShape).IsInRange((ClassicPlayer)player));
                    if (house != null)
                    {
                        HouseHandler.BreakIntoHouse(player, house.id);
                        return;
                    }
                    else { HUDHandler.SendNotification(player, 3, 2500, "Das kannst du hier nicht benutzen."); return; }
                }
                else if (itemname == "Schraubendreher")
                {
                    var atmPos = ServerATM.ServerATM_.FirstOrDefault(x => player.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 1f));
                    int usingAccountNumber = Convert.ToInt32(ECData);
                    if (atmPos != null)
                    {
                        _ = RobberyHandler.breakUpATM(player, new Position(atmPos.posX, atmPos.posY, atmPos.posZ));
                        return;
                    }

                    var house = ServerHouses.ServerHouses_.FirstOrDefault(x => x.ownerId > 0 && x.isLocked && ((ClassicColshape)x.entranceShape).IsInRange((ClassicPlayer)player));
                    if (house != null)
                    {
                        HouseHandler.BreakIntoHouse(player, house.id);
                        return;
                    }
                    else { HUDHandler.SendNotification(player, 3, 2500, "Das kannst du hier nicht benutzen."); return; }
                }
                else if (itemname == "Lappen")
                {
                    player.EmitLocked("Client:Inventory:Clean", "Vehicle", 0);
                }
                else if (itemname == "Weedsamen")
                {
                    if (!CharactersInventory.ExistCharacterItem(charId, "Blumentopf", fromContainer)) { HUDHandler.SendNotification(player, 3, 2500, "Du hast keinen Blumentopf dabei."); return; }
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Blumentopf", 1, fromContainer);
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Weedsamen", 1, fromContainer);
                    player.EmitLocked("Client:Animation:PlayAnimation", "amb@world_human_gardener_plant@male@idle_a", "idle_a", 2000, 15, false);
                    WeedPlantHandler.PlaceNewWeedpot(player);
                }
                else if (itemname == "Pflanzenwasser")
                {
                    WeedPlantHandler.fillNearestPotWithWater(player);
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Dünger", 1, fromContainer);
                }
                else if (itemname == "Dünger")
                {
                    WeedPlantHandler.fertilizeNearestPot(player);
                    CharactersInventory.RemoveCharacterItemAmount(charId, "Dünger", 1, fromContainer);
                }
                else if (itemname == "PD-Absperrung")
                {
                    ObjectHandler.PlaceNewBarrier(player);
                }
                else if (itemname == "Smartphone")
                {
                    //Alt.Log("Phone benutzt.");
                    if (Characters.IsCharacterPhoneEquipped(charId))
                    {
                        //Alt.Log("Phone benutzt2.");
                        player.EmitLocked("Client:Smartphone:equipPhone", false, Characters.GetCharacterPhonenumber(charId), Characters.IsCharacterPhoneFlyModeEnabled(charId), Characters.GetCharacterPhoneWallpaper(charId));
                        HUDHandler.SendNotification(player, 1, 2500, "Smartphone ausgeschaltet.");
                        Alt.Emit("Server:Smartphone:leaveRadioFrequence", player);
                    }
                    else
                    {
                        //Alt.Log("Phone benutzt3.");
                        player.EmitLocked("Client:Smartphone:equipPhone", true, Characters.GetCharacterPhonenumber(charId), Characters.IsCharacterPhoneFlyModeEnabled(charId), Characters.GetCharacterPhoneWallpaper(charId));
                        HUDHandler.SendNotification(player, 3, 2500, "Smartphone eingeschaltet.");
                    }
                    Characters.SetCharacterPhoneEquipped(charId, !Characters.IsCharacterPhoneEquipped(charId));
                    SmartphoneHandler.RequestLSPDIntranet((ClassicPlayer)player);
                }
                else
                {
                    Console.WriteLine(itemname);
                }

                if (ServerItems.hasItemAnimation(ServerItems.ReturnNormalItemName(itemname))) { InventoryAnimation(player, ServerItems.GetItemAnimationName(ServerItems.ReturnNormalItemName(itemname)), 0); }

                RequestInventoryItems(player);
                //HUDHandler.SendNotification(player, 2, 5000, $"DEBUG: Der Gegenstand {itemname} ({itemAmount}) wurde erfolgreich aus ({fromContainer}) benutzt.");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Inventory:DropItem")]
        public void DropItem(ClassicPlayer player, string itemname, int itemAmount, string fromContainer)
        {
            try
            {
                if (player == null || !player.Exists || itemname == "" || itemAmount <= 0 || fromContainer == "" || User.GetPlayerOnline(player) == 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                string normalItemName = ServerItems.ReturnNormalItemName(itemname);
                if (ServerItems.IsItemDroppable(itemname) == false) { HUDHandler.SendNotification(player, 3, 2500, $"Diesen Gegenstand kannst du nicht wegwerfen ({itemname})."); return; }
                int charId = player.CharacterId;
                if (charId <= 0 || CharactersInventory.ExistCharacterItem(charId, itemname, fromContainer) == false) return;
                if (CharactersInventory.GetCharacterItemAmount(charId, itemname, fromContainer) < itemAmount) { HUDHandler.SendNotification(player, 3, 2500, $"Die angegebene wegzuwerfende Anzahl ist nicht vorhanden ({itemname})."); return; }
                if (itemname == "Smartphone")
                {
                    if (Characters.IsCharacterPhoneEquipped(charId)) { HUDHandler.SendNotification(player, 3, 2500, "Du musst dein Handy erst ausschalten / ablegen."); return; }
                }
                else if (itemname == "Rucksack")
                {
                    if (Characters.GetCharacterBackpack(charId) == 31)
                    {
                        if (CharactersInventory.GetCharacterItemAmount(charId, "Rucksack", "inventory") == itemAmount)
                        {
                            HUDHandler.SendNotification(player, 3, 2500, "Du musst deinen Rucksack erst ablegen, bevor du diesen wegwerfen kannst.");
                            return;
                        }
                        else
                        {
                            CharactersInventory.RemoveCharacterItemAmount(charId, itemname, itemAmount, fromContainer);
                            InventoryAnimation(player, "drop", 0);
                            HUDHandler.SendNotification(player, 1, 2500, $"Der Gegenstand {itemname} ({itemAmount}) wurde erfolgreich weggeworfen ({fromContainer}).");
                            return;
                        }
                    }
                    else
                    {
                        CharactersInventory.RemoveCharacterItemAmount(charId, itemname, itemAmount, fromContainer);
                        InventoryAnimation(player, "drop", 0);
                        HUDHandler.SendNotification(player, 1, 2500, $"Der Gegenstand {itemname} ({itemAmount}) wurde erfolgreich weggeworfen ({fromContainer}).");
                        return;
                    }
                }
                else if (itemname == "Tasche")
                {
                    if (Characters.GetCharacterBackpack(charId) == 45)
                    {
                        if (CharactersInventory.GetCharacterItemAmount(charId, "Tasche", "inventory") == itemAmount)
                        {
                            HUDHandler.SendNotification(player, 3, 2500, "Du musst zuerst deine Tasche ablegen, bevor du diese wegwerfen kannst.");
                            return;
                        }
                        else
                        {
                            CharactersInventory.RemoveCharacterItemAmount(charId, itemname, itemAmount, fromContainer);
                            InventoryAnimation(player, "drop", 0);
                            HUDHandler.SendNotification(player, 1, 2500, $"Der Gegenstand {itemname} ({itemAmount}) wurde erfolgreich weggeworfen ({fromContainer}).");
                            RequestInventoryItems(player);
                            return;
                        }
                    }
                    else
                    {
                        CharactersInventory.RemoveCharacterItemAmount(charId, itemname, itemAmount, fromContainer);
                        InventoryAnimation(player, "drop", 0);
                        HUDHandler.SendNotification(player, 1, 2500, $"Der Gegenstand {itemname} ({itemAmount}) wurde erfolgreich weggeworfen ({fromContainer}).");
                        RequestInventoryItems(player);
                        return;
                    }
                }
                else if (ServerItems.GetItemType(itemname) == "weapon")
                {
                    if ((string)Characters.GetCharacterWeapon(player, "PrimaryWeapon") == normalItemName || (string)Characters.GetCharacterWeapon(player, "SecondaryWeapon") == normalItemName || (string)Characters.GetCharacterWeapon(player, "SecondaryWeapon2") == normalItemName || (string)Characters.GetCharacterWeapon(player, "FistWeapon") == normalItemName)
                    {
                        if (CharactersInventory.GetCharacterItemAmount(charId, normalItemName, fromContainer) == itemAmount) { HUDHandler.SendNotification(player, 3, 2500, "Du musst zuerst deine Waffe ablegen."); return; }
                    }
                }
                ServerDroppedItems.AddItem(itemname, itemAmount, new AltV.Net.Data.Position(player.Position.X, player.Position.Y + 1f, player.Position.Z - 1), DateTime.Now, player.Dimension);
                CharactersInventory.RemoveCharacterItemAmount(charId, itemname, itemAmount, fromContainer);
                player.EmitLocked("Client:Inventory:PlayAnimation", "anim@mp_snowball", "pickup_snowball", 500, 1, false);
                HUDHandler.SendNotification(player, 1, 2500, $"{itemAmount}x {itemname} weggeworfen.");
                RequestInventoryItems(player);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Inventory:GiveItem")]
        public void GiveItem(ClassicPlayer player, string itemname, int itemAmount, string fromContainer, int targetPlayerId)
        {
            try
            {
                if (player == null || !player.Exists || itemname == "" || itemAmount <= 0 || fromContainer == "" || targetPlayerId == 0) return;
                player.EmitLocked("Client:Inventory:closeCEF");
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                if (!ServerItems.IsItemGiveable(itemname)) { HUDHandler.SendNotification(player, 3, 2500, $"Diesen Gegenstand kannst du nicht weggeben ({itemname})."); return; }
                int charId = player.CharacterId;
                if (charId <= 0 || !CharactersInventory.ExistCharacterItem(charId, itemname, fromContainer)) return;
                if (CharactersInventory.GetCharacterItemAmount(charId, itemname, fromContainer) < itemAmount) { HUDHandler.SendNotification(player, 3, 2500, $"Die angegebene Anzahl ist nicht vorhanden ({itemname})."); return; }
                if (CharactersInventory.IsItemActive(player, itemname)) { HUDHandler.SendNotification(player, 3, 2500, $"Ausgerüstete Gegenstände können nicht abgegeben werden."); return; }
                float itemWeight = ServerItems.GetItemWeight(itemname) * itemAmount;
                float invWeight = CharactersInventory.GetCharacterItemWeight(targetPlayerId, "inventory");
                float backpackWeight = CharactersInventory.GetCharacterItemWeight(targetPlayerId, "backpack");
                float schluesselWeight = CharactersInventory.GetCharacterItemWeight(targetPlayerId, "Schluessel");
                float brieftascheWeight = CharactersInventory.GetCharacterItemWeight(targetPlayerId, "brieftasche");
                var targetPlayer = Alt.GetAllPlayers().ToList().FirstOrDefault(x => x.GetCharacterMetaId() == (ulong)targetPlayerId);
                if (targetPlayer == null) return;
                if (!player.Position.IsInRange(targetPlayer.Position, 2f)) { HUDHandler.SendNotification(player, 3, 2500, "Die Person ist zu weit entfernt."); return; }
                if (invWeight + itemWeight > 15f && backpackWeight + itemWeight > Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(targetPlayerId))) { HUDHandler.SendNotification(player, 3, 2500, $"Der Spieler hat nicht genug Platz in seinen Taschen."); return; }

                if (itemname.Contains("Bargeld"))
                {
                    HUDHandler.SendNotification(targetPlayer, 1, 2500, $"Dir wurde ({itemAmount}x) {itemname} gegeben.");
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast ({itemAmount}x) {itemname} weiter gegeben.");
                    CharactersInventory.AddCharacterItem(targetPlayerId, itemname, itemAmount, "brieftasche");
                    CharactersInventory.RemoveCharacterItemAmount(charId, itemname, itemAmount, fromContainer);
                    InventoryAnimation(player, "give", 0);
                    return;
                }
                else if (itemname.Contains("Ausweis "))
                {
                    HUDHandler.SendNotification(targetPlayer, 1, 2500, $"Dir wurde ({itemAmount}x) {itemname} gegeben.");
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast ({itemAmount}x) {itemname} weiter gegeben.");
                    CharactersInventory.AddCharacterItem(targetPlayerId, itemname, itemAmount, "brieftasche");
                    CharactersInventory.RemoveCharacterItemAmount(charId, itemname, itemAmount, fromContainer);
                    InventoryAnimation(player, "give", 0);
                    return;
                }
                else if (itemname.Contains("EC-Karte "))
                {
                    HUDHandler.SendNotification(targetPlayer, 1, 2500, $"Dir wurde ({itemAmount}x) {itemname} gegeben.");
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast ({itemAmount}x) {itemname} weiter gegeben.");
                    CharactersInventory.AddCharacterItem(targetPlayerId, itemname, itemAmount, "brieftasche");
                    CharactersInventory.RemoveCharacterItemAmount(charId, itemname, itemAmount, fromContainer);
                    InventoryAnimation(player, "give", 0);
                    return;
                }
                else if (itemname.Contains("Fahrzeugschluessel"))
                {
                    HUDHandler.SendNotification(targetPlayer, 1, 2500, $"Dir wurde ({itemAmount}x) {itemname} gegeben.");
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast ({itemAmount}x) {itemname} weiter gegeben.");
                    CharactersInventory.AddCharacterItem(targetPlayerId, itemname, itemAmount, "schluessel");
                    CharactersInventory.RemoveCharacterItemAmount(charId, itemname, itemAmount, fromContainer);
                    InventoryAnimation(player, "give", 0);
                    return;
                }
                else if (itemname.Contains("Generalschluessel"))
                {
                    HUDHandler.SendNotification(targetPlayer, 1, 2500, $"Dir wurde ({itemAmount}x) {itemname} gegeben.");
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast ({itemAmount}x) {itemname} weiter gegeben.");
                    CharactersInventory.AddCharacterItem(targetPlayerId, itemname, itemAmount, "schluessel");
                    CharactersInventory.RemoveCharacterItemAmount(charId, itemname, itemAmount, fromContainer);
                    InventoryAnimation(player, "give", 0);
                    return;
                }
                else if (itemname.Contains("Handschellenschluessel"))
                {
                    HUDHandler.SendNotification(targetPlayer, 1, 2500, $"Dir wurde ({itemAmount}x) {itemname} gegeben.");
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast ({itemAmount}x) {itemname} weiter gegeben.");
                    CharactersInventory.AddCharacterItem(targetPlayerId, itemname, itemAmount, "schluessel");
                    CharactersInventory.RemoveCharacterItemAmount(charId, itemname, itemAmount, fromContainer);
                    InventoryAnimation(player, "give", 0);
                    return;
                }

                if (invWeight + itemWeight <= 15f)
                {
                    HUDHandler.SendNotification(targetPlayer, 1, 2500, $"Dir wurde ({itemAmount}x) {itemname} gegeben.");
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast ({itemAmount}x) {itemname} weiter gegeben.");
                    CharactersInventory.AddCharacterItem(targetPlayerId, itemname, itemAmount, "inventory");
                    CharactersInventory.RemoveCharacterItemAmount(charId, itemname, itemAmount, fromContainer);
                    InventoryAnimation(player, "give", 0);
                    return;
                }

                if (Characters.GetCharacterBackpack(targetPlayerId) != -2 && backpackWeight + itemWeight <= Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(targetPlayerId)))
                {
                    HUDHandler.SendNotification(targetPlayer, 1, 2500, $"Dir wurde ({itemAmount}x) {itemname} gegeben.");
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast ({itemAmount}x) {itemname} weiter gegeben.");
                    CharactersInventory.AddCharacterItem(targetPlayerId, itemname, itemAmount, "backpack");
                    CharactersInventory.RemoveCharacterItemAmount(charId, itemname, itemAmount, fromContainer);
                    InventoryAnimation(player, "give", 0);
                    return;
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:PlayerSearch:TakeItem")]
        public void PlayerSearchTakeItem(ClassicPlayer player, int givenTargetCharId, string itemName, string itemLocation, int itemAmount)
        {
            try
            {
                if (player == null || !player.Exists || givenTargetCharId <= 0 || itemName == "" || itemAmount <= 0 || itemLocation == "") return;
                int charId = player.CharacterId;
                if (charId <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                var targetPlayer = Alt.GetAllPlayers().ToList().FirstOrDefault(x => x.GetCharacterMetaId() == (ulong)givenTargetCharId);
                int targetCharId = (int)targetPlayer.GetCharacterMetaId();
                if (targetCharId != givenTargetCharId) return;
                if (targetPlayer == null || !targetPlayer.Exists) return;
                if (!player.Position.IsInRange(targetPlayer.Position, 3f)) { HUDHandler.SendNotification(player, 3, 2500, "Du bist zuweit vom Spieler entfernt."); return; }
                if (!targetPlayer.HasPlayerHandcuffs() && !targetPlayer.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Der Spieler ist nicht gefesselt."); return; }
                if (!ServerItems.IsItemDroppable(itemName) || !ServerItems.IsItemGiveable(itemName)) { HUDHandler.SendNotification(player, 3, 2500, "Diesen Gegenstand kannst du nicht entfernen."); return; }
                if (!CharactersInventory.ExistCharacterItem(targetCharId, itemName, itemLocation)) { HUDHandler.SendNotification(player, 3, 2500, "Dieser Gegenstand existiert nicht mehr."); return; }
                if (CharactersInventory.IsItemActive(targetPlayer, itemName)) { HUDHandler.SendNotification(player, 3, 2500, "Ausgerüstete Gegenstände können nicht entwendet werden."); return; }
                if (CharactersInventory.GetCharacterItemAmount(targetCharId, itemName, itemLocation) < itemAmount) { HUDHandler.SendNotification(player, 3, 2500, "Soviele Gegenstände hat der Spieler davon nicht."); return; }
                float itemWeight = ServerItems.GetItemWeight(itemName) * itemAmount;
                float invWeight = CharactersInventory.GetCharacterItemWeight(charId, "inventory");
                float backpackWeight = CharactersInventory.GetCharacterItemWeight(charId, "backpack");
                float schluesselWeight = CharactersInventory.GetCharacterItemWeight(charId, "Schluessel");
                float brieftascheWeight = CharactersInventory.GetCharacterItemWeight(charId, "brieftasche");
                if (invWeight + itemWeight > 15f && backpackWeight + itemWeight > Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId))) { HUDHandler.SendNotification(player, 3, 2500, $"Du hast nicht genug Platz in deinen Taschen."); return; }
                CharactersInventory.RemoveCharacterItemAmount(targetCharId, itemName, itemAmount, itemLocation);
                if (itemName.Contains("Bargeld"))
                {
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast der Person {itemName} ({itemAmount}x) abgenommen.");
                    HUDHandler.SendNotification(targetPlayer, 1, 2500, $"Dir wurde der {itemName} ({itemAmount}x) abgenommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "brieftasche");
                    return;
                }
                else if (itemName.Contains("Ausweis "))
                {
                    HUDHandler.SendNotification(player, 1, 2500, $"Das kannst du der Person nicht Abnehmen!");
                    //HUDHandler.SendNotification(targetPlayer, 1, 2500, $"Dir wurde der {itemName} ({itemAmount}x) abgenommen.");
                    //CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "brieftasche");
                    return;
                }
                else if (itemName.Contains("EC-Karte "))
                {
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast der Person {itemName} ({itemAmount}x) abgenommen.");
                    HUDHandler.SendNotification(targetPlayer, 1, 2500, $"Dir wurde der {itemName} ({itemAmount}x) abgenommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "brieftasche");
                    return;
                }
                else if (itemName.Contains("Fahrzeugschluessel"))
                {
                    HUDHandler.SendNotification(player, 1, 2500, $"Das kannst du der Person nicht Abnehmen!");
                    //HUDHandler.SendNotification(targetPlayer, 1, 2500, $"Dir wurde der {itemName} ({itemAmount}x) abgenommen.");
                    //CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "schluessel");
                    return;
                }
                else if (itemName.Contains("Generalschluessel"))
                {
                    HUDHandler.SendNotification(player, 1, 2500, $"Das kannst du der Person nicht Abnehmen!");
                    //HUDHandler.SendNotification(targetPlayer, 1, 2500, $"Dir wurde der {itemName} ({itemAmount}x) abgenommen.");
                    //CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "schluessel");
                    return;
                }
                else if (itemName == ("Smartphone"))
                {
                    targetPlayer.EmitLocked("Client:Smartphone:equipPhone", false, Characters.GetCharacterPhonenumber(charId), Characters.IsCharacterPhoneFlyModeEnabled(charId), Characters.GetCharacterPhoneWallpaper(charId));
                    Alt.Emit("Server:Smartphone:leaveRadioFrequence", targetPlayer);
                    return;
                }
                else if (invWeight + itemWeight <= 15f)
                {
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast der Person {itemName} ({itemAmount}x) abgenommen.");
                    HUDHandler.SendNotification(targetPlayer, 1, 2500, $"Dir wurde der {itemName} ({itemAmount}x) abgenommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "inventory");
                    return;
                }
                //

                if (Characters.GetCharacterBackpack(charId) != -2 && backpackWeight + itemWeight <= Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId)))
                {
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast der Person {itemName} ({itemAmount}x) abgenommen.");
                    HUDHandler.SendNotification(targetPlayer, 1, 2500, $"Dir wurde der {itemName} ({itemAmount}x) abgenommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "backpack");
                    return;
                }

                if (Characters.GetCharacterBackpack(charId) != -2 && backpackWeight + itemWeight <= Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId)))
                {
                    HUDHandler.SendNotification(player, 1, 2500, $"Du hast der Person {itemName} ({itemAmount}x) abgenommen.");
                    HUDHandler.SendNotification(targetPlayer, 1, 2500, $"Dir wurde der {itemName} ({itemAmount}x) abgenommen.");
                    CharactersInventory.AddCharacterItem(charId, itemName, itemAmount, "schluessel");
                    return;
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void InventoryAnimation(IPlayer player, string Animation, int duration)
        {
            if (player == null || !player.Exists || player.IsInVehicle || Animation == "") return;
            if (Animation == "eat") player.EmitLocked("Client:Inventory:AttachObject", "burger");
            else if (Animation == "drink") player.EmitLocked("Client:Inventory:AttachObject", "drink");
            else if (Animation == "alcohol") player.EmitLocked("Client:Inventory:AttachObject", "alcohol");
            else if (Animation == "drop") player.EmitLocked("Client:Inventory:PlayAnimation", "anim@narcotics@trash", "drop_front", 500, 1, false);
            else if (Animation == "give") player.EmitLocked("Client:Inventory:PlayAnimation", "anim@narcotics@trash", "drop_front", 500, 1, false);
            else if (Animation == "farmPickup") player.EmitLocked("Client:Inventory:PlayAnimation", "pickup_object", "pickup_low", duration, 1, false);
            else if (Animation == "handcuffs") player.EmitLocked("Client:Inventory:PlayAnimation", "mp_arresting", "sprint", -1, 50, false);
            else if (Animation == "revive") player.EmitLocked("Client:Inventory:PlayAnimation", "missheistfbi3b_ig8_2", "cpr_loop_paramedic", duration, 1, false);
            else if (Animation == "weste") player.EmitLocked("Client:Inventory:PlayAnimation", "anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01", 3000, 49, false);
            else if (Animation == "Kokain") player.EmitLocked("Client:Inventory:PlayAnimation", "anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01", 2000, 49, false);
            else if (Animation == "verband") player.EmitLocked("Client:Inventory:PlayAnimation", "anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01", 3000, 49, false);
            else if (Animation == "Kokain2") player.EmitLocked("Client:Inventory:PlayAnimation", "anim@mp_player_intupperface_palm", "idle_a", 2000, 49, false);
            else if (Animation == "Trash") player.EmitLocked("Client:Inventory:AttachObject", "trash");
            else if (Animation == "Pickaxe") player.EmitLocked("Client:Inventory:AttachObject", "pickaxe");
            else if (Animation == "Trash2") player.EmitLocked("Client:Inventory:PlayAnimation", "anim@heists@narcotics@trash", "throw_b", 0, 49, false);
        }

        internal static void DetachObject(IPlayer player)
        {
            if (player == null || !player.Exists) return;
            player.EmitLocked("Client:Inventory:DetachObject");
        }

        internal static void StopAnimation(IPlayer player, string animDict, string animName)
        {
            if (player == null || !player.Exists) return;
            player.EmitLocked("Client:Inventory:StopAnimation", animDict, animName);
        }
    }
}
