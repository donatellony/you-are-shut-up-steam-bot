namespace YouAreShutUp.SteamBot.Steam.Cache.Models;

public sealed record TypedCacheKey
{
    public TypedCacheKey(string type, string key)
    {
        Type = type;
        Key = key;
    }

    private string Type { get; }
    private string Key { get; }

    public string GetKey()
    {
        return $"{Type}:{Key}";
    }

    public override string ToString()
    {
        return GetKey();
    }
}