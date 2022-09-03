//faction stuff thingys
//data = Characters.GetCharacterFactionInformations(charId);

[AsyncClientEvent("Server:Raycast:showIdcard")]
public void showIdCard(IPlayer player, IPlayer targetPlayer)
{
    try
    {
            if (player == null || targetPlayer == null || !player.Exists || !targetPlayer.Exists) return;
            int charId = (int)player.GetCharacterMetaId();
            int targetId = (int)targetPlayer.GetCharacterMetaId();
            if (charId <= 0 || targetId <= 0) return;
            if (Characters.GetCharacterAccState(charId) <= 0) return;
            if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 5000, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
            var data = "[]";

            data = Characters.GetCharacterInformations(charId);
            if (data == null || data == "[]") return;
            player.EmitLocked("Client:IdentityCard:showIdentityCard", "perso", data);
            targetPlayer.EmitLocked("Client:IdentityCard:showIdentityCard", "perso", data);
            InventoryHandler.InventoryAnimation(player, "give", 0);
        }
        catch (Exception e)
        {
        Alt.Log($"{e}");
    }
}
//ToDo
[AsyncClientEvent("Server:Raycast:showDriversLic")]
public void showDriversLic(IPlayer player, IPlayer targetPlayer)
{
    try
    {
            if (player == null || targetPlayer == null || !player.Exists || !targetPlayer.Exists) return;
            int charId = (int)player.GetCharacterMetaId();
            int targetId = (int)targetPlayer.GetCharacterMetaId();
            if (charId <= 0 || targetId <= 0) return;
            if (Characters.GetCharacterAccState(charId) <= 0) return;
            if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 5000, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
            var data = "[]";

            data = Characters.GetCharacterInformations(charId);
            if (data == null || data == "[]") return;
            player.EmitLocked("Client:IdentityCard:showDriversLic", "drivers", data);
            targetPlayer.EmitLocked("Client:IdentityCard:showDriversLic", "drivers", data);
            InventoryHandler.InventoryAnimation(player, "give", 0);
        }
        catch (Exception e)
        {
        Alt.Log($"{e}");
    }
}
//ToDo
[AsyncClientEvent("Server:Raycast:showWepLic")]
public void showWepLic(IPlayer player, IPlayer targetPlayer)
{
    try
    {
            if (player == null || targetPlayer == null || !player.Exists || !targetPlayer.Exists) return;
            int charId = (int)player.GetCharacterMetaId();
            int targetId = (int)targetPlayer.GetCharacterMetaId();
            if (charId <= 0 || targetId <= 0) return;
            if (Characters.GetCharacterAccState(charId) <= 0) return;
            if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 5000, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
            var data = "[]";

            data = Characters.GetCharacterInformations(charId);
            if (data == null || data == "[]") return;
            player.EmitLocked("Client:IdentityCard:showWepLic", "wep", data);
            targetPlayer.EmitLocked("Client:IdentityCard:showWepLic", "wep", data);
            InventoryHandler.InventoryAnimation(player, "give", 0);
        }
        catch (Exception e)
        {
        Alt.Log($"{e}");
    }
}
[AsyncClientEvent("Server:Raycast:showFactionCard")]
public void showFactionCard(IPlayer player, IPlayer targetPlayer)
{
    try
    {
            if (player == null || targetPlayer == null || !player.Exists || !targetPlayer.Exists) return;
            int charId = (int)player.GetCharacterMetaId();
            int targetId = (int)targetPlayer.GetCharacterMetaId();
            if (charId <= 0 || targetId <= 0) return;
            if (Characters.GetCharacterAccState(charId) <= 0) return;
            if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 5000, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
            var data = "[]";

            data = Characters.GetCharacterFactionInformations(charId);
            if (data == null || data == "[]") return;
            player.EmitLocked("Client:IdentityCard:showFactionCard", "faction", data);
            targetPlayer.EmitLocked("Client:IdentityCard:showFactionCard", "faction", data);
            InventoryHandler.InventoryAnimation(player, "give", 0);
        }
        catch (Exception e)
        {
        Alt.Log($"{e}");
    }
}
[AsyncClientEvent("Server:Raycast:showPoliceCard")]
public void showPoliceCard(IPlayer player, IPlayer targetPlayer)
{
    try
    {
            if (player == null || targetPlayer == null || !player.Exists || !targetPlayer.Exists) return;
            int charId = (int)player.GetCharacterMetaId();
            int targetId = (int)targetPlayer.GetCharacterMetaId();
            if (charId <= 0 || targetId <= 0) return;
            if (Characters.GetCharacterAccState(charId) <= 0) return;
            if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 5000, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
            var data = "[]";

            data = Characters.GetCharacterFactionInformations(charId);
            if (data == null || data == "[]") return;
            player.EmitLocked("Client:IdentityCard:showPoliceCard", "police", data);
            targetPlayer.EmitLocked("Client:IdentityCard:showPoliceCard", "police", data);
            InventoryHandler.InventoryAnimation(player, "give", 0);
        }
        catch (Exception e)
        {
        Alt.Log($"{e}");
    }
}
[AsyncClientEvent("Server:Raycast:showFIBCard")]
public void showFIBCard(IPlayer player, IPlayer targetPlayer)
{
    try
    {
            if (player == null || targetPlayer == null || !player.Exists || !targetPlayer.Exists) return;
            int charId = (int)player.GetCharacterMetaId();
            int targetId = (int)targetPlayer.GetCharacterMetaId();
            if (charId <= 0 || targetId <= 0) return;
            if (Characters.GetCharacterAccState(charId) <= 0) return;
            if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 5000, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
            var data = "[]";

            data = Characters.GetCharacterFactionInformations(charId);
            if (data == null || data == "[]") return;
            player.EmitLocked("Client:IdentityCard:showFIBCard", "fib", data);
            targetPlayer.EmitLocked("Client:IdentityCard:showFIBCard", "fib", data);
            InventoryHandler.InventoryAnimation(player, "give", 0);
        }
        catch (Exception e)
        {
        Alt.Log($"{e}");
    }
}
[AsyncClientEvent("Server:Raycast:showLSMDCard")]
public void showLSMDCard(IPlayer player, IPlayer targetPlayer)
{
    try
    {
            if (player == null || targetPlayer == null || !player.Exists || !targetPlayer.Exists) return;
            int charId = (int)player.GetCharacterMetaId();
            int targetId = (int)targetPlayer.GetCharacterMetaId();
            if (charId <= 0 || targetId <= 0) return;
            if (Characters.GetCharacterAccState(charId) <= 0) return;
            if (player.HasPlayerHandcuffs() || player.HasPlayerRopeCuffs()) { HUDHandler.SendNotification(player, 3, 5000, "Wie willst du das mit Handschellen/Fesseln machen?"); return; }
            var data = "[]";

            data = Characters.GetCharacterFactionInformations(charId);
            if (data == null || data == "[]") return;
            player.EmitLocked("Client:IdentityCard:showLSMDCard", "lsmd", data);
            targetPlayer.EmitLocked("Client:IdentityCard:showLSMDCard", "lsmd", data);
            InventoryHandler.InventoryAnimation(player, "give", 0);
        }
        catch (Exception e)
        {
        Alt.Log($"{e}");
    }
}

