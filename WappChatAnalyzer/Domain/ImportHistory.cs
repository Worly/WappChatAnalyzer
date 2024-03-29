﻿using System;
using WappChatAnalyzer.DTOs;

namespace WappChatAnalyzer.Domain
{
    public class ImportHistory
    {
        public int Id { get; set; }
        public DateTime ImportDateTime { get; set; }

        public DateTime FirstMessageDateTime { get; set; }
        public DateTime LastMessageDateTime { get; set; }
        public int MessageCount { get; set; }
        public int FromMessageId { get; set; }
        public int ToMessageId { get; set; }

        public int Overlap { get; set; }

        public int WorkspaceId { get; set; }
        public Workspace Workspace { get; set; }

        public ImportHistoryDTO GetDTO()
        {
            return new ImportHistoryDTO()
            {
                Id = Id,
                ImportDateTime = ImportDateTime,
                FirstMessageDateTime = FirstMessageDateTime,
                LastMessageDateTime = LastMessageDateTime,
                MessageCount = MessageCount,
                FromMessageId = FromMessageId,
                ToMessageId = ToMessageId,
                Overlap = Overlap
            };
        }
    }
}
