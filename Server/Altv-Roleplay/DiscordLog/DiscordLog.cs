using System;
using Discord.Webhook;
using Discord.Webhook.HookRequest;

namespace Altv_Roleplay.Handler
{
    class DiscordLog
    {
        internal static void SendEmbed(string type, string nickname, string text)
        {
            DiscordWebhook hook = new DiscordWebhook();

            switch (type)
            {
                case "adminmenu":
                    hook.HookUrl = "https://discord.com/api/webhooks/884229225702367242/Wo8vrBaFBC5S9BhSm78kZnNOH3xfzgqEBfcy750LbwnzjsawU6YI2qV9ixEfE6VzWEZb";
                    break;
                case "frak":
                    hook.HookUrl = "https://discord.com/api/webhooks/884229368963018772/M1E4eHgh4_D9FWD5IQ49Nth6Eox4u5xpHed0oppYkbm_iZjHmlLJdFk13Z5LhBrWTQPV";
                    break;
                case "death":
                    hook.HookUrl = "https://discord.com/api/webhooks/884229458448498738/zGMvmth16y9DkRkIByyCrkYzWRL4p_ZxxN21GwtI3WEVDOp2dPn6oa0-YpUoKnKj3jG2";
                    break;
                default:
                    hook.HookUrl = "https://discord.com/api/webhooks/862737976451006484/IRRFfWuKl1yLarkQ7seJuNjjKgb3EZheXl0ddOnryxvw3fN3qBmP6jLkmIdWJbhVB5BQ";
                    break;
            }

            if (hook.HookUrl == "YOUR_WEBHOOK") return; //Hier YOUR_WEBHOOK nicht ersetzen

            DiscordHookBuilder builder = DiscordHookBuilder.Create(Nickname: nickname, AvatarUrl: "https://abload.de/img/logo_no-bgynjfy.png?width=519&height=519");

            DiscordEmbed embed = new DiscordEmbed(
                            Title: "Log",
                            Description: text,
                            Color: 0xf54242,
                            FooterText: "Log",
                            FooterIconUrl: "https://abload.de/img/logo_no-bgynjfy.png?width=519&height=519");
            builder.Embeds.Add(embed);

            DiscordHook HookMessage = builder.Build();
            hook.Hook(HookMessage);
        }
    }
}
