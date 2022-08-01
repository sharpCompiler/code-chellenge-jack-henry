namespace Code_Challenge.Domain.Core;

public abstract class Aggregate<T> 
    where T : class
{
    protected readonly T ModelState;

    protected Aggregate(T state)
    {
        ModelState = state;
    }


    public T GetState()
    {
        return ModelState;
    }
}