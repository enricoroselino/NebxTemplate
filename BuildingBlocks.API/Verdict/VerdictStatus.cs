namespace BuildingBlocks.API.Verdict;

public enum VerdictStatus
{
    Ok,
    NoContent,
    Created,
    NotFound,
    Forbidden,
    Unauthorized,
    BadRequest,
    UnprocessableEntity,
    Conflict,
    InternalError,
}