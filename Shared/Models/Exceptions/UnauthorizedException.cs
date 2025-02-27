namespace Shared.Models.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException() : base("Unauthorized, you must be authenticated to access this resource.")
    {
    }
}