using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WappChatAnalyzer.Domain;

namespace WappChatAnalyzer.Services
{
    //public class LocalMessageService : IMessageService
    //{
    //    public List<Message> Messages { get; private set; }

    //    public List<string> Senders { get; private set; }

    //    public LocalMessageService()
    //    {
    //        Messages = MessageUtils.ParseMessages(System.IO.File.ReadAllLines(@"C:\Users\Tino\Documents\WhatsApp Chat with Lara.txt"));
    //        Senders = Messages.Select(o => o.Sender).Distinct().ToList();
    //    }

    //    public List<string> GetAllSenders()
    //    {
    //        return Senders;
    //    }

    //    public IQueryable<Message> GetAllMessages()
    //    {
    //        return Messages.AsQueryable();
    //    }

    //    public int ImportMessages(List<Message> messages)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public List<ImportHistory> GetImportHistory()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
