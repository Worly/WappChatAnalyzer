using DelegateDecompiler;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WappChatAnalyzer.DTOs;
using WappChatAnalyzer.Services;

namespace WappChatAnalyzer.Domain
{
    [Index(nameof(SentDateTime))]
    public class Message
    {
        public static readonly int NORMALIZED_HOURS = -7;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public DateTime SentDateTime { get; set; }
        [Computed]
        [NotMapped]
        public DateTime NormalizedSentDate { get => SentDateTime.AddHours(NORMALIZED_HOURS).Date; }
        public int SenderId { get; set; }
        public Sender Sender { get; set; }
        public string Text { get; set; }
        public bool IsMedia { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WorkspaceId { get; set; }
        public Workspace Workspace { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasKey(o => new { o.Id, o.WorkspaceId });
        }

        public MessageDTO GetDTO()
        {
            return new MessageDTO()
            {
                Id = Id,
                Sender = Sender?.GetDTO(),
                SentDateTime = SentDateTime,
                Text = Text,
                IsMedia = IsMedia
            };
        }
    }

    public static class MessageExtensions
    {
        public static IQueryable<Message> Filter(this IQueryable<Message> messages, Sender fromSender = null, DateTime? onDate = null)
        {
            if (fromSender != null)
                messages = messages.Where(o => o.SenderId == fromSender.Id);

            if (onDate != null)
                messages = messages.Where(o => o.NormalizedSentDate == onDate);

            return messages;
        }

        public static IQueryable<Message> FilterDateRange(this IQueryable<Message> messages, DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate != null)
                messages = messages.Where(o => o.NormalizedSentDate >= fromDate.Value.Date).Decompile();

            if (toDate != null)
                messages = messages.Where(o => o.NormalizedSentDate <= toDate.Value.Date).Decompile();

            return messages;
        }
    }

    public static class MessageUtils
    {
        public static List<Message> ParseMessages(string[] lines, List<Sender> senders)
        {
            var messages = new List<Message>();

            Message currentMessage = null;

            foreach (var line in lines)
            {
                if (TryParseMessageStart(line, out DateTime sentDateTime, out string sender, out string message, out bool isMedia))
                {
                    var senderObj = senders.FirstOrDefault(o => o.Name == sender);
                    if (senderObj == null)
                        senders.Add(senderObj = new Sender() { Name = sender });
                    messages.Add(currentMessage = new Message()
                    {
                        SentDateTime = sentDateTime,
                        Sender = senderObj,
                        Text = message,
                        IsMedia = isMedia
                    });
                }
                else
                    currentMessage.Text += "\n" + line;
            }

            return messages;
        }

        private static bool TryParseMessageStart(string line, out DateTime sentDateTime, out string sender, out string message, out bool isMedia)
        {
            try
            {

                sentDateTime = DateTime.UtcNow;
                sender = null;
                message = line;
                isMedia = false;

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

                isMedia = message == "<Media omitted>";
            }
            catch(Exception e)
            {
                throw new Exception("Error on message line: " + line, e);
            }

            return true;
        }
    }
}
