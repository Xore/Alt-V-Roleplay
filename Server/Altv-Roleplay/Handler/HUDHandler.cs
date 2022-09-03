﻿using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;
using System;

namespace Altv_Roleplay.Handler
{
    class HUDHandler : IScript
    {

        public static void CreateHUDBrowser(IPlayer client)
        {
            if (client == null || !client.Exists) return;
            client.EmitLocked("Client:HUD:CreateCEF", Characters.GetCharacterHunger(User.GetPlayerOnline(client)), Characters.GetCharacterThirst(User.GetPlayerOnline(client)), CharactersInventory.GetCharacterItemAmount(User.GetPlayerOnline(client), "Bargeld", "brieftasche"));
        }

        [ScriptEvent(ScriptEventType.PlayerEnterVehicle)]
        public void OnPlayerEnterVehicle_Handler(IVehicle vehicle, IPlayer client, byte seat)
        {
            try
            {
                if (client == null || !client.Exists) return;
                client.EmitLocked("Client:HUD:updateHUDPosInVeh", true);
                client.EmitLocked("Client:SPEEDO:updateFuel", ServerVehicles.GetVehicleFuel(vehicle), ServerVehicles.GetVehicleKM(vehicle));
                client.EmitLocked("Client:HUD:GetDistanceForVehicleKM");
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [ScriptEvent(ScriptEventType.PlayerLeaveVehicle)]
        public void OnPlayerLeaveVehicle_Handler(IVehicle vehicle, IPlayer client, byte seat)
        {
            try
            {
                if (client == null || !client.Exists) return;
                client.EmitLocked("Client:HUD:updateHUDPosInVeh", false, 0, 0);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static void SendInformationToVehicleHUD(IPlayer player)
        {
            if (player == null || !player.Exists) return;
            IVehicle Veh = player.Vehicle;
            if (!Veh.Exists) return;
            long vehID = Veh.GetVehicleId();
            if (vehID == 0) return;
            player.EmitLocked("Client:HUD:SetPlayerHUDVehicleInfos", ServerVehicles.GetVehicleFuel(Veh), ServerVehicles.GetVehicleKM(Veh));
        }

        public static void SendProgress(IPlayer client, string message, string notytype, int time = 5000, string layout = "topCenter")
        {
            try
            {
                if (client == null || !client.Exists) return;
                client.EmitLocked("xpert:notification:send", message, notytype, time, layout);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static void SendNotification(IPlayer client, int type, int duration, string msg, int delay = 0) //1 Info | 2 Success | 3 Warning | 4 Error
        {
            try
            {
                if (client == null || !client.Exists) return;
                client.EmitLocked("Client:HUD:sendNotification", type, duration, msg, delay);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        [AsyncClientEvent("Server:Vehicle:UpdateVehicleKM")]
        public void UpdateVehicleKM(IPlayer player, float km)
        {
            //KM = bei 600 Meter = 600
            //600 / 1000 = 0,6   = 0,6km ?
            try
            {
                if (player == null || !player.Exists || km <= 0) return;
                if (!player.IsInVehicle || player.Vehicle == null) return;
                float fKM = km / 1000;
                fKM = fKM + ServerVehicles.GetVehicleKM(player.Vehicle);
                ServerVehicles.SetVehicleKM(player.Vehicle, fKM);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        internal static void SendNotification(ClassicPlayer client, int v1, int v2, string v3, int v4)
        {
            throw new NotImplementedException();
        }
    }
}
