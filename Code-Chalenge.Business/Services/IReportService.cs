using Code_Challenge.Business.Dto;
using Code_Challenge.Business.Repository;

namespace Code_Challenge.Business.Services;

public interface IReportService
{
    Task<ReportDto> GetReportAsync();
}
public class ReportService : IReportService
{
    private readonly ITweetRepository _tweetRepository;
    private readonly IHashTagRepository _hashTagRepository;

    public ReportService(ITweetRepository tweetRepository, IHashTagRepository hashTagRepository)
    {
        _tweetRepository = tweetRepository;
        _hashTagRepository = hashTagRepository;
    }
    public async Task<ReportDto> GetReportAsync()
    {
        var topTenHashTags = await _hashTagRepository.GetTopTenAsync();
        var totalCountTweets = await _tweetRepository.CountAsync();

        return new ReportDto(totalCountTweets, topTenHashTags.Select(x => new HashTagDto(x.Name, x.UsedInTweets.Count())).ToArray());
    }
}