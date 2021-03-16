using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.Services
{
    public interface IChatAnalyzerService
    {
        int TotalNumberOfMessages(IEnumerable<Message> messages);
        int TotalNumberOfWords(IEnumerable<Message> messages);
        int TotalNumberOfCharacters(IEnumerable<Message> messages);
        int TotalNumberOfMedia(IEnumerable<Message> messages);
        int TotalNumberOfEmojis(IEnumerable<Message> messages);
    }

    public class ChatAnalyzerService : IChatAnalyzerService
    {
        private IEmojiService emojiService;

        public ChatAnalyzerService(IEmojiService emojiService)
        {
            this.emojiService = emojiService;
        }

        public int TotalNumberOfMessages(IEnumerable<Message> messages)
        {
            return messages.Count();
        }

        public int TotalNumberOfWords(IEnumerable<Message> messages)
        {
            return messages.Sum(o => o.Text.Count(c => c == ' ') + 1);
        }

        public int TotalNumberOfCharacters(IEnumerable<Message> messages)
        {
            return messages.Sum(o =>
            {
                return new StringInfo(o.Text).LengthInTextElements;
            });
        }

        public int TotalNumberOfMedia(IEnumerable<Message> messages)
        {
            return messages.Count(o => o.Text == "<Media omitted>");
        }

        public int TotalNumberOfEmojis(IEnumerable<Message> messages)
        {
            return messages.Sum(o =>
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
            });
        }
    }
}
