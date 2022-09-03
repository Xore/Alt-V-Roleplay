using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using System;

namespace Altv_Roleplay.Handler
{
    class ClothesRadialMenuHandler : IScript
    {
        [AsyncClientEvent("Server:ClothesRadial:GetClothesRadialItems")]
        public void GetAnimationItems(IPlayer player)
        {
            try
            {
                var interactHTML = "";
                interactHTML += "<li><p id='InteractionMenu-SelectedTitle'>Schließen</p></li><li class='interactitem' data-action='close' data-actionstring='Schließen'><img src='../utils/img/cancel.png'></li>";

                interactHTML += "<li class='interactitem' id='InteractionMenu-maske' data-action='maske' data-actionstring='Maske ausziehen'><img src='../utils/img/Maske.png'></li>";
                interactHTML += "<li class='interactitem' id='InteractionMenu-hut' data-action='hut' data-actionstring='Hut ausziehen'><img src='../utils/img/witch-hat.png'></li>";
                interactHTML += "<li class='interactitem' id='InteractionMenu-brille' data-action='brille' data-actionstring='Brille ausziehen'><img src='../utils/img/sun-glasses.png'></li>";
                interactHTML += "<li class='interactitem' id='InteractionMenu-tshirt' data-action='tshirt' data-actionstring='T-Shirt ausziehen'><img src='../utils/img/shirt.png'></li>";
                interactHTML += "<li class='interactitem' id='InteractionMenu-unterhemd' data-action='unterhemd' data-actionstring='Unterhemd ausziehen'><img src='../utils/img/undershirt.png'></li>";
                interactHTML += "<li class='interactitem' id='InteractionMenu-hose' data-action='hose' data-actionstring='Hose ausziehen'><img src='../utils/img/jeans.png'></li>";
                interactHTML += "<li class='interactitem' id='InteractionMenu-schuhe' data-action='schuhe' data-actionstring='Schuhe ausziehen'><img src='../utils/img/shoes.png'></li>";
                interactHTML += "<li class='interactitem' id='InteractionMenu-weste' data-action='weste' data-actionstring='Weste Ausziehen'><img src='../utils/img/Armor.png'></li>";
                interactHTML += "<li class='interactitem' id='InteractionMenu-kette' data-action='kette' data-actionstring='Kette ausziehen'><img src='../utils/img/necklace.png'></li>";

                player.EmitLocked("Client:ClothesRadial:SetMenuItems", interactHTML);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:ClothesRadial:SetNormalSkin")]
        public static void SetNormalSkin(IPlayer player, string action)
        {
            if (player == null || !player.Exists) return;
            int charid = User.GetPlayerOnline(player);
            int type = 0;
            string ClothesType = "Cloth";
            string TypeText = "none";
            if (charid == 0) return;

            if (action == "maske")
            {
                if (!player.HasData("HasMaskOn"))
                {
                    if (Characters.GetCharacterGender(charid) == false)
                    {
                        player.EmitLocked("Client:SpawnArea:setCharClothes", 1, 0, 0);
                    }
                    else
                    {
                        player.EmitLocked("Client:SpawnArea:setCharClothes", 1, 0, 0);
                    }
                    player.SetData("HasMaskOn", true);
                    player.EmitLocked("Client:Inventory:PlayAnimation", "mp_masks@standard_car@ds@", "put_on_mask", 800, 49, false);
                    return;
                }
                type = 1;
                TypeText = "Mask";
                player.DeleteData("HasMaskOn");
                player.EmitLocked("Client:Inventory:PlayAnimation", "missheist_agency2ahelmet", "take_off_helmet_stand", 800, 49, false);
            }
            else if (action == "hut")
            {
                if (!player.HasData("HasHatOn"))
                {
                    if (Characters.GetCharacterGender(charid) == false)
                    {
                        player.EmitLocked("Client:SpawnArea:setCharAccessory", 0, 11, 0);
                    }
                    else
                    {
                        player.EmitLocked("Client:SpawnArea:setCharAccessory", 0, 120, 0);
                    }
                    player.SetData("HasHatOn", true);
                    player.EmitLocked("Client:Inventory:PlayAnimation", "missheist_agency2ahelmet", "take_off_helmet_stand", 1200, 49, false);
                    return;
                }
                type = 0;
                TypeText = "Hat";
                ClothesType = "Prop";
                player.DeleteData("HasHatOn");
                player.EmitLocked("Client:Inventory:PlayAnimation", "missheist_agency2ahelmet", "take_off_helmet_stand", 1200, 49, false);
            }
            else if (action == "brille")
            {
                if (!player.HasData("HasGlassesOn"))
                {
                    if (Characters.GetCharacterGender(charid) == false)
                    {
                        player.EmitLocked("Client:SpawnArea:setCharAccessory", 1, 0, 0);
                    }
                    else
                    {
                        player.EmitLocked("Client:SpawnArea:setCharAccessory", 1, 15, 0);
                    }
                    player.SetData("HasGlassesOn", true);
                    player.EmitLocked("Client:Inventory:PlayAnimation", "clothingspecs", "take_off", 1400, 49, false);
                    return;
                }
                type = 1;
                TypeText = "Glass";
                ClothesType = "Prop";
                player.DeleteData("HasGlassesOn");
                player.EmitLocked("Client:Inventory:PlayAnimation", "clothingspecs", "take_off", 1400, 49, false);
            }
            else if (action == "tshirt")
            {
                if (!player.HasData("HasShirtOn"))
                {
                    if (Characters.GetCharacterGender(charid) == false)
                    {
                        player.EmitLocked("Client:SpawnArea:setCharClothes", 3, 15, 0);
                        player.EmitLocked("Client:SpawnArea:setCharClothes", 11, 15, 0);
                    }
                    else
                    {
                        player.EmitLocked("Client:SpawnArea:setCharClothes", 3, 15, 0);
                        player.EmitLocked("Client:SpawnArea:setCharClothes", 11, 15, 0);
                    }
                    player.SetData("HasShirtOn", true);
                    player.EmitLocked("Client:Inventory:PlayAnimation", "missmic4", "michael_tux_fidget", 1500, 49, false);
                    return;
                }
                type = 11;
                TypeText = "Top";
                player.DeleteData("HasShirtOn");
                player.EmitLocked("Client:SpawnArea:setCharClothes", 3, ServerClothes.GetClothesDraw(Characters.GetCharacterClothes(charid, "Torso"), Convert.ToInt32(Characters.GetCharacterGender(((ClassicPlayer)player).CharacterId))), ServerClothes.GetClothesTexture(Characters.GetCharacterClothes(charid, "Torso"), Convert.ToInt32(Characters.GetCharacterGender(((ClassicPlayer)player).CharacterId))));
                player.EmitLocked("Client:Inventory:PlayAnimation", "missmic4", "michael_tux_fidget", 1500, 49, false);
            }
            else if (action == "unterhemd")
            {
                if (!player.HasData("HasUndershirtOn"))
                {
                    if (Characters.GetCharacterGender(charid) == false)
                    {
                        player.EmitLocked("Client:SpawnArea:setCharClothes", 8, 15, 0);
                    }
                    else
                    {
                        player.EmitLocked("Client:SpawnArea:setCharClothes", 8, 15, 0);
                    }
                    player.SetData("HasUndershirtOn", true);
                    player.EmitLocked("Client:Inventory:PlayAnimation", "missmic4", "michael_tux_fidget", 1500, 49, false);
                    return;
                }
                type = 8;
                TypeText = "Undershirt";
                player.DeleteData("HasUndershirtOn");
                player.EmitLocked("Client:Inventory:PlayAnimation", "missmic4", "michael_tux_fidget", 1500, 49, false);
            }
            else if (action == "hose")
            {
                if (!player.HasData("HasPantsOn"))
                {
                    if (Characters.GetCharacterGender(charid) == false)
                    {
                        player.EmitLocked("Client:SpawnArea:setCharClothes", 4, 21, 0);
                    }
                    else
                    {
                        player.EmitLocked("Client:SpawnArea:setCharClothes", 4, 15, 0);
                    }
                    player.SetData("HasPantsOn", true);
                    player.EmitLocked("Client:Inventory:PlayAnimation", "re@construction", "out_of_breath", 1300, 15, false);
                    return;
                }
                type = 4;
                TypeText = "Leg";
                player.DeleteData("HasPantsOn");
                player.EmitLocked("Client:Inventory:PlayAnimation", "re@construction", "out_of_breath", 1300, 15, false);
            }
            else if (action == "schuhe")
            {
                if (!player.HasData("HasShoesOn"))
                {
                    if (Characters.GetCharacterGender(charid) == false)
                    {
                        player.EmitLocked("Client:SpawnArea:setCharClothes", 6, 34, 0);
                    }
                    else
                    {
                        player.EmitLocked("Client:SpawnArea:setCharClothes", 6, 35, 0);
                    }
                    player.SetData("HasShoesOn", true);
                    player.EmitLocked("Client:Inventory:PlayAnimation", "random@domestic", "pickup_low", 1200, 15, false);
                    return;
                }
                type = 6;
                TypeText = "Feet";
                player.DeleteData("HasShoesOn");
                player.EmitLocked("Client:Inventory:PlayAnimation", "random@domestic", "pickup_low", 1200, 15, false);
            }
            else if (action == "weste")
            {
                if (!player.HasData("HasArmorOn"))
                {
                    player.EmitLocked("Client:SpawnArea:setCharClothes", 9, 0, 0);
                    player.SetData("HasArmorOn", true);
                    player.EmitLocked("Client:Inventory:PlayAnimation", "clothingtie", "try_tie_negative_a", 1200, 49, false);
                    return;
                }
                type = 9;
                TypeText = "Armor";
                player.DeleteData("HasArmorOn");
                player.EmitLocked("Client:Inventory:PlayAnimation", "clothingtie", "try_tie_negative_a", 1200, 49, false);
            }
            else if (action == "kette")
            {
                if (!player.HasData("HasNecklaceOn"))
                {
                    player.EmitLocked("Client:SpawnArea:setCharClothes", 7, 0, 0);
                    player.SetData("HasNecklaceOn", true);
                    player.EmitLocked("Client:Inventory:PlayAnimation", "clothingtie", "try_tie_positive_a", 2100, 49, false);
                    return;
                }
                type = 7;
                TypeText = "Necklace";
                player.DeleteData("HasNecklaceOn");
                player.EmitLocked("Client:Inventory:PlayAnimation", "clothingtie", "try_tie_positive_a", 2100, 49, false);
            }


            if (TypeText == "none") return;
            if (ClothesType == "Prop")
            {
                player.EmitLocked("Client:SpawnArea:setCharAccessory", type, ServerClothes.GetClothesDraw(Characters.GetCharacterClothes(charid, TypeText), Convert.ToInt32(Characters.GetCharacterGender(((ClassicPlayer)player).CharacterId))), ServerClothes.GetClothesTexture(Characters.GetCharacterClothes(charid, TypeText), Convert.ToInt32(Characters.GetCharacterGender(((ClassicPlayer)player).CharacterId))));
                return;
            }
            player.EmitLocked("Client:SpawnArea:setCharClothes", type, ServerClothes.GetClothesDraw(Characters.GetCharacterClothes(charid, TypeText), Convert.ToInt32(Characters.GetCharacterGender(((ClassicPlayer)player).CharacterId))), ServerClothes.GetClothesTexture(Characters.GetCharacterClothes(charid, TypeText), Convert.ToInt32(Characters.GetCharacterGender(((ClassicPlayer)player).CharacterId))));
        }
    }
}