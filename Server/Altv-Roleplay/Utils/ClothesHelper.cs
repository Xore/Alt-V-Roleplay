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

namespace Altv_Roleplay.Utils
{
    class ClothesHelper : IScript
    {
        public static void LoadClothesHelper()
        {
            //ACLS CLOTHES
            //EntityStreamer.MarkerStreamer.Create(EntityStreamer.MarkerTypes.MarkerTypeHorizontalSplitArrowCircle, Constants.Positions.Clothes_ACLS, new System.Numerics.Vector3(1), color: new Rgba(255, 51, 51, 100), dimension: 0, streamRange: 20);
            //EntityStreamer.HelpTextStreamer.Create("Drücke E um deine Arbeitskleidung anzuziehen.", Constants.Positions.Clothes_ACLS, streamRange: 2);
        }
    }
    class KeyHandler : IScript
    {
        [AsyncClientEvent("Server:KeyHandler:PressE")]
        public void PressE(IPlayer player)
        {
            lock (player)
            {
                if (player == null || !player.Exists) return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 2500, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
                {

                    #region Native
                    if (player.Position.IsInRange(Constants.Positions.Clothes_FBI, 1.5f) && !player.IsInVehicle)
                    {
                        int factionId = ServerFactions.GetCharacterFactionId(charId);
                        if (factionId == 12)
                        {
                           player.EmitLocked("Client:FBI:openClotes");
                           Characters.SetCharacterArmor(charId, 100);
                        }
                        else
                        {
                            HUDHandler.SendNotification(player, 3, 2500, "Du darfst den Kleiderschrank nicht nutzen!");
                        }
                    }
                    if (player.Position.IsInRange(Constants.Positions.Clothes_Police, 1.5f) && !player.IsInVehicle)
                    {
                        int factionId = ServerFactions.GetCharacterFactionId(charId);
                        if (factionId == 2)
                        {
                            player.EmitLocked("Client:LSPD:openClotes");
                            Characters.SetCharacterArmor(charId, 100);
                        }
                        else
                        {
                            HUDHandler.SendNotification(player, 3, 2500, "Du darfst den Kleiderschrank nicht nutzen!");
                        }
                    }
                    if (player.Position.IsInRange(Constants.Positions.Clothes_Bennys, 1.5f) && !player.IsInVehicle)
                    {
                        int factionId = ServerFactions.GetCharacterFactionId(charId);
                        if (factionId == 14)
                        {
                            player.EmitLocked("Client:Bennys:openClotes");
                        }
                        else
                        {
                            HUDHandler.SendNotification(player, 3, 2500, "Du darfst den Kleiderschrank nicht nutzen!");
                        }
                    }
                    #endregion

                    if (player.Position.IsInRange(Constants.Positions.Clothes_Medic, 2.5f) && !player.IsInVehicle)
                    {
                        int factionId = ServerFactions.GetCharacterFactionId(charId);
                        if (factionId == 3)
                        {
                            if (!player.HasData("HasMedicClothesOn"))
                            {
                                if (Characters.GetCharacterGender(charId) == false)
                                {
                                    //MALE
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 3, 85, 0);
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 4, 129, 0);
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 6, 61, 0);
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 8, 15, 0);
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 11, 250, 0);
                                }
                                else if (Characters.GetCharacterGender(charId) == true)
                                {
                                    //FEMALE
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 3, 109, 0);//Torso
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 4, 136, 0);//Legs
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 6, 25, 0);//Shoes
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 8, 189, 0);//Undershirt
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 11, 258, 1);//Tops
                                }
                                HUDHandler.SendNotification(player, 1, 2500, "Du hast deine Arbeitsklamotten angezogen.");
                                player.SetData("HasMedicClothesOn", true);
                            }
                            else
                            {
                                player.DeleteData("HasMedicClothesOn");
                                Characters.SetCharacterCorrectClothes(player);
                                HUDHandler.SendNotification(player, 1, 2500, "Du hast deine Arbeitsklamotten ausgezogen.");
                            }
                        }
                        else
                        {
                            HUDHandler.SendNotification(player, 3, 2500, "Du darfst den Kleiderschrank nicht nutzen!");
                        }
                        return;
                    }

                    if (player.Position.IsInRange(Constants.Positions.Clothes_ACLS, 2f) && !player.IsInVehicle)
                    {
                        int factionId = ServerFactions.GetCharacterFactionId(charId);
                        if (factionId == 4)
                        {
                            if (!player.HasData("HasMechanicClothesOn"))
                            {
                                if (Characters.GetCharacterGender(charId) == false)
                                {
                                    //MALE
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 3, 96, 0);
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 4, 31, 0);
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 6, 60, 0);
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 8, 122, 0);
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 9, 63, 0);
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 11, 317, 0);
                                }
                                else if (Characters.GetCharacterGender(charId) == true)
                                {
                                    //FEMALE
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 3, 109, 0);//Torso
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 4, 101, 16);//Legs
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 6, 25, 0);//Shoes
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 8, 189, 0);//Undershirt
                                    player.EmitLocked("Client:SpawnArea:setCharClothes", 11, 258, 1);//Tops
                                }
                                HUDHandler.SendNotification(player, 1, 2500, "Du hast deine Arbeitsklamotten angezogen.");
                                player.SetData("HasMechanicClothesOn", true);
                            }
                            else
                            {
                                player.DeleteData("HasMechanicClothesOn");
                                Characters.SetCharacterCorrectClothes(player);
                                HUDHandler.SendNotification(player, 1, 2500, "Du hast deine Arbeitsklamotten ausgezogen.");
                            }
                        }
                        else
                        {
                            HUDHandler.SendNotification(player, 3, 2500, "Du darfst den Kleiderschrank nicht nutzen!");
                        }
                        return;
                    }
                }

            }

        }

    }

}