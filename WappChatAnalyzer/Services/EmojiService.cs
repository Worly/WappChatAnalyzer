using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WappChatAnalyzer.Services
{
    public interface IEmojiService
    {
        bool IsEmoji(string textElement);
        bool TryGetEmoji(string textElement, out Emoji emoji);
        Emoji GetEmojiByCodePoints(string codePoints);
    }

    public class EmojiService : IEmojiService
    {
        private Dictionary<string, Emoji> emojis { get; set; } = new Dictionary<string, Emoji>();

        private Dictionary<string, string> nameFixes = new Dictionary<string, string>()
        {
            { "knocked-out face", "dizzy face" },
            { "water pistol", "pistol" }
        };

        public EmojiService()
        {
            string allEmojis;
            using (WebClient client = new WebClient())
            {
                allEmojis = client.DownloadString("https://www.unicode.org/Public/emoji/13.1/emoji-test.txt");
            }

            int? codePointsWidth = null, statusWidth = null;

            foreach(var line in allEmojis.Split('\n'))
            {
                if (line == "" || line[0] == '#')
                    continue;

                if (codePointsWidth == null || statusWidth == null)
                {
                    codePointsWidth = line.IndexOf(';');
                    statusWidth = line.IndexOf('#', codePointsWidth.Value) - codePointsWidth;
                }

                var codePoints = line.Substring(0, codePointsWidth.Value).Trim();
                var status = line.Substring(codePointsWidth.Value + 2, statusWidth.Value - 2).Trim();

                var indexOfHash = line.IndexOf('#');
                var indexOfE = line.IndexOf('E', indexOfHash);
                var indexOfSpace = line.IndexOf(' ', indexOfE);
                var name = line.Substring(indexOfSpace + 1);

                if (nameFixes.ContainsKey(name))
                    name = nameFixes[name];

                var newEmoji = new Emoji()
                {
                    CodePoints = codePoints,
                    Status = status,
                    Name = name
                };

                if (status != "fully-qualified")
                    newEmoji.FullyQualified = emojis.FirstOrDefault(o => o.Value.Name == name && o.Value.Status == "fully-qualified").Value;

                if (newEmoji.FullyQualified == null)
                    newEmoji.FullyQualified = newEmoji;

                emojis.Add(codePoints, newEmoji);
            }
        }

        public bool IsEmoji(string textElement)
        {
            var codePoints = textElement.EnumerateRunes().Select(o => o.Value.ToString("X")).Aggregate((f, s) => f + " " + s).Trim();

            if (!emojis.TryGetValue(codePoints, out Emoji emoji))
                return false;

            return emoji.Status != "component";
        }

        public bool TryGetEmoji(string textElement, out Emoji emoji)
        {
            var codePoints = textElement.EnumerateRunes().Select(o => o.Value.ToString("X")).Aggregate((f, s) => f + " " + s).Trim();

            if (!emojis.TryGetValue(codePoints, out emoji))
                return false;

            return emoji.Status != "component";
        }

        public Emoji GetEmojiByCodePoints(string codePoints)
        {
            return emojis[codePoints].FullyQualified;
        }
    }

    public class Emoji
    {
        public string CodePoints { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }

        public Emoji FullyQualified { get; set; }
    }
}
