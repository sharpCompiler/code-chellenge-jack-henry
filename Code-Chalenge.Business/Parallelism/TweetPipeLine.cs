using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks.Dataflow;
using Code_Challenge.Business.Dto;
using Code_Challenge.Business.PipelineSteps;
using Code_Challenge.Business.Repository;
using Code_Challenge.Domain.Model;
using Serilog;

namespace Code_Challenge.Business.Parallelism;

public class TweetPipeLine 
{
    private readonly ILogger _logger;
    private readonly ITweetRepository _tweetRepository;
    private readonly IHashTagRepository _hashTagRepository;

    public TweetPipeLine (ILogger logger, ITweetRepository tweetRepository, IHashTagRepository hashTagRepository)
    {
        _logger = logger;
        _tweetRepository = tweetRepository;
        _hashTagRepository = hashTagRepository;
    }

    public async Task Process(IEnumerable<TweetDto> dtos) //Error handling here can be done in a different ways, we can talk about it during the next interview
    {
        var contextLogger = _logger.ForContext("CorrelationId", Guid.NewGuid());
        contextLogger.Information("Staring to process Tweets");
        var options = new ExecutionDataflowBlockOptions() { MaxMessagesPerTask = 5 };

        contextLogger.Information("Setting up Step one");
        //step 1
        var tweetBlock = new TransformBlock<TweetDto, Tweet>(d =>
        {
            var getTweetModel = new GetTweetModel(contextLogger, _tweetRepository);
            return getTweetModel.ExecuteAsync(d);
        }, options);

        contextLogger.Information("Setting up Step Two");
        //step 2
        var extractBlock = new TransformManyBlock<Tweet, HashTagsInTweet>(d =>
        {
            var extract = new ExtractHashTagNames(contextLogger);
            return extract.ExecuteAsync(d);
        }, options);

        contextLogger.Information("Setting up Step Three");
        //step 3
        var hashTagBock = new ActionBlock<HashTagsInTweet>(d =>
        {
            var hashTagModel = new GetHashTagModel(contextLogger, _hashTagRepository);
            return hashTagModel.ExecuteAsync(d);
        }, options);

        var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

        contextLogger.Information("Linking steps together");

        tweetBlock.LinkTo(extractBlock, linkOptions);
        extractBlock.LinkTo(hashTagBock, linkOptions);

        contextLogger.Information("Sending the payload to the pipeline");
        foreach (var dto in dtos)
            await tweetBlock.SendAsync(dto);

        contextLogger.Information("Starting the pipeline");
        tweetBlock.Complete();

        await hashTagBock.Completion;
        contextLogger.Information("Process completed");


    }
}