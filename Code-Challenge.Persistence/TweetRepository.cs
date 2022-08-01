using System.Numerics;
using Code_Challenge.Business.Repository;
using Code_Challenge.Domain.Model;

namespace Code_Challenge.Persistence;

public class TweetRepository : ITweetRepository
{
    public async Task AddUpdateAsync(Tweet aggregate)
    {
        await Task.CompletedTask;

        var state = aggregate.GetState();
        Storage.Tweets.AddOrUpdate(state.TwitterId, a => state, (o, n) => state);
    }

    public async Task<Tweet?> GetByTweetIdAsync(int tweetId)
    {
        await Task.CompletedTask;
        
        if (!Storage.Tweets.ContainsKey(tweetId)) 
            return null;
        
        var state = Storage.Tweets[tweetId];
        return new Tweet(state);
    }

    public async Task<int> CountAsync()
    {
        await Task.CompletedTask;
        return Storage.Tweets.Count;
    }
}