using Code_Challenge.Business.Dto;
using Code_Challenge.Business.PipelineSteps;
using Code_Challenge.Business.Repository;
using Code_Challenge.Domain.Model;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;


namespace Code_Challenge.Tests.Business;

[TestClass]
public class TweetHandlerTests
{
    private ITweetRepository _tweetRepository;
    private ILogger _logger;
    
    [TestInitialize]
    public void Setup()
    {
        _tweetRepository = A.Fake<ITweetRepository>();
        _logger = A.Fake<ILogger>();
    }


    [TestMethod]
    public async Task Create_New_Tweet()
    {
        //arrange
        var dto = new TweetDto(12, "testuser", "test text", "#abc #alfa #beta", DateTime.Now.AddMinutes(-1));
        var handler = new GetTweetModel(_logger, _tweetRepository);
        A.CallTo(() => _tweetRepository.GetByTweetIdAsync(dto.Id)).Returns((Tweet)null);

        //act
        await handler.ExecuteAsync(dto);

        //assert
        A.CallTo(() => _tweetRepository.GetByTweetIdAsync(dto.Id)).MustHaveHappened(1, Times.Exactly);
        A.CallTo(() => _tweetRepository.AddUpdateAsync(A<Tweet>.That.Matches(x => x.TwitterId == dto.Id && x.IsReTweeted == false))).MustHaveHappened(1, Times.Exactly);
    }


   [TestMethod]
    public async Task Old_Tweet_Is_ReTweeted()
    {
        //arrange
        var dto = new TweetDto(12, "testuser", "test text", "#abc #alfa #beta", DateTime.Now.AddMinutes(-1));
        var handler = new GetTweetModel(_logger, _tweetRepository);
        A.CallTo(() => _tweetRepository.GetByTweetIdAsync(dto.Id)).Returns(Tweet.Create(12, "OldUser", dto.Text, dto.HashTags, DateTime.Now.AddMinutes(-15)));

        //act
        await handler.ExecuteAsync(dto);

        //assert
        A.CallTo(() => _tweetRepository.GetByTweetIdAsync(dto.Id)).MustHaveHappened(1, Times.Exactly);
        A.CallTo(() => _tweetRepository.AddUpdateAsync(A<Tweet>.That.Matches(x => x.TwitterId == dto.Id && x.IsReTweeted == true))).MustHaveHappened(1, Times.Exactly);
    }



}