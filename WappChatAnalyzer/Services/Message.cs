using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WappChatAnalyzer.Services
{
    public class Message
    {
        public DateTime SentDateTime { get; set; }
        public DateTime SentDateNormalized { get => SentDateTime.AddHours(-7).Date; }
        public string Sender { get; set; }
        public string Text { get; set; }

        public static List<Message> ParseMessages(string[] lines)
        {
            var messages = new List<Message>();

            Message currentMessage = null;

            foreach (var line in lines)
            {
                if (TryParseMessageStart(line, out DateTime sentDateTime, out string sender, out string message))
                {
                    messages.Add(currentMessage = new Message()
                    {
                        SentDateTime = sentDateTime,
                        Sender = sender,
                        Text = message
                    });
                }
                else
                    currentMessage.Text += "\n" + line;
            }

            return messages;
        }

        private static bool TryParseMessageStart(string line, out DateTime sentDateTime, out string sender, out string message)
        {
            sentDateTime = DateTime.UtcNow;
            sender = null;
            message = line;

            if (line.Length < 20)
                return false;

            var isDate =
                line[0].IsNumber() &&
                line[1].IsNumber() &&
                line[2] == '/' &&
                line[3].IsNumber() &&
                line[4].IsNumber() &&
                line[5] == '/' &&
                line[6].IsNumber() &&
                line[7].IsNumber() &&
                line[8].IsNumber() &&
                line[9].IsNumber();

            var isTime =
                line[12].IsNumber() &&
                line[13].IsNumber() &&
                line[14] == ':' &&
                line[15].IsNumber() &&
                line[16].IsNumber();

            var hasDash = line[18] == '-';

            if (!isDate || !isTime || !hasDash)
                return false;

            sentDateTime = new DateTime(
                int.Parse(line.Substring(6, 4)),
                int.Parse(line.Substring(3, 2)),
                int.Parse(line.Substring(0, 2)),
                int.Parse(line.Substring(12, 2)),
                int.Parse(line.Substring(15, 2)),
                0);

            var messageStart = line.IndexOf(':', 20) + 1;
            sender = line.Substring(20, messageStart - 20 - 1);

            message = line.Substring(messageStart + 1);

            return true;
        }
    }

    public static class MessageExtensions
    {
        public static IEnumerable<Message> Filter(this IEnumerable<Message> messages, string fromSender = null, DateTime? onDate = null)
        {
            if (fromSender != null)
                messages = messages.Where(o => o.Sender == fromSender);

            if (onDate != null)
                messages = messages.Where(o => o.SentDateNormalized == onDate);

            return messages;
        }
    }
}
