using System.Collections.Generic;

namespace WappChatAnalyzer.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        
        public string PasswordHash { get; set; }
        public byte[] Salt { get; set; }

        public int? SelectedWorkspaceId { get; set; }

        public ICollection<Workspace> Workspaces { get; set; }

        //Metode
        public List<string> GetRoles()
        {
            var roles = new List<string>();

            return roles;
        }
    }
}
