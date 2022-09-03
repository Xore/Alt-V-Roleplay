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
using System.Text;
using System.Threading.Tasks;

namespace Altv_Roleplay.Handler
{
    class RobberyHandler : IScript
    {
        #region Pacific Bank Robbery
        public static Position bankRobPosition = new Position(254.28131f, 225.389f, 101.8689f);
        public static Position bankExitPosition = new Position(252.31f, 220.21f, 101.66f);
        public static List<bankPickUpPosition> bankPickUpPositions = new List<bankPickUpPosition>
        {
            new bankPickUpPosition { position = new Position(258.1055f, 217.97803f, 101.666f)},
            new bankPickUpPosition { position = new Position(260.545f, 217.252f, 101.666f)},
            new bankPickUpPosition { position = new Position(259.3978f, 214.021f, 101.666f)},
            new bankPickUpPosition { position = new Position(275.063f, 215.037f, 101.666f)},
            new bankPickUpPosition { position = new Position(262.180f, 213.059f, 101.666f)},
            new bankPickUpPosition { position = new Position(264.250f, 212.096f, 101.666f)},
            new bankPickUpPosition { position = new Position(265.912f, 213.665f, 101.666f)},
            new bankPickUpPosition { position = new Position(265.542f, 215.657f, 101.666f)},
            new bankPickUpPosition { position = new Position(263.39f, 216.435f, 101.666f)},

        };
        public static bool isBankCurrentlyRobbing = false;
        public static bool isBankOpened = false;

        internal static void EnterExitBank(ClassicPlayer player)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0) return;
                if (player.Position.IsInRange(bankRobPosition, 2f))
                {
                    if (isBankOpened == false) { HUDHandler.SendNotification(player, 1, 2500, "Der Tresor ist verschlossen, öffne diesen erst mit einem Schweißbrenner."); return; }
                    player.Position = bankExitPosition;
                }
                else if (player.Position.IsInRange(bankExitPosition, 2f))
                    player.Position = bankRobPosition;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        internal static void pickUpBankGold(ClassicPlayer player, bankPickUpPosition bankRobPosGold)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0 || bankRobPosGold == null) return;
                if (!isBankOpened) { HUDHandler.SendNotification(player, 1, 2500, "Der Tresor wurde noch nicht aufgeschweißt."); return; }
                if (bankRobPosGold.isPickedUp) { HUDHandler.SendNotification(player, 1, 2500, "Dieses Fach wurde bereits leer geräumt oder ist verschlossen."); return; }
                int randomAnzahl = new Random().Next(45, 65);
                float weight = ServerItems.GetItemWeight("Goldbarren") * randomAnzahl;
                if (CharactersInventory.GetCharacterItemWeight(player.CharacterId, "inventory") + weight <= 15f)
                    CharactersInventory.AddCharacterItem(player.CharacterId, "Goldbarren", randomAnzahl, "inventory");
                else if (CharactersInventory.GetCharacterItemWeight(player.CharacterId, "backpack") + weight <= Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(player.CharacterId)))
                    CharactersInventory.AddCharacterItem(player.CharacterId, "Goldbarren", randomAnzahl, "backpack");
                else { HUDHandler.SendNotification(player, 1, 2500, "Du hast keinen Platz mehr für die Goldbarren!"); return; }
                bankRobPosGold.isPickedUp = true;
                player.Emit("Client:Inventory:PlayAnimation", "anim@narcotics@trash", "drop_front", 500, 1, false);
                HUDHandler.SendNotification(player, 1, 2500, $"Du hast {randomAnzahl} Goldbarren.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        internal static async Task breakUpBank(ClassicPlayer player)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0) return;
                if (ServerFactions.GetFactionDutyMemberCount(2) + ServerFactions.GetFactionDutyMemberCount(12) < 10)
                {
                   HUDHandler.SendNotification(player, 1, 2500, "Es sind nicht genug Beamte im Dienst.");
                   return;
                }
                if (isBankOpened || isBankCurrentlyRobbing) { HUDHandler.SendNotification(player, 1, 2500, "Der Tresor wird bereits aufgeschweißt oder ist bereits offen."); return; }
                if (!CharactersInventory.ExistCharacterItem((int)player.GetCharacterMetaId(), "Schweißbrenner", "inventory") && !CharactersInventory.ExistCharacterItem((int)player.GetCharacterMetaId(), "Schweißbrenner", "backpack")) { HUDHandler.SendNotification(player, 1, 2500, "Du hast nicht das benötigte Item."); return; }
                isBankCurrentlyRobbing = true;
                HUDHandler.SendProgress(player, "Du schweißt den Tresor auf...", "alert", 100000);
                player.EmitLocked("Client:Inventory:PlayAnimation", "amb@world_human_welding@male@idle_a", "idle_a", 600000, 1, false);
                ServerFactions.AddNewFactionDispatchNoName("Stiller Alarm", 2, $"Aktiver Staatsbank Raub", player.Position);
                ServerFactions.AddNewFactionDispatchNoName("Stiller Alarm", 2, $"Aktiver Staatsbank Raub", player.Position);
                ServerFactions.AddNewFactionDispatchNoName("Stiller Alarm", 12, $"Aktiver Staatsbank Raub", player.Position);
                ServerFactions.AddNewFactionDispatchNoName("Stiller Alarm", 12, $"Aktiver Staatsbank Raub", player.Position);

                foreach (var PDmember in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && x.GetCharacterMetaId() > 0 && ServerFactions.IsCharacterInAnyFaction((int)x.GetCharacterMetaId()) && ServerFactions.GetCharacterFactionId((int)x.GetCharacterMetaId()) == 1 || ServerFactions.GetCharacterFactionId((int)x.GetCharacterMetaId()) == 3))
                    HUDHandler.SendNotification(PDmember, 1, 2500, "Ein Einbruch in die Staatsbank wurde gemeldet.");

                await Task.Delay(600000);
                if (player == null || !player.Exists || player.CharacterId <= 0 || !player.Position.IsInRange(bankRobPosition, 5f))
                {
                    isBankCurrentlyRobbing = false;
                    isBankOpened = false;
                    return;
                }

                isBankCurrentlyRobbing = false;
                isBankOpened = true;
                foreach (var goldPos in bankPickUpPositions)
                    goldPos.isPickedUp = false;

                player.EmitLocked("Client:Inventory:StopAnimation");
                CharactersInventory.RemoveCharacterItemAmount2((int)player.GetCharacterMetaId(), "Schweißbrenner", 1);
                HUDHandler.SendNotification(player, 1, 2500, "Der Tresor ist offen, gehe hinein, du hast 5 Minuten Zeit.");
                await Task.Delay(300000); //reset bank
                foreach (var goldPos in bankPickUpPositions)
                    goldPos.isPickedUp = true;
                isBankOpened = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        #endregion

        #region ATM

        public static bool isATMCurrentlyRobbing = false;
        public static bool isATMOpened = false;

        internal static async Task breakUpATM(ClassicPlayer player, Position atmpos)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0) return;
                var atmPos2 = ServerATM.ServerATM_.FirstOrDefault(x => player.Position.IsInRange(new Position(x.posX, x.posY, x.posZ), 1f));
                //if (atmPos2.isrobbed == 1) { HUDHandler.SendNotification(player, 3, 5000, "Der ATM wurde bereits ausgeraubt"); return; }

                if (atmPos2.isrobbed == 1)
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Der Bankautomaten wurde bereits ausgeraubt.");
                    return;
                }

                if (ServerFactions.GetFactionDutyMemberCount(2) + ServerFactions.GetFactionDutyMemberCount(12) < 2)
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Das Sicherheitssystem wurde erst erneuert!");
                    return;
                }

                ServerATM.SetRobbed(ServerATM.GetATMIdbypos(atmpos), 1);

                /*ServerFactions.AddNewFactionDispatch(0, 2, $"Aktiver Bankautomaten Raub", player.Position);
                ServerFactions.AddNewFactionDispatch(0, 12, $"Aktiver Bankautomaten Raub", player.Position);*/

                HUDHandler.SendNotification(player, 1, 2500, "Du schweißt den Bankautomaten auf...");
                player.EmitLocked("Client:Inventory:PlayAnimation", "amb@world_human_welding@male@idle_a", "idle_a", 240000, 1, false); //Animation 

                foreach (var PDmember in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && x.GetCharacterMetaId() > 0 && ServerFactions.IsCharacterInAnyFaction((int)x.GetCharacterMetaId()) && ServerFactions.GetCharacterFactionId((int)x.GetCharacterMetaId()) == 2))
                    HUDHandler.SendNotification(PDmember, 1, 2500, "Ein Bankautomaten meldet einen stillen Alarm.");
                ServerFactions.AddNewFactionDispatchNoName("Stiller Alarm", 2, "Ein Bankautomaten meldet einen stillen Alarm.", atmpos);
                ServerFactions.AddNewFactionDispatchNoName("Stiller Alarm", 12, "Ein Bankautomaten meldet einen stillen Alarm.", atmpos);

                await Task.Delay(17500); //Zeit bis offen 1
                if (!player.Position.IsInRange(atmpos, 2f))
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Du hast dich vom Bankautomaten entfernt - Abbruch");
                    foreach (var PDmember in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && x.GetCharacterMetaId() > 0 && ServerFactions.IsCharacterInAnyFaction((int)x.GetCharacterMetaId()) && ServerFactions.GetCharacterFactionId((int)x.GetCharacterMetaId()) == 2))
                        HUDHandler.SendNotification(PDmember, 3, 2500, "Der Bankautomaten konnte nicht aufgebrochen werden!");
                    ServerATM.SetRobbed(ServerATM.GetATMIdbypos(atmpos), 0);
                    return;
                }
                await Task.Delay(17500); //Zeit bis offen 2
                if (!player.Position.IsInRange(atmpos, 2f))
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Du hast dich vom Bankautomaten entfernt - Abbruch");
                    foreach (var PDmember in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && x.GetCharacterMetaId() > 0 && ServerFactions.IsCharacterInAnyFaction((int)x.GetCharacterMetaId()) && ServerFactions.GetCharacterFactionId((int)x.GetCharacterMetaId()) == 2))
                        HUDHandler.SendNotification(PDmember, 3, 2500, "Der Bankautomaten konnte nicht aufgebrochen werden!");
                    ServerATM.SetRobbed(ServerATM.GetATMIdbypos(atmpos), 0);
                    return;
                }
                await Task.Delay(17500); //Zeit bis offen 3
                if (!player.Position.IsInRange(atmpos, 2f))
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Du hast dich vom Bankautomaten entfernt - Abbruch");
                    foreach (var PDmember in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && x.GetCharacterMetaId() > 0 && ServerFactions.IsCharacterInAnyFaction((int)x.GetCharacterMetaId()) && ServerFactions.GetCharacterFactionId((int)x.GetCharacterMetaId()) == 2))
                        HUDHandler.SendNotification(PDmember, 3, 2500, "Der Bankautomaten konnte nicht aufgebrochen werden!");
                    ServerATM.SetRobbed(ServerATM.GetATMIdbypos(atmpos), 0);
                    return;
                }
                await Task.Delay(17500); //Zeit bis offen 4
                if (!player.Position.IsInRange(atmpos, 2f))
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Du hast dich vom Bankautomaten entfernt - Abbruch");
                    foreach (var PDmember in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && x.GetCharacterMetaId() > 0 && ServerFactions.IsCharacterInAnyFaction((int)x.GetCharacterMetaId()) && ServerFactions.GetCharacterFactionId((int)x.GetCharacterMetaId()) == 2))
                        HUDHandler.SendNotification(PDmember, 31, 2500, "Der Bankautomaten konnte nicht aufgebrochen werden!");
                    ServerATM.SetRobbed(ServerATM.GetATMIdbypos(atmpos), 0);
                    return;
                }
                await Task.Delay(17500); //Zeit bis offen 1
                if (!player.Position.IsInRange(atmpos, 2f))
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Du hast dich vom Bankautomaten entfernt - Abbruch");
                    foreach (var PDmember in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && x.GetCharacterMetaId() > 0 && ServerFactions.IsCharacterInAnyFaction((int)x.GetCharacterMetaId()) && ServerFactions.GetCharacterFactionId((int)x.GetCharacterMetaId()) == 2))
                        HUDHandler.SendNotification(PDmember, 3, 2500, "Der Bankautomaten konnte nicht aufgebrochen werden!");
                    ServerATM.SetRobbed(ServerATM.GetATMIdbypos(atmpos), 0);
                    return;
                }
                await Task.Delay(17500); //Zeit bis offen 2
                if (!player.Position.IsInRange(atmpos, 2f))
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Du hast dich vom Bankautomaten entfernt - Abbruch");
                    foreach (var PDmember in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && x.GetCharacterMetaId() > 0 && ServerFactions.IsCharacterInAnyFaction((int)x.GetCharacterMetaId()) && ServerFactions.GetCharacterFactionId((int)x.GetCharacterMetaId()) == 2))
                        HUDHandler.SendNotification(PDmember, 3, 2500, "Der Bankautomaten konnte nicht aufgebrochen werden!");
                    ServerATM.SetRobbed(ServerATM.GetATMIdbypos(atmpos), 0);
                    return;
                }
                await Task.Delay(17500); //Zeit bis offen 3
                if (!player.Position.IsInRange(atmpos, 2f))
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Du hast dich vom Bankautomaten entfernt - Abbruch");
                    foreach (var PDmember in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && x.GetCharacterMetaId() > 0 && ServerFactions.IsCharacterInAnyFaction((int)x.GetCharacterMetaId()) && ServerFactions.GetCharacterFactionId((int)x.GetCharacterMetaId()) == 2))
                        HUDHandler.SendNotification(PDmember, 3, 2500, "Der Bankautomaten konnte nicht aufgebrochen werden!");
                    ServerATM.SetRobbed(ServerATM.GetATMIdbypos(atmpos), 0);
                    return;
                }
                await Task.Delay(17500); //Zeit bis offen 4
                if (!player.Position.IsInRange(atmpos, 2f))
                {
                    HUDHandler.SendNotification(player, 3, 2500, "Du hast dich vom Bankautomaten entfernt - Abbruch");
                    foreach (var PDmember in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && x.GetCharacterMetaId() > 0 && ServerFactions.IsCharacterInAnyFaction((int)x.GetCharacterMetaId()) && ServerFactions.GetCharacterFactionId((int)x.GetCharacterMetaId()) == 2))
                        HUDHandler.SendNotification(PDmember, 3, 2500, "Der Bankautomaten konnte nicht aufgebrochen werden!");
                    ServerATM.SetRobbed(ServerATM.GetATMIdbypos(atmpos), 0);
                    return;
                }// Insgesamt 240000

                player.EmitLocked("Client:Inventory:StopAnimation");
                int rnd = new Random().Next(800, 3500); //Wie viel geld soll der ATM geben? //FASTCHANGE
                HUDHandler.SendNotification(player, 1, 2500, $"Der Bankautomaten ist geöffnet und du erhälst {rnd}$ aus dem Bankautomaten");
                foreach (var PDmember in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && x.GetCharacterMetaId() > 0 && ServerFactions.IsCharacterInAnyFaction((int)x.GetCharacterMetaId()) && ServerFactions.GetCharacterFactionId((int)x.GetCharacterMetaId()) == 2))
                    HUDHandler.SendNotification(PDmember, 1, 2500, "Der Bankautomat wurde aufgebrochen!");
                ServerFactions.AddNewFactionDispatchNoName("Stiller Alarm", 2, $"Der Bankautomat wurde aufgebrochen! Es wurden {rnd}$ gestohlen!", atmpos);
                ServerFactions.AddNewFactionDispatchNoName("Stiller Alarm", 12, $"Der Bankautomat wurde aufgebrochen! Es wurden {rnd}$ gestohlen!", atmpos);

                CharactersInventory.AddCharacterItem(player.CharacterId, "Bargeld", rnd, "inventory");

                await Task.Delay(3600000); //60 minuten 3600000

                ServerATM.SetRobbed(ServerATM.GetATMIdbypos(atmpos), 0);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        #endregion

        #region Jewelery 
        public static Position jeweleryRobPosition = new Position(-621.01f, -228.448f, 38.041f);
        public static bool isJeweleryCurrentlyRobbing = false;

        public static async Task robJewelery(ClassicPlayer player)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0) return;
                if (ServerFactions.GetFactionDutyMemberCount(2) + ServerFactions.GetFactionDutyMemberCount(12) < 10)
                {
                    HUDHandler.SendNotification(player, 1, 2500, "Es sind nicht genug Beamte im Dienst.");
                    return;
                }
                if (isJeweleryCurrentlyRobbing) { HUDHandler.SendNotification(player, 1, 2500, "Der Juwelier wird oder wurde bereits ausgeraubt."); return; }
                isJeweleryCurrentlyRobbing = true;
                HUDHandler.SendProgress(player, "Du raubst den Juwelier aus...", "alert", 600000);
                player.EmitLocked("Client:Inventory:PlayAnimation", "anim@narcotics@trash", "drop_front", 600000, 1, false);

                foreach (var PDmember in Alt.GetAllPlayers().ToList().Where(x => x != null && x.Exists && x.GetCharacterMetaId() > 0 && ServerFactions.IsCharacterInAnyFaction((int)x.GetCharacterMetaId()) && ServerFactions.GetCharacterFactionId((int)x.GetCharacterMetaId()) == 1 || ServerFactions.GetCharacterFactionId((int)x.GetCharacterMetaId()) == 3))
                    HUDHandler.SendNotification(PDmember, 1, 2500, "Ein Einbruch in den Juwelier wurde gemeldet.");
                ServerFactions.AddNewFactionDispatchNoName("Stiller Alarm", 2, $"Aktiver Juwelier Raub", player.Position);
                ServerFactions.AddNewFactionDispatchNoName("Stiller Alarm", 2, $"Aktiver Juwelier Raub", player.Position);
                ServerFactions.AddNewFactionDispatchNoName("Stiller Alarm", 12, $"Aktiver Juwelier Raub", player.Position);
                ServerFactions.AddNewFactionDispatchNoName("Stiller Alarm", 12, $"Aktiver Juwelier Raub", player.Position);

                await Task.Delay(600000);
                if (player == null || !player.Exists || player.CharacterId <= 0 || !player.Position.IsInRange(jeweleryRobPosition, 5f))
                {
                    isJeweleryCurrentlyRobbing = false;
                    return;
                }
                isJeweleryCurrentlyRobbing = false;
                player.EmitLocked("Client:Inventory:StopAnimation");
                int randomAmount = new Random().Next(50, 140);
                HUDHandler.SendNotification(player, 1, 2500, $"Du konntest {randomAmount} Diamanten erbeuten, verschwinde.");
                CharactersInventory.AddCharacterItem(player.CharacterId, "Diamanten", randomAmount, "inventory");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        #endregion
    }

    public partial class bankPickUpPosition
    {
        public Position position { get; set; }
        public bool isPickedUp { get; set; } = false;
    }
    
    public partial class LSPDPickUpPosition
    {
        public Position position { get; set; }
        public bool isPickedUp { get; set; } = false;
    }
}
