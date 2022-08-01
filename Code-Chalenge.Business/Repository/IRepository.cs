using Code_Challenge.Domain.Core;

namespace Code_Challenge.Business.Repository;

public interface IRepository<in TAggregate, TState>
    where TState : class
    where TAggregate : Aggregate<TState>
{
    Task AddUpdateAsync(TAggregate aggregate);
}