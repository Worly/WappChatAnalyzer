using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WappChatAnalyzer.Domain;

namespace WappChatAnalyzer.Services
{
    public interface IMessageService
    {
        List<Sender> GetAllSenders(int workspaceId);
        IQueryable<Message> GetAllMessages(int workspaceId);
        int ImportMessages(int workspaceId, List<Message> messages, out Message firstImportedMessage);
        List<ImportHistory> GetImportHistory(int workspaceId);
        List<Message> GetMessages(int workspaceId, int fromId, int toId);
        Message GetFirstMessageOfDayBefore(int workspaceId, DateOnly date);
        Message GetFirstMessageOfDayAfter(int workspaceId, DateOnly date);
    }

    public class MessageService : IMessageService
    {

        private MainDbContext context;
        public MessageService(MainDbContext context)
        {
            this.context = context;
        }

        public List<Sender> GetAllSenders(int workspaceId)
        {
            return context.Senders
                .Where(o => o.WorkspaceId == workspaceId)
                .ToList();
        }

        public IQueryable<Message> GetAllMessages(int workspaceId)
        {
            return context.Messages.Where(o => o.WorkspaceId == workspaceId);
        }

        public int ImportMessages(int workspaceId, List<Message> messages, out Message firstImportedMessage)
        {
            foreach (var m in messages)
            {
                m.WorkspaceId = workspaceId;
                m.Sender.WorkspaceId = workspaceId;
            }

            var lastMessage = context.Messages
                .Where(o => o.WorkspaceId == workspaceId)
                .OrderBy(o => o.Id)
                .LastOrDefault();

            var nextMessageId = lastMessage != null ? lastMessage.Id + 1 : 1;

            Func<Message, Message, bool> messagesEquals = (Message f, Message s) => f.SentDate == s.SentDate && f.SentTime == s.SentTime && f.Sender == s.Sender && f.Text == s.Text;

            if (lastMessage == null)
            {
                for (int i = 0; i < messages.Count; i++)
                    messages[i].Id = 1 + i;

                context.Messages.AddRange(messages);

                firstImportedMessage = messages.FirstOrDefault();

                var fMessage = messages.First();
                var lMessage = messages.Last();

                context.ImportHistories.Add(new ImportHistory()
                {
                    ImportDateTime = DateTime.Now,
                    FirstMessageDateTime = fMessage.SentDate.ToDateTime(fMessage.SentTime),
                    LastMessageDateTime = lMessage.SentDate.ToDateTime(lMessage.SentTime),
                    MessageCount = messages.Count,
                    FromMessageId = 1,
                    ToMessageId = messages.Count,
                    Overlap = 0,
                    WorkspaceId = workspaceId
                });

                context.SaveChanges();

                return messages.Count;
            }
            else
            {
                var messagesToFetch = messages.Where(o => o.SentDate == lastMessage.SentDate && o.SentTime == lastMessage.SentTime).Count();
                var messagesInDatabase = context.Messages.Where(o => o.WorkspaceId == workspaceId).OrderByDescending(o => o.Id).Take(messagesToFetch).ToList();

                var newMessagesStartIndex = -1;

                int matchingMessageIndex = messages.FindLastIndex(o => messagesEquals(o, lastMessage));
                for (int i = matchingMessageIndex; i >= 0; i--)
                {
                    var ok = true;
                    for (int j = 0; j < messagesInDatabase.Count && i >= j; j++)
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

                firstImportedMessage = messages[newMessagesStartIndex];

                for (int i = newMessagesStartIndex; i < messages.Count; i++)
                    messages[i].Id = nextMessageId + (i - newMessagesStartIndex);

                context.Messages.AddRange(messages.Skip(newMessagesStartIndex));

                var fMessage = messages.First();
                var lMessage = messages.Last();

                context.ImportHistories.Add(new ImportHistory()
                {
                    ImportDateTime = DateTime.Now,
                    FirstMessageDateTime = fMessage.SentDate.ToDateTime(fMessage.SentTime),
                    LastMessageDateTime = lMessage.SentDate.ToDateTime(lMessage.SentTime),
                    MessageCount = messages.Count,
                    FromMessageId = nextMessageId,
                    ToMessageId = nextMessageId + (messages.Count - 1 - newMessagesStartIndex),
                    Overlap = newMessagesStartIndex,
                    WorkspaceId = workspaceId
                });

                context.SaveChanges();

                return messages.Count - newMessagesStartIndex;
            }
        }

        public List<ImportHistory> GetImportHistory(int workspaceId)
        {
            return context.ImportHistories
                .Where(o => o.WorkspaceId == workspaceId)
                .ToList();
        }

        public List<Message> GetMessages(int workspaceId, int fromId, int toId)
        {
            return context.Messages
                .Include(o => o.Sender)
                .Where(o => o.WorkspaceId == workspaceId)
                .Where(o => o.Id >= fromId && o.Id <= toId)
                .OrderBy(o => o.Id)
                .ToList();
        }

        public Message GetFirstMessageOfDayBefore(int workspaceId, DateOnly date)
        {
            return context.Messages
                .Where(o => o.WorkspaceId == workspaceId)
                .Where(o => o.SentDate < date)
                .OrderByDescending(o => o.SentDate)
                .ThenBy(o => o.SentTime)
                .ThenBy(o => o.Id)
                .FirstOrDefault();
        }

        public Message GetFirstMessageOfDayAfter(int workspaceId, DateOnly date)
        {
            return context.Messages
                .Where(o => o.WorkspaceId == workspaceId)
                .OrderBy(o => o.SentDate)
                .ThenBy(o => o.SentTime)
                .ThenBy(o => o.Id)
                .Where(o => o.SentDate >= date)
                .FirstOrDefault();
        }
    }
}
