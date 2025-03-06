namespace Shared.Verdict;

public interface IVerdict
{
    public VerdictStatus Status { get; }
    public string ErrorMessage { get; }
    public Dictionary<string, string>? ValidationErrors { get; }
    public object? GetValue();
    public string Location { get; }
}