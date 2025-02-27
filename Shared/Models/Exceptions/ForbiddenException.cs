namespace Shared.Models.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException() : base("Access denied, you do not have permission to perform this action.")
    {
    }
}