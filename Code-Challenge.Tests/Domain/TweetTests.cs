using Code_Challenge.Domain.Exceptions;
using Code_Challenge.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Code_Challenge.Tests.Domain;

[TestClass]
public class TweetTests
{
    [TestMethod]
    public void ReTweeted_By_New_User()
    {
        //arrange
        var tweet = Tweet.Create(12, "Username", "text", "#hashtag", DateTime.Now.AddMinutes(-4));
        var reTweetedBy = "Test user";
        var reTweetedAt = DateTime.Now.AddMinutes(14);

        //act
        tweet.ReTweeted(reTweetedBy, reTweetedAt);

        //assert
        Assert.IsTrue(tweet.IsReTweeted);
    }



    [TestMethod]
    public void ReTweeted_By_Other_User()
    {
        //arrange
        var tweet = Tweet.Create(12, "Username", "text", "#hashtag", DateTime.Now.AddMinutes(-4));
        var reTweetedBy = "Test user";
        var reTweetedAt = DateTime.Now.AddMinutes(14);

        //act
        tweet.ReTweeted(reTweetedBy, reTweetedAt);

        //assert
        Assert.IsTrue(tweet.ReTweetedBy.ContainsKey(reTweetedBy));

    }

    [TestMethod]
    public void ReTweeted_By_Same_User_Throws_Exception()
    {
        //arrange
        var tweet = Tweet.Create(12, "Username", "text", "#hashtag", DateTime.Now.AddMinutes(-4));
        var reTweetedBy = "Test user";
        var reTweetedAt = DateTime.Now.AddMinutes(14);

        //act
        tweet.ReTweeted(reTweetedBy, reTweetedAt);

        //assert
        Assert.ThrowsException<AlreadyReTweetedBySameUserException>(() => tweet.ReTweeted(reTweetedBy, reTweetedAt));
    }
}