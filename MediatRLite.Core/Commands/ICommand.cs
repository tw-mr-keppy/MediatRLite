namespace MediatRLite.Core.Commands;

/// <summary>
/// ICommand interface for commands that return a result.
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface ICommand<TResult> { }

/// <summary>
///  ICommand interface for commands that do not return a result.
/// </summary>
public interface ICommand { }