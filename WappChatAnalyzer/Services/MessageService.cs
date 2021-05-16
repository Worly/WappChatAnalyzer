using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WappChatAnalyzer.Domain;

namespace WappChatAnalyzer.Services
{
    public interface IMessageService
    {
        List<string> GetAllSenders();
        IQueryable<Message> GetAllMessages();
        int ImportMessages(List<Message> messages);
        List<ImportHistory> GetImportHistory();
        List<Message> GetMessages(int fromId, int toId);
        Message GetFirstMessageOfDayBefore(DateTime dateTime);
        Message GetFirstMessageOfDayAfter(DateTime dateTime);
    }

    public class MessageService : IMessageService
    {
        private MainDbContext context;
        public MessageService(MainDbContext context)
        {
            this.context = context;
        }

        public List<string> GetAllSenders()
        {
            return context.Messages.Select(o => o.Sender).Distinct().ToList();
        }

        public IQueryable<Message> GetAllMessages()
        {
            return context.Messages;
        }

        public int ImportMessages(List<Message> messages)
        {
            var lastMessage = context.Messages.OrderBy(o => o.Id).LastOrDefault();
            var nextMessageId = lastMessage != null ? lastMessage.Id + 1 : 1;

            Func<Message, Message, bool> messagesEquals = (Message f, Message s) => f.SentDateTime == s.SentDateTime && f.Sender == s.Sender && f.Text == s.Text;

            if (lastMessage == null)
            {
                for (int i = 0; i < messages.Count; i++)
                    messages[i].Id = 1 + i;

                context.Messages.AddRange(messages);

                context.ImportHistories.Add(new ImportHistory()
                {
                    ImportDateTime = DateTime.Now,
                    FirstMessageDateTime = messages.First().SentDateTime,
                    LastMessageDateTime = messages.Last().SentDateTime,
                    MessageCount = messages.Count,
                    FromMessageId = 1,
                    ToMessageId = messages.Count,
                    Overlap = 0
                });

                context.SaveChanges();

                return messages.Count;
            }
            else
            {
                var messagesToFetch = messages.Where(o => o.SentDateTime == lastMessage.SentDateTime).Count();
                var messagesInDatabase = context.Messages.OrderByDescending(o => o.Id).Take(messagesToFetch).ToList();

                var newMessagesStartIndex = -1;

                int matchingMessageIndex = messages.FindLastIndex(o => messagesEquals(o, lastMessage));
                for(int i = matchingMessageIndex; i >= 0; i--)
                {
                    var ok = true;
                    for(int j = 0; j < messagesInDatabase.Count && i >= j; j++)
                    {
                        var inDatabaseMessage = messagesInDatabase[j];
                        var newMessage = messages[i - j];
                        if (!messagesEquals(inDatabaseMessage, newMessage))
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                    {
                        newMessagesStartIndex = i + 1;
                        break;
                    }
                }

                if (newMessagesStartIndex == -1)
                    throw new InvalidOperationException("There is no overlap in new messages and old messages");

                for(int i = newMessagesStartIndex; i < messages.Count; i++)
                {
                    messages[i].Id = nextMessageId + (i - newMessagesStartIndex);
                    context.Messages.Add(messages[i]);
                }

                context.ImportHistories.Add(new ImportHistory()
                {
                    ImportDateTime = DateTime.Now,
                    FirstMessageDateTime = messages.First().SentDateTime,
                    LastMessageDateTime = messages.Last().SentDateTime,
                    MessageCount = messages.Count,
                    FromMessageId = nextMessageId,
                    ToMessageId = nextMessageId + (messages.Count - 1 - newMessagesStartIndex),
                    Overlap = newMessagesStartIndex
                });

                context.SaveChanges();

                return messages.Count - newMessagesStartIndex;
            }
        }

        public List<ImportHistory> GetImportHistory()
        {
            return context.ImportHistories.ToList();
        }

        public List<Message> GetMessages(int fromId, int toId)
        {
            return context.Messages.Where(o => o.Id >= fromId && o.Id <= toId).ToList();
        }

        public Message GetFirstMessageOfDayBefore(DateTime dateTime)
        {
            return this.context.Messages
                .Where(o => o.SentDateTime < dateTime)
                .OrderByDescending(o => o.SentDateTime.Date)
                .ThenBy(o => o.SentDateTime.TimeOfDay)
                .ThenBy(o => o.Id)
                .FirstOrDefault();
        }

        public Message GetFirstMessageOfDayAfter(DateTime dateTime)
        {
            return this.context.Messages
                .OrderBy(o => o.Id)
                .Where(o => o.SentDateTime >= dateTime)
                .FirstOrDefault();
        }
    }
}
