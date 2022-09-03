using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Altv_Roleplay.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altv_Roleplay.Handler
{
    class WeaponHandler
    {
        public static void EquipCharacterWeapon(IPlayer player, string type, string wName, int amount, string fromContainer)
        {
            try
            {
                int charId = User.GetPlayerOnline(player);
                string wType = "None";
                string normalWName = "None";
                string ammoWName = "None";
                WeaponModel wHash = 0;

                switch (wName)
                {
                    case "Pistol":
                    case "Pistol Munition":
                        wType = "Secondary";
                        normalWName = "Pistol";
                        ammoWName = "Pistol Munition";
                        wHash = (WeaponModel)0x1B06D571;
                        break;
                    case "PistolMKII":
                    case "PistolMKII Munition":
                        wType = "Secondary";
                        normalWName = "PistolMKII";
                        ammoWName = "PistolMKII Munition";
                        wHash = (WeaponModel)0xBFE256D4;
                        break;
                    case "Combat-Pistol":
                    case "Combat-Pistol Munition":
                        wType = "Secondary";
                        normalWName = "Combat-Pistol";
                        ammoWName = "Combat-Pistol Munition";
                        wHash = (WeaponModel)0x5EF9FEC4;
                        break;
                    case "SNS-Pistol":
                    case "SNS-Pistol Munition":
                        wType = "Secondary";
                        normalWName = "SNS-Pistol";
                        ammoWName = "SNS-Pistol Munition";
                        wHash = (WeaponModel)0xBFD21232;
                        break;
                    case "50.Pistol":
                    case "50.Pistol Munition":
                        wType = "Secondary";
                        normalWName = "50.Pistol";
                        ammoWName = "50.Pistol Munition";
                        wHash = (WeaponModel)0x99AEEB3B;
                        break;
                    case "Revolver":
                    case "Revolver Munition":
                        wType = "Secondary";
                        normalWName = "Revolver";
                        ammoWName = "Revolver Munition";
                        wHash = (WeaponModel)0xC1B3C3D1;
                        break;
                    case "Heavy-Pistol":
                    case "Heavy-Pistol Munition":
                        wType = "Secondary";
                        normalWName = "Heavy-Pistol";
                        ammoWName = "Heavy-Pistol Munition";
                        wHash = (WeaponModel)0xD205520E;
                        break;
                    case "Tazer":
                        wType = "Secondary";
                        wHash = WeaponModel.StunGun;
                        break;
                    case "Leuchtpistole":
                    case "Leuchtpistole Munition":
                        wType = "Secondary";
                        normalWName = "Leuchtpistole";
                        ammoWName = "Leuchtpistole Munition";
                        wHash = (WeaponModel)0x47757124;
                        break;
                    case "Combat-PDW":
                    case "Combat-PDW Munition":
                        wType = "Primary";
                        normalWName = "Combat-PDW";
                        ammoWName = "Combat-PDW Munition";
                        wHash = (WeaponModel)0x0A3D4D34;
                        break;
                    case "Karabiner":
                    case "Karabiner Munition":
                        wType = "Primary";
                        normalWName = "Karabiner";
                        ammoWName = "Karabiner Munition";
                        wHash = (WeaponModel)0x83BF0278;
                        break;
                    case "SMG":
                    case "SMG Munition":
                        wType = "Primary";
                        normalWName = "SMG";
                        ammoWName = "SMG Munition";
                        wHash = (WeaponModel)0x2BE6766B;
                        break;
                    case "SMGMKII":
                    case "SMGMKII Munition":
                        wType = "Primary";
                        normalWName = "SMGMKII";
                        ammoWName = "SMGMKII Munition";
                        wHash = (WeaponModel)0x78A97CD0;
                        break;
                    case "Micro-SMG":
                    case "Micro-SMG Munition":
                        wType = "Primary";
                        normalWName = "Micro-SMG";
                        ammoWName = "Micro-SMG Munition";
                        wHash = (WeaponModel)0x13532244;
                        break;
                    case "Gusenberg":
                    case "Gusenberg Munition":
                        wType = "Primary";
                        normalWName = "Gusenberg";
                        ammoWName = "Gusenberg Munition";
                        wHash = (WeaponModel)0x61012683;
                        break;
                    case "Compact":
                    case "Compact Munition":
                        wType = "Primary";
                        normalWName = "Compact";
                        ammoWName = "Compact Munition";
                        wHash = (WeaponModel)0x624FE830;
                        break;
                    case "Assault-Rifle":
                    case "Assault-Rifle Munition":
                        wType = "Primary";
                        normalWName = "Assault-Rifle";
                        ammoWName = "Assault-Rifle Munition";
                        wHash = (WeaponModel)0xBFEFFF6D;
                        break;
                    case "Schrotflinte":
                    case "Schrot Munition":
                        wType = "Primary";
                        normalWName = "Schrotflinte";
                        ammoWName = "Schrot Munition";
                        wHash = (WeaponModel)0x1D073A89;
                        break;
                    case "Abgesägte":
                    case "Abgesägte-Munition":
                        wType = "Primary";
                        normalWName = "Abgesägte";
                        ammoWName = "Abgesägte-Munition";
                        wHash = (WeaponModel)0x7846A318;
                        break;
                    case "Schlagstock":
                        wType = "Fist";
                        normalWName = "Schlagstock";
                        wHash = (WeaponModel)0x678B81B1;
                        break;
                    case "Messer":
                        wType = "Fist";
                        normalWName = "Messer";
                        wHash = (WeaponModel)0x99B507EA;
                        break;
                    case "Brecheisen":
                        wType = "Fist";
                        normalWName = "Brecheisen";
                        wHash = (WeaponModel)0x84BD7BFD;
                        break;
                    case "Baseballschlaeger":
                        wType = "Fist";
                        normalWName = "Baseballschlaeger";
                        wHash = (WeaponModel)0x958A4A8F;
                        break;
                    case "Dolch":
                        wType = "Fist";
                        normalWName = "Dolch";
                        wHash = (WeaponModel)0x92A27487;
                        break;
                    case "Hammer":
                        wType = "Fist";
                        normalWName = "Hammer";
                        wHash = (WeaponModel)0x4E875F73;
                        break;
                    case "Axt":
                        wType = "Fist";
                        normalWName = "Axt";
                        wHash = (WeaponModel)0xF9DCBF2D;
                        break;
                    case "Machete":
                        wType = "Fist";
                        normalWName = "Machete";
                        wHash = (WeaponModel)0xDD5DF8D9;
                        break;
                    case "Springmesser":
                        wType = "Fist";
                        normalWName = "Springmesser";
                        wHash = (WeaponModel)0xDFE37640;
                        break;
                    case "Schlagring":
                        wType = "Fist";
                        normalWName = "Schlagring";
                        wHash = (WeaponModel)0xD8DF3C3C;
                        break;
                    case "Taschenlampe":
                        wType = "Fist";
                        normalWName = "Taschenlampe";
                        wHash = (WeaponModel)0x8BB05FD7;
                        break;
                    case "Golfschlaeger":
                        wType = "Fist";
                        normalWName = "Golfschlaeger";
                        wHash = (WeaponModel)0x440E4788;
                        break;
                }
                if (wName == "Silencer")
                {
                    SetWeaponComponents(player, "Silencer");
                }
                else if (wName == "Mounted-Scope")
                {
                    SetWeaponComponents(player, "Mounted-Scope");
                }
                else if (wName == "Compensator")
                {
                    SetWeaponComponents(player, "Compensator");
                }
                else if (wName == "Extended-Clip")
                {
                    SetWeaponComponents(player, "Extended-Clip");
                }

                else if (wName == "Bushstroke-Camo")
                {
                    SetWeaponComponents(player, "Bushstroke-Camo");
                }

                if (type == "Weapon")
                {
                    if (wType == "Primary")
                    {
                        string primWeapon = (string)Characters.GetCharacterWeapon(player, "PrimaryWeapon");

                        if (primWeapon == "None")
                        {
                            player.GiveWeapon(wHash, 0, true);
                            Characters.SetCharacterWeapon(player, "PrimaryWeapon", wName);
                            Characters.SetCharacterWeapon(player, "PrimaryAmmo", 0);
                            SetWeaponComponents(player, wName);
                            HUDHandler.SendNotification(player, 2, 2500, $"{wName} erfolgreich ausgerüstet.");
                            return;
                        }
                        else if (primWeapon == wName)
                        {
                            int wAmmoAmount = (int)Characters.GetCharacterWeapon(player, "PrimaryAmmo");
                            float invWeight = CharactersInventory.GetCharacterItemWeight(charId, "inventory");
                            float backpackWeight = CharactersInventory.GetCharacterItemWeight(charId, "backpack");
                            float bigWeight = invWeight + backpackWeight;
                            float itemWeight = ServerItems.GetItemWeight($"{ammoWName}");
                            float multiWeight = itemWeight * wAmmoAmount;
                            float finalWeight = bigWeight + multiWeight;
                            float helpWeight = 20f + Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId));
                            bool inBackpack = false;

                            if (invWeight + multiWeight > 20f && backpackWeight + multiWeight > Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId))) { HUDHandler.SendNotification(player, 3, 2500, "Nicht genügend Platz."); return; }

                            if (wAmmoAmount >= 1 && ammoWName != "None" && finalWeight <= helpWeight)
                            {
                                if (invWeight + multiWeight <= 20f) { CharactersInventory.AddCharacterItem(charId, $"{ammoWName}", wAmmoAmount, "inventory"); inBackpack = false; } else { inBackpack = true; }
                                if (backpackWeight + multiWeight <= Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId)) && inBackpack == true) { CharactersInventory.AddCharacterItem(charId, $"{ammoWName}", wAmmoAmount, "backpack"); }
                            }

                            if (finalWeight <= helpWeight)
                            {
                                HUDHandler.SendNotification(player, 3, 2500, $"{wName} erfolgreich abgelegt.");
                                Characters.SetCharacterWeapon(player, "PrimaryWeapon", "None");
                                Characters.SetCharacterWeapon(player, "PrimaryAmmo", 0);
                                player.RemoveWeapon(wHash);
                            }
                        }
                        else
                        {
                            HUDHandler.SendNotification(player, 3, 2500, "Du musst zuerst deine Hauptwaffe ablegen bevor du eine neue anlegen kannst.");
                        }
                    }
                    else if (wType == "Fist")
                    {
                        string fistWeapon = (string)Characters.GetCharacterWeapon(player, "FistWeapon");
                        if (fistWeapon == "None")
                        {
                            player.GiveWeapon(wHash, 0, false);
                            Characters.SetCharacterWeapon(player, "FistWeapon", wName);
                            Characters.SetCharacterWeapon(player, "FistWeaponAmmo", 0);
                            HUDHandler.SendNotification(player, 2, 2500, $"{wName} erfolgreich ausgerüstet.");
                        }
                        else if (fistWeapon == wName)
                        {
                            float curWeight = CharactersInventory.GetCharacterItemWeight(charId, "inventory") + CharactersInventory.GetCharacterItemWeight(charId, "backpack");
                            float maxWeight = 20f + Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId));
                            if (curWeight < maxWeight) { Characters.SetCharacterWeapon(player, "FistWeapon", "None"); Characters.SetCharacterWeapon(player, "FistWeaponAmmo", 0); player.RemoveWeapon(wHash); HUDHandler.SendNotification(player, 3, 2500, $"{wName} erfolgreich abgelegt."); }
                            else { HUDHandler.SendNotification(player, 3, 2500, "Du hast nicht genügend Platz."); }
                        }
                        else
                        {
                            HUDHandler.SendNotification(player, 3, 2500, "Du musst zuerst deine Schlagwaffe ablegen bevor du eine neue anlegen kannst.");
                        }
                    }
                    else if (wType == "Secondary")
                    {
                        string secondaryWeapon = (string)Characters.GetCharacterWeapon(player, "SecondaryWeapon");
                        string secondaryWeapon2 = (string)Characters.GetCharacterWeapon(player, "SecondaryWeapon2");

                        if (secondaryWeapon == "None")
                        {
                            if (secondaryWeapon2 == wName)
                            {
                                int ammoAmount = (int)Characters.GetCharacterWeapon(player, "SecondaryAmmo2");
                                float invWeight = CharactersInventory.GetCharacterItemWeight(charId, "inventory");
                                float backpackWeight = CharactersInventory.GetCharacterItemWeight(charId, "backpack");
                                float bigWeight = invWeight + backpackWeight;
                                float itemWeight = ServerItems.GetItemWeight($"{ammoWName}");
                                float multiWeight = itemWeight * ammoAmount;
                                float finalWeight = bigWeight + multiWeight;
                                float helpWeight = 20f + Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId));
                                bool inBackpack = false;
                                if (invWeight + multiWeight > 20f && backpackWeight + multiWeight > Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId))) { HUDHandler.SendNotification(player, 3, 2500, "Nicht genügend Platz."); return; }

                                if (ammoAmount >= 1 && ammoWName != "None" && finalWeight <= helpWeight)
                                {
                                    if (invWeight + multiWeight <= 20f) { CharactersInventory.AddCharacterItem(charId, $"{ammoWName}", ammoAmount, "inventory"); inBackpack = false; } else { inBackpack = true; }
                                    if (backpackWeight + multiWeight <= Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId)) && inBackpack == true) { CharactersInventory.AddCharacterItem(charId, $"{ammoWName}", ammoAmount, "backpack"); }
                                }

                                if (finalWeight <= helpWeight)
                                {
                                    HUDHandler.SendNotification(player, 1, 2500, $"{wName} erfolgreich abgelegt.");
                                    Characters.SetCharacterWeapon(player, "SecondaryWeapon2", "None");
                                    Characters.SetCharacterWeapon(player, "SecondaryAmmo2", "None");
                                    player.RemoveWeapon(wHash);
                                }
                            }
                            else
                            {
                                player.GiveWeapon(wHash, 0, true);
                                Characters.SetCharacterWeapon(player, "SecondaryWeapon", wName);
                                Characters.SetCharacterWeapon(player, "SecondaryAmmo", 0);
                                SetWeaponComponents(player, wName);
                                HUDHandler.SendNotification(player, 1, 2500, $"{wName} erfolgreich ausgerüstet.");
                            }
                        }
                        else if (secondaryWeapon == wName)
                        {
                            int ammoAmount = (int)Characters.GetCharacterWeapon(player, "SecondaryAmmo");
                            float invWeight = CharactersInventory.GetCharacterItemWeight(charId, "inventory");
                            float backpackWeight = CharactersInventory.GetCharacterItemWeight(charId, "backpack");
                            float bigWeight = invWeight + backpackWeight;
                            float itemWeight = ServerItems.GetItemWeight($"{ammoWName}");
                            float multiWeight = itemWeight * ammoAmount;
                            float finalWeight = bigWeight + multiWeight;
                            float helpWeight = 20f + Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId));
                            bool inBackpack = false;
                            if (invWeight + multiWeight > 20f && backpackWeight + multiWeight > Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId))) { HUDHandler.SendNotification(player, 3, 2500, "Nicht genügend Platz."); return; }
                            if (ammoAmount >= 1 && ammoWName != "None" && finalWeight <= helpWeight)
                            {
                                if (invWeight + multiWeight <= 20f) { CharactersInventory.AddCharacterItem(charId, $"{ammoWName}", ammoAmount, "inventory"); inBackpack = false; } else { inBackpack = true; }
                                if (backpackWeight + multiWeight <= Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId)) && inBackpack == true) { CharactersInventory.AddCharacterItem(charId, $"{ammoWName}", ammoAmount, "backpack"); }
                            }

                            if (finalWeight <= helpWeight)
                            {
                                HUDHandler.SendNotification(player, 1, 2500, $"{wName} erfolgreich abgelegt.");
                                Characters.SetCharacterWeapon(player, "SecondaryWeapon", "None");
                                Characters.SetCharacterWeapon(player, "SecondaryAmmo", 0);
                                player.RemoveWeapon(wHash);
                            }
                        }
                        else
                        {
                            if (secondaryWeapon2 == "None")
                            {
                                player.GiveWeapon(wHash, 0, true);
                                Characters.SetCharacterWeapon(player, "SecondaryWeapon2", wName);
                                Characters.SetCharacterWeapon(player, "SecondaryAmmo2", 0);
                                SetWeaponComponents(player, wName);
                                HUDHandler.SendNotification(player, 1, 2500, $"{wName} erfolgreich ausgerüstet.");
                            }
                            else if (secondaryWeapon2 == wName)
                            {
                                int ammoAmount = (int)Characters.GetCharacterWeapon(player, "SecondaryAmmo2");
                                float invWeight = CharactersInventory.GetCharacterItemWeight(charId, "inventory");
                                float backpackWeight = CharactersInventory.GetCharacterItemWeight(charId, "backpack");
                                float bigWeight = invWeight + backpackWeight;
                                float itemWeight = ServerItems.GetItemWeight($"{ammoWName}");
                                float multiWeight = itemWeight * ammoAmount;
                                float finalWeight = bigWeight + multiWeight;
                                float helpWeight = 20f + Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId));
                                bool inBackpack = false;

                                if (ammoAmount >= 1 && ammoWName != "None" && finalWeight <= helpWeight)
                                {
                                    if (invWeight + multiWeight <= 20f) { CharactersInventory.AddCharacterItem(charId, $"{ammoWName}", ammoAmount, "inventory"); inBackpack = false; } else { inBackpack = true; }
                                    if (backpackWeight + multiWeight <= Characters.GetCharacterBackpackSize(Characters.GetCharacterBackpack(charId)) && inBackpack == true) { CharactersInventory.AddCharacterItem(charId, $"{ammoWName}", ammoAmount, "backpack"); }
                                }

                                if (finalWeight <= helpWeight)
                                {
                                    HUDHandler.SendNotification(player, 1, 2500, $"{wName} erfolgreich abgelegt.");
                                    Characters.SetCharacterWeapon(player, "SecondaryWeapon2", "None");
                                    Characters.SetCharacterWeapon(player, "SecondaryAmmo2", 0);
                                    player.RemoveWeapon(wHash);
                                }
                            }
                            else { HUDHandler.SendNotification(player, 3, 2500, "Du musst zuerst deine Sekundärwaffe ablegen bevor du eine neue anlegen kannst."); }
                        }
                    }
                }
                else if (type == "Ammo")
                {
                    if (wType == "Primary")
                    {
                        string primaryWeapon = (string)Characters.GetCharacterWeapon(player, "PrimaryWeapon");
                        if (primaryWeapon == "None") { HUDHandler.SendNotification(player, 3, 2500, "Du hast keine Primärwaffe angelegt."); }
                        else if (primaryWeapon == normalWName)
                        {
                            int newAmmo = (int)Characters.GetCharacterWeapon(player, "PrimaryAmmo") + amount;
                            player.GiveWeapon(wHash, newAmmo, true);
                            Characters.SetCharacterWeapon(player, "PrimaryAmmo", newAmmo);
                            HUDHandler.SendNotification(player, 1, 2500, $"Du hast {wName} in deine Waffe geladen.");

                            if (CharactersInventory.ExistCharacterItem(charId, $"{wName}", fromContainer))
                            {
                                CharactersInventory.RemoveCharacterItemAmount(charId, $"{wName}", amount, fromContainer);
                            }
                        }
                        else
                        {
                            HUDHandler.SendNotification(player, 3, 2500, "Die Munition passt nicht in deine Waffe.");
                        }
                    }
                    else if (wType == "Secondary")
                    {
                        string secondaryWeapon = (string)Characters.GetCharacterWeapon(player, "SecondaryWeapon");
                        if (secondaryWeapon == "None") { HUDHandler.SendNotification(player, 3, 2500, "Du hast keine Sekundärwaffe angelegt."); }
                        else if (secondaryWeapon == normalWName)
                        {
                            int newAmmo = (int)Characters.GetCharacterWeapon(player, "SecondaryAmmo") + amount;
                            player.GiveWeapon(wHash, newAmmo, true);
                            Characters.SetCharacterWeapon(player, "SecondaryAmmo", newAmmo);
                            HUDHandler.SendNotification(player, 1, 2500, $"Du hast {wName} deine Waffe geladen.");

                            if (CharactersInventory.ExistCharacterItem(charId, $"{wName}", fromContainer))
                            {
                                CharactersInventory.RemoveCharacterItemAmount(charId, $"{wName}", amount, fromContainer);
                            }
                        }
                        else
                        {
                            string secondary2Weapon = (string)Characters.GetCharacterWeapon(player, "SecondaryWeapon2");
                            if (secondary2Weapon == "None") { HUDHandler.SendNotification(player, 3, 2500, "Du hast keine Sekundärwaffe angelegt."); }
                            else if (secondary2Weapon == normalWName)
                            {
                                int newAmmo = (int)Characters.GetCharacterWeapon(player, "SecondaryAmmo2") + amount;
                                player.GiveWeapon(wHash, newAmmo, true);
                                Characters.SetCharacterWeapon(player, "SecondaryAmmo2", newAmmo);
                                HUDHandler.SendNotification(player, 1, 2500, $"Du hast {wName} deine Waffe geladen.");

                                if (CharactersInventory.ExistCharacterItem(charId, $"{wName}", fromContainer))
                                {
                                    CharactersInventory.RemoveCharacterItemAmount(charId, $"{wName}", amount, fromContainer);
                                }
                            }
                            else
                            {
                                HUDHandler.SendNotification(player, 3, 2500, "Die Munition passt nicht in deine Waffe.");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static void SetWeaponComponents(IPlayer player, string wName)
        {
            if (player == null || !player.Exists) return;
            switch (wName)
            {
                case "Karabiner":
                    player.AddWeaponComponent(WeaponModel.CarbineRifle, 0x7BC4CDDC); //Flashlight
                    player.AddWeaponComponent(WeaponModel.CarbineRifle, 0xA0D89C42); //Scope
                    player.AddWeaponComponent(WeaponModel.CarbineRifle, 0xC164F53); //Grip
                    break;
                case "PistolMKII":
                    player.AddWeaponComponent(WeaponModel.PistolMkII, 0x85FEA109); //Ammo
                    player.AddWeaponComponent(WeaponModel.PistolMkII, 0x43FD595B); //Flashlight
                    break;
                case "SMGMKII":
                    player.AddWeaponComponent(WeaponModel.SMGMkII, 0x9FDB5652); // Holographic
                    player.AddWeaponComponent(WeaponModel.SMGMkII, 0xC867A07B); // Tactical Muzzle Brake
                    player.AddWeaponComponent(WeaponModel.SMGMkII, 0xA564D78B); // Heavy Barrel
                    break;
                case "Silencer":
                    player.AddWeaponComponent(WeaponModel.SMGMkII, 0xC304849A); // Silencer
                    player.AddWeaponComponent(WeaponModel.PistolMkII, 0x65EA7EBB); // Silencer
                    break;
                case "Bushstroke-Camo":
                    player.AddWeaponComponent(WeaponModel.SMGMkII, 0x3815A945); // Camo
                    player.AddWeaponComponent(WeaponModel.PistolMkII, 0x15F7A390); // Camo
                    break;
                case "Extended-Clip":
                    player.AddWeaponComponent(WeaponModel.SMGMkII, 0xB9835B2E); // Clip
                    player.AddWeaponComponent(WeaponModel.PistolMkII, 0x5ED6C128); // Clip
                    player.AddWeaponComponent(WeaponModel.PistolMkII, 0x85FEA109); //Ammo
                    player.AddWeaponComponent(WeaponModel.SMGMkII, 0x3A1BD6FA); //Ammo
                    break;
                case "Compensator":
                    player.AddWeaponComponent(WeaponModel.PistolMkII, 0x21E34793); // Comp
                    break;
                case "Mounted-Scope":
                    player.AddWeaponComponent(WeaponModel.SMGMkII, 0x9FDB5652); // Scope
                    player.AddWeaponComponent(WeaponModel.PistolMkII, 0x8ED4BB70); // Scope
                    break;
            }
        }

        public static WeaponModel GetWeaponModelByName(string wName)
        {
            WeaponModel wHash = 0;
            switch (wName)
            {
                case "Pistol": wHash = WeaponModel.Pistol; break;
                case "PistolMKII": wHash = WeaponModel.PistolMkII; break;
                case "Combat-Pistol": wHash = WeaponModel.CombatPistol; break;
                case "SNS-Pistol": wHash = WeaponModel.SNSPistol; break;
                case "50.Pistol": wHash = WeaponModel.HeavyPistol; break;
                case "Revolver": wHash = WeaponModel.HeavyRevolver; break;
                case "Heavy-Pistol": wHash = WeaponModel.HeavyPistol; break;
                case "Gusenberg": wHash = WeaponModel.GusenbergSweeper; break;
                case "Compact": wHash = WeaponModel.CompactRifle; break;
                case "Tazer": wHash = WeaponModel.StunGun; break;
                case "Leuchtpistole": wHash = WeaponModel.FlareGun; break;
                case "Combat-PDW": wHash = WeaponModel.CombatPDW; break;
                case "Karabiner": wHash = WeaponModel.CarbineRifle; break;
                case "SMG": wHash = WeaponModel.SMG; break;
                case "Micro-SMG": wHash = WeaponModel.MicroSMG; break;
                case "Assault-Rifle": wHash = WeaponModel.AssaultRifle; break;
                case "Schrotflinte": wHash = WeaponModel.PumpShotgun; break;
                case "Abgesägte": wHash = WeaponModel.SawedOffShotgun; break;
                case "Schlagstock": wHash = WeaponModel.Nightstick; break;
                case "Messer": wHash = WeaponModel.Knife; break;
                case "Brecheisen": wHash = WeaponModel.Crowbar; break;
                case "Baseballschlaeger": wHash = WeaponModel.BaseballBat; break;
                case "Dolch": wHash = WeaponModel.AntiqueCavalryDagger; break;
                case "Hammer": wHash = WeaponModel.Hammer; break;
                case "Axt": wHash = WeaponModel.Hatchet; break;
                case "Machete": wHash = WeaponModel.Machete; break;
                case "Springmesser": wHash = WeaponModel.Switchblade; break;
                case "Schlagring": wHash = WeaponModel.BrassKnuckles; break;
                case "Taschenlampe": wHash = WeaponModel.Flashlight; break;
                case "Golfschlaeger": wHash = WeaponModel.GolfClub; break;
                case "SMGMKII": wHash = WeaponModel.SMGMkII; break;
            }
            return wHash;
        }
    }
}