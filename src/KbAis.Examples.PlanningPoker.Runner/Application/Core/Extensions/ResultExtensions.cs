using CSharpFunctionalExtensions;

namespace KbAis.Examples.PlanningPoker.Runner.Application.Core.Extensions;

public static class ResultExtensions {

    public static async Task<Result<(T, K)>> Tap<T, K>(this Task<Result<(T, K)>> resultTask, Func<T, K, Task> func) {
        var result = await resultTask;

        if (result.IsSuccess) {
            await func(result.Value.Item1, result.Value.Item2);
        }

        return result;
    }

    public static async Task<Result<(A First, C Second)>> Map<A, B, C>(
        this Task<Result<(A, B)>> resultTask, Func<A, B, C> func
    ) {
        var result = await resultTask;

        if (result.IsFailure) {
            return Result.Failure<(A First, C Second)>(result.Error);
        }

        var c = func(result.Value.Item1, result.Value.Item2);

        return Result.Success((result.Value.Item1, c));
    }

    public static Result<(T First, K Second)> Bind<T, T2, K>(
        this Result<(T, T2)> result, Func<T, T2, Result<K>> func
    ) {
        var (_, isFailure, v, error) = result;

        if (isFailure) {
            return Result.Failure<(T, K)>(error);
        }

        var resultToReturn = func(v.Item1, v.Item2);

        return resultToReturn.IsFailure
            ? Result.Failure<(T, K)>(resultToReturn.Error)
            : Result.Success((v.Item1, resultToReturn.Value));
    }

    // Both
    public static async Task<Result<(T1 First, K Second)>> Bind<T1, T2, K>(
        this Task<Result<(T1 First, T2 Second)>> resultTask, Func<T1, T2, Task<Result<K>>> func
    ) {
        var (_, isFailure, v, error) = await resultTask;

        if (isFailure) {
            return Result.Failure<(T1, K)>(error);
        }

        var resultToReturn = await func(v.Item1, v.Item2);

        return resultToReturn.IsFailure
            ? Result.Failure<(T1, K)>(resultToReturn.Error)
            : Result.Success((v.Item1, resultToReturn.Value));
    }

    // Right
    public static async Task<Result<(T1 First, K Second)>> Bind<T1, T2, K>(
        this Result<(T1 First, T2 Second)> result, Func<T1, T2, Task<Result<K>>> func
    ) {
        var (_, isFailure, v, error) = result;

        if (isFailure) {
            return Result.Failure<(T1, K)>(error);
        }

        var resultToReturn = await func(v.Item1, v.Item2);

        return resultToReturn.IsFailure
            ? Result.Failure<(T1, K)>(resultToReturn.Error)
            : Result.Success((v.Item1, resultToReturn.Value));
    }

    // Left
    public static async Task<Result> Bind<T1, T2>(
        this Task<Result<(T1 First, T2 Second)>> resultTask, Func<T1, T2, Result> func
    ) {
        var (_, isFailure, v, error) = await resultTask;

        if (isFailure) {
            return Result.Failure<(T1, T2)>(error);
        }

        var resultToReturn = func(v.Item1, v.Item2);

        return resultToReturn.IsFailure
            ? Result.Failure<(T1, T2)>(resultToReturn.Error) : Result.Success();
    }

    public static async Task<Result<(T1 First, K Second)>> Bind<T1, T2, K>(
        this Task<Result<(T1 First, T2 Second)>> resultTask, Func<T1, T2, Result<K>> func
    ) {
        var (_, isFailure, v, error) = await resultTask;

        if (isFailure) {
            return Result.Failure<(T1, K)>(error);
        }

        var resultToReturn = func(v.Item1, v.Item2);

        return resultToReturn.IsFailure
            ? Result.Failure<(T1, K)>(resultToReturn.Error)
            : Result.Success((v.Item1, resultToReturn.Value));
    }

    public static async Task<Result<(T First, K Second)>> BindIf<T, K>(
        this Task<Result<(T First, K Second)>> resultTask, Func<T, K, bool> predicate, Func<T, K, Result<K>> func
    ) {
        var result = await resultTask
            .ConfigureAwait(Result.Configuration.DefaultConfigureAwait);

        if (result.IsFailure || predicate(result.Value.Item1, result.Value.Item2) == false) {
            return result;
        }

        var resultToReturn = func(result.Value.Item1, result.Value.Item2);

        return resultToReturn.IsFailure
            ? Result.Failure<(T, K)>(resultToReturn.Error)
            : Result.Success((result.Value.Item1, resultToReturn.Value));
    }

    public static async Task<Result<(T First, K Second)>> BindIf<T, K>(
        this Task<Result<(T First, K Second)>> resultTask, Func<T, K, bool> predicate, Func<T, K, Task<Result<K>>> func
    ) {
        var result = await resultTask
            .ConfigureAwait(Result.Configuration.DefaultConfigureAwait);

        if (result.IsFailure || predicate(result.Value.Item1, result.Value.Item2) == false) {
            return result;
        }

        var resultToReturn = await func(result.Value.Item1, result.Value.Item2)
            .ConfigureAwait(Result.Configuration.DefaultConfigureAwait);

        return resultToReturn.IsFailure
            ? Result.Failure<(T, K)>(resultToReturn.Error)
            : Result.Success((result.Value.Item1, resultToReturn.Value));
    }

}
