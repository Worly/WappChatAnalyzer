using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.DTOs
{
    public class Filter
    {
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public string GroupingPeriod { get; set; }
        public string Per { get; set; }
        public bool PerReciprocal { get; set; }
    }
}
