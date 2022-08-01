using Code_Challenge.Domain.Model;
using Code_Challenge.Domain.States;

namespace Code_Challenge.Business.Repository;

public interface IHashTagRepository : IRepository<HashTag, HashTagState>
{
    Task<HashTag?> GetAsync(string name);
    Task<IEnumerable<HashTag>> GetTopTenAsync();
}