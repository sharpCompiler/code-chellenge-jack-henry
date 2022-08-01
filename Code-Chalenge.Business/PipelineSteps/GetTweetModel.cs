using Code_Challenge.Business.Dto;
using Code_Challenge.Business.Repository;
using Code_Challenge.Domain.Model;
using Serilog;

namespace Code_Challenge.Business.PipelineSteps;

public class GetTweetModel : IPipelineStep<TweetDto, Tweet>
{
    private readonly ILogger _logger;
    private readonly ITweetRepository _tweetRepository;

    public GetTweetModel(ILogger logger, ITweetRepository tweetRepository)
    {
        _logger = logger;
        _tweetRepository = tweetRepository;
    }
    public async Task<Tweet> ExecuteAsync(TweetDto input)
    {
        _logger.Information($"Processing Tweet with id {input.Id}");
        var model = await _tweetRepository.GetByTweetIdAsync(input.Id);
        if (model == null)
        {
            model = Tweet.Create(input.Id, input.Username, input.Text, input.HashTags, input.CreatedDateTime);
            _logger.Information($"Tweet with id {input.Id} is Created.");
        }
        else
        {
            model.ReTweeted(input.Username, DateTime.Now);
            _logger.Information($"Tweet with id {input.Id} has been ReTweeted");
        }

        await _tweetRepository.AddUpdateAsync(model);
        _logger.Information($"Saved Tweet with id {input.Id}");

        return model;
    }
}