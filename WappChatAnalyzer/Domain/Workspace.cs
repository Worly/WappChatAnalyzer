using WappChatAnalyzer.DTOs;

namespace WappChatAnalyzer.Domain
{
    public class Workspace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public WorkspaceDTO GetDTO()
        {
            return new WorkspaceDTO()
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
