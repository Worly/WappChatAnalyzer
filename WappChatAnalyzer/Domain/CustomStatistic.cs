using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.Domain
{
    public class CustomStatistic
    {
        public int Id { get; set; }
        [MaxLength(60)]
        public string Name { get; set; }
        public string Regex { get; set; }
    }
}
