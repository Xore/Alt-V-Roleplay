using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Handler;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;
using System;

namespace Altv_Roleplay.Elevator
{
    public static class Positions
    {
        public static class Elevators
        {
            //MD
            public static readonly Position MDGARAGE = new Position(-419.222f, -344.95386f, 24.224854f);
            public static readonly Position MDEG = new Position(-436.12747f, -359.48572f, 34.941406f);
            public static readonly Position MDSW1 = new Position(-490.58902f, -327.42856f, 42.30481f);
            public static readonly Position MDSW2 = new Position(-490.4967f, -327.92966f, 69.50037f);
            public static readonly Position MDHELI = new Position(-443.72308f, -332.43958f, 78.16113f);
            //FIB
            public static readonly Position FIBGARAGE = new Position(109.97802f, -736.4176f, 33.121582f);
            public static readonly Position FIBEG = new Position(138.85715f, -762.94946f, 45.742065f);
            public static readonly Position FIBSW1 = new Position(136.18022f, -761.84174f, 242.14355f);
            public static readonly Position FIBHELI = new Position(141.32307f, -734.82196f, 262.8352f);
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
                    if (player.Position.IsInRange(Positions.Elevators.MDGARAGE, 1.5f) || player.Position.IsInRange(Positions.Elevators.MDEG, 1.5f) || player.Position.IsInRange(Positions.Elevators.MDSW1, 1.5f) || player.Position.IsInRange(Positions.Elevators.MDSW2, 1.5f) || player.Position.IsInRange(Positions.Elevators.MDHELI, 1.5f))
                    {
                        player.EmitLocked("Client:Elevator:openMD");
                    }
                    if (player.Position.IsInRange(Positions.Elevators.FIBGARAGE, 1.5f) || player.Position.IsInRange(Positions.Elevators.FIBEG, 1.5f) || player.Position.IsInRange(Positions.Elevators.FIBSW1, 1.5f) || player.Position.IsInRange(Positions.Elevators.FIBHELI, 1.5f))
                    {
                        player.EmitLocked("Client:Elevator:openFIB");
                    }
                }

            }

        }
        [AsyncClientEvent("Server:Elevator:MD")]
        public void StartMD(IPlayer player, int level)
        {
            try
            {
                if (player == null || !player.Exists || level <= 0) return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;

                switch (level)
                {
                    case 1:
                        player.Position = Positions.Elevators.MDGARAGE;
                        break;
                    case 2:
                        player.Position = Positions.Elevators.MDEG;
                        break;
                    case 3:
                        player.Position = Positions.Elevators.MDSW1;
                        break;
                    case 4:
                        player.Position = Positions.Elevators.MDSW2;
                        break;
                    case 5:
                        player.Position = Positions.Elevators.MDHELI;
                        break;
                }
                return;
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }
        [AsyncClientEvent("Server:Elevator:FIB")]
        public void StartFIB(IPlayer player, int level)
        {
            try
            {
                if (player == null || !player.Exists || level <= 0) return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;

                switch (level)
                {
                    case 1:
                        player.Position = Positions.Elevators.FIBGARAGE;
                        break;
                    case 2:
                        player.Position = Positions.Elevators.FIBEG;
                        break;
                    case 3:
                        player.Position = Positions.Elevators.FIBSW1;
                        break;
                    case 4:
                        player.Position = Positions.Elevators.FIBHELI;
                        break;
                }
                return;
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

    }
}