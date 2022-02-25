using System;

namespace WappChatAnalyzer.DTOs
{
    public class ImportHistoryDTO
    {
        public int Id { get; set; }
        public DateTime ImportDateTime { get; set; }

        public DateTime FirstMessageDateTime { get; set; }
        public DateTime LastMessageDateTime { get; set; }
        public int MessageCount { get; set; }
        public int FromMessageId { get; set; }
        public int ToMessageId { get; set; }

        public int Overlap { get; set; }
    }
}
