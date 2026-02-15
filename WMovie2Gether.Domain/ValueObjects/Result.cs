namespace WMovie2Gether.Domain.ValueObjects;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Message { get; }
    public IEnumerable<string> Errors { get; }

    protected Result(bool isSuccess, string message, IEnumerable<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Message = message;
        Errors = errors ?? Enumerable.Empty<string>();
    }

    public static Result Success(string message = "Operation completed successfully.")
    {
        return new Result(true, message);
    }

    public static Result Failure(string message, IEnumerable<string>? errors = null)
    {
        return new Result(false, message, errors);
    }

    public static Result Failure(string message, string error)
    {
        return new Result(false, message, new[] { error });
    }
}

public class Result<TData> : Result
{
    public TData? Data { get; }

    private Result(bool isSuccess, string message, TData? data = default, IEnumerable<string>? errors = null)
        : base(isSuccess, message, errors)
    {
        Data = data;
    }

    public static Result<TData> Success(TData data, string message = "Operation completed successfully.")
    {
        return new Result<TData>(true, message, data);
    }

    public static new Result<TData> Failure(string message, IEnumerable<string>? errors = null)
    {
        return new Result<TData>(false, message, default, errors);
    }

    public static new Result<TData> Failure(string message, string error)
    {
        return new Result<TData>(false, message, default, new[] { error });
    }
}
