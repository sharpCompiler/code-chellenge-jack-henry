using Code_Challenge.Domain.Model;
using Serilog;

namespace Code_Challenge.Business.PipelineSteps;

public class ExtractHashTagNames : IPipelineStep<Tweet, IEnumerable<HashTagsInTweet>>
{
    private readonly ILogger _logger;

    public ExtractHashTagNames(ILogger logger)
    {
        _logger = logger;
    }


    public Task<IEnumerable<HashTagsInTweet>> ExecuteAsync(Tweet input)
    {
        _logger.Information("Converting Hash Tags used in Tweet model to HashTag model");
        var result = input.HashTags.Replace("#", "").Split(' ');
        var hashTags = result.Select(x => new HashTagsInTweet(input.TwitterId, x));
        return Task.FromResult(hashTags);
    }
}

public record HashTagsInTweet(int TweetId, string HashTag);