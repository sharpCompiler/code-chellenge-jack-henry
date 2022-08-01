using Code_Challenge.Domain.Model;
using Code_Challenge.Domain.States;

namespace Code_Challenge.Business.Repository;

public interface ITweetRepository : IRepository<Tweet, TweetState>
{
    Task<Tweet?> GetByTweetIdAsync(int tweetId);
    Task<int> CountAsync();
}