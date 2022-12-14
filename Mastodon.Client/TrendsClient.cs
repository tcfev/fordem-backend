namespace Mastodon.Client;

public sealed class TrendsClient
{
    private readonly MastodonClient _client;

    internal TrendsClient(MastodonClient client)
    {
        _client = client;
    }

    public Task<List<Tag>?> GetTagsAsync()
    {
        return _client.HttpClient.GetFromJsonAsync<List<Tag>>("api/v1/trends/tags", MastodonClient._options);
    }

    public Task<List<Status>?> GetStatusesAsync()
    {
        return _client.HttpClient.GetFromJsonAsync<List<Status>>("api/v1/trends/statuses", MastodonClient._options);
    }
}
