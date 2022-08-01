using Code_Challenge.Business.Repository;
using Code_Challenge.Domain.Model;
using Serilog;

namespace Code_Challenge.Business.PipelineSteps;

public class GetHashTagModel : IPipelineStep<HashTagsInTweet, HashTag>
{
    private readonly IHashTagRepository _hashTagRepository;
    private readonly ILogger _logger;

    public GetHashTagModel(ILogger logger, IHashTagRepository hashTagRepository)
    {
        _logger = logger;
        _hashTagRepository = hashTagRepository;
    }

    public async Task<HashTag> ExecuteAsync(HashTagsInTweet input)
    {
        _logger.Information($"Processing Hash tag with name {input.HashTag}");
        var model = await _hashTagRepository.GetAsync(input.HashTag);
        if (model == null)
        {
            model = HashTag.Create(input.HashTag, input.TweetId);
            _logger.Information($"Hash tag with name {model.Name} is Created.");
        }
        else
        {
            model.AddTweet(input.TweetId);
            _logger.Information($"Tweet Id {input.TweetId} is added to Hash tag with name {model.Name}.");
        }
        await _hashTagRepository.AddUpdateAsync(model);
        _logger.Information($"Saved hash tag with name {model.Name}");

        return model;
    }
}
