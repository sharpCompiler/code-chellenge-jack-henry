namespace Code_Challenge.Business.PipelineSteps;

public interface IPipelineStep<in TInput, TOutput>
{
    Task<TOutput> ExecuteAsync(TInput input);
}