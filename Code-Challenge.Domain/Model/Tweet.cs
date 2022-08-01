using Code_Challenge.Domain.Core;
using Code_Challenge.Domain.Exceptions;
using Code_Challenge.Domain.States;

namespace Code_Challenge.Domain.Model;

public class Tweet : Aggregate<TweetState>
{
    public Tweet(TweetState state) : base(state)
    {
    }

    public bool IsReTweeted => ModelState.IsReTweeted;
    public int TwitterId => ModelState.TwitterId;
    public IReadOnlyDictionary<string, DateTime> ReTweetedBy => ModelState.ReTweetedBy;
    public string HashTags => ModelState.HashTags;

    public static Tweet Create(int tweetId, string username, string text, string hashTags, DateTime createdDate)
    {
        var tweetState = new TweetState()
        {
            TwitterId = tweetId,
            CreatedDate = createdDate,
            Owner = username,
            Text = text,
            HashTags = hashTags
        };
        return new Tweet(tweetState);
    }

    public void ReTweeted(string reTweetedBy, DateTime reTweetedAt)
    {
        if (ModelState.ReTweetedBy.ContainsKey(reTweetedBy))
            throw new AlreadyReTweetedBySameUserException();

        ModelState.ReTweetedBy.Add(reTweetedBy, reTweetedAt);

        ModelState.IsReTweeted = true;
    }
}