using Code_Challenge.Business.Dto;
using Code_Challenge.Business.PipelineSteps;
using Code_Challenge.Business.Repository;
using Code_Challenge.Domain.Model;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;


namespace Code_Challenge.Tests.Business;

[TestClass]
public class HashTagHandlerTests
{
    private IHashTagRepository _hashTagRepository;
    private ILogger _logger;
    
    [TestInitialize]
    public void Setup()
    {
        _hashTagRepository = A.Fake<IHashTagRepository>();
        _logger = A.Fake<ILogger>();
    }


    [TestMethod]
    public async Task Create_New_Hash_Tags()
    {
        //arrange
        var input = new HashTagsInTweet(12, "abc");
        var handler = new GetHashTagModel(_logger, _hashTagRepository);
        A.CallTo(() => _hashTagRepository.GetAsync("abc")).Returns((HashTag)null);

        //act
        await handler.ExecuteAsync(input);

        //assert
        A.CallTo(() => _hashTagRepository.GetAsync("abc")).MustHaveHappened(1, Times.Exactly);
        A.CallTo(() => _hashTagRepository.AddUpdateAsync(A<HashTag>.That.Matches(x => x.Name == "abc" && x.UsedInTweets.All(a => a == input.TweetId)))).MustHaveHappened(1, Times.Exactly);
    }




    [TestMethod]
    public async Task Add_Tweet_In_Hash_Tags()
    {
        //arrange
        var input = new HashTagsInTweet(12, "abc");
        var handler = new GetHashTagModel(_logger, _hashTagRepository);
        A.CallTo(() => _hashTagRepository.GetAsync("abc")).Returns(HashTag.Create("abc", 11));

        //act
        await handler.ExecuteAsync(input);

        //assert
        A.CallTo(() => _hashTagRepository.GetAsync("abc")).MustHaveHappened(1, Times.Exactly);
        A.CallTo(() => _hashTagRepository.AddUpdateAsync(A<HashTag>.That.Matches(x => x.Name == "abc" && x.UsedInTweets.SequenceEqual(new []{11, 12})))).MustHaveHappened(1, Times.Exactly);
    }
}