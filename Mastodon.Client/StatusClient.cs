namespace Mastodon.Client;

public sealed class StatusClient
{
    private readonly MastodonClient _client;

    internal StatusClient(MastodonClient client)
    {
        _client = client;
    }

    public async Task<Status?> CreateAsync(CreateStatus status)
    {
        var response = await _client.HttpClient.PostAsJsonAsync($"api/v1/statuses", status, MastodonClient._options);
        var content = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<Status>(content, MastodonClient._options);

        return result;
    }

    public Task<Status?> GetByIdAsync(string id)
    {
        return _client.HttpClient.GetFromJsonAsync<Status>($"api/v1/statuses/{id}", MastodonClient._options);
    }

    public async Task<Status?> DeleteByIdAsync(string id)
    {
        var response = await _client.HttpClient.DeleteAsync($"api/v1/statuses/{id}");
        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<Status>(content, MastodonClient._options);
    }

    /// <summary>
    /// View who reblogged a given status.
    /// </summary>
    /// <param name="maxId">Internal parameter. Use HTTP Link header for pagination.</param>
    /// <param name="sinceId">Internal parameter. Use HTTP Link header for pagination.</param>
    /// <param name="minId">Internal parameter. Use HTTP Link header for pagination.</param>
    /// <param name="limit">Maximum number of results to return. Defaults to 40 accounts. Max 80 accounts.</param>
    /// <returns>View who reblogged a given status.</returns>
    public Task<List<Account>?> GetRebloggedByAsync(string id, string? maxId = null, string? sinceId = null, string? minId = null, uint? limit = null)
    {
        var q = new QueryBuilder();

        q.Add("max_id", maxId);
        q.Add("since_id", sinceId);
        q.Add("min_id", minId);
        q.Add("limit", limit);

        var url = q.GetUrl($"api/v1/statuses/{id}/reblogged_by");

        return _client.HttpClient.GetFromJsonAsync<List<Account>>(url, MastodonClient._options);
    }

    /// <summary>
    /// View who favourited a given status.
    /// </summary>
    /// <param name="maxId">Internal parameter. Use HTTP Link header for pagination.</param>
    /// <param name="sinceId">Internal parameter. Use HTTP Link header for pagination.</param>
    /// <param name="minId">Internal parameter. Use HTTP Link header for pagination.</param>
    /// <param name="limit">Maximum number of results to return. Defaults to 40 accounts. Max 80 accounts.</param>
    /// <returns>View who favourited a given status.</returns>
    public Task<Response<List<Account>>> GetFavouritedByAsync(string id, string? maxId = null, string? sinceId = null, string? minId = null, uint? limit = null)
    {
        var q = new QueryBuilder();

        q.Add("max_id", maxId);
        q.Add("since_id", sinceId);
        q.Add("min_id", minId);
        q.Add("limit", limit);

        var url = q.GetUrl($"api/v1/statuses/{id}/favourited_by");

        return _client.HttpClient.GetFromJsonWithHeadersAsync<List<Account>>(url, MastodonClient._options);
    }

    public Task<Context?> GetContextAsync(string id)
    {
        return _client.HttpClient.GetFromJsonAsync<Context>($"api/v1/statuses/{id}/context", MastodonClient._options);
    }

    public async Task<Status?> FavoriteAsync(string id)
    {
        var response = await _client.HttpClient.PostAsync($"api/v1/statuses/{id}/favourite", new StringContent(string.Empty));
        var result = await response.Content.ReadFromJsonAsync<Status>(MastodonClient._options);

        return result;
    }

    public async Task<Status?> UnfavoriteAsync(string id)
    {
        var response = await _client.HttpClient.PostAsync($"api/v1/statuses/{id}/unfavourite", new StringContent(string.Empty));
        var result = await response.Content.ReadFromJsonAsync<Status>(MastodonClient._options);

        return result;
    }

    public async Task<Status?> BookmarkAsync(string id)
    {
        var response = await _client.HttpClient.PostAsync($"api/v1/statuses/{id}/bookmark", new StringContent(string.Empty));
        var result = await response.Content.ReadFromJsonAsync<Status>(MastodonClient._options);

        return result;
    }

    public async Task<Status?> UnbookmarkAsync(string id)
    {
        var response = await _client.HttpClient.PostAsync($"api/v1/statuses/{id}/unbookmark", new StringContent(string.Empty));
        var result = await response.Content.ReadFromJsonAsync<Status>(MastodonClient._options);

        return result;
    }

    public async Task<Status?> MuteAsync(string id)
    {
        var response = await _client.HttpClient.PostAsync($"api/v1/statuses/{id}/mute", new StringContent(string.Empty));
        var result = await response.Content.ReadFromJsonAsync<Status>(MastodonClient._options);

        return result;
    }

    public async Task<Status?> UnmuteAsync(string id)
    {
        var response = await _client.HttpClient.PostAsync($"api/v1/statuses/{id}/unmute", new StringContent(string.Empty));
        var result = await response.Content.ReadFromJsonAsync<Status>(MastodonClient._options);

        return result;
    }

    public Task<Response<Status>> PinAsync(string id)
    {
        return _client.HttpClient.PostFromAsync<Status>($"api/v1/statuses/{id}/pin", MastodonClient._options);
    }

    public async Task<Status?> UnpinAsync(string id)
    {
        var response = await _client.HttpClient.PostAsync($"api/v1/statuses/{id}/unpin", new StringContent(string.Empty));
        var result = await response.Content.ReadFromJsonAsync<Status>(MastodonClient._options);

        return result;
    }

    public Task<Response<Status>> ReblogAsync(string id, string? visibility = null)
    {
        var form = new List<KeyValuePair<string, string>>();

        if (!string.IsNullOrWhiteSpace(visibility))
        {
            form.Add(new KeyValuePair<string, string>("visibility", visibility));
        }

        return _client.HttpClient.PostFromAsync<Status>($"api/v1/statuses/{id}/reblog", MastodonClient._options, form);

    }

    public Task<Response<Status>> UnreblogAsync(string id)
    {
        return _client.HttpClient.PostFromAsync<Status>($"api/v1/statuses/{id}/unreblog", options: MastodonClient._options);
    }

    /// <summary>
    /// Obtain the source properties for a status so that it can be edited.
    /// </summary>
    /// <param name="id">The local ID of the Status in the database.</param>
    /// <returns></returns>
    public Task<StatusSource?> GetSourceAsync(string id)
    {
        return _client.HttpClient.GetFromJsonAsync<StatusSource>($"api/v1/statuses/{id}/source", MastodonClient._options);
    }

    /// <summary>
    /// Get all known versions of a status, including the initial and current states.
    /// </summary>
    /// <param name="id">The local ID of the Status in the database.</param>
    public Task<List<StatusEdit>?> GetHistoryAsync(string id)
    {
        return _client.HttpClient.GetFromJsonAsync<List<StatusEdit>>($"api/v1/statuses/{id}/history", MastodonClient._options);
    }
}
