namespace BuildingBlocks.Contract.Infrastructure.JwtManager;

public class TokenResult
{
    public string Value { get; private set; }
    public int ExpiresIn { get; private set; }

    protected TokenResult(string value, int expiresIn)
    {
        Value = value;
        ExpiresIn = expiresIn;
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
    internal TokenResult(TId id, string value, int expiresIn) : base(value, expiresIn)
    {
        Id = id;
    }

    public TId Id { get; private set; }
}