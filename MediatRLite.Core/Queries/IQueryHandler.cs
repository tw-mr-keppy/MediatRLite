namespace MediatRLite.Core.Queries;

/// <summary>
/// IQueryHandler interface for handling queries that return a result.
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
    /// <summary>
    /// Defines a handler for a request
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}