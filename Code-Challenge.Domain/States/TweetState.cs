using System.Collections.Concurrent;
using Code_Challenge.Domain.Core;

namespace Code_Challenge.Domain.States;

public class TweetState 
{
    public int TwitterId { get; init; }
    public bool IsReTweeted { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public Dictionary<string, DateTime> ReTweetedBy { get; set; } = new();
    public string HashTags { get; set; } = string.Empty;
}