using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WappChatAnalyzer.Domain;

namespace WappChatAnalyzer.Services
{
    public interface IStatisticFuncsService
    {
        StatisticFunc TotalNumberOfMessages();
        StatisticFunc TotalNumberOfWords();
        StatisticFunc TotalNumberOfCharacters();
        StatisticFunc TotalNumberOfMedia();
        StatisticFunc TotalNumberOfEmojis();
        StatisticFunc TotalNumberOfEmoji(string emojiCodePoints);
        StatisticFunc TotalNumberOfCustom(CustomStatistic customStatistic);
    }

    public class StatisticFuncsService : IStatisticFuncsService
    {
        private IEmojiService emojiService;

        public StatisticFuncsService(IEmojiService emojiService)
        {
            this.emojiService = emojiService;
        }

        public StatisticFunc TotalNumberOfMessages()
        {
            return new StatisticFunc()
            {
                Name = "Messages",
                Func = m => m.Count(o => !o.IsMedia)
            };
        }

        public StatisticFunc TotalNumberOfWords()
        {
            return new StatisticFunc()
            {
                Name = "Words",
                Func = m => m.Where(o => !o.IsMedia).Sum(o => o.Text.Count(c => c == ' ') + 1)
            };
        }

        public StatisticFunc TotalNumberOfCharacters()
        {
            return new StatisticFunc()
            {
                Name = "Characters",
                Func = m => m.Where(o => !o.IsMedia).Sum(o => new StringInfo(o.Text).LengthInTextElements)
            };
        }

        public StatisticFunc TotalNumberOfMedia()
        {
            return new StatisticFunc()
            {
                Name = "Media",
                Func = m => m.Count(o => o.IsMedia)
            };
        }

        public StatisticFunc TotalNumberOfEmojis()
        {
            return new StatisticFunc()
            {
                Name = "Emojis",
                Func = m => m.Where(o => !o.IsMedia).Sum(o =>
                {
                    var emojiCount = 0;
                    var enumerator = StringInfo.GetTextElementEnumerator(o.Text);
                    while (enumerator.MoveNext())
                    {
                        var textElement = enumerator.GetTextElement();
                        if (emojiService.IsEmoji(textElement))
                            emojiCount++;
                    }

                    return emojiCount;
                })
            };
        }

        public StatisticFunc TotalNumberOfEmoji(string emojiCodePoints)
        {
            return new StatisticFunc()
            {
                Name = "Emoji/" + emojiCodePoints,
                Func = m => emojiService.CountEmojiInMessages(m, emojiCodePoints)
            };
        }

        public StatisticFunc TotalNumberOfCustom(CustomStatistic customStatistic)
        {
            var regex = new Regex(customStatistic.Regex, RegexOptions.IgnoreCase);

            return new StatisticFunc()
            {
                Name = customStatistic.Name,
                Func = m => m.Sum(o => regex.Matches(o.Text).Count)
            };
        }
    }

    public class StatisticFunc
    {
        public string Name { get; set; }
        public Func<IEnumerable<Message>, int> Func { get; set; }
    }
}
