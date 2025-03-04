namespace BuildingBlocks.API.Services.JwtManager;

public class TokenResult
{
    public string Value { get; private set; }
    public int ExpiresOn { get; private set; }

    protected TokenResult(string value, int expiresOn)
    {
        Value = value;
        ExpiresOn = expiresOn;
    }

    public static TokenResult Create(string value, int expiresIn)
    {
        return new TokenResult(value, expiresIn);
    }

    public static TokenResult<TId> Create<TId>(TId id, string value, int expiresIn)
    {
        return new TokenResult<TId>(id, value, expiresIn);
    }
}

public class TokenResult<TId> : TokenResult
{
    internal TokenResult(TId id, string value, int expiresOn) : base(value, expiresOn)
    {
        Id = id;
    }

    public TId Id { get; private set; }
}