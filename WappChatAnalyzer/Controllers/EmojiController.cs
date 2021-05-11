using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WappChatAnalyzer.Domain;
using WappChatAnalyzer.DTOs;
using WappChatAnalyzer.Services;

namespace WappChatAnalyzer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmojiController : ControllerBase
    {
        private IEmojiService emojiService;
        private IMessageService messageService;
        private IStatisticService statisticService;

        public EmojiController(IEmojiService emojiService, IMessageService messageService, IStatisticService statisticService)
        {
            this.emojiService = emojiService;
            this.messageService = messageService;
            this.statisticService = statisticService;
        }

        [HttpGet("getEmoji")]
        public EmojiDTO GetEmoji([FromQuery] string emojiCodePoints)
        {
            var emoji = emojiService.GetEmojiByCodePoints(emojiCodePoints);
            return new EmojiDTO()
            {
                Name = emoji.Name,
                Status = emoji.Status,
                CodePoints = emoji.CodePoints
            };
        }

        [HttpGet("getEmojiInfoTotal")]
        public EmojiInfoTotal GetEmojiInfoTotal([FromQuery] Filter filter)
        {
            var messages = messageService.GetAllMessages().FilterDateRange(filter.FromDate, filter.ToDate).ToList();

            var result = new Dictionary<string, Dictionary<string, int>>();

            var tasks = new List<Task>();
            var taskResults = new ConcurrentBag<Dictionary<string, Dictionary<string, int>>>();

            var batchSize = messages.Count / Environment.ProcessorCount;

            for (int i = 0; i < messages.Count; i += batchSize)
            {
                var start = i;
                var size = batchSize;
                if (messages.Count - i - batchSize < batchSize / 2)
                {
                    size = messages.Count - i;
                    i += size - batchSize;
                }

                tasks.Add(new Task(() =>
                {
                    taskResults.Add(GetEmojiInfoTotal(messages.Skip(start).Take(size)));
                }));
            }

            tasks.ForEach(t => t.Start());
            Task.WaitAll(tasks.ToArray());

            foreach (var item in taskResults)
            {
                foreach (var emoji in item)
                {
                    if (!result.TryGetValue(emoji.Key, out Dictionary<string, int> dictionary))
                        result.Add(emoji.Key, dictionary = new Dictionary<string, int>());

                    foreach (var sender in emoji.Value)
                    {
                        if (!dictionary.TryGetValue(sender.Key, out int count))
                            dictionary.Add(sender.Key, count = 0);

                        count += sender.Value;
                        dictionary[sender.Key] = count;
                    }
                }
            }

            return new EmojiInfoTotal()
            {
                EmojiInfos = result.OrderByDescending(o => o.Value.Sum(i => i.Value)).ThenBy(o => o.Key).Select(o => new EmojiInfo()
                {
                    Total = o.Value.Sum(i => i.Value),
                    EmojiCodePoints = o.Key,
                    BySenders = o.Value
                }).ToList()
            };
        }

        private Dictionary<string, Dictionary<string, int>> GetEmojiInfoTotal(IEnumerable<Message> inMessages)
        {
            var result = new Dictionary<string, Dictionary<string, int>>();

            foreach (var message in inMessages)
            {
                var enumerator = StringInfo.GetTextElementEnumerator(message.Text);
                while (enumerator.MoveNext())
                {
                    var textElement = enumerator.GetTextElement();
                    if (emojiService.TryGetEmoji(textElement, out Emoji emoji))
                    {
                        emoji = emoji.FullyQualified;

                        if (!result.TryGetValue(emoji.CodePoints, out Dictionary<string, int> forSenders))
                            result.Add(emoji.CodePoints, forSenders = new Dictionary<string, int>());

                        if (!forSenders.TryGetValue(message.Sender, out int count))
                            forSenders.Add(message.Sender, count = 0);

                        count++;
                        forSenders[message.Sender] = count;
                    }
                }

            }

            return result;
        }


        [HttpGet("getStatistic/singleEmoji/{emojiCodePoints}")]
        public Statistic<int> GetStatisticSingleEmoji(string emojiCodePoints, [FromQuery] Filter filter)
        {
            var result = statisticService.GetStatistic(messageService, messages => emojiService.CountEmojiInMessages(messages, emojiCodePoints), "SingleEmoji", filter);

            return result;
        }
    }
}
