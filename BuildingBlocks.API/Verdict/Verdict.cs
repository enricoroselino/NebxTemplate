using Shared.Models.Exceptions;

namespace BuildingBlocks.API.Verdict;

public class Verdict<T> : IVerdict
{
    protected Verdict()
    {
    }

    internal Verdict(T value) => Value = value;

    protected internal Verdict(T value, string successMessage) : this(value)
    {
    }

    protected Verdict(VerdictStatus status) => Status = status;
    public static implicit operator T(Verdict<T> verdict) => verdict.Value;
    public static implicit operator Verdict<T>(T value) => new(value);

    public static implicit operator Verdict<T>(Verdict verdict) => new(default(T)!)
    {
        Status = verdict.Status,
        ValidationErrors = verdict.ValidationErrors,
        ErrorMessage = verdict.ErrorMessage,
    };

    public T Value { get; private init; } = default!;
    public VerdictStatus Status { get; private init; } = VerdictStatus.Ok;
    public bool IsSuccess => Status is VerdictStatus.Ok or VerdictStatus.NoContent or VerdictStatus.Created;
    public string ErrorMessage { get; protected init; } = string.Empty;
    public Dictionary<string, string>? ValidationErrors { get; protected init; }
    public string Location { get; private init; } = string.Empty;

    public object? GetValue() => Value;
    public static Verdict<T> Success(T value) => new(value);
    public static Verdict<T> Created(T value) => new(VerdictStatus.Created) { Value = value };

    public static Verdict<T> Created(T value, string location) =>
        new(VerdictStatus.Created) { Value = value, Location = location };

    public static Verdict<T> NoContent() => new(VerdictStatus.NoContent);

    public static Verdict<T> NotFound(string errorMessage) =>
        new(VerdictStatus.NotFound) { ErrorMessage = errorMessage };

    public static Verdict<T> Forbidden(string? errorMessage = null) => new(VerdictStatus.Forbidden)
        { ErrorMessage = errorMessage ?? new ForbiddenException().Message };

    public static Verdict<T> Unauthorized(string? errorMessage = null) => new(VerdictStatus.Unauthorized)
        { ErrorMessage = errorMessage ?? new UnauthorizedException().Message };

    public static Verdict<T> BadRequest(string errorMessage) =>
        new(VerdictStatus.BadRequest) { ErrorMessage = errorMessage };

    public static Verdict<T> BadRequest(Dictionary<string, string> errors) =>
        new(VerdictStatus.BadRequest)
            { ValidationErrors = errors, ErrorMessage = "One or more validation failures have occurred." };

    public static Verdict<T> UnprocessableEntity(string errorMessage) =>
        new(VerdictStatus.UnprocessableEntity) { ErrorMessage = errorMessage };

    public static Verdict<T> Conflict(string errorMessage) =>
        new(VerdictStatus.Conflict) { ErrorMessage = errorMessage };

    public static Verdict<T> InternalError(string errorMessage) =>
        new(VerdictStatus.InternalError) { ErrorMessage = errorMessage };
}

public class Verdict : Verdict<Verdict>
{
    private Verdict()
    {
    }


    private Verdict(VerdictStatus status) : base(status)
    {
    }


    public static Verdict Success() => new();
    public static Verdict<T> Success<T>(T value) => new(value);
    public static Verdict<T> Created<T>(T value) => new(value);
    public new static Verdict NoContent() => new(VerdictStatus.NoContent);

    public new static Verdict NotFound(string errorMessage) =>
        new(VerdictStatus.NotFound) { ErrorMessage = errorMessage };

    public new static Verdict Forbidden(string? errorMessage = null) => new(VerdictStatus.Forbidden)
        { ErrorMessage = errorMessage ?? new ForbiddenException().Message };

    public new static Verdict Unauthorized(string? errorMessage = null) => new(VerdictStatus.Unauthorized)
        { ErrorMessage = errorMessage ?? new UnauthorizedException().Message };

    public new static Verdict BadRequest(string errorMessage) =>
        new(VerdictStatus.BadRequest) { ErrorMessage = errorMessage };

    public new static Verdict BadRequest(Dictionary<string, string> errors) =>
        new(VerdictStatus.BadRequest)
            { ValidationErrors = errors, ErrorMessage = "One or more validation failures have occurred." };

    public new static Verdict UnprocessableEntity(string errorMessage) =>
        new(VerdictStatus.UnprocessableEntity) { ErrorMessage = errorMessage };

    public new static Verdict Conflict(string errorMessage) =>
        new(VerdictStatus.Conflict) { ErrorMessage = errorMessage };

    public new static Verdict InternalError(string errorMessage) =>
        new(VerdictStatus.InternalError) { ErrorMessage = errorMessage };
}