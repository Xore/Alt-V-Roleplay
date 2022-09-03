using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Model;
using System;
using System.Threading.Tasks;

namespace Altv_Roleplay.Handler
{
    class RegisterHandler : IScript
    {
        [AsyncClientEvent("Server:Register:RegisterNewPlayer")]
        public async void RegisterNewPlayer(IPlayer player, string username, string email, string pass, string passrepeat)
        {
            if (player == null || !player.Exists) return;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass) || string.IsNullOrWhiteSpace(passrepeat))
            {
                player.EmitLocked("Client:Login:showError", "Eines der Felder wurde nicht ordnungsgemäß ausgefüllt.");
                return;
            }

            if (User.ExistPlayerName(username))
            {
                player.EmitLocked("Client:Login:showError", "Der eingegebene Benutzername ist bereits vergeben.");
                return;
            }

            if (User.ExistPlayerEmail(email))
            {
                player.EmitLocked("Client:Login:showError", "Die eingegebene E-Mail ist bereits vergeben.");
                return;
            }

            if (pass != passrepeat)
            {
                player.EmitLocked("Client:Login:showError", "Die eingegebenen Passwörter stimmen nicht überein.");
                return;
            }

            if (User.ExistPlayerSocialClub(player))
            {
                player.EmitLocked("Client:Login:showError", "Es existiert bereits ein Konto mit deiner Socialclub ID.");
                return;
            }

            player.EmitLocked("Client:Login:showArea", "login");

            await Task.Delay(new Random().Next(2000, 2500));
            if (User.ExistPlayerSocialClub(player) || User.ExistPlayerEmail(email) || User.ExistPlayerName(username)) return;
            User.CreatePlayerAccount(player, username, email, pass);
        }

        [AsyncClientEvent("Server:Whitelist:success")]
        public async void WhitelistSuccess(IPlayer player)
        {
            if (player == null || !player.Exists) return;

            User.SetPlayerWhitelistState(User.GetPlayerAccountId(player), true);

            player.EmitLocked("Client:Login:showArea", "login");

            await Task.Delay(new Random().Next(2000, 2500));
        }

        [AsyncClientEvent("Server:Whitelist:unsuccess")]
        public async void WhitelistUnsuccess(IPlayer player)
        {
            if (player == null || !player.Exists) return;

            User.SetPlayerWhitelistState(User.GetPlayerAccountId(player), false);
            User.IncreaseWhitelistTime(User.GetPlayerAccountId(player), 15);

            player.EmitLocked("Client:Login:showArea", "login");

            await Task.Delay(new Random().Next(2000, 2500));
        }
    }
}
