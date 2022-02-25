using System.ComponentModel.DataAnnotations;
using WappChatAnalyzer.DTOs;

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

        public CustomStatisticDTO GetDTO()
        {
            return new CustomStatisticDTO()
            {
                Id = Id,
                Name = Name,
                Regex = Regex
            };
        }
    }
}
