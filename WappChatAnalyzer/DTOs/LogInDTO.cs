using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.DTOs
{
    public class LogInDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LogInResponseDTO
    {
        public string Token { get; set; }
    }
}
