using WappChatAnalyzer.DTOs;

namespace WappChatAnalyzer.Domain
{
    public class Sender
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int WorkspaceId { get; set; }
        public Workspace Workspace { get; set; }

        public SenderDTO GetDTO()
        {
            return new SenderDTO()
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
