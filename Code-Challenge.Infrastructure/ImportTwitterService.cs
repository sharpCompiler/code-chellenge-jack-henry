using Code_Challenge.Business.Definitions;
using Code_Challenge.Business.Dto;
using Code_Challenge.Infrastructure.DataGenerator;
using Polly;
using Serilog;

namespace Code_Challenge.Infrastructure;

public class ImportTwitterService : IImportTwitterService
{
    private readonly ILogger _logger;
    private int _currentId =1;

    public ImportTwitterService(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<TweetDto>> GetTweetsAsync()
    {
       _logger.Information("Getting data from Twitter API - Started");
        var retryPolicy = Policy.Handle<Exception>()
            .WaitAndRetryAsync(retryCount: 2, sleepDurationProvider: _ => TimeSpan.FromSeconds(3));

        return await retryPolicy.ExecuteAsync(async () =>
        {
            if (new Random().Next(0, 1) == 1)
                throw new Exception("API is not available at this moment");

            await Task.Delay(100); // simulating http
            var numberOfTweets = new Random().Next(45, 153);

            var tweets = new List<TweetDto>();
            for (var i = 0; i < numberOfTweets; i++)
            {
                var tweet = new TweetDto(_currentId, FirstNameSource.NextValue(), TextSource.NextValue(),
                    HashTagSource.NextValue(), DateTime.Now);
                tweets.Add(tweet);

                _currentId++;
            }

            _logger.Information("Getting data from Twitter API done Successfully");
            return tweets;
        });
    }
}