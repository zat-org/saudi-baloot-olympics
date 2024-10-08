﻿
namespace BalootOlympicsTeamsApi.Common;

public static class ResultExtensions
{
    public static Result Merge(this Result result1, Result result2)
    {
        var res = new Result();
        res.Errors.AddRange(result1.Errors);
        res.Errors.AddRange(result2.Errors);
        res.Successes.AddRange(result1.Successes);
        res.Successes.AddRange(result2.Successes);
        return res;
    }
    public static Result Merge<T>(this Result<T> result1, Result<T> result2)
    {
        var res = new Result();
        res.Errors.AddRange(result1.Errors);
        res.Errors.AddRange(result2.Errors);
        res.Successes.AddRange(result1.Successes);
        res.Successes.AddRange(result2.Successes);
        return res;
    }
    public static Result OnSuccess(this Result result, Action action)
    {
        if (result.IsFailed) return result;
        action();
        return Result.Ok();
    }
    public static Result OnSuccess(this Result result, Func<Result> func)
    {
        if (result.IsFailed) return result;
        return func();
    }

    public static Result<OutT> OnSuccess<InT, OutT>(this Result<InT> result, Func<InT, Result<OutT>> func)
    {
        if (result.IsSuccess)
            return func(result.Value);
        return result.ToResult<OutT>();
    }
    public static Result OnSuccess<InT>(this Result<InT> result, Func<InT, Result> func)
    {
        if (result.IsSuccess)
            return func(result.Value);
        return result.ToResult();
    }

    public static Result OnSuccess<InT>(this Result<InT> result, Action<InT> func)
    {
        if (result.IsSuccess)
        {
            func(result.Value);
            return Result.Ok();
        }
        return result.ToResult();
    }
    public static Result<OutT> OnSuccess<OutT>(this Result result, Func<Result<OutT>> func)
    {
        if (result.IsSuccess)
            return func();
        return result;
    }


    public static Result OnSuccessAsync(this Result result, Func<Task<Result>> func)
    {
        if (result.IsSuccess)
            return func().GetAwaiter().GetResult();
        return result;
    }

    public static Result<T> OnSuccessAsync<T>(this Result result, Func<Task<Result<T>>> func)
    {
        if (result.IsSuccess)
            return func().GetAwaiter().GetResult();
        return result;
    }

    public static Result OnSuccessAsync<T>(this Result<T> result, Func<T, Task<Result>> func)
    {
        if (result.IsSuccess)
            return func(result.Value).GetAwaiter().GetResult();
        return result.ToResult();
    }

    public static Result OnSuccessAsync<T>(this Result<T> result, Func<T, Task> func)
    {
        if (result.IsSuccess)
        {
            func(result.Value).GetAwaiter().GetResult();
            return Result.Ok();
        }
        return result.ToResult();
    }

    public static Result<OutT> OnSuccessAsync<InT, OutT>(this Result<InT> result, Func<InT, Task<Result<OutT>>> func)
    {
        if (result.IsSuccess)
            return func(result.Value).GetAwaiter().GetResult();
        return result.ToResult<OutT>();
    }

    public static OutT Handle<OutT>(this Result result, Func<OutT> OnSuccessFunc, Func<List<IError>, OutT> OnFailureFunc)
    {
        if (result.IsSuccess)
        {
            return OnSuccessFunc();
        }
        else
        {
            return OnFailureFunc(result.Errors);
        }
    }
    public static void Handle<InT>(this Result<InT> result, Action<InT> OnSuccessFunc, Action<List<IError>> OnFailureFunc)
    {
        if (result.IsSuccess)
        {
            OnSuccessFunc(result.Value);
        }
        else
        {
            OnFailureFunc(result.Errors);
        }
    }

    public static OutT Handle<InT, OutT>(this Result<InT> result, Func<InT, OutT> OnSuccessFunc, Func<List<IError>, OutT> OnFailureFunc)
    {
        if (result.IsSuccess)
        {
            return OnSuccessFunc(result.Value);
        }
        else
        {
            return OnFailureFunc(result.Errors);
        }
    }

}
public static class ResultResolver
{
    public static IActionResult ResolveToIActionResult(this Result result, Func<IActionResult> success, string traceId) =>
        result switch
        {
            { IsSuccess: true } => success(),
            { IsFailed: true } => result.Errors.First().HandleToIActionResult(traceId),
            _ => throw new InvalidOperationException()
        };

    public static IActionResult ResolveToIActionResult<T>(this Result<T> result,
        Func<T, IActionResult> success, string traceId) =>
        result switch
        {
            { IsSuccess: true } => success(result.Value),
            { IsFailed: true } => result.Errors.First().HandleToIActionResult(traceId),
            _ => throw new InvalidOperationException()
        };

    public static IResult ResolveToIResult(this Result result, Func<IResult> success, string traceId) =>
       result switch
       {
           { IsSuccess: true } => success(),
           { IsFailed: true } => result.Errors.First().HandleToIResult(traceId),
           _ => throw new InvalidOperationException()
       };

    public static IResult ResolveToIResult<T>(this Result<T> result,
        Func<T, IResult> success, string traceId) =>
        result switch
        {
            { IsSuccess: true } => success(result.Value),
            { IsFailed: true } => result.Errors.First().HandleToIResult(traceId),
            _ => throw new InvalidOperationException()
        };
}
public static class ErrorHandler
{
    public static IActionResult HandleToIActionResult(this IError error, string traceId)
    {
        if (error is ResultError resultError)
            return resultError.ToIActionResult(traceId);
        else
            return new JsonResult(new ErrorResponse(ErrorType.InternalServerError, error.Message, traceId))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
    }
    public static IResult HandleToIResult(this IError error, string traceId)
    {
        if (error is ResultError resultError)
            return resultError.ToIResult(traceId);
        else
            return Results.Json(new ErrorResponse(ErrorType.InternalServerError, error.Message, traceId),
            statusCode: StatusCodes.Status500InternalServerError);
    }
}
