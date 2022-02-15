namespace WappChatAnalyzer.Domain
{
    public class Sender
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int WorkspaceId { get; set; }
        public Workspace Workspace { get; set; }
    }
}
