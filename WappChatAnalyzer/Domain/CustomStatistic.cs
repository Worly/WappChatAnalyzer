using System.ComponentModel.DataAnnotations;

namespace WappChatAnalyzer.Domain
{
    public class CustomStatistic
    {
        public int Id { get; set; }
        [MaxLength(60)]
        public string Name { get; set; }
        public string Regex { get; set; }

        public int WorkspaceId { get; set; }
        public Workspace Workspace { get; set; }
    }
}
