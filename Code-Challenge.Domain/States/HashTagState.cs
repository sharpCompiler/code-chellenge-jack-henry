using Code_Challenge.Domain.Core;

namespace Code_Challenge.Domain.States;

public class HashTagState 
{
    public string Name { get; init; }

    public List<int> UsedInTweets { get; } = new();

    /// <summary>
    /// represent how many time this Name has been used
    /// </summary>
    public int Count { get; set; }
}