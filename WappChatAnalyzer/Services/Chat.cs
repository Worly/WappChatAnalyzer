using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.Services
{
    public interface IChat
    {
        public List<Message> Messages { get; }

        public List<string> Senders { get; }
    }

    public class Chat : IChat
    {
        public List<Message> Messages { get; private set; }

        public List<string> Senders { get; private set; }

        public Chat()
        {
            Messages = Message.ParseMessages(System.IO.File.ReadAllLines(@"C:\Users\Tino\Documents\WhatsApp Chat with Lara.txt"));
            Senders = Messages.Select(o => o.Sender).Distinct().ToList();
        }
    }
}
