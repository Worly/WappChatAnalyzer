using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        
        public string PasswordHash { get; set; }
        public byte[] Salt { get; set; }

        //Metode
        public List<string> GetRoles()
        {
            var roles = new List<string>();

            return roles;
        }
    }
}
