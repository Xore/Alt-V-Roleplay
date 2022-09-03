using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;
using System;
using System.Linq;


namespace Altv_Roleplay.Handler
{
    class DeathHandler : IScript
    {
        [ScriptEvent(ScriptEventType.PlayerDead)]
        public void OnPlayerDeath(ClassicPlayer player, IEntity killer, uint weapon)
        {
            try
            {
                if (player == null || !player.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                if (charId <= 0) return;
                if (Characters.IsCharacterUnconscious(charId)) return;
                if (Characters.IsCharacterInJail(charId))
                {
                    player.Spawn(new Position(1691.4594f, 2565.7056f, 45.556763f), 0);
                    player.Position = new Position(1691.4594f, 2565.7056f, 45.556763f);
                    return;
                }
                openDeathscreen(player);
                Characters.SetCharacterUnconscious(charId, true, 10); // Von 15 auf 10 geändert.
                ServerFactions.AddNewFactionDispatchNoName("Handy Notruf", 3, $"Eine Verletzte Person wurde gemeldet", player.Position);
                foreach (var p in Alt.Server.GetPlayers().Where(x => x != null && x.Exists && x.GetCharacterMetaId() > 0).ToList())
                {
                    if (!ServerFactions.IsCharacterInAnyFaction((int)p.GetCharacterMetaId()) || !ServerFactions.IsCharacterInFactionDuty((int)p.GetCharacterMetaId()) || ServerFactions.GetCharacterFactionId((int)p.GetCharacterMetaId()) != 3) continue;
                    HUDHandler.SendNotification(p, 3, 3500, "Eine Verletzte Person wurde gemeldet");
                }

                Alt.Emit("Server:Smartphone:leaveRadioFrequence", player);

                SmartphoneHandler.denyCall(player);

                ClassicPlayer killerPlayer = (ClassicPlayer)killer;
                if (killerPlayer == null || !killerPlayer.Exists) return;
                WeaponModel weaponModel = (WeaponModel)weapon;
                if (weaponModel == WeaponModel.Fist) return;
                if (Enum.IsDefined(typeof(AntiCheat.forbiddenWeapons), (Utils.AntiCheat.forbiddenWeapons)weaponModel))
                {
                    User.SetPlayerBanned(killerPlayer, true, $"Weapon: {weaponModel}");
                    killerPlayer.Kick("Bitte im Support Melden!");
                    player.Health = 200;
                    return;
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void openDeathscreen(IPlayer player)
        {
            try
            {
                if (player == null || !player.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                if (charId <= 0) return;
                Position pos = new Position(player.Position.X, player.Position.Y, player.Position.Z + 1);
                player.Spawn(pos);
                player.EmitLocked("Client:Ragdoll:SetPedToRagdoll", true, 0); //Ragdoll setzen
                player.EmitLocked("Client:Inventory:PlayAnimation", "misssolomon_5@end", "dead_black_ops", 300000, 1, false);
                player.EmitLocked("Client:Deathscreen:openCEF"); // Deathscreen öffnen
                player.SetPlayerIsUnconscious(true);
                DiscordLog.SendEmbed("death", "NightOut-Admin | Log", Characters.GetCharacterName((int)player.GetCharacterMetaId()) + " ist Bewustlos");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void closeDeathscreen(IPlayer player)
        {
            try
            {
                if (player == null || !player.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                if (charId <= 0) return;
                player.EmitLocked("Client:Deathscreen:closeCEF");
                player.SetPlayerIsUnconscious(false);
                player.SetPlayerIsFastFarm(false);
                player.EmitLocked("Client:Ragdoll:SetPedToRagdoll", false, 2000);
                Characters.SetCharacterUnconscious(charId, false, 0);
                Characters.SetCharacterFastFarm(charId, false, 0);
                player.EmitLocked("Client:Inventory:StopEffect", "DrugsMichaelAliensFight");

                foreach (var item in CharactersInventory.CharactersInventory_.ToList().Where(x => x.charId == charId))
                {
                    if (item.itemName.Contains("EC-Karte") || item.itemName.Contains("Ausweis") || item.itemName.Contains("Fahrzeugpapiere") || item.itemName.Contains("Fuehrerschein") || item.itemName.Contains("Waffenschein") || item.itemName.Contains("Fahrzeugschluessel")) continue;
                    CharactersInventory.RemoveCharacterItem(charId, item.itemName, item.itemLocation);
                }

                Characters.SetCharacterWeapon(player, "PrimaryWeapon", "None");
                Characters.SetCharacterWeapon(player, "PrimaryAmmo", 0);
                Characters.SetCharacterWeapon(player, "SecondaryWeapon2", "None");
                Characters.SetCharacterWeapon(player, "SecondaryWeapon", "None");
                Characters.SetCharacterWeapon(player, "SecondaryAmmo2", 0);
                Characters.SetCharacterWeapon(player, "SecondaryAmmo", 0);
                Characters.SetCharacterWeapon(player, "FistWeapon", "None");
                Characters.SetCharacterWeapon(player, "FistWeaponAmmo", 0);
                player.EmitLocked("Client:Smartphone:equipPhone", false, Characters.GetCharacterPhonenumber(charId), Characters.IsCharacterPhoneFlyModeEnabled(charId), Characters.GetCharacterPhoneWallpaper(charId));
                Characters.SetCharacterPhoneEquipped(charId, false);
                player.RemoveAllWeaponsAsync();
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        internal static void revive(IPlayer player)
        {
            try
            {
                if (player == null || !player.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                if (charId <= 0) return;
                player.EmitLocked("Client:Deathscreen:closeCEF");
                player.SetPlayerIsUnconscious(false);
                player.EmitLocked("Client:Ragdoll:SetPedToRagdoll", false, 2000);
                Characters.SetCharacterUnconscious(charId, false, 0);
                ServerFactions.SetFactionBankMoney(3, ServerFactions.GetFactionBankMoney(3) + 1500); //ToDo: Preis anpassen
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
    }
}
