using System.Text.Json.Serialization;

namespace Shared.Models.Responses;

public abstract class Response
{
    protected Response()
    {
    }

    public static Response<TData> Build<TData>(TData data, Meta? meta = null) => new(data, meta);
}

[Serializable]
public class Response<TData> : Response
{
    internal Response(TData data, Meta? meta = null)
    {
        Data = data;
        Meta = meta;
    }

    [JsonPropertyOrder(1)] public TData Data { get; init; }
    [JsonPropertyOrder(2)] public Meta? Meta { get; init; }
}