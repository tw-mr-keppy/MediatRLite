namespace MediatRLite.Core.Behaviors;

/// <summary>
/// IPipelineBehavior interface for handling input and output in a pipeline.
/// </summary>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public interface IPipelineBehavior<in TInput, TOutput>
{
    /// <summary>
    ///  Defines a behavior for processing input and output in a pipeline.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TOutput> HandleAsync(TInput input, Func<Task<TOutput>> next, CancellationToken cancellationToken = default);
}

/// <summary>
///  IPipelineBehavior interface for handling input in a pipeline without returning a result.
/// </summary>
/// <typeparam name="TInput"></typeparam>
public interface IPipelineBehavior<in TInput>
{
    /// <summary>
    ///  Defines a behavior for processing input in a pipeline without returning a result.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task HandleAsync(TInput input, Func<Task> next, CancellationToken cancellationToken = default);
}