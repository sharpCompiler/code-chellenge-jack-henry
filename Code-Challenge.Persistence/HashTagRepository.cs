using Code_Challenge.Business.Repository;
using Code_Challenge.Domain.Model;

namespace Code_Challenge.Persistence;

public class HashTagRepository : IHashTagRepository
{

    public async Task AddUpdateAsync(HashTag aggregate)
    {
        await Task.CompletedTask;

        var state = aggregate.GetState();
        Storage.HashTags.AddOrUpdate(state.Name, a => state, (o, n) => state);
    }

    public async Task<HashTag?> GetAsync(string name)
    {
        await Task.CompletedTask;

        if (!Storage.HashTags.ContainsKey(name))
            return null;

        var state = Storage.HashTags[name];
        return new HashTag(state);
    }

    public async Task<IEnumerable<HashTag>> GetTopTenAsync()
    {
        await Task.CompletedTask;
        return Storage.HashTags.OrderByDescending(x => x.Value.Count).Take(5).Select(x =>new HashTag(x.Value));
    }
}