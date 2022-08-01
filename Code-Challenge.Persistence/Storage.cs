using System.Collections.Concurrent;
using Code_Challenge.Domain.States;

namespace Code_Challenge.Persistence;

public static class Storage
{
    public static ConcurrentDictionary<int, TweetState> Tweets { get; } = new();
    public static ConcurrentDictionary<string, HashTagState> HashTags{ get; } = new();
}