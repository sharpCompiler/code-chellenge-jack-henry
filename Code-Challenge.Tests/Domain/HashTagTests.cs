using Code_Challenge.Business.Dto;
using Code_Challenge.Domain.Exceptions;
using Code_Challenge.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Code_Challenge.Tests.Domain;

[TestClass]
public class HashTagTests
{
    [TestMethod]
    public void New_HashTag_In_Tweet()
    {
        //arrange
        var dto = new TweetDto(12, "testuser","test text", "#abc #alfa #beta", DateTime.Now.AddMinutes(-1));

        //act
        var hashTags = HashTag.Create(dto.HashTags, dto.Id);

        //assert
        Assert.IsTrue(hashTags.UsedInTweets.Count() == 1);
        Assert.IsTrue(hashTags.UsedInTweets.All(a => a == 12));
    }

    [TestMethod]
    public void Same_HashTag_In_A_New_Tweet()
    {
        //arrange
        var dto = new TweetDto(12, "testuser","test text", "#abc #alfa #beta", DateTime.Now.AddMinutes(-1));

        //act
        var hashTag = HashTag.Create(dto.HashTags, dto.Id);
        hashTag.AddTweet(11);

        //assert
        Assert.IsTrue(hashTag.UsedInTweets.Count() == 2, "Count failed");

        Assert.IsTrue(hashTag.UsedInTweets.First() == 12, "First item Failed");
        Assert.IsTrue(hashTag.UsedInTweets.Last() == 11, "Last Item Failed");
    }

}