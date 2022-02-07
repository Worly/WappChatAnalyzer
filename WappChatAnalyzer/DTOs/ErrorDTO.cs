using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.DTOs
{
    public class ErrorBuilder
    {
        public IReadOnlyDictionary<string, string> Errors { get => errors; }

        private Dictionary<string, string> errors = new Dictionary<string, string>();

        public ErrorBuilder Add(string property, string errorCode)
        {
            errors.Add(property, errorCode);
            return this;
        }

        public ErrorBuilder Add(string errorCode)
        {
            errors.Add("other", errorCode);
            return this;
        }
    }
}
