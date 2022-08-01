using Code_Challenge.Business.Dto;

namespace Code_Challenge.Business.Definitions;

public interface IImportTwitterService
{
    Task<IEnumerable<TweetDto>> GetTweetsAsync();
}