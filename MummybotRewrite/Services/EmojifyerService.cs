using Discord;
using Discord.Webhook;
using Discord.WebSocket;
using Mummybot.Attributes;
using System;
using System.Collections.Generic;
using System.Text;


namespace Mummybot.Services
{
    public class EmojifyerService
    {
        private DiscordSocketClient _client;
        private readonly Dictionary<char, string> emjiyerino;

        public EmojifyerService(DiscordSocketClient client)
        {
            _client = client;
            emjiyerino = Emojifyswag.LoadEmojis();

        }

        [Initialiser()]
        public void LoadEmojifier()
        {
            _client.MessageReceived += async (context) =>
            {
                if (context.Channel.Id == 248015464972681216 && context.Author.Id != _client.CurrentUser.Id && !(context.Author is IWebhookUser))
                {
                    await context.DeleteAsync();
                    List<string> messages = GetEmojiString(context.Content);

                    foreach (string message in messages)
                    {
                        IGuildUser user = context.Author as IGuildUser;
                        await (context.Channel as SocketTextChannel).SendMessageAsync(message);
                    }
                }
            };
        }
        public List<string> GetEmojiString(string message)
        {
            StringBuilder sb = new StringBuilder();

            string parm = message.ToLower();
            int lenght = " :regional_indicator_*: ".Length;
            int messagelenght = 0;
            List<string> messages = new List<string>();

            char[] chararrayparm = parm.ToCharArray();


            for (int i = 0; i < chararrayparm.Length; i++)
            {
                foreach (var p in emjiyerino)
                {
                    if (p.Key == chararrayparm[i])
                    {
                        sb.Append(" ");
                        sb.Append(p.Value);
                        messagelenght += lenght;
                        break;

                    }
                    if (messagelenght > 1800)
                    {
                        messages.Add(sb.ToString());
                        sb = new StringBuilder();
                        messagelenght = 0;
                    }

                }

            }

            messages.Add(sb.ToString());

            return messages;
        }
    }

    public class Emojifyswag
    {
        public static Dictionary<char, string> LoadEmojis()
        {
            Dictionary<char, string> charemoji = new Dictionary<char, string>
            {
                #region emojifyer dicionary
                { 'a', ":regional_indicator_a:" },
                { 'b', ":regional_indicator_b:" },
                { 'c', ":regional_indicator_c:" },
                { 'd', ":regional_indicator_d:" },
                { 'e', ":regional_indicator_e:" },
                { 'f', ":regional_indicator_f:" },
                { 'g', ":regional_indicator_g:" },
                { 'h', ":regional_indicator_h:" },
                { 'i', ":regional_indicator_i:" },
                { 'j', ":regional_indicator_j:" },
                { 'k', ":regional_indicator_k:" },
                { 'l', ":regional_indicator_l:" },
                { 'm', ":regional_indicator_m:" },
                { 'n', ":regional_indicator_n:" },
                { 'o', ":regional_indicator_o:" },
                { 'p', ":regional_indicator_p:" },
                { 'q', ":regional_indicator_q:" },
                { 'r', ":regional_indicator_r:" },
                { 's', ":regional_indicator_s:" },
                { 't', ":regional_indicator_t:" },
                { 'u', ":regional_indicator_u:" },
                { 'v', ":regional_indicator_v:" },
                { 'w', ":regional_indicator_w:" },
                { 'x', ":regional_indicator_x:" },
                { 'y', ":regional_indicator_y:" },
                { 'z', ":regional_indicator_z:" },

                { '1', ":one:" },
                { '2', ":two:" },
                { '3', ":three:" },
                { '4', ":four:" },
                { '5', ":five:" },
                { '6', ":six:" },
                { '7', ":seven:" },
                { '8', ":eight:" },
                { '9', ":nine:" },
                { '0', ":zero:" },
                { ' ', "    " }
            };

            #endregion
            return charemoji;
        }
    }

}