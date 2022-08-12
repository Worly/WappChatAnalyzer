using WappChatAnalyzer.DTOs;

namespace WappChatAnalyzer.Domain
{
    public class WorkspaceShare
    {
        public int Id { get; set; }

        public int WorkspaceId { get; set; }
        public Workspace Workspace { get; set; }

        public string SharedUserEmail { get; set; }

        public WorkspaceShareDTO GetDTO()
        {
            return new WorkspaceShareDTO()
            {
                Id = Id,
                SharedUserEmail = SharedUserEmail
            };
        }
    }
}
