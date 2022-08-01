using Code_Challenge.Domain.Core;
using Code_Challenge.Domain.Exceptions;
using Code_Challenge.Domain.States;

namespace Code_Challenge.Domain.Model;

public class HashTag : Aggregate<HashTagState>
{
    public string Name => ModelState.Name;
    public IEnumerable<int> UsedInTweets => ModelState.UsedInTweets;

    public HashTag(HashTagState state) : base(state)
    {
    }

    public static HashTag Create(string name, int tweetId)
    {
        var state = new HashTagState()
        {
            Name = name,
            Count = 1,
            UsedInTweets = { tweetId }
        };
        return new HashTag(state);
    }


    public void AddTweet(int tweetId)
    {
        ModelState.UsedInTweets.Add(tweetId);
    }
}